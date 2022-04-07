using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Repositories
{
    public abstract partial class RepositoryBase<TEntity> : IRepository<TEntity>
    {
        #region Insert
        /// <inheritdoc />
        public virtual Task<int> InsertAsync(TEntity entity)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }
        /// <inheritdoc />
        public virtual Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        #endregion Insert

        #region Delete
        /// <inheritdoc />
        public virtual Task<int> DeleteAsync(string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }
        /// <inheritdoc />
        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        public virtual Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        public virtual Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return base.ExecuteAsync(sql, new { Id = id });
        }
        /// <inheritdoc />
        public virtual Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return base.ExecuteAsync(sql, new { Ids = ids }, useTransaction);
        }
        #endregion Delete

        #region Query
        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> QueryAsync(string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.QueryAsync<TEntity>(sql, param);
        }
        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> QueryAllAsync(string? orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.QueryAsync<TEntity>(sql, null);
        }
        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return base.QueryAsync<TEntity>(sql, new { Id = id });
        }
        /// <inheritdoc />
        public virtual Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return base.QueryAsync<TEntity>(sql, new { Ids = ids });
        }
        /// <inheritdoc />
        public virtual Task<uint> QueryRecordCountAsync(string? whereBy = null, object? param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteScalarAsync<uint>(sql, param);
        }
        /// <inheritdoc />
        public abstract Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        /// <inheritdoc />
        public abstract Task<PaginationResultDto> QueryPagedAsync(PaginationQueryDto dto, string keywordMappedPropName);
        /// <inheritdoc />
        public abstract Task<PaginationResultDto> QueryPagedAsync(PaginationQueryDto dto, string[] keywordMappedPropNames);
        #endregion Query

        #region Update
        /// <inheritdoc />
        public virtual Task<int> UpdateAsync(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }
        /// <inheritdoc />
        public virtual Task<int> UpdateAsync(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        public virtual Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        public virtual Task<int> UpdateByIdAsync(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return base.ExecuteAsync(sql, entity);
        }
        /// <inheritdoc />
        public virtual Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return base.ExecuteAsync(sql, new { Id = id, Value = value });
        }

        #endregion Update

        #region Other
        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoAsync(TEntity entity);
        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);
        /// <inheritdoc />
        public abstract Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoAsync(string tableName, TEntity entity);
        /// <inheritdoc />
        public abstract Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);
        /// <inheritdoc />
        public abstract Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);
        #endregion
    }
}