namespace FormatToolsProject
{
    partial class FormatToolsForm
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
            this.TextBox_Config = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.导入配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DataGridView_Data = new System.Windows.Forms.DataGridView();
            this.生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.初始化表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Data)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBox_Config
            // 
            this.TextBox_Config.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Config.Location = new System.Drawing.Point(3, 36);
            this.TextBox_Config.Multiline = true;
            this.TextBox_Config.Name = "TextBox_Config";
            this.TextBox_Config.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_Config.Size = new System.Drawing.Size(914, 404);
            this.TextBox_Config.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入配置ToolStripMenuItem,
            this.导出配置ToolStripMenuItem,
            this.初始化表ToolStripMenuItem,
            this.生成ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(918, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 导入配置ToolStripMenuItem
            // 
            this.导入配置ToolStripMenuItem.Name = "导入配置ToolStripMenuItem";
            this.导入配置ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.导入配置ToolStripMenuItem.Text = "导入配置";
            this.导入配置ToolStripMenuItem.Click += new System.EventHandler(this.导入配置ToolStripMenuItem_Click);
            // 
            // 导出配置ToolStripMenuItem
            // 
            this.导出配置ToolStripMenuItem.Name = "导出配置ToolStripMenuItem";
            this.导出配置ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.导出配置ToolStripMenuItem.Text = "导出配置";
            this.导出配置ToolStripMenuItem.Click += new System.EventHandler(this.导出配置ToolStripMenuItem_Click);
            // 
            // DataGridView_Data
            // 
            this.DataGridView_Data.AllowUserToDeleteRows = false;
            this.DataGridView_Data.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_Data.Location = new System.Drawing.Point(3, 446);
            this.DataGridView_Data.Name = "DataGridView_Data";
            this.DataGridView_Data.RowTemplate.Height = 27;
            this.DataGridView_Data.Size = new System.Drawing.Size(914, 253);
            this.DataGridView_Data.TabIndex = 2;
            // 
            // 生成ToolStripMenuItem
            // 
            this.生成ToolStripMenuItem.Name = "生成ToolStripMenuItem";
            this.生成ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.生成ToolStripMenuItem.Text = "生成";
            this.生成ToolStripMenuItem.Click += new System.EventHandler(this.生成ToolStripMenuItem_Click);
            // 
            // 初始化表ToolStripMenuItem
            // 
            this.初始化表ToolStripMenuItem.Name = "初始化表ToolStripMenuItem";
            this.初始化表ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.初始化表ToolStripMenuItem.Text = "初始化表";
            this.初始化表ToolStripMenuItem.Click += new System.EventHandler(this.初始化表ToolStripMenuItem_Click);
            // 
            // FormatToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 703);
            this.Controls.Add(this.DataGridView_Data);
            this.Controls.Add(this.TextBox_Config);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormatToolsForm";
            this.Text = "格式化工具";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Data)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox_Config;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导入配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出配置ToolStripMenuItem;
        private System.Windows.Forms.DataGridView DataGridView_Data;
        private System.Windows.Forms.ToolStripMenuItem 生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始化表ToolStripMenuItem;
    }
}

