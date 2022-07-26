using Dapper;
using IceCoffee.DbCore.Dtos;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.OptionalAttributes;
using System.Data;
using System.Reflection;
using System.Text;

namespace IceCoffee.DbCore.Repositories
{
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
        /// 基于实体上列名的插入语句
        /// </summary>
        public static string Insert_Statement { get; private set; }

        /// <summary>
        /// 主键列名
        /// </summary>
        public static string[]? KeyNames { get; private set; }

        /// <summary>
        /// 默认实体删除更新Where约束
        /// </summary>
        public static string? KeyNameWhereBy { get; private set; }

        /// <summary>
        /// 基于实体上列名的选择语句
        /// </summary>
        public static string Select_Statement { get; private set; }

        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName { get; private set; }

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

        public virtual int Delete(string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        public virtual int Delete(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        public virtual int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        public virtual int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} IN @Ids", TableName, idColumnName);
            return base.Execute(sql, new { Ids = ids }, useTransaction);
        }

        public virtual int DeleteBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", tableName, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        public virtual int DeleteById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return base.Execute(sql, new { Id = id });
        }

        public virtual int DeleteByTableName(string tableName, string whereBy, object? param = null, bool useTransaction = false)
        {
            string sql = string.Format("DELETE FROM {0} {1}", tableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        public virtual int DeleteByTableName(string tableName, TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}", tableName, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        public virtual int Insert(TEntity entity)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entity);
        }

        public virtual int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", TableName, Insert_Statement), entities, useTransaction);
        }

        public virtual int InsertBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", tableName, Insert_Statement), entities, useTransaction);
        }

        public virtual int InsertByTableName(string tableName, TEntity entity)
        {
            return base.Execute(string.Format("INSERT INTO {0} {1}", tableName, Insert_Statement), entity);
        }

        public abstract int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        public virtual IEnumerable<TEntity> QueryAll(string? orderBy = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2}", Select_Statement, TableName,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, null);
        }

        public virtual IEnumerable<TEntity> Query(string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, TableName,
                whereBy == null ? string.Empty : "WHERE " + whereBy,
                orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, param);
        }

        public virtual IEnumerable<TEntity> QueryById<TId>(string idColumnName, TId id)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Id = id });
        }

        public virtual IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @Ids", Select_Statement, TableName, idColumnName);
            return base.Query<TEntity>(sql, new { Ids = ids });
        }

        public virtual IEnumerable<TEntity> QueryByTableName(string tableName, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", Select_Statement, tableName,
               whereBy == null ? string.Empty : "WHERE " + whereBy,
               orderBy == null ? string.Empty : "ORDER BY " + orderBy);
            return base.Query<TEntity>(sql, param);
        }

        public abstract IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        public abstract IEnumerable<TEntity> QueryPagedByTableName(string tableName, int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null);

        public abstract PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        public abstract PaginationResultDto<TEntity> QueryPagedByTableName(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        public virtual int QueryRecordCount(string? whereBy = null, object? param = null)
        {
            return this.QueryRecordCountByTableName(TableName, whereBy, param);
        }

        public virtual int QueryRecordCountByTableName(string tableName, string? whereBy = null, object? param = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} {1}", tableName, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.ExecuteScalar<int>(sql, param);
        }

        public abstract int ReplaceInto(TEntity entity);

        public abstract int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract int ReplaceIntoBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        public abstract int ReplaceIntoByTableName(string tableName, TEntity entity);

        public virtual int Update(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        public virtual int Update(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        public virtual int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        public virtual int UpdateBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entities, useTransaction);
        }

        public virtual int UpdateById(string idColumnName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", TableName, UpdateSet_Statement, idColumnName);
            return base.Execute(sql, entity);
        }

        public virtual int UpdateByTableName(string tableName, string setClause, string whereBy, object param, bool useTransaction = false)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", tableName, setClause, whereBy == null ? string.Empty : "WHERE " + whereBy);
            return base.Execute(sql, param, useTransaction);
        }

        public virtual int UpdateByTableName(string tableName, TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, UpdateSet_Statement, KeyNameWhereBy);
            return base.Execute(sql, entity);
        }

        public virtual int UpdateColumnById<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return base.Execute(sql, new { Id = id, Value = value });
        }
    }
}