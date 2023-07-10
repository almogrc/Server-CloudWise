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
                return _machine;
            }
        }
        public string IP { get; }
        internal MachineDataManager MachineDataManager { get; private set; }
        internal CollectManager CollectManager { get; private set; }
        public LinkedList<DataPoint> GetData(string exporterType, string query, DateTime start, params string[] values)
        {
            eExporterType exporter;
            if(!Enum.TryParse(exporterType, true, out exporter)) { throw new Exception("can't parse exporter"); }
            LinkedList<DataPoint> result;
            if(exporter == eExporterType.node)
            {
                eNodeExporterData eNodeExporter;
                if (!Enum.TryParse(query, true, out eNodeExporter)) { throw new Exception($"can't parse {query} to enum {"eNodeExporterData"}"); }
                CollectManager.nodeExporter.Collect(eNodeExporter, start, values);
                MachineDataManager.NodeData.Data[eNodeExporter] = CollectManager.nodeExporter
                    .Builder.GetResult(eNodeExporter).Data[eNodeExporter];
                result = MachineDataManager.NodeData.Data[eNodeExporter];
            }
            else // processExporter
            {
                result = new LinkedList<DataPoint>();
                eProcessExporterData eProcessExporter;
                if (!Enum.TryParse(query, true, out eProcessExporter)) { throw new Exception($"can't parse {query} to enum {"eProcessExporter"}"); }
                CollectManager.processExporter.Collect(eProcessExporter, start, values);
                if (!MachineDataManager.Groups.GroupNameToGroupData.ContainsKey(values[0]))
                {
                    MachineDataManager.Groups.GroupNameToGroupData.
                        Add(values[0], CollectManager.processExporter.Builder.GetResult(eProcessExporter, values[0]).GroupNameToGroupData[values[0]]);
                }
                else
                {
                    MachineDataManager.Groups.GroupNameToGroupData[values[0]].Data[eProcessExporter] = 
                        CollectManager.processExporter.Builder.GetResult(eProcessExporter, values[0]).GroupNameToGroupData[values[0]]
                        .Data[eProcessExporter];
                }

                result = MachineDataManager.Groups.GroupNameToGroupData[values[0]].Data[eProcessExporter];
            }
            PredictData predictData = PredictForcasting(result);
            predictData._foreCasts.ForEach(dataPoint=>result.AddLast(dataPoint));
            return result;
        }
        public void CollectInformation()
        {
            try
            {
                CollectManager.nodeExporter.Collect();
                CollectManager.processExporter.Collect();
                MachineDataManager.NodeData = CollectManager.nodeExporter.Builder.GetResult();
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
        public PredictData PredictForcasting(LinkedList<DataPoint> dataPointToPredict)
        {
            TimeSeriesForecating algo = new TimeSeriesForecating(dataPointToPredict);
            algo.Predict();
            return new PredictData(algo.Result, dataPointToPredict.Last().Date);
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
