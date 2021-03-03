using Dapper;
using IceCoffee.DbCore.ExceptionCatch;

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
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// RepositoryBase
    /// </summary>
    public abstract class RepositoryBase : IRepositoryBase
    {
        private static ThreadLocal<IUnitOfWork> _unitOfWork;

        static RepositoryBase()
        {
            _unitOfWork = new ThreadLocal<IUnitOfWork>(() =>
            {
                return new UnitOfWork();
            });
        }


        /// <inheritdoc />
        public virtual IUnitOfWork UnitOfWork => _unitOfWork.Value;
        /// <inheritdoc />
        public DbConnectionInfo DbConnectionInfo => _dbConnectionInfo;

        /// <summary>
        /// 覆盖默认工作单元
        /// </summary>
        /// <param name="func"></param>
        public static void OverrideUnitOfWork(Func<IUnitOfWork> func)
        {
            _unitOfWork = new ThreadLocal<IUnitOfWork>(func);
        }

        /// <summary>
        /// protected dbConnectionInfo
        /// </summary>
        protected readonly DbConnectionInfo _dbConnectionInfo;
        /// <summary>
        /// 实例化 RepositoryBase
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public RepositoryBase(DbConnectionInfo dbConnectionInfo)
        {
            this._dbConnectionInfo = dbConnectionInfo;
        }
        /// <summary>
        /// 执行参数化 SQL 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns>受影响的行数</returns>
        protected virtual int Execute(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
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
        /// <summary>
        /// 异步执行参数化 SQL 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns>受影响的行数</returns>
        protected virtual async Task<int> ExecuteAsync(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
                tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

                int result = await conn.ExecuteAsync(sql, param, tran, commandType: CommandType.Text);

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
        /// <summary>
        /// 执行参数化 SQL 语句，选择单个值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        protected virtual TReturn ExecuteScalar<TReturn>(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);

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
        /// <summary>
        /// 异步执行参数化 SQL 语句，选择单个值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        protected virtual async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object param = null, bool useTransaction = false)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;

            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);

                TReturn result = await conn.ExecuteScalarAsync<TReturn>(sql, param, commandType: CommandType.Text);

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
        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TEntity> Query<TEntity>(string sql, object param = null)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
                tran = unitOfWork.DbTransaction;
                return conn.Query<TEntity>(sql, param, tran, commandType: CommandType.Text);
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
        /// <summary>
        /// 异步执行查询语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
                tran = unitOfWork.DbTransaction;
                return await conn.QueryAsync<TEntity>(sql, param, tran, commandType: CommandType.Text);
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
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TReturn> ExecProcedure<TReturn>(string procName, DynamicParameters parameters)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
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
        /// <summary>
        /// 异步执行存储过程
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TReturn>> ExecProcedureAsync<TReturn>(string procName, DynamicParameters parameters)
        {
            IUnitOfWork unitOfWork = UnitOfWork;
            IDbConnection conn = null;
            IDbTransaction tran = null;
            try
            {
                conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(_dbConnectionInfo);
                tran = unitOfWork.DbTransaction;

                return await conn.QueryAsync<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
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
    /// <para>ExecuteAsync、QueryAsync实际上调用DbCommand.ExecuteNonQueryAsync 方法</para>
    /// <para>https://docs.microsoft.com/zh-cn/dotnet/api/system.data.common.dbcommand.executenonqueryasync</para>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract partial class RepositoryBase<TEntity> : RepositoryBase, IRepositoryBase<TEntity> where TEntity : IEntityBase
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
        /// 基于实体上列名的插入语句
        /// </summary>
        public static string Insert_Statement { get; private set; }

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
            try
            {
                PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute<NotMappedAttribute>(true) == null).ToArray();

                IEnumerable<PropertyInfo> keyPropInfos = properties.Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>(true) != null);

                if (keyPropInfos.Any())
                {
                    StringBuilder keyNameWhereByBuilder = new StringBuilder();
                    List<string> _keyNames = new List<string>();
                    foreach (var item in keyPropInfos)
                    {
                        string _keyName = null;
                        var attribute = item.GetCustomAttribute<ColumnAttribute>(true);

                        if (attribute == null)
                        {
                            _keyName = item.Name;
                        }
                        else
                        {
                            _keyName = attribute.Name;
                        }

                        _keyNames.Add(_keyName);
                        //"{1}=@{1} AND {2}=@{2}"
                        keyNameWhereByBuilder.AppendFormat("{0}=@{1} AND ", _keyName, item.Name);
                    }

                    keyNameWhereByBuilder.Remove(keyNameWhereByBuilder.Length - 5, 5);

                    KeyNames = _keyNames.ToArray();
                    KeyNameWhereBy = keyNameWhereByBuilder.ToString();
                }

                TableName = typeof(TEntity).GetCustomAttribute<TableAttribute>(true)?.Name;
                TableName = TableName ?? typeof(TEntity).Name;

                StringBuilder stringBuilder1 = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                // 是否定义了IgnoreSelect特性
                bool isDefineIgnoreSelect = false;
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
                    else
                    {
                        isDefineIgnoreSelect = true;
                    }

                    // 过滤定义了IgnoreUpdate特性的属性
                    if (prop.GetCustomAttribute<IgnoreUpdateAttribute>(true) == null)
                    {
                        stringBuilder4.AppendFormat("{0}=@{1},", columnName, propertyName);
                    }
                }

                Insert_Statement = string.Format("({0}) VALUES({1})",
                    stringBuilder1.Remove(stringBuilder1.Length - 1, 1).ToString(),
                    stringBuilder2.Remove(stringBuilder2.Length - 1, 1).ToString());
                Select_Statement = isDefineIgnoreSelect ? stringBuilder3.Remove(stringBuilder3.Length - 1, 1).ToString() : "*";
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
            catch (Exception ex)
            {
                throw new DbCoreException("初始化实体映射异常", ex);
            }
        }
        /// <summary>
        /// 实例化 RepositoryBase
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public RepositoryBase(DbConnectionInfo dbConnectionInfo) : base(dbConnectionInfo)
        {
        }

        #endregion 构造

        #region Insert
        /// <inheritdoc />
        [CatchException("插入数据异常")]
        public virtual int Insert(TEntity entity)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }
        /// <inheritdoc />
        [CatchException("批量插入数据异常")]
        public virtual int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        #endregion Insert

        #region Delete
        /// <inheritdoc />
        [CatchException("删除数据异常")]
        public virtual int Delete(string whereBy, object param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("删除数据异常")]
        public virtual int Delete(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("批量删除数据异常")]
        public virtual int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("通过Id删除数据异常")]
        public virtual int DeleteById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return base.Execute(sql, new { Id = id });
        }
        /// <inheritdoc />
        [CatchException("通过Id批量删除数据异常")]
        public virtual int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return Execute(sql, new { Ids = ids }, useTransaction);
        }
        #endregion Delete

        #region Query
        /// <inheritdoc />
        [CatchException("查询数据异常")]
        public virtual IEnumerable<TEntity> Query(string whereBy = null, string orderBy = null, object param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, TableName, 
                whereBy == null ? string.Empty : "WHERE " + whereBy, 
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, param);
        }
        /// <inheritdoc />
        [CatchException("查询所有数据异常")]
        public virtual IEnumerable<TEntity> QueryAll(string orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName, 
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, null);
        }
        /// <inheritdoc />
        [CatchException("通过Id查询数据异常")]
        public virtual IEnumerable<TEntity> QueryById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Id = id });
        }
        /// <inheritdoc />
        [CatchException("通过Id批量查询数据异常")]
        public virtual IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Ids = ids });
        }
        /// <inheritdoc />
        [CatchException("获取记录条数异常")]
        public virtual long QueryRecordCount(string whereBy = null, object param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteScalar<long>(sql, param);
        }
        /// <inheritdoc />
        public abstract IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
           string whereBy = null, string orderBy = null, object param = null);

        #endregion Query

        #region Update
        /// <inheritdoc />
        [CatchException("更新数据异常")]
        public virtual int Update(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("更新数据异常")]
        public virtual int Update(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("批量更新数据异常")]
        public virtual int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }
        /// <inheritdoc />
        [CatchException("通过Id更新数据异常")]
        public virtual int UpdateById(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return base.Execute(sql, entity);
        }
        /// <inheritdoc />
        [CatchException("通过Id更新记录的一列异常")]
        public virtual int UpdateColumnById<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return base.Execute(sql, new { Id = id, Value = value });
        }

        #endregion Update
        /// <inheritdoc />
        public abstract int ReplaceInto(TEntity entity, bool useLock = false);
        /// <inheritdoc />
        public abstract int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
        /// <inheritdoc />
        public abstract int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);

        /// <inheritdoc />
        public abstract int ReplaceInto(string tableName, TEntity entity, bool useLock = false);
        /// <inheritdoc />
        public abstract int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
        /// <inheritdoc />
        public abstract int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
    }
}