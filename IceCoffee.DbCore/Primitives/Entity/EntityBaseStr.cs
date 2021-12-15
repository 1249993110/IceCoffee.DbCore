using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// EntityBase泛型参数为string的默认实现
    /// </summary>
    public abstract class EntityBaseStr : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey, IgnoreUpdate]
        public virtual string Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime CreatedDate { get; set; }
    }
}