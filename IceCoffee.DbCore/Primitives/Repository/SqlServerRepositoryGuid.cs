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
    public class SqlServerRepositoryGuid<TEntity> : RepositoryBaseGuid<TEntity, SqlConnection> where TEntity : EntityBase<Guid>
    {
        new protected SqlConnection Connection;

        public SqlServerRepositoryGuid(DbConnectionInfo dbConnectionInfo)
            : this(dbConnectionInfo.ConnectionString, dbConnectionInfo.UseConnectionPool)
        {
            if (dbConnectionInfo.DatabaseType != DatabaseType.SQLServer)
            {
                throw new Exception("数据库类型不匹配");
            }
        }

        public SqlServerRepositoryGuid(string connectionString, bool useConnectionPool) : base(connectionString, useConnectionPool)
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
