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
        /// SQLite数据库
        /// </summary>
        SQLite,

        /// <summary>
        /// SQL Server数据库
        /// </summary>
        SQLServer,

        /// <summary>
        /// MySQL数据库
        /// </summary>
        MySQL,

        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle,

        /// <summary>
        /// Aceess数据库
        /// </summary>
        Aceess
    }
}