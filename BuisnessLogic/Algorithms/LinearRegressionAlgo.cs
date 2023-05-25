using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using System.Xml.Linq;
using MathNet.Numerics.LinearRegression;

namespace BuisnessLogic.Algorithms
{
    internal class LinearRegressionAlgo : IPredictiveAlgorithm
    {
        public List<double> independentVariable { get; set; }
        public List<double> dependentVariable { get; set; }
        public double Slope { get; private set; }
        public double Intercept { get; private set; }
      
        public void Predict()
        { 
            (double a, double b) res = Fit.Line(independentVariable.ToArray(), dependentVariable.ToArray());
            Intercept = res.a;
            Slope = res.b;
        }
    }
}
