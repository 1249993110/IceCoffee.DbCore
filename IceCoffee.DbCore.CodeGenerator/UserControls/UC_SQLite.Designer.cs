namespace IceCoffee.DbCore.CodeGenerator.UserControls
{
    partial class UC_SQLite
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            ListViewGroup listViewGroup1 = new ListViewGroup("表", HorizontalAlignment.Left);
            ListViewGroup listViewGroup2 = new ListViewGroup("视图", HorizontalAlignment.Left);
            button_checkAndUncheck = new Button();
            textBox_preview = new TextBox();
            columnHeader_primaryKey = new ColumnHeader();
            columnHeader_nullable = new ColumnHeader();
            columnHeader_typeName = new ColumnHeader();
            columnHeader_columnName = new ColumnHeader();
            listView_fields = new ListView();
            columnHeader = new ColumnHeader();
            listView_entities = new ListView();
            button_generate = new Button();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            textBox_dbConnType = new TextBox();
            label_dbConnType = new Label();
            textBox_namespacePrefix = new TextBox();
            label_namespacePrefix = new Label();
            textBox_rootDir = new TextBox();
            label_rootPath = new Label();
            button_connect = new Button();
            label_dbConnectionString = new Label();
            textBox_dbConnectionString = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // button_checkAndUncheck
            // 
            button_checkAndUncheck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_checkAndUncheck.Location = new Point(737, 75);
            button_checkAndUncheck.Name = "button_checkAndUncheck";
            button_checkAndUncheck.Size = new Size(75, 23);
            button_checkAndUncheck.TabIndex = 30;
            button_checkAndUncheck.Text = "全选/反选";
            button_checkAndUncheck.UseVisualStyleBackColor = true;
            button_checkAndUncheck.Click += button_checkAndUncheck_Click;
            // 
            // textBox_preview
            // 
            textBox_preview.Dock = DockStyle.Fill;
            textBox_preview.Location = new Point(0, 0);
            textBox_preview.Multiline = true;
            textBox_preview.Name = "textBox_preview";
            textBox_preview.ScrollBars = ScrollBars.Both;
            textBox_preview.Size = new Size(395, 289);
            textBox_preview.TabIndex = 9;
            textBox_preview.WordWrap = false;
            // 
            // columnHeader_primaryKey
            // 
            columnHeader_primaryKey.Text = "主键";
            // 
            // columnHeader_nullable
            // 
            columnHeader_nullable.Text = "可空";
            // 
            // columnHeader_typeName
            // 
            columnHeader_typeName.Text = "类型名";
            // 
            // columnHeader_columnName
            // 
            columnHeader_columnName.Text = "列名";
            // 
            // listView_fields
            // 
            listView_fields.Columns.AddRange(new ColumnHeader[] { columnHeader_columnName, columnHeader_typeName, columnHeader_nullable, columnHeader_primaryKey });
            listView_fields.Dock = DockStyle.Fill;
            listView_fields.Location = new Point(0, 0);
            listView_fields.Name = "listView_fields";
            listView_fields.Size = new Size(261, 289);
            listView_fields.TabIndex = 5;
            listView_fields.UseCompatibleStateImageBehavior = false;
            listView_fields.View = View.Details;
            // 
            // columnHeader
            // 
            columnHeader.Text = "实体";
            // 
            // listView_entities
            // 
            listView_entities.CheckBoxes = true;
            listView_entities.Columns.AddRange(new ColumnHeader[] { columnHeader });
            listView_entities.Dock = DockStyle.Fill;
            listViewGroup1.Header = "表";
            listViewGroup1.Name = "listViewGroup_tables";
            listViewGroup2.Header = "视图";
            listViewGroup2.Name = "listViewGroup_views";
            listView_entities.Groups.AddRange(new ListViewGroup[] { listViewGroup1, listViewGroup2 });
            listView_entities.Location = new Point(0, 0);
            listView_entities.MultiSelect = false;
            listView_entities.Name = "listView_entities";
            listView_entities.Size = new Size(135, 289);
            listView_entities.TabIndex = 1;
            listView_entities.UseCompatibleStateImageBehavior = false;
            listView_entities.View = View.Details;
            listView_entities.SelectedIndexChanged += listView_entities_SelectedIndexChanged;
            // 
            // button_generate
            // 
            button_generate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_generate.Location = new Point(737, 110);
            button_generate.Name = "button_generate";
            button_generate.Size = new Size(75, 23);
            button_generate.TabIndex = 29;
            button_generate.Text = "生成";
            button_generate.UseVisualStyleBackColor = true;
            button_generate.Click += button_generate_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Cursor = Cursors.VSplit;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listView_entities);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(listView_fields);
            splitContainer1.Size = new Size(400, 289);
            splitContainer1.SplitterDistance = 135;
            splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.Cursor = Cursors.VSplit;
            splitContainer2.Location = new Point(13, 157);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(textBox_preview);
            splitContainer2.Size = new Size(799, 289);
            splitContainer2.SplitterDistance = 400;
            splitContainer2.TabIndex = 28;
            // 
            // textBox_dbConnType
            // 
            textBox_dbConnType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_dbConnType.Location = new Point(135, 110);
            textBox_dbConnType.Name = "textBox_dbConnType";
            textBox_dbConnType.PlaceholderText = "输入仓储基类";
            textBox_dbConnType.Size = new Size(587, 23);
            textBox_dbConnType.TabIndex = 27;
            textBox_dbConnType.Text = "DefaultDbConnectionInfo";
            // 
            // label_dbConnType
            // 
            label_dbConnType.AutoSize = true;
            label_dbConnType.Location = new Point(13, 113);
            label_dbConnType.Name = "label_dbConnType";
            label_dbConnType.Size = new Size(116, 17);
            label_dbConnType.TabIndex = 26;
            label_dbConnType.Text = "数据库连接信息类型";
            // 
            // textBox_namespacePrefix
            // 
            textBox_namespacePrefix.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_namespacePrefix.Location = new Point(135, 75);
            textBox_namespacePrefix.Name = "textBox_namespacePrefix";
            textBox_namespacePrefix.PlaceholderText = "输入命名空间前缀";
            textBox_namespacePrefix.Size = new Size(587, 23);
            textBox_namespacePrefix.TabIndex = 25;
            textBox_namespacePrefix.Text = "IceCoffee.Test.Data";
            // 
            // label_namespacePrefix
            // 
            label_namespacePrefix.AutoSize = true;
            label_namespacePrefix.Location = new Point(13, 78);
            label_namespacePrefix.Name = "label_namespacePrefix";
            label_namespacePrefix.Size = new Size(80, 17);
            label_namespacePrefix.TabIndex = 24;
            label_namespacePrefix.Text = "命名空间前缀";
            // 
            // textBox_rootDir
            // 
            textBox_rootDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_rootDir.Location = new Point(135, 43);
            textBox_rootDir.Name = "textBox_rootDir";
            textBox_rootDir.PlaceholderText = "输入根路径";
            textBox_rootDir.Size = new Size(587, 23);
            textBox_rootDir.TabIndex = 23;
            textBox_rootDir.Text = "C:\\Users\\Administrator\\Desktop\\Temp";
            // 
            // label_rootPath
            // 
            label_rootPath.AutoSize = true;
            label_rootPath.Location = new Point(13, 46);
            label_rootPath.Name = "label_rootPath";
            label_rootPath.Size = new Size(44, 17);
            label_rootPath.TabIndex = 22;
            label_rootPath.Text = "根路径";
            // 
            // button_connect
            // 
            button_connect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_connect.Location = new Point(737, 11);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(75, 23);
            button_connect.TabIndex = 21;
            button_connect.Text = "连接";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // label_dbConnectionString
            // 
            label_dbConnectionString.AutoSize = true;
            label_dbConnectionString.Location = new Point(13, 14);
            label_dbConnectionString.Name = "label_dbConnectionString";
            label_dbConnectionString.Size = new Size(80, 17);
            label_dbConnectionString.TabIndex = 20;
            label_dbConnectionString.Text = "数据库连接串";
            // 
            // textBox_dbConnectionString
            // 
            textBox_dbConnectionString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_dbConnectionString.Location = new Point(135, 11);
            textBox_dbConnectionString.Name = "textBox_dbConnectionString";
            textBox_dbConnectionString.PlaceholderText = "输入数据库连接串";
            textBox_dbConnectionString.Size = new Size(587, 23);
            textBox_dbConnectionString.TabIndex = 19;
            textBox_dbConnectionString.Text = "Data Source=./Data/database.db";
            // 
            // UC_SQLite
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button_checkAndUncheck);
            Controls.Add(button_generate);
            Controls.Add(splitContainer2);
            Controls.Add(textBox_dbConnType);
            Controls.Add(label_dbConnType);
            Controls.Add(textBox_namespacePrefix);
            Controls.Add(label_namespacePrefix);
            Controls.Add(textBox_rootDir);
            Controls.Add(label_rootPath);
            Controls.Add(button_connect);
            Controls.Add(label_dbConnectionString);
            Controls.Add(textBox_dbConnectionString);
            Name = "UC_SQLite";
            Size = new Size(827, 459);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_checkAndUncheck;
        private TextBox textBox_preview;
        private ColumnHeader columnHeader_primaryKey;
        private ColumnHeader columnHeader_nullable;
        private ColumnHeader columnHeader_typeName;
        private ColumnHeader columnHeader_columnName;
        private ListView listView_fields;
        private ColumnHeader columnHeader;
        private ListView listView_entities;
        private Button button_generate;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private TextBox textBox_dbConnType;
        private Label label_dbConnType;
        private TextBox textBox_namespacePrefix;
        private Label label_namespacePrefix;
        private TextBox textBox_rootDir;
        private Label label_rootPath;
        private Button button_connect;
        private Label label_dbConnectionString;
        private TextBox textBox_dbConnectionString;
    }
}
