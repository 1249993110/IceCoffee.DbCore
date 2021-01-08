using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识忽略更新字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreUpdateAttribute : Attribute
    {
    }
}