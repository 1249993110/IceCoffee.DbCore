using IceCoffee.Common;
using System.Configuration;
using System.Data;

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
        /// 数据库连接串的key
        /// </summary>
        /// <param name="connectionStringKey"></param>
        public DbConnectionInfo(string connectionStringKey)
        {
            var connStrSetting = ConfigurationManager.ConnectionStrings[connectionStringKey];

            DatabaseType databaseType = DatabaseType.Unknown;

            switch (connStrSetting.ProviderName)
            {
                case "System.Data.SQLite":
                    databaseType = DatabaseType.SQLite;
                    break;
                case "System.Data.SqlClient":
                    databaseType = DatabaseType.SQLServer;
                    break;
                case "System.Data.Odbc":
                    databaseType = DatabaseType.MySQL;
                    break;
                case "System.Data.OracleClient":
                case "Oracle.DataAccess.Client":
                    databaseType = DatabaseType.Oracle;
                    break;
                case "System.Data.OleDb":
                    databaseType = DatabaseType.Aceess;
                    break;
                default:
                    break;
            }

            ConnectionString = connStrSetting.ConnectionString;
            DatabaseType = databaseType;
        }


        public DbConnectionInfo(string connectionString, DatabaseType databaseType)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
        }

        /// <summary>
        /// 从连接信息得到一个数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            return ConnectionFactory.GetConnectionFromPool(this);
        }
    }
}