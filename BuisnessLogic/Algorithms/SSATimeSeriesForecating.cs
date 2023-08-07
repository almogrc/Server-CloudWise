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
            //var pipline = _context.Forecasting.ForecastBySamira(
            //     "Forecast",
            //     "Value",
            //     windowSize: (int)(_data.Count * 0.07),
            //     seriesLength: (int)(_data.Count * 0.8),
            //     trainSize: (int)(_data.Count * 0.6),
            //     horizon: (int)(_data.Count * 0.2));

           // var arimaPipeline = pipline.ReplaceEstimator("Forecast", _context.Transforms.Forecasting.ForecastBySarima
           //     ("Value", windowSize: 5, seriesLength: (int)(_data.Count * 0.7),
           //     trainSize: (int)(_data.Count * 0.3), 
           //     horizon: (int)(_data.Count * 0.2)));
            //var pipline = _context.Forecasting.ForecastBySsa(
            //    "Forecast",
            //    "Value",
            //    windowSize: 100,
            //    seriesLength: _data.Count,
            //    trainSize: _data.Count,
            //    horizon: (int)(_data.Count*0.2)
            //   );
            var model = pipline.Fit(dataView);
            using (var forcastingEngine = model.CreateTimeSeriesEngine<DataPoint, DataPointForcast>(context))
            {
                var forcasts = forcastingEngine.Predict();
                result = forcasts.Forecast;
                return result.ToList();
                // foreach (var item in forcasts.Value){

                //foreach(var item in forcasts.Forecast) 
                //    Console.WriteLine(item);
             //   }

            }
        }
    }
}
