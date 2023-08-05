using System.Threading.Tasks;
using System;

namespace Server_cloudata.Services.Collector
{
    public interface IProcessExporterService<T>
    {
        Task<T> GetData(string dataType, DateTime from, DateTime to, string address, params string[] values);
    }
}
