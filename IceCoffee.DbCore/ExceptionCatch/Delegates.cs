using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 服务层异常事件处理器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ex"></param>
    public delegate void ServiceExceptionCaughtEventHandler(object sender, ServiceException ex);
}
