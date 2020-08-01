using IceCoffee.DbCore.OptionalAttributes;

namespace IceCoffee.DbCore.Primitives.Entity
{
    /// <summary>
    /// 仓储基于主键Key进行CRUD的实现基础，实体必须继承此类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey]
        public abstract TKey Key { get; set; }

        protected EntityBase(TKey key)
        {
            this.Key = key;
        }

        /// <summary>
        /// 生成主键
        /// </summary>
        /// <returns></returns>
        protected abstract TKey GenerateKey();

        /// <summary>
        /// 初始化，生成主键
        /// </summary>
        public virtual void Init()
        {
            Key = this.GenerateKey();
        }

        /// <summary>
        /// 创建一个实体，调用Init方法
        /// </summary>
        public static TEntity Create<TEntity>() where TEntity : IEntity, new()
        {
            TEntity entity = new TEntity();
            entity.Init();
            return entity;
        }

        #region 重载 备用

        //public override int GetHashCode()
        //{
        //    int result;
        //    if (object.ReferenceEquals(this.Key, null) == false)
        //    {
        //        TKey iD = this.Key;
        //        result = iD.GetHashCode();
        //    }
        //    else
        //    {
        //        result = base.GetHashCode();
        //    }
        //    return result;
        //}

        //public override bool Equals(object entity)
        //{
        //    return entity is EntityBase<TKey> && this == (EntityBase<TKey>)entity;
        //}

        //public static bool operator ==(EntityBase<TKey> entity1, EntityBase<TKey> entity2)
        //{
        //    bool result;
        //    if (entity1 == null && entity2 == null)
        //    {
        //        result = true;
        //    }
        //    else if (entity1 == null || entity2 == null)
        //    {
        //        result = false;
        //    }
        //    else if (object.Equals(entity1.Key, null))
        //    {
        //        result = false;
        //    }
        //    else
        //    {
        //        TKey iD = entity1.Key;
        //        if (iD.Equals(default(TKey)))
        //        {
        //            result = false;
        //        }
        //        else
        //        {
        //            iD = entity1.Key;
        //            result = iD.Equals(entity2.Key);
        //        }
        //    }
        //    return result;
        //}

        //public static bool operator !=(EntityBase<TKey> entity1, EntityBase<TKey> entity2)
        //{
        //    return !(entity1 == entity2);
        //}

        #endregion 重载 备用
    }
}