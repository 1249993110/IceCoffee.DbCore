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

namespace IceCoffee.DbCore.Primitives.Service
{
    public abstract partial class ServiceBase<TEntity, TKey, TDto, TQuery> : IServiceBase<TDto, TQuery>, IDbSession, IDisposable, IExceptionCaughtSignal
        where TDto : DtoBase<TQuery>, new() 
        where TEntity : EntityBase<TKey>, new()
    {
        #region 字段&属性
        private readonly RepositoryBase<TEntity, TKey> _repository;

        /// <summary>
        /// 仓储
        /// </summary>
        protected internal virtual IRepositoryBase<TEntity, TKey> Repository
        {
            get { return _repository; }
        }

        IDbConnection IDbSession.Connection
        {
            get { return _repository.Connection; }
        }

        IDbTransaction IDbSession.Transaction
        {
            get { return _repository.Transaction; }
            set { _repository.Transaction = value; }
        }
        

        public ServiceBase(IRepositoryBase<TEntity, TKey> repository)
        {
            _repository = repository as RepositoryBase<TEntity, TKey>;
        }
        #endregion

        #region IDisposable Support
        /// <summary>
        /// 关闭数据库连接，如果useConnectionPool为true
        /// </summary>
        public void CloseService()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            (_repository as IDisposable).Dispose();
        }
        #endregion

        #region 默认实现

        [CatchSyncException("插入数据异常")]
        public void Insert(TDto dto)
        {
            TEntity entity = DtoToEntity(dto);
            entity.Init();
            Repository.InsertOne(entity);
        }

        [CatchSyncException("删除数据异常")]
        public void Remove(TDto dto)
        {
            Repository.DeleteOne(DtoToEntity(dto));
        }

        [CatchSyncException("获取一条数据")]
        public TDto GetOneById<TId>(TId id, string idColumnName)
        {
            return EntityToDto(Repository.QueryOneById(id, idColumnName));
        }

        [CatchSyncException("获取全部数据异常")]
        public List<TDto> GetAll(string orderBy = null)
        {
            List<TDto> dtos = new List<TDto>();
            foreach (var item in Repository.QueryAll(orderBy))
            {
                dtos.Add(EntityToDto(item));
            }
            return dtos;
        }

        [CatchSyncException("获取全部记录条数异常")]
        public long GetRecordCount()
        {
            return Repository.QueryRecordCount();
        }

        [CatchSyncException("更新数据异常")]
        public void Update(TDto dto)
        {
            Repository.UpdateOne(DtoToEntity(dto));
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
        /// 将Dto转换为实体
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected virtual TEntity DtoToEntity(TDto dto)
        {
            return dto == null ? null : ObjectClone<TDto, TEntity>.ShallowCopy(dto);
        }
    }

}
