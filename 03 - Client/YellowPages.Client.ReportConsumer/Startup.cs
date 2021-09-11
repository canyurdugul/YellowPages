using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YellowPages.Client.ReportConsumer.MessageBus;
using YellowPages.Client.ReportConsumer.Services;
using YellowPages.Queue.Common.Abstract;
using YellowPages.Queue.Common.Concrete;
using YellowPages.Queue.Process.Abstract;
using YellowPages.Queue.Process.Concrete;

namespace YellowPages.Client.ReportConsumer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs\\Log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            BaseBootstrapper bootstrapper = new BaseBootstrapper(services);
            services.AddHttpContextAccessor();

            services.AddTransient<IConsumerService, MessageBusConsumerService>();
            services.AddTransient<IMessageBusService, MessageBusCommonService>();

            services.AddTransient<IApplicationStarter, ConsumerStarter>();
            services.AddTransient<IProcessService, ProcessService>();

            services.AddHostedService<LifetimeEventsHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
