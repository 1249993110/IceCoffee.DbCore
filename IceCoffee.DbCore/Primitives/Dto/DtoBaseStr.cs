namespace IceCoffee.DbCore.Primitives.Dto
{
    /// <summary>
    /// 数据传输对象 主键为 string 的实现基类
    /// </summary>
    public abstract class DtoBaseStr : DtoBase
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public string Key { get; set; }
    }
}