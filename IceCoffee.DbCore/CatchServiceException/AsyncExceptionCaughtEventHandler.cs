using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.CatchServiceException
{
    /// <summary>
    /// 异步服务层异常事件处理器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AsyncExceptionCaughtEventHandler(object sender, ServiceException e);
}
