using IceCoffee.DbCore.Dtos;

namespace IceCoffee.DbCore.Repositories
{
    public abstract partial class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity>
    {
        public virtual Task<int> DeleteAsync(string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }

        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }

        public virtual Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }

        public virtual Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return base.ExecuteAsync(sql, new { Ids = ids }, useTransaction);
        }

        public virtual Task<int> DeleteBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", tableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }

        public virtual Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return base.ExecuteAsync(sql, new { Id = id });
        }

        public virtual Task<int> DeleteByTableNameAsync(string tableName, string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", tableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }

        public virtual Task<int> DeleteByTableNameAsync(string tableName, TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", tableName, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }

        public virtual Task<int> InsertAsync(TEntity entity)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }

        public virtual Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        public virtual Task<int> InsertBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", tableName, Insert_Statement), entities, useTransaction);
        }

        public virtual Task<int> InsertByTableNameAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format("INSERT INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        public abstract Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        public virtual Task<IEnumerable<TEntity>> QueryAllAsync(string? orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.QueryAsync<TEntity>(sql, null);
        }

        public virtual Task<IEnumerable<TEntity>> QueryAsync(string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.QueryAsync<TEntity>(sql, param);
        }

        public virtual Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return base.QueryAsync<TEntity>(sql, new { Id = id });
        }

        public virtual Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return base.QueryAsync<TEntity>(sql, new { Ids = ids });
        }

        public virtual Task<IEnumerable<TEntity>> QueryByTableNameAsync(string tableName, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, tableName,
               whereBy == null ? string.Empty : "WHERE " + whereBy,
               orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.QueryAsync<TEntity>(sql, param);
        }

        public abstract Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        public abstract Task<IEnumerable<TEntity>> QueryPagedByTableNameAsync(string tableName, int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        public abstract Task<PaginationResultDto<TEntity>> QueryPagedAsync(PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        public abstract Task<PaginationResultDto<TEntity>> QueryPagedByTableNameAsync(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        public virtual Task<int> QueryRecordCountAsync(string? whereBy = null, object? param = null)
        {
            return this.QueryRecordCountByTableNameAsync(TableName, whereBy, param);
        }

        public virtual Task<int> QueryRecordCountByTableNameAsync(string tableName, string? whereBy = null, object? param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", tableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteScalarAsync<int>(sql, param);
        }

        public abstract Task<int> ReplaceIntoAsync(TEntity entity);

        public abstract Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract Task<int> ReplaceIntoBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity);

        public virtual Task<int> UpdateAsync(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }

        public virtual Task<int> UpdateAsync(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }

        public virtual Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }

        public virtual Task<int> UpdateBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entities, useTransaction);
        }

        public virtual Task<int> UpdateByIdAsync(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return base.ExecuteAsync(sql, entity);
        }

        public virtual Task<int> UpdateByTableNameAsync(string tableName, string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", tableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteAsync(sql, param, useTransaction);
        }

        public virtual Task<int> UpdateByTableNameAsync(string tableName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.ExecuteAsync(sql, entity);
        }

        public virtual Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return base.ExecuteAsync(sql, new { Id = id, Value = value });
        }
    }
}