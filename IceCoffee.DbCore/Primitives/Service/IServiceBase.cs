using System.Collections.Generic;

namespace IceCoffee.DbCore.Primitives.Service
{
    public partial interface IServiceBase<TDto, TQuery>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        int Add(TDto dto);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        int AddBatch(IEnumerable<TDto> dtos);

        /// <summary>
        /// 根据条件和匿名对象删除任意数据
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int RemoveAny(string whereBy, object param = null, bool useTransaction = false);

        /// <summary>
        /// 删除数据
        /// </summary>
        int Remove(TDto dto);

        /// <summary>
        /// 通过ID删除数据
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        int RemoveById<TId>(TId id, string idColumnName);

        /// <summary>
        /// 通过ID获取数据
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        /// <returns></returns>
        List<TDto> GetById<TId>(TId id, string idColumnName);

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<TDto> GetAll(string orderBy = null);

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        long GetRecordCount(string whereBy = null, object param = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="setClause"></param>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dto"></param>
        int Update(TDto dto);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        /// <param name="idColumnName"></param>
        int UpdateById<TId>(TDto dto, TId id, string idColumnName);
    }
}