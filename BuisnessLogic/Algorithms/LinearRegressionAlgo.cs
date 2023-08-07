using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using System.Xml.Linq;
using MathNet.Numerics.LinearRegression;
using BuisnessLogic.Model;

namespace BuisnessLogic.Algorithms
{
    internal class LinearRegressionAlgo : IPredictiveAlgorithm
    {
        public List<double> independentVariable { get; set; }
        public List<double> dependentVariable { get; set; }
        public double Slope { get; private set; }
        public double Intercept { get; private set; }
        
        public async Task<List<float>> Predict(List<DataPoint> _data) // todo 
        { 
            (double a, double b) res = Fit.Line(independentVariable.ToArray(), dependentVariable.ToArray());
            Intercept = res.a;
            Slope = res.b;
            return new List<float>();
        }
    }
}
