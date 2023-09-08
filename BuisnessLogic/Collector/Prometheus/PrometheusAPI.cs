using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Extentions;
namespace BuisnessLogic.Collector.Prometheus
{
    public class PrometheusAPI
    {
        public string URLBase => $"http://cloudwiseproduction.westeurope.cloudapp.azure.com:{Port}{Api}";
        public string Api => @"/api/v1";
        public readonly string Port = "9090";
        public string QueryRange => @"/query_range";
        public string QueryKey => "query";
        public string StartKey => "start";
        public string EndKey => "end";
        public string stepKey => "step";

        public Uri BuildUrlQuery(string query)
        {
            return new Uri($@"{URLBase}/{QueryKey}?{QueryKey}={query}");
        }
        public Uri BuildUrlQueryRange(string query, DateTime from, DateTime to, string step = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}={query}&{StartKey}={from.ToRfc3339String()}&{EndKey}={to.ToRfc3339String()}&{stepKey}={CalculateStep(from, to)}");
        }
        public Uri BuildUrlQueryRangeWithRate(string query, DateTime from, DateTime to, string step = "1m", string rate = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}=rate({query}[{rate}])&{StartKey}={from.ToRfc3339String()}&{EndKey}={to.ToRfc3339String()}&{stepKey}={CalculateStep(from, to)}");
        }
        public Uri BuildUrlQueryRangeWithIRate(string query, DateTime from, DateTime to, string unit = "1" ,string step = "1m", string rate = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}=irate({query}[{rate}])*{unit}&{StartKey}={from.ToRfc3339String()}&{EndKey}={to.ToRfc3339String()}&{stepKey}={CalculateStep(from, to)}");
        }
        public Uri BuildUrlQueryRangeWithIRatePercentage(string query, DateTime from, DateTime to, string step = "1", string rate = "1m")
        {
            return new Uri($@"{URLBase}{QueryRange}?{QueryKey}=100*(1-irate({query}[{rate}]))&{StartKey}={from.ToRfc3339String()}&{EndKey}={to.ToRfc3339String()}&{stepKey}={CalculateStep(from, to)}");
        }
        public string Irate(string query, string rate = "1m") // query include the instance and params!
        {
            return $"irate({query}[{rate}])";
        }

        private string CalculateStep(DateTime from, DateTime to)
        {
            int maxPoints = 10000;
            TimeSpan duration = to - from;
            double step = duration.TotalMinutes / maxPoints;
            return $"{(int)Math.Ceiling(step)}m";
        }
        //public string Instance{ get; } = "9090";
    }
}
