namespace DialogueDataFileEditor
{
    partial class SearchNodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuStrip_Search = new System.Windows.Forms.MenuStrip();
            this.ToolStripTextBox_SearchTitle = new System.Windows.Forms.ToolStripTextBox();
            this.ToolStripTextBox_TopDialogueID = new System.Windows.Forms.ToolStripTextBox();
            this.ToolStripTextBox_Keyword = new System.Windows.Forms.ToolStripTextBox();
            this.ToolStripComboBox_DialogueType = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStripMenuItem_Search = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_Main = new System.Windows.Forms.TreeView();
            this.StatusStrip_Main = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel_Explan = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripTextBox_NPCID = new System.Windows.Forms.ToolStripTextBox();
            this.ToolStripMenuItem_Auto = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_Search.SuspendLayout();
            this.StatusStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip_Search
            // 
            this.MenuStrip_Search.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip_Search.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripTextBox_SearchTitle,
            this.ToolStripTextBox_TopDialogueID,
            this.ToolStripTextBox_Keyword,
            this.ToolStripTextBox_NPCID,
            this.ToolStripComboBox_DialogueType,
            this.ToolStripMenuItem_Search,
            this.ToolStripMenuItem_Clear,
            this.ToolStripMenuItem_Auto});
            this.MenuStrip_Search.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip_Search.Name = "MenuStrip_Search";
            this.MenuStrip_Search.Size = new System.Drawing.Size(872, 32);
            this.MenuStrip_Search.TabIndex = 0;
            // 
            // ToolStripTextBox_SearchTitle
            // 
            this.ToolStripTextBox_SearchTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToolStripTextBox_SearchTitle.Enabled = false;
            this.ToolStripTextBox_SearchTitle.Name = "ToolStripTextBox_SearchTitle";
            this.ToolStripTextBox_SearchTitle.Size = new System.Drawing.Size(80, 28);
            this.ToolStripTextBox_SearchTitle.Text = "搜索条件";
            // 
            // ToolStripTextBox_TopDialogueID
            // 
            this.ToolStripTextBox_TopDialogueID.Name = "ToolStripTextBox_TopDialogueID";
            this.ToolStripTextBox_TopDialogueID.Size = new System.Drawing.Size(100, 28);
            this.ToolStripTextBox_TopDialogueID.Tag = "对话ID";
            this.ToolStripTextBox_TopDialogueID.Enter += new System.EventHandler(this.ToolStripTextBox_Enter);
            this.ToolStripTextBox_TopDialogueID.Leave += new System.EventHandler(this.ToolStripTextBox_Leave);
            this.ToolStripTextBox_TopDialogueID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripTextBox_KeyUp);
            // 
            // ToolStripTextBox_Keyword
            // 
            this.ToolStripTextBox_Keyword.Name = "ToolStripTextBox_Keyword";
            this.ToolStripTextBox_Keyword.Size = new System.Drawing.Size(100, 28);
            this.ToolStripTextBox_Keyword.Tag = "对话名关键字";
            this.ToolStripTextBox_Keyword.Enter += new System.EventHandler(this.ToolStripTextBox_Enter);
            this.ToolStripTextBox_Keyword.Leave += new System.EventHandler(this.ToolStripTextBox_Leave);
            this.ToolStripTextBox_Keyword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripTextBox_KeyUp);
            // 
            // ToolStripComboBox_DialogueType
            // 
            this.ToolStripComboBox_DialogueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToolStripComboBox_DialogueType.Items.AddRange(new object[] {
            "None",
            "Normal",
            "Task",
            "Ask"});
            this.ToolStripComboBox_DialogueType.Name = "ToolStripComboBox_DialogueType";
            this.ToolStripComboBox_DialogueType.Size = new System.Drawing.Size(121, 28);
            // 
            // ToolStripMenuItem_Search
            // 
            this.ToolStripMenuItem_Search.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ToolStripMenuItem_Search.Name = "ToolStripMenuItem_Search";
            this.ToolStripMenuItem_Search.Size = new System.Drawing.Size(51, 28);
            this.ToolStripMenuItem_Search.Text = "搜索";
            this.ToolStripMenuItem_Search.Click += new System.EventHandler(this.ToolStripMenuItem_Search_Click);
            // 
            // ToolStripMenuItem_Clear
            // 
            this.ToolStripMenuItem_Clear.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ToolStripMenuItem_Clear.Name = "ToolStripMenuItem_Clear";
            this.ToolStripMenuItem_Clear.Size = new System.Drawing.Size(81, 28);
            this.ToolStripMenuItem_Clear.Text = "清空条件";
            this.ToolStripMenuItem_Clear.Click += new System.EventHandler(this.ToolStripMenuItem_Clear_Click);
            // 
            // TreeView_Main
            // 
            this.TreeView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_Main.Location = new System.Drawing.Point(0, 32);
            this.TreeView_Main.Name = "TreeView_Main";
            this.TreeView_Main.Size = new System.Drawing.Size(872, 540);
            this.TreeView_Main.TabIndex = 1;
            this.TreeView_Main.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_Main_NodeMouseDoubleClick);
            // 
            // StatusStrip_Main
            // 
            this.StatusStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel_Explan});
            this.StatusStrip_Main.Location = new System.Drawing.Point(0, 547);
            this.StatusStrip_Main.Name = "StatusStrip_Main";
            this.StatusStrip_Main.Size = new System.Drawing.Size(872, 25);
            this.StatusStrip_Main.TabIndex = 2;
            // 
            // ToolStripStatusLabel_Explan
            // 
            this.ToolStripStatusLabel_Explan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolStripStatusLabel_Explan.Name = "ToolStripStatusLabel_Explan";
            this.ToolStripStatusLabel_Explan.Size = new System.Drawing.Size(701, 20);
            this.ToolStripStatusLabel_Explan.Text = "对话ID仅表示对话关系节点的第一个子节点的ID，NPC的ID仅表示对话关系节点的第一个子节点的NPCID";
            // 
            // ToolStripTextBox_NPCID
            // 
            this.ToolStripTextBox_NPCID.Name = "ToolStripTextBox_NPCID";
            this.ToolStripTextBox_NPCID.Size = new System.Drawing.Size(100, 28);
            this.ToolStripTextBox_NPCID.Tag = "NPC的ID";
            this.ToolStripTextBox_NPCID.Enter += new System.EventHandler(this.ToolStripTextBox_Enter);
            this.ToolStripTextBox_NPCID.Leave += new System.EventHandler(this.ToolStripTextBox_Leave);
            this.ToolStripTextBox_NPCID.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ToolStripTextBox_KeyUp);
            // 
            // ToolStripMenuItem_Auto
            // 
            this.ToolStripMenuItem_Auto.Image = global::DialogueDataFileEditor.Properties.Resources.Empty;
            this.ToolStripMenuItem_Auto.Name = "ToolStripMenuItem_Auto";
            this.ToolStripMenuItem_Auto.Size = new System.Drawing.Size(101, 28);
            this.ToolStripMenuItem_Auto.Text = "自动更新";
            this.ToolStripMenuItem_Auto.Click += new System.EventHandler(this.ToolStripMenuItem_Auto_Click);
            // 
            // SearchNodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 572);
            this.Controls.Add(this.StatusStrip_Main);
            this.Controls.Add(this.TreeView_Main);
            this.Controls.Add(this.MenuStrip_Search);
            this.MainMenuStrip = this.MenuStrip_Search;
            this.Name = "SearchNodeForm";
            this.Text = "搜索节点";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchNodeForm_FormClosing);
            this.MenuStrip_Search.ResumeLayout(false);
            this.MenuStrip_Search.PerformLayout();
            this.StatusStrip_Main.ResumeLayout(false);
            this.StatusStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip_Search;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBox_SearchTitle;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBox_TopDialogueID;
        private System.Windows.Forms.ToolStripComboBox ToolStripComboBox_DialogueType;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBox_Keyword;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Search;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Clear;
        private System.Windows.Forms.TreeView TreeView_Main;
        private System.Windows.Forms.StatusStrip StatusStrip_Main;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel_Explan;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBox_NPCID;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Auto;
    }
}