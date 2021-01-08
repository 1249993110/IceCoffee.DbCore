using Dapper;
using IceCoffee.DbCore.UnitWork;
using System;
using System.Collections.Generic;

namespace IceCoffee.DbCore.Primitives.Service
{
    /// <summary>
    /// 使用工作单元执行任意多表操作
    /// </summary>
    public class AnyService
    {
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }
        /// <summary>
        /// 多表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TSixth"></typeparam>
        /// <typeparam name="TSeventh"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="unitOfWork"></param>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="splitOn"></param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(IUnitOfWork unitOfWork,
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, string splitOn = "Id")
        {
            return unitOfWork.DbConnection.Query(sql, map, param, unitOfWork.DbTransaction, splitOn: splitOn);
        }

        /// <summary>
        /// 多表插入、修改、删除
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IUnitOfWork unitOfWork, List<Tuple<string, object>> trans)
        {
            int rowsAffected = 0;
            foreach (var tran in trans)
            {
                rowsAffected += unitOfWork.DbConnection.Execute(tran.Item1, tran.Item2, unitOfWork.DbTransaction);
            }

            return rowsAffected;
        }
    }
}