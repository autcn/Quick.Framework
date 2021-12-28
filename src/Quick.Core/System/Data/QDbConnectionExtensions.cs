namespace System.Data
{
    public static class QDbConnectionExtensions
    {
        public static IDbTransaction OpenTransaction(this IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
            return dbConnection.BeginTransaction();
        }
    }
}
