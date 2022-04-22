using Dapper;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.UnitWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// RepositoryBase
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        public readonly DbConnectionInfo DbConnectionInfo;

        /// <summary>
        /// 实例化 RepositoryBase
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public RepositoryBase(DbConnectionInfo dbConnectionInfo)
        {
            this.DbConnectionInfo = dbConnectionInfo;
        }

        /// <summary>
        /// 得到工作单元，默认返回 <see cref="UnitOfWork.Default"/>
        /// </summary>
        /// <returns></returns>
        protected virtual IUnitOfWork GetUnitOfWork()
        {
            var unitOfWork = UnitOfWork.Default;
            if (unitOfWork.IsExplicitSubmit)
            {
                if (unitOfWork.DbConnection == null)
                {
                    unitOfWork.EnterContext(DbConnectionInfo);
                }
                // 判断是否跨数据库使用工作单元
                else if (unitOfWork.DbConnectionInfo != this.DbConnectionInfo)
                {
                    throw new DbCoreException("工作单元无法跨数据库使用");
                }
            }

            return unitOfWork;
        }

        /// <summary>
        /// 执行参数化 SQL 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns>受影响的行数</returns>
        protected virtual int Execute(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                int result = conn.Execute(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch(Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.Execute", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
        protected virtual async Task<int> ExecuteAsync(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                int result = await conn.ExecuteAsync(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch(Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
        protected virtual TReturn ExecuteScalar<TReturn>(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                var result = conn.ExecuteScalar<TReturn>(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch(Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteScalar", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
        protected virtual async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                var result = await conn.ExecuteScalarAsync<TReturn>(sql, param, tran, commandType: CommandType.Text);
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch(Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteScalarAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
        protected virtual IEnumerable<TEntity> Query<TEntity>(string sql, object? param = null)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return conn.Query<TEntity>(sql, param, tran, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.Query", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
        protected virtual async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object? param = null)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return await conn.QueryAsync<TEntity>(sql, param, tran, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.QueryAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return conn.Query<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
                    transaction: tran, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.ExecProcedure", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return await conn.QueryAsync<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
                    transaction: tran, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.ExecProcedureAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
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
    public abstract partial class RepositoryBase<TEntity> : RepositoryBase, IRepository<TEntity>
    {
        #region 公共静态属性

        /// <summary>
        /// 主键列名
        /// </summary>
        public static string[]? KeyNames { get; private set; }

        /// <summary>
        /// 默认实体删除更新Where约束
        /// </summary>
        public static string? KeyNameWhereBy { get; private set; }

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
                    var _keyNames = new List<string>();
                    foreach (var item in keyPropInfos)
                    {
                        string? _keyName = null;
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

                TableName = typeof(TEntity).GetCustomAttribute<TableAttribute>(true)?.Name ?? typeof(TEntity).Name;

                var stringBuilder1 = new StringBuilder();
                var stringBuilder2 = new StringBuilder();
                var stringBuilder3 = new StringBuilder();
                var stringBuilder4 = new StringBuilder();

                foreach (PropertyInfo prop in properties)
                {
                    string propertyName = prop.Name;
                    string columnName;

                    var columnAttribute = prop.GetCustomAttribute<ColumnAttribute>(true);
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

                Insert_Statement = string.Format("({0}) VALUES({1})",
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
        public virtual int Insert(TEntity entity)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }

        /// <inheritdoc />
        public virtual int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        #endregion Insert

        #region Delete

        /// <inheritdoc />
        public virtual int Delete(string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        /// <inheritdoc />
        public virtual int Delete(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        /// <inheritdoc />
        public virtual int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        /// <inheritdoc />
        public virtual int DeleteById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return base.Execute(sql, new { Id = id });
        }

        /// <inheritdoc />
        public virtual int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return Execute(sql, new { Ids = ids }, useTransaction);
        }

        #endregion Delete

        #region Query

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> Query(string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, param);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> QueryAll(string? orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, null);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> QueryById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Id = id });
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Ids = ids });
        }

        /// <inheritdoc />
        public virtual uint QueryRecordCount(string? whereBy = null, object? param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteScalar<uint>(sql, param);
        }

        /// <inheritdoc />
        public abstract IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        /// <inheritdoc />
        public abstract PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, string keywordMappedPropName);
        /// <inheritdoc />
        public abstract PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, string[] keywordMappedPropNames);
        #endregion Query

        #region Update

        /// <inheritdoc />
        public virtual int Update(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        /// <inheritdoc />
        public virtual int Update(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        /// <inheritdoc />
        public virtual int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        /// <inheritdoc />
        public virtual int UpdateById(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return base.Execute(sql, entity);
        }

        /// <inheritdoc />
        public virtual int UpdateColumnById<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return base.Execute(sql, new { Id = id, Value = value });
        }

        #endregion Update

        #region Other
        /// <inheritdoc />
        public abstract int ReplaceInto(TEntity entity);

        /// <inheritdoc />
        public abstract int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <inheritdoc />
        public abstract int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <inheritdoc />
        public abstract int ReplaceInto(string tableName, TEntity entity);

        /// <inheritdoc />
        public abstract int ReplaceIntoBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <inheritdoc />
        public abstract int InsertIgnoreBatch(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);
        #endregion
    }
}