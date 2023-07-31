using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Collector.ProcessExporter;

namespace BuisnessLogic.Model
{
    internal class CollectManager
    {
        //internal ICollector<Groups, eProcessExporterData> processExporter { get; set; }
        //internal ICollector<, eNodeExporterData> nodeExporter { get; set; }
        
        public CollectManager()
        {
            //processExporter = new ProcessExporterCollector();
            //nodeExporter = new NodeExporterCollector();
        }
    }
}
