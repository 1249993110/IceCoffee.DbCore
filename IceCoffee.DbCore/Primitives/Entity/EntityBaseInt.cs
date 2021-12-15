using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// EntityBase泛型参数为int的默认实现
    /// </summary>
    public abstract class EntityBaseInt : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey, IgnoreUpdate, IgnoreInsert]
        public virtual int Id { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime CreatedDate { get; set; }

    }
}
