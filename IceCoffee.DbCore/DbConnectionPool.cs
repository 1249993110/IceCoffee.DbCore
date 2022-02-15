using IceCoffee.Common.Pools;
using IceCoffee.DbCore.ExceptionCatch;
using System;
using System.Data;
using System.Data.Common;

namespace IceCoffee.DbCore
{
    /// <summary>
    /// 数据库连接池
    /// </summary>
    public class DbConnectionPool : LocklessPool<IDbConnection>
    {
        private readonly string _connectionString;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString => _connectionString;

        /// <summary>
        /// 实例化数据库连接池
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="connectionGenerator"></param>
        public DbConnectionPool(string connectionString, Func<IDbConnection> connectionGenerator) : base(connectionGenerator)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// 申请时检查是否打开
        /// </summary>
        /// <returns></returns>
        public override IDbConnection Get()
        {
            try
            {
                var conn = base.Get();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = _connectionString;
                    conn.Open();
                }

                return conn;
            }
            catch (Exception ex)
            {
                throw new DbCoreException("数据库连接打开失败！请检查驱动是否正常", ex);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="dbConnection"></param>
        public override void Return(IDbConnection dbConnection)
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                base.Return(dbConnection);
            }
            else
            {
                dbConnection.Dispose();
            }
        }
    }
}
