using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识忽略查询字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreSelectAttribute : Attribute
    {
    }
}