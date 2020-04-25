using System;
using System.Collections.Generic;
using System.Linq;
using BluSenseWorker.DataAccess;
using BluSenseWorker.Models;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker.BusinessLogic
{
    public class RepFileBusinessLogic
    {
        private readonly IConfiguration _configuration;
        private readonly RepFileDataAccess _dataAccess;

        public RepFileBusinessLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            _dataAccess = new RepFileDataAccess(_configuration);
        }

        internal void Save(IEnumerable<RepFile> data)
        {
            var item = _dataAccess.GetLastRepFile(data.FirstOrDefault());
            var startIndex = data.ToList().FindIndex(x => x.SN == item?.SN && x.Date == item?.Date && x.Time == item?.Time);
            var endIndex = startIndex == 0 ? 0 : data.Count() - 1;
            var list = data.Skip(startIndex).Take(endIndex - startIndex);

            _dataAccess.InsertList(list);
        }
    }
}
