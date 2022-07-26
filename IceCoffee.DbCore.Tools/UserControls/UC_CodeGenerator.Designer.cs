﻿
namespace IceCoffee.DbCore.Tools.UserControls
{
    partial class UC_CodeGenerator
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
            this.textBox_dbConnectionString = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label_dbConnectionString = new System.Windows.Forms.Label();
            this.textBox_rootPath = new System.Windows.Forms.TextBox();
            this.label_rootPath = new System.Windows.Forms.Label();
            this.textBox_namespacePrefix = new System.Windows.Forms.TextBox();
            this.label_namespacePrefix = new System.Windows.Forms.Label();
            this.button_generate = new System.Windows.Forms.Button();
            this.button_checkAndUncheck = new System.Windows.Forms.Button();
            this.textBox_basicRepository = new System.Windows.Forms.TextBox();
            this.label_basicRepository = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView_entities = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.listView_columns = new System.Windows.Forms.ListView();
            this.columnHeader_columnName = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_typeName = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_nullable = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_primaryKey = new System.Windows.Forms.ColumnHeader();
            this.textBox_preview = new System.Windows.Forms.TextBox();
            this.label_dbConnType = new System.Windows.Forms.Label();
            this.textBox_dbConnType = new System.Windows.Forms.TextBox();
            this.textBox_subPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            // textBox_dbConnectionString
            // 
            this.textBox_dbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dbConnectionString.Location = new System.Drawing.Point(151, 18);
            this.textBox_dbConnectionString.Name = "textBox_dbConnectionString";
            this.textBox_dbConnectionString.PlaceholderText = "输入数据库连接串";
            this.textBox_dbConnectionString.Size = new System.Drawing.Size(683, 23);
            this.textBox_dbConnectionString.TabIndex = 1;
            this.textBox_dbConnectionString.Text = "Data Source=.;Initial Catalog=Test;uid=sa;pwd=;TrustServerCertificate=True;";
            // 
            // button_connect
            // 
            this.button_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_connect.Location = new System.Drawing.Point(852, 18);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 2;
            this.button_connect.Text = "连接";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label_dbConnectionString
            // 
            this.label_dbConnectionString.AutoSize = true;
            this.label_dbConnectionString.Location = new System.Drawing.Point(29, 21);
            this.label_dbConnectionString.Name = "label_dbConnectionString";
            this.label_dbConnectionString.Size = new System.Drawing.Size(80, 17);
            this.label_dbConnectionString.TabIndex = 3;
            this.label_dbConnectionString.Text = "数据库连接串";
            // 
            // textBox_rootPath
            // 
            this.textBox_rootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rootPath.Location = new System.Drawing.Point(151, 47);
            this.textBox_rootPath.Name = "textBox_rootPath";
            this.textBox_rootPath.PlaceholderText = "输入根路径";
            this.textBox_rootPath.Size = new System.Drawing.Size(683, 23);
            this.textBox_rootPath.TabIndex = 1;
            this.textBox_rootPath.Text = "C:\\Users\\Administrator\\Desktop\\Temp";
            // 
            // label_rootPath
            // 
            this.label_rootPath.AutoSize = true;
            this.label_rootPath.Location = new System.Drawing.Point(29, 50);
            this.label_rootPath.Name = "label_rootPath";
            this.label_rootPath.Size = new System.Drawing.Size(44, 17);
            this.label_rootPath.TabIndex = 3;
            this.label_rootPath.Text = "根路径";
            // 
            // textBox_namespacePrefix
            // 
            this.textBox_namespacePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_namespacePrefix.Location = new System.Drawing.Point(151, 77);
            this.textBox_namespacePrefix.Name = "textBox_namespacePrefix";
            this.textBox_namespacePrefix.PlaceholderText = "输入命名空间前缀";
            this.textBox_namespacePrefix.Size = new System.Drawing.Size(683, 23);
            this.textBox_namespacePrefix.TabIndex = 1;
            this.textBox_namespacePrefix.Text = "HYCX.Test.Data";
            // 
            // label_namespacePrefix
            // 
            this.label_namespacePrefix.AutoSize = true;
            this.label_namespacePrefix.Location = new System.Drawing.Point(29, 80);
            this.label_namespacePrefix.Name = "label_namespacePrefix";
            this.label_namespacePrefix.Size = new System.Drawing.Size(80, 17);
            this.label_namespacePrefix.TabIndex = 3;
            this.label_namespacePrefix.Text = "命名空间前缀";
            // 
            // button_generate
            // 
            this.button_generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_generate.Location = new System.Drawing.Point(852, 106);
            this.button_generate.Name = "button_generate";
            this.button_generate.Size = new System.Drawing.Size(75, 23);
            this.button_generate.TabIndex = 2;
            this.button_generate.Text = "生成";
            this.button_generate.UseVisualStyleBackColor = true;
            this.button_generate.Click += new System.EventHandler(this.button_generate_Click);
            // 
            // button_checkAndUncheck
            // 
            this.button_checkAndUncheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_checkAndUncheck.Location = new System.Drawing.Point(852, 142);
            this.button_checkAndUncheck.Name = "button_checkAndUncheck";
            this.button_checkAndUncheck.Size = new System.Drawing.Size(75, 23);
            this.button_checkAndUncheck.TabIndex = 6;
            this.button_checkAndUncheck.Text = "全选/反选";
            this.button_checkAndUncheck.UseVisualStyleBackColor = true;
            this.button_checkAndUncheck.Click += new System.EventHandler(this.button_checkAndUncheck_Click);
            // 
            // textBox_basicRepository
            // 
            this.textBox_basicRepository.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_basicRepository.Location = new System.Drawing.Point(151, 106);
            this.textBox_basicRepository.Name = "textBox_basicRepository";
            this.textBox_basicRepository.PlaceholderText = "输入仓储基类";
            this.textBox_basicRepository.Size = new System.Drawing.Size(683, 23);
            this.textBox_basicRepository.TabIndex = 1;
            this.textBox_basicRepository.Text = "IceCoffee.DbCore.Repositories.SqlServerRepository<TEntity>";
            // 
            // label_basicRepository
            // 
            this.label_basicRepository.AutoSize = true;
            this.label_basicRepository.Location = new System.Drawing.Point(29, 109);
            this.label_basicRepository.Name = "label_basicRepository";
            this.label_basicRepository.Size = new System.Drawing.Size(56, 17);
            this.label_basicRepository.TabIndex = 3;
            this.label_basicRepository.Text = "仓储基类";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer2.Location = new System.Drawing.Point(29, 212);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBox_preview);
            this.splitContainer2.Size = new System.Drawing.Size(805, 409);
            this.splitContainer2.SplitterDistance = 499;
            this.splitContainer2.TabIndex = 9;
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
            this.splitContainer1.Panel2.Controls.Add(this.listView_columns);
            this.splitContainer1.Size = new System.Drawing.Size(499, 409);
            this.splitContainer1.SplitterDistance = 200;
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
            this.listView_entities.HideSelection = false;
            this.listView_entities.Location = new System.Drawing.Point(0, 0);
            this.listView_entities.MultiSelect = false;
            this.listView_entities.Name = "listView_entities";
            this.listView_entities.Size = new System.Drawing.Size(200, 409);
            this.listView_entities.TabIndex = 1;
            this.listView_entities.UseCompatibleStateImageBehavior = false;
            this.listView_entities.View = System.Windows.Forms.View.Details;
            this.listView_entities.SelectedIndexChanged += new System.EventHandler(this.listView_entities_SelectedIndexChanged);
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "实体";
            // 
            // listView_columns
            // 
            this.listView_columns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_columnName,
            this.columnHeader_typeName,
            this.columnHeader_nullable,
            this.columnHeader_primaryKey});
            this.listView_columns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_columns.HideSelection = false;
            this.listView_columns.Location = new System.Drawing.Point(0, 0);
            this.listView_columns.Name = "listView_columns";
            this.listView_columns.Size = new System.Drawing.Size(295, 409);
            this.listView_columns.TabIndex = 5;
            this.listView_columns.UseCompatibleStateImageBehavior = false;
            this.listView_columns.View = System.Windows.Forms.View.Details;
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
            this.textBox_preview.Size = new System.Drawing.Size(302, 409);
            this.textBox_preview.TabIndex = 9;
            this.textBox_preview.WordWrap = false;
            // 
            // label_dbConnType
            // 
            this.label_dbConnType.AutoSize = true;
            this.label_dbConnType.Location = new System.Drawing.Point(29, 138);
            this.label_dbConnType.Name = "label_dbConnType";
            this.label_dbConnType.Size = new System.Drawing.Size(116, 17);
            this.label_dbConnType.TabIndex = 13;
            this.label_dbConnType.Text = "数据库连接信息类型";
            // 
            // textBox_dbConnType
            // 
            this.textBox_dbConnType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dbConnType.Location = new System.Drawing.Point(151, 135);
            this.textBox_dbConnType.Name = "textBox_dbConnType";
            this.textBox_dbConnType.PlaceholderText = "输入仓储基类";
            this.textBox_dbConnType.Size = new System.Drawing.Size(683, 23);
            this.textBox_dbConnType.TabIndex = 12;
            this.textBox_dbConnType.Text = "HYCX.Test.Data.Primitives.DefaultDbConnectionInfo";
            // 
            // textBox_subPath
            // 
            this.textBox_subPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_subPath.Location = new System.Drawing.Point(151, 164);
            this.textBox_subPath.Name = "textBox_subPath";
            this.textBox_subPath.PlaceholderText = "输入命名空间前缀";
            this.textBox_subPath.Size = new System.Drawing.Size(683, 23);
            this.textBox_subPath.TabIndex = 1;
            this.textBox_subPath.Text = "subPath";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "子目录";
            // 
            // UC_CodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_dbConnType);
            this.Controls.Add(this.textBox_dbConnType);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.button_checkAndUncheck);
            this.Controls.Add(this.label_basicRepository);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_namespacePrefix);
            this.Controls.Add(this.label_rootPath);
            this.Controls.Add(this.label_dbConnectionString);
            this.Controls.Add(this.textBox_basicRepository);
            this.Controls.Add(this.textBox_subPath);
            this.Controls.Add(this.textBox_namespacePrefix);
            this.Controls.Add(this.button_generate);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_rootPath);
            this.Controls.Add(this.textBox_dbConnectionString);
            this.Name = "UC_CodeGenerator";
            this.Size = new System.Drawing.Size(948, 650);
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
        private System.Windows.Forms.TextBox textBox_dbConnectionString;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label_dbConnectionString;
        private System.Windows.Forms.TextBox textBox_rootPath;
        private System.Windows.Forms.Label label_rootPath;
        private System.Windows.Forms.TextBox textBox_namespacePrefix;
        private System.Windows.Forms.Label label_namespacePrefix;
        private System.Windows.Forms.Button button_generate;
        private System.Windows.Forms.Button button_checkAndUncheck;
        private System.Windows.Forms.TextBox textBox_basicRepository;
        private System.Windows.Forms.Label label_basicRepository;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView_entities;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ListView listView_columns;
        private System.Windows.Forms.ColumnHeader columnHeader_columnName;
        private System.Windows.Forms.ColumnHeader columnHeader_typeName;
        private System.Windows.Forms.ColumnHeader columnHeader_nullable;
        private System.Windows.Forms.ColumnHeader columnHeader_primaryKey;
        private System.Windows.Forms.TextBox textBox_preview;
        private System.Windows.Forms.Label label_dbConnType;
        private System.Windows.Forms.TextBox textBox_dbConnType;
        private System.Windows.Forms.TextBox textBox_subPath;
        private System.Windows.Forms.Label label1;
    }
}