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

namespace IceCoffee.DbCore
{
    internal static class ConnectionFactory<TDbConnection> where TDbConnection : IDbConnection, new()
    {
        //SQLite线程安全条件：各线程不能共用一个数据库连接


        /// <summary>
        /// ThreadLocal连接字典
        /// </summary>
        private static readonly Dictionary<string, ThreadLocal<TDbConnection>> threadLocalConDict;

        /// <summary>
        /// 连接池字典 
        /// </summary>
        private static readonly Dictionary<string, DbConnectionPool<TDbConnection>> connectionPoolDict;

        static ConnectionFactory()
        {
            threadLocalConDict = new Dictionary<string, ThreadLocal<TDbConnection>>();
            connectionPoolDict = new Dictionary<string, DbConnectionPool<TDbConnection>>();
        }

        /// <summary>
        /// 从ThreadLocal连接字典中获得一个数据库连接的存储容器
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        internal static ThreadLocal<TDbConnection> GetConnectionFromThreadLocal(string connectionString)
        {
            if (threadLocalConDict.ContainsKey(connectionString) == false)
            {
                threadLocalConDict.Add(connectionString, new ThreadLocal<TDbConnection>(() =>
                {
                    TDbConnection dbConnection = new TDbConnection
                    {
                        ConnectionString = connectionString
                    };
                    return dbConnection;
                }, true));
            }
            return threadLocalConDict[connectionString];
        }

        /// <summary>
        /// 从连接池中获得一个数据库连接
        /// </summary>
        /// <returns></returns>
        internal static TDbConnection GetConnectionFromPool(string connectionString)
        {
            if (connectionPoolDict.ContainsKey(connectionString) == false)
            {
                var sqliteConnectionPool = new DbConnectionPool<TDbConnection>(connectionString);
                connectionPoolDict.Add(connectionString, sqliteConnectionPool);
            }

            return connectionPoolDict[connectionString].Take();
        }


        /// <summary>
        /// 回收数据库连接到连接池
        /// </summary>
        internal static void CollectDbConnectionToPool(TDbConnection dbConnection)
        {
            if (connectionPoolDict.ContainsKey(dbConnection.ConnectionString))
            {
                connectionPoolDict[dbConnection.ConnectionString].Add(dbConnection);
            }
            else
            {
                dbConnection.Dispose();
            }
        }
    }
}
