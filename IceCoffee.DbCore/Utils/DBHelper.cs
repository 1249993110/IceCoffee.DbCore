using Dapper;

using IceCoffee.DbCore.ExceptionCatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace IceCoffee.DbCore.Utils
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 创建SQLite数据库
        /// </summary>
        public static void CreateSQLiteDB(string path)
        {
            SQLiteConnection.CreateFile(path);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyList"></param>
        public static void CreateTable(DbConnectionInfo dbConnectionInfo, string tableName, string[] propertyList)
        {
            try
            {
                var connection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                connection.Execute(string.Format("CREATE TABLE IF NOT EXISTS {0}({1})", tableName, string.Join(",", propertyList)));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("创建表异常", ex);
            }
        }

        /// <summary>
        /// 删除SQLite数据库
        /// </summary>
        public static void DeleteSQLiteDB(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="tableName"></param>
        public static void DropTable(DbConnectionInfo dbConnectionInfo, string tableName)
        {
            try
            {
                var connection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
                connection.Execute(string.Format("DROP TABLE IF EXISTS {0}", tableName));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("删除表异常", ex);
            }
        }

        /// <summary>
        /// 从连接池中获取数据库连接，以执行sql语句
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static int ExecuteSql(DbConnectionInfo dbConnectionInfo, string sql, object? param = null)
        {
            IDbConnection connection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
            try
            {
                return connection.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("执行 SQL 语句异常", ex);
            }
            finally
            {
                if (connection != null)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(connection);
                }
            }
        }

        /// <summary>
        /// 从连接池中获取数据库连接，以执行sql语句
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static IEnumerable<TEntity> QueryAny<TEntity>(DbConnectionInfo dbConnectionInfo, string sql, object? param = null)
        {
            IDbConnection connection = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
            try
            {
                return connection.Query<TEntity>(sql, param);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("执行 SQL 语句异常", ex);
            }
            finally
            {
                if (connection != null)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(connection);
                }
            }
        }
    }
}