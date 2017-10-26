namespace SkillDataFileEditor
{
    partial class TypeTextBox
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
            this.TextBox_Main = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBox_Main
            // 
            this.TextBox_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Main.Location = new System.Drawing.Point(0, 0);
            this.TextBox_Main.Name = "TextBox_Main";
            this.TextBox_Main.Size = new System.Drawing.Size(150, 25);
            this.TextBox_Main.TabIndex = 0;
            this.TextBox_Main.TextChanged += new System.EventHandler(this.TextBox_Main_TextChanged);
            // 
            // TypeTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBox_Main);
            this.Name = "TypeTextBox";
            this.Size = new System.Drawing.Size(150, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox_Main;
    }
}
