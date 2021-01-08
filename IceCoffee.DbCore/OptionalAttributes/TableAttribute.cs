using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识表名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 实例化 TableAttribute
        /// </summary>
        /// <param name="tableName"></param>
        public TableAttribute(string tableName)
        {
            Name = tableName;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; private set; }
    }
}