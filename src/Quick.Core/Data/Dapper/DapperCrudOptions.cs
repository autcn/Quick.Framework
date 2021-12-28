namespace Dapper
{
    public enum DatabaseDialect
    {
        SQLServer,
        PostgreSQL,
        SQLite,
        MySQL,
    }
    public class DapperCrudOptions
    {
        public DapperCrudOptions()
        {
            Dialect = DatabaseDialect.SQLServer;
            TableNameResolver = new DefaultTableNameResolver(this);
            ColumnNameResolver = new DefaultColumnNameResolver(this);
        }
        #region Private members
        private DatabaseDialect _dialect;

        public string Encapsulation { get; private set; }
        public string GetIdentitySql { get; private set; }
        public string GetPagedListSql { get; private set; }

        #endregion

        #region Public
        public bool StringBuilderCacheEnabled { get; set; } = true;
        public ITableNameResolver TableNameResolver { get; set; }
        public IColumnNameResolver ColumnNameResolver { get; set; }
        public DatabaseDialect Dialect
        {
            get => _dialect;
            set
            {
                _dialect = value;
                switch (_dialect)
                {
                    case DatabaseDialect.PostgreSQL:
                        _dialect = DatabaseDialect.PostgreSQL;
                        Encapsulation = "\"{0}\"";
                        GetIdentitySql = string.Format("SELECT LASTVAL() AS id");
                        GetPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                        break;
                    case DatabaseDialect.SQLite:
                        _dialect = DatabaseDialect.SQLite;
                        Encapsulation = "\"{0}\"";
                        GetIdentitySql = string.Format("SELECT LAST_INSERT_ROWID() AS id");
                        GetPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                        break;
                    case DatabaseDialect.MySQL:
                        _dialect = DatabaseDialect.MySQL;
                        Encapsulation = "`{0}`";
                        GetIdentitySql = string.Format("SELECT LAST_INSERT_ID() AS id");
                        GetPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {Offset},{RowsPerPage}";
                        break;
                    default:
                        _dialect = DatabaseDialect.SQLServer;
                        Encapsulation = "[{0}]";
                        GetIdentitySql = string.Format("SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [id]");
                        GetPagedListSql = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNumber BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                        break;
                }
            }
        }
        #endregion
    }
}
