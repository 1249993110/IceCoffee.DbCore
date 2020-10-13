using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 自动处理服务层异常
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AutoHandleExceptionAttribute : Attribute
    {
        /// <summary>
        /// 是否自动处理异常
        /// </summary>
        public bool IsAutoHandle { get; set; } = true;
    }
}
