using Apache.Arrow;
using BuisnessLogic.Algorithms;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Model;
using Server_cloudata.Services.Collector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata.Services.Predict
{
    public class PredictService : IPredicteService
    {
        public PredictService(INodeCollectorService<Metric> collector, IProcessExporterService<List<Metric>> collectorService)
        {
            _nodeCollector = collector;
            _processCollector = collectorService;
        }
        private readonly DateTime _from = DateTime.UtcNow.AddDays(-7);
        private readonly DateTime _to = DateTime.UtcNow;
        private INodeCollectorService<Metric> _nodeCollector;
        private IProcessExporterService<List<Metric>> _processCollector;

        public async Task<Metric> GetPredictResult(eNodeExporterData eNodeExporter, string address, IPredictiveAlgorithm predictAlgo)
        {
            Metric metric = await _nodeCollector.GetData(eNodeExporter.ToString(), _from, _to, address);
            List<float> predictions = await predictAlgo.Predict(metric.DataPoints);
            List<DataPoint> predictDataPoint = await CalculateDates(predictions, metric.DataPoints);
            metric.DataPoints = predictDataPoint;
            return metric;
        }
        public async Task<List<Metric>> GetPredictResult(eProcessExporterData eProcessExporterData, string address, IPredictiveAlgorithm predictAlgo)
        {
            List<Metric> metrics = await _processCollector.GetData(eProcessExporterData.ToString(), _from, _to, address);
            foreach (Metric metric in metrics)
            {
                List<float> predictions = await predictAlgo.Predict(metric.DataPoints);
                List<DataPoint> predictDataPoint = await CalculateDates(predictions, metric.DataPoints);
                metric.DataPoints = predictDataPoint;
            }
            return metrics;
        }
        private async Task<List<DataPoint>> CalculateDates(List<float> predictions, List<DataPoint> dataPoints)
        {
            List<DataPoint> res = new List<DataPoint>();
            TimeSpan duration = (dataPoints[1].Date) - (dataPoints.First().Date);
            DateTime currDateTime = dataPoints.Last().Date;
            for (int i = 0; i < predictions.Count; i++)
            {
                currDateTime = currDateTime + duration;
                res.Add(new DataPoint() { Date = currDateTime, Value = predictions[i] });
            }
            return res;
        }
    }
}
