using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Dto
{
    /// <summary>
    /// 数据传输对象 主键为 int 的实现基类
    /// </summary>
    public abstract class DtoBaseInt : DtoBase
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public int Key { get; set; }
    }
}
