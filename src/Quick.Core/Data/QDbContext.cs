using System;
using System.Data;

namespace Quick
{
    public abstract class QDbContext : IDbContext
    {
        public QDbContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IDbConnection _dbConnection;
        public IDbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    _dbConnection = CreateConnection(ConnectionString);
                }
                return _dbConnection;
            }
        }
        private string _connectionString;
        public virtual string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    string connStringName = ConnectionStringNameAttribute.GetConnStringName(GetType());
                    var dbConfig = _serviceProvider.GetService<DatabaseConfiguration>();
                    if (!dbConfig.ContainsKey(connStringName))
                    {
                        throw new QException($"The database connection name {connStringName} dose not exist in configuration.");
                    }
                    _connectionString = dbConfig[connStringName];
                }
                return _connectionString;
            }
        }

        private readonly IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;

        protected abstract IDbConnection CreateConnection(string connectionString);

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                if (_dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
                _dbConnection.Dispose();
                _dbConnection = null;
            }
        }
    }
}
