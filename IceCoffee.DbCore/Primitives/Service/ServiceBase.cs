using AutoMapper;
using AutoMapper.Mappers;
using IceCoffee.Common;
using IceCoffee.DbCore.CatchServiceException;
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
    public static class EntityDtoMapper
    {
        internal static IMapper mapper;

        /// <summary>
        /// 初始化Entity与Dto之间的自动映射
        /// </summary>
        /// <param name="types"></param>
        public static void InitMap(IEnumerable<Type> types)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                var normalTypes = types.Where(t => t.IsSubclassOf(typeof(ServiceBase)));
                var customTypes = types.Where(t => t.GetInterfaces().Any(i => typeof(ICustomMappings).IsAssignableFrom(t)))
                                    .Select(t => (ICustomMappings)Activator.CreateInstance(t));

                foreach (var normalType in normalTypes)
                {
                    var genericArgs = GetBaseGenericTypes(normalType);
                    if (genericArgs != null)
                    {
                        var entityType = genericArgs.FirstOrDefault(p => typeof(IEntity).IsAssignableFrom(p));
                        var dtoType = genericArgs.FirstOrDefault(p => typeof(IDtoBase).IsAssignableFrom(p));

                        if (entityType != null && dtoType != null)
                        {
                            cfg.CreateMap(entityType, dtoType, MemberList.None);
                            cfg.CreateMap(dtoType, entityType, MemberList.None);
                        }
                    }
                }

                foreach (var customType in customTypes)
                {
                    customType.CreateMappings(cfg);
                }
            });

#if DEBUG
            try
            {
                config.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                throw new Exception("初始化Entity与Dto之间的自动映射异常", ex);
            }
#endif

            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 递归获取基类的模板类型参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type[] GetBaseGenericTypes(Type type)
        {
            if (type.BaseType.IsGenericType)
            {
                Type[] types = type.BaseType.GetGenericArguments();
                if (types.Length > 1)
                {
                    return types;
                }
            }
            else if (type == typeof(object))
            {
                return null;
            }

            return GetBaseGenericTypes(type.BaseType);
        }
    }

    public abstract class ServiceBase
    {
        protected static IMapper Mapper => EntityDtoMapper.mapper;

        public abstract DbConnectionInfo DbConnectionInfo { get; }
    }

    public abstract partial class ServiceBase<TEntity, TKey, TDto, TQuery> : ServiceBase, IServiceBase<TDto, TQuery>, IExceptionCaught
        where TDto : DtoBase<TQuery>, new()
        where TEntity : EntityBase<TKey>, new()
    {
        #region 字段&属性

        private readonly IRepositoryBase<TEntity, TKey> _repository;

        /// <summary>
        /// 默认仓储
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

        #endregion 字段&属性

        #region 默认实现

        [CatchSyncException("插入数据异常")]
        public virtual int Add(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            return Repository.Insert(entity);
        }

        [CatchSyncException("批量插入数据异常")]
        public virtual int AddBatch(IEnumerable<TDto> dtos)
        {
            return Repository.InsertBatch(DtoToEntity(dtos));
        }

        [CatchSyncException("删除任意数据异常")]
        public virtual int RemoveAny(string whereBy, object param = null, bool useTransaction = false)
        {
            return Repository.DeleteAny(whereBy, param, useTransaction);
        }

        [CatchSyncException("删除数据异常")]
        public virtual int Remove(TDto dto)
        {
            return Repository.Delete(DtoToEntity(dto));
        }

        [CatchSyncException("通过ID删除数据异常")]
        public virtual int RemoveById<TId>(TId id, string idColumnName)
        {
            return Repository.DeleteById(id, idColumnName);
        }

        [CatchSyncException("通过ID获取数据异常")]
        public virtual List<TDto> GetById<TId>(TId id, string idColumnName)
        {
            return EntityToDto(Repository.QueryById(id, idColumnName));
        }

        [CatchSyncException("获取全部数据异常")]
        public virtual List<TDto> GetAll(string orderBy = null)
        {
            return EntityToDto(Repository.QueryAll(orderBy));
        }

        [CatchSyncException("获取记录条数异常")]
        public virtual long GetRecordCount(string whereBy = null, object param = null)
        {
            return Repository.QueryRecordCount(whereBy, param);
        }

        public virtual int UpdateAny(string setClause, string whereBy, object param, bool useTransaction = false)
        {
            return Repository.UpdateAny(setClause, whereBy, param, useTransaction);
        }

        [CatchSyncException("更新数据异常")]
        public virtual int Update(TDto dto)
        {
            return Repository.Update(DtoToEntity(dto));
        }

        [CatchSyncException("更新数据异常")]
        public virtual int UpdateById<TId>(TDto dto, TId id, string idColumnName)
        {
            return Repository.UpdateById(DtoToEntity(dto), id, idColumnName);
        }
        #endregion 默认实现

        /// <summary>
        /// 将实体转换为Dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TDto EntityToDto(TEntity entity)
        {
            return entity == null ? null : Mapper.Map<TDto>(entity);//ObjectClone<TEntity, TDto>.ShallowCopy(entity);
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
            if (dto == null)
            {
                return null;
            }
            else
            {
                TEntity entity = Mapper.Map<TEntity>(dto);//ObjectClone<TDto, TEntity>.ShallowCopy(dto);
                if (dto.Key == null)
                {
                    entity.Init();
                }
                
                return entity;
            }
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
                entitys.Add(entity);
            }
            return entitys;
        }
    }
}