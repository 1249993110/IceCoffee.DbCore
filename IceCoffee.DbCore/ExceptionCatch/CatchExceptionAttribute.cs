using IceCoffee.DbCore.Primitives.Service;
using PostSharp.Aspects;
using System;
using System.Diagnostics;
using System.Linq;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 捕获数据库核心驱动层异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CatchExceptionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 实例化 CatchExceptionAttribute
        /// </summary>
        /// <param name="errorMessage"></param>
        public CatchExceptionAttribute(string errorMessage)
        {
            // 确定应用于迭代器或异步方法（编译为状态机）时方面的行为方式
            // 从PostSharp 5.0开始，FlowBehavior也适用于异步方法，ApplyToStateMachine默认为true
            // ApplyToStateMachine = true;

            ErrorMessage = errorMessage;
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs args)
        {
            throw new DbCoreException(ErrorMessage, args.Exception);
        }
    }
}