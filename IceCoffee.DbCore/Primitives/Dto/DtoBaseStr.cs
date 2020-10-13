namespace IceCoffee.DbCore.Primitives.Dto
{
    public abstract class DtoBaseStr : DtoBase
    {
        /// <summary>
        /// 唯一查询标识，对应表主键
        /// </summary>
        public string Key { get; set; }
    }
}