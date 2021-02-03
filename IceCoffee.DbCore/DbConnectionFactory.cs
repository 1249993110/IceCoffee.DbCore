
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;

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
        private static readonly ConcurrentDictionary<string, DbConnectionPool> _connectionPoolDict;

        private static Func<string, DbProviderFactory, DbConnectionPool> _dbConnectionPoolGenerator;

        static DbConnectionFactory()
        {
            _connectionPoolDict = new ConcurrentDictionary<string, DbConnectionPool>();
            _dbConnectionPoolGenerator = (connStr, factory) => new DbConnectionPool(connStr, factory);
        }

        private static DbProviderFactory GetProvider(DatabaseType databaseType)
        {
            DbProviderFactory factory = null;
            switch (databaseType)
            {
                case DatabaseType.SQLite:
                    factory = SQLiteFactory.Instance;
                    break;

                case DatabaseType.SQLServer:
                    factory = SqlClientFactory.Instance;
                    break;

                case DatabaseType.MySQL:
                    break;

                case DatabaseType.Oracle:
                    break;

                default:
                    throw new ExceptionCatch.DbCoreException("未定义的数据库类型");
            }
            return factory;
        }

        /// <summary>
        /// 覆盖数据库连接池生成器
        /// </summary>
        /// <param name="func"></param>
        public static void OverrideDbConnectionPoolGenerator(Func<string, DbProviderFactory, DbConnectionPool> func)
        {
            _dbConnectionPoolGenerator = func;
        }

        /// <summary>
        /// 根据连接信息获取静态连接池
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <returns></returns>
        public static DbConnectionPool GetDbConnectionPool(DbConnectionInfo dbConnectionInfo)
        {
            string connStr = dbConnectionInfo.ConnectionString;
            if (_connectionPoolDict.TryGetValue(connStr, out DbConnectionPool pool))
            {
                return pool;
            }
            else
            {
                DbConnectionPool newPool = _dbConnectionPoolGenerator(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                _connectionPoolDict.TryAdd(connStr, newPool);

                return newPool;
            }
        }

        /// <summary>
        /// 清理所有Pool
        /// </summary>
        public static void ClearAll()
        {
            foreach (var item in _connectionPoolDict)
            {
                item.Value.Dispose();
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
            if (_connectionPoolDict.TryGetValue(connStr, out DbConnectionPool pool))
            {
                return pool.Take();
            }
            else
            {
                DbConnectionPool newPool = _dbConnectionPoolGenerator(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                _connectionPoolDict.TryAdd(connStr, newPool);

                return newPool.Take();
            }
        }

        /// <summary>
        /// 回收数据库连接到连接池
        /// </summary>
        public static void CollectDbConnectionToPool(IDbConnection dbConnection)
        {
            if (_connectionPoolDict.TryGetValue(dbConnection.ConnectionString, out DbConnectionPool pool))
            {
                if (pool.Put(dbConnection))
                {
                    return;
                }
            }

            dbConnection.Dispose();
        }
    }
}