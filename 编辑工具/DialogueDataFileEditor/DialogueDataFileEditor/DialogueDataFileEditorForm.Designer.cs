namespace DialogueDataFileEditor
{
    partial class DialogueDataFileEditorForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.搜索节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.展开节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.收起节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.FlowLayoutPanel_Main = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.Panel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.添加节点ToolStripMenuItem,
            this.删除节点ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.搜索节点ToolStripMenuItem,
            this.展开节点ToolStripMenuItem,
            this.收起节点ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1277, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开方案ToolStripMenuItem,
            this.新建方案ToolStripMenuItem,
            this.保存方案ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开方案ToolStripMenuItem
            // 
            this.打开方案ToolStripMenuItem.Name = "打开方案ToolStripMenuItem";
            this.打开方案ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.打开方案ToolStripMenuItem.Text = "打开方案";
            this.打开方案ToolStripMenuItem.Click += new System.EventHandler(this.打开方案ToolStripMenuItem_Click);
            // 
            // 新建方案ToolStripMenuItem
            // 
            this.新建方案ToolStripMenuItem.Name = "新建方案ToolStripMenuItem";
            this.新建方案ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.新建方案ToolStripMenuItem.Text = "新建方案";
            this.新建方案ToolStripMenuItem.Click += new System.EventHandler(this.新建方案ToolStripMenuItem_Click);
            // 
            // 保存方案ToolStripMenuItem
            // 
            this.保存方案ToolStripMenuItem.Enabled = false;
            this.保存方案ToolStripMenuItem.Name = "保存方案ToolStripMenuItem";
            this.保存方案ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.保存方案ToolStripMenuItem.Text = "保存方案";
            this.保存方案ToolStripMenuItem.Click += new System.EventHandler(this.保存方案ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(25, 24);
            this.toolStripMenuItem1.Text = "|";
            // 
            // 添加节点ToolStripMenuItem
            // 
            this.添加节点ToolStripMenuItem.Enabled = false;
            this.添加节点ToolStripMenuItem.Name = "添加节点ToolStripMenuItem";
            this.添加节点ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.添加节点ToolStripMenuItem.Text = "添加节点";
            this.添加节点ToolStripMenuItem.Click += new System.EventHandler(this.添加节点ToolStripMenuItem_Click);
            // 
            // 删除节点ToolStripMenuItem
            // 
            this.删除节点ToolStripMenuItem.Enabled = false;
            this.删除节点ToolStripMenuItem.Name = "删除节点ToolStripMenuItem";
            this.删除节点ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.删除节点ToolStripMenuItem.Text = "删除节点";
            this.删除节点ToolStripMenuItem.Click += new System.EventHandler(this.删除节点ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(25, 24);
            this.toolStripMenuItem2.Text = "|";
            // 
            // 搜索节点ToolStripMenuItem
            // 
            this.搜索节点ToolStripMenuItem.Enabled = false;
            this.搜索节点ToolStripMenuItem.Name = "搜索节点ToolStripMenuItem";
            this.搜索节点ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.搜索节点ToolStripMenuItem.Text = "搜索节点";
            this.搜索节点ToolStripMenuItem.Click += new System.EventHandler(this.搜索节点ToolStripMenuItem_Click);
            // 
            // 展开节点ToolStripMenuItem
            // 
            this.展开节点ToolStripMenuItem.Enabled = false;
            this.展开节点ToolStripMenuItem.Name = "展开节点ToolStripMenuItem";
            this.展开节点ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.展开节点ToolStripMenuItem.Text = "展开节点";
            this.展开节点ToolStripMenuItem.Click += new System.EventHandler(this.展开节点ToolStripMenuItem_Click);
            // 
            // 收起节点ToolStripMenuItem
            // 
            this.收起节点ToolStripMenuItem.Enabled = false;
            this.收起节点ToolStripMenuItem.Name = "收起节点ToolStripMenuItem";
            this.收起节点ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.收起节点ToolStripMenuItem.Text = "收起节点";
            this.收起节点ToolStripMenuItem.Click += new System.EventHandler(this.收起节点ToolStripMenuItem_Click);
            // 
            // Panel_Main
            // 
            this.Panel_Main.AutoScroll = true;
            this.Panel_Main.Controls.Add(this.FlowLayoutPanel_Main);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 28);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(1277, 774);
            this.Panel_Main.TabIndex = 1;
            // 
            // FlowLayoutPanel_Main
            // 
            this.FlowLayoutPanel_Main.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowLayoutPanel_Main.Location = new System.Drawing.Point(3, 3);
            this.FlowLayoutPanel_Main.Name = "FlowLayoutPanel_Main";
            this.FlowLayoutPanel_Main.Size = new System.Drawing.Size(1271, 768);
            this.FlowLayoutPanel_Main.TabIndex = 0;
            this.FlowLayoutPanel_Main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FlowLayoutPanel_Main_MouseClick);
            // 
            // DialogueDataFileEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 802);
            this.Controls.Add(this.Panel_Main);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DialogueDataFileEditorForm";
            this.Text = "对话数据编辑工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogueDataFileEditorForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Panel_Main.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 添加节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除节点ToolStripMenuItem;
        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 搜索节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 展开节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 收起节点ToolStripMenuItem;
    }
}

