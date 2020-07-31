using System;
using IceCoffee.DbCore.Primitives;
using System.Data;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using IceCoffee.DbCore.CatchServiceException;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.Common;
using IceCoffee.DbCore.Domain;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract class ServiceBase
    {
        public abstract DbConnectionInfo DbConnectionInfo { get; }
    }
    public abstract partial class ServiceBase<TEntity, TKey, TDto, TQuery> : ServiceBase, IServiceBase<TDto, TQuery>, IExceptionCaught
        where TDto : DtoBase<TQuery>, new() 
        where TEntity : EntityBase<TKey>, new()
    {
        #region 字段&属性
        private readonly IRepositoryBase<TEntity, TKey> _repository;

        /// <summary>
        /// 仓储
        /// </summary>
        protected virtual IRepositoryBase<TEntity, TKey> Repository
        {
            get { return _repository; }
        }

        public override DbConnectionInfo DbConnectionInfo => (_repository as RepositoryBase).dbConnectionInfo;

        public ServiceBase(IRepositoryBase<TEntity, TKey> repository)
        {
            _repository = repository;
        }
        #endregion


        #region 默认实现

        [CatchSyncException("插入数据异常")]
        public void Insert(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            Repository.Insert(entity);
        }

        [CatchSyncException("插入数据异常")]
        public void InsertBatch(IEnumerable<TDto> dtos)
        {
            Repository.InsertBatch(DtoToEntity(dtos));
        }

        [CatchSyncException("删除数据异常")]
        public void Remove(TDto dto)
        {
            Repository.Delete(DtoToEntity(dto));
        }

        [CatchSyncException("通过ID获取数据异常")]
        public List<TDto> GetById<TId>(TId id, string idColumnName)
        {
            return EntityToDto(Repository.QueryById(id, idColumnName));
        }

        [CatchSyncException("获取全部数据异常")]
        public List<TDto> GetAll(string orderBy = null)
        {
            return EntityToDto(Repository.QueryAll(orderBy));
        }

        [CatchSyncException("获取全部记录条数异常")]
        public long GetRecordCount()
        {
            return Repository.QueryRecordCount();
        }

        [CatchSyncException("更新数据异常")]
        public void Update(TDto dto)
        {
            Repository.Update(DtoToEntity(dto));
        }
        #endregion

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TDto EntityToDto(TEntity entity)
        {
            return entity == null ? null : ObjectClone<TEntity, TDto>.ShallowCopy(entity);
        }

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        protected virtual List<TDto> EntityToDto(IEnumerable<TEntity> entitys)
        {
            List<TDto> dtos = new List<TDto>();
            foreach (var item in entitys)
            {
                dtos.Add(EntityToDto(item));
            }
            return dtos;
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TEntity DtoToEntity(TDto dto)
        {
            return dto == null ? null : ObjectClone<TDto, TEntity>.ShallowCopy(dto);
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected virtual List<TEntity> DtoToEntity(IEnumerable<TDto> dtos)
        {
            List<TEntity> entitys = new List<TEntity>();
            foreach (var item in dtos)
            {
                TEntity entity = DtoToEntity(item);
                entity.Init();
                entitys.Add(entity);
            }
            return entitys;
        }
    }

}
