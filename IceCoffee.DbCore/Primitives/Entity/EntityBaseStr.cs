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
        [PrimaryKey, Column("Id"), IgnoreUpdate]
        public virtual string Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 初始化，默认生成主键，不生成创建日期，应由数据库生成
        /// </summary>
        public override object Init()
        {
            Key = Guid.NewGuid().ToString();
            // CreatedDate = DateTime.Now;
            return this;
        }
    }
}