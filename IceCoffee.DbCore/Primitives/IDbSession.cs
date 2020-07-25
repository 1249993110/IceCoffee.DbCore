using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives
{
    /// <summary>
    /// 数据库操作会话
    /// </summary>
    public interface IDbSession
    {
        /// <summary>
        /// 表示到数据源的连接
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 表示要在数据源上执行的事务，由AOP切面提供
        /// </summary>
        IDbTransaction Transaction { get; set; }
    }
}
