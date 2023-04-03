using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Requester
{
    public abstract class AgentApi
    {
        public abstract string Port { get; }

        public abstract string GetCpu(/*DateTime start, DateTime End,*/ string steps = "15s");
        public abstract string GetMemory();
        //more and more
    }
}
