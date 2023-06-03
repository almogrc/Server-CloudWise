using BuisnessLogic.Collector.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Model
{
    /// <summary>
    /// in one group we hold one kind of program that can run few times (process)
    /// </summary>
    public class Group
    {
        public string Name { get; set; }
        public int NumberOfProcesses { get; set; } = 1;
        //cpuUsage
        public Dictionary<eCPUMode, LinkedList<DataPoint>> CpuUsage { get; set; }
        //memoryUsage
        public Dictionary<eMemoryType, LinkedList<DataPoint>> MemoryUsage { get; set; }
        public Dictionary<eProcessExporterData, LinkedList<DataPoint>> Data { get; set; } 
        public Group(string name, int numberOfProcesses = 1)
        {
            Name = name;
            NumberOfProcesses = numberOfProcesses;
            Data = new Dictionary<eProcessExporterData, LinkedList<DataPoint>>();
            CpuUsage = new Dictionary<eCPUMode, LinkedList<DataPoint>>();
            MemoryUsage = new Dictionary<eMemoryType, LinkedList<DataPoint>>();
        }
        


        

    }
}
