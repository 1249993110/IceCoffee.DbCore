namespace IceCoffee.DbCore.Dtos
{
    public class PaginationQueryDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量, 值小于 0 时返回所有记录
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string? Order { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool Desc { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 关键词对应的字段名称数组
        /// </summary>
        public string[]? KeywordMappedColumnNames { get; set; }

        /// <summary>
        /// 前置 where 条件字符串
        /// </summary>
        public string? PreWhereBy { get; set; }
    }
}