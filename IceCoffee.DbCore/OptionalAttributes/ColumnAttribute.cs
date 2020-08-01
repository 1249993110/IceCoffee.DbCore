using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
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