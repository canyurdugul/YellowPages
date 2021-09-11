using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YellowPages.Queue.Common.Abstract;

namespace YellowPages.Client.ReportConsumer.Services
{
    public class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IEnumerable<IApplicationStarter> _applicationStarters;

        public LifetimeEventsHostedService(ILogger<LifetimeEventsHostedService> logger, IHostApplicationLifetime appLifetime, IEnumerable<IApplicationStarter> applicationStarters)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _applicationStarters = applicationStarters;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            foreach (var app in _applicationStarters)
                app.Start();

            _logger.LogInformation("OnStarted has been called.");
        }

        private void OnStopping()
        {
            foreach (var app in _applicationStarters)
                app.Stop();

            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called.");
        }
    }
}
