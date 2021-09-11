using Fixer.Core.Scheduler;
using Fixer.Hosting.Extension;
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
using YellowPages.Client.ReportPublisher.MessageBus;
using YellowPages.Client.ReportPublisher.Workers;
using YellowPages.Queue.Common.Abstract;
using YellowPages.Queue.Common.Concrete;

namespace YellowPages.Client.ReportPublisher
{
    public class Startup
    {
        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs\\Log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            BaseBootstrapper bootstrapper = new BaseBootstrapper(services);

            services.AddSingleton<ISchedulerTask, ReportPublisherWorker>();
            services.AddSingleton<IPublisherService, MessageBusPublisherService>();
            services.AddTransient<IMessageBusService, MessageBusCommonService>();

            services.AddScheduler(Configuration);

            services.AddHttpContextAccessor();
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
