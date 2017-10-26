namespace SkillDataFileEditor
{
    partial class AddSkillForm
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
            this.Label_SkillID = new System.Windows.Forms.Label();
            this.Label_SkillName = new System.Windows.Forms.Label();
            this.TextBox_SkillID = new System.Windows.Forms.TextBox();
            this.TextBox_SkillName = new System.Windows.Forms.TextBox();
            this.Button_OK = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label_SkillID
            // 
            this.Label_SkillID.AutoSize = true;
            this.Label_SkillID.Location = new System.Drawing.Point(41, 27);
            this.Label_SkillID.Name = "Label_SkillID";
            this.Label_SkillID.Size = new System.Drawing.Size(53, 15);
            this.Label_SkillID.TabIndex = 0;
            this.Label_SkillID.Text = "技能ID";
            // 
            // Label_SkillName
            // 
            this.Label_SkillName.AutoSize = true;
            this.Label_SkillName.Location = new System.Drawing.Point(41, 68);
            this.Label_SkillName.Name = "Label_SkillName";
            this.Label_SkillName.Size = new System.Drawing.Size(52, 15);
            this.Label_SkillName.TabIndex = 1;
            this.Label_SkillName.Text = "技能名";
            // 
            // TextBox_SkillID
            // 
            this.TextBox_SkillID.Location = new System.Drawing.Point(100, 24);
            this.TextBox_SkillID.Name = "TextBox_SkillID";
            this.TextBox_SkillID.Size = new System.Drawing.Size(78, 25);
            this.TextBox_SkillID.TabIndex = 2;
            // 
            // TextBox_SkillName
            // 
            this.TextBox_SkillName.Location = new System.Drawing.Point(99, 65);
            this.TextBox_SkillName.Name = "TextBox_SkillName";
            this.TextBox_SkillName.Size = new System.Drawing.Size(169, 25);
            this.TextBox_SkillName.TabIndex = 3;
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(57, 107);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(65, 36);
            this.Button_OK.TabIndex = 4;
            this.Button_OK.Text = "确认";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Location = new System.Drawing.Point(175, 107);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(65, 36);
            this.Button_Cancel.TabIndex = 5;
            this.Button_Cancel.Text = "取消";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // AddSkillForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 155);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.TextBox_SkillName);
            this.Controls.Add(this.TextBox_SkillID);
            this.Controls.Add(this.Label_SkillName);
            this.Controls.Add(this.Label_SkillID);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 202);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 202);
            this.Name = "AddSkillForm";
            this.Text = "添加技能";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_SkillID;
        private System.Windows.Forms.Label Label_SkillName;
        private System.Windows.Forms.TextBox TextBox_SkillID;
        private System.Windows.Forms.TextBox TextBox_SkillName;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Button Button_Cancel;
    }
}