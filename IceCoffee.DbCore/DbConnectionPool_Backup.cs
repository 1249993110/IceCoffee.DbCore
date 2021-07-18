//#if NET45

//#region net45
//using IceCoffee.Common.Pools;
//using IceCoffee.DbCore.ExceptionCatch;
//using System;
//using System.Data;
//using System.Data.Common;

//namespace IceCoffee.DbCore
//{
//    /// <summary>
//    /// 数据库连接池
//    /// </summary>
//    public class DbConnectionPool : LocklessPool<IDbConnection>
//    {
//        private readonly string _connectionString;

//        private readonly DbProviderFactory _factory;

//        /// <summary>
//        /// 连接字符串
//        /// </summary>
//        public string ConnectionString => _connectionString;

//        /// <summary>
//        /// 实例化数据库连接池
//        /// </summary>
//        /// <param name="connectionString"></param>
//        /// <param name="factory"></param>
//        /// <param name="maxConnectionCount">默认大小CPU*2</param>
//        public DbConnectionPool(string connectionString, DbProviderFactory factory, int maxConnectionCount = 0) : base(maxConnectionCount)
//        {
//            this._connectionString = connectionString;
//            this._factory = factory;
//        }

//        /// <summary>
//        /// <inheritdoc />
//        /// </summary>
//        /// <returns></returns>
//        protected override IDbConnection Create()
//        {
//            var conn = _factory?.CreateConnection();
//            if (conn == null)
//            {
//                throw new DbCoreException("连接创建失败！请检查驱动是否正常");
//            }

//            conn.ConnectionString = _connectionString;

//            conn.Open();

//            return conn;
//        }

//        /// <summary>
//        /// 申请时检查是否打开
//        /// </summary>
//        /// <returns></returns>
//        public override IDbConnection Get()
//        {
//            var conn = base.Get();
//            if (conn.State == ConnectionState.Closed)
//            {
//                conn.Open();
//            }

//            return conn;
//        }

//        /// <summary>
//        /// <inheritdoc />
//        /// </summary>
//        /// <param name="dbConnection"></param>
//        public override void Return(IDbConnection dbConnection)
//        {
//            if(dbConnection.State == ConnectionState.Open)
//            {
//                base.Return(dbConnection);
//            }
//            else
//            {
//                dbConnection.Dispose();
//            }
//        }
//    }
//}
//#endregion

//#else

//#region net461;netstandard2.0;netcoreapp3.1
//using IceCoffee.Common.Pools;
//using IceCoffee.DbCore.ExceptionCatch;
//using Microsoft.Extensions.ObjectPool;
//using System;
//using System.Data;
//using System.Data.Common;

//namespace IceCoffee.DbCore
//{
//    /// <summary>
//    /// The default <see cref="ObjectPoolProvider"/>.
//    /// </summary>
//    public class DbConnectionObjectPoolProvider : DefaultObjectPoolProvider
//    {
//        public virtual IObjectPool<IDbConnection> Create(IPooledObjectPolicy<IDbConnection> policy)
//        {
//            return base.Create(policy);
//        }
//    }


//    public class DbConnectionPool1 : ObjectPool<IDbConnection>, IObjectPool<IDbConnection>
//    {
//        private readonly ObjectPool<IDbConnection> _pool;

//        public DbConnectionPool1(string connectionString, DbProviderFactory factory, int maxConnectionCount = 0)
//        {
//            var objectPProvider = new DefaultObjectPoolProvider() 
//            { 
//                MaximumRetained = maxConnectionCount 
//            };

//            _pool = objectPProvider.Create(new ChannelObjectPoolPolicy(connectionString, factory));
//        }

//        public override IDbConnection Get()
//        {
//            throw new NotImplementedException();
//        }

//        public override void Return(IDbConnection obj)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class ChannelObjectPoolPolicy : IPooledObjectPolicy<IDbConnection>
//    {
//        private readonly string _connectionString;

//        private readonly DbProviderFactory _factory;

//        /// <summary>
//        /// 连接字符串
//        /// </summary>
//        public string ConnectionString => _connectionString;

//        public ChannelObjectPoolPolicy(string connectionString, DbProviderFactory factory)
//        {
//            _factory = factory;
//            _connectionString = connectionString;
//        }

//        /// <summary>
//        /// <inheritdoc/>
//        /// </summary>
//        /// <returns></returns>
//        public IDbConnection Create()
//        {
//            var conn = _factory?.CreateConnection();
//            if (conn == null)
//            {
//                throw new DbCoreException("连接创建失败！请检查驱动是否正常");
//            }

//            conn.ConnectionString = _connectionString;

//            conn.Open();

//            return conn;
//        }

//        /// <summary>
//        /// <inheritdoc/>
//        /// </summary>
//        /// <param name="dbConnection"></param>
//        /// <returns></returns>
//        public bool Return(IDbConnection dbConnection)
//        {
//            if (dbConnection.State != ConnectionState.Open)
//            {
//                dbConnection.Dispose();
//                return false;
//            }

//            return true;
//        }
//    }
//}
//#endregion

//#endif
