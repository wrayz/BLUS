using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Scheduler
{
    public class TimedHostedService: IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IConfiguration _configuration;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration)
       {
           _logger = logger;
           _configuration = configuration;
       }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Checking folder...");
            var path = this._configuration.GetValue<string>("FolderPath");
            var fileCount = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).Length;

            _logger.LogInformation($"{fileCount}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping check.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}