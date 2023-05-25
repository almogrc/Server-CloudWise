using BuisnessLogic.Exceptions;
using System;
using BuisnessLogic.Loggers;
using System.Collections.Generic;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Algorithms;
using System.Linq;
using BuisnessLogic.DTO;

namespace BuisnessLogic.Model
{
    public class Machine
    {
        public Machine()
        {
            MachineDataManager = new MachineDataManager();
            CollectManager = new CollectManager();
            Logger.Instance.Info($"Machine created.");
        }

        public string IP { get; }
        internal MachineDataManager MachineDataManager { get; private set; }
        internal CollectManager CollectManager { get; private set; }

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
        public LinearRegressionData PredictData()
        {
            LinkedList<KeyValuePair<DateTime, double>> data = MachineDataManager.Groups.GroupNameToGroupData["prometheus"].CpuUsage[CPUMode.user];
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
