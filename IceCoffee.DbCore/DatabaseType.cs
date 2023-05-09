namespace IceCoffee.DbCore
{
    /// <summary>
    /// 支持的数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// 未知的数据库类型
        /// </summary>
        Unknown,

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
        /// SQLite数据库
        /// </summary>
        SQLite,
    }
}