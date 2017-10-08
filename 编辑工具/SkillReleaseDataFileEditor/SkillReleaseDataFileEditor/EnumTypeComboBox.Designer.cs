namespace SkillDataFileEditor
{
    partial class EnumTypeComboBox
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
            this.ComboBox_Main = new System.Windows.Forms.ComboBox();
            this.Label_Message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ComboBox_Main
            // 
            this.ComboBox_Main.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ComboBox_Main.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Main.FormattingEnabled = true;
            this.ComboBox_Main.Location = new System.Drawing.Point(3, 3);
            this.ComboBox_Main.Name = "ComboBox_Main";
            this.ComboBox_Main.Size = new System.Drawing.Size(121, 23);
            this.ComboBox_Main.TabIndex = 0;
            this.ComboBox_Main.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Main_SelectedIndexChanged);
            // 
            // Label_Message
            // 
            this.Label_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Label_Message.AutoSize = true;
            this.Label_Message.Location = new System.Drawing.Point(130, 6);
            this.Label_Message.Name = "Label_Message";
            this.Label_Message.Size = new System.Drawing.Size(0, 15);
            this.Label_Message.TabIndex = 1;
            // 
            // EnumTypeComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Label_Message);
            this.Controls.Add(this.ComboBox_Main);
            this.Name = "EnumTypeComboBox";
            this.Size = new System.Drawing.Size(244, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBox_Main;
        private System.Windows.Forms.Label Label_Message;
    }
}
