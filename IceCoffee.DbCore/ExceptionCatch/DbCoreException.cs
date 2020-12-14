using IceCoffee.Common;
using System;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 数据库封装异常
    /// </summary>
    public class DbCoreException : CustomExceptionBase
    {
        public DbCoreException(string message) : base(message)
        {
        }

        public DbCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}