﻿using BuisnessLogic.Collector.Enums.Atributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Collector.Enums
{
    public enum eProcessExporterData
    {
        [QueryValue("namedprocess_namegroup_cpu_seconds_total")]
        cpu,
        [QueryValue("namedprocess_namegroup_memory_bytes")]
        proportionalMemoryResident,
        [QueryValue("namedprocess_namegroup_read_bytes_total")]
        readBytes
    }
}