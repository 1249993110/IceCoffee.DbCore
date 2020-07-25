using IceCoffee.Common;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Dto
{
    public abstract class DtoBase<TQuery>
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public TQuery Key { get; set; }
    }
}
