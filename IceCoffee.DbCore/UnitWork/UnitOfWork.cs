﻿using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.ExceptionCatch;
using System.Data;

namespace IceCoffee.DbCore.UnitWork
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isExplicitSubmit;

        private IDbConnection _dbConnection;

        internal IDbTransaction _dbTransaction;
        /// <inheritdoc />
        public bool IsExplicitSubmit => _isExplicitSubmit;
        /// <inheritdoc />
        public IDbConnection DbConnection => _dbConnection;
        /// <inheritdoc />
        public IDbTransaction DbTransaction => _dbTransaction;
        /// <inheritdoc />
        public virtual void EnterContext(DbConnectionInfo dbConnectionInfo)
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
        }
        /// <inheritdoc />
        public virtual void SaveChanges()
        {
            // 防止多次执行或跨线程使用
            if (_isExplicitSubmit == true)
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
            if (_isExplicitSubmit == true)
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
    }
}