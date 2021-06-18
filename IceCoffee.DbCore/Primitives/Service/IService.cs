using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Repository;
using System.Collections.Generic;

namespace IceCoffee.DbCore.Primitives.Service
{
    /// <summary>
    /// 服务模板接口
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public partial interface IService<TDto> where TDto : IDto
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        TDto Add(TDto dto);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        List<TDto> AddBatch(IEnumerable<TDto> dtos, bool useTransaction = false);

        /// <summary>
        /// 根据条件和匿名对象删除任意数据
        /// </summary>
        /// <param name="whereBy"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        int Remove(string whereBy, object param = null, bool useTransaction = false);

        /// <summary>
        /// 删除数据
        /// </summary>
        int Remove(TDto dto);

        /// <summary>
        /// 通过ID删除数据
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        int RemoveById<TId>(string idColumnName, TId id);

        /// <summary>
        /// 通过ID获取数据
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="idColumnName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        List<TDto> GetById<TId>(string idColumnName, TId id);

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
        int Update(string setClause, string whereBy, object param, bool useTransaction = false);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="dto"></param>
        int Update(TDto dto);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="idColumnName"></param>
        /// <param name="dto"></param>
        int UpdateById(string idColumnName, TDto dto);
    }
}