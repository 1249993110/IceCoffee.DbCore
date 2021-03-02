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
        [PrimaryKey, Column("GUID"), IgnoreUpdate, IgnoreInsert]
        public virtual int Key { get; set; }

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
            // CreatedDate = DateTime.Now;
            return this;
        }
    }
}
