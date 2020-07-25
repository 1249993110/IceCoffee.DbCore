
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public partial interface IServiceBase<TDto, TQuery>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        Task InsertAsync(TDto dto);

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
