using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.CatchServiceException
{
    public interface IExceptionCaught
    {
        /// <summary>
        /// 发射异常捕获信号
        /// </summary>
        /// <param name="ex"></param>
        void EmitSignal(object sender, ServiceException ex);

        /// <summary>
        /// 是否自动处理服务层异步方法异常
        /// </summary>
        bool IsAutoHandleAsyncServiceException { get; set; }
    }
}
