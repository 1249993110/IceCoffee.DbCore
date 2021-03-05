using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public abstract partial class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        #region Insert
        /// <inheritdoc />
        [CatchException("插入数据异常")]
        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            return await base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }
        /// <inheritdoc />
        [CatchException("批量插入数据异常")]
        public virtual async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return await base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        #endregion Insert

        #region Delete
        /// <inheritdoc />
        [CatchException("删除数据异常")]
        public virtual async Task<int> DeleteAsync(string whereBy, object param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await base.ExecuteAsync(sql, param, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("删除数据异常")]
        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return await base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("批量删除数据异常")]
        public virtual async Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return await base.ExecuteAsync(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("通过Id删除数据异常")]
        public virtual async Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return await base.ExecuteAsync(sql, new { Id = id });
        }
        /// <inheritdoc />
        [CatchException("通过Id批量删除数据异常")]
        public virtual async Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return await base.ExecuteAsync(sql, new { Ids = ids }, useTransaction);
        }
        #endregion Delete

        #region Query
        /// <inheritdoc />
        [CatchException("查询数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryAsync(string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format("SELECT {0} FROM {0} {1} {2}", TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return await base.QueryAsync<TEntity>(sql, param);
        }
        /// <inheritdoc />
        [CatchException("查询所有数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryAllAsync(string orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return await base.QueryAsync<TEntity>(sql, null);
        }
        /// <inheritdoc />
        [CatchException("通过Id查询数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return await base.QueryAsync<TEntity>(sql, new { Id = id });
        }
        /// <inheritdoc />
        [CatchException("通过Id批量查询数据异常")]
        public virtual async Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return await base.QueryAsync<TEntity>(sql, new { Ids = ids });
        }
        /// <inheritdoc />
        [CatchException("获取记录条数异常")]
        public virtual async Task<long> QueryRecordCountAsync(string whereBy = null, object param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await base.ExecuteScalarAsync<long>(sql, param);
        }
        /// <inheritdoc />
        public abstract Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
           string whereBy = null, string orderBy = null, object param = null);

        #endregion Query

        #region Update
        /// <inheritdoc />
        [CatchException("更新数据异常")]
        public virtual async Task<int> UpdateAsync(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return await base.ExecuteAsync(sql, param, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("更新数据异常")]
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return await base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("批量更新数据异常")]
        public virtual async Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return await base.ExecuteAsync(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("通过Id更新数据异常")]
        public virtual async Task<int> UpdateByIdAsync(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return await base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("通过Id更新记录的一列异常")]
        public virtual async Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return await base.ExecuteAsync(sql, new { Id = id, Value = value });
        }

        #endregion Update

        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoAsync(TEntity entity, bool useLock = false);
        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
        /// <inheritdoc />
        public abstract Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);

        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoAsync(string tableName, TEntity entity, bool useLock = false);
        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
        /// <inheritdoc />
        public abstract Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
    }
}