﻿using Dapper;
using IceCoffee.DbCore.ExceptionCatch;
using System.Data;

namespace IceCoffee.DbCore.Utils
{
    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 从连接池中获取数据库连接, 以执行任意sql语句
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        public static int ExecuteAny(DbConnectionInfo dbConnectionInfo, string sql, object? param = null, bool useTransaction = false)
        {
            var conn = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
            var tran = useTransaction ? conn.BeginTransaction() : null;

            try
            {
                int result = conn.Execute(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in DBHelper.ExecuteAny", ex);
            }
            finally
            {
                if (conn != null)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 从连接池中获取数据库连接, 以执行任意查询sql语句
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static IEnumerable<TEntity> QueryAny<TEntity>(DbConnectionInfo dbConnectionInfo, string sql, object? param = null)
        {
            var conn = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
            try
            {
                return conn.Query<TEntity>(sql, param);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in DBHelper.QueryAny", ex);
            }
            finally
            {
                if (conn != null)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 从连接池中获取数据库连接, 执行参数化 SQL 语句, 选择单个值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        public static TReturn ExecuteScalar<TReturn>(DbConnectionInfo dbConnectionInfo, string sql, object? param = null, bool useTransaction = false)
        {
            var conn = DbConnectionFactory.GetConnectionFromPool(dbConnectionInfo);
            var tran = useTransaction ? conn.BeginTransaction() : null;

            try
            {
                var result = conn.ExecuteScalar<TReturn>(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in DBHelper.ExecuteScalar", ex);
            }
            finally
            {
                DbConnectionFactory.CollectDbConnectionToPool(conn);
            }
        }
    }
}