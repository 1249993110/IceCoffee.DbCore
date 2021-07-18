using IceCoffee.DbCore.ExceptionCatch;
using System;
using System.Data;
using System.Threading;

namespace IceCoffee.DbCore.UnitWork
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        /// <inheritdoc />
        protected readonly System.Timers.Timer timer;
        /// <inheritdoc />
        protected bool isExplicitSubmit;
        /// <inheritdoc />
        protected IDbConnection dbConnection;
        /// <inheritdoc />
        protected IDbTransaction dbTransaction;

        /// <inheritdoc />
        public bool IsExplicitSubmit => isExplicitSubmit;
        /// <inheritdoc />
        public IDbConnection DbConnection => dbConnection;
        /// <inheritdoc />
        public IDbTransaction DbTransaction => dbTransaction;

        /// <summary>
        /// 实例化工作单元
        /// </summary>
        /// <param name="maxHoldTime">工作单元最大被持有时长（单位：毫秒）默认值：10000 毫秒</param>
        /// <remarks>
        /// 工作单元状态检查使用 <see cref="System.Timers.Timer"/>, 其精确度大约为 50 毫秒
        /// </remarks>
        public UnitOfWork(double maxHoldTime = 10000)
        {
            timer = new System.Timers.Timer(maxHoldTime);
            timer.Elapsed += OnForceEnd;
            timer.AutoReset = false;
        }

        /// <inheritdoc />
        private void OnForceEnd(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (dbConnection != null)
            {
                ForceEnd();
            }
        }

        /// <inheritdoc />
        public virtual IUnitOfWork EnterContext()
        {
            // 防止多次执行或跨线程使用
            if (dbConnection == null)
            {
                isExplicitSubmit = true;
            }
            else
            {
                throw new DbCoreException(string.Format("多次执行 {0} 或 跨线程使用工作单元", nameof(EnterContext)));
            }

            return this;
        }

        /// <inheritdoc />
        public virtual IUnitOfWork EnterContext(DbConnectionInfo dbConnectionInfo)
        {
            // 防止多次执行或跨线程使用
            if (dbConnection == null)
            {
                isExplicitSubmit = true;
                dbConnection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                dbTransaction = dbConnection.BeginTransaction();
                timer.Start();
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
            if (dbConnection != null)
            {
                isExplicitSubmit = false;
                timer.Stop();

                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(dbConnection);
                dbConnection = null;
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
            if (dbConnection != null)
            {
                isExplicitSubmit = false;
                timer.Stop();

                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(dbConnection);
                dbConnection = null;
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
            isExplicitSubmit = false;

            dbTransaction.Rollback();
            dbTransaction.Dispose();
            dbTransaction = null;

            dbConnection.Dispose();
            dbConnection = null;
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