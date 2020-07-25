using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public interface IServiceBaseGuid<TDto> : IServiceBase<TDto, string>
    {
    }
}
