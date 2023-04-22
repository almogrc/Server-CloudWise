using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
namespace BuisnessLogic.Collector
{
    interface ICollector<T>
    {
        IBuilder<T> Builder { get; }
        void Collect();
        void Collect(string information);
    }
}
