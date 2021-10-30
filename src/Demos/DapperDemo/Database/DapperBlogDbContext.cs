using Dapper;
using Microsoft.Data.Sqlite;
using Quick;
using System;
using System.Data;

namespace DapperDemo.Database
{
    [SingletonDependency]
    [ConnectionStringName("Student")]
    public class StudentDapperDbContext : DapperDbContext
    {
        public StudentDapperDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        protected override void OnConfiguring(DapperCrudOptions options)
        {
            options.Dialect = DatabaseDialect.SQLite;
        }
        protected override IDbConnection CreateConnection(string connectionString)
        {
            return new SqliteConnection(connectionString);
        }
    }
}
