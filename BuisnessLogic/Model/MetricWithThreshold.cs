using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    public class MetricWithThreshold : Metric
    {
        public MetricWithThreshold(Metric metric) : base(metric.DataPoints, metric.From, metric.To, metric.DataType, metric.Type, metric.Name)
        {
        }

        public float Warning { get; set; }
        public float Critical { get; set; }
    }
}
