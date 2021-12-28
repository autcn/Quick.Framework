using Quick;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dapper
{
    public interface IDapperDbContext : IDbContext
    {
        #region Get

        TEntity Get<TEntity>(object id);
        TEntity Get<TEntity>(CrudIdBuilder builder);
        object Get(CrudIdTypeBuilder builder);
        Task<TEntity> GetAsync<TEntity>(object id);
        Task<TEntity> GetAsync<TEntity>(CrudIdBuilder builder);
        Task<object> GetAsync(CrudIdTypeBuilder builder);

        TEntity GetOrDefault<TEntity>(object id);
        TEntity GetOrDefault<TEntity>(CrudIdBuilder builder);
        object GetOrDefault(CrudIdTypeBuilder builder);
        Task<TEntity> GetOrDefaultAsync<TEntity>(object id);
        Task<TEntity> GetOrDefaultAsync<TEntity>(CrudIdBuilder builder);
        Task<object> GetOrDefaultAsync(CrudIdTypeBuilder builder);

        IEnumerable<TEntity> GetList<TEntity>();
        IEnumerable<TEntity> GetList<TEntity>(object conditionObject, string appendSql);
        IEnumerable<TEntity> GetList<TEntity>(CrudListBuilder builder);
        IEnumerable<object> GetList(CrudListTypeBuilder builder);

        Task<IEnumerable<TEntity>> GetListAsync<TEntity>();
        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(object conditionObject, string appendSql);
        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(CrudListBuilder builder);
        Task<IEnumerable<object>> GetListAsync(CrudListTypeBuilder builder);

        IEnumerable<TEntity> GetListPaged<TEntity>(object conditionObject, string orderBy, int page, int rowsPerPage);
        IEnumerable<TEntity> GetListPaged<TEntity>(CrudPagedQueryListBuilder builder);
        IEnumerable<object> GetListPaged(CrudPagedQueryListTypeBuilder builder);

        Task<IEnumerable<TEntity>> GetListPagedAsync<TEntity>(object conditionObject, string orderBy, int page, int rowsPerPage);
        Task<IEnumerable<TEntity>> GetListPagedAsync<TEntity>(CrudPagedQueryListBuilder builder);
        Task<IEnumerable<object>> GetListPagedAsync(CrudPagedQueryListTypeBuilder builder);

        #endregion

        #region Insert
        int Insert<TEntity>(TEntity entity) where TEntity : class;
        int Insert(CrudEntityBuilder builder);
        Task<int> InsertAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> InsertAsync(CrudEntityBuilder builder);
        #endregion

        #region Update
        int Update<TEntity>(TEntity entity) where TEntity : class;
        int Update(CrudEntityBuilder builder);
        Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> UpdateAsync(CrudEntityBuilder builder);

        int UpdateWhere<TEntity>(TEntity entity, object conditionObject, params string[] fields) where TEntity : class;
        int UpdateWhere(CrudUpdateWhereBuilder builder);
        Task<int> UpdateWhereAsync<TEntity>(TEntity entity, object conditionObject, params string[] fields) where TEntity : class;
        Task<int> UpdateWhereAsync(CrudUpdateWhereBuilder builder);
        #endregion

        #region Delete
        int Delete<TEntity>(TEntity id) where TEntity : class;
        int Delete<TEntity>(object id);
        int Delete<TEntity>(CrudIdBuilder builder);
        int Delete(CrudIdTypeBuilder builder);
        int DeleteList<TEntity>(object conditionObject);
        int DeleteList<TEntity>();
        int DeleteList<TEntity>(string appendSql);
        int DeleteList<TEntity>(CrudListBuilder builder);
        int DeleteList(CrudListTypeBuilder builder);

        Task<int> DeleteAsync<TEntity>(TEntity id) where TEntity : class;
        Task<int> DeleteAsync<TEntity>(object id);
        Task<int> DeleteAsync<TEntity>(CrudIdBuilder builder);
        Task<int> DeleteAsync(CrudIdTypeBuilder builder);
        Task<int> DeleteListAsync<TEntity>(object conditionObject);
        Task<int> DeleteListAsync<TEntity>();
        Task<int> DeleteListAsync<TEntity>(string appendSql);
        Task<int> DeleteListAsync<TEntity>(CrudListBuilder builder);
        Task<int> DeleteListAsync(CrudListTypeBuilder builder);
        #endregion

        #region Transaction
        void Transaction(Action<IDbTransaction> action);
        Task TransactionAsync(Action<IDbTransaction> action);
        #endregion

        #region Other
        int Count<TEntity>(object conditionObject);
        int Count<TEntity>();
        int Count<TEntity>(CrudListBuilder builder);
        int Count(CrudListTypeBuilder builder);

        Task<int> CountAsync<TEntity>(object conditionObject);
        Task<int> CountAsync<TEntity>();
        Task<int> CountAsync<TEntity>(CrudListBuilder builder);
        Task<int> CountAsync(CrudListTypeBuilder builder);

        bool Exist<TEntity>(object conditionObject);
        bool Exist<TEntity>();
        bool Exist<TEntity>(CrudListBuilder builder);
        bool Exist(CrudListTypeBuilder builder);

        Task<bool> ExistAsync<TEntity>(object conditionObject);
        Task<bool> ExistAsync<TEntity>();
        Task<bool> ExistAsync<TEntity>(CrudListBuilder builder);
        Task<bool> ExistAsync(CrudListTypeBuilder builder);

        #endregion
    }
}
