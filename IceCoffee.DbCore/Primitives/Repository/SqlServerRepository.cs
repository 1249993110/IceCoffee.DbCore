using Dapper;

using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public const string ReplaceInto_Statement = "IF EXISTS(SELECT 1 FROM {0} {1} WHERE {2}) BEGIN UPDATE {0} SET {3} WHERE {2} END ELSE BEGIN INSERT INTO {0} {4} END";
        /// <summary>
        /// 插入或忽略 SQL 语句
        /// </summary>
        public const string InsertIgnore_Statement = "IF NOT EXISTS(SELECT 1 FROM {0} {1} WHERE {2}) BEGIN INSERT INTO {0} {3} END";
        /// <summary>
        /// 使用锁 SQL 语句
        /// </summary>
        public const string UseLock_Statement = "WITH (UPDLOCK,SERIALIZABLE)";

        /// <summary>
        /// 实例化 SqlServerRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public SqlServerRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if(dbConnectionInfo.DatabaseType != DatabaseType.SQLServer)
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
        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
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
        public override int ReplaceInto(TEntity entity, bool useLock = false)
        {
            return this.ReplaceInto(TableName, entity, useLock);
        }
        /// <inheritdoc />
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return this.ReplaceIntoBatch(TableName, entities, useTransaction, useLock);
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return this.InsertIgnoreBatch(TableName, entities, useTransaction, useLock);
        }

        /// <inheritdoc />
        [CatchException("插入或更新一条记录异常")]
        public override int ReplaceInto(string tableName, TEntity entity, bool useLock = false)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement), entity);
        }
        /// <inheritdoc />
        [CatchException("插入或更新多条记录异常")]
        public override int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return base.Execute(string.Format(ReplaceInto_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement), entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return base.Execute(string.Format(InsertIgnore_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, Insert_Statement), entities, useTransaction);
        }
        #endregion

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
        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override async Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
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
            return await base.QueryAsync<TEntity>(sql, param);
        }
        /// <inheritdoc />
        [CatchException("插入或更新一条记录异常")]
        public override async Task<int> ReplaceIntoAsync(TEntity entity, bool useLock = false)
        {
            return await this.ReplaceIntoAsync(TableName, entity, useLock);
        }
        /// <inheritdoc />
        [CatchException("插入或更新多条记录异常")]
        public override async Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return await this.ReplaceIntoBatchAsync(TableName, entities, useTransaction, useLock);
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override async Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return await this.InsertIgnoreBatchAsync(TableName, entities, useTransaction, useLock);
        }

        /// <inheritdoc />
        [CatchException("插入或更新一条记录异常")]
        public override async Task<int> ReplaceIntoAsync(string tableName, TEntity entity, bool useLock = false)
        {
            return await base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement), entity);
        }
        /// <inheritdoc />
        [CatchException("插入或更新多条记录异常")]
        public override async Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return await base.ExecuteAsync(string.Format(ReplaceInto_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement), entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override async Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return await base.ExecuteAsync(string.Format(InsertIgnore_Statement, tableName,
                useLock ? UseLock_Statement : string.Empty,
                KeyNameWhereBy, Insert_Statement), entities, useTransaction);
        }
        #endregion
    }
}