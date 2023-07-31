using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server_cloudata.Models;
using Server_cloudata.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using BuisnessLogic.Collector.Enums;
using BuisnessLogic.Model;
using BuisnessLogic.Collector.NodeExporter;
using BuisnessLogic.Collector;
using Server_cloudata.Services.Collector;
using Server_cloudata.Middleware;
using BuisnessLogic.Collector.Builder;

namespace Server_cloudata
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //~~~
            services.Configure<CustomerDatabaseSettings>(Configuration.GetSection("CustomerDatabase"));
            services.AddSingleton<CustomersService>();
            services.AddTransient<ICollectorService<Metric>, NodeCollectorService>();
            services.AddTransient<ICollector<eNodeExporterData>, NodeExporterCollector>();
            services.AddTransient<IBuilder<List<DataPoint>>, DataPointsBuilder>();
            services.AddLogging();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Replace with your React app's origin
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // Allow credentials (cookies)
                });
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(180);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();          
            app.UseAuthorization();
            app.UseMiddleware<Middleware.SessionMiddleware>();
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/machine"), appBuilder =>
            {
                appBuilder.UseMiddleware<MachineMiddleware>();
            });
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
