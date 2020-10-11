using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// EntityBase泛型参数为int的默认实现
    /// </summary>
    public abstract class EntityBaseInt : EntityBase<int>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("GUID"), IgnoreUpdate, IgnoreInsert]
        public override int Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime? CreatedDate { get; set; }

        public EntityBaseInt() : base(0)
        {
        }

        protected EntityBaseInt(int key) : base(key)
        {
        }

        /// <summary>
        /// 默认不生成主键
        /// </summary>
        protected override int GenerateKey()
        {
            return 0;
        }

        /// <summary>
        /// 初始化，默认不生成主键，不生成创建日期，应由数据库生成
        /// </summary>
        public override void Init()
        {
            
        }
    }
}
