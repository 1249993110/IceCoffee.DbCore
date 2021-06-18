using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Repository;
using IceCoffee.DbCore.Primitives.Service;
using PostSharp.Aspects;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IceCoffee.DbCore.UnitWork
{
    /// <summary>
    /// 工作单元，使用AOP切面完成数据库事务操作
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UnitOfWorkAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 实例化 UnitOfWorkAttribute
        /// </summary>
        public UnitOfWorkAttribute()
        {
            // UnitOfWorkAttribute应比CatchExceptionAttribute优先级低
            AspectPriority = 1;
        }
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs args)
        {
            ServiceBase service = args.Instance as ServiceBase;

            if (service == null)
            {
                throw new DbCoreException("服务必须继承 ServiceBase");
            }

            UnitOfWork.Default.EnterContext(service.DbConnectionInfo);
        }
        /// <inheritdoc />
        public override void OnSuccess(MethodExecutionArgs args)
        {
            UnitOfWork.Default.SaveChanges();
        }
        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.RethrowException;

            UnitOfWork.Default.Rollback();
        }
    }
}