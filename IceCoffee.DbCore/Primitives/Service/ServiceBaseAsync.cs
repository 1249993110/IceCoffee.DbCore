using IceCoffee.DbCore.CatchServiceException;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract partial class ServiceBase<TEntity, TKey, TDto, TQuery> : ServiceBase, IServiceBase<TDto, TQuery>, IExceptionCaught
        where TDto : DtoBase<TQuery>, new()
        where TEntity : EntityBase<TKey>, new()
    {
        /// <summary>
        /// 捕获异步服务层异常
        /// </summary>
        public static event AsyncExceptionCaughtEventHandler AsyncExceptionCaught;

        void IExceptionCaught.EmitSignal(object sender, ServiceException e)
        {
            AsyncExceptionCaught?.Invoke(this, e);
        }

        /// <summary>
        /// 是否自动处理异步服务层异常
        /// </summary>
        public bool IsAutoHandleAsyncServiceException { get; set; } = true;

        #region 默认实现

        [CatchAsyncException("插入数据异常")]
        public virtual async Task AddAsync(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            await Repository.InsertAsync(entity);
        }

        [CatchAsyncException("删除数据异常")]
        public virtual async Task RemoveAsync(TDto dto)
        {
            await Repository.DeleteAsync(DtoToEntity(dto));
        }

        [CatchAsyncException("删除全部数据异常")]
        public virtual async Task RemoveAllAsync()
        {
            await Repository.DeleteAllAsync();
        }

        [CatchAsyncException("获取全部数据异常")]
        public virtual async Task<List<TDto>> GetAllAsync(string orderBy = null)
        {
            List<TDto> dtos = new List<TDto>();
            var entitys = await Repository.QueryAllAsync(orderBy);
            return EntityToDto(entitys);
        }

        [CatchAsyncException("更新数据异常")]
        public virtual async Task UpdateAsync(TDto dto)
        {
            await Repository.UpdateAsync(DtoToEntity(dto));
        }

        #endregion 默认实现
    }
}