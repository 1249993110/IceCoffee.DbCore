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
        [PrimaryKey, Column("Id"), IgnoreUpdate, IgnoreInsert]
        public virtual Guid Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 初始化，默认不生成主键，不生成创建日期，应由数据库生成
        /// </summary>
        public override object Init()
        {
            // Key = Guid.NewGuid();
            // CreatedDate = DateTime.Now;
            return this;
        }
    }
}