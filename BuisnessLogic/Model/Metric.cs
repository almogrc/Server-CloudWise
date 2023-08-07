using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    public class Metric
    {

        public Metric(List<DataPoint> dataPoints, DateTime from, DateTime to, string dataType, string type, string name)
        {
            DataPoints = dataPoints;
            From = from;
            To = to;
            DataType = dataType;
            Type = type;
            Name = name;
        }

        public string Name { get; }
        public List<DataPoint> DataPoints { get; set; }
        public DateTime From { get; }
        public DateTime To { get; }
        public string DataType { get; }
        public string Type { get; } 

    }
}
