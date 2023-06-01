using BuisnessLogic.Exceptions;
using System;
using BuisnessLogic.Loggers;
using System.Collections.Generic;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Algorithms;
using System.Linq;
using BuisnessLogic.Algorithms.DTOPrediction;
using BuisnessLogic.DTO;

namespace BuisnessLogic.Model
{
    public class Machine
    {
        private Machine()
        {
            MachineDataManager = new MachineDataManager();
            CollectManager = new CollectManager();
            Logger.Instance.Info($"Machine created.");
        }
        private static Machine _machine;
        private static object _machineLock = new object();
        public static Machine MachineInstance
        {
            get {
                if (_machine == null)
                {
                    lock( _machineLock )
                    {
                        if(_machine == null)
                        {
                            _machine = new Machine();
                        }
                    }
                }
                return _machine
            }
        }
        public string IP { get; }
        internal MachineDataManager MachineDataManager { get; private set; }
        internal CollectManager CollectManager { get; private set; }
        public List<DataPoint> GetData(string exporterType, string query, DateTime start)
        {
            eExporterType exporter;
            if(!Enum.TryParse(query, out exporter)) { throw new Exception("can't parse exporter"); }
            if(exporter == eExporterType.node)
            {
                CollectManager.nodeExporter.Collect(query, start);
            }
            else if(exporter == eExporterType.process)
            {
                CollectManager.processExporter.Collect(query, start);
            }
        }
        public void CollectInformation()
        {
            try
            {
                CollectManager.nodeExporter.Collect();
                CollectManager.processExporter.Collect();
                MachineDataManager.MachineData = CollectManager.nodeExporter.Builder.GetResult();
                MachineDataManager.Groups = CollectManager.processExporter.Builder.GetResult();
            }
            catch (UnexpectedTypeException utex)
            {
                Logger.Instance.Error(utex.Message + Environment.NewLine + utex.StackTrace);
            }
            catch (UnsuccessfulResponseException urex)
            {
                Logger.Instance.Error(urex.Message + Environment.NewLine + urex.StackTrace);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public PredictData PredictForcasting()
        {
            LinkedList<DataPoint> data = MachineDataManager.Groups.GroupNameToGroupData["prometheus"].CpuUsage[eCPUMode.user];
            data.Reverse();
            TimeSeriesForecating algo = new TimeSeriesForecating(data);
            algo.Predict();
            return new PredictData(algo.Result, 60, data.Last().Date);
        }
        public LinearRegressionData LinearRegression()
        {
            LinkedList<DataPoint> data = MachineDataManager.Groups.GroupNameToGroupData["prometheus"].CpuUsage[eCPUMode.user];
            List<double> independetVariable = new List<double>();
            List<double> dependentVariable = new List<double>();
            double i = 1;
                data.AsEnumerable().ToList().ForEach(x => {
                    independetVariable.Add(i++);
                    dependentVariable.Add(x.Value);
                });
            LinearRegressionAlgo linearRegression = new LinearRegressionAlgo();
            linearRegression.independentVariable = independetVariable;
            linearRegression.dependentVariable = dependentVariable;
            linearRegression.Predict();
            return new LinearRegressionData() { Slope = linearRegression.Slope, Intercept = linearRegression.Intercept, Data = data };

        }
    }
}
