using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
using System.Reflection;

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

        #region Async

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
                (pageIndex - 1) * pageSize,
                pageSize);
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
                whereBy = $"{keywordMappedColumnNames[0]} LIKE CONCAT('%',@Keyword,'%')";
                for (int i = 1, len = keywordMappedColumnNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedColumnNames[i]} LIKE CONCAT('%',@Keyword,'%')";
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

        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatchByTableName(TableName, entities, useTransaction);
        }

        public override int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format(InsertIgnore_Statement, tableName, KeyNameWhereBy, Insert_Statement),
                entities,
                useTransaction);
        }

        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
                            string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryPagedByTableName(TableName, pageIndex, pageSize, whereBy, orderBy, param);
        }

        public override PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, params string[] keywordMappedColumnNames)
        {
            return this.QueryPagedByTableName(TableName, dto, keywordMappedColumnNames);
        }

        public override IEnumerable<TEntity> QueryPagedByTableName(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            if (pageSize < 0)
            {
                return base.Query(whereBy, orderBy, param);
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

        public override PaginationResultDto<TEntity> QueryPagedByTableName(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames)
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
                whereBy = $"{keywordMappedColumnNames[0]} LIKE CONCAT('%',@Keyword,'%')";
                for (int i = 1, len = keywordMappedColumnNames.Length; i < len; ++i)
                {
                    whereBy += $" OR {keywordMappedColumnNames[i]} LIKE CONCAT('%',@Keyword,'%')";
                }
            }

            IEnumerable<TEntity> items;
            int total = this.QueryRecordCountByTableName(tableName, whereBy, dto);

            if (total == 0)
            {
                items = Enumerable.Empty<TEntity>();
            }
            else
            {
                items = this.QueryPagedByTableName(tableName, dto.PageIndex, dto.PageSize, whereBy, orderBy, dto);
            }

            return new PaginationResultDto<TEntity>() { Items = items, Total = total };
        }

        public override int ReplaceInto(TEntity entity)
        {
            return this.ReplaceIntoByTableName(TableName, entity);
        }

        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatchByTableName(TableName, entities, useTransaction);
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