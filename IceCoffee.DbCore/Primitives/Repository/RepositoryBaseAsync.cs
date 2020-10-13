using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public abstract partial class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        #region Insert
        public async Task<int> InsertAsync(TEntity entity)
        {
            return await Task.Run(() => { return Insert(entity); });
        }
        public async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities)
        {
            return await Task.Run(() => { return InsertBatch(entities); });
        }
        #endregion Insert

        #region Delete
        
        public async Task<int> DeleteAnyAsync(string whereBy, object param = null, bool useTransaction = false)
        {
            return await Task.Run(() => { return DeleteAny(whereBy, param, useTransaction); });
        }
        
        public async Task<int> DeleteAsync(TEntity entity)
        {
            return await Task.Run(() => { return Delete(entity); });
        }
        
        public async Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities)
        {
            return await Task.Run(() => { return DeleteBatch(entities); });
        }
        
        public async Task<int> DeleteByIdAsync<TId>(TId id, string idColumnName)
        {
            return await Task.Run(() => { return DeleteById(id, idColumnName); });
        }
        
        public async Task<int> DeleteBatchByIdsAsync<TId>(IEnumerable<TId> ids, string idColumnName)
        {
            return await Task.Run(() => { return DeleteBatchByIds(ids, idColumnName); });
        }
        
        public async Task<int> DeleteAllAsync()
        {
            return await Task.Run(() => { return DeleteAll(); });
        }
        #endregion Delete

        #region Query
        
        public async Task<IEnumerable<TEntity>> QueryAnyAsync(string columnNames, string whereBy, string orderby, object param = null)
        {
            return await Task.Run(() => { return QueryAny(columnNames, whereBy, orderby, param); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryAllAsync(string orderby = null)
        {
            return await Task.Run(() => { return QueryAll(orderby); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(TId id, string idColumnName)
        {
            return await Task.Run(() => { return QueryById(id, idColumnName); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(IEnumerable<TId> ids, string idColumnName)
        {
            return await Task.Run(() => { return QueryByIds(ids, idColumnName); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryPagedAsync(int pageNumber, int rowsPerPage,
            string whereBy = null, string orderby = null, object param = null)
        {
            return await Task.Run(() =>
            {
                return QueryPaged(pageNumber, rowsPerPage, whereBy, orderby, param);
            });
        }
        
        public async Task<long> QueryRecordCountAsync(string whereBy = null, object param = null)
        {
            return await Task.Run(() => { return QueryRecordCount(whereBy, param); });
        }
        #endregion Query

        #region Update
        public async Task<int> UpdateAnyAsync(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return await Task.Run(() => { return UpdateAny(setClause, whereBy, param, useTransaction); });
        }
        
        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() => { return Update(entity); });
        }
        
        public async Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities)
        {
            return await Task.Run(() => { return UpdateBatch(entities); });
        }
        
        public async Task<int> UpdateByIdAsync<TId>(TEntity entity, TId id, string idColumnName)
        {
            return await Task.Run(() => { return UpdateById(entity, id, idColumnName); });
        }
        
        public async Task<int> UpdateColumnByIdAsync<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName)
        {
            return await Task.Run(() => { return UpdateColumnById(id, value, idColumnName, valueColumnName); });
        }
        #endregion Update
    }
}