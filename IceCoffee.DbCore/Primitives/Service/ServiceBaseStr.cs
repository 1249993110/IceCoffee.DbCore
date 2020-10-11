using IceCoffee.Common;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;

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