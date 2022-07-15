﻿using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Dtos;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// PostgreSQL 数据库仓储, 仅支持 PostgreSQL 9.5 +
    /// </summary>
    public class PostgreSqlRepository<TEntity> : RepositoryBase<TEntity>
    {
        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4} OFFSET {5} ";

        /// <summary>
        /// 插入或更新 SQL 语句
        /// </summary>
        public const string ReplaceInto_Statement = "INSERT INTO {0} {3} ON CONFLICT WHERE {1} DO UPDATE {0} SET {2}";

        /// <summary>
        /// 插入或忽略 SQL 语句
        /// </summary>
        public const string InsertIgnore_Statement = "INSERT INTO {0} {2} ON CONFLICT WHERE {1} DO NOTHING";

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
            string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if(pageSize < 0)
            {
                return base.Query(whereBy, orderBy, param);
            }

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
        public override PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, string keywordMappedPropName)
        {
            return QueryPagedAsync(dto, keywordMappedPropName).Result;
        }

        /// <inheritdoc />
        public override PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, string[] keywordMappedPropNames)
        {
            return QueryPagedAsync(dto, keywordMappedPropNames).Result;
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
            string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if (pageSize < 0)
            {
                return base.QueryAsync(whereBy, orderBy, param);
            }

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
        public override async Task<PaginationResultDto<TEntity>> QueryPagedAsync(PaginationQueryDto dto, string keywordMappedPropName)
        {
            string? orderBy = null;

            if (string.IsNullOrEmpty(dto.Order) == false)
            {
                // 避免sql注入
                if (typeof(TEntity).GetProperty(dto.Order, BindingFlags.Instance | BindingFlags.Public) != null)
                {
                    orderBy = dto.Order + (dto.Desc ? " DESC" : " ASC");
                }
            }

            string? whereBy = null;
            if (string.IsNullOrEmpty(dto.Keyword) == false)
            {
                whereBy = $"{keywordMappedPropName} ILIKE CONCAT('%',@Keyword,'%')";
            }

            IEnumerable<TEntity> items;
            int total = await this.QueryRecordCountAsync(whereBy, dto);

            if (total == 0)
            {
                items = Enumerable.Empty<TEntity>();
            }
            else
            {
                items = await this.QueryPagedAsync(dto.PageIndex, dto.PageSize, whereBy, orderBy, dto);
            }

            return new PaginationResultDto<TEntity>() { Items = items, Total = total };
        }

        /// <inheritdoc />
        public override async Task<PaginationResultDto<TEntity>> QueryPagedAsync(PaginationQueryDto dto, string[] keywordMappedPropNames)
        {
            string? orderBy = null;

            if (string.IsNullOrEmpty(dto.Order) == false)
            {
                // 避免sql注入
                if (typeof(TEntity).GetProperty(dto.Order, BindingFlags.Instance | BindingFlags.Public) != null)
                {
                    orderBy = dto.Order + (dto.Desc ? " DESC" : " ASC");
                }
            }

            string? whereBy = null;
            if (string.IsNullOrEmpty(dto.Keyword) == false)
            {
                whereBy = $"{keywordMappedPropNames[0]} ILIKE CONCAT('%',@Keyword,'%')";
                for (int i = 1, len = keywordMappedPropNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedPropNames[i]} ILIKE CONCAT('%',@Keyword,'%')";
                }
            }

            IEnumerable<TEntity> items;
            int total = await this.QueryRecordCountAsync(whereBy, dto);

            if (total == 0)
            {
                items = Enumerable.Empty<TEntity>();
            }
            else
            {
                items = await this.QueryPagedAsync(dto.PageIndex, dto.PageSize, whereBy, orderBy, dto);
            }

            return new PaginationResultDto<TEntity>() { Items = items, Total = total };
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