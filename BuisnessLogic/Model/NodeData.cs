using BuisnessLogic.Collector.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    internal class NodeData
    {
        public Dictionary<eNodeExporterData, LinkedList<DataPoint>> Data { get; set; }
        public NodeData()
        {
            Data = new Dictionary<eNodeExporterData, LinkedList<DataPoint>>();
        }
    }
}
