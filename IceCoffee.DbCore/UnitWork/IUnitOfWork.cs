﻿using IceCoffee.DbCore.Domain;
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
        /// 是否需要显示进行提交（save()）
        /// 默认为false，即在repository方法中自动完成提交，值为true时，表示需要显示调用save()方法
        /// </summary>
        bool IsExplicitSubmit { get; }

        IDbConnection DbConnection { get; set; }

        IDbTransaction DbTransaction { get; }
    }
}