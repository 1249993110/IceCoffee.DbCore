using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Repository;
using IceCoffee.DbCore.Primitives.Service;
using PostSharp.Aspects;
using PostSharp.Constraints;


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
        /// 数据库工作单元
        /// </summary>
        private IUnitOfWork _unitOfWork;

        private IDbConnection _dbConnection;

        public UnitOfWorkAttribute()
        {
            // UnitOfWorkAttribute应比CatchExceptionAttribute优先级低
            AspectPriority = 1;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Debug.Assert(args.Method.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) == null, "工作单元无法标记异步方法");

            ServiceBase service = args.Instance as ServiceBase;

            Debug.Assert(service != null, "服务必须继承ServiceBase");

            _unitOfWork = RepositoryBase.unitWork.Value;

            _unitOfWork.EnterContext(service.DbConnectionInfo);
            
            _dbConnection = _unitOfWork.DbConnection;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            _unitOfWork.SaveChanges();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.RethrowException;

            _unitOfWork.DbTransaction.Rollback();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            ConnectionFactory.CollectDbConnectionToPool(_dbConnection);
        }
    }
}
