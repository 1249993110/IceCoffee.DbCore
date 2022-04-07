using IceCoffee.Common.Pools;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
#pragma warning disable

namespace IceCoffee.DbCore
{
    /// <summary>
    /// 数据库连接工厂，为每一个连接串建立一个连接池
    /// </summary>
    public static class DbConnectionFactory
    {
        /// <summary>
        /// 连接池字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, IObjectPool<IDbConnection>> _connectionPoolDict;

        /// <summary>
        /// 覆盖数据库连接池生成器委托
        /// </summary>
        private static Func<string, DbProviderFactory, IObjectPool<IDbConnection>> _dbConnectionPoolGenerator;

        static DbConnectionFactory()
        {
            _connectionPoolDict = new ConcurrentDictionary<string, IObjectPool<IDbConnection>>();
            _dbConnectionPoolGenerator = DefaultDbConnectionPoolGenerator;
        }

        private static IObjectPool<IDbConnection> DefaultDbConnectionPoolGenerator(string connStr, DbProviderFactory factory)
        {
            return new DbConnectionPool(connStr, factory.CreateConnection);
        }

        private static DbProviderFactory GetProvider(DatabaseType databaseType)
        {
            DbProviderFactory factory;
            switch (databaseType)
            {
                case DatabaseType.SQLite:
                    factory = SQLiteFactory.Instance;
                    break;

                case DatabaseType.SQLServer:
                    factory = SqlClientFactory.Instance;
                    break;
                case DatabaseType.Aceess:
                case DatabaseType.PostgreSQL:
                case DatabaseType.MySQL:
                case DatabaseType.Oracle:
                default:
                    throw new ExceptionCatch.DbCoreException("未定义的数据库类型");
            }

            return factory;
        }

        /// <summary>
        /// 覆盖数据库连接池生成器
        /// </summary>
        /// <param name="func"></param>
        public static void OverrideDbConnectionPoolGenerator(Func<string, DbProviderFactory, IObjectPool<IDbConnection>> func)
        {
            _dbConnectionPoolGenerator = func;
        }

        /// <summary>
        /// 根据连接信息获取静态连接池
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <returns></returns>
        public static IObjectPool<IDbConnection> GetDbConnectionPool(DbConnectionInfo dbConnectionInfo)
        {
            string connStr = dbConnectionInfo.ConnectionString;

            if (_connectionPoolDict.TryGetValue(connStr, out IObjectPool<IDbConnection>? pool))
            {
                return pool;
            }
            else
            {
                IObjectPool<IDbConnection> newPool = _dbConnectionPoolGenerator(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                _connectionPoolDict.TryAdd(connStr, newPool);

                return newPool;
            }
        }

        /// <summary>
        /// 清理所有Pool
        /// </summary>
        public static void ClearAll()
        {
            foreach (var connectionPool in _connectionPoolDict.Values)
            {
                if(connectionPool is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _connectionPoolDict.Clear();
        }

        /// <summary>
        /// 从连接池中获得一个数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetConnectionFromPool(DbConnectionInfo dbConnectionInfo)
        {
            string connStr = dbConnectionInfo.ConnectionString;
            if (_connectionPoolDict.TryGetValue(connStr, out IObjectPool<IDbConnection> pool))
            {
                return pool.Get();
            }
            else
            {
                IObjectPool<IDbConnection> newPool = _dbConnectionPoolGenerator(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                _connectionPoolDict.TryAdd(connStr, newPool);

                return newPool.Get();
            }
        }

        /// <summary>
        /// 回收数据库连接到连接池
        /// </summary>
        public static void CollectDbConnectionToPool(IDbConnection dbConnection)
        {
            if (_connectionPoolDict.TryGetValue(dbConnection.ConnectionString, out IObjectPool<IDbConnection> pool))
            {
                pool.Return(dbConnection);
                return;
            }

            dbConnection.Dispose();
        }
    }
}