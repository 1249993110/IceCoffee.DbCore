using IceCoffee.Common;
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
    public abstract class ServiceBaseGuid<TEntity, TDto> : ServiceBase<TEntity, Guid, TDto, string>, IServiceBaseGuid<TDto>
        where TDto : DtoBaseGuid, new()
        where TEntity : EntityBaseGuid, new()
    {
        public ServiceBaseGuid(IRepositoryBase<TEntity, Guid> repository) : base(repository)
        {

        }

        protected override TDto EntityToDto(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            else
            {
                TDto dto = ObjectClone<TEntity, TDto>.ShallowCopy(entity);
                dto.Key = entity.Key.ToString();
                return dto;
            }
        }

        protected override TEntity DtoToEntity(TDto dto)
        {
            if(dto == null)
            {
                return null;
            }
            else
            {
                TEntity entity = ObjectClone<TDto, TEntity>.ShallowCopy(dto);
                entity.Key = dto.Key == null ? Guid.Empty : new Guid(dto.Key);
                return entity;
            }
        }
    }
}
