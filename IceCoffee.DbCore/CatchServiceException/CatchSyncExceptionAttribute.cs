using PostSharp.Aspects;
using System;

namespace IceCoffee.DbCore.CatchServiceException
{
    /// <summary>
    /// 捕获同步方法异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CatchSyncExceptionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        public CatchSyncExceptionAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception.GetType() == typeof(ServiceException))
            {
                args.FlowBehavior = FlowBehavior.RethrowException;
            }
            else
            {
                throw new ServiceException(ErrorMessage, args.Exception);
            }
        }
    }
}