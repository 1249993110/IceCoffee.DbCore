using Dapper;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// SqlServer数据库仓储
    /// </summary>
    public class SqlServerRepository<TEntity> : RepositoryBase<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// 插入或更新sql语句
        /// </summary>
        public const string ReplaceInto_Statement = 
@"IF EXISTS(SELECT 1 FROM {0} {1} WHERE {2})
BEGIN
    UPDATE {0} SET {3} WHERE {2}
End
ELSE
BEGIN
    {4}
END";
        /// <summary>
        /// 插入或忽略sql语句
        /// </summary>
        public const string InsertIgnore_Statement =
@"IF NO EXISTS(SELECT 1 FROM {0} {1} WHERE {2})
BEGIN
{3}
END";

        public SqlServerRepository(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
            if(dbConnectionInfo.DatabaseType != DatabaseType.SQLServer)
            {
                throw new DbException("数据库类型不匹配");
            }
        }

        [CatchException("获取与条件匹配的所有记录的分页列表异常")]
        public override IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
            string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format("SELECT * FROM {0} {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
                TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy ?? (KeyNames.Length == 0 ? "1" : string.Join(",", KeyNames)),
                (pageIndex - 1) * pageSize,
                pageSize);
            return base.QueryAny<TEntity>(sql, param);
        }

        [CatchException("插入或更新一条记录异常")]
        public override int ReplaceInto(TEntity entity, bool useLock = false)
        {
            return Execute(string.Format(ReplaceInto_Statement, TableName, 
                useLock ? "WITH (UPDLOCK,SERIALIZABLE)" : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement_Fixed), entity);
        }

        [CatchException("插入或更新多条记录异常")]
        public override int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return Execute(string.Format(ReplaceInto_Statement, TableName,
                useLock ? "WITH (UPDLOCK,SERIALIZABLE)" : string.Empty,
                KeyNameWhereBy, UpdateSet_Statement, Insert_Statement_Fixed), entities, useTransaction);
        }

        [CatchException("插入多条记录异常")]
        public override int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false)
        {
            return Execute(string.Format(InsertIgnore_Statement, TableName,
                useLock ? "WITH (UPDLOCK,SERIALIZABLE)" : string.Empty, 
                KeyNameWhereBy, Insert_Statement_Fixed), entities, useTransaction);
        }
    }
}