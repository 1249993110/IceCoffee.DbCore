namespace IceCoffee.DbCore.CodeGenerator
{
    internal static class Utils
    {
        public static string GetBasicRepositoryName(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.MySQL:
                    return "MySqlRepository";

                case DatabaseType.SQLServer:
                    return "SqlServerRepository";

                case DatabaseType.PostgreSQL:
                    return "PostgreSqlRepository";

                case DatabaseType.SQLite:
                    return "SqliteRepository";

                default:
                    throw new ArgumentException("Undefined database type: " + databaseType.ToString());
            }
        }
    }
}