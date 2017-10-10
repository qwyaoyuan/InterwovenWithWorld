namespace SkillDataFileEditor
{
    partial class AutoArrayControl
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
            this.FlowLayoutPanel_Main = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // FlowLayoutPanel_Main
            // 
            this.FlowLayoutPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlowLayoutPanel_Main.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowLayoutPanel_Main.Location = new System.Drawing.Point(0, 0);
            this.FlowLayoutPanel_Main.Name = "FlowLayoutPanel_Main";
            this.FlowLayoutPanel_Main.Size = new System.Drawing.Size(355, 254);
            this.FlowLayoutPanel_Main.TabIndex = 0;
            this.FlowLayoutPanel_Main.SizeChanged += new System.EventHandler(this.FlowLayoutPanel_Main_SizeChanged);
            // 
            // AutoArrayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FlowLayoutPanel_Main);
            this.Name = "AutoArrayControl";
            this.Size = new System.Drawing.Size(355, 254);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Main;
    }
}
