using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Builder
{
    interface IBuilder<T>
    {
        Dictionary<string, T> GetResult();
        void Build();
    }
}
