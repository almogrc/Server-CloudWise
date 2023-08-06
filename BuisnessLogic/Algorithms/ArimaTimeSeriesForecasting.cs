using BuisnessLogic.Model;
using System.Collections.Generic;
using System.Linq;
using Python.Runtime;
using System;

namespace BuisnessLogic.Algorithms
{
    internal class ArimaTimeSeriesForecasting : IPredictiveAlgorithm
    {
        private List<DataPoint> _data;
        public float[] Result { get; private set; }

        public ArimaTimeSeriesForecasting(List<DataPoint> data)
        {
            _data = data;
        }

        public void Predict()
        {
            using (Py.GIL())
            {
                dynamic statsmodels = Py.Import("statsmodels.api");

                dynamic values = new PyList((PyObject)_data.Select(dp => (double)dp.Value));

                dynamic timeSeries = statsmodels.tsa.tsatools.timeseries_dates_from_str(_data.Select(dp => dp.Date.ToString()));

                // Create ARIMA model with order (p, d, q)
                dynamic order = new { p = 1, d = 1, q = 1 }; // Example order values, adjust as needed
                dynamic model = statsmodels.tsa.ARIMA(values, order: order);
                dynamic results = model.fit(disp: 0);

                dynamic forecast = results.forecast(steps: 10);

                Result = forecast.ToArray<float>();
            }
        }
    }
}
