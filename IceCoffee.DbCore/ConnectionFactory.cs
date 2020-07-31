using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Utils;
using System.Data;
using System.Data.SQLite;
using System.Collections;
using IceCoffee.DbCore.Primitives;
using System.Threading;
using System.Data.Common;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using IceCoffee.DbCore.Domain;
using System.Diagnostics;

namespace IceCoffee.DbCore
{
    internal static class ConnectionFactory
    {
        /// <summary>
        /// 连接池字典 
        /// </summary>
        private static readonly ConcurrentDictionary<string, DbConnectionPool> connectionPoolDict;

        static ConnectionFactory()
        {
            connectionPoolDict = new ConcurrentDictionary<string, DbConnectionPool>();
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
                DbProviderFactory factory = null;
                switch (dbConnectionInfo.DatabaseType)
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

                DbConnectionPool newPool = new DbConnectionPool(connStr, factory);
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
                if(pool.Put(dbConnection))
                {
                    return;
                }
            }

            dbConnection.Dispose();
        }
    }
}
