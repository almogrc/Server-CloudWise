using BuisnessLogic.Collector.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    internal class MachineDataManager // task manager
    {
        public Groups Groups { get; set; }
        public NodeData NodeData { get; set; }

        public MachineDataManager() 
        {
            Groups = new Groups();
            NodeData = new NodeData();
        }

    }
}
