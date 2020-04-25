using System.Collections.Generic;
using BluSenseWorker.Models;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker.DataAccess
{
    public class RepFileDataAccess : DefaultDataAccess<RepFile>
    {
        public RepFileDataAccess(IConfiguration configuration) : base(configuration) { }

        public void InsertList(IEnumerable<RepFile> data)
        {
            var sql = "INSERT INTO dbo.RepFiles (Model ,SN ,LogDate ,LogTime ,TestItem ,Result ,SWVersion ,REPFile ,LOGFile ,IMGFile ,UseStatus, LogDateTime) VALUES " +
                "(@Model ,@SN ,@Date ,@Time ,@TestItem ,@Result ,@SWVersion ,@REP ,@LOG ,@IMG ,@Use, @LogDateTime)";
            SaveListByDapper(data, sql);
        }

        internal RepFile GetLastRepFile(RepFile condition)
        {
            var sql = "SELECT TOP 1 SN, LogDate as Date, LogTime as Time FROM dbo.RepFiles WHERE SN = @SN ORDER BY LogDate, LogTime DESC;";
            return Query(sql, condition);
        }
    }
}
