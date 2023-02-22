using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
using System.Collections;
using System.Reflection;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// PostgreSQL 数据库仓储, 仅支持 PostgreSQL 9.5 +
    /// </summary>
    public class PostgreSqlRepository<TEntity> : RepositoryBase<TEntity>
    {
        /// <summary>
        /// 插入或忽略 SQL 语句
        /// </summary>
        public const string InsertIgnore_Statement = "INSERT INTO {0} {2} ON CONFLICT WHERE {1} DO NOTHING";

        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4} OFFSET {5} ";

        /// <summary>
        /// 插入或更新 SQL 语句
        /// </summary>
        public const string ReplaceInto_Statement = "INSERT INTO {0} {3} ON CONFLICT WHERE {1} DO UPDATE {0} SET {2}";

        /// <summary>
        /// 实例化 PostgreSqlRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public PostgreSqlRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.PostgreSQL)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }

        protected override string KeywordLikeClause => "ILIKE CONCAT('%',@Keyword,'%')";

        #region Async

        public override Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            if (ids is not IList)
            {
                ids = ids.ToArray();
            }
            string sql = string.Format("DELETE FROM {0} WHERE {1}=ANY(@Ids)", TableName, idColumnName);
            return base.ExecuteAsync(sql, new { Ids = ids }, useTransaction);
        }

        public override Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            if (ids is not IList)
            {
                ids = ids.ToArray();
            }
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=ANY(@Ids)", Select_Statement, TableName, idColumnName);
            return base.QueryAsync<TEntity>(sql, new { Ids = ids });
        }

        public override Task<IEnumerable<TEntity>> QueryPagedByTableNameAsync(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if (pageSize < 0)
            {
                return base.QueryByTableNameAsync(tableName, whereBy, orderBy, param);
            }

            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                tableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                pageSize,
                (pageIndex - 1) * pageSize);
            return base.QueryAsync<TEntity>(sql, param);
        }

        public override Task<int> ReplaceIntoBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entities,
                useTransaction);
        }

        public override Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entity);
        }

        #endregion

        #region Sync

        public override int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            if (ids is not IList)
            {
                ids = ids.ToArray();
            }
            string sql = string.Format("DELETE FROM {0} WHERE {1}=ANY(@Ids)", TableName, idColumnName);
            return base.Execute(sql, new { Ids = ids }, useTransaction);
        }

        public override int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            if (ids is not IList)
            {
                ids = ids.ToArray();
            }
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=ANY(@Ids)", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Ids = ids });
        }

        public override IEnumerable<TEntity> QueryPagedByTableName(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if (pageSize < 0)
            {
                return base.QueryByTableName(tableName, whereBy, orderBy, param);
            }

            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                tableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                pageSize,
                (pageIndex - 1) * pageSize);
            return base.Query<TEntity>(sql, param);
        }

        public override int ReplaceIntoBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entities,
                useTransaction);
        }

        public override int ReplaceIntoByTableName(string tableName, TEntity entity)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entity);
        }

        #endregion
    }
}