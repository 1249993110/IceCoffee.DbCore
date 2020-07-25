using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace IceCoffee.DbCore.Primitives.Service
{
    public class AnyService
    {
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="dbSession"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }

        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }

        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }

        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }

        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }

        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(IDbSession dbSession,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, string splitOn = "Id")
        {
            return dbSession.Connection.Query(sql, map, param, dbSession.Transaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表插入、修改、删除
        /// </summary>
        /// <param name="dbSession"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbSession dbSession, List<Tuple<string, object>> trans)
        {
            try
            {
                dbSession.Connection.Open();
                dbSession.Transaction = dbSession.Connection.BeginTransaction();
            }
            catch (Exception e)
            {
                throw new Exception("数据库事务操作异常", e);
            }

            try
            {
                // 受影响的行数
                int rowsAffected = 0;
                foreach (var tran in trans)
                {
                    // 执行事务
                    rowsAffected += dbSession.Connection.Execute(tran.Item1, tran.Item2, dbSession.Transaction);
                }

                // 提交事务
                dbSession.Transaction.Commit();

                return rowsAffected;
            }
            catch
            {
                dbSession.Transaction.Rollback();
                throw;
            }
            finally
            {
                dbSession.Transaction = null;
            }
        }
    }
}
