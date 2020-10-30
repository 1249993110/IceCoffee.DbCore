using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SQLite数据库仓储
    /// </summary>
    public class SQLiteRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase
    {
        public SQLiteRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLite)
            {
                throw new DbException("数据库类型不匹配");
            }
        }

        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            throw new NotImplementedException();
        }

        public override int ReplaceInto(TEntity entity, bool useLock = false)
        {
            throw new NotImplementedException();
        }

        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            throw new NotImplementedException();
        }
    }
}