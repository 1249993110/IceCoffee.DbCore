using IceCoffee.DbCore.Domain;
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
            Debug.Assert(dbConnectionInfo.DatabaseType == DatabaseType.SQLite, "数据库类型不匹配");
        }

        protected override IEnumerable<AnyEntity> QueryPaged<AnyEntity>(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null, string tableName = null)
        {
            throw new NotImplementedException();
        }
    }
}