using BuisnessLogic.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server_cloudata.Services.Collector
{
    public interface INodeCollectorService<T>
    {
        Task<T> GetData(string dataType, DateTime from, DateTime to, string address, params string[] values);
    }
}
