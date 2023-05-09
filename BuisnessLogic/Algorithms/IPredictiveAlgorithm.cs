using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Algorithms
{
    interface IPredictiveAlgorithm
    {
        List<double> Predict();
    }
}
