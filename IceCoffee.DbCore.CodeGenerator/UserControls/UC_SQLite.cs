using IceCoffee.Common.WinForm;
using IceCoffee.DbCore.CodeGenerator.Models;
using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Utils;
using System.Data;
using System.Reflection.Emit;
using System.Text;

namespace IceCoffee.DbCore.CodeGenerator.UserControls
{
    public partial class UC_SQLite : UserControl, IView
    {
        private DbConnectionInfo _dbConnectionInfo;

        public string Label => "SQLite";

        public int Sort => 1;

        public UC_SQLite()
        {
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            try
            {
                GetEntities();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_checkAndUncheck_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView_entities.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        private void button_generate_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.InitDirectory(this.textBox_rootDir.Text);

                GenerateCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetEntities()
        {
            string connStr = this.textBox_dbConnectionString.Text;
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception("数据库连接串不能为空!");
            }

            _dbConnectionInfo = new DbConnectionInfo()
            {
                ConnectionString = connStr,
                DatabaseType = DatabaseType.SQLite
            };

            var tables = DBHelper.QueryAny<EntityInfo>(_dbConnectionInfo, "SELECT name AS Name FROM sqlite_master WHERE type='table' ORDER BY name");
            var views = DBHelper.QueryAny<EntityInfo>(_dbConnectionInfo, "SELECT name AS Name FROM sqlite_master WHERE type='view' ORDER BY name");

            this.listView_entities.BeginUpdate();
            this.listView_entities.Items.Clear();
            foreach (var item in tables)
            {
                if(item.Name == "sqlite_sequence")
                {
                    continue;
                }
                this.listView_entities.Items.Add(new ListViewItem(item.Name, this.listView_entities.Groups[0]) { Checked = false });
            }
            foreach (var item in views)
            {
                this.listView_entities.Items.Add(new ListViewItem(item.Name, this.listView_entities.Groups[1]) { Checked = false });
            }

            this.listView_entities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.listView_entities.EndUpdate();

        }

        class TableColumns
        {
            [Column("name")]
            public string Name { get; set; }

            [Column("type")]
            public string Type { get; set; }

            [Column("notnull")]
            public bool NotNull { get; set; }

            [Column("pk")]
            public bool PK { get; set; }
        }

        private IEnumerable<FieldInfo> GetFieldsInfo(string tableName)
        {
            var result = new List<FieldInfo>();
            var fields = DBHelper.QueryAny<TableColumns>(_dbConnectionInfo, $"PRAGMA table_info('{tableName}')");

            foreach (var field in fields)
            {
                string columnName = field.Name;

                var f = new FieldInfo()
                {
                    ColumnName = columnName,
                    IsNullable = field.NotNull == false,
                    TypeName = field.Type,
                    IsPrimaryKey = field.PK
                };

                result.Add(f);
            }

            return result;
        }

        private void GenerateCode()
        {
            var generator = new DefaultGenerator(this.textBox_namespacePrefix.Text, Utils.GetBasicRepositoryName(_dbConnectionInfo.DatabaseType), this.textBox_dbConnType.Text);

            foreach (ListViewItem item in this.listView_entities.CheckedItems)
            {
                string entityName = item.Text;
                bool isView = item.Group.Header == "视图";
                var es = new EntityInfo()
                {
                    Name = entityName,
                    FieldInfos = GetFieldsInfo(entityName),
                    IsView = isView
                };

                string entityClass = generator.GenerateEntityClass(es);
                string path = Path.Combine(this.textBox_rootDir.Text, $"Entities/{generator.GetClassName(es)}.cs");
                File.WriteAllText(path, entityClass, Encoding.UTF8);

                string iRepository = generator.GenerateIRepository(es);
                path = Path.Combine(this.textBox_rootDir.Text, $"IRepositories/I{generator.GetRepositoryName(es)}.cs");
                File.WriteAllText(path, iRepository, Encoding.UTF8);

                string repository = generator.GenerateRepository(es);
                path = Path.Combine(this.textBox_rootDir.Text, $"Repositories/{generator.GetRepositoryName(es)}.cs");
                File.WriteAllText(path, repository, Encoding.UTF8);
            }

            MessageBox.Show("生成成功");
        }

        private void listView_entities_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listView_entities.SelectedItems.Count > 0)
                {
                    var selectedItem = this.listView_entities.SelectedItems[0];
                    string entityName = selectedItem.Text;
                    var fieldInfos = GetFieldsInfo(entityName);

                    this.listView_fields.BeginUpdate();
                    this.listView_fields.Items.Clear();
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

                    var entityInfo = new EntityInfo()
                    {
                        Name = entityName,
                        FieldInfos = fieldInfos,
                        IsView = selectedItem.Group.Header == "视图"
                    };

                    this.Preview(entityInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Preview(EntityInfo entityInfo)
        {
            var generator = new DefaultGenerator(this.textBox_namespacePrefix.Text, Utils.GetBasicRepositoryName(_dbConnectionInfo.DatabaseType), this.textBox_dbConnType.Text);

            this.textBox_preview.Text = generator.GenerateEntityClass(entityInfo) + Environment.NewLine
                + generator.GenerateIRepository(entityInfo) + Environment.NewLine
                + generator.GenerateRepository(entityInfo) + Environment.NewLine;
        }
    }
}