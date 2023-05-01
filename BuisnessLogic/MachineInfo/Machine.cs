using BuisnessLogic.Collector;
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
        public Dictionary<string, Group> Groups { get; }
        internal ICollector<Group> processExporter { get; set; }



        public void CollectInformation()
        {
            processExporter = new ProcessExporterCollector();
            processExporter.Collect();
        }
    }
}
