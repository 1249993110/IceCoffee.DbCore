using IceCoffee.DbCore.Dtos;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// 使用 Task 执行异步操作
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial interface IRepository<TEntity>
    {
        #region Insert

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// 通过表名插入一条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertByTableNameAsync(string tableName, TEntity entity);

        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> InsertBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名插入多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> InsertBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        #endregion Insert

        #region Delete

        /// <summary>
        /// 通过条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(string whereBy, object? param = null, bool useTransaction = false);

        /// <summary>
        /// 通过表名通过条件和匿名对象执行任意删除语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> DeleteByTableNameAsync(string tableName, string whereBy, object? param = null, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键删除记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TEntity entity);

        /// <summary>
        /// 通过表名和默认主键删除记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteByTableNameAsync(string tableName, TEntity entity);

        /// <summary>
        /// 通过默认主键删除多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> DeleteBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名和默认主键删除多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> DeleteBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过ID删除记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync<TId>(string idColumnName, TId id);

        /// <summary>
        /// 通过多个ID删除多条记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="ids"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> DeleteBatchByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids, bool useTransaction = false);

        #endregion Delete

        #region Query

        /// <summary>
        /// 通过条件、顺序字符串和匿名对象执行任意查询语句
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync(string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 通过表名、条件、顺序字符串和匿名对象执行任意查询语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryByTableNameAsync(string tableName, string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 查询关联表的所有记录
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAllAsync(string? orderBy = null);

        /// <summary>
        /// 通过ID获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryByIdAsync<TId>(string idColumnName, TId id);

        /// <summary>
        /// 通过多个ID获取记录
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryByIdsAsync<TId>(string idColumnName, IEnumerable<TId> ids);

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量, 值小于 0 时返回所有记录</param>
        /// <param name="whereBy">where条件字符串, 不能为空字符串""</param>
        /// <param name="orderBy">顺序字符串</param>
        /// <param name="param">带参数的匿名对象</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryPagedAsync(int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null);

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
        Task<IEnumerable<TEntity>> QueryPagedByTableNameAsync(string tableName, int pageIndex, int pageSize, string? whereBy = null, string? orderBy = null, object? param = null);

        /// <summary>
        /// 获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PaginationResultDto<TEntity>> QueryPagedAsync(PaginationQueryDto dto);

        /// <summary>
        /// 通过表名获取与条件匹配的所有记录的分页列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PaginationResultDto<TEntity>> QueryPagedByTableNameAsync(string tableName, PaginationQueryDto dto);

        /// <summary>
        /// 获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> QueryRecordCountAsync(string? whereBy = null, object? param = null);

        /// <summary>
        /// 通过表名获取与条件匹配的所有记录的计数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> QueryRecordCountByTableNameAsync(string tableName, string? whereBy = null, object? param = null);

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
        Task<int> UpdateAsync(string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 通过表名、set子句、条件和匿名对象执行任意更新语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="setClause"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> UpdateByTableNameAsync(string tableName, string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 通过默认主键更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// 通过表名和默认主键更新记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateByTableNameAsync(string tableName, TEntity entity);

        /// <summary>
        /// 通过默认主键更新多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> UpdateBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过表名和默认主键更新多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> UpdateBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 通过ID更新记录
        /// </summary>
        /// <param name="idColumnName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateByIdAsync(string idColumnName, TEntity entity);

        /// <summary>
        /// 通过ID更新记录的一列
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <param name="valueColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> UpdateColumnByIdAsync<TId, TValue>(string idColumnName, TId id, string valueColumnName, TValue value);

        #endregion Update

        #region Other

        /// <summary>
        /// 插入或更新一条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> ReplaceIntoAsync(TEntity entity);

        /// <summary>
        /// 插入或更新多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> ReplaceIntoBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 插入多条记录, 忽略已经存在的冲突记录
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreBatchAsync(IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 插入或更新一条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> ReplaceIntoByTableNameAsync(string tableName, TEntity entity);

        /// <summary>
        /// 插入或更新多条记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> ReplaceIntoBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        /// <summary>
        /// 插入多条记录, 忽略已经存在的冲突记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreBatchByTableNameAsync(string tableName, IEnumerable<TEntity> entities, bool useTransaction = false);

        #endregion Other
    }
}