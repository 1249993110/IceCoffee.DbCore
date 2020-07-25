using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IceCoffee.DbCore.Primitives.Entity;

namespace IceCoffee.DbCore.Primitives.Repository
{
    public abstract class RepositoryBaseStr<TEntity, TDbConnection> : RepositoryBase<TEntity, string>, IDisposable
            where TEntity : EntityBase<string> where TDbConnection : IDbConnection, new()
    {
        #region 构造数据库连接
        public readonly bool _useConnectionPool;

        private readonly ThreadLocal<TDbConnection> _threadLocal;

        private readonly TDbConnection _connection;

        /// <summary>
        /// 表示到数据源的连接
        /// </summary>
        internal protected override sealed IDbConnection Connection
        {
            get
            {
                if (_useConnectionPool == false)
                {
                    return _threadLocal.Value;
                }
                else
                {
                    return _connection;
                }
            }
        }

        public RepositoryBaseStr(string connectionString, bool useConnectionPool)
        {
            _useConnectionPool = useConnectionPool;

            if (_useConnectionPool == false)
            {
                _threadLocal = ConnectionFactory<TDbConnection>.GetConnectionFromThreadLocal(connectionString);
            }
            else
            {
                _connection = ConnectionFactory<TDbConnection>.GetConnectionFromPool(connectionString);
            }
        }

        #region IDisposable Support

        private bool _isDisposed = false; // 检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (_useConnectionPool && _isDisposed == false)
            {
                if (disposing && _useConnectionPool)
                {
                    ConnectionFactory<TDbConnection>.CollectDbConnectionToPool(_connection);
                }
                _isDisposed = true;
            }
        }
        /// <summary>
        /// 回收连接至连接池
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//标记gc不在调用析构函数
        }

        #endregion

        #endregion
    }
}
