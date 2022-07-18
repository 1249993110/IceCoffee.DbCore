﻿using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
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

        public override Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatchByTableNameAsync(TableName, entities, useTransaction);
        }

        public override Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryPagedByTableNameAsync(TableName, pageIndex, pageSize, whereBy, orderBy, param);
        }

        
        public override Task<PaginationResultDto<TEntity>> QueryPagedAsync(PaginationQueryDto dto, params string[] keywordMappedColumnNames)
        {
            return this.QueryPagedByTableNameAsync(TableName, dto, keywordMappedColumnNames);
        }


        public override Task<IEnumerable<TEntity>> QueryPagedByTableNameAsync(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if (pageSize < 0)
            {
                return base.QueryAsync(whereBy, orderBy, param);
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

        public override async Task<PaginationResultDto<TEntity>> QueryPagedByTableNameAsync(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames)
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
                whereBy = $"{keywordMappedColumnNames[0]} ILIKE CONCAT('%',@Keyword,'%')";
                for (int i = 1, len = keywordMappedColumnNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedColumnNames[i]} ILIKE CONCAT('%',@Keyword,'%')";
                }
            }

            IEnumerable<TEntity> items;
            int total = await this.QueryRecordCountByTableNameAsync(tableName, whereBy, dto);

            if (total == 0)
            {
                items = Enumerable.Empty<TEntity>();
            }
            else
            {
                items = await this.QueryPagedByTableNameAsync(tableName, dto.PageIndex, dto.PageSize, whereBy, orderBy, dto);
            }

            return new PaginationResultDto<TEntity>() { Items = items, Total = total };
        }

        public override Task<int> ReplaceIntoAsync(TEntity entity)
        {
            return this.ReplaceIntoByTableNameAsync(TableName, entity);
        }

        
        public override Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatchByTableNameAsync(TableName, entities, useTransaction);
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
    }
}