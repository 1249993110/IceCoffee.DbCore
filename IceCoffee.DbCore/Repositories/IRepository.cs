using IceCoffee.DbCore.Dtos;

namespace IceCoffee.DbCore.Repositories
{
    public partial interface IRepository<TEntity>
    {
        #region Insert

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 通过表名插入一条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        int InsertByTableName(string tableName, TEntity entity);

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int InsertBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名插入多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int InsertBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        #endregion Insert

        #region Delete

        /// <summary>
        /// 通过条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int Delete(string whereBy, object? param = null, bool useTransaction = false);

        /// <summary>
        /// 通过表名、条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteByTableName(string tableName, string whereBy, object? param = null, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 通过表名和默认主键删除记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteByTableName(string tableName, TEntity entity);

        /// <summary>
        /// 通过默认主键删除多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名和默认主键删除多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int DeleteBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

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
        /// <param name="whereBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Query(string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 通过表名、条件、顺序字符串和匿名对象执行任意查询语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryByTableName(string tableName, string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 查询关联表的所有记录
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryAll(string? orderBy = null);

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
        /// <param name="pageSize">每页数量, 值小于 0 时返回所有记录</param>
        /// <param name="whereBy">where条件字符串, 不能为空字符串""</param>
        /// <param name="orderBy">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryPaged(int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 通过表名获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="tableName">页码</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量, 值小于 0 时返回所有记录</param>
        /// <param name="whereBy">where条件字符串, 不能为空字符串""</param>
        /// <param name="orderBy">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        IEnumerable<TEntity> QueryPagedByTableName(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="keywordMappedColumnNames">关键词对应的字段名称数组</param>
        /// <returns></returns>
        PaginationResultDto<TEntity> QueryPaged(PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        /// <summary>
        /// 通过表名获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dto"></param>
        /// <param name="keywordMappedColumnNames">关键词对应的字段名称数组</param>
        /// <returns></returns>
        PaginationResultDto<TEntity> QueryPagedByTableName(string tableName, PaginationQueryDto dto, params string[] keywordMappedColumnNames);

        /// <summary>
        /// 获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int QueryRecordCount(string? whereBy = null, object? param = null);

        /// <summary>
        /// 通过表名获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int QueryRecordCountByTableName(string tableName, string? whereBy = null, object? param = null);

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
        int Update(string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 通过表名、set子句、条件和匿名对象执行任意更新语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="setClause"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateByTableName(string tableName, string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        /// <summary>
        /// 通过表名、默认主键更新记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateByTableName(string tableName, TEntity entity);

        /// <summary>
        /// 通过默认主键更新多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名、默认主键更新多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

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

        #region Other

        /// <summary>
        /// 插入或更新一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int ReplaceInto(TEntity entity);

        /// <summary>
        /// 插入或更新多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int ReplaceIntoBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 插入多条记录, 忽略已经存在的冲突记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int InsertIgnoreBatch(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名插入或更新一条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        int ReplaceIntoByTableName(string tableName, TEntity entity);

        /// <summary>
        /// 通过表名插入或更新多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int ReplaceIntoBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名插入多条记录, 忽略已经存在的冲突记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int InsertIgnoreBatchByTableName(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        #endregion Other
    }
}