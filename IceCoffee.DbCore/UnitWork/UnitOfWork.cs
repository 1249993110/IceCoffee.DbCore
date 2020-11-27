using IceCoffee.DbCore.Domain;
using System.Data;

namespace IceCoffee.DbCore.UnitWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _isExplicitSubmit;

        private IDbConnection _dbConnection;

        internal IDbTransaction _dbTransaction;

        public bool IsExplicitSubmit => _isExplicitSubmit;

        public IDbConnection DbConnection => _dbConnection;

        public IDbTransaction DbTransaction => _dbTransaction;

        public virtual void EnterContext(DbConnectionInfo dbConnectionInfo)
        {
            // 防止多次执行
            if (_isExplicitSubmit == false)
            {
                _isExplicitSubmit = true;
                _dbConnection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }

        public virtual void SaveChanges()
        {
            // 防止多次执行
            if (_isExplicitSubmit == true)
            {
                _isExplicitSubmit = false;

                _dbTransaction.Commit();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
        }

        public virtual void Rollback()
        {
            // 防止多次执行
            if (_isExplicitSubmit == true)
            {
                _isExplicitSubmit = false;

                _dbTransaction.Rollback();
                _dbTransaction.Dispose();
                _dbTransaction = null;

                DbConnectionFactory.CollectDbConnectionToPool(_dbConnection);
                _dbConnection = null;
            }
        }
    }
}