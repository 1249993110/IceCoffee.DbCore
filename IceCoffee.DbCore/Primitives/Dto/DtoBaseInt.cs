using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Dto
{
    public abstract class DtoBaseInt : DtoBase
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public int Key { get; set; }
    }
}
