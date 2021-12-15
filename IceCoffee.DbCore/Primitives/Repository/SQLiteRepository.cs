using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SQLite 数据库仓储
    /// </summary>
    public class SQLiteRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4} OFFSET {5} ";

        /// <summary>
        /// 实例化 SQLiteRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public SQLiteRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLite)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }

        #region Sync

        /// <inheritdoc />
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                pageSize,
                (pageIndex - 1) * pageSize);
            return base.Query<TEntity>(sql, param);
        }

        /// <inheritdoc />
        public override int ReplaceInto(TEntity entity)
        {
            return ReplaceInto(TableName, entity);
        }

        /// <inheritdoc />

        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return ReplaceIntoBatch(TableName, entities, useTransaction);
        }

        /// <inheritdoc />

        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return InsertIgnoreBatch(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override int ReplaceInto(string tableName, TEntity entity)
        {
            return base.Execute(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        /// <inheritdoc />
        public override int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        /// <inheritdoc />
        public override int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT OR IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        #endregion Sync

        #region Async

        /// <inheritdoc />
        public override Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                pageSize,
                (pageIndex - 1) * pageSize);
            return base.QueryAsync<TEntity>(sql, param);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(TEntity entity)
        {
            return ReplaceIntoAsync(TableName, entity);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return ReplaceIntoBatchAsync(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return InsertIgnoreBatchAsync(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT OR IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        #endregion Async
    }
}