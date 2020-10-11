using IceCoffee.DbCore.Domain;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;

namespace IceCoffee.DbCore
{
    /// <summary>
    /// 数据库连接工厂
    /// </summary>
    public static class DbConnectionFactory
    {
        /// <summary>
        /// 连接池字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, DbConnectionPool> connectionPoolDict;

        static DbConnectionFactory()
        {
            connectionPoolDict = new ConcurrentDictionary<string, DbConnectionPool>();
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
                    Debug.Assert(false, "未定义的数据库类型");
                    break;
            }
            return factory;
        }

        /// <summary>
        /// 根据连接信息获取静态连接池
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnectionPool GetDbConnectionPool(DbConnectionInfo dbConnectionInfo)
        {
            string connStr = dbConnectionInfo.ConnectionString;
            if (connectionPoolDict.TryGetValue(connStr, out DbConnectionPool pool))
            {
                return pool;
            }
            else
            {
                DbConnectionPool newPool = new DbConnectionPool(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                connectionPoolDict.TryAdd(connStr, newPool);

                return newPool;
            }
        }

        /// <summary>
        /// 从连接池中获得一个数据库连接
        /// </summary>
        /// <returns></returns>
        internal static IDbConnection GetConnectionFromPool(DbConnectionInfo dbConnectionInfo)
        {
            string connStr = dbConnectionInfo.ConnectionString;
            if (connectionPoolDict.TryGetValue(connStr, out DbConnectionPool pool))
            {
                return pool.Take();
            }
            else
            {
                DbConnectionPool newPool = new DbConnectionPool(connStr, GetProvider(dbConnectionInfo.DatabaseType));
                connectionPoolDict.TryAdd(connStr, newPool);

                return newPool.Take();
            }
        }

        /// <summary>
        /// 回收数据库连接到连接池
        /// </summary>
        internal static void CollectDbConnectionToPool(IDbConnection dbConnection)
        {
            if (connectionPoolDict.TryGetValue(dbConnection.ConnectionString, out DbConnectionPool pool))
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