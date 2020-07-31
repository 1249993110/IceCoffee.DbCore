﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.UnitWork;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public partial interface IRepositoryBase<TEntity, in TKey> where TEntity : IEntity<TKey>
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
        /// <param name="entitys"></param>
        /// <returns></returns>
        int InsertBatch(IEnumerable<TEntity> entitys);
        #endregion

        #region Delete
        /// <summary>
        /// 根据条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteAny(string whereBy, object param = null, bool useTransaction = false);
        /// <summary>
        /// 根据默认主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);
        /// <summary>
        /// 根据默认主键删除多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int DeleteBatch(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int DeleteById<TId>(TId id, string idColumnName);
        /// <summary>
        /// 根据多个ID删除多条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="ids"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int DeleteBatchByIds<TId>(IEnumerable<TId> ids, string idColumnName);
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
        /// <param name="whereBy"></param>
        /// <param name="orderby"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAny(string columnNames, string whereBy, string orderby, object param = null);
        /// <summary>
        /// 查询关联表的所有记录
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAll(string orderby = null);

        /// <summary>
        /// 根据ID获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryById<TId>(TId id, string idColumnName);
        /// <summary>
        /// 根据多个ID获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="ids"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryByIds<TId>(IEnumerable<TId> ids, string idColumnName);
        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页数量</param>
        /// <param name="whereBy">where条件字符串</param>
        /// <param name="orderby">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryPaged(int pageNumber, int rowsPerPage, string whereBy = null, string orderby = null, object param = null);
        /// <summary>
        /// 获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        long QueryRecordCount(string whereBy = null, object param = null);
        #endregion

        #region Update
        /// <summary>
        /// 根据set子句、条件和匿名对象执行任意更新语句
        /// </summary>
        /// <param name="setClause"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false);
        /// <summary>
        /// 根据默认主键更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);
        /// <summary>
        /// 根据默认主键更新多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int UpdateBatch(IEnumerable<TEntity> entitys);
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
        int UpdateColumnById<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName);
        #endregion       
    }
}
