using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.SqliteTypeHandlers;
using System.Reflection;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// SQLite 数据库仓储
    /// </summary>
    public class SqliteRepository<TEntity> : RepositoryBase<TEntity>
    {
        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4} OFFSET {5} ";

        /// <summary>
        /// 实例化 SQLiteRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public SqliteRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLite)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }
        
        protected override string KeywordLikeClause => "LIKE '%'||@Keyword||'%'";

        #region Async

        public override Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT OR IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
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
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        public override Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        #endregion

        #region Sync

        public override int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT OR IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
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
            return base.Execute(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        public override int ReplaceIntoByTableName(string tableName, TEntity entity)
        {
            return base.Execute(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        #endregion
    }
}