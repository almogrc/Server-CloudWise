﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Extentions;
namespace BuisnessLogic.Collector.Prometheus
{
    internal class PrometheusAPI
    {
        public string URLBase => $"http://localhost:{Port}{Api}";
        public string Api => @"/api/v1";
        public readonly string Port = "9090";
        public string QueryRange => @"/query_range";
        public string QueryKey => "query";
        public string StartKey => "start";
        public string EndKey => "end";
        public string stepKey => "step";
        public Uri BuildUrlQueryRange(string query, DateTime start, DateTime end, string step = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}={query}&{StartKey}={start.ToRfc3339String()}&{EndKey}={end.ToRfc3339String()}&{stepKey}={step}");
        }
        public Uri BuildUrlQueryRangeWithRate(string query, DateTime start, DateTime end, string step = "1m", string rate = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}=rate({query}[{rate}])&{StartKey}={start.ToRfc3339String()}&{EndKey}={end.ToRfc3339String()}&{stepKey}={step}");
        }

        //public string Instance{ get; } = "9090";
    }
}
