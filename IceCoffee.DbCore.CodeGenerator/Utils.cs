namespace IceCoffee.DbCore.CodeGenerator
{
    internal static class Utils
    {
        public static void InitDirectory(string rootDir)
        {
            Directory.CreateDirectory(Path.Combine(rootDir, "Entities"));
            Directory.CreateDirectory(Path.Combine(rootDir, "IRepositories"));
            Directory.CreateDirectory(Path.Combine(rootDir, "Repositories"));

        }
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