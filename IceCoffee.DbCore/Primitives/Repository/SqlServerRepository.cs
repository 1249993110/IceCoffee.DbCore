using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SqlServer数据库仓储
    /// </summary>
    public class SqlServerRepository<TEntity> : RepositoryBaseStr<TEntity, SqlConnection> where TEntity : EntityBase<string>
    {
        new protected SqlConnection Connection;

        public SqlServerRepository(DbConnectionInfo dbConnectionInfo)
            : this(dbConnectionInfo.ConnectionString, dbConnectionInfo.UseConnectionPool)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLite)
            {
                throw new Exception("数据库类型不匹配");
            }
        }

        public SqlServerRepository(string connectionString, bool useConnectionPool) : base(connectionString, useConnectionPool)
        {
            Connection = base.Connection as SqlConnection;
        }

        public override IEnumerable<TEntity> QueryListPaged(int pageNumber, int rowsPerPage,
            string conditions = null, string orderby = null, object param = null)
        {
            throw new NotImplementedException();
        }
    }
}
