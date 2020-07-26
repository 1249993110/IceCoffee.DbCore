//using IceCoffee.DbCore.Primitives.Entity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IceCoffee.DbCore.Primitives.Repository
//{
//    /// <summary>
//    /// 使用Task.Run的执行异步操作
//    /// </summary>
//    /// <typeparam name="TEntity"></typeparam>
//    /// <typeparam name="TKey"></typeparam>
//    public partial interface IRepositoryBase<TEntity, in TKey> where TEntity : IEntity<TKey>
//    {
//        #region Insert
//        /// <summary>
//        /// 插入一条记录
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        Task<int> InsertOneAsync(TEntity entity);
//        /// <summary>
//        /// 插入多条记录
//        /// </summary>
//        /// <param name="entitys"></param>
//        /// <returns></returns>
//        Task<int> InsertListAsync(IEnumerable<TEntity> entitys);
//        #endregion

//        #region Delete
//        /// <summary>
//        /// 根据条件和匿名对象执行任意删除语句
//        /// </summary>
//        /// <param name="conditions"></param>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        Task<int> DeleteAnyAsync(string conditions, object param = null);
//        /// <summary>
//        /// 根据默认主键删除记录
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        Task<int> DeleteOneAsync(TEntity entity);
//        /// <summary>
//        /// 根据默认主键删除多条记录
//        /// </summary>
//        /// <param name="entitys"></param>
//        /// <returns></returns>
//        Task<int> DeleteListAsync(IEnumerable<TEntity> entitys);
//        /// <summary>
//        /// 根据主键删除记录
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        Task<int> DeleteOneByKeyAsync(TKey key);
//        /// <summary>
//        /// 根据多个主键删除多条记录
//        /// </summary>
//        /// <param name="keys"></param>
//        /// <returns></returns>
//        Task<int> DeleteListByKeysAsync(IEnumerable<TKey> keys);
//        /// <summary>
//        /// 根据ID删除记录
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="id"></param>
//        /// <param name="idColumnName"></param>
//        /// <returns></returns>
//        Task<int> DeleteOneByIdAsync<TId>(TId id, string idColumnName);
//        /// <summary>
//        /// 根据多个ID删除多条记录
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="ids"></param>
//        /// <param name="idColumnName"></param>
//        /// <returns></returns>
//        Task<int> DeleteListByIdsAsync<TId>(IEnumerable<TId> ids, string idColumnName);
//        /// <summary>
//        /// 删除关联表的所有记录
//        /// </summary>
//        /// <returns></returns>
//        Task<int> DeleteAllAsync();
//        #endregion

//        #region Query
//        /// <summary>
//        /// 根据条件、顺序字符串和匿名对象执行任意查询语句
//        /// </summary>
//        /// <param name="columnNames"></param>
//        /// <param name="conditions"></param>
//        /// <param name="orderby"></param>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        Task<IEnumerable<TEntity>> QueryAnyAsync(string columnNames, string conditions, string orderby, object param = null);
//        /// <summary>
//        /// 根据主键获取一条记录
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        Task<TEntity> QueryOneByKeyAsync(TKey key);
//        /// <summary>
//        /// 根据多个主键获取多条记录
//        /// </summary>
//        /// <param name="keys"></param>
//        /// <returns></returns>
//        Task<IEnumerable<TEntity>> QueryListByKeysAsync(IEnumerable<TKey> keys);
//        /// <summary>
//        /// 查询关联表的所有记录
//        /// </summary>
//        /// <returns></returns>
//        Task<IEnumerable<TEntity>> QueryAllAsync(string orderby = null);
//        /// <summary>
//        /// 根据ID获取一条记录
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="id"></param>
//        /// <param name="idColumnName"></param>
//        /// <returns></returns>
//        Task<TEntity> QueryOneByIdAsync<TId>(TId id, string idColumnName);
//        /// <summary>
//        /// 根据ID获取多条记录
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="id"></param>
//        /// <param name="idColumnName"></param>
//        /// <returns></returns>
//        Task<IEnumerable<TEntity>> QueryListByIdAsync<TId>(TId id, string idColumnName);
//        /// <summary>
//        /// 获取与条件匹配的所有记录的分页列表
//        /// </summary>
//        /// <param name="pageNumber">页码</param>
//        /// <param name="rowsPerPage">每页数量</param>
//        /// <param name="conditions">where条件字符串</param>
//        /// <param name="orderby">顺序字符串</param>
//        /// <param name="param">带参数的匿名对象</param>
//        /// <returns></returns>
//        Task<IEnumerable<TEntity>> QueryListPagedAsync(int pageNumber, int rowsPerPage, string conditions = null, string orderby = null, object param = null);
//        /// <summary>
//        /// 获取与条件匹配的所有记录的计数
//        /// </summary>
//        /// <param name="conditions"></param>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        Task<long> QueryRecordCountAsync(string conditions = null, object param = null);
//        #endregion

//        #region Update
//        /// <summary>
//        /// 根据set子句、条件和匿名对象执行任意更新语句
//        /// </summary>
//        /// <param name="conditions"></param>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        Task<int> UpdateAnyAsync(string setClause, string conditions, object param);
//        /// <summary>
//        /// 根据默认主键更新记录
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        Task<int> UpdateOneAsync(TEntity entity);
//        /// <summary>
//        /// 根据默认主键更新多条记录
//        /// </summary>
//        /// <param name="entitys"></param>
//        /// <returns></returns>
//        Task<int> UpdateListAsync(IEnumerable<TEntity> entitys);
//        /// <summary>
//        /// 根据ID更新记录
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="entity"></param>
//        /// <param name="id"></param>
//        /// <param name="idColumnName"></param>
//        /// <returns></returns>
//        Task<int> UpdateByIdAsync<TId>(TEntity entity, TId id, string idColumnName);
//        /// <summary>
//        /// 根据ID更新记录的一列
//        /// </summary>
//        /// <typeparam name="TId"></typeparam>
//        /// <typeparam name="TValue"></typeparam>
//        /// <param name="id"></param>
//        /// <param name="value"></param>
//        /// <param name="idColumnName"></param>
//        /// <param name="valueColumnName"></param>
//        /// <returns></returns>
//        Task<int> UpdateOneColByIdAsync<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName);
//        #endregion

//    }
//}
