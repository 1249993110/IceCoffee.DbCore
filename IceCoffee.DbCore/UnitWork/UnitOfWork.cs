﻿using IceCoffee.DbCore.ExceptionCatch;
using System;
using System.Data;
using System.Threading;

namespace IceCoffee.DbCore.UnitWork
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        /// <inheritdoc />
        protected bool _isExplicitSubmit;
        /// <inheritdoc />
        protected System.Timers.Timer _timer;
        /// <inheritdoc />
        protected IDbConnection _dbConnection;
        /// <inheritdoc />
        protected IDbTransaction _dbTransaction;

        /// <inheritdoc />
        public bool IsExplicitSubmit => _isExplicitSubmit;
        /// <inheritdoc />
        public IDbConnection DbConnection => _dbConnection;
        /// <inheritdoc />
        public IDbTransaction DbTransaction => _dbTransaction;

        /// <summary>
        /// 实例化工作单元
        /// </summary>
        /// <param name="maxHoldTime">工作单元最大被持有时长（单位：毫秒）默认值：10000 毫秒</param>
        /// <remarks>
        /// 工作单元状态检查使用 <see cref="System.Timers.Timer"/>, 其精确度大约为 50 毫秒
        /// </remarks>
        public UnitOfWork(double maxHoldTime = 10000)
        {
            _timer = new System.Timers.Timer(maxHoldTime);
            _timer.Elapsed += OnForceEnd;
            _timer.AutoReset = false;
        }

        /// <inheritdoc />
        private void OnForceEnd(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_isExplicitSubmit)
            {
                ForceEnd();
            }
        }

        /// <inheritdoc />
        public virtual IUnitOfWork EnterContext(DbConnectionInfo dbConnectionInfo)
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit == false)
            {
                _isExplicitSubmit = true;
                _dbConnection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                _dbTransaction = _dbConnection.BeginTransaction();
                _timer.Start();
            }
            else
            {
                throw new DbCoreException(string.Format("多次执行 {0} 或 跨线程使用工作单元", nameof(EnterContext)));
            }

            return this;
        }

        /// <inheritdoc />
        public virtual void SaveChanges()
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit)
            {
                _timer.Stop();
                _isExplicitSubmit = false;

                _dbTransaction.Commit();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
            else
            {
                throw new DbCoreException(string.Format("设计错误，多次执行 {0} 或 跨线程使用工作单元", nameof(SaveChanges)));
            }
        }
        /// <inheritdoc />
        public virtual void Rollback()
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit)
            {
                _timer.Stop();
                _isExplicitSubmit = false;

                _dbTransaction.Rollback();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
            else
            {
                throw new DbCoreException(string.Format("设计错误，多次执行 {0} 或 跨线程使用工作单元", nameof(Rollback)));
            }
        }

        /// <summary>
        /// 当工作单元被持有超时时将被强制结束
        /// </summary>
        protected virtual void ForceEnd()
        {
            _isExplicitSubmit = false;

            _dbTransaction.Rollback();
            _dbTransaction.Dispose();
            _dbTransaction = null;

            _dbConnection.Dispose();
            _dbConnection = null;
        }
        #region 默认实现

        private static readonly object _singleton_Lock = new object();

        private static ThreadLocal<IUnitOfWork> _threadLocal;

        /// <summary>
        /// 获得默认工作单元实例在当前线程的实例值
        /// </summary>
        public static IUnitOfWork Default 
        {
            get
            {
                if (_threadLocal == null) // 双if + lock
                {
                    lock (_singleton_Lock)
                    {
                        if (_threadLocal == null)
                        {
                            _threadLocal = new ThreadLocal<IUnitOfWork>(() =>
                            {
                                return new UnitOfWork();
                            });
                        }
                    }
                }

                return _threadLocal.Value;
            }
        } 

        /// <summary>
        /// 覆盖默认工作单元
        /// </summary>
        /// <param name="func"></param>
        public static void OverrideUnitOfWork(Func<IUnitOfWork> func)
        {
            _threadLocal = new ThreadLocal<IUnitOfWork>(func);
        }

        #endregion
    }
}