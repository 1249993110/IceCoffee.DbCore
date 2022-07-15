namespace IceCoffee.DbCore
{
    /// <summary>
    /// 预设的数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// 未知的数据库类型
        /// </summary>
        Unknown,

        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle,

        /// <summary>
        /// MySQL数据库
        /// </summary>
        MySQL,

        /// <summary>
        /// Microsoft SQL Server数据库
        /// </summary>
        SQLServer,

        /// <summary>
        /// PostgreSQL数据库
        /// </summary>
        PostgreSQL,

        /// <summary>
        /// IBM Db2数据库
        /// </summary>
        IBM_Db2,

        /// <summary>
        /// Microsoft Access数据库
        /// </summary>
        Aceess,

        /// <summary>
        /// SQLite数据库
        /// </summary>
        SQLite,

        /// <summary>
        /// MariaDB数据库
        /// </summary>
        MariaDB,

        /// <summary>
        /// Snowflake数据库
        /// </summary>
        Snowflake,
    }
}