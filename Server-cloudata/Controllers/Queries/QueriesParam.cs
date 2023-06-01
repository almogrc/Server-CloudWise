using BuisnessLogic.Collector.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Server_cloudata.Controllers.Queries
{
    public class QueriesParam
    {
        [BindRequired]
        public string Query { get; set; }
        
        [BindRequired]
        public string Exporter { get; set; }
        
        [BindRequired]
        public DateTime Start { get; set; }

        internal void CheckValidation()
        {
            eExporterType exporterType;
            if(!Enum.TryParse(Exporter.ToLower(), out exporterType))
            {
                throw new Exception($"Exporter : {Exporter} invalid parameter");
            }
            if(exporterType == eExporterType.node)
            {
                eNodeExporterData nodeExporterData;
                if (!Enum.TryParse(Query.ToLower(), out nodeExporterData))
                {
                    throw new Exception($"Query : {Query} invalid parameter");
                }
            }
            else
            {
                eProcessExporterData processExporterData;
                if (!Enum.TryParse(Query.ToLower(), out processExporterData))
                {
                    throw new Exception($"Query : {Query} invalid parameter");
                }
            }
        }
    }
}
