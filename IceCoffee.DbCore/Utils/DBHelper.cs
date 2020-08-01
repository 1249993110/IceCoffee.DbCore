using Dapper;
using IceCoffee.DbCore.Domain;
using System.Data.SQLite;

namespace IceCoffee.DbCore.Utils
{
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
        /// 获取当前线程的数据库连接，以执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        public static int ExecuteSQlite(DbConnectionInfo dbConnectionInfo, string sql)
        {
            return dbConnectionInfo.GetDbConnection().Execute(sql);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyList"></param>
        public static void CreateTable(DbConnectionInfo dbConnectionInfo, string tableName, string[] propertyList)
        {
            dbConnectionInfo.GetDbConnection().Execute(string.Format("CREATE TABLE IF NOT EXISTS {0}({1})", tableName, string.Join(",", propertyList)));
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
        public static void DropTable(DbConnectionInfo dbConnectionInfo, string tableName)
        {
            dbConnectionInfo.GetDbConnection().Execute(string.Format("DROP TABLE IF EXISTS {0}", tableName));
        }
    }
}