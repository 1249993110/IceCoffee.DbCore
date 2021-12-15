using IceCoffee.Common;
using IceCoffee.DbCore.ExceptionCatch;

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
    /// <summary>
    /// ServiceBase
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// 当前仓储数据库连接信息
        /// </summary>
        protected internal readonly DbConnectionInfo DbConnectionInfo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public ServiceBase(DbConnectionInfo dbConnectionInfo)
        {
            DbConnectionInfo = dbConnectionInfo;
        }
    }

    public abstract partial class ServiceBase<TEntity, TDto> : ServiceBase, IService<TDto>
        where TDto : class, IDto
        where TEntity : class, IEntity
    {
        #region 字段&属性

        private readonly IRepository<TEntity> _repository;

        /// <summary>
        /// 默认仓储
        /// </summary>
        protected virtual IRepository<TEntity> Repository => _repository;

        /// <summary>
        /// 实例化 ServiceBase
        /// </summary>
        /// <param name="repository"></param>
        public ServiceBase(IRepository<TEntity> repository) : base((repository as RepositoryBase).DbConnectionInfo)
        {
            _repository = repository;
        }

        #endregion 字段&属性

        #region 默认实现
        /// <inheritdoc />
        public virtual int Add(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            return Repository.Insert(entity);
        }
        /// <inheritdoc />
        public virtual int AddBatch(IEnumerable<TDto> dtos, bool useTransaction = false)
        {
            List<TEntity> entities = DtoToEntity(dtos);
            return Repository.InsertBatch(entities, useTransaction);
        }
        /// <inheritdoc />
        public virtual int Remove(string whereBy, object param = null, bool useTransaction = false)
        {
            return Repository.Delete(whereBy, param, useTransaction);
        }
        /// <inheritdoc />
        public virtual int Remove(TDto dto)
        {
            return Repository.Delete(DtoToEntity(dto));
        }
        /// <inheritdoc />
        public virtual int RemoveById<TId>(string idColumnName, TId id)
        {
            return Repository.DeleteById(idColumnName, id);
        }
        /// <inheritdoc />
        public virtual List<TDto> GetById<TId>(string idColumnName, TId id)
        {
            return EntityToDto(Repository.QueryById(idColumnName, id));
        }
        /// <inheritdoc />
        public virtual List<TDto> GetAll(string orderBy = null)
        {
            return EntityToDto(Repository.QueryAll(orderBy));
        }
        /// <inheritdoc />
        public virtual uint GetRecordCount(string whereBy = null, object param = null)
        {
            return Repository.QueryRecordCount(whereBy, param);
        }
        /// <inheritdoc />
        public virtual int Update(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return Repository.Update(setClause, whereBy, param, useTransaction);
        }
        /// <inheritdoc />
        public virtual int Update(TDto dto)
        {
            return Repository.Update(DtoToEntity(dto));
        }
        /// <inheritdoc />
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