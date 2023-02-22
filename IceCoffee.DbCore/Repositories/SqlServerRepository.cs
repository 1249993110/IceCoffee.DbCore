using IceCoffee.DbCore.ExceptionCatch;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// SqlServer 数据库仓储, 仅支持 Sql Server 2012 及以上版本
    /// </summary>
    public class SqlServerRepository<TEntity> : RepositoryBase<TEntity>
    {
        /// <summary>
        /// 插入或忽略 SQL 语句
        /// </summary>
        public const string InsertIgnore_Statement = "IF NOT EXISTS(SELECT 1 FROM {0} WHERE {1}) BEGIN INSERT INTO {0} {2} END";

        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY";

        /// <summary>
        /// 插入或更新 SQL 语句
        /// </summary>
        public const string ReplaceInto_Statement = "UPDATE {0} SET {1} WHERE {2} IF @@ROWCOUNT=0 BEGIN INSERT INTO {0} {3} END";

        /// <summary>
        /// 实例化 SqlServerRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public SqlServerRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLServer)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }

        protected override string KeywordLikeClause => "LIKE CONCAT('%',@Keyword,'%')";

        #region Async

        public override Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
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
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.QueryAsync<TEntity>(sql, param);
        }

        public override Task<int> ReplaceIntoBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, UpdateSet_Statement, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, UpdateSet_Statement, KeyNameWhereBy, Insert_Statement),
                entity);
        }

        #endregion Async

        #region Sync

        public override int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
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
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.Query<TEntity>(sql, param);
        }

        public override int ReplaceIntoBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName, UpdateSet_Statement, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override int ReplaceIntoByTableName(string tableName, TEntity entity)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName, UpdateSet_Statement, KeyNameWhereBy, Insert_Statement),
                entity);
        }

        #endregion Sync
    }
}