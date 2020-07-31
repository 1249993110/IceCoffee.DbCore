using IceCoffee.DbCore.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.UnitWork
{
    public class UnitOfWork : IUnitOfWork
    {        
        private bool _useSameUnitOfWork;

        private IDbConnection _dbConnection;

        internal IDbTransaction _dbTransaction;

        public bool UseUnitOfWork => _useSameUnitOfWork;

        public IDbConnection DbConnection { get => _dbConnection; set => _dbConnection = value; }

        public IDbTransaction DbTransaction => _dbTransaction;

        public virtual void EnterContext(DbConnectionInfo dbConnectionInfo)
        {
            // 防止多次执行
            if (_useSameUnitOfWork == false)
            {
                _useSameUnitOfWork = true;
                _dbConnection = ConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }

        public virtual void SaveChanges()
        {
            // 防止多次执行
            if(_useSameUnitOfWork == true)
            {
                _useSameUnitOfWork = false;
                _dbTransaction.Commit();

                _dbConnection = null;
                _dbTransaction = null;
            }
        }
    }
}
