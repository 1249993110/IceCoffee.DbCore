using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;

namespace IceCoffee.DbCore
{
    public class DbConnectionPool<TDbConnection> : ObjectPool<TDbConnection> where TDbConnection : IDbConnection, new()
    {
        private readonly uint maxConnectionCount;

        private readonly string connectionString;

        public DbConnectionPool(string connectionString, uint maxConnectionCount = 50)
        {
            this.connectionString = connectionString;
            this.maxConnectionCount = maxConnectionCount;
        }

        protected override TDbConnection Create()
        {
            TDbConnection dbConnection = new TDbConnection
            {
                ConnectionString = connectionString
            };
            return dbConnection;
        }

        public override void Add(TDbConnection item)
        {
            if(item == null)
            {
                return;
            }

            if (base.Count > maxConnectionCount)
            {
                item.Dispose();
            }
            else if(item.State != ConnectionState.Open)// 返回是否有效。无效对象将会被抛弃
            {
                base.Add(item);
            }
        }
    }
}
