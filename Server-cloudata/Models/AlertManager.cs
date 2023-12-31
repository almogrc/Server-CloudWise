﻿using Server_cloudata.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Server_cloudata.Services.Collector;
using BuisnessLogic.Model;
using BuisnessLogic.Collector;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Enums;
using System.Linq;
using BuisnessLogic.Collector.Enums.Atributes;
using BuisnessLogic.Extentions;
using SendGrid;
using SendGrid.Helpers.Mail;
using Server_cloudata.Enums;
using Org.BouncyCastle.Asn1.Ocsp;
using Server_cloudata.ServerDataManager;
using System.Runtime.Intrinsics.Arm;
using Server_cloudata.Models;
using MongoDB.Driver;
using Server_cloudata.DTO;
using Org.BouncyCastle.Asn1.Cms;

//namespace Server_cloudata.Models
//{
//    /// <summary>
//    /// Alert manager per customer and threshhold refresher per machine
//    /// </summary>
public class AlertManager
{
    private Dictionary<string, ThresholdRefresher> _machineIdToThresholdRefresher;
    private string _customerEmail;
    private CustomersService _customersService;

    public AlertManager(CustomersService customersService, string customerEmail)
    {
        _customerEmail = customerEmail;
        _customersService = customersService;
        _machineIdToThresholdRefresher = new Dictionary<string, ThresholdRefresher>();
    }

    public async Task UpdateOrCreateThresholdRefresher(string machineId, INodeCollectorService<Metric> collector, ThresholdsCollector thresholdsCollector)
    {
        Customer customer = await _customersService.GetAsyncByEmail(_customerEmail);

        if (!_machineIdToThresholdRefresher.ContainsKey(machineId))
        {
            _machineIdToThresholdRefresher.Add(machineId, new ThresholdRefresher(_customersService, customer.VMs.Find(vm => vm.Name == machineId), _customerEmail, collector, thresholdsCollector));
        }
    }

    public class ThresholdRefresher
    {
        private CustomersService _customersService;
        private INodeCollectorService<Metric> _nodeCollector;
        private ICollector<eNodeExporterData> _collector;
        private ThresholdsCollector _thresholdsCollector;
        private VirtualMachine _virtualMachine;
        private Timer _timer;
        private string _customerEmail;

