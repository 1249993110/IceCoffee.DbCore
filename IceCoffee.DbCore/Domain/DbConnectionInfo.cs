using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public DbConnectionInfo(string connectionString, DatabaseType databaseType)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
        }

        /// <summary>
        /// 从连接信息得到一个数据库连接
        /// </summary>
        /// <returns></returns>
        public DbConnection GetDbConnection()
        {
            return ConnectionFactory.GetConnectionFromPool(this); ;
        }
    }
}
