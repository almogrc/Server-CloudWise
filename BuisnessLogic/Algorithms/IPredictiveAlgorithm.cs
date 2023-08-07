using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Algorithms
{
    public interface IPredictiveAlgorithm
    {
        Task<List<float>> Predict(List<DataPoint> data);
    }
}
