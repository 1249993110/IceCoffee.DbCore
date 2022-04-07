using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IceCoffee.DbCore.Tools.Models.CodeGenerator;
using IceCoffee.DbCore;
using IceCoffee.DbCore.Utils;

namespace IceCoffee.DbCore.Tools.UserControls
{
    //SELECT * FROM ::fn_listextendedproperty (NULL, 'SCHEMA', 'dbo', 'TABLE', 'T_Day', NULL, default) T_Day
    public partial class UC_CodeGenerator : UserControl
    {
        private DbConnectionInfo _dbConnectionInfo;

        private readonly ConcurrentDictionary<string, EntityStructure> _entityStructureDict = new ConcurrentDictionary<string, EntityStructure>();

        public UC_CodeGenerator()
        {
            InitializeComponent();
            base.Text = "代码生成器";
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            try
            {
                GetDatabaseObjects();
                GetDatabaseObjectColumns();
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

        private void GenerateCode()
        {
            foreach (ListViewItem item in this.listView_entities.CheckedItems)
            {
                string entityName = item.Text;
                bool isView = item.Group.Header == "视图";
                EntityStructure entityStructure = new EntityStructure()
                {
                    EntityName = entityName,
                    ColumnStructures = _entityStructureDict[entityName].ColumnStructures,
                    IsView = isView
                };

                string subPath = this.textBox_subPath.Text;

                string entityClass = GenerateEntityClass(entityStructure);
                string path = Path.Combine(this.textBox_rootPath.Text, $"Entities/{subPath}/{entityName}.cs");
                File.WriteAllText(path, entityClass, Encoding.UTF8);

                string iRepositoryName;
                string iRepository = GenerateIRepository(entityName, isView, out iRepositoryName);
                path = Path.Combine(this.textBox_rootPath.Text, $"IRepositories/{subPath}/{iRepositoryName}.cs");
                File.WriteAllText(path, iRepository, Encoding.UTF8);

                string repositoryName;
                string repository = GenerateRepository(entityName, isView, out repositoryName);
                path = Path.Combine(this.textBox_rootPath.Text, $"Repositories/{subPath}/{repositoryName}.cs");
                File.WriteAllText(path, repository, Encoding.UTF8);
            }

            MessageBox.Show("生成成功");
        }
 

        private string GenerateEntityClass(EntityStructure es)
        {
            StringBuilder sb = new StringBuilder(2048)
                .AppendLine("using System;")
                .AppendLine("using IceCoffee.DbCore.Primitives.Entity;")
                .AppendLine("using IceCoffee.DbCore.OptionalAttributes;")
                .AppendLine()
                .AppendLine($"namespace {this.textBox_namespacePrefix.Text}.Entities.{this.textBox_subPath.Text}")
                .AppendLine("{")
                .AppendLine("    /// <summary>")
                .AppendLine("    /// ")
                .AppendLine("    /// </summary>")
                .AppendLine($"    public class {es.EntityName} : EntityBase")
                .AppendLine("    {");

            int length = es.ColumnStructures.Count;
            for (int i = 0; i < length; ++i)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// ");
                sb.AppendLine("        /// </summary>");
                if (es.ColumnStructures[i].IsPrimaryKey)
                {
                    sb.AppendLine("        [PrimaryKey]");
                }

                string cSharpType = GetCSharpType(es.ColumnStructures[i].TypeName, es.ColumnStructures[i].IsNullable);
                sb.AppendFormat("        public {0} {1} {2}", cSharpType, es.ColumnStructures[i].ColumnName, "{ get; set; }").AppendLine();
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GenerateIRepository(string entityName, bool isView, out string repositoryName)
        {
            if (entityName.StartsWith("T_") || entityName.StartsWith("V_"))
            {
                repositoryName = $"I{entityName.Substring(2)}Repository";
            }
            else
            {
                repositoryName = $"I{entityName}Repository";
            }

            if (isView)
            {
                repositoryName = repositoryName.Insert(1, "V");
            }

            string namespacePrefix = this.textBox_namespacePrefix.Text;
            StringBuilder sb = new StringBuilder(256)
                .AppendLine($"using {namespacePrefix}.Entities.{this.textBox_subPath.Text};")
                .AppendLine()
                .AppendLine($"namespace {namespacePrefix}.IRepositories.{this.textBox_subPath.Text}")
                .AppendLine("{")
                .AppendLine($"    public interface {repositoryName} : IceCoffee.DbCore.Repositories.IRepository<{entityName}>")
                .AppendLine("    {")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private string GenerateRepository(string entityName, bool isView, out string repositoryName)
        {
            if (entityName.StartsWith("T_") || entityName.StartsWith("V_"))
            {
                repositoryName = $"{entityName.Substring(2)}Repository";
            }
            else
            {
                repositoryName = $"{entityName}Repository";
            }

            if (isView)
            {
                repositoryName = repositoryName.Insert(0, "V");
            }

            string namespacePrefix = this.textBox_namespacePrefix.Text;
            string basicRepository = this.textBox_basicRepository.Text;
            int index = basicRepository.IndexOf("<");
            if (index != -1)
            {
                basicRepository = basicRepository.Substring(0, index);
            }

            StringBuilder sb = new StringBuilder(256)
                .AppendLine($"using {namespacePrefix}.Entities.{this.textBox_subPath.Text};")
                .AppendLine($"using {namespacePrefix}.IRepositories.{this.textBox_subPath.Text};")
                .AppendLine()
                .AppendLine($"namespace {namespacePrefix}.Repositories.{this.textBox_subPath.Text}")
                .AppendLine("{")
                .AppendLine($"    public class {repositoryName} : {basicRepository}<{entityName}>, I{repositoryName}")
                .AppendLine("    {")
                .AppendLine($"        public {repositoryName}({this.textBox_dbConnType.Text} dbConnectionInfo) : base(dbConnectionInfo)")
                .AppendLine("        {")
                .AppendLine("        }")
                .AppendLine("    }")
                .AppendLine("}");

            return sb.ToString();
        }

        private static string GetCSharpType(string sqlserverType, bool isNullable)
        {
            string nullable = isNullable ? "?" : string.Empty;

            switch (sqlserverType)
            {
                case "bigint":
                    return "long" + nullable;
                case "binary":
                case "varbinary":
                case "image":
                    return "byte[]";
                case "bit":
                    return "bool" + nullable;
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    return "string";
                case "date":
                case "datetime":
                    return "DateTime" + nullable;
                case "datetimeoffset":
                    return "DateTimeOffset" + nullable;
                case "decimal":
                case "numeric":
                    return "decimal" + nullable;
                case "float":
                    return "float" + nullable;
                case "int":
                    return "int" + nullable;
                case "smallint":
                    return "short" + nullable;
                case "tinyint":
                    return "byte" + nullable;
                case "uniqueidentifier":
                    return "Guid" + nullable;
                default:
                    throw new Exception("未定义的类型" + sqlserverType);
            }
        }

        private void InitDirectory()
        {
            CreateDirDirectory("Entities");
            CreateDirDirectory("Entities");
            CreateDirDirectory("IRepositories");
            CreateDirDirectory("IRepositories");
            CreateDirDirectory("Repositories");
            CreateDirDirectory("Repositories");
        }

        private void CreateDirDirectory(string dir)
        {
            dir = Path.Combine(this.textBox_rootPath.Text, dir, this.textBox_subPath.Text);
            Directory.CreateDirectory(dir);
        }

        private void GetDatabaseObjects()
        {
            string connStr = this.textBox_dbConnectionString.Text;
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("数据库连接串不能为空！");
            }

            _dbConnectionInfo = new DbConnectionInfo()
            {
                ConnectionString = connStr,
                DatabaseType = DatabaseType.SQLServer
            };


            var tables = DBHelper.QueryAny<T_Entity>(_dbConnectionInfo, "SELECT [name] FROM SYS.TABLES ORDER BY [name]");
            var views = DBHelper.QueryAny<T_Entity>(_dbConnectionInfo, "SELECT [name] FROM SYS.VIEWS ORDER BY [name]");

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
        private void GetDatabaseObjectColumns()
        {
            this.button_generate.Enabled = false;
            _entityStructureDict.Clear();

            List<Task> tasks = new List<Task>();
            foreach (ListViewItem item in this.listView_entities.Items)
            {
                string tableName = item.Text;
                bool isView = item.Group.Header == "视图";
                var task = Task.Run(() =>
                {
                    var columns = DBHelper.QueryAny<T_Columns>(_dbConnectionInfo, "EXEC SP_COLUMNS " + tableName);
                    var pkeys = isView ? null : DBHelper.QueryAny<T_PKeys>(_dbConnectionInfo, "EXEC SP_PKEYS " + tableName).Select(p => p.Column_Name).ToHashSet();

                    _entityStructureDict.TryAdd(tableName, new EntityStructure()
                    {
                        EntityName = tableName,
                        ColumnStructures = T_Columns_To_ColumnStructure(columns, pkeys)
                    });
                });

                tasks.Add(task);
            }

            Task.WhenAll(tasks).ContinueWith((task) =>
            {
                MainForm.MainThreadContext.Post((state) =>
                {
                    this.button_generate.Enabled = true;
                }, null);
            });
        }

        private static IList<ColumnStructure> T_Columns_To_ColumnStructure(IEnumerable<T_Columns> columns, HashSet<string> pKeys)
        {
            List<ColumnStructure> list = new List<ColumnStructure>();
            foreach (var column in columns)
            {
                list.Add(new ColumnStructure()
                {
                    ColumnName = column.Column_Name,
                    IsNullable = column.Nullable,
                    TypeName = column.Type_Name,
                    IsPrimaryKey = pKeys != null && pKeys.Contains(column.Column_Name)
                });
            }

            return list;
        }

        private void listView_entities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_entities.SelectedItems.Count > 0)
            {
                string entityName = this.listView_entities.SelectedItems[0].Text;

                this.listView_columns.BeginUpdate();
                this.listView_columns.Items.Clear();
                if (_entityStructureDict.TryGetValue(entityName, out var columns))
                {
                    foreach (var item in columns.ColumnStructures)
                    {
                        this.listView_columns.Items.Add(new ListViewItem(new string[]
                        {
                            item.ColumnName,
                            item.TypeName,
                            item.IsNullable ? "是" : "否",
                            item.IsPrimaryKey ? "是" : "否"
                        }));
                    }
                }

                this.listView_columns.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                this.listView_columns.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                this.listView_columns.EndUpdate();

                this.Preview(entityName);
            }
        }

        private void button_checkAndUncheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView_entities.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void Preview(string entityName)
        {
            if (_entityStructureDict.ContainsKey(entityName))
            {
                EntityStructure entityStructure = new EntityStructure()
                {
                    EntityName = entityName,
                    ColumnStructures = _entityStructureDict[entityName].ColumnStructures,
                };

                string entityClass = GenerateEntityClass(entityStructure);
                this.textBox_preview.Text = entityClass;
            }
        }
    }
}
