using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SqlServer数据库仓储
    /// </summary>
    public class SqlServerRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} OFFSET {4} ROWS FETCH NEXT {5} ROWS ONLY";

        /// <summary>
        /// 插入或更新 SQL 语句
        /// </summary>
        public const string ReplaceInto_Statement = "IF EXISTS(SELECT 1 FROM {0} WHERE {1}) BEGIN UPDATE {0} SET {2} WHERE {1} END ELSE BEGIN INSERT INTO {0} {3} END";

        /// <summary>
        /// 插入或忽略 SQL 语句
        /// </summary>
        public const string InsertIgnore_Statement = "IF NOT EXISTS(SELECT 1 FROM {0} WHERE {1}) BEGIN INSERT INTO {0} {2} END";

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

        #region Sync

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// 此实现仅在 Sql Server 2012 及以上版本可用
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereBy">不能为空字符串""</param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.Query<TEntity>(sql, param);
        }

        /// <inheritdoc />
        public override int ReplaceInto(TEntity entity)
        {
            return this.ReplaceInto(TableName, entity);
        }

        /// <inheritdoc />
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatch(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatch(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override int ReplaceInto(string tableName, TEntity entity)
        {
            return base.Execute(
                string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entity);
        }

        /// <inheritdoc />
        public override int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(
                string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement), 
                entities, 
                useTransaction);
        }

        /// <inheritdoc />
        public override int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(
                string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        #endregion Sync

        #region Async

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// 此实现仅在 Sql Server 2012 及以上版本可用
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereBy">不能为空字符串""</param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format(
                QueryPaged_Statement,
                Select_Statement,
                TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? ((KeyNames == null || KeyNames.Length == 0) ? "1" : string.Join(",", KeyNames)),
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.QueryAsync<TEntity>(sql, param);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(TEntity entity)
        {
            return this.ReplaceIntoAsync(TableName, entity);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatchAsync(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatchAsync(TableName, entities, useTransaction);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entity);
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName, KeyNameWhereBy, UpdateSet_Statement, Insert_Statement),
                entities, 
                useTransaction);
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement), 
                entities, 
                useTransaction);
        }

        #endregion Async
    }
}