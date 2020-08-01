using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// EntityBase泛型参数为string的默认实现
    /// </summary>
    public abstract class EntityBaseStr : EntityBase<string>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID"), IgnoreUpdate]
        public override string Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate]
        public DateTime? CreatedDate { get; set; }

        public EntityBaseStr() : base(string.Empty)
        {
        }

        protected EntityBaseStr(string key) : base(key)
        {
        }

        /// <summary>
        /// 生成主键
        /// </summary>
        protected override string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 初始化，生成主键和创建日期
        /// </summary>
        public override void Init()
        {
            base.Init();
            CreatedDate = DateTime.Now;
        }
    }
}