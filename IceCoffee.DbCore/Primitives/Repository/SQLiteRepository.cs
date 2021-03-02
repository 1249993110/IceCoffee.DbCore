
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SQLite 数据库仓储
    /// </summary>
    public class SQLiteRepository<TEntity> : RepositoryBase<TEntity> where TEntity : IEntityBase
    {
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
        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入或更新一条记录异常")]
        public override int ReplaceInto(TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入或更新多条记录异常")]
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override int ReplaceInto(string tableName, TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Async
        /// <inheritdoc />
        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override async Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入或更新一条记录异常")]
        public override async Task<int> ReplaceIntoAsync(TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入或更新多条记录异常")]
        public override async Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        [CatchException("插入多条记录异常")]
        public override async Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override Task<int> ReplaceIntoAsync(string tableName, TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override Task<int> ReplaceIntoBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        /// <inheritdoc />
        public override Task<int> InsertIgnoreBatchAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        
        #endregion
    }
}