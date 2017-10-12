namespace SkillDataFileEditor
{
    partial class NewProjectForm
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
            this.Label_ProjectName = new System.Windows.Forms.Label();
            this.TextBox_ProjectName = new System.Windows.Forms.TextBox();
            this.Label_Folder = new System.Windows.Forms.Label();
            this.TextBox_Folder = new System.Windows.Forms.TextBox();
            this.Button_SelectFolder = new System.Windows.Forms.Button();
            this.FolderBrowserDialog_Select = new System.Windows.Forms.FolderBrowserDialog();
            this.Button_OK = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label_ProjectName
            // 
            this.Label_ProjectName.AutoSize = true;
            this.Label_ProjectName.Location = new System.Drawing.Point(12, 15);
            this.Label_ProjectName.Name = "Label_ProjectName";
            this.Label_ProjectName.Size = new System.Drawing.Size(52, 15);
            this.Label_ProjectName.TabIndex = 0;
            this.Label_ProjectName.Text = "方案名";
            // 
            // TextBox_ProjectName
            // 
            this.TextBox_ProjectName.Location = new System.Drawing.Point(80, 12);
            this.TextBox_ProjectName.Name = "TextBox_ProjectName";
            this.TextBox_ProjectName.Size = new System.Drawing.Size(372, 25);
            this.TextBox_ProjectName.TabIndex = 1;
            // 
            // Label_Folder
            // 
            this.Label_Folder.AutoSize = true;
            this.Label_Folder.Location = new System.Drawing.Point(12, 62);
            this.Label_Folder.Name = "Label_Folder";
            this.Label_Folder.Size = new System.Drawing.Size(52, 15);
            this.Label_Folder.TabIndex = 2;
            this.Label_Folder.Text = "文件夹";
            // 
            // TextBox_Folder
            // 
            this.TextBox_Folder.Location = new System.Drawing.Point(80, 59);
            this.TextBox_Folder.Name = "TextBox_Folder";
            this.TextBox_Folder.ReadOnly = true;
            this.TextBox_Folder.Size = new System.Drawing.Size(486, 25);
            this.TextBox_Folder.TabIndex = 1;
            // 
            // Button_SelectFolder
            // 
            this.Button_SelectFolder.Location = new System.Drawing.Point(572, 56);
            this.Button_SelectFolder.Name = "Button_SelectFolder";
            this.Button_SelectFolder.Size = new System.Drawing.Size(48, 30);
            this.Button_SelectFolder.TabIndex = 3;
            this.Button_SelectFolder.Text = "...";
            this.Button_SelectFolder.UseVisualStyleBackColor = true;
            this.Button_SelectFolder.Click += new System.EventHandler(this.Button_SelectFolder_Click);
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(458, 6);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 41);
            this.Button_OK.TabIndex = 4;
            this.Button_OK.Text = "确认";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Location = new System.Drawing.Point(539, 6);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 38);
            this.Button_Cancel.TabIndex = 4;
            this.Button_Cancel.Text = "取消";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 102);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Button_SelectFolder);
            this.Controls.Add(this.Label_Folder);
            this.Controls.Add(this.TextBox_Folder);
            this.Controls.Add(this.TextBox_ProjectName);
            this.Controls.Add(this.Label_ProjectName);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(645, 149);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(645, 149);
            this.Name = "NewProjectForm";
            this.Text = "新建方案";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_ProjectName;
        private System.Windows.Forms.TextBox TextBox_ProjectName;
        private System.Windows.Forms.Label Label_Folder;
        private System.Windows.Forms.TextBox TextBox_Folder;
        private System.Windows.Forms.Button Button_SelectFolder;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog_Select;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Button Button_Cancel;
    }
}