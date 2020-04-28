using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Scheduler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var milliseconds = this._configuration.GetValue<int>("IntervalMinutes") * 60 * 1000;

                bool isExist = IsTarFileExists();
                if (isExist) DoWork();

                await Task.Delay(milliseconds, stoppingToken);
            }
        }

        private bool IsTarFileExists()
        {
            var path = this._configuration.GetValue<string>("FolderPath");
            var fileCount = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).Length;
            _logger.LogInformation($"Checking {path} at {DateTime.Now}");
            _logger.LogInformation($"File count: {fileCount}");

            return fileCount > 0;
        }

        private void DoWork()
        {
            var cmd = _configuration.GetValue<string>("Cmd");
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = cmd,
                UseShellExecute = false,
                RedirectStandardError = true,
            };

            _logger.LogInformation($"cmd run: {cmd}");
            process.Start();
        }
    }
}
