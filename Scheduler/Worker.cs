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
            var pattern = this._configuration.GetValue<string>("FilePattern");
            var fileCount = Directory.GetFiles(path, pattern, SearchOption.AllDirectories).Length;

            _logger.LogInformation($"Checking {path} at {DateTime.Now}");
            _logger.LogInformation($"File count: {fileCount}");

            return fileCount > 0;
        }

        private void DoWork()
        {
            var cmd = _configuration.GetValue<string>("Cmd");
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                FileName = @"C:\Python39\python.exe",
                Arguments = $"\"{cmd}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var process = Process.Start(startInfo))
            {
                _logger.LogInformation($"cmd run: {cmd}");
                _logger.LogInformation($"cmd run: {process.StandardError.ReadToEnd()}");
                _logger.LogInformation($"cmd run: {process.StandardOutput.ReadToEnd()}");
            }
        }
    }
}
