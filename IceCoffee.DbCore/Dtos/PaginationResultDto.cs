using System.Collections;

namespace IceCoffee.DbCore.Dtos
{
    public class PaginationResultDto
    {
        /// <summary>
        /// 总计
        /// </summary>
        public uint Total { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public IEnumerable? Items { get; set; }
    }
}