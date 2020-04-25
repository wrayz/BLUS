using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BluSenseWorker.BusinessLogic;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker
{
    public class Worker
    {
        private readonly IConfiguration _configuration;

        public Worker(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Execute(string name)
        {
            var path = GetFilePath(name);
            try
            {
                Console.WriteLine($"[Worker Starting] Reading {path} at {DateTime.Now}");

                var reader = new RepFileReader(path);
                var parser = new RepFileParser(reader);
                parser.Parsing();

                Console.WriteLine($"[Worker DB] Saving RepFiles at {DateTime.Now}");
                var repFileLogic = new RepFileBusinessLogic(_configuration);
                repFileLogic.Save(parser.RepFiles);

                // Console.WriteLine($"[Worker DB] Saving BluBox at {DateTime.Now}");
                var bluboxLogic = new BluBoxBussinessLogic(_configuration);
                bluboxLogic.Save(name, parser.RepFiles.LastOrDefault());

                Console.WriteLine($"[Worder Finished] Done at {DateTime.Now}.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"[Worker Warning] {path} is not exited.");
            }
            catch (IOException)
            {
                Console.WriteLine($"[Worker Warning] {path} is being used by another process.");
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"[Worker Error] {e.ToString()}");
            }
        }

        private string GetFilePath(string name)
        {
            var folder = this._configuration.GetValue<string>("FolderPath");
            return $"{folder}/{name}.csv";
        }
    }
}
