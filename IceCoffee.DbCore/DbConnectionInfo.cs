﻿namespace IceCoffee.DbCore
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DbConnectionInfo
    {
        /// <summary>
        /// 实例化 <see cref="DbConnectionInfo"/>
        /// </summary>
        public DbConnectionInfo()
        {
        }

        /// <summary>
        /// 连接串
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <inheritdoc/>
        public static bool operator !=(DbConnectionInfo? left, DbConnectionInfo? right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public static bool operator ==(DbConnectionInfo? left, DbConnectionInfo? right)
        {
            return left?.ConnectionString == right?.ConnectionString
                && left?.DatabaseType == right?.DatabaseType;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is DbConnectionInfo dbConnectionInfo)
            {
                return this == dbConnectionInfo;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }

            return ConnectionString.GetHashCode();
        }
    }
}