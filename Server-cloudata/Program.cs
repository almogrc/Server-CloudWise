using BuisnessLogic.Requester;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server_cloudata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //NodeExporter nodeExporter = new NodeExporter();
            //string cpuResults = nodeExporter.GetCpu();
            //string memoryResults = nodeExporter.GetMemory();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
