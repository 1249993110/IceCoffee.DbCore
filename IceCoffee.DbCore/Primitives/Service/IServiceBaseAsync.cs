using IceCoffee.DbCore.Primitives.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public partial interface IServiceBase<TDto> : IServiceBase where TDto : IDtoBase
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        Task<TDto> AddAsync(TDto dto);

        /// <summary>
        /// 删除数据
        /// </summary>
        Task<int> RemoveAsync(TDto dto);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <returns></returns>
        Task<int> RemoveAllAsync();

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<List<TDto>> GetAllAsync(string orderBy = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        Task<int> UpdateAsync(TDto dto);
    }
}