
using IceCoffee.DbCore.ExceptionCatch;
using System;
using System.Data;
using System.Threading;

namespace IceCoffee.DbCore.UnitWork
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isExplicitSubmit;

        private IDbConnection _dbConnection;

        private IDbTransaction _dbTransaction;
        /// <inheritdoc />
        public bool IsExplicitSubmit => _isExplicitSubmit;
        /// <inheritdoc />
        public IDbConnection DbConnection => _dbConnection;
        /// <inheritdoc />
        public IDbTransaction DbTransaction => _dbTransaction;
        /// <inheritdoc />
        public virtual IUnitOfWork EnterContext(DbConnectionInfo dbConnectionInfo)
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit == false)
            {
                _isExplicitSubmit = true;
                _dbConnection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                _dbTransaction = _dbConnection.BeginTransaction();
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
                _isExplicitSubmit = false;

                _dbTransaction.Commit();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
            else
            {
                throw new DbCoreException(string.Format("多次执行 {0} 或 跨线程使用工作单元", nameof(SaveChanges)));
            }
        }
        /// <inheritdoc />
        public virtual void Rollback()
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit)
            {
                _isExplicitSubmit = false;

                _dbTransaction.Rollback();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
            else
            {
                throw new DbCoreException(string.Format("多次执行 {0} 或 跨线程使用工作单元", nameof(Rollback)));
            }
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