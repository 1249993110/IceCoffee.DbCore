﻿using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using IceCoffee.DbCore.Primitives;
using PostSharp.Aspects;
using PostSharp.Constraints;


namespace IceCoffee.DbCore.UnitOfWork
{
    /// <summary>
    /// 工作单元，使用AOP切面完成数据库事务操作
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UnitOfWorkAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 数据库操作会话
        /// </summary>
        private DbSession dbSession;

        public UnitOfWorkAttribute()
        {
            // UnitOfWorkAttribute应比CatchExceptionAttribute优先级低
            AspectPriority = 1;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            Debug.Assert(args.Arguments.Count > 0, "仓储接口参数个数应大于0");

            dbSession = args.Arguments[args.Arguments.Count - 1] as DbSession;

            Debug.Assert(dbSession != null, "数据库操作会话不能为空");

            (args.Instance as Primitives.Repository.IRepositoryBase).GetThreadLocal == true

            try
            {
                dbSession.Connection.Open();
                dbSession.transaction = dbSession.connection.BeginTransaction();
            }
            catch (Exception e)
            {
                throw new Exception("数据库事务操作异常", e);
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            dbSession.Transaction.Commit();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            dbSession.Transaction.Rollback();
            args.FlowBehavior = FlowBehavior.RethrowException;
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            dbSession.Transaction = null;
        }
    }
}
