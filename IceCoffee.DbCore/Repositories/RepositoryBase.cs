using Dapper;
using IceCoffee.DbCore.ExceptionCatch;
using System.Data;

namespace IceCoffee.DbCore.Repositories
{
    /// <summary>
    /// RepositoryBase
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        public readonly DbConnectionInfo DbConnectionInfo;

        /// <summary>
        /// 实例化 RepositoryBase
        /// </summary>
        /// <param name="dbConnectionInfo"></param>
        public RepositoryBase(DbConnectionInfo dbConnectionInfo)
        {
            this.DbConnectionInfo = dbConnectionInfo;
        }

        /// <summary>
        /// 得到工作单元, 默认返回 <see cref="UnitOfWork.Default"/>
        /// </summary>
        /// <returns></returns>
        protected virtual IUnitOfWork GetUnitOfWork()
        {
            var unitOfWork = UnitOfWork.Default;
            if (unitOfWork.IsExplicitSubmit)
            {
                if (unitOfWork.DbConnection == null)
                {
                    unitOfWork.EnterContext(DbConnectionInfo);
                }
                // 判断是否跨数据库使用工作单元
                else if (unitOfWork.DbConnectionInfo != this.DbConnectionInfo)
                {
                    throw new DbCoreException("工作单元无法跨数据库使用");
                }
            }

            return unitOfWork;
        }

        /// <summary>
        /// 执行参数化 SQL 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns>受影响的行数</returns>
        protected virtual int Execute(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                int result = conn.Execute(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.Execute", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 异步执行参数化 SQL 语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns>受影响的行数</returns>
        protected virtual async Task<int> ExecuteAsync(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                int result = await conn.ExecuteAsync(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 执行参数化 SQL 语句, 选择单个值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        protected virtual TReturn ExecuteScalar<TReturn>(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                var result = conn.ExecuteScalar<TReturn>(sql, param, tran, commandType: CommandType.Text);

                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteScalar", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 异步执行参数化 SQL 语句, 选择单个值
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        protected virtual async Task<TReturn> ExecuteScalarAsync<TReturn>(string sql, object? param = null, bool useTransaction = false)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction ?? (useTransaction ? conn.BeginTransaction() : null);

            try
            {
                var result = await conn.ExecuteScalarAsync<TReturn>(sql, param, tran, commandType: CommandType.Text);
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Commit();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (useTransaction && unitOfWork.IsExplicitSubmit == false)
                {
                    tran?.Rollback();
                }

                throw new DbCoreException("Error in RepositoryBase.ExecuteScalarAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TEntity> Query<TEntity>(string sql, object? param = null)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return conn.Query<TEntity>(sql, param, tran, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.Query", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 异步执行查询语句
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object? param = null)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return await conn.QueryAsync<TEntity>(sql, param, tran, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.QueryAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TReturn> ExecProcedure<TReturn>(string procName, DynamicParameters parameters)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return conn.Query<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
                    transaction: tran, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.ExecProcedure", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }

        /// <summary>
        /// 异步执行存储过程
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual async Task<IEnumerable<TReturn>> ExecProcedureAsync<TReturn>(string procName, DynamicParameters parameters)
        {
            var unitOfWork = GetUnitOfWork();
            var conn = unitOfWork.DbConnection ?? DbConnectionFactory.GetConnectionFromPool(DbConnectionInfo);
            var tran = unitOfWork.DbTransaction;
            try
            {
                return await conn.QueryAsync<TReturn>(new CommandDefinition(commandText: procName, parameters: parameters,
                    transaction: tran, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new DbCoreException("Error in RepositoryBase.ExecProcedureAsync", ex);
            }
            finally
            {
                if (unitOfWork.IsExplicitSubmit == false)
                {
                    DbConnectionFactory.CollectDbConnectionToPool(conn);
                }
            }
        }
    }
}