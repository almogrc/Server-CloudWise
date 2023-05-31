using BuisnessLogic.Model;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.DTO
{
    public class PredictData
    {
        public List<DataPoint> _foreCasts;
        public PredictData(float[] forecasts, int stepsInSeconds, DateTime lastDate) 
        { 
            _foreCasts = new List<DataPoint>();
            updateDateTimeForecasts(forecasts, stepsInSeconds, lastDate);
        }

        private void updateDateTimeForecasts(float[] forecasts, int stepsInSeconds, DateTime lastDate)
        {
            DataPoint currDataPoint;
            foreach(var forecast in forecasts)
            {
                currDataPoint = new DataPoint() { Value = forecast, Date = lastDate.AddSeconds(stepsInSeconds) };
                _foreCasts.Add(currDataPoint);
                lastDate = currDataPoint.Date;
            }
        }
    }
}
