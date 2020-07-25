using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Primitives.Entity;

namespace IceCoffee.DbCore.Primitives.Repository
{
    /// <summary>
    /// 通用仓储基类
    /// <para>ExecuteAsync、QueryAsync实际上调用DbCommand.ExecuteNonQueryAsync 方法，是同步</para>
    /// <para>https://docs.microsoft.com/zh-cn/dotnet/api/system.data.common.dbcommand.executenonqueryasync</para>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract partial class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        /// <summary>
        /// sql语句缓存字典
        /// </summary>
        //protected static readonly ConcurrentDictionary<string, string> sqlStatementCache = new ConcurrentDictionary<string, string>();

        #region 内部保护属性
        internal protected abstract IDbConnection Connection { get; }
        /// <summary>
        /// 表示要在数据源上执行的事务，由AOP切面提供
        /// </summary>
        internal protected IDbTransaction Transaction { get; set; }
        #endregion

        #region 公共静态属性
        /// <summary>
        /// 主键列名
        /// </summary>
        public static string KeyName { get; private set; }
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
        #endregion

        #region 静态构造
        static RepositoryBase()
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>(false) == null).ToArray();

            PropertyInfo keyPropInfo = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>(true) != null);
            KeyName = keyPropInfo.GetCustomAttribute<ColumnAttribute>(false)?.Name;
            KeyName = KeyName ?? keyPropInfo.Name;
            TableName = typeof(TEntity).GetCustomAttribute<TableAttribute>(false)?.Name;
            TableName = TableName ?? typeof(TEntity).Name;

            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            StringBuilder stringBuilder4 = new StringBuilder();

            
            foreach (PropertyInfo prop in properties)
            {
                string propertyName = prop.Name;
                string columnName;
                
                ColumnAttribute columnAttribute = prop.GetCustomAttribute<ColumnAttribute>(false);
                columnName = columnAttribute != null ? columnAttribute.Name : prop.Name;                

                // 过滤定义了IgnoreInsert特性的属性
                if (prop.GetCustomAttribute<IgnoreInsertAttribute>(false) == null)
                {
                    stringBuilder1.AppendFormat("{0},", columnName);
                    stringBuilder2.AppendFormat("@{0},", propertyName);
                }
                // 过滤定义了IgnoreSelect特性的属性
                if (prop.GetCustomAttribute<IgnoreSelectAttribute>(false) == null)
                {
                    stringBuilder3.AppendFormat("{0},", columnName);
                }
                // 过滤定义了IgnoreUpdate特性的属性
                if (prop.GetCustomAttribute<IgnoreUpdateAttribute>(false) == null)
                {
                    stringBuilder4.AppendFormat("{0}=@{1},", columnName, propertyName);
                }                
            }

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
                    if(result != null)
                    {
                        return result;
                    }
                    // Column特性为空则返回默认对应列名的属性
                    return properties.FirstOrDefault(prop => prop.Name == columnName);
                }
            );

            SqlMapper.SetTypeMap(typeof(TEntity), propertyMap);
        }
        #endregion

        //private static string GetSqlFromCache([CallerMemberName]string callerMethodName = "")
        //{
        //    if(sqlStatementCache.TryGetValue(callerMethodName, out string sql))
        //    {
        //        return sql;
        //    }
        //    return null;
        //}
        //private static string CacheSql(string sql, [CallerMemberName]string callerMethodName = "")
        //{
        //    sqlStatementCache[callerMethodName] = sql;
        //    return sql;
        //}

        #region Insert
        public virtual int InsertOne(TEntity entity)
        {
            return Connection.Execute(Insert_Statement_Fixed, entity, Transaction);
        }
        public virtual int InsertList(IEnumerable<TEntity> entitys)
        {
            return Connection.Execute(Insert_Statement_Fixed, entitys, Transaction);
        }
        #endregion

        #region Delete
        public virtual int DeleteAny(string conditions, object param = null)
        {
            string sql = string.Format("DELETE FROM {0} {1}", TableName, conditions == null ? null : "WHERE " + conditions);
            return Connection.Execute(sql, param, Transaction);
        }
        public virtual int DeleteOne(TEntity entity)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Key", TableName, KeyName);
            return Connection.Execute(sql, entity, Transaction);
        }
        public virtual int DeleteList(IEnumerable<TEntity> entitys)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Key", TableName, KeyName);
            return Connection.Execute(sql, entitys, Transaction);
        }
        public virtual int DeleteOneByKey(TKey key)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Key", TableName, KeyName);
            return Connection.Execute(sql, new { Key = key }, Transaction);
        }
        public virtual int DeleteListByKeys(IEnumerable<TKey> keys)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Key", TableName, KeyName);
            List<object> param = new List<object>();
            foreach (var key in keys)
            {
                param.Add(new { Key = key });
            }
            return Connection.Execute(sql, param, Transaction);
        }
        public virtual int DeleteOneById<TId>(TId id, string idColumnName)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            return Connection.Execute(sql, new { Id = id }, Transaction);
        }
        public virtual int DeleteListByIds<TId>(IEnumerable<TId> ids, string idColumnName)
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1}=@Id", TableName, idColumnName);
            List<object> param = new List<object>();
            foreach (var id in ids)
            {
                param.Add(new { Id = id });
            }
            return Connection.Execute(sql, param, Transaction);
        }
        public virtual int DeleteAll()
        {
            string sql = string.Format("DELETE FROM {0}", TableName);
            return Connection.Execute(sql, null, Transaction);
        }
        #endregion

        #region Query
        public virtual IEnumerable<TEntity> QueryAny(string columnNames, string conditions, string orderby, object param = null)
        {
            string sql = string.Format("SELECT {0} FROM {1} {2} {3}", columnNames, TableName, conditions == null ? null : "WHERE " + conditions, orderby);
            return Connection.Query<TEntity>(sql, param, Transaction);
        }
        public virtual TEntity QueryOneByKey(TKey key)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Key", Select_Statement, TableName, KeyName);
            return Connection.QueryFirstOrDefault<TEntity>(sql, new { Key = key }, Transaction);
        }
        public virtual IEnumerable<TEntity> QueryListByKeys(IEnumerable<TKey> keys)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Key", Select_Statement, TableName, KeyName);
            List<object> param = new List<object>();
            foreach (var key in keys)
            {
                param.Add(new { Key = key });
            }
            return Connection.Query<TEntity>(sql, param, Transaction);
        }
        public virtual IEnumerable<TEntity> QueryAll(string orderby = null)
        {
            string sql = string.Format("SELECT {0} FROM {1}{2}", Select_Statement, TableName, orderby == null ? null : " ORDER BY " + orderby);
            return Connection.Query<TEntity>(sql, null, Transaction);
        }
        public virtual TEntity QueryOneById<TId>(TId id, string idColumnName)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return Connection.QueryFirstOrDefault<TEntity>(sql, new { Id = id }, Transaction);
        }
        public virtual IEnumerable<TEntity> QueryListById<TId>(TId id, string idColumnName)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}=@Id", Select_Statement, TableName, idColumnName);
            return Connection.Query<TEntity>(sql, new { Id = id }, Transaction);
        }       
        public virtual long QueryRecordCount(string conditions = null, object param = null)
        {
            string sql = string.Format("SELECT COUNT(*) AS Total FROM {0} {1}", TableName, conditions == null ? null : "WHERE " + conditions);
            return Connection.QueryFirstOrDefault(sql, param, Transaction).Total;
        }
        #region 待实现
        public abstract IEnumerable<TEntity> QueryListPaged(int pageNumber, int rowsPerPage,
           string conditions = null, string orderby = null, object param = null);
        #endregion
        #endregion

        #region Update
        public virtual int UpdateAny(string setClause, string conditions, object param)
        {
            string sql = string.Format("UPDATE {0} SET {1} {2}", TableName, setClause, conditions == null ? null : "WHERE " + conditions);
            return Connection.Execute(sql, param, Transaction);
        }
        public virtual int UpdateOne(TEntity entity)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@Key", TableName, UpdateSet_Statement, KeyName);
            return Connection.Execute(sql, entity, Transaction);
        }
        public virtual int UpdateList(IEnumerable<TEntity> entitys)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@Key", TableName, UpdateSet_Statement, KeyName);
            return Connection.Execute(sql, entitys, Transaction);
        }
        public virtual int UpdateById<TId>(TEntity entity, TId id, string idColumnName)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@Id", TableName, UpdateSet_Statement, idColumnName);
            return Connection.Execute(sql, new { Id = id }, Transaction);
        }
        public virtual int UpdateOneColById<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName)
        {
            string sql = string.Format("UPDATE {0} SET {1}=@Value WHERE {2}=@Id", TableName, valueColumnName, idColumnName);
            return Connection.Execute(sql, new { Id = id, Value = value }, Transaction);
        }
        #endregion


        #region Old
        //#region 插入

        //#region 同步
        //public virtual int InsertData(string tableName, string[] columnNames, TEntity t)
        //{
        //    return insertData(tableName, columnNames, t);
        //}
        //public virtual int InsertData(TEntity t)
        //{
        //    return insertData(TableName, t);
        //}
        //public virtual int InsertData(string tableName, string[] columnNames, IEnumerable<TEntity> t)
        //{
        //    return insertData(tableName, columnNames, t);
        //}
        //public virtual int InsertData(IEnumerable<TEntity> t)
        //{
        //    return insertData(TableName, t);
        //}

        //public virtual int ReplaceInsertData(string[] columnNames, TEntity t)
        //{
        //    return replaceInsertData(TableName, columnNames, t);
        //}
        //#endregion

        //#region 异步
        //public virtual async Task<int> InsertDataAsync(string tableName, string[] columnNames, TEntity t)
        //{
        //    var result = await Task.Run(() => { return insertData(tableName, columnNames, t); });
        //    return result;
        //}
        //public virtual async Task<int> InsertDataAsync(TEntity t)
        //{
        //    int result = await Task.Run(() => { return insertData(TableName, t); });
        //    return result;
        //}
        //public virtual async Task<int> InsertDataAsync(string tableName, string[] columnNames, IEnumerable<TEntity> t)
        //{
        //    var result = await Task.Run(() => { return insertData(tableName, columnNames, t); });
        //    return result;
        //}
        //public virtual async Task<int> InsertDataAsync(IEnumerable<TEntity> t)
        //{
        //    var result = await Task.Run(() => { return insertData(TableName, t); });
        //    return result;
        //}
        //#endregion

        //[CatchException(Error = "插入数据异常")]
        //private int insertData(string tableName, string[] columnNames, object param)
        //{
        //    string sql = string.Format("INSERT INTO {0} ({1}) VALUES(@{2})", tableName,
        //        string.Join(",", columnNames), string.Join(",@", columnNames));
        //    return Connection.Execute(sql, param, Transaction);
        //}
        //[CatchException(Error = "插入数据异常")]
        //private int insertData(string tableName, object param)
        //{
        //    string sql = string.Format("INSERT INTO {0} ({1}) VALUES(@{2})", tableName, string.Join(",", ColumnNames), string.Join(",@", PropertyNames));
        //    return Connection.Execute(sql, param, Transaction);
        //}
        //[CatchException(Error = "插入或更新数据异常")]
        //private int replaceInsertData(string tableName, string[] columnNames, object param)
        //{
        //    string sql = string.Format("REPLACE INTO {0} ({1}) VALUES(@{2})", tableName,
        //        string.Join(",", columnNames), string.Join(",@", columnNames));
        //    return Connection.Execute(sql, param, Transaction);
        //}
        //#endregion

        //#region 查询

        //#region 同步
        //[CatchException(Error = "查询数据异常")]
        //public virtual IEnumerable<TEntity> QueryData(string tableName, string clauses = null, TEntity t = null, string[] columnNames = null)
        //{
        //    string columnName = (columnNames == null || columnNames.Length == 0) ? "*" : 
        //        (columnNames.Length == 1 ? columnNames[0] : string.Join(",", columnNames));
        //    string sql = string.Format("SELECT {0} FROM {1} {2}", columnName, tableName, clauses ?? string.Empty);
        //    var entitys = Connection.Query<TEntity>(sql, t, Transaction);

        //    return entitys;
        //}
        //public virtual IEnumerable<TEntity> QueryData(string clauses, TEntity t, string[] columnNames = null)
        //{
        //    return QueryData(TableName, clauses, t, columnNames);
        //}
        //public virtual IEnumerable<TEntity> QueryAllData(string tableName, string clauses = null)
        //{
        //    return QueryData(tableName, clauses, null, null);
        //}
        //public virtual TEntity QueryOneData(string clauses, TEntity t)
        //{
        //    return QueryData(TableName, clauses, t, null).FirstOrDefault();
        //}
        //[CatchException(Error = "查询记录条数异常")]
        //public virtual long QueryCountOfRecords(string clauses, TEntity t)
        //{
        //    return Connection.Query(string.Format("SELECT COUNT(*) AS TOTAL FROM {0} {1}",
        //        TableName, clauses), t, Transaction).FirstOrDefault().total;
        //}
        //public virtual TEntity QueryOneDataByID<TId>(string tableName, string idColumnName, TId id)
        //{
        //    return Connection.Query<TEntity>(string.Format("SELECT * FROM {0} WHERE {1}=@id", tableName, idColumnName), new { id }, Transaction).FirstOrDefault();
        //}
        //#endregion

        //#region 异步
        //public virtual async Task<IEnumerable<TEntity>> QueryDataAsync(string tableName, string clauses = null, TEntity t = null, string[] columnNames = null)
        //{
        //    var result = await Task.Run(() => { return QueryData(tableName, clauses, t, columnNames); });
        //    return result;
        //}
        //public virtual async Task<IEnumerable<TEntity>> QueryDataAsync(string clauses, TEntity t, string[] columnNames = null)
        //{
        //    var result = await Task.Run(() => { return QueryData(TableName, clauses, t, columnNames); });
        //    return result;
        //}
        //public virtual async Task<IEnumerable<TEntity>> QueryAllDataAsync(string tableName, string clauses = null)
        //{
        //    var result = await Task.Run(() => { return QueryData(tableName, clauses, null, null); });
        //    return result;
        //}
        //public virtual async Task<TEntity> QueryOneDataAsync(string clauses, TEntity t)
        //{
        //    var result = await Task.Run(() => { return QueryData(TableName, clauses, t, null).FirstOrDefault(); });
        //    return result;
        //}
        //public virtual async Task<long> QueryCountOfRecordsAsync(string clauses, TEntity t)
        //{
        //    var result = await Task.Run(() => { return QueryCountOfRecords(clauses, t); });
        //    return result;
        //}
        //public virtual async Task<TEntity> QueryOneDataByIDAsync<TId>(string tableName, string idColumnName, TId id)
        //{
        //    var result = await Task.Run(() => { return QueryOneDataByID(tableName, idColumnName, id); });
        //    return result;
        //}
        //#endregion

        //#endregion

        //#region 删除

        //#region 同步
        //[CatchException(Error = "删除数据异常")]
        //public virtual int DeleteData(string tableName, string clauses, TEntity t = null)
        //{
        //    return Connection.Execute(string.Format("DELETE FROM {0} {1}", tableName, clauses), t, Transaction);
        //}
        //public virtual int DeleteData(string clauses, TEntity t)
        //{
        //    return DeleteData(TableName, clauses, t);
        //}
        //[CatchException(Error = "通过ID删除数据异常")]
        //public virtual int DeleteDataByID<TId>(string tableName, string idColumnName, TId id)
        //{
        //    return Connection.Execute(string.Format("DELETE FROM {0} WHERE {1}=@id", tableName, idColumnName), new { id }, Transaction);
        //}
        //[CatchException(Error = "删除全部数据异常")]
        //public virtual int DeleteAllData(string tableName)
        //{
        //    return Connection.Execute(string.Format("DELETE FROM {0}", tableName), null, Transaction);
        //}
        //#endregion

        //#region 异步
        //public virtual async Task<int> DeleteDataAsync(string tableName, string clauses, TEntity t = null)
        //{
        //    var result = await Task.Run(() => { return DeleteData(tableName, clauses, t); });
        //    return result;
        //}
        //public virtual async Task<int> DeleteDataAsync(string clauses, TEntity t)
        //{
        //    var result = await Task.Run(() => { return DeleteData(clauses, t); });
        //    return result;
        //}
        //public virtual async Task<int> DeleteDataByIDAsync<TId>(string tableName, string idColumnName, TId id)
        //{
        //    var result = await Task.Run(() => { return DeleteDataByID(tableName, idColumnName, id); });
        //    return result;
        //}
        //public virtual async Task<int> DeleteAllDataAsync(string tableName)
        //{
        //    var result = await Task.Run(() => { return DeleteAllData(tableName); });
        //    return result;
        //}
        //#endregion

        //#endregion

        //#region 修改

        //#region 同步
        //public virtual int UpdateData(string[] columnNames, string clauses, TEntity t)
        //{
        //    return updateData(TableName, columnNames, clauses, t);
        //}
        //public virtual int UpdateData(string tableName, string[] columnNames, string clauses, TEntity t)
        //{
        //    return updateData(tableName, columnNames, clauses, t);
        //}

        //[CatchException(Error = "更新数据异常")]
        //private int updateData(string tableName, string[] columnNames, string clauses, object param)
        //{
        //    int len = columnNames.Length;
        //    string[] columnName = new string[len];
        //    for (int i = 0; i < len; ++i)
        //    {
        //        columnName[i] = string.Format("{0}=@{0}", columnNames[i]);
        //    }
        //    string sql = string.Format("UPDATE {0} SET {1} {2}", tableName, string.Join(",", columnName), clauses);
        //    return Connection.Execute(sql, param, Transaction);
        //}
        //public virtual int UpdateDataByID(string idColumnName, string[] columnNames, TEntity t)
        //{
        //    return updateData(TableName, columnNames, string.Format("WHERE {0}=@{0}", idColumnName), t);
        //}
        //[CatchException(Error = "更新数据异常")]
        //public virtual int UpdateData<TValue>(string tableName, string valueColumnName, TValue value, string clauses = null)
        //{
        //    string sql = string.Format("UPDATE {0} SET {1}=@value {2}", tableName, valueColumnName, clauses);
        //    return Connection.Execute(sql, new { value }, Transaction);
        //}
        //[CatchException(Error = "更新数据异常")]
        //public int UpdateDataByID<TId, TValue>(string tableName, string idColumnName, TId id, string valueColumnName, TValue value)
        //{
        //    string sql = string.Format("UPDATE {0} SET {1}=@value WHERE {2}=@id", tableName, valueColumnName, idColumnName);
        //    return Connection.Execute(sql, new { id, value }, Transaction);
        //}
        //[CatchException(Error = "更新数据异常")]
        //public int UpdateDataByID<TId>(string tableName, string idColumnName, TId id, string valueColumnName, string valueString)
        //{
        //    string sql = string.Format("UPDATE {0} SET {1}={2} WHERE {3}=@id", tableName, valueColumnName, valueString, idColumnName);
        //    return Connection.Execute(sql, new { id }, Transaction);
        //}
        //#endregion

        //#region 异步
        //public virtual async Task<int> UpdateDataAsync(string[] columnNames, string clauses, TEntity t)
        //{
        //    var result = await Task.Run(() => { return updateData(TableName, columnNames, clauses, t); });
        //    return result;
        //}
        //public virtual async Task<int> UpdateDataAsync(string tableName, string[] columnNames, string clauses, TEntity t)
        //{
        //    var result = await Task.Run(() => { return updateData(tableName, columnNames, clauses, t); });
        //    return result;
        //}

        //public virtual async Task<int> UpdateDataByIDAsync(string idColumnName, string[] columnNames, TEntity t)
        //{
        //    var result = await Task.Run(() => { return updateData(TableName, columnNames, string.Format("WHERE {0}=@{0}", idColumnName), t); });
        //    return result;
        //}

        //public virtual async Task<int> UpdateDataAsync<TValue>(string tableName, string valueColumnName, TValue value, string clauses = null)
        //{
        //    var result = await Task.Run(() => { return UpdateData(tableName, valueColumnName, value, clauses); });
        //    return result;
        //}
        //#endregion

        //#endregion
        #endregion
    }

}
