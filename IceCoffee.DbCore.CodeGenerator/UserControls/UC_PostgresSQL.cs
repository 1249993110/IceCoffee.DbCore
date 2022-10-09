using IceCoffee.DbCore.CodeGenerator.Models;
using IceCoffee.DbCore.Utils;
using System.Data;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator.UserControls
{
    public partial class UC_PostgresSQL : UserControl, IView
    {
        private DbConnectionInfo _dbConnectionInfo;

        public UC_PostgresSQL()
        {
            InitializeComponent();
        }

        public string Label => "PostgresSQL";

        public int Sort => 1;

        private static string GetClassName(EntityStructure es)
        {
            string entityName = es.EntityName;

            int len = entityName.Length;
            var sb = new StringBuilder(len);
            sb.Append(es.IsView ? "V_" : "T_");
            sb.Append(char.ToUpper(entityName[2]));

            if (len >= 3)
            {
                for (int i = 3; i < len; i++)
                {
                    if (entityName[i] == '_')
                    {
                        i++;
                        sb.Append(char.ToUpper(entityName[i]));
                        continue;
                    }
                    else
                    {
                        sb.Append(entityName[i]);
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetCSharpType(string typeName, bool isNullable)
        {
            string nullable = isNullable ? "?" : string.Empty;

            switch (typeName)
            {
                case "binary":
                case "varbinary":
                case "image":
                    return "byte[]";

                case "bool":
                    return "bool" + nullable;

                case "char":
                case "bpchar":
                case "varchar":
                case "text":
                    return "string";

                case "timestamp":
                    return "DateTime" + nullable;

                case "decimal":
                case "numeric":
                    return "decimal" + nullable;

                case "int4":
                    return "int" + nullable;

                case "uuid":
                    return "Guid" + nullable;

                default:
                    throw new Exception("未定义的类型" + typeName);
            }
        }

        private static string GetIRepositoryName(EntityStructure es)
        {
            string className = GetClassName(es);
            return $"I{(es.IsView ? "V" : string.Empty)}{className.Substring(2)}Repository";
        }

        private static string GetPropName(string columnName)
        {
            if (columnName.ToLower().StartsWith("fk_"))
            {
                columnName = columnName.Substring(3);
            }

            int len = columnName.Length;
            var sb = new StringBuilder(len);

            sb.Append(char.ToUpper(columnName[0]));

            if (len >= 1)
            {
                for (int i = 1; i < len; i++)
                {
                    if (columnName[i] == '_')
                    {
                        i++;
                        sb.Append(char.ToUpper(columnName[i]));
                        continue;
                    }
                    else
                    {
                        sb.Append(columnName[i]);
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetRepositoryName(EntityStructure es)
        {
            string className = GetClassName(es);
            return $"{(es.IsView ? "V" : string.Empty)}{className.Substring(2)}Repository";
        }

        private void button_checkAndUncheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView_entities.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            try
            {
                GetTablesAndViews();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_generate_Click(object sender, EventArgs e)
        {
            try
            {
                InitDirectory();

                GenerateCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateDirectory(string dir)
        {
            Directory.CreateDirectory(Path.Combine(this.textBox_rootPath.Text, dir));
        }

        private void GenerateCode()
        {
            foreach (ListViewItem item in this.listView_entities.CheckedItems)
            {
                string entityName = item.Text;
                bool isView = item.Group.Header == "视图";
                var es = new EntityStructure()
                {
                    EntityName = entityName,
                    FieldInfos = GetFieldsInfo(entityName),
                    IsView = isView
                };

                string entityClass = GenerateEntityClass(es);
                string path = Path.Combine(this.textBox_rootPath.Text, $"Entities/{GetClassName(es)}.cs");
                File.WriteAllText(path, entityClass, Encoding.UTF8);

                string iRepository = GenerateIRepository(es);
                path = Path.Combine(this.textBox_rootPath.Text, $"IRepositories/{GetIRepositoryName(es)}.cs");
                File.WriteAllText(path, iRepository, Encoding.UTF8);

                string repository = GenerateRepository(es);
                path = Path.Combine(this.textBox_rootPath.Text, $"Repositories/{GetRepositoryName(es)}.cs");
                File.WriteAllText(path, repository, Encoding.UTF8);
            }

            MessageBox.Show("生成成功");
        }

        private string GenerateEntityClass(EntityStructure es)
        {
            var sb = new StringBuilder(2048)
                .AppendLine($"namespace {this.textBox_namespacePrefix.Text}.Entities")
                .AppendLine("{")
                .AppendLine("    /// <summary>")
                .AppendLine("    /// ")
                .AppendLine("    /// </summary>")
                .AppendLine($"    [Table(\"{es.EntityName}\")]")
                .AppendLine($"    public class {GetClassName(es)}")
                .AppendLine("    {");

            foreach (var field in es.FieldInfos)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// ");
                sb.AppendLine("        /// </summary>");
                if (field.IsPrimaryKey)
                {
                    sb.AppendLine("        [PrimaryKey]");
                }

                sb.AppendLine($"        [Column(\"{field.ColumnName}\")]");

                string cSharpType = GetCSharpType(field.TypeName, field.IsNullable);
                sb.AppendFormat("        public {0} {1} {2}", cSharpType, GetPropName(field.ColumnName), "{ get; set; }")
                    .AppendLine();
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateIRepository(EntityStructure es)
        {
            string namespacePrefix = this.textBox_namespacePrefix.Text;
            StringBuilder sb = new StringBuilder(256)
                .AppendLine($"using {namespacePrefix}.Entities;")
                .AppendLine()
                .AppendLine($"namespace {namespacePrefix}.IRepositories")
                .AppendLine("{")
                .AppendLine($"    public interface {GetIRepositoryName(es)} : IRepository<{GetClassName(es)}>")
                .AppendLine("    {")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private string GenerateRepository(EntityStructure es)
        {
            string namespacePrefix = this.textBox_namespacePrefix.Text;
            string basicRepository = "PostgreSqlRepository";
            string repositoryName = GetRepositoryName(es);

            StringBuilder sb = new StringBuilder(256)
                .AppendLine($"using {namespacePrefix}.Entities;")
                .AppendLine($"using {namespacePrefix}.IRepositories;")
                .AppendLine()
                .AppendLine($"namespace {namespacePrefix}.Repositories")
                .AppendLine("{")
                .AppendLine($"    public class {repositoryName} : {basicRepository}<{GetClassName(es)}>, I{repositoryName}")
                .AppendLine("    {")
                .AppendLine($"        public {repositoryName}({this.textBox_dbConnType.Text} dbConnectionInfo) : base(dbConnectionInfo)")
                .AppendLine("        {")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private IEnumerable<FieldInfo> GetFieldsInfo(string tableName)
        {
            var fields = DBHelper.QueryAny<FieldInfo>(_dbConnectionInfo, $"SELECT column_name AS ColumnName,udt_name AS TypeName,(CASE is_nullable WHEN 'NO' THEN FALSE ELSE TRUE END) AS IsNullable FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '{tableName}'");

            var primaryKeys = DBHelper.QueryAny<FieldInfo>(_dbConnectionInfo, $"SELECT a.attname AS ColumnName FROM pg_index AS i JOIN pg_attribute AS a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey) WHERE i.indrelid = '{tableName}'::regclass AND i.indisprimary").Select(s => s.ColumnName);

            foreach (var field in fields)
            {
                string columnName = field.ColumnName;

                if (primaryKeys.Contains(columnName))
                {
                    field.IsPrimaryKey = true;
                }
            }

            return fields;
        }

        private void GetTablesAndViews()
        {
            string connStr = this.textBox_dbConnectionString.Text;
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("数据库连接串不能为空！");
            }

            _dbConnectionInfo = new DbConnectionInfo()
            {
                ConnectionString = connStr,
                DatabaseType = DatabaseType.PostgreSQL
            };

            var tables = DBHelper.QueryAny<T_Entity>(_dbConnectionInfo, "SELECT tablename AS Name FROM pg_tables WHERE schemaname='public'");
            var views = DBHelper.QueryAny<T_Entity>(_dbConnectionInfo, "SELECT viewname AS Name FROM pg_views WHERE schemaname='public'");

            this.listView_entities.BeginUpdate();
            this.listView_entities.Items.Clear();
            foreach (var item in tables)
            {
                this.listView_entities.Items.Add(new ListViewItem(item.Name, this.listView_entities.Groups[0]) { Checked = true });
            }
            foreach (var item in views)
            {
                this.listView_entities.Items.Add(new ListViewItem(item.Name, this.listView_entities.Groups[1]) { Checked = true });
            }

            this.listView_entities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.listView_entities.EndUpdate();
        }
        private void InitDirectory()
        {
            CreateDirectory("Entities");
            CreateDirectory("IRepositories");
            CreateDirectory("Repositories");
        }

        private void listView_entities_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listView_entities.SelectedItems.Count > 0)
                {
                    var selectedItem = this.listView_entities.SelectedItems[0];
                    string entityName = selectedItem.Text;

                    this.listView_fields.BeginUpdate();
                    this.listView_fields.Items.Clear();

                    var fieldInfos = GetFieldsInfo(entityName);
                    foreach (var item in fieldInfos)
                    {
                        this.listView_fields.Items.Add(new ListViewItem(new string[]
                        {
                            item.ColumnName,
                            item.TypeName,
                            item.IsNullable ? "是" : "否",
                            item.IsPrimaryKey ? "是" : "否"
                        }));
                    }

                    this.listView_fields.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.listView_fields.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                    this.listView_fields.EndUpdate();

                    this.Preview(entityName, fieldInfos, selectedItem.Group.Header == "视图");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Preview(string entityName, IEnumerable<FieldInfo> fieldInfos, bool isView)
        {
            var entityStructure = new EntityStructure()
            {
                EntityName = entityName,
                FieldInfos = fieldInfos,
                IsView = isView
            };

            string entityClass = GenerateEntityClass(entityStructure);
            this.textBox_preview.Text = entityClass;
        }
    }
}