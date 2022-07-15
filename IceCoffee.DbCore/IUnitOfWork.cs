
/* 项目“IceCoffee.DbCore (netcoreapp3.1)”的未合并的更改
在此之前:
using System.Data;
在此之后:
using IceCoffee;
using IceCoffee.DbCore;
using IceCoffee.DbCore;
using IceCoffee.DbCore.UnitWork;
using System.Data;
*/
using System.Data;

namespace IceCoffee.DbCore
{
    /// <summary>
    /// 工作单元
    /// </summary>
    /// <remarks>
    /// 提供一个进入当前线程工作单元上下文和保存方法, 它可以对调用层公开, 减少连库次数, 保证数据原子性、一致性、隔离性、持久性
    /// <para>确保在单个线程中使用工作单元, 工作单元无法跨线程、跨进程或跨数据库使用</para>
    /// </remarks>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 进入工作单元上下文
        /// </summary>
        /// <returns></returns>
        IUnitOfWork EnterContext();

        /// <summary>
        /// 进入工作单元上下文
        /// </summary>
        /// <returns></returns>
        IUnitOfWork EnterContext(DbConnectionInfo dbConnectionInfo);

        /// <summary>
        /// 保存到数据库
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 是否需要显示进行提交（save()）
        /// 默认为false, 即在repository方法中自动完成提交, 值为true时, 表示需要显示调用save()方法
        /// </summary>
        bool IsExplicitSubmit { get; }

        /// <summary>
        /// 从挂起状态回滚事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// DbConnection
        /// </summary>
        IDbConnection? DbConnection { get; }

        /// <summary>
        /// DbTransaction
        /// </summary>
        IDbTransaction? DbTransaction { get; }

        /// <summary>
        /// 数据库连接信息
        /// </summary>
        DbConnectionInfo? DbConnectionInfo { get; }
    }
}