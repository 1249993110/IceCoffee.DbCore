namespace IceCoffee.DbCore.Primitives.Dto
{
    public abstract class DtoBase<TQuery> : IDtoBase
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public TQuery Key { get; set; }
    }
}