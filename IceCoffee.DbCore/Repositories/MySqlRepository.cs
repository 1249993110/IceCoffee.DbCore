using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// MySql数据库仓储
    /// </summary>
    public class MySqlRepository<TEntity> : RepositoryBase<TEntity>
    {
        /// <summary>
        /// 分页查询 SQL 语句
        /// </summary>
        public const string QueryPaged_Statement = "SELECT {0} FROM {1} {2} ORDER BY {3} LIMIT {4} OFFSET {5} ";

        /// <summary>
        /// 实例化 MySqlRepository
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public MySqlRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.MySQL)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }

        #region Sync

        /// <inheritdoc />
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string? whereBy = null, string? orderBy = null, object? param = null)
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
            string? whereBy = null, string? orderBy = null, object? param = null)
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
                whereBy = $"{keywordMappedPropName} LIKE CONCAT('%',@Keyword,'%')";
            }

            IEnumerable<TEntity> items;
            int total = await this.QueryRecordCountAsync(whereBy, dto);

            if(total == 0)
            {
                items = Enumerable.Empty<TEntity>();
            }
            else
            {
                items = await this.QueryPagedAsync(dto.PageIndex, dto.PageSize, whereBy, orderBy, dto);
            }

            return new PaginationResultDto<TEntity>() { Items = items, Total = total };
        }

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
                whereBy = $"{keywordMappedPropNames[0]} LIKE CONCAT('%',@Keyword,'%')";
                for (int i = 1, len = keywordMappedPropNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedPropNames[i]} LIKE CONCAT('%',@Keyword,'%')";
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
            return base.ExecuteAsync(string.Format("INSERT IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        #endregion Async
    }
}