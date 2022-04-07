namespace IceCoffee.DbCore.Primitives.Dto
{
    /// <summary>
    /// 数据传输对象基类
    /// </summary>
    public abstract class DtoBase<TKey> : IDto
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public TKey? Id { get; set; }
    }
}