using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
using System.Reflection;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// SQLite 数据库仓储
    /// </summary>
    public class SQLiteRepository<TEntity> : RepositoryBase<TEntity>
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

        public override Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return InsertIgnoreBatchByTableNameAsync(TableName, entities, useTransaction);
        }

        public override Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("INSERT OR IGNORE INTO {0} {1}", tableName, Insert_Statement), entities);
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
                whereBy = $"{keywordMappedColumnNames[0]} LIKE '%'||@Keyword||'%'";
                for (int i = 1, len = keywordMappedColumnNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedColumnNames[i]} LIKE '%'||@Keyword||'%'";
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
            return ReplaceIntoByTableNameAsync(TableName, entity);
        }

        public override Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return ReplaceIntoBatchByTableNameAsync(TableName, entities, useTransaction);
        }

        public override Task<int> ReplaceIntoBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entities);
        }

        public override Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity)
        {
            return base.ExecuteAsync(string.Format("REPLACE INTO {0} {1}", tableName, Insert_Statement), entity);
        }
    }
}