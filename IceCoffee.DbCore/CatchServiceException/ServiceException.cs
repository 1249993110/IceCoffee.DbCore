using IceCoffee.Common;
using System;

namespace IceCoffee.DbCore.CatchServiceException
{
    public class ServiceException : CustomExceptionBase
    {
        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}