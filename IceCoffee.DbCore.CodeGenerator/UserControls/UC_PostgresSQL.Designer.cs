namespace IceCoffee.DbCore.CodeGenerator.UserControls
{
    partial class UC_PostgresSQL
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("表", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("视图", System.Windows.Forms.HorizontalAlignment.Left);
            this.label_dbConnectionString = new System.Windows.Forms.Label();
            this.textBox_dbConnectionString = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label_rootPath = new System.Windows.Forms.Label();
            this.textBox_rootPath = new System.Windows.Forms.TextBox();
            this.label_namespacePrefix = new System.Windows.Forms.Label();
            this.textBox_namespacePrefix = new System.Windows.Forms.TextBox();
            this.label_dbConnType = new System.Windows.Forms.Label();
            this.textBox_dbConnType = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView_entities = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.listView_fields = new System.Windows.Forms.ListView();
            this.columnHeader_columnName = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_typeName = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_nullable = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_primaryKey = new System.Windows.Forms.ColumnHeader();
            this.textBox_preview = new System.Windows.Forms.TextBox();
            this.button_generate = new System.Windows.Forms.Button();
            this.button_checkAndUncheck = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_dbConnectionString
            // 
            this.label_dbConnectionString.AutoSize = true;
            this.label_dbConnectionString.Location = new System.Drawing.Point(16, 16);
            this.label_dbConnectionString.Name = "label_dbConnectionString";
            this.label_dbConnectionString.Size = new System.Drawing.Size(80, 17);
            this.label_dbConnectionString.TabIndex = 5;
            this.label_dbConnectionString.Text = "数据库连接串";
            // 
            // textBox_dbConnectionString
            // 
            this.textBox_dbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dbConnectionString.Location = new System.Drawing.Point(138, 13);
            this.textBox_dbConnectionString.Name = "textBox_dbConnectionString";
            this.textBox_dbConnectionString.PlaceholderText = "输入数据库连接串";
            this.textBox_dbConnectionString.Size = new System.Drawing.Size(749, 23);
            this.textBox_dbConnectionString.TabIndex = 4;
            this.textBox_dbConnectionString.Text = "Host=;Username=postgres;Password=;Database=postgres";
            // 
            // button_connect
            // 
            this.button_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_connect.Location = new System.Drawing.Point(904, 13);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 6;
            this.button_connect.Text = "连接";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label_rootPath
            // 
            this.label_rootPath.AutoSize = true;
            this.label_rootPath.Location = new System.Drawing.Point(16, 48);
            this.label_rootPath.Name = "label_rootPath";
            this.label_rootPath.Size = new System.Drawing.Size(44, 17);
            this.label_rootPath.TabIndex = 7;
            this.label_rootPath.Text = "根路径";
            // 
            // textBox_rootPath
            // 
            this.textBox_rootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rootPath.Location = new System.Drawing.Point(138, 45);
            this.textBox_rootPath.Name = "textBox_rootPath";
            this.textBox_rootPath.PlaceholderText = "输入根路径";
            this.textBox_rootPath.Size = new System.Drawing.Size(749, 23);
            this.textBox_rootPath.TabIndex = 8;
            this.textBox_rootPath.Text = "C:\\Users\\Administrator\\Desktop\\Temp";
            // 
            // label_namespacePrefix
            // 
            this.label_namespacePrefix.AutoSize = true;
            this.label_namespacePrefix.Location = new System.Drawing.Point(16, 80);
            this.label_namespacePrefix.Name = "label_namespacePrefix";
            this.label_namespacePrefix.Size = new System.Drawing.Size(80, 17);
            this.label_namespacePrefix.TabIndex = 9;
            this.label_namespacePrefix.Text = "命名空间前缀";
            // 
            // textBox_namespacePrefix
            // 
            this.textBox_namespacePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_namespacePrefix.Location = new System.Drawing.Point(138, 77);
            this.textBox_namespacePrefix.Name = "textBox_namespacePrefix";
            this.textBox_namespacePrefix.PlaceholderText = "输入命名空间前缀";
            this.textBox_namespacePrefix.Size = new System.Drawing.Size(749, 23);
            this.textBox_namespacePrefix.TabIndex = 10;
            this.textBox_namespacePrefix.Text = "IceCoffee.Test.Data";
            // 
            // label_dbConnType
            // 
            this.label_dbConnType.AutoSize = true;
            this.label_dbConnType.Location = new System.Drawing.Point(16, 115);
            this.label_dbConnType.Name = "label_dbConnType";
            this.label_dbConnType.Size = new System.Drawing.Size(116, 17);
            this.label_dbConnType.TabIndex = 14;
            this.label_dbConnType.Text = "数据库连接信息类型";
            // 
            // textBox_dbConnType
            // 
            this.textBox_dbConnType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dbConnType.Location = new System.Drawing.Point(138, 112);
            this.textBox_dbConnType.Name = "textBox_dbConnType";
            this.textBox_dbConnType.PlaceholderText = "输入仓储基类";
            this.textBox_dbConnType.Size = new System.Drawing.Size(749, 23);
            this.textBox_dbConnType.TabIndex = 15;
            this.textBox_dbConnType.Text = "DefaultDbConnectionInfo";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer2.Location = new System.Drawing.Point(16, 159);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBox_preview);
            this.splitContainer2.Size = new System.Drawing.Size(963, 364);
            this.splitContainer2.SplitterDistance = 483;
            this.splitContainer2.TabIndex = 16;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView_entities);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView_fields);
            this.splitContainer1.Size = new System.Drawing.Size(483, 364);
            this.splitContainer1.SplitterDistance = 164;
            this.splitContainer1.TabIndex = 6;
            // 
            // listView_entities
            // 
            this.listView_entities.CheckBoxes = true;
            this.listView_entities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.listView_entities.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "表";
            listViewGroup1.Name = "listViewGroup_tables";
            listViewGroup2.Header = "视图";
            listViewGroup2.Name = "listViewGroup_views";
            this.listView_entities.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView_entities.Location = new System.Drawing.Point(0, 0);
            this.listView_entities.MultiSelect = false;
            this.listView_entities.Name = "listView_entities";
            this.listView_entities.Size = new System.Drawing.Size(164, 364);
            this.listView_entities.TabIndex = 1;
            this.listView_entities.UseCompatibleStateImageBehavior = false;
            this.listView_entities.View = System.Windows.Forms.View.Details;
            this.listView_entities.SelectedIndexChanged += new System.EventHandler(this.listView_entities_SelectedIndexChanged);
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "实体";
            // 
            // listView_fields
            // 
            this.listView_fields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_columnName,
            this.columnHeader_typeName,
            this.columnHeader_nullable,
            this.columnHeader_primaryKey});
            this.listView_fields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_fields.Location = new System.Drawing.Point(0, 0);
            this.listView_fields.Name = "listView_fields";
            this.listView_fields.Size = new System.Drawing.Size(315, 364);
            this.listView_fields.TabIndex = 5;
            this.listView_fields.UseCompatibleStateImageBehavior = false;
            this.listView_fields.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_columnName
            // 
            this.columnHeader_columnName.Text = "列名";
            // 
            // columnHeader_typeName
            // 
            this.columnHeader_typeName.Text = "类型名";
            // 
            // columnHeader_nullable
            // 
            this.columnHeader_nullable.Text = "可空";
            // 
            // columnHeader_primaryKey
            // 
            this.columnHeader_primaryKey.Text = "主键";
            // 
            // textBox_preview
            // 
            this.textBox_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_preview.Location = new System.Drawing.Point(0, 0);
            this.textBox_preview.Multiline = true;
            this.textBox_preview.Name = "textBox_preview";
            this.textBox_preview.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_preview.Size = new System.Drawing.Size(476, 364);
            this.textBox_preview.TabIndex = 9;
            this.textBox_preview.WordWrap = false;
            // 
            // button_generate
            // 
            this.button_generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_generate.Location = new System.Drawing.Point(904, 112);
            this.button_generate.Name = "button_generate";
            this.button_generate.Size = new System.Drawing.Size(75, 23);
            this.button_generate.TabIndex = 17;
            this.button_generate.Text = "生成";
            this.button_generate.UseVisualStyleBackColor = true;
            this.button_generate.Click += new System.EventHandler(this.button_generate_Click);
            // 
            // button_checkAndUncheck
            // 
            this.button_checkAndUncheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_checkAndUncheck.Location = new System.Drawing.Point(904, 77);
            this.button_checkAndUncheck.Name = "button_checkAndUncheck";
            this.button_checkAndUncheck.Size = new System.Drawing.Size(75, 23);
            this.button_checkAndUncheck.TabIndex = 18;
            this.button_checkAndUncheck.Text = "全选/反选";
            this.button_checkAndUncheck.UseVisualStyleBackColor = true;
            this.button_checkAndUncheck.Click += new System.EventHandler(this.button_checkAndUncheck_Click);
            // 
            // UC_PostgresSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_checkAndUncheck);
            this.Controls.Add(this.button_generate);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.textBox_dbConnType);
            this.Controls.Add(this.label_dbConnType);
            this.Controls.Add(this.textBox_namespacePrefix);
            this.Controls.Add(this.label_namespacePrefix);
            this.Controls.Add(this.textBox_rootPath);
            this.Controls.Add(this.label_rootPath);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.label_dbConnectionString);
            this.Controls.Add(this.textBox_dbConnectionString);
            this.Name = "UC_PostgresSQL";
            this.Size = new System.Drawing.Size(1002, 537);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label_dbConnectionString;
        private TextBox textBox_dbConnectionString;
        private Button button_connect;
        private Label label_rootPath;
        private TextBox textBox_rootPath;
        private Label label_namespacePrefix;
        private TextBox textBox_namespacePrefix;
        private Label label_dbConnType;
        private TextBox textBox_dbConnType;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer1;
        private ListView listView_entities;
        private ColumnHeader columnHeader;
        private ListView listView_fields;
        private ColumnHeader columnHeader_columnName;
        private ColumnHeader columnHeader_typeName;
        private ColumnHeader columnHeader_nullable;
        private ColumnHeader columnHeader_primaryKey;
        private TextBox textBox_preview;
        private Button button_generate;
        private Button button_checkAndUncheck;
    }
}
