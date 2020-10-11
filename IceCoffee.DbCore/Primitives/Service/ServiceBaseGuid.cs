using IceCoffee.Common;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract class ServiceBaseGuid<TEntity, TDto> : ServiceBase<TEntity, Guid, TDto, string>, IServiceBaseGuid<TDto>
        where TDto : DtoBaseGuid, new()
        where TEntity : EntityBaseGuid, new()
    {
        public ServiceBaseGuid(IRepositoryBase<TEntity, Guid> repository) : base(repository)
        {
            
        }
    }
}