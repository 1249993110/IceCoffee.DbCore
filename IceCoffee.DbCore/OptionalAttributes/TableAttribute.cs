using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识表名或视图名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 实例化 TableAttribute
        /// </summary>
        /// <param name="name"></param>
        public TableAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 表名或视图名
        /// </summary>
        public string Name { get; private set; }
    }
}