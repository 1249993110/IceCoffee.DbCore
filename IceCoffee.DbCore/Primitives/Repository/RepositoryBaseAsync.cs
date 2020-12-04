using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public abstract partial class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        #region Insert
        [CatchException("插入数据异常")]
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            return await ExecuteAsync(Insert_Statement_Fixed, entity);
        }
        [CatchException("批量插入数据异常")]
        public virtual async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return await ExecuteAsync(Insert_Statement_Fixed, entities, useTransaction);
        }

        #endregion Insert

        #region Delete
        [CatchException("删除任意数据异常")]
        public virtual async Task<int> DeleteAnyAsync(string whereBy, object param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await ExecuteAsync(sql, param, useTransaction);
        }
        [CatchException("删除数据异常")]
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return await ExecuteAsync(sql, entity);
        }
        [CatchException("批量删除数据异常")]
        public virtual async Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return await ExecuteAsync(sql, entities, useTransaction);
        }
        [CatchException("通过Id删除数据异常")]
        public virtual async Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return await ExecuteAsync(sql, new { Id = id });
        }
        [CatchException("通过Id批量删除数据异常")]
        public virtual async Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return await ExecuteAsync(sql, new { Ids = ids }, useTransaction);
        }
        #endregion Delete

        #region Query
        [CatchException("查询任意数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryAnyAsync(string columnNames = null, string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", columnNames ?? "*", TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return await QueryAsync<TEntity>(sql, param);
        }
        [CatchException("查询所有数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryAllAsync(string orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return await QueryAsync<TEntity>(sql, null);
        }
        [CatchException("通过Id查询数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return await QueryAsync<TEntity>(sql, new { Id = id });
        }
        [CatchException("通过Id批量查询数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return await QueryAsync<TEntity>(sql, new { Ids = ids });
        }

        [CatchException("获取记录条数异常")]
        public virtual async Task<long> QueryRecordCountAsync(string whereBy = null, object param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await ExecuteScalarAsync<long>(sql, param);
        }

        public abstract Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
           string whereBy = null, string orderBy = null, object param = null);

        #endregion Query

        #region Update
        [CatchException("更新任意数据异常")]
        public virtual async Task<int> UpdateAnyAsync(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await ExecuteAsync(sql, param, useTransaction);
        }
        [CatchException("更新数据异常")]
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return await ExecuteAsync(sql, entity);
        }
        [CatchException("批量更新意数据异常")]
        public virtual async Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return await ExecuteAsync(sql, entities, useTransaction);
        }
        [CatchException("通过Id更新数据异常")]
        public virtual async Task<int> UpdateByIdAsync(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return await ExecuteAsync(sql, entity);
        }
        [CatchException("通过Id更新记录的一列异常")]
        public virtual async Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return await ExecuteAsync(sql, new { Id = id, Value = value });
        }

        #endregion Update

        public abstract Task<int> ReplaceIntoAsync(TEntity entity, bool useLock = false);

        public abstract Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);

        public abstract Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
    }
}