using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using BuisnessLogic.Model;
using BuisnessLogic.Algorithms.DTOPrediction;
using Newtonsoft.Json.Linq;

namespace BuisnessLogic.Algorithms
{
    public class SSATimeSeriesForecating : IPredictiveAlgorithm
    {
        
        public async Task<List<float>> Predict(List<DataPoint> data)
        {
            float[] result;
            MLContext context = new MLContext();
            IDataView dataView = context.Data.LoadFromEnumerable(data);
            
            var pipline = context.Forecasting.ForecastBySsa(
                "Forecast",
                "Value",
                windowSize: (int)(data.Count * 0.07),
                seriesLength: (int)(data.Count * 0.8), 
                trainSize: (int)(data.Count * 0.6), 
                horizon: (int)(data.Count * 0.2));
            var model = pipline.Fit(dataView);
            using (var forcastingEngine = model.CreateTimeSeriesEngine<DataPoint, DataPointForcast>(context))
            {
                var forcasts = forcastingEngine.Predict();
                result = forcasts.Forecast;
                return result.ToList();
            }
        }
    }
}
