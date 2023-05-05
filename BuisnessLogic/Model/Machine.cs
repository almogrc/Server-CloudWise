using BuisnessLogic.Collector;
using BuisnessLogic.Collector.ProcessExporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    public class Machine
    {
        public string IP { get; }
        internal MachineDataManager MachineDataManager { get; private set; }   
        internal CollectManager CollectManager { get; private set; }
     
        public Machine()
        {
            MachineDataManager = new MachineDataManager();
            CollectManager = new CollectManager();
        }
        public void CollectInformation()
        {
            //CollectManager.processExporter.Collect();
            //MachineDataManager.Groups = CollectManager.processExporter.Builder.GetResult();
            CollectManager.nodeExporter.Collect();
            MachineDataManager.MachineData = CollectManager.nodeExporter.Builder.GetResult();
        }
    }
}
