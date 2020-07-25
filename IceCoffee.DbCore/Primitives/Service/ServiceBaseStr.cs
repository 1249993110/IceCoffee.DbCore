using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract class ServiceBaseStr<TEntity, TDto> : ServiceBase<TEntity, string, TDto, string>, IServiceBaseStr<TDto>
        where TDto : DtoBaseStr, new()
        where TEntity : EntityBase<string>, new()
    {
        public ServiceBaseStr(IRepositoryBase<TEntity, string> repository) : base(repository)
        {
            
        }
    }
}
