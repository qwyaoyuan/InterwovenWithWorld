namespace SkillDataFileEditor
{
    partial class AutoItemControl
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
            this.FlowLayoutPanel_Release_Other = new System.Windows.Forms.FlowLayoutPanel();
            this.Panel_Release_Other = new System.Windows.Forms.Panel();
            this.Panel_Release_Other.SuspendLayout();
            this.SuspendLayout();
            // 
            // FlowLayoutPanel_Release_Other
            // 
            this.FlowLayoutPanel_Release_Other.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowLayoutPanel_Release_Other.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowLayoutPanel_Release_Other.Location = new System.Drawing.Point(0, 0);
            this.FlowLayoutPanel_Release_Other.Name = "FlowLayoutPanel_Release_Other";
            this.FlowLayoutPanel_Release_Other.Size = new System.Drawing.Size(603, 64);
            this.FlowLayoutPanel_Release_Other.TabIndex = 1;
            // 
            // Panel_Release_Other
            // 
            this.Panel_Release_Other.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Release_Other.AutoScroll = true;
            this.Panel_Release_Other.Controls.Add(this.FlowLayoutPanel_Release_Other);
            this.Panel_Release_Other.Location = new System.Drawing.Point(0, 0);
            this.Panel_Release_Other.Name = "Panel_Release_Other";
            this.Panel_Release_Other.Size = new System.Drawing.Size(606, 499);
            this.Panel_Release_Other.TabIndex = 7;
            // 
            // AutoItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Panel_Release_Other);
            this.Name = "AutoItemControl";
            this.Size = new System.Drawing.Size(606, 499);
            this.Panel_Release_Other.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Release_Other;
        private System.Windows.Forms.Panel Panel_Release_Other;
    }
}
