using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// 实体 主键为 GUID 的实现基类
    /// </summary>
    public abstract class EntityBaseGuid : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey, IgnoreUpdate, IgnoreInsert]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime CreatedDate { get; set; }

    }
}