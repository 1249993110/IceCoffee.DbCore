using IceCoffee.Common;
using System.Configuration;
using System.Data;

namespace IceCoffee.DbCore
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DbConnectionInfo
    {
        /// <summary>
        /// 连接名
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

#if NET45
        /// <summary>
        /// 由ConfigurationManager通过key得到ConnectionString和ProviderName
        /// </summary>
        /// <param name="connectionStringKey">数据库连接串的key</param>
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

            ConnectionName = connectionStringKey;
            ConnectionString = connStrSetting.ConnectionString;
            DatabaseType = databaseType;
        }
#endif

        /// <summary>
        /// 实例化 <see cref="DbConnectionInfo"/>
        /// </summary>
        public DbConnectionInfo()
        {
        }
    }
}