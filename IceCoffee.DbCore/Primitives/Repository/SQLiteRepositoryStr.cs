using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
    public class SQLiteRepositoryStr<TEntity> : RepositoryBaseStr<TEntity, SQLiteConnection> where TEntity : EntityBase<string>
    {
        new protected SQLiteConnection Connection;

        public SQLiteRepositoryStr(DbConnectionInfo dbConnectionInfo)
            : this(dbConnectionInfo.ConnectionString, dbConnectionInfo.UseConnectionPool)
        {
            if(dbConnectionInfo.DatabaseType != DatabaseType.SQLite)
            {
                throw new Exception("数据库类型不匹配");
            }
        }

        public SQLiteRepositoryStr(string connectionString, bool useConnectionPool) : base(connectionString, useConnectionPool)
        {
            Connection = base.Connection as SQLiteConnection;
        }

        public override IEnumerable<TEntity> QueryListPaged(int pageNumber, int rowsPerPage,
            string conditions = null, string orderby = null, object param = null)
        {
            throw new NotImplementedException();
        }
    }
}

