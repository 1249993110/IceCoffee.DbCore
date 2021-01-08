using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    /// <inheritdoc />
    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IServiceBase<TDto>
        where TDto : DtoBase, new()
        where TEntity : EntityBase, new()
    {
        #region 默认实现
        /// <inheritdoc />
        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            await Repository.InsertAsync(entity);
            return EntityToDto(entity);
        }
        /// <inheritdoc />
        public virtual async Task<int> RemoveAsync(TDto dto)
        {
            return await Repository.DeleteAsync(DtoToEntity(dto));
        }
        /// <inheritdoc />
        public virtual async Task<List<TDto>> GetAllAsync(string orderBy = null)
        {
            var entities = await Repository.QueryAllAsync(orderBy);
            return EntityToDto(entities);
        }
        /// <inheritdoc />
        public virtual async Task<int> UpdateAsync(TDto dto)
        {
            return await Repository.UpdateAsync(DtoToEntity(dto));
        }

        #endregion 默认实现
    }
}