using Dapper;

namespace IceCoffee.DbCore.SqliteTypeHandlers
{
    public abstract class SqliteTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        // Parameters are converted by Microsoft.Data.Sqlite
        public override void SetValue(System.Data.IDbDataParameter parameter, T value)
            => parameter.Value = value;
    }
}