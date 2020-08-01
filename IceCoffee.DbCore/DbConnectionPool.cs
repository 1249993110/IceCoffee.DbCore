using IceCoffee.Common.Pools;
using System;
using System.Data;
using System.Data.Common;

namespace IceCoffee.DbCore
{
    public class DbConnectionPool : ConnectionPool<IDbConnection>
    {
        private readonly string _connectionString;

        private readonly DbProviderFactory _factory;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString => _connectionString;

        public DbConnectionPool(string connectionString, DbProviderFactory factory, int maxConnectionCount = 1000)
        {
            this._connectionString = connectionString;
            this._factory = factory;

            Min = Environment.ProcessorCount;
            if (Min < 2)
            {
                Min = 2;
            }
            if (Min > 8)
            {
                Min = 8;
            }

            Max = maxConnectionCount;

            IdleTime = 60;
            AllIdleTime = 180;
        }

        protected override IDbConnection Create()
        {
            var conn = _factory?.CreateConnection();
            if (conn == null)
            {
                var msg = "连接创建失败！请检查驱动是否正常";

                throw new Exception(Name + " " + msg);
            }

            conn.ConnectionString = ConnectionString;

            try
            {
                conn.Open();
            }
            catch (DbException)
            {
                throw;
            }

            return conn;
        }

        /// <summary>申请时检查是否打开</summary>
        public override IDbConnection Take()
        {
            var conn = base.Take();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        /// <summary>释放时，返回是否有效。无效对象将会被抛弃</summary>
        /// <param name="value"></param>
        protected override bool OnPut(IDbConnection value)
        {
            return value.State == ConnectionState.Open;
        }

        /// <summary>借一个连接执行指定操作</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public T Execute<T>(Func<IDbConnection, T> callback)
        {
            var conn = Take();
            try
            {
                return callback(conn);
            }
            finally
            {
                Put(conn);
            }
        }
    }
}