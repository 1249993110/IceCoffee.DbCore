using PostSharp.Aspects;
using System;

namespace IceCoffee.DbCore.CatchServiceException
{
    /// <summary>
    /// 捕获异步方法异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CatchAsyncExceptionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        public CatchAsyncExceptionAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
            // 确定应用于迭代器或异步方法（编译为状态机）时方面的行为方式
            // 从PostSharp 5.0开始，FlowBehavior也适用于异步方法，ApplyToStateMachine默认为true
            // ApplyToStateMachine = true;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Instance is IExceptionCaught instance)
            {
                if (instance.IsAutoHandleAsyncServiceException)
                {
                    args.FlowBehavior = FlowBehavior.Return;
                    instance.EmitSignal(instance, new ServiceException(ErrorMessage, args.Exception));
                }
                else
                {
                    args.FlowBehavior = FlowBehavior.RethrowException;
                }
            }
        }
    }
}