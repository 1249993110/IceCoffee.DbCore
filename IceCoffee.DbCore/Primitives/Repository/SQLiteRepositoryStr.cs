using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;

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

