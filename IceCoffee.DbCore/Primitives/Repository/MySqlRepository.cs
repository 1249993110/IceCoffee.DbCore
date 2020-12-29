﻿using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// MySql数据库仓储
    /// </summary>
    public class MySqlRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase
    {
        public MySqlRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.MySQL)
            {
                throw new DbCoreException("数据库类型不匹配");
            }
        }

        #region Sync

        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入或更新一条记录异常")]
        public override int ReplaceInto(TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入或更新多条记录异常")]
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入多条记录异常")]
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Async

        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override async Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入或更新一条记录异常")]
        public override async Task<int> ReplaceIntoAsync(TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入或更新多条记录异常")]
        public override async Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        [CatchException("插入多条记录异常")]
        public override async Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}