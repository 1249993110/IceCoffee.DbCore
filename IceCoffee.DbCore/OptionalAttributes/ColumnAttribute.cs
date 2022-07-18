namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识列名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 实例化 ColumnAttribute
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnAttribute(string columnName)
        {
            Name = columnName;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }
    }
}