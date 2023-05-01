using BuisnessLogic.Collector;
using BuisnessLogic.MachineInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Server_cloudata
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Machine machine = new Machine();
            machine.CollectInformation();
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
