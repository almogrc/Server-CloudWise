using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Model;

namespace BuisnessLogic.DTO
{
    public class LinearRegressionData
    {
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public LinkedList<DataPoint> Data { get; set; }
    }
}
