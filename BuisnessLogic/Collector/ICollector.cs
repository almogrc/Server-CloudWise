using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessLogic.Collector.Builder;
using BuisnessLogic.Collector.Prometheus;

namespace BuisnessLogic.Collector
{
    public interface ICollector<E>
    {
        //Task<string> BuildQuery(string query, DateTime from, DateTime to, string address, params string[] values)
        //Task<string> Collect();
        Task<string> Collect(string query, DateTime from, DateTime to, string address, params string[] values);
        Task<string> Collect(string query, string address);
    }
}
