using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Requester
{
    public interface Exporter
    {
        string GetCpu(/*DateTime start, DateTime End,*/ string steps = "15s");
        string GetMemory();
        //more and more
    }
}
