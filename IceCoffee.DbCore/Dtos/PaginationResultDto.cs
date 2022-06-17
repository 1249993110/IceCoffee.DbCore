using System.Collections;
using System.Collections.Generic;

namespace IceCoffee.DbCore.Dtos
{
    public class PaginationResultDto<T>
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public IEnumerable<T>? Items { get; set; }
    }
}