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
        void Insert(TDto dto);

        /// <summary>
        /// 删除数据
        /// </summary>
        void Remove(TDto dto);

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
        /// 获取全部记录条数
        /// </summary>
        /// <returns></returns>
        long GetRecordCount();

        /// <summary>
        /// 更新数据
        /// </summary>
        void Update(TDto dto);
    }
}
