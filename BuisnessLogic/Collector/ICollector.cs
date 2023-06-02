using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Prometheus;

namespace BuisnessLogic.Collector
{
    interface ICollector<T, E>
    {
        IBuilder<T, E> Builder { get; }
        void Collect();
        void Collect(E query, DateTime start);
    }
}
