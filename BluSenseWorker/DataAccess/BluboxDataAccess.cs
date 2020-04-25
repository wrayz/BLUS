using System.Collections.Generic;
using BluSenseWorker.Models;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker.DataAccess
{
    public class BluboxDataAccess : DefaultDataAccess<Blubox>
    {
        private IConfiguration _configuration;

        public BluboxDataAccess(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        internal int? GetId(Blubox entity)
        {
            var sql = $"SELECT Id FROM Bluboxs WHERE Model = @Model AND SN = @SN;";
            return Query(sql, entity)?.Id;
        }

        internal void Insert(Blubox blubox)
        {
            var sql = "INSERT INTO dbo.BluBoxs (Model, SN, SwVersion, UseStatus, UpdateTime) VALUES (@Model, @SN, @SwVersion, @UseStatus, @UpdateTime);";
            var data = new List<Blubox>();
            data.Add(blubox);

            SaveListByDapper(data, sql);
        }

        internal void Update(Blubox blubox)
        {
            var sql = "UPDATE dbo.BluBoxs SET SwVersion =  @SwVersion, UseStatus =  @UseStatus, UpdateTime = @UpdateTime WHERE Model = @Model AND SN = @SN;";
            var data = new List<Blubox>();
            data.Add(blubox);

            SaveListByDapper(data, sql);
        }
    }
}
