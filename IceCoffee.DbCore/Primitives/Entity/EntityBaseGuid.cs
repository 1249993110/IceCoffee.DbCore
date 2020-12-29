using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    public abstract class EntityBaseGuid : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey, Column("GUID"), IgnoreUpdate, IgnoreInsert]
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