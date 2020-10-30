using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.UnitWork;
using System.Collections.Generic;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public partial interface IRepositoryBase<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        #region Insert

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        #endregion Insert

        #region Delete

        /// <summary>
        /// 通过条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteAny(string whereBy, object param = null, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 通过默认主键删除多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过Id删除记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// 
        /// <returns></returns>
        int DeleteById<TId>(string idColumnName, TId id);

        /// <summary>
        /// 通过多个Id删除多条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="ids"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteBatchByIds<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false);
        #endregion Delete

        #region Query

        /// <summary>
        /// 通过条件、顺序字符串和匿名对象执行任意查询语句
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="whereBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAny(string columnNames = null, string whereBy = null, string orderBy = null, object param = null);

        /// <summary>
        /// 查询关联表的所有记录
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAll(string orderBy = null);

        /// <summary>
        /// 通过Id获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryById<TId>(string idColumnName, TId id);

        /// <summary>
        /// 通过多个Id获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryByIds<TId>(string idColumnName, IEnumerable<TId> ids);

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="whereBy">where条件字符串</param>
        /// <param name="orderBy">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize, string whereBy = null, string orderBy = null, object param = null);

        /// <summary>
        /// 获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        long QueryRecordCount(string whereBy = null, object param = null);

        #endregion Query

        #region Update

        /// <summary>
        /// 通过set子句、条件和匿名对象执行任意更新语句
        /// </summary>
        /// <param name="setClause"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// 通过默认主键更新多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过Id更新记录
        /// </summary>
        /// <param name="idColumnName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateById(string idColumnName, TEntity entity);

        /// <summary>
        /// 通过Id更新记录的一列
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <param name="valueColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int UpdateColumnById<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value);

        #endregion Update

        /// <summary>
        /// 插入或更新一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="useLock"></param>
        /// <returns></returns>
        int ReplaceInto(TEntity entity, bool useLock = false);

        /// <summary>
        /// 插入或更新多条记录
        /// 先尝试更新如结果为 0，则进行批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <param name="useLock"></param>
        /// <returns></returns>
        int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);

        /// <summary>
        /// 插入多条记录，忽略已经存在的冲突记录
        /// 先通过主键过滤已存在再批量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <param name="useLock"></param>
        /// <returns></returns>
        int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false, bool useLock = false);
    }
}