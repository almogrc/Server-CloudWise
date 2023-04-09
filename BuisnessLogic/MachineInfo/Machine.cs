using BuisnessLogic.Requester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.MachineInfo
{
    public class Machine
    {
        public string IP { get; }
        public List<Process> Processes { get; }
        public ProcessExporter processExporter { get; }
    }
}
