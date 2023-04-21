using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector
{
    interface ICollector
    {
        void Collect();
        void Collect(string information);
    }
}
