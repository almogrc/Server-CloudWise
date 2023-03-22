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
                string url = $@"{URLBase}{Query_range(CpuUsageQuery())}&start={1679508923/*ConvertToUnix(start)*/}&end={1679515857/*ConvertToUnix(end)*/}&step={steps}";

                return RequestClient.GetAsync(url).Result;
            }catch(Exception e)
            {
                throw e;
            }
        }

        public override string GetMemory()
        {
            //todo
            try
            {
                string url = "http://20.85.68.63:9090/api/v1/query_range?query=node_memory_MemTotal_bytes{instance='10.0.0.4:9100'}-node_memory_MemFree_bytes{instance='10.0.0.4:9100'}-node_memory_Buffers_bytes{instance ='10.0.0.4:9100'}-node_memory_Cached_bytes{instance='10.0.0.4:9100'}&start=1679508923&end=1679515857&step=15s";
                return RequestClient.GetAsync(url).Result;
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
