using IceCoffee.DbCore.OptionalAttributes;
using System;

namespace IceCoffee.DbCore.Primitives.Entity
{
    public abstract class EntityBaseGuid : EntityBase<Guid>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("GUID"), IgnoreUpdate, IgnoreInsert]
        public override Guid Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime? CreatedDate { get; set; }

        public EntityBaseGuid() : base(Guid.Empty)
        {
        }

        protected EntityBaseGuid(Guid guid) : base(guid)
        {
        }

        /// <summary>
        /// 生成主键
        /// </summary>
        protected override Guid GenerateKey()
        {
            return Guid.NewGuid();
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