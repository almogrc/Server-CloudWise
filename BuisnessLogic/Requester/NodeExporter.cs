using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic.Requester
{
    public class NodeExporter : AgentApi
    {
        RequestClient RequestClient = new RequestClient();
        public string URLBase => $"http://{IP}:{Port}{Api}";
        public string IP => "20.85.68.63";
        public override string Port => "9090";
        public string Api => @"/api/v1";
        public string Query_range(string query) => $@"/query_range?query=irate({query})";
        public string CpuUsageQuery(string modeToExclude = "idle", string jobName = "DEMO-VMs", int timeInMinute = 5)
        {
            return $"node_cpu_seconds_total{{mode!='{modeToExclude}',job='{jobName}'}}[{timeInMinute}m]";
        }
        public long ConvertToUnix(DateTime time) => ((DateTimeOffset)time).ToUnixTimeSeconds();
        public override string GetCpu(/*DateTime start, DateTime end, */string steps = "15s")
        {
            try
            {
                long s = 1679320085;
                string url = $@"{URLBase}{Query_range(CpuUsageQuery())}&start={1679312407/*ConvertToUnix(start)*/}&end={1679320085/*ConvertToUnix(end)*/}&step={steps}";

                return RequestClient.GetAsync(url).Result;
            }catch(Exception e)
            {
                throw e;
            }
                 //(node_cpu_seconds_total{mode!=%22idle%22,job=%22DEMO-VMs%22}[5m])
                 //&start=1679312407&end=1679320085&step=15s
        }

        public override string GetMemory()
        {
            throw new NotImplementedException();
        }
    }
}
