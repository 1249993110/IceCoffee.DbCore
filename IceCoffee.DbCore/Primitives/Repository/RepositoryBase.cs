using Dapper;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.UnitWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public class RepositoryBase
    {
        private static ThreadLocal<IUnitOfWork> _unitOfWork;

        static RepositoryBase()
        {
            _unitOfWork = new ThreadLocal<IUnitOfWork>(() =>
            {
                return new UnitOfWork();
            });
        }

        /// <summary>
        /// 覆盖默认工作单元
        /// </summary>
        /// <param name="func"></param>
        public static void OverrideUnitOfWork(Func<IUnitOfWork> func)
        {
            _unitOfWork = new ThreadLocal<IUnitOfWork>(func);
        }

        protected internal readonly DbConnectionInfo dbConnectionInfo;

        public RepositoryBase(DbConnectionInfo dbConnectionInfo)
        {
            this.dbConnectionInfo = dbConnectionInfo;
        }

        internal static IUnitOfWork UnitOfWork => _unitOfWork.Value;

        protected virtual int Execute(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

                int result = conn.Execute(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran.Commit();
                }

                return result;
            }
            catch
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran.Rollback();
                }
                throw;
            }
            finally
            {
                if (conn != null && unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        protected virtual TReturn ExecuteScalar<TReturn>(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);

                TReturn result = conn.ExecuteScalar<TReturn>(sql, param, commandType: CommandType.Text);

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null && unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        protected virtual IEnumerable<AnyEntity> QueryAny<AnyEntity>(string sql, object param = null)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                tran = unitOfWork.DbTransaction;
                return conn.Query<AnyEntity>(sql, param, tran, commandType: CommandType.Text);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null && unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        protected virtual IEnumerable<TReturn> ExecProcedure<TReturn>(string procName, DynamicParameters parameters)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                tran = unitOfWork.DbTransaction;

                return conn.Query<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
                    transaction: tran, commandType: CommandType.StoredProcedure));
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn != null && unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

    }

    /// <summary>
    /// 通用仓储基类
    /// <para>ExecuteAsync、QueryAsync实际上调用DbCommand.ExecuteNonQueryAsync 方法，是同步执行</para>
    /// <para>https://docs.microsoft.com/zh-cn/dotnet/api/system.data.common.dbcommand.executenonqueryasync</para>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract partial class RepositoryBase<TEntity> : RepositoryBase, IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        #region 公共静态属性

        /// <summary>
        /// 主键列名
        /// </summary>
        public static string[] KeyNames { get; private set; }

        /// <summary>
        /// 默认实体删除更新Where约束
        /// </summary>
        public static string KeyNameWhereBy { get; private set; }

        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName { get; private set; }

        /// <summary>
        /// 基于实体上列名的插入语句_固定的
        /// </summary>
        public static string Insert_Statement_Fixed { get; private set; }

        /// <summary>
        /// 基于实体上列名的选择语句
        /// </summary>
        public static string Select_Statement { get; private set; }

        /// <summary>
        /// 基于实体上列名的更新语句
        /// </summary>Select
        public static string UpdateSet_Statement { get; private set; }

        #endregion 公共静态属性

        #region 构造

        static RepositoryBase()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>(true) == null).ToArray();
            
            IEnumerable<PropertyInfo> keyPropInfos = properties.Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>(true) != null);

            StringBuilder keyNameWhereByBuilder = new StringBuilder();
            List<string> _keyNames = new List<string>();
            foreach (var item in keyPropInfos)
            {
                string _keyName = item.GetCustomAttribute<ColumnAttribute>(true)?.Name;
                if(_keyName == null)
                {
                    _keyName = item.Name;
                }
                _keyNames.Add(_keyName);
                //"{1}=@{1} AND {2}=@{2}"
                keyNameWhereByBuilder.AppendFormat("{0}=@{0} AND ", _keyName);
            }
            keyNameWhereByBuilder.Remove(keyNameWhereByBuilder.Length - 5, 5);

            KeyNames = _keyNames.ToArray();
            KeyNameWhereBy = keyNameWhereByBuilder.ToString();

            TableName = typeof(TEntity).GetCustomAttribute<TableAttribute>(true)?.Name;
            TableName = TableName ?? typeof(TEntity).Name;
            
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            StringBuilder stringBuilder4 = new StringBuilder();

            foreach (PropertyInfo prop in properties)
            {
                string propertyName = prop.Name;
                string columnName;

                ColumnAttribute columnAttribute = prop.GetCustomAttribute<ColumnAttribute>(true);
                columnName = columnAttribute != null ? columnAttribute.Name : prop.Name;

                // 过滤定义了IgnoreInsert特性的属性
                if (prop.GetCustomAttribute<IgnoreInsertAttribute>(true) == null)
                {
                    stringBuilder1.AppendFormat("{0},", columnName);
                    stringBuilder2.AppendFormat("@{0},", propertyName);
                }
                // 过滤定义了IgnoreSelect特性的属性
                if (prop.GetCustomAttribute<IgnoreSelectAttribute>(true) == null)
                {
                    stringBuilder3.AppendFormat("{0},", columnName);
                }
                // 过滤定义了IgnoreUpdate特性的属性
                if (prop.GetCustomAttribute<IgnoreUpdateAttribute>(true) == null)
                {
                    stringBuilder4.AppendFormat("{0}=@{1},", columnName, propertyName);
                }
            }

            Debug.Assert(stringBuilder1.Length > 0
                && stringBuilder2.Length > 0
                && stringBuilder3.Length > 0
                && stringBuilder4.Length > 0, "实体应存在CURD属性");

            Insert_Statement_Fixed = string.Format("INSERT INTO {0} ({1}) VALUES({2})", TableName,
                stringBuilder1.Remove(stringBuilder1.Length - 1, 1).ToString(),
                stringBuilder2.Remove(stringBuilder2.Length - 1, 1).ToString());
            Select_Statement = stringBuilder3.Remove(stringBuilder3.Length - 1, 1).ToString();
            UpdateSet_Statement = stringBuilder4.Remove(stringBuilder4.Length - 1, 1).ToString();

            var propertyMap = new CustomPropertyTypeMap(typeof(TEntity),
                (type, columnName) =>
                {
                    // 过滤定义了Column特性的属性
                    var result = properties.FirstOrDefault(prop => prop
                        .GetCustomAttributes(false)
                        .OfType<ColumnAttribute>()
                        .Any(attr => attr.Name == columnName));
                    if (result != null)
                    {
                        return result;
                    }
                    // Column特性为空则返回默认对应列名的属性
                    return properties.FirstOrDefault(prop => prop.Name == columnName);
                }
            );

            SqlMapper.SetTypeMap(typeof(TEntity), propertyMap);
        }

        public RepositoryBase(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        #endregion 构造

        new public virtual IUnitOfWork UnitOfWork => RepositoryBase.UnitOfWork;

        protected virtual IEnumerable<TEntity> Query(string sql, object param = null)
        {
            return QueryAny<TEntity>(sql, param);
        }

        #region Insert
        [CatchException("插入数据异常")]
        public virtual int Insert(TEntity entity)
        {
            return Execute(Insert_Statement_Fixed, entity);
        }
        [CatchException("批量插入数据异常")]
        public virtual int InsertBatch(IEnumerable<TEntity> entities)
        {
            return Execute(Insert_Statement_Fixed, entities, true);
        }

        #endregion Insert

        #region Delete
        [CatchException("删除任意数据异常")]
        public virtual int DeleteAny(string whereBy, object param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return Execute(sql, param, useTransaction);
        }
        [CatchException("删除数据异常")]
        public virtual int Delete(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return Execute(sql, entity);
        }
        [CatchException("批量删除数据异常")]
        public virtual int DeleteBatch(IEnumerable<TEntity> entities)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return Execute(sql, entities, true);
        }
        [CatchException("通过Id删除数据异常")]
        public virtual int DeleteById<TId>(TId id, string idColumnName)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return Execute(sql, new { Id = id });
        }
        [CatchException("通过Id批量删除数据异常")]
        public virtual int DeleteBatchByIds<TId>(IEnumerable<TId> ids, string idColumnName)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            List<object> param = new List<object>();
            foreach (var id in ids)
            {
                param.Add(new { Id = id });
            }
            return Execute(sql, param, true);
        }
        [CatchException("删除所有数据异常")]
        public virtual int DeleteAll()
        {
            string sql = string.Format("DELETE FROM {0}", TableName);
            return Execute(sql);
        }

        #endregion Delete

        #region Query
        [CatchException("查询任意数据异常")]
        public virtual IEnumerable<TEntity> QueryAny(string columnNames, string whereBy, string orderby, object param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", columnNames, TableName, whereBy == null ? string.Empty : "WHERE " + whereBy, orderby);
            return Query(sql, param);
        }
        [CatchException("查询所有数据异常")]
        public virtual IEnumerable<TEntity> QueryAll(string orderby = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName, orderby == null ? string.Empty : "ORDER BY " + orderby);
            return Query(sql, null);
        }
        [CatchException("通过Id查询数据异常")]
        public virtual IEnumerable<TEntity> QueryById<TId>(TId id, string idColumnName)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return Query(sql, new { Id = id });
        }
        [CatchException("通过Id批量查询数据异常")]
        public virtual IEnumerable<TEntity> QueryByIds<TId>(IEnumerable<TId> ids, string idColumnName)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            List<object> param = new List<object>();
            foreach (var id in ids)
            {
                param.Add(new { Id = id });
            }
            return Query(sql, param);
        }

        [CatchException("获取记录条数异常")]
        public virtual long QueryRecordCount(string whereBy = null, object param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return ExecuteScalar<long>(sql, param);
        }

        #region 待实现

        public abstract IEnumerable<TEntity> QueryPaged(int pageNumber, int rowsPerPage,
           string whereBy = null, string orderby = null, object param = null);

        #endregion 待实现

        #endregion Query

        #region Update
        [CatchException("更新任意数据异常")]
        public virtual int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return Execute(sql, param, useTransaction);
        }
        [CatchException("更新数据异常")]
        public virtual int Update(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return Execute(sql, entity);
        }
        [CatchException("批量更新意数据异常")]
        public virtual int UpdateBatch(IEnumerable<TEntity> entities)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return Execute(sql, entities, true);
        }
        [CatchException("通过Id更新数据异常")]
        public virtual int UpdateById<TId>(TEntity entity, TId id, string idColumnName)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return Execute(sql, entity);
        }
        [CatchException("通过Id更新记录的一列异常")]
        public virtual int UpdateColumnById<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return Execute(sql, new { Id = id, Value = value });
        }

        #endregion Update
    }
}