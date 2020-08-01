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
    public class SQLiteRepositoryStr<TEntity> : RepositoryBase<TEntity, string> where TEntity : EntityBase<string>
    {
        public SQLiteRepositoryStr(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            Debug.Assert(dbConnectionInfo.DatabaseType == DatabaseType.SQLite, "数据库类型不匹配");
        }

        public override IEnumerable<TEntity> QueryPaged(int pageNumber, int rowsPerPage,
            string whereBy = null, string orderby = null, object param = null)
        {
            throw new NotImplementedException();
        }
    }
}