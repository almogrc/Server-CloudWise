using BuisnessLogic.Collector;
using BuisnessLogic.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BuisnessLogic.Loggers;

namespace Server_cloudata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var host = CreateHostBuilder(args).Build();

            //var logger = host.Services.GetRequiredService<ILogger<Machine>>();
            //logger.LogInformation("Host created.");
            try
            {
                //Machine machine = new Machine();
                //machine.CollectInformation();
                //machine.PredictData();
                //string memoryResults = nodeExporter.GetMemory();
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //Logger.Instance.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                /*.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Information)*/;
    }
}
