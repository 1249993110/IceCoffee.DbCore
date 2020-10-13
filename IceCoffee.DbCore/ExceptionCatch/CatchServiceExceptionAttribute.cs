using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 捕获服务层异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CatchServiceExceptionAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 是否自动处理异常条件缓存
        /// </summary>
        private bool? _isAutoHandleException;

        public CatchServiceExceptionAttribute()
        {
        }

        public CatchServiceExceptionAttribute(string errorMessage)
        {
            // 确定应用于迭代器或异步方法（编译为状态机）时方面的行为方式
            // 从PostSharp 5.0开始，FlowBehavior也适用于异步方法，ApplyToStateMachine默认为true
            // ApplyToStateMachine = true;

            ErrorMessage = errorMessage;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if(_isAutoHandleException.HasValue == false)
            {
                object[] attributes = args.Method.GetCustomAttributes(typeof(AutoHandleExceptionAttribute), true);

                if (attributes.Length > 0)
                {
                    _isAutoHandleException = (attributes[0] as AutoHandleExceptionAttribute).IsAutoHandle;
                }
                else
                {
                    _isAutoHandleException = false;
                }
            }

            if (_isAutoHandleException.Value)// 自动处理异常,引发静态事件
            {
                Debug.Assert(args.Instance is IExceptionCaught, "服务必须继承于IExceptionCaught");

                IExceptionCaught instance = args.Instance as IExceptionCaught;// 确定实例继承于IExceptionCaught
                args.FlowBehavior = FlowBehavior.Return;

                instance.EmitSignal(instance, new ServiceException(ErrorMessage, args.Exception));
            }
            else
            {
                throw new ServiceException(ErrorMessage, args.Exception);
            }
        }
    }
}
