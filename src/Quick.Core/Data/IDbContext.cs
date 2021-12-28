using System;
using System.Data;

namespace Quick
{
    public interface IDbContext : IDisposable
    {
        IDbConnection DbConnection { get; }
        string ConnectionString { get; }
    }
}
