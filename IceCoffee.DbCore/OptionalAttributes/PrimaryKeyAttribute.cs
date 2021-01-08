using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    /// <summary>
    /// 标识主键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}