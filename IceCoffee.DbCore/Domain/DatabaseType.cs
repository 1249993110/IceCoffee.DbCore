using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Domain
{
    public enum DatabaseType
    {
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
        Oracle
    }
}
