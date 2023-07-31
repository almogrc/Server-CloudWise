using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Prometheus;

namespace BuisnessLogic.Collector
{
    public class AbstractExporter
    {
        protected RequestClient _client;
        protected string Instance => $"{IP}:{Port}";
        public string IP { get; set; }
        protected string Port { get; private set; }
        protected PrometheusAPI _prometheusAPI;

        public AbstractExporter(string port, string ip = "localhost")
        {
            IP = ip;
            Port = port;
            _client = new RequestClient();
            _prometheusAPI = new PrometheusAPI();
        }
    }

}


