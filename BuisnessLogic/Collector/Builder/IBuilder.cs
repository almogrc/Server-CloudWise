using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Builder
{
    public interface IBuilder<T>
    {
        Task<T> Build(string dataToConvert);
    }
}
