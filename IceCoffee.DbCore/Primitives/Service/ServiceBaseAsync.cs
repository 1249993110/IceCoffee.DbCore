using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    /// <inheritdoc />
    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IService<TDto>
        where TDto : class, IDto
        where TEntity : class, IEntity
    {
        #region 默认实现
        /// <inheritdoc />
        public virtual Task<int> AddAsync(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            return Repository.InsertAsync(entity);
        }
        /// <inheritdoc />
        public virtual Task<int> RemoveAsync(TDto dto)
        {
            return Repository.DeleteAsync(DtoToEntity(dto));
        }
        /// <inheritdoc />
        public virtual async Task<List<TDto>> GetAllAsync(string orderBy = null)
        {
            var entities = await Repository.QueryAllAsync(orderBy);
            return EntityToDto(entities);
        }
        /// <inheritdoc />
        public virtual Task<int> UpdateAsync(TDto dto)
        {
            return Repository.UpdateAsync(DtoToEntity(dto));
        }

        #endregion 默认实现
    }
}