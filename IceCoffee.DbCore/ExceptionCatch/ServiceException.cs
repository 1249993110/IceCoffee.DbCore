using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 服务层封装异常
    /// </summary>
    public class ServiceException : CustomExceptionBase
    {
        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
