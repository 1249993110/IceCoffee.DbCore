using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SqlServer数据库仓储
    /// </summary>
    public class SqlServerRepositoryGuid<TEntity> : RepositoryBase<TEntity, Guid> where TEntity : EntityBase<Guid>
    {
        public SqlServerRepositoryGuid(DbConnectionInfo dbConnectionInfo)
        {
            Debug.Assert(dbConnectionInfo.DatabaseType == DatabaseType.SQLServer, "数据库类型不匹配");
        }

        //public override IEnumerable<TEntity> QueryListPaged(int pageNumber, int rowsPerPage,
        //    string conditions = null, string orderby = null, object param = null)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
