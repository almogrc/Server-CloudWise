using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Builder
{
    interface IBuilder<T,E>
    {
        T GetResult(E eData);
        T GetResult();
        void Build();
        void Build(E eData);
    }
}
