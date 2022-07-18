namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识忽略插入特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreInsertAttribute : Attribute
    {
    }
}