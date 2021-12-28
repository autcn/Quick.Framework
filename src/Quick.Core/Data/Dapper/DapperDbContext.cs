using Quick;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Dapper
{
    public abstract class DapperDbContext : QDbContext, IDapperDbContext
    {
        public DapperDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = new DapperCrudOptions();
            OnConfiguring(_options);
            _sqlBuilder = new DapperCrudSqlBuilder(_options);
        }

        private DapperCrudOptions _options;
        private DapperCrudSqlBuilder _sqlBuilder;

        protected virtual void OnConfiguring(DapperCrudOptions options)
        {

        }

        #region Get

        public TEntity Get<TEntity>(object id)
        {
            return Get<TEntity>(new CrudIdBuilder(id));
        }
        public TEntity Get<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return (TEntity)Get(typeBuilder);
        }
        public TEntity GetOrDefault<TEntity>(object id)
        {
            return GetOrDefault<TEntity>(new CrudIdBuilder(id));
        }
        public TEntity GetOrDefault<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return (TEntity)GetOrDefault(typeBuilder);
        }

        public Task<TEntity> GetAsync<TEntity>(object id)
        {
            return GetAsync<TEntity>(new CrudIdBuilder(id));
        }
        public Task<TEntity> GetAsync<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetAsync(typeBuilder).ContinueWith(task => (TEntity)task.Result);
        }
        public Task<TEntity> GetOrDefaultAsync<TEntity>(object id)
        {
            return GetOrDefaultAsync<TEntity>(new CrudIdBuilder(id));
        }
        public Task<TEntity> GetOrDefaultAsync<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetOrDefaultAsync(typeBuilder).ContinueWith(task => (TEntity)task.Result);
        }

        private (string, object) BuildGet(CrudIdTypeBuilder builder)
        {
            Type idType = builder.Id.GetType();
            var idProps = _sqlBuilder.GetIdProperties(builder.EntityType).ToList();

            if (!idProps.Any())
                throw new ArgumentException("Get<TEntity> only supports an entity with [Key] or Id property");

            var name = _sqlBuilder.GetTableName(builder.EntityType);
            var sb = new StringBuilder();
            sb.Append("Select ");
            //create a new empty instance of the type to get the base properties
            _sqlBuilder.BuildSelect(sb, _sqlBuilder.GetScaffoldableProperties(builder.EntityType),
                    builder.Fields, builder.FieldsPolicy);
            sb.AppendFormat(" from {0} where ", name);

            _sqlBuilder.BuildWhere(builder.EntityType, sb, idProps, null);

            object conditionObj = builder.Id;

            if (idType.IsSimpleType())
            {
                var dynParms = new DynamicParameters();
                dynParms.Add("@" + idProps.First().Name, builder.Id);
                conditionObj = dynParms;
            }

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("Get:{0} {1} with Id: {2}", builder.EntityType, sb, builder.Id));

            return (sb.ToString(), conditionObj);
        }
        public object Get(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildGet(builder);
            return DbConnection.QueryFirst(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }
        public object GetOrDefault(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildGet(builder);
            return DbConnection.QueryFirstOrDefault(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<object> GetAsync(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildGet(builder);
            return DbConnection.QueryFirstAsync(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<object> GetOrDefaultAsync(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildGet(builder);
            return DbConnection.QueryFirstOrDefaultAsync(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }

        public IEnumerable<TEntity> GetList<TEntity>(object conditionObject, string appendSql)
        {
            return GetList<TEntity>(new CrudListBuilder(conditionObject).SetAppendSql(appendSql));
        }
        public IEnumerable<TEntity> GetList<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetList(typeBuilder).Cast<TEntity>();
        }
        public IEnumerable<TEntity> GetList<TEntity>()
        {
            return GetList<TEntity>(null, null);
        }

        public Task<IEnumerable<TEntity>> GetListAsync<TEntity>(object conditionObject, string appendSql)
        {
            return GetListAsync<TEntity>(new CrudListBuilder(conditionObject).SetAppendSql(appendSql));
        }
        public Task<IEnumerable<TEntity>> GetListAsync<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetListAsync(typeBuilder).ContinueWith(task => task.Result.Cast<TEntity>());
        }
        public Task<IEnumerable<TEntity>> GetListAsync<TEntity>()
        {
            return GetListAsync<TEntity>(null, null);
        }

        private (string, object) BuildGetList(CrudListTypeBuilder builder)
        {
            var tableName = _sqlBuilder.GetTableName(builder.EntityType);
            object conditionObject = builder.ConditionObject;
            if (conditionObject != null && (!conditionObject.GetType().IsClass || conditionObject.GetType().IsSimpleType()))
            {
                throw new ArgumentException("conditionObject must be an object in class");
            }
            var sb = new StringBuilder();
            var whereprops = _sqlBuilder.GetAllProperties(conditionObject).ToArray();
            sb.Append("Select ");
            //create a new empty instance of the type to get the base properties
            _sqlBuilder.BuildSelect(sb, _sqlBuilder.GetScaffoldableProperties(builder.EntityType), builder.Fields, builder.FieldsPolicy);
            sb.AppendFormat(" from {0}", tableName);

            if (whereprops.Any())
            {
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, whereprops, conditionObject);
            }

            sb.Append(" " + builder.AppendSql ?? "");

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("GetList:{0} {1}", builder.EntityType, sb));

            return (sb.ToString(), conditionObject);
        }
        public IEnumerable<object> GetList(CrudListTypeBuilder builder)
        {
            var buildResult = BuildGetList(builder);
            return DbConnection.Query(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.IsBuffered, builder.CommandTimeout);
        }
        public Task<IEnumerable<object>> GetListAsync(CrudListTypeBuilder builder)
        {
            var buildResult = BuildGetList(builder);
            return DbConnection.QueryAsync(builder.EntityType, buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }

        public IEnumerable<TEntity> GetListPaged<TEntity>(object conditionObject, string orderBy, int page, int rowsPerPage)
        {
            return GetListPaged<TEntity>(new CrudPagedQueryListBuilder(conditionObject, orderBy).SetPage(page).SetRowsPerPage(rowsPerPage));
        }
        public IEnumerable<TEntity> GetListPaged<TEntity>(CrudPagedQueryListBuilder builder)
        {
            var typeBuilder = new CrudPagedQueryListTypeBuilder(typeof(TEntity), builder.ConditionObject, builder.OrderBy);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetListPaged(typeBuilder).Cast<TEntity>();
        }

        public Task<IEnumerable<TEntity>> GetListPagedAsync<TEntity>(object conditionObject, string orderBy, int page, int rowsPerPage)
        {
            return GetListPagedAsync<TEntity>(new CrudPagedQueryListBuilder(conditionObject, orderBy).SetPage(page).SetRowsPerPage(rowsPerPage));
        }
        public Task<IEnumerable<TEntity>> GetListPagedAsync<TEntity>(CrudPagedQueryListBuilder builder)
        {
            var typeBuilder = new CrudPagedQueryListTypeBuilder(typeof(TEntity), builder.ConditionObject, builder.OrderBy);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return GetListPagedAsync(typeBuilder).ContinueWith(task => task.Result.Cast<TEntity>());
        }

        private string BuildGetListPaged(CrudPagedQueryListTypeBuilder builder)
        {
            object conditionObject = builder.ConditionObject;
            if (conditionObject != null && (!conditionObject.GetType().IsClass || conditionObject.GetType().IsSimpleType()))
            {
                throw new ArgumentException("conditionObject must be an object in class");
            }
            if (string.IsNullOrEmpty(_options.GetPagedListSql))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            if (builder.Page < 1)
                throw new Exception("Page must be greater than 0");


            var whereprops = _sqlBuilder.GetAllProperties(conditionObject).ToArray();
            var idProps = _sqlBuilder.GetIdProperties(builder.EntityType).ToList();
            //if (!idProps.Any())
            //    throw new ArgumentException("Entity must have at least one [Key] property");

            var tableName = _sqlBuilder.GetTableName(builder.EntityType);
            var query = _options.GetPagedListSql;
            string orderby = builder.OrderBy;
            if (string.IsNullOrEmpty(builder.OrderBy))
            {
                orderby = _sqlBuilder.GetColumnName(idProps.First());
            }
            var sbWhere = new StringBuilder();
            if (whereprops.Any())
            {
                sbWhere.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sbWhere, whereprops, conditionObject);
            }
            sbWhere.Append(" " + builder.AppendSql ?? "");
            //create a new empty instance of the type to get the base properties
            var sbSelect = new StringBuilder();
            _sqlBuilder.BuildSelect(sbSelect, _sqlBuilder.GetScaffoldableProperties(builder.EntityType), builder.Fields, builder.FieldsPolicy);
            query = query.Replace("{SelectColumns}", sbSelect.ToString());
            query = query.Replace("{TableName}", tableName);
            query = query.Replace("{PageNumber}", builder.Page.ToString());
            query = query.Replace("{RowsPerPage}", builder.RowsPerPage.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", sbWhere.ToString());
            query = query.Replace("{Offset}", ((builder.Page - 1) * builder.RowsPerPage).ToString());

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("GetListPaged:{0} {1}", builder.EntityType, query));

            return query;
        }
        public IEnumerable<object> GetListPaged(CrudPagedQueryListTypeBuilder builder)
        {
            string strSql = BuildGetListPaged(builder);
            return DbConnection.Query(builder.EntityType, strSql, builder.ConditionObject, builder.DbTransaction, builder.IsBuffered, builder.CommandTimeout);
        }
        public Task<IEnumerable<object>> GetListPagedAsync(CrudPagedQueryListTypeBuilder builder)
        {
            string strSql = BuildGetListPaged(builder);
            return DbConnection.QueryAsync(builder.EntityType, strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout);
        }

        #endregion

        #region Insert
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return Insert(new CrudEntityBuilder(entity));
        }
        public Task<int> InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return InsertAsync(new CrudEntityBuilder(entity));
        }

        private (string, bool, PropertyInfo) BuildInsert(CrudEntityBuilder builder)
        {
            bool isAutoKey = false;
            bool genGuid = false;
            var idProps = _sqlBuilder.GetIdProperties(builder.EntityType).ToList();
            PropertyInfo idProperty = null;
            if (idProps.Count == 1)
            {
                idProperty = idProps.First();
                if (idProperty.PropertyType.IsNumberType())
                {
                    DatabaseGeneratedAttribute generatedAttribute = idProperty.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (generatedAttribute != null && generatedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                    {
                        isAutoKey = true;
                    }
                }
                else if (idProperty.PropertyType == typeof(Guid))
                {
                    DatabaseGeneratedAttribute generatedAttribute = idProperty.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (generatedAttribute != null && generatedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                    {
                        genGuid = true;
                    }
                }
            }
            var tableName = _sqlBuilder.GetTableName(builder.Entity);

            var sb = new StringBuilder();
            sb.AppendFormat("insert into {0}", tableName);
            sb.Append(" (");
            _sqlBuilder.BuildInsertParameters(builder.EntityType, sb, builder.Fields, builder.FieldsPolicy, isAutoKey);
            sb.Append(") ");
            sb.Append("values");
            sb.Append(" (");
            _sqlBuilder.BuildInsertValues(builder.EntityType, sb, builder.Fields, builder.FieldsPolicy, isAutoKey);
            sb.Append(")");

            //对于自增来说
            if (isAutoKey)
            {
                sb.Append(";" + _options.GetIdentitySql);
            }

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("Insert: {0}", sb));

            if (genGuid)
            {
                idProperty.SetValue(builder.Entity, GuidGenerator.SequentialGuid());
            }

            return (sb.ToString(), isAutoKey, idProperty);
        }
        public int Insert(CrudEntityBuilder builder)
        {
            var buildResult = BuildInsert(builder);
            string strSql = buildResult.Item1;
            bool isAutoKey = buildResult.Item2;
            PropertyInfo idProperty = buildResult.Item3;
            int ret = 0;
            if (isAutoKey)
            {
                ret = DbConnection.ExecuteScalar<int>(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout);
                if (ret > 0)
                    idProperty.SetValue(builder.Entity, ret);
            }
            else
            {
                ret = DbConnection.Execute(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout);
            }
            return ret;
        }
        public Task<int> InsertAsync(CrudEntityBuilder builder)
        {
            var buildResult = BuildInsert(builder);
            string strSql = buildResult.Item1;
            bool isAutoKey = buildResult.Item2;
            PropertyInfo idProperty = buildResult.Item3;

            if (isAutoKey)
            {
                return DbConnection.ExecuteScalarAsync<int>(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout)
                    .ContinueWith(task =>
                    {
                        if (task.Result > 0)
                            idProperty.SetValue(builder.Entity, task.Result);
                        return task.Result;
                    });
            }
            else
            {
                return DbConnection.ExecuteAsync(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout);
            }
        }
        #endregion

        #region Update
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            return Update(new CrudEntityBuilder(entity));
        }

        public Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return UpdateAsync(new CrudEntityBuilder(entity));
        }

        private string BuildUpdate(CrudEntityBuilder builder)
        {
            var masterSb = new StringBuilder();
            _sqlBuilder.StringBuilderCache(masterSb, $"{builder.EntityType.FullName}_Update_{builder.Fields}_{builder.FieldsPolicy}", sb =>
            {
                var idProps = _sqlBuilder.GetIdProperties(builder.EntityType).ToList();

                if (!idProps.Any())
                    throw new ArgumentException("Entity must have at least one [Key] or Id property");

                var tableName = _sqlBuilder.GetTableName(builder.EntityType);
                sb.AppendFormat("update {0}", tableName);
                sb.AppendFormat(" set ");
                _sqlBuilder.BuildUpdateSet(builder.EntityType, sb, builder.Fields, builder.FieldsPolicy, false);
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, idProps, null);

                if (Debugger.IsAttached)
                    Trace.WriteLine(String.Format("Update: {0}", sb));
            });
            return masterSb.ToString();
        }
        public int Update(CrudEntityBuilder builder)
        {
            string strSql = BuildUpdate(builder);
            return DbConnection.Execute(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<int> UpdateAsync(CrudEntityBuilder builder)
        {
            string strSql = BuildUpdate(builder);
            return DbConnection.ExecuteAsync(strSql, builder.Entity, builder.DbTransaction, builder.CommandTimeout);
        }

        public int UpdateWhere<TEntity>(TEntity entity, object conditionObject, params string[] fields) where TEntity : class
        {
            return UpdateWhere(new CrudUpdateWhereBuilder(entity, conditionObject).SetFields(fields));
        }

        public Task<int> UpdateWhereAsync<TEntity>(TEntity entity, object conditionObject, params string[] fields) where TEntity : class
        {
            return UpdateWhereAsync(new CrudUpdateWhereBuilder(entity, conditionObject).SetFields(fields));
        }

        private (string, object) BuildUpdateWhere(CrudUpdateWhereBuilder builder)
        {
            var sb = new StringBuilder();
            var tableName = _sqlBuilder.GetTableName(builder.EntityType);
            var whereprops = _sqlBuilder.GetAllProperties(builder.ConditionObject).ToArray();

            sb.AppendFormat("update {0}", tableName);
            sb.AppendFormat(" set ");
            _sqlBuilder.BuildUpdateSet(builder.EntityType, sb, builder.Fields, builder.FieldsPolicy, true);
            //sb.Append("Select ");
            //create a new empty instance of the type to get the base properties
            //_sqlBuilder.BuildSelect(sb, _sqlBuilder.GetScaffoldableProperties(builder.EntityType), builder.Fields, builder.FieldsPolicy);
            //sb.AppendFormat(" from {0}", tableName);
            string suffix = "_2";
            object paramObj = builder.Entity;

            //如果有条件，需要构造where语句，merge两个实体的值
            if (whereprops.Any())
            {
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, whereprops, builder.ConditionObject, suffix);
                //合并两个实体的参数
                var dynParms = new DynamicParameters();
                _sqlBuilder.AddObject(dynParms, builder.Entity, builder.Fields);
                _sqlBuilder.AddObject(dynParms, builder.ConditionObject, null, suffix);
                paramObj = dynParms;
            }
            sb.Append(" " + builder.AppendSql ?? "");

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("UpdateWhere: {0}", sb));
            return (sb.ToString(), paramObj);
        }
        public int UpdateWhere(CrudUpdateWhereBuilder builder)
        {
            var buildResult = BuildUpdateWhere(builder);
            return DbConnection.Execute(buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<int> UpdateWhereAsync(CrudUpdateWhereBuilder builder)
        {
            var buildResult = BuildUpdateWhere(builder);
            return DbConnection.ExecuteAsync(buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }

        #endregion

        #region Delete
        public int Delete<TEntity>(TEntity id) where TEntity : class
        {
            return Delete<TEntity>((object)id);
        }
        public int Delete<TEntity>(object id)
        {
            return Delete<TEntity>(new CrudIdBuilder(id));
        }
        public int Delete<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return Delete(typeBuilder);
        }

        public Task<int> DeleteAsync<TEntity>(TEntity id) where TEntity : class
        {
            return DeleteAsync<TEntity>((object)id);
        }
        public Task<int> DeleteAsync<TEntity>(object id)
        {
            return DeleteAsync<TEntity>(new CrudIdBuilder(id));
        }
        public Task<int> DeleteAsync<TEntity>(CrudIdBuilder builder)
        {
            var typeBuilder = new CrudIdTypeBuilder(typeof(TEntity), builder.Id);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return DeleteAsync(typeBuilder);
        }

        private (string, object) BuildDelete(CrudIdTypeBuilder builder)
        {
            var idProps = _sqlBuilder.GetIdProperties(builder.EntityType);

            if (!idProps.Any())
                throw new ArgumentException("Delete only supports an entity with a [Key] or Id property");

            var tableName = _sqlBuilder.GetTableName(builder.EntityType);

            var sb = new StringBuilder();
            sb.AppendFormat("Delete from {0} where ", tableName);

            _sqlBuilder.BuildWhere(builder.EntityType, sb, idProps, null);
            Type idType = builder.Id.GetType();
            object conditionObj = builder.Id;

            if (idType.IsSimpleType())
            {
                var dynParms = new DynamicParameters();
                dynParms.Add("@" + idProps.First().Name, builder.Id);
                conditionObj = dynParms;
            }

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("Delete:{0} {1}", builder.EntityType, sb));

            return (sb.ToString(), conditionObj);
        }
        public int Delete(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildDelete(builder);
            return DbConnection.Execute(buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<int> DeleteAsync(CrudIdTypeBuilder builder)
        {
            var buildResult = BuildDelete(builder);
            return DbConnection.ExecuteAsync(buildResult.Item1, buildResult.Item2, builder.DbTransaction, builder.CommandTimeout);
        }

        public int DeleteList<TEntity>(object conditionObject)
        {
            return DeleteList<TEntity>(new CrudListBuilder(conditionObject));
        }
        public int DeleteList<TEntity>()
        {
            return DeleteList<TEntity>(new CrudListBuilder(null));
        }
        public int DeleteList<TEntity>(string appendSql)
        {
            return DeleteList<TEntity>(new CrudListBuilder(null).SetAppendSql(appendSql));
        }
        public int DeleteList<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return DeleteList(typeBuilder);
        }

        public Task<int> DeleteListAsync<TEntity>(object conditionObject)
        {
            return DeleteListAsync<TEntity>(new CrudListBuilder(conditionObject));
        }
        public Task<int> DeleteListAsync<TEntity>()
        {
            return DeleteListAsync<TEntity>(new CrudListBuilder(null));
        }
        public Task<int> DeleteListAsync<TEntity>(string appendSql)
        {
            return DeleteListAsync<TEntity>(new CrudListBuilder(null).SetAppendSql(appendSql));
        }
        public Task<int> DeleteListAsync<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return DeleteListAsync(typeBuilder);
        }

        private string BuildDeleteList(CrudListTypeBuilder builder)
        {
            var sb = new StringBuilder();

            var name = _sqlBuilder.GetTableName(builder.EntityType);

            var whereprops = _sqlBuilder.GetAllProperties(builder.ConditionObject).ToArray();
            sb.AppendFormat("Delete from {0}", name);
            if (whereprops.Any())
            {
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, whereprops, builder.ConditionObject);
            }

            sb.Append(" " + builder.AppendSql ?? "");

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("DeleteList:{0} {1}", builder.EntityType, sb));
            return sb.ToString();
        }
        public int DeleteList(CrudListTypeBuilder builder)
        {
            string strSql = BuildDeleteList(builder);
            return DbConnection.Execute(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<int> DeleteListAsync(CrudListTypeBuilder builder)
        {
            string strSql = BuildDeleteList(builder);
            return DbConnection.ExecuteAsync(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout);
        }
        #endregion

        #region  Transaction
        public void Transaction(Action<IDbTransaction> action)
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }
            IDbTransaction dbTransaction = DbConnection.BeginTransaction();
            try
            {
                action?.Invoke(dbTransaction);
                dbTransaction.Commit();
            }
            catch
            {
                dbTransaction.Rollback();
                throw;
            }
        }
        public Task TransactionAsync(Action<IDbTransaction> action)
        {
            return Task.Run(() => Transaction(action));
        }
        #endregion

        #region Other

        public int Count<TEntity>(object conditionObject)
        {
            return Count<TEntity>(new CrudListBuilder(conditionObject));
        }
        public int Count<TEntity>()
        {
            return Count<TEntity>(new CrudListBuilder(null));
        }
        public int Count<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return Count(typeBuilder);
        }

        public Task<int> CountAsync<TEntity>(object conditionObject)
        {
            return CountAsync<TEntity>(new CrudListBuilder(conditionObject));
        }
        public Task<int> CountAsync<TEntity>()
        {
            return CountAsync<TEntity>(new CrudListBuilder(null));
        }
        public Task<int> CountAsync<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return CountAsync(typeBuilder);
        }

        private string BuildCount(CrudListTypeBuilder builder)
        {
            var tableName = _sqlBuilder.GetTableName(builder.EntityType);

            var sb = new StringBuilder();
            var whereprops = _sqlBuilder.GetAllProperties(builder.ConditionObject).ToArray();
            sb.Append("Select count(1)");
            sb.AppendFormat(" from {0}", tableName);
            if (whereprops.Any())
            {
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, whereprops, builder.ConditionObject);
            }
            sb.Append(" " + builder.AppendSql ?? "");

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("Count:{0}", sb));
            return sb.ToString();
        }
        public int Count(CrudListTypeBuilder builder)
        {
            string strSql = BuildCount(builder);
            return DbConnection.ExecuteScalar<int>(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout);
        }
        public Task<int> CountAsync(CrudListTypeBuilder builder)
        {
            string strSql = BuildCount(builder);
            return DbConnection.ExecuteScalarAsync<int>(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout);
        }

        public bool Exist<TEntity>(object conditionObject)
        {
            return Exist<TEntity>(new CrudListBuilder(conditionObject));
        }
        public bool Exist<TEntity>()
        {
            return Exist<TEntity>(new CrudListBuilder(null));
        }
        public bool Exist<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return Exist(typeBuilder);
        }

        public Task<bool> ExistAsync<TEntity>(object conditionObject)
        {
            return ExistAsync<TEntity>(new CrudListBuilder(conditionObject));
        }
        public Task<bool> ExistAsync<TEntity>()
        {
            return ExistAsync<TEntity>(new CrudListBuilder(null));
        }
        public Task<bool> ExistAsync<TEntity>(CrudListBuilder builder)
        {
            var typeBuilder = new CrudListTypeBuilder(typeof(TEntity), builder.ConditionObject);
            typeBuilder.Import(builder);
            typeBuilder.SetEntityType(typeof(TEntity));
            return ExistAsync(typeBuilder);
        }

        private string BuildExist(CrudListTypeBuilder builder)
        {
            var name = _sqlBuilder.GetTableName(builder.EntityType);
            var sb = new StringBuilder();
            var whereprops = _sqlBuilder.GetAllProperties(builder.ConditionObject).ToArray();
            if (_options.Dialect == DatabaseDialect.SQLServer)
            {
                sb.Append("Select top 1 1 AS C");
            }
            else
            {
                sb.Append("Select 1 AS C");
            }
            sb.AppendFormat(" from {0}", name);
            if (whereprops.Any())
            {
                sb.Append(" where ");
                _sqlBuilder.BuildWhere(builder.EntityType, sb, whereprops, builder.ConditionObject);
            }
            sb.Append(" " + builder.AppendSql ?? "");
            if (_options.Dialect != DatabaseDialect.SQLServer)
            {
                sb.Append(" limit 1");
            }
            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("Exist:{0} {1}", builder.EntityType, sb));
            return sb.ToString();
        }
        public bool Exist(CrudListTypeBuilder builder)
        {
            string strSql = BuildExist(builder);
            return DbConnection.ExecuteScalar<int?>(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout) != null;
        }
        public Task<bool> ExistAsync(CrudListTypeBuilder builder)
        {
            string strSql = BuildExist(builder);
            return DbConnection.ExecuteScalarAsync<int?>(strSql, builder.ConditionObject, builder.DbTransaction, builder.CommandTimeout)
                .ContinueWith(task => task.Result != null);
        }

        #endregion
    }
}
