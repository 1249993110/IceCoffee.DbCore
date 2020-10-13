﻿using IceCoffee.DbCore.OptionalAttributes;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// 仓储基于主键Key进行CRUD的实现基础，实体必须继承此类
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        /// <summary>
        /// 初始化，返回对象自身
        /// </summary>
        public virtual object Init()
        {
            return null;
        }

        /// <summary>
        /// 创建一个实体，调用Init方法
        /// </summary>
        public static TEntity Create<TEntity>() where TEntity : IEntity, new()
        {
            TEntity entity = new TEntity();
            entity.Init();
            return entity;
        }
    }
}