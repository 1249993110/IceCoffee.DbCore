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
        public async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return await Task.Run(() => { return InsertBatch(entities, useTransaction); });
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
        
        public async Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return await Task.Run(() => { return DeleteBatch(entities, useTransaction); });
        }
        
        public async Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id)
        {
            return await Task.Run(() => { return DeleteById(idColumnName, id); });
        }
        
        public async Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            return await Task.Run(() => { return DeleteBatchByIds(idColumnName, ids, useTransaction); });
        }
        #endregion Delete

        #region Query
        
        public async Task<IEnumerable<TEntity>> QueryAnyAsync(string columnNames, string whereBy = null, string orderBy = null, object param = null)
        {
            return await Task.Run(() => { return QueryAny(columnNames, whereBy, orderBy, param); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryAllAsync(string orderBy = null)
        {
            return await Task.Run(() => { return QueryAll(orderBy); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id)
        {
            return await Task.Run(() => { return QueryById(idColumnName, id); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            return await Task.Run(() => { return QueryByIds(idColumnName, ids); });
        }
        
        public async Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            return await Task.Run(() =>
            {
                return QueryPaged(pageIndex, pageSize, whereBy, orderBy, param);
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
        
        public async Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return await Task.Run(() => { return UpdateBatch(entities, useTransaction); });
        }
        
        public async Task<int> UpdateByIdAsync(string idColumnName, TEntity entity)
        {
            return await Task.Run(() => { return UpdateById(idColumnName, entity); });
        }
        
        public async Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            return await Task.Run(() => { return UpdateColumnById(idColumnName, id, valueColumnName, value); });
        }
        #endregion Update
    }
}