
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Model;
using BuisnessLogic.Algorithms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server_cloudata.Services.Predict
{
    public interface IPredicteService
    {
        Task<Metric> GetPredictResult(eNodeExporterData eNodeExporter, string machineName, IPredictiveAlgorithm predictAlgo);
        Task<List<Metric>> GetPredictResult(eProcessExporterData eProcessExporterData, string machineName, IPredictiveAlgorithm predictAlgo);
    }
}
