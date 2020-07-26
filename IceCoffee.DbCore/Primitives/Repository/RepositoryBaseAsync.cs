//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Dapper;
//using IceCoffee.DbCore.OptionalAttributes;
//using IceCoffee.DbCore.Primitives.Entity;

//namespace IceCoffee.DbCore.Primitives.Repository
//{
//    public abstract partial class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : EntityBase<TKey>
//    {
//        #region Insert
//        public async Task<int> InsertOneAsync(TEntity entity)
//        {
//            return await Task.Run(() => { return InsertOne(entity); });
//        }
//        public async Task<int> InsertListAsync(IEnumerable<TEntity> entitys)
//        {
//            return await Task.Run(() => { return InsertList(entitys); });
//        }
//        #endregion

//        #region Delete
//        public async Task<int> DeleteAnyAsync(string conditions, object param = null)
//        {
//            return await Task.Run(() => { return DeleteAny(conditions, param); });
//        }
//        public async Task<int> DeleteOneAsync(TEntity entity)
//        {
//            return await Task.Run(() => { return DeleteOne(entity); });
//        }
//        public async Task<int> DeleteListAsync(IEnumerable<TEntity> entitys)
//        {
//            return await Task.Run(() => { return DeleteList(entitys); });
//        }
//        public async Task<int> DeleteOneByKeyAsync(TKey key)
//        {
//            return await Task.Run(() => { return DeleteOneByKey(key); });
//        }
//        public async Task<int> DeleteListByKeysAsync(IEnumerable<TKey> keys)
//        {
//            return await Task.Run(() => { return DeleteListByKeys(keys); });
//        }
//        public async Task<int> DeleteOneByIdAsync<TId>(TId id, string idColumnName)
//        {
//            return await Task.Run(() => { return DeleteOneById(id, idColumnName); });
//        }
//        public async Task<int> DeleteListByIdsAsync<TId>(IEnumerable<TId> ids, string idColumnName)
//        {
//            return await Task.Run(() => { return DeleteListByIds(ids, idColumnName); });
//        }
//        public async Task<int> DeleteAllAsync()
//        {
//            return await Task.Run(() => { return DeleteAll(); });
//        }
//        #endregion

//        #region Query
//        public async Task<IEnumerable<TEntity>> QueryAnyAsync(string columnNames, string conditions, string orderby, object param = null)
//        {
//            return await Task.Run(() => { return QueryAny(columnNames, conditions, orderby, param); });
//        }
//        public async Task<TEntity> QueryOneByKeyAsync(TKey key)
//        {
//            return await Task.Run(() => { return QueryOneByKey(key); });
//        }
//        public async Task<IEnumerable<TEntity>> QueryListByKeysAsync(IEnumerable<TKey> keys)
//        {
//            return await Task.Run(() => { return QueryListByKeys(keys); });
//        }
//        public async Task<IEnumerable<TEntity>> QueryAllAsync(string orderby = null)
//        {
//            return await Task.Run(() => { return QueryAll(orderby); });
//        }
//        public async Task<TEntity> QueryOneByIdAsync<TId>(TId id, string idColumnName)
//        {
//            return await Task.Run(() => { return QueryOneById(id, idColumnName); });
//        }
//        public async Task<IEnumerable<TEntity>> QueryListByIdAsync<TId>(TId id, string idColumnName)
//        {
//            return await Task.Run(() => { return QueryListById(id, idColumnName); });
//        }
//        public async Task<IEnumerable<TEntity>> QueryListPagedAsync(int pageNumber, int rowsPerPage,
//            string conditions = null, string orderby = null, object param = null)
//        {
//            return await Task.Run(() =>
//            {
//                return QueryListPaged(pageNumber, rowsPerPage, conditions, orderby, param);
//            });
//        }
//        public async Task<long> QueryRecordCountAsync(string conditions = null, object param = null)
//        {
//            return await Task.Run(() => { return QueryRecordCount(conditions, param); });
//        }
//        #endregion

//        #region Update
//        public async Task<int> UpdateAnyAsync(string setClause, string conditions, object param)
//        {
//            return await Task.Run(() => { return UpdateAny(setClause, conditions, param); });
//        }
//        public async Task<int> UpdateOneAsync(TEntity entity)
//        {
//            return await Task.Run(() => { return UpdateOne(entity); });
//        }
//        public async Task<int> UpdateListAsync(IEnumerable<TEntity> entitys)
//        {
//            return await Task.Run(() => { return UpdateList(entitys); });
//        }
//        public async Task<int> UpdateByIdAsync<TId>(TEntity entity, TId id, string idColumnName)
//        {
//            return await Task.Run(() => { return UpdateById(entity, id, idColumnName); });
//        }
//        public async Task<int> UpdateOneColByIdAsync<TId, TValue>(TId id, TValue value, string idColumnName, string valueColumnName)
//        {
//            return await Task.Run(() => { return UpdateOneColById(id, value, idColumnName, valueColumnName); });
//        }
//        #endregion
//    }
//}
