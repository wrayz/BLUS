using System;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute(args);
        }

        private static void Execute(params string[] names)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                           .Build();

            var worker = new Worker(configuration);
            foreach (var name in names)
                worker.Execute(name);
        }
    }
}