        public ThresholdRefresher(CustomersService customersService, VirtualMachine virtualMachine, string customerEmail, INodeCollectorService<Metric> nodeCollector, ThresholdsCollector thresholdsCollector)
        {
            _customersService = customersService;
            _virtualMachine = virtualMachine;
            _nodeCollector = nodeCollector;
            _collector = new NodeExporterCollector();
            _thresholdsCollector = thresholdsCollector;
            _timer = new Timer(Start, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
            _customerEmail = customerEmail;
        }

        public async void Start(object state)
        {
            try
            {
                _virtualMachine = _customersService._customersCollection.Find(customer => customer.Email == _customerEmail).Single().VMs.Find(vm => vm.Name == _virtualMachine.Name);
                List<eNodeExporterData> allEnumValues = new List<eNodeExporterData>((eNodeExporterData[])Enum.GetValues(typeof(eNodeExporterData)));
                List<eNodeExporterData> nodeExporterValues = allEnumValues.Where(e => e.HasAttribute<AlertAttribute>()).ToList();

                if (!await CheckMachine())
                {
                   await HandleAlertsWhenMachineIsDown();
                }

                foreach (var thresholdKey in _virtualMachine.ThresholdsNode.Keys)
                {
                    if (!nodeExporterValues.Contains(thresholdKey))
                    {
                        continue;
                    }

                    DateTime from = DateTime.UtcNow.AddMinutes(-3);
                    DateTime to = DateTime.UtcNow;
                    MetricWithThreshold metric = await _thresholdsCollector.AddThresholsToMetric((await _nodeCollector.GetData(thresholdKey.ToString(), from, to, _virtualMachine.Address)), _virtualMachine);
                    //metric = await UpdateMetricValuesToPrecents(metric, thresholdKey);

                    foreach (var dataPoint in metric.DataPoints)
                    {
                        if (dataPoint.Value > metric.Critical || dataPoint.Value > metric.Critical)
                        {
                            Alert alert = new Alert
                            {
                                VMName = _virtualMachine.Name,
                                //ThresholdKey = thresholdKey,
                                CurrentValue = dataPoint.Value,
                                StartTime = dataPoint.Date,
                                EmailRecipient = _customerEmail
                            };

                            if (dataPoint.Value > metric.Critical)
                            {
                                alert.ThresholdValue = metric.Critical;
                                // alert.AlertType = eAlertType.Danger;
                            }
                            else // warning
                            {
                                alert.ThresholdValue = metric.Warning;
                                // alert.AlertType = eAlertType.Warning;
                            }

                            if (_virtualMachine.Alerts == null)
                            {
                                _virtualMachine.Alerts = new List<Alert>();
                            }

                            Customer customer = await _customersService.GetAsyncByEmail(_customerEmail);

                            if (customer != null)
                            {
                                if (NeededToUpdateClient(alert, _virtualMachine.Alerts))
                                {
                                    await SendAlertEmail(alert, customer.Name);
                                    _virtualMachine.Alerts.Add(alert);
                                    //update database
                                    var existingVM = customer.VMs.FirstOrDefault(vm => vm.Name == _virtualMachine.Name);
                                    if (existingVM != null)
                                    {
                                        existingVM.Alerts = _virtualMachine.Alerts;
                                        await _customersService.UpdateVMAsync(customer);
                                    }
                                }
                            }
                        }
                    }
                }

                _timer.Change(TimeSpan.FromMinutes(3), Timeout.InfiniteTimeSpan);
            }
            catch (Exception e)
            {
                StopTimer();
            }
        }

        private async Task<bool> CheckMachine()
        {
            return await ServerUtils.CheckVMStatus(_virtualMachine, await _collector.Collect("Status", ""));
        }
        private async Task<Metric> UpdateMetricValuesToPrecents(Metric metric, eNodeExporterData thresholdKey)
        {
            if (thresholdKey == eNodeExporterData.RamUsage)
            {
                float ramValue = (await _nodeCollector.GetData(eNodeExporterData.Ram.ToString(), DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow, _virtualMachine.Name)).DataPoints.First().Value;
                foreach (var dataPoint in metric.DataPoints)
                {
                    dataPoint.Value = (dataPoint.Value * 100) / ramValue;
                }
            }
            return metric;
        }

        private bool NeededToUpdateClient(Alert newAlert, List<Alert> alerts)
        {
            if (alerts.Exists(oldAlert => oldAlert.StartTime.Date == newAlert.StartTime.Date &&
            oldAlert.AlertName == newAlert.AlertName/*&& oldAlert.AlertType == newAlert.AlertType*/))
            {
                return false;
            }
            return true;
        }

        public void StopTimer()
        {
            _timer.Dispose();
        }

        private async Task SendAlertEmail(Alert alert, string customerName)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("cloudwise04@gmail.com", "CloudWise");
            var to = new EmailAddress(alert.EmailRecipient, customerName);
            var subject = $"Alert: Threshold exceeded for VM '{alert.VMName}'";
            var plainTextContent = $"Threshold '{alert.AlertName}' with value '{alert.ThresholdValue}' was exceeded for VM '{alert.VMName}'. Current value is '{alert.CurrentValue}'.";
            var htmlContent = $"<p>Threshold '{alert.AlertName}' with value '{alert.ThresholdValue}' was exceeded for VM '{alert.VMName}'. Current value is '{alert.CurrentValue}'.</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        private async Task HandleAlertsWhenMachineIsDown()
        {
            Alert alert = new Alert
            {
                VMName = _virtualMachine.Name,
                AlertName = "Machine is down",
                //ThresholdKey = thresholdKey,
                //CurrentValue = dataPoint.Value,
                StartTime = DateTime.Now,
                EmailRecipient = _customerEmail
            };

            if (_virtualMachine.Alerts == null)
            {
                _virtualMachine.Alerts = new List<Alert>();
            }

            Customer customer = await _customersService.GetAsyncByEmail(_customerEmail);

            if (customer != null)
            {
                if (NeededToUpdateClient(alert, _virtualMachine.Alerts))
                {
                    await SendAlertByEmailWhenMachineIsDown(alert, customer.Name);
                    _virtualMachine.Alerts.Add(alert);
                    var existingVM = customer.VMs.FirstOrDefault(vm => vm.Name == _virtualMachine.Name);
                    if (existingVM != null)
                    {
                        existingVM.Alerts = _virtualMachine.Alerts;
                        await _customersService.UpdateVMAsync(customer);
                    }
                }
            }
        }

        private async Task SendAlertByEmailWhenMachineIsDown(Alert alert, string customerName)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("cloudwise04@gmail.com", "CloudWise");
            var to = new EmailAddress(alert.EmailRecipient, customerName);
            var subject = alert.VMName;
            var plainTextContent = $"{alert.AlertName}' Time '{alert.StartTime}'.";
            var htmlContent = "";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }

}

