using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SqlServer数据库仓储
    /// </summary>
    public class SqlServerRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase
    {
        public SqlServerRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            Debug.Assert(dbConnectionInfo.DatabaseType == DatabaseType.SQLServer, "数据库类型不匹配");
        }

        protected override IEnumerable<AnyEntity> QueryPaged<AnyEntity>(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null, string tableName = null)
        {
            string sql = string.Format("SELECT * FROM {0} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
                tableName ?? typeof(AnyEntity).Name,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? "1",
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.QueryAny<AnyEntity>(sql, param);
        }
    }
}