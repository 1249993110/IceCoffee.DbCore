using IceCoffee.DbCore.CodeGenerator.Models;
using System;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator
{
    internal class DefaultGenerator : IGenerator
    {
        private readonly string _namespacePrefix;
        private readonly string _basicRepository;
        private readonly string _dbConnectionInfoTypeName;

        public DefaultGenerator(string namespacePrefix, string basicRepository, string dbConnectionInfoTypeName)
        {
            _namespacePrefix = namespacePrefix;
            _basicRepository = basicRepository;
            _dbConnectionInfoTypeName = dbConnectionInfoTypeName;
        }

        public virtual string GenerateEntityClass(EntityInfo entityInfo)
        {
            var sb = new StringBuilder()
                .AppendLine($"namespace {_namespacePrefix}.Entities")
                .AppendLine("{")
                .AppendLine("    /// <summary>")
                .AppendLine("    /// ")
                .AppendLine("    /// </summary>");

            string className = GetClassName(entityInfo);
            if (className != entityInfo.Name)
            {
                sb.AppendLine($"    {GetTableAttribute(entityInfo)}");
            }

            sb.AppendLine($"    public class {className}")
                .AppendLine("    {");

            foreach (var fieldInfo in entityInfo.FieldInfos)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// ");
                sb.AppendLine("        /// </summary>");
                if (fieldInfo.IsPrimaryKey)
                {
                    sb.AppendLine("        [PrimaryKey]");
                }

                string propName = GetPropertyName(fieldInfo);
                if (propName != fieldInfo.ColumnName)
                {
                    sb.AppendLine($"        {GetColumnAttribute(fieldInfo)}");
                }

                string cSharpType = GetCSharpType(fieldInfo);
                sb.AppendFormat("        public {0} {1} {2}", cSharpType, propName, "{ get; set; }")
                    .AppendLine();
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        public virtual string GenerateIRepository(EntityInfo entityInfo)
        {
            StringBuilder sb = new StringBuilder()
                .AppendLine($"using {_namespacePrefix}.Entities;")
                .AppendLine()
                .AppendLine($"namespace {_namespacePrefix}.IRepositories")
                .AppendLine("{")
                .AppendLine($"    public interface I{GetRepositoryName(entityInfo)} : IRepository<{GetClassName(entityInfo)}>")
                .AppendLine("    {")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        public virtual string GenerateRepository(EntityInfo entityInfo)
        {
            string repositoryName = GetRepositoryName(entityInfo);

            StringBuilder sb = new StringBuilder()
                .AppendLine($"using {_namespacePrefix}.Entities;")
                .AppendLine($"using {_namespacePrefix}.IRepositories;")
                .AppendLine()
                .AppendLine($"namespace {_namespacePrefix}.Repositories")
                .AppendLine("{")
                .AppendLine($"    public class {repositoryName} : {_basicRepository}<{GetClassName(entityInfo)}>, I{repositoryName}")
                .AppendLine("    {")
                .AppendLine($"        public {repositoryName}({_dbConnectionInfoTypeName} dbConnectionInfo) : base(dbConnectionInfo)")
                .AppendLine("        {")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        public virtual string GetClassName(EntityInfo entityInfo)
        {
            return entityInfo.Name;
        }

        public virtual string GetTableAttribute(EntityInfo entityInfo)
        {
            return "[Table(\"" + entityInfo.Name + "\")]";
        }

        public virtual string GetRepositoryName(EntityInfo entityInfo)
        {
            return $"{(entityInfo.IsView ? "V" : string.Empty)}{GetClassName(entityInfo).Substring(2)}Repository";
        }

        public virtual string GetCSharpType(FieldInfo fieldInfo)
        {
            string typeName = fieldInfo.TypeName.ToLower();
            string prefix = fieldInfo.IsNullable ? "?" : string.Empty;

            switch (typeName)
            {
                case "bigint":
                    return "long" + prefix;
                case "binary":
                case "varbinary":
                case "image":
                    return "byte[]";
                case "bool":
                case "bit":
                    return "bool" + prefix;
                case "char":
                case "bpchar":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "date":
                case "datetime":
                case "timestamp":
                    return "DateTime" + prefix;
                case "datetimeoffset":
                    return "DateTimeOffset" + prefix;
                case "decimal":
                case "numeric":
                    return "decimal" + prefix;
                case "float":
                    return "float" + prefix;
                case "real":
                    return "double" + prefix;
                case "int":
                case "int4":
                case "int identity":
                case "integer":
                    return "int" + prefix;
                case "smallint":
                    return "short" + prefix;
                case "tinyint":
                    return "byte" + prefix;
                case "uniqueidentifier":
                case "uuid":
                    return "Guid" + prefix;

                default:
                    throw new Exception("Undefined type: " + typeName);
            }
        }

        public virtual string GetPropertyName(FieldInfo fieldInfo)
        {
            string name = fieldInfo.ColumnName;
            if (name.ToUpper().StartsWith("FK_"))
            {
                name = name.Substring(3);
            }

            return name;
        }

        public virtual string GetColumnAttribute(FieldInfo fieldInfo)
        {
            string name = fieldInfo.ColumnName;
            return "[Column(\"[" + name + "]\")]";
        }
    }
}