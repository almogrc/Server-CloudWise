using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Enums.Atributes;

namespace BuisnessLogic.Collector.Enums
{
    public enum eNodeExporterData
    {
        [QueryValue("node_memory_MemAvailable_bytes")]
        AvailabeBytes,
        [QueryValue("node_memory_MemTotal_bytes")]
        MemTotalBytes,
        [QueryValue("node_memory_MemFree_bytes")]
        MemFreeBytes,
        [QueryValue("node_memory_Cached_bytes")]
        MemCachedBytes,
        [QueryValue("node_memory_Buffers_bytes")]
        MemBuffersBytes,
        [QueryValue("node_memory_SReclaimable_bytes")]
        MemSRecliamableBytes,
        [TypeValue("GB")]
        [Alert]
        RamUsage,
        [TypeValue("Byte")]
        [QueryValue("node_network_receive_bytes_total")]
        [Alert]
        NetworkRecBytes,
        [TypeValue("Byte")]
        [QueryValue("node_network_transmit_bytes_total")]
        [Alert]
        NetworkTransmitBytes,
        [QueryValue("node_cpu_seconds_total")]
        [TypeValue("%")]
        [Alert]
        CPUUsage,
        [QueryValue("node_memory_MemTotal_bytes")]
        [TypeValue("GB")]
        Ram
    }
}
