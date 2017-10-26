namespace SynthesisDataFileEditor
{
    partial class SynthesisDataFileEditorForm
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
            this.MenuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_Main = new System.Windows.Forms.TreeView();
            this.MenuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip_Main
            // 
            this.MenuStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.添加节点ToolStripMenuItem,
            this.删除节点ToolStripMenuItem});
            this.MenuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip_Main.Name = "MenuStrip_Main";
            this.MenuStrip_Main.Size = new System.Drawing.Size(995, 28);
            this.MenuStrip_Main.TabIndex = 0;
            this.MenuStrip_Main.Text = "menuStrip1";
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
            this.保存方案ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
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
            // TreeView_Main
            // 
            this.TreeView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_Main.Location = new System.Drawing.Point(0, 28);
            this.TreeView_Main.Name = "TreeView_Main";
            this.TreeView_Main.Size = new System.Drawing.Size(995, 602);
            this.TreeView_Main.TabIndex = 1;
            this.TreeView_Main.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Main_NodeMouseDoubleClick);
            // 
            // SynthesisDataFileEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 630);
            this.Controls.Add(this.TreeView_Main);
            this.Controls.Add(this.MenuStrip_Main);
            this.MainMenuStrip = this.MenuStrip_Main;
            this.Name = "SynthesisDataFileEditorForm";
            this.Text = "合成条件编辑";
            this.MenuStrip_Main.ResumeLayout(false);
            this.MenuStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存方案ToolStripMenuItem;
        private System.Windows.Forms.TreeView TreeView_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 添加节点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除节点ToolStripMenuItem;
    }
}

