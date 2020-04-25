using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker.DataAccess
{
    public abstract class DefaultDataAccess<T>
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnectionStringBuilder _builder;

        public DefaultDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _builder = new SqlConnectionStringBuilder(this._configuration.GetConnectionString("DefaultConnection"));
        }

        public T Query(string sql, T entity)
        {
            T obj;

            using(SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();

                using(SqlTransaction transaction = connection.BeginTransaction())
                {
                    obj = connection.QuerySingleOrDefault<T>(sql, entity, transaction: transaction);
                    transaction.Commit();
                }
            }

            return obj;
        }

        internal void SaveListByDapper(IEnumerable<T> data, string sql)
        {
            using(SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();

                using(SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(sql, data, transaction : transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
