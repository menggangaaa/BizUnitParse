namespace BizUnitParse
{
    partial class MainPanel
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
            this.txtDirPath = new System.Windows.Forms.TextBox();
            this.btnDirPathSelect = new System.Windows.Forms.Button();
            this.isIDE = new System.Windows.Forms.RadioButton();
            this.isClient = new System.Windows.Forms.RadioButton();
            this.initJarBar = new System.Windows.Forms.ProgressBar();
            this.btnInitJar = new System.Windows.Forms.Button();
            this.labInitJar = new System.Windows.Forms.Label();
            this.labFilter = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.txtEntityFilter = new System.Windows.Forms.TextBox();
            this.bizUnitTree = new System.Windows.Forms.TreeView();
            this.entityTable = new System.Windows.Forms.DataGridView();
            this.column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.entityTable)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDirPath
            // 
            this.txtDirPath.Location = new System.Drawing.Point(5, 5);
            this.txtDirPath.Name = "txtDirPath";
            this.txtDirPath.Size = new System.Drawing.Size(350, 29);
            this.txtDirPath.TabIndex = 0;
            this.txtDirPath.Text = "D:\\workspaces\\Project_1";
            this.txtDirPath.TextChanged += new System.EventHandler(this.txtDirPath_TextChanged);
            // 
            // btnDirPathSelect
            // 
            this.btnDirPathSelect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDirPathSelect.Location = new System.Drawing.Point(360, 5);
            this.btnDirPathSelect.Name = "btnDirPathSelect";
            this.btnDirPathSelect.Size = new System.Drawing.Size(70, 30);
            this.btnDirPathSelect.TabIndex = 1;
            this.btnDirPathSelect.Text = "选择...";
            this.btnDirPathSelect.UseVisualStyleBackColor = true;
            this.btnDirPathSelect.Click += new System.EventHandler(this.btnDirPathSelect_Click);
            // 
            // isIDE
            // 
            this.isIDE.AutoSize = true;
            this.isIDE.Checked = true;
            this.isIDE.Location = new System.Drawing.Point(435, 9);
            this.isIDE.Name = "isIDE";
            this.isIDE.Size = new System.Drawing.Size(103, 23);
            this.isIDE.TabIndex = 2;
            this.isIDE.TabStop = true;
            this.isIDE.Text = "开发环境";
            this.isIDE.UseVisualStyleBackColor = true;
            this.isIDE.CheckedChanged += new System.EventHandler(this.isIDE_CheckedChanged);
            // 
            // isClient
            // 
            this.isClient.AutoSize = true;
            this.isClient.Location = new System.Drawing.Point(540, 9);
            this.isClient.Name = "isClient";
            this.isClient.Size = new System.Drawing.Size(84, 23);
            this.isClient.TabIndex = 3;
            this.isClient.Text = "客户端";
            this.isClient.UseVisualStyleBackColor = true;
            this.isClient.CheckedChanged += new System.EventHandler(this.isClient_CheckedChanged);
            // 
            // initJarBar
            // 
            this.initJarBar.Location = new System.Drawing.Point(630, 5);
            this.initJarBar.Name = "initJarBar";
            this.initJarBar.Size = new System.Drawing.Size(150, 29);
            this.initJarBar.TabIndex = 4;
            // 
            // btnInitJar
            // 
            this.btnInitJar.Location = new System.Drawing.Point(780, 4);
            this.btnInitJar.Name = "btnInitJar";
            this.btnInitJar.Size = new System.Drawing.Size(150, 31);
            this.btnInitJar.TabIndex = 5;
            this.btnInitJar.Text = "初始化Jar包";
            this.btnInitJar.UseVisualStyleBackColor = true;
            this.btnInitJar.Click += new System.EventHandler(this.btnInitJar_Click);
            // 
            // labInitJar
            // 
            this.labInitJar.AutoSize = true;
            this.labInitJar.ForeColor = System.Drawing.Color.Red;
            this.labInitJar.Location = new System.Drawing.Point(638, 10);
            this.labInitJar.Name = "labInitJar";
            this.labInitJar.Size = new System.Drawing.Size(134, 19);
            this.labInitJar.TabIndex = 6;
            this.labInitJar.Text = "未初始化Jar包";
            // 
            // labFilter
            // 
            this.labFilter.AutoSize = true;
            this.labFilter.Location = new System.Drawing.Point(940, 10);
            this.labFilter.Name = "labFilter";
            this.labFilter.Size = new System.Drawing.Size(66, 19);
            this.labFilter.TabIndex = 7;
            this.labFilter.Text = "筛选：";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(1010, 5);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(161, 29);
            this.txtFilter.TabIndex = 8;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(1175, 4);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(50, 31);
            this.btnLeft.TabIndex = 9;
            this.btnLeft.Text = "<";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(1230, 4);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(50, 31);
            this.btnRight.TabIndex = 10;
            this.btnRight.Text = ">";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // txtEntityFilter
            // 
            this.txtEntityFilter.Location = new System.Drawing.Point(5, 40);
            this.txtEntityFilter.Name = "txtEntityFilter";
            this.txtEntityFilter.Size = new System.Drawing.Size(350, 29);
            this.txtEntityFilter.TabIndex = 11;
            this.txtEntityFilter.TextChanged += new System.EventHandler(this.txtEntityFilter_TextChanged);
            this.txtEntityFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEntityFilter_KeyDown);
            // 
            // bizUnitTree
            // 
            this.bizUnitTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.bizUnitTree.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bizUnitTree.Location = new System.Drawing.Point(5, 75);
            this.bizUnitTree.Name = "bizUnitTree";
            this.bizUnitTree.Size = new System.Drawing.Size(350, 682);
            this.bizUnitTree.TabIndex = 12;
            this.bizUnitTree.DoubleClick += new System.EventHandler(this.bizUnitTree_DoubleClick);
            // 
            // entityTable
            // 
            this.entityTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityTable.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.entityTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.entityTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column,
            this.name,
            this.tableName,
            this.relTable,
            this.dataType,
            this.relPath});
            this.entityTable.Location = new System.Drawing.Point(360, 40);
            this.entityTable.Name = "entityTable";
            this.entityTable.ReadOnly = true;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.entityTable.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.entityTable.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.entityTable.RowTemplate.Height = 23;
            this.entityTable.RowTemplate.ReadOnly = true;
            this.entityTable.Size = new System.Drawing.Size(920, 717);
            this.entityTable.TabIndex = 13;
            this.entityTable.DoubleClick += new System.EventHandler(this.entityTable_DoubleClick);
            this.entityTable.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.entityTable_KeyPress);
            this.entityTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.entityTable_MouseDown);
            // 
            // column
            // 
            this.column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.column.HeaderText = "字段";
            this.column.Name = "column";
            this.column.ReadOnly = true;
            this.column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.name.HeaderText = "名称";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tableName
            // 
            this.tableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tableName.HeaderText = "数据库名称";
            this.tableName.Name = "tableName";
            this.tableName.ReadOnly = true;
            this.tableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tableName.Width = 150;
            // 
            // relTable
            // 
            this.relTable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.relTable.HeaderText = "关系表";
            this.relTable.Name = "relTable";
            this.relTable.ReadOnly = true;
            this.relTable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataType
            // 
            this.dataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataType.HeaderText = "数据类型";
            this.dataType.Name = "dataType";
            this.dataType.ReadOnly = true;
            this.dataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataType.Width = 120;
            // 
            // relPath
            // 
            this.relPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.relPath.HeaderText = "关联路径";
            this.relPath.Name = "relPath";
            this.relPath.ReadOnly = true;
            this.relPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.relPath.Visible = false;
            this.relPath.Width = 120;
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 761);
            this.Controls.Add(this.entityTable);
            this.Controls.Add(this.bizUnitTree);
            this.Controls.Add(this.txtEntityFilter);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.labFilter);
            this.Controls.Add(this.labInitJar);
            this.Controls.Add(this.btnInitJar);
            this.Controls.Add(this.initJarBar);
            this.Controls.Add(this.isClient);
            this.Controls.Add(this.isIDE);
            this.Controls.Add(this.btnDirPathSelect);
            this.Controls.Add(this.txtDirPath);
            this.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "业务单元解析";
            this.Activated += new System.EventHandler(this.MainPanel_Activated);
            this.Load += new System.EventHandler(this.MainPanel_Load);
            this.SizeChanged += new System.EventHandler(this.MainPanel_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.entityTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDirPath;
        private System.Windows.Forms.Button btnDirPathSelect;
        private System.Windows.Forms.RadioButton isIDE;
        private System.Windows.Forms.RadioButton isClient;
        private System.Windows.Forms.ProgressBar initJarBar;
        private System.Windows.Forms.Button btnInitJar;
        private System.Windows.Forms.Label labInitJar;
        private System.Windows.Forms.Label labFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.TextBox txtEntityFilter;
        private System.Windows.Forms.TreeView bizUnitTree;
        private System.Windows.Forms.DataGridView entityTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn column;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn relTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn relPath;
    }
}

