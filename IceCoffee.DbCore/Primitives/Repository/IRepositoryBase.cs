using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Primitives.Entity;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public partial interface IRepositoryBase<TEntity, in TKey> where TEntity : IEntity<TKey>
    {
        #region Insert
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int InsertOne(TEntity entity);
        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int InsertList(IEnumerable<TEntity> entitys);
        #endregion

        #region Delete
        /// <summary>
        /// 根据条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int DeleteAny(string conditions, object param = null);
        /// <summary>
        /// 根据默认主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteOne(TEntity entity);
        /// <summary>
        /// 根据默认主键删除多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int DeleteList(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 根据主键删除记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int DeleteOneByKey(TKey key);
        /// <summary>
        /// 根据多个主键删除多条记录
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        int DeleteListByKeys(IEnumerable<TKey> keys);
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int DeleteOneById<TId>(TId id, string idColumnName);
        /// <summary>
        /// 根据多个ID删除多条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="ids"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int DeleteListByIds<TId>(IEnumerable<TId> ids, string idColumnName);
        /// <summary>
        /// 删除关联表的所有记录
        /// </summary>
        /// <returns></returns>
        int DeleteAll();
        #endregion

        #region Query
        /// <summary>
        /// 根据条件、顺序字符串和匿名对象执行任意查询语句
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAny(string columnNames, string conditions, string orderby, object param = null);
        /// <summary>
        /// 根据主键获取一条记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity QueryOneByKey(TKey key);
        /// <summary>
        /// 根据多个主键获取多条记录
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryListByKeys(IEnumerable<TKey> keys);
        /// <summary>
        /// 查询关联表的所有记录
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAll(string orderby = null);
        /// <summary>
        /// 根据ID获取一条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        TEntity QueryOneById<TId>(TId id, string idColumnName);
        /// <summary>
        /// 根据ID获取多条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryListById<TId>(TId id, string idColumnName);
        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页数量</param>
        /// <param name="conditions">where条件字符串</param>
        /// <param name="orderby">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryListPaged(int pageNumber, int rowsPerPage, string conditions = null, string orderby = null, object param = null);
        /// <summary>
        /// 获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        long QueryRecordCount(string conditions = null, object param = null);
        #endregion

        #region Update
        /// <summary>
        /// 根据set子句、条件和匿名对象执行任意更新语句
        /// </summary>
        /// <param name="setClause"></param>
        /// <param name="conditions"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int UpdateAny(string setClause, string conditions, object param);
        /// <summary>
        /// 根据默认主键更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateOne(TEntity entity);
        /// <summary>
        /// 根据默认主键更新多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int UpdateList(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 根据ID更新记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int UpdateById<TId>(TEntity entity, TId id, string idColumnName);
        /// <summary>
        /// 根据ID更新记录的一列
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="idColumnName"></param>
        /// <param name="valueColumnName"></param>
        /// <returns></returns>
        int UpdateOneColById<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName);
        #endregion

        #region Old
        //#region 插入        

        ///// <summary>
        ///// 插入一条数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="t"></param>
        ///// <returns>受影响的行数</returns>
        //int InsertData(string tableName, string[] columnNames, TEntity t);

        ///// <summary>
        ///// 插入一条数据
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int InsertData(TEntity t);

        ///// <summary>
        ///// 插入多条数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int InsertData(string tableName, string[] columnNames, IEnumerable<TEntity> t);

        ///// <summary>
        ///// 插入多条数据
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int InsertData(IEnumerable<TEntity> t);


        ///// <summary>
        ///// 插入一条数据
        ///// 记录存在则修改、不存在则添加
        ///// </summary>
        ///// <param name="columnNames"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //int ReplaceInsertData(string[] columnNames, TEntity t);

        ///// <summary>
        ///// 异步插入一条数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //Task<int> InsertDataAsync(string tableName, string[] columnNames, TEntity t);

        ///// <summary>
        ///// 异步插入一条数据
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> InsertDataAsync(TEntity t);

        ///// <summary>
        ///// 异步插入多条数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> InsertDataAsync(string tableName, string[] columnNames, IEnumerable<TEntity> t);

        ///// <summary>
        ///// 异步插入多条数据
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> InsertDataAsync(IEnumerable<TEntity> t);

        //#endregion

        //#region 查询
        ///// <summary>
        ///// 查询多行数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="columnNames">需要返回的列</param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //IEnumerable<TEntity> QueryData(string tableName, string clauses = null, TEntity t = null, string[] columnNames = null);

        ///// <summary>
        ///// 查询多行数据
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="columnNames">需要返回的列</param>
        ///// <returns></returns>
        //IEnumerable<TEntity> QueryData(string clauses, TEntity t, string[] columnNames = null);

        ///// <summary>
        ///// 查询全部数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //IEnumerable<TEntity> QueryAllData(string tableName, string clauses = null);

        ///// <summary>
        ///// 查询一行数据
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //TEntity QueryOneData(string clauses, TEntity t);

        ///// <summary>
        ///// 查询记录条数
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //long QueryCountOfRecords(string clauses, TEntity t);

        ///// <summary>
        ///// 通过ID查询一行数据
        ///// </summary>
        ///// <typeparam name="TId"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //TEntity QueryOneDataByID<TId>(string tableName, string idColumnName, TId id);

        ///// <summary>
        ///// 异步查询多行数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="columnNames">需要返回的列</param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //Task<IEnumerable<TEntity>> QueryDataAsync(string tableName, string clauses = null, TEntity t = null, string[] columnNames = null);

        ///// <summary>
        ///// 异步查询多行数据
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="columnNames">需要返回的列</param>
        ///// <returns></returns>
        //Task<IEnumerable<TEntity>> QueryDataAsync(string clauses, TEntity t, string[] columnNames = null);

        ///// <summary>
        ///// 异步查询全部数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //Task<IEnumerable<TEntity>> QueryAllDataAsync(string tableName, string clauses = null);

        ///// <summary>
        ///// 异步查询一行数据
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //Task<TEntity> QueryOneDataAsync(string clauses, TEntity t);

        ///// <summary>
        ///// 异步查询记录条数
        ///// </summary>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns></returns>
        //Task<long> QueryCountOfRecordsAsync(string clauses, TEntity t);

        ///// <summary>
        ///// 异步通过ID查询一行数据
        ///// </summary>
        ///// <typeparam name="TId"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<TEntity> QueryOneDataByIDAsync<TId>(string tableName, string idColumnName, TId id);
        //#endregion

        //#region 删除
        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int DeleteData(string tableName, string clauses, TEntity t = null);

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <returns>受影响的行数</returns>
        //int DeleteData(string clauses, TEntity t);

        ///// <summary>
        ///// 通过ID删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int DeleteDataByID<TId>(string tableName, string idColumnName, TId id);

        ///// <summary>
        ///// 删除所有数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <returns></returns>
        //int DeleteAllData(string tableName);

        ///// <summary>
        ///// 异步删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> DeleteDataAsync(string tableName, string clauses, TEntity t = null);

        ///// <summary>
        ///// 异步删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> DeleteDataAsync(string clauses, TEntity t);

        ///// <summary>
        ///// 通过ID异步删除数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> DeleteDataByIDAsync<TId>(string tableName, string idColumnName, TId id);

        ///// <summary>
        ///// 异步删除所有数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <returns></returns>
        //Task<int> DeleteAllDataAsync(string tableName);
        //#endregion

        //#region 修改
        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="columnNames"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int UpdateData(string[] columnNames, string clauses, TEntity t);

        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //int UpdateData(string tableName, string[] columnNames, string clauses, TEntity t);

        ///// <summary>
        ///// 通过ID修改数据
        ///// </summary>
        ///// <param name="columnNames"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="t"></param>
        ///// <returns>受影响的行数</returns>
        //int UpdateDataByID(string idColumnName, string[] columnNames, TEntity t);

        ///// <summary>
        ///// 修改一列数据
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="valueColumnName"></param>
        ///// <param name="clauses"></param>
        ///// <param name="value"></param>
        ///// <returns>受影响的行数</returns>
        //int UpdateData<TValue>(string tableName, string valueColumnName, TValue value, string clauses = null);

        ///// <summary>
        ///// 通过ID修改一行数据
        ///// </summary>
        ///// <typeparam name="TId"></typeparam>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <param name="valueColumnName"></param>
        ///// <param name="value"></param>
        ///// <returns>受影响的行数</returns>
        //int UpdateDataByID<TId, TValue>(string tableName, string idColumnName, TId id, string valueColumnName, TValue value);

        ///// <summary>
        ///// 通过ID修改一行数据
        ///// </summary>
        ///// <typeparam name="TId"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="id"></param>
        ///// <param name="valueColumnName"></param>
        ///// <param name="valueString">值的字符串形式</param>
        ///// <returns>受影响的行数</returns>
        //int UpdateDataByID<TId>(string tableName, string idColumnName, TId id, string valueColumnName, string valueString);

        ///// <summary>
        ///// 异步修改数据
        ///// </summary>
        ///// <param name="columnNames"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> UpdateDataAsync(string[] columnNames, string clauses, TEntity t);

        ///// <summary>
        ///// 异步修改数据
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="columnNames"></param>
        ///// <param name="clauses">子句</param>
        ///// <param name="t"></param>
        ///// <param name="transaction"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> UpdateDataAsync(string tableName, string[] columnNames, string clauses, TEntity t);

        ///// <summary>
        ///// 通过ID异步修改数据
        ///// </summary>
        ///// <param name="columnNames"></param>
        ///// <param name="idColumnName"></param>
        ///// <param name="t"></param>
        ///// <returns>受影响的行数</returns>
        //Task<int> UpdateDataByIDAsync(string idColumnName, string[] columnNames, TEntity t);

        ///// <summary>
        ///// 异步修改一列数据
        ///// </summary>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="tableName"></param>
        ///// <param name="valueColumnName"></param>
        ///// <param name="clauses"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //Task<int> UpdateDataAsync<TValue>(string tableName, string valueColumnName, TValue value, string clauses = null);

        /////// <summary>
        /////// 通过ID修改数据
        /////// </summary>
        /////// <param name="columnNames"></param>
        /////// <param name="expression">ID的表达式</param>
        /////// <param name="t"></param>
        /////// <returns></returns>
        ////int UpdateDataByID(string[] columnNames, Expression<Func<TEntity, object>> expression, TEntity t);

        //#endregion
        #endregion
    }
}
