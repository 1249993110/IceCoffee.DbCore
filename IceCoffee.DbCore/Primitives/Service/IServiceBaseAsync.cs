using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public partial interface IServiceBase<TDto, TQuery>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        Task AddAsync(TDto dto);

        /// <summary>
        /// 删除数据
        /// </summary>
        Task RemoveAsync(TDto dto);

        /// <summary>
        /// 删除全部数据
        /// </summary>
        Task RemoveAllAsync();

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<List<TDto>> GetAllAsync(string orderBy = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        Task UpdateAsync(TDto dto);
    }
}