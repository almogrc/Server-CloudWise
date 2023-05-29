using BuisnessLogic.Collector.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    internal class MachineData
    {
        public Dictionary<NodeExporterData, LinkedList<DataPoint>> Data { get; set; }
        public MachineData()
        {
            Data = new Dictionary<NodeExporterData, LinkedList<DataPoint>>();
        }
    }
}
