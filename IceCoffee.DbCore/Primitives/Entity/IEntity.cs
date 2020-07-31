using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Entity
{
    public interface IEntity
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }

    public interface IEntity<out TKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Key { get; }
    }
}
