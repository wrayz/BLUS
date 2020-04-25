using System.Collections.Generic;
using System.Linq;
using BluSenseWorker.Interfaces;
using BluSenseWorker.Models;
using ServiceStack.Text;

namespace BluSenseWorker.BusinessLogic
{
    public class RepFileParser : IParser
    {
        public IEnumerable<RepFile> RepFiles;
        private readonly IReader _reader;
        private List<RepFile> _data;

        public RepFileParser(IReader reader)
        {
            _reader = reader;
        }

        public void Parsing()
        {
            _data = new List<RepFile>();

            _reader.GetDictionaries()
                .ForEach(item => _data.Add(new RepFile
                {
                    Model = "D6.0",
                    Date = item.Get("Date").TrimEnd(),
                    Time = item.Get("Time").TrimEnd(),
                    TestItem = item.Get("Test Item"),
                    Result = item.Get("Result"),
                    SN = item.Get("SN").TrimEnd(),
                    SWVersion = item.Get("SW Version"),
                    REP = item.Get("REP"),
                    LOG = item.Get("LOG"),
                    IMG = item.Get("IMG"),
                    Use = item.Get("Use"),
                }));

            GetList();
        }

        public void GetList()
        {
            RepFiles = _data.Distinct(new RepFileComparer());
        }
    }
}
