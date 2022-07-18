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
            return this.DeleteAsync(whereBy, param, useTransaction).Result;
        }

        public virtual int Delete(TEntity entity)
        {
            return this.DeleteAsync(entity).Result;
        }

        public virtual int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.DeleteBatchAsync(entities, useTransaction).Result;
        }

        public virtual int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false)
        {
            return this.DeleteBatchByIdsAsync(idColumnName, ids, useTransaction).Result;
        }

        public virtual int DeleteBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.DeleteBatchByTableNameAsync(tableName, entities, useTransaction).Result;
        }

        public virtual int DeleteById<TId>(string idColumnName, TId id)
        {
            return this.DeleteByIdAsync(idColumnName, id).Result;
        }

        public virtual int DeleteByTableName(string tableName, TEntity entity)
        {
            return this.DeleteByTableNameAsync(tableName, entity).Result;
        }

        public virtual int DeleteByTableName(string tableName, string whereBy, object? param = null, bool useTransaction = false)
        {
            return this.DeleteByTableNameAsync(tableName, whereBy, param, useTransaction).Result;
        }

        public virtual int Insert(TEntity entity)
        {
            return this.InsertAsync(entity).Result;
        }

        public virtual int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertBatchAsync(entities, useTransaction).Result;
        }

        public virtual int InsertBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertBatchByTableNameAsync(tableName, entities, useTransaction).Result;
        }

        public virtual int InsertByTableName(string tableName, TEntity entity)
        {
            return this.InsertByTableNameAsync(tableName, entity).Result;
        }

        public virtual int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatchAsync(entities, useTransaction).Result;
        }

        public virtual int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.InsertIgnoreBatchByTableNameAsync(tableName, entities, useTransaction).Result;
        }

        public virtual IEnumerable<TEntity> Query(string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryAsync(whereBy, orderBy, param).Result;
        }

        public virtual IEnumerable<TEntity> QueryAll(string? orderBy = null)
        {
            return this.QueryAllAsync(orderBy).Result;
        }

        public virtual IEnumerable<TEntity> QueryById<TId>(string idColumnName, TId id)
        {
            return this.QueryByIdAsync(idColumnName, id).Result;
        }

        public virtual IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids)
        {
            return this.QueryByIdsAsync(idColumnName, ids).Result;
        }

        public virtual IEnumerable<TEntity> QueryByTableName(string tableName, string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryByTableNameAsync(tableName, whereBy, orderBy, param).Result;
        }

        public virtual IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryPagedAsync(pageIndex, pageSize, whereBy, orderBy, param).Result;
        }

        public virtual IEnumerable<TEntity> QueryPagedByTableName(string tableName, int pageIndex, int pageSize,
           string? whereBy = null, string? orderBy = null, object? param = null)
        {
            return this.QueryPagedByTableNameAsync(tableName, pageIndex, pageSize, whereBy, orderBy, param).Result;
        }

        public virtual PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, params string[] keywordMappedColumnNames)
        {
            return this.QueryPagedAsync(dto, keywordMappedColumnNames).Result;
        }

        public virtual PaginationResultDto<TEntity> QueryPagedByTableName(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames)
        {
            return this.QueryPagedByTableNameAsync(tableName, dto, keywordMappedColumnNames).Result;
        }

        public virtual int QueryRecordCount(string? whereBy = null, object? param = null)
        {
            return this.QueryRecordCountAsync(whereBy, param).Result;
        }

        public virtual int QueryRecordCountByTableName(string tableName, string? whereBy = null, object? param = null)
        {
            return this.QueryRecordCountByTableNameAsync(tableName, whereBy, param).Result;
        }

        public virtual int ReplaceInto(TEntity entity)
        {
            return this.ReplaceIntoAsync(entity).Result;
        }

        public virtual int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatchAsync(entities, useTransaction).Result;
        }

        public virtual int ReplaceIntoBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.ReplaceIntoBatchByTableNameAsync(tableName, entities, useTransaction).Result;
        }

        public virtual int ReplaceIntoByTableName(string tableName, TEntity entity)
        {
            return this.ReplaceIntoByTableNameAsync(tableName, entity).Result;
        }

        public virtual int Update(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return this.UpdateAsync(setClause, whereBy, param, useTransaction).Result;
        }

        public virtual int Update(TEntity entity)
        {
            return this.UpdateAsync(entity).Result;
        }

        public virtual int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.UpdateBatchAsync(entities, useTransaction).Result;
        }

        public virtual int UpdateBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false)
        {
            return this.UpdateBatchByTableNameAsync(tableName, entities, useTransaction).Result;
        }

        public virtual int UpdateById(string idColumnName, TEntity entity)
        {
            return this.UpdateByIdAsync(idColumnName, entity).Result;
        }

        public virtual int UpdateByTableName(string tableName, string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return this.UpdateByTableNameAsync(tableName, setClause, whereBy, param, useTransaction).Result;
        }

        public virtual int UpdateByTableName(string tableName, TEntity entity)
        {
            return this.UpdateByTableNameAsync(tableName, entity).Result;
        }

        public virtual int UpdateColumnById<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value)
        {
            return this.UpdateColumnByIdAsync(idColumnName, id, valueColumnName, value).Result;
        }
    }
}