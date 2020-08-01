using System;

namespace IceCoffee.DbCore.OptionalAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreInsertAttribute : Attribute
    {
    }
}