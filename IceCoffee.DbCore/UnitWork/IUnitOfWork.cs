using IceCoffee.DbCore.Domain;
using System.Data;

namespace IceCoffee.DbCore.UnitWork
{
    /// <summary>
    /// 工作单元
    /// 提供一个进入当前线程工作单元上下文和保存方法，它可以对调用层公开，减少连库次数
    /// 确保在单个线程中使用工作单元
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 进入工作单元上下文
        /// </summary>
        /// <returns></returns>
        void EnterContext(DbConnectionInfo dbConnectionInfo);

        /// <summary>
        /// 保存到数据库
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 使用工作单元
        /// </summary>
        bool UseUnitOfWork { get; }

        IDbConnection DbConnection { get; set; }

        IDbTransaction DbTransaction { get; }
    }
}