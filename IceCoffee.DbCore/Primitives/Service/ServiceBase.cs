using AutoMapper;
using AutoMapper.Mappers;
using IceCoffee.Common;
using IceCoffee.DbCore.ExceptionCatch;
using IceCoffee.DbCore.Domain;
using IceCoffee.DbCore.Primitives.Dto;
using IceCoffee.DbCore.Primitives.Entity;
using IceCoffee.DbCore.Primitives.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract class ServiceBase
    {
        protected static IMapper Mapper => EntityDtoMapper.mapper;

        public abstract DbConnectionInfo DbConnectionInfo { get; }
    }

    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IServiceBase<TDto>, IExceptionCaught
        where TDto : DtoBase, new()
        where TEntity : EntityBase, new()
    {
        #region 字段&属性

        private readonly IRepositoryBase<TEntity> _repository;

        /// <summary>
        /// 默认仓储
        /// </summary>
        protected virtual IRepositoryBase<TEntity> Repository
        {
            get { return _repository; }
        }

        public override DbConnectionInfo DbConnectionInfo => (_repository as RepositoryBase).dbConnectionInfo;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        #endregion 字段&属性

        #region 默认实现
        [CatchServiceException]
        public virtual TDto Add(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            Repository.Insert(entity);
            return EntityToDto(entity);
        }
        [CatchServiceException]
        public virtual List<TDto> AddBatch(IEnumerable<TDto> dtos)
        {
            List<TEntity> entities = DtoToEntity(dtos);
            foreach (var entity in entities)
            {
                entity.Init();
            }
            Repository.InsertBatch(entities);
            return EntityToDto(entities);
        }
        [CatchServiceException]
        public virtual int RemoveAny(string whereBy, object param = null, bool useTransaction = false)
        {
            return Repository.DeleteAny(whereBy, param, useTransaction);
        }
        [CatchServiceException]
        public virtual int Remove(TDto dto)
        {
            return Repository.Delete(DtoToEntity(dto));
        }
        [CatchServiceException]
        public virtual int RemoveById<TId>(TId id, string idColumnName)
        {
            return Repository.DeleteById(id, idColumnName);
        }
        [CatchServiceException]
        public virtual List<TDto> GetById<TId>(TId id, string idColumnName)
        {
            return EntityToDto(Repository.QueryById(id, idColumnName));
        }
        [CatchServiceException]
        public virtual List<TDto> GetAll(string orderBy = null)
        {
            return EntityToDto(Repository.QueryAll(orderBy));
        }
        [CatchServiceException]
        public virtual long GetRecordCount(string whereBy = null, object param = null)
        {
            return Repository.QueryRecordCount(whereBy, param);
        }
        [CatchServiceException]
        public virtual int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return Repository.UpdateAny(setClause, whereBy, param, useTransaction);
        }
        [CatchServiceException]
        public virtual int Update(TDto dto)
        {
            return Repository.Update(DtoToEntity(dto));
        }
        [CatchServiceException]
        public virtual int UpdateById<TId>(TDto dto, TId id, string idColumnName)
        {
            return Repository.UpdateById(DtoToEntity(dto), id, idColumnName);
        }
        #endregion 默认实现

        #region Entity to Dto and Dto to Entity

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TDto EntityToDto(TEntity entity)
        {
            return entity == null ? null : Mapper.Map<TDto>(entity);
        }

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual List<TDto> EntityToDto(IEnumerable<TEntity> entities)
        {
            return entities == null ? null : Mapper.Map<List<TDto>>(entities);
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TEntity DtoToEntity(TDto dto)
        {
            return dto == null ? null : Mapper.Map<TEntity>(dto);
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected virtual List<TEntity> DtoToEntity(IEnumerable<TDto> dtos)
        {
            return dtos == null ? null : Mapper.Map<List<TEntity>>(dtos);
        }

        #endregion
    }
}