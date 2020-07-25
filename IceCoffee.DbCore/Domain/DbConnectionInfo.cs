using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IceCoffee.DbCore.Domain
{
    public sealed class DbConnectionInfo
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 是否使用数据库连接池，否则使用ThreadLocal
        /// </summary>
        public bool UseConnectionPool { get; set; }

        public DbConnectionInfo(string connectionString, DatabaseType databaseType, bool useConnectionPool = false)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
            UseConnectionPool = useConnectionPool;
        }

        /// <summary>
        /// 从连接信息得到一个数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            IDbConnection connection = null;
            switch (this.DatabaseType)
            {
                case DatabaseType.SQLite:
                    connection = ConnectionFactory<SQLiteConnection>.GetConnectionFromThreadLocal(this.ConnectionString).Value;
                    break;
                case DatabaseType.SQLServer:
                    break;
                case DatabaseType.MySQL:
                    break;
                case DatabaseType.Oracle:
                    break;
                default:
                    throw new Exception("未定义的数据库类型");
            }
            return connection;
        }
    }
}
