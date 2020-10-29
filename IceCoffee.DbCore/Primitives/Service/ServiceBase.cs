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
using Mapster;

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract class ServiceBase
    {
        // protected static IMapper Mapper => EntityDtoMapper.mapper;

        public abstract DbConnectionInfo DbConnectionInfo { get; }
    }

    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IServiceBase<TDto>
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
        public virtual TDto Add(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            Repository.Insert(entity);
            return EntityToDto(entity);
        }
        public virtual List<TDto> AddBatch(IEnumerable<TDto> dtos, bool useTransaction = false)
        {
            List<TEntity> entities = DtoToEntity(dtos);
            foreach (var entity in entities)
            {
                entity.Init();
            }
            Repository.InsertBatch(entities, useTransaction);
            return EntityToDto(entities);
        }
        public virtual int RemoveAny(string whereBy, object param = null, bool useTransaction = false)
        {
            return Repository.DeleteAny(whereBy, param, useTransaction);
        }
        
        public virtual int Remove(TDto dto)
        {
            return Repository.Delete(DtoToEntity(dto));
        }
        
        public virtual int RemoveById<TId>(string idColumnName, TId id)
        {
            return Repository.DeleteById(idColumnName, id);
        }
        
        public virtual List<TDto> GetById<TId>(string idColumnName, TId id)
        {
            return EntityToDto(Repository.QueryById(idColumnName, id));
        }
        
        public virtual List<TDto> GetAll(string orderBy = null)
        {
            return EntityToDto(Repository.QueryAll(orderBy));
        }
        
        public virtual long GetRecordCount(string whereBy = null, object param = null)
        {
            return Repository.QueryRecordCount(whereBy, param);
        }
        
        public virtual int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return Repository.UpdateAny(setClause, whereBy, param, useTransaction);
        }
        
        public virtual int Update(TDto dto)
        {
            return Repository.Update(DtoToEntity(dto));
        }
        
        public virtual int UpdateById(string idColumnName, TDto dto)
        {
            return Repository.UpdateById(idColumnName, DtoToEntity(dto));
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
            return entity == null ? null : entity.Adapt<TDto>();
        }

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual List<TDto> EntityToDto(IEnumerable<TEntity> entities)
        {
            return entities == null ? null : entities.Adapt<List<TDto>>();
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TEntity DtoToEntity(TDto dto)
        {
            return dto == null ? null : dto.Adapt<TEntity>();
        }

        /// <summary>
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        protected virtual List<TEntity> DtoToEntity(IEnumerable<TDto> dtos)
        {
            return dtos == null ? null : dtos.Adapt<List<TEntity>>();
        }

        #endregion
    }
}