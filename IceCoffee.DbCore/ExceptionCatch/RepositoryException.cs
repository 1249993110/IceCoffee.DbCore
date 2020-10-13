using IceCoffee.Common;
using System;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 仓储层封装异常
    /// </summary>
    public class RepositoryException : CustomExceptionBase
    {
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}