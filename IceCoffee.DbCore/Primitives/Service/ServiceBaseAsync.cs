using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IServiceBase<TDto>, IExceptionCaught
        where TDto : DtoBase, new()
        where TEntity : EntityBase, new()
    {
        /// <summary>
        /// 捕获服务层操作异常事件
        /// </summary>
        public static event ServiceExceptionCaughtEventHandler ExceptionCaught;

        void IExceptionCaught.EmitSignal(object sender, ServiceException ex)
        {
            ExceptionCaught?.Invoke(this, ex);
        }

        #region 默认实现
        [CatchServiceException]
        [AutoHandleException]
        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            await Repository.InsertAsync(entity);
            return EntityToDto(entity);
        }
        [CatchServiceException]
        [AutoHandleException]
        public virtual async Task<int> RemoveAsync(TDto dto)
        {
            return await Repository.DeleteAsync(DtoToEntity(dto));
        }
        [CatchServiceException]
        [AutoHandleException]
        public virtual async Task<int> RemoveAllAsync()
        {
            return await Repository.DeleteAllAsync();
        }
        [CatchServiceException]
        [AutoHandleException]
        public virtual async Task<List<TDto>> GetAllAsync(string orderBy = null)
        {
            List<TDto> dtos = new List<TDto>();
            var entities = await Repository.QueryAllAsync(orderBy);
            return EntityToDto(entities);
        }
        [CatchServiceException]
        [AutoHandleException]
        public virtual async Task<int> UpdateAsync(TDto dto)
        {
            return await Repository.UpdateAsync(DtoToEntity(dto));
        }

        #endregion 默认实现
    }
}