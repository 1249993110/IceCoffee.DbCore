using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// MySql数据库仓储
    /// </summary>
    public class MySqlRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IEntity
    {
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
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int ReplaceInto(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int ReplaceInto(string tableName, TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        public override PaginationResultDto QueryPaged(PaginationQueryDto dto, string keywordMappedPropName)
        {
            throw new NotImplementedException();
        }

        #endregion Sync

        #region Async

        /// <inheritdoc />
        public override Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(string tableName, TEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            throw new NotImplementedException();
        }

        public override Task<PaginationResultDto> QueryPagedAsync(PaginationQueryDto dto, string keywordMappedPropName)
        {
            throw new NotImplementedException();
        }

        public override Task<PaginationResultDto> QueryPagedAsync(PaginationQueryDto dto, string[] keywordMappedPropNames)
        {
            throw new NotImplementedException();
        }

        public override PaginationResultDto QueryPaged(PaginationQueryDto dto, string[] keywordMappedPropNames)
        {
            throw new NotImplementedException();
        }

        #endregion Async
    }
}