using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.CatchServiceException
{
    /// <summary>
    /// 捕获同步方法异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CatchSyncExceptionAttribute : OnExceptionAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        public CatchSyncExceptionAttribute(string error)
        {
            Error = error;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception.GetType() == typeof(ServiceException))
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
            }
            else
            {
                throw new ServiceException(Error, args.Exception);
            }
        }
    }
}
