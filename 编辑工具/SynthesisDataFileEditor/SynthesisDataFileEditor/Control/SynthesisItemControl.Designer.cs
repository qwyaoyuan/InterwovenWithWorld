namespace SynthesisDataFileEditor
{
    partial class SynthesisItemControl
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
            this.Label_SynthesisItem = new System.Windows.Forms.Label();
            this.TextBox_SynthesisItem = new System.Windows.Forms.TextBox();
            this.Label_Num = new System.Windows.Forms.Label();
            this.NumericUpDown_Num = new System.Windows.Forms.NumericUpDown();
            this.Label_QualityRange = new System.Windows.Forms.Label();
            this.ComboBox_MinQuality = new System.Windows.Forms.ComboBox();
            this.ComboBox_MaxQuality = new System.Windows.Forms.ComboBox();
            this.Label_QualityTo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Num)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_SynthesisItem
            // 
            this.Label_SynthesisItem.AutoSize = true;
            this.Label_SynthesisItem.Location = new System.Drawing.Point(2, 7);
            this.Label_SynthesisItem.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_SynthesisItem.Name = "Label_SynthesisItem";
            this.Label_SynthesisItem.Size = new System.Drawing.Size(29, 12);
            this.Label_SynthesisItem.TabIndex = 0;
            this.Label_SynthesisItem.Text = "类型";
            // 
            // TextBox_SynthesisItem
            // 
            this.TextBox_SynthesisItem.Location = new System.Drawing.Point(29, 4);
            this.TextBox_SynthesisItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextBox_SynthesisItem.Name = "TextBox_SynthesisItem";
            this.TextBox_SynthesisItem.ReadOnly = true;
            this.TextBox_SynthesisItem.Size = new System.Drawing.Size(76, 21);
            this.TextBox_SynthesisItem.TabIndex = 1;
            // 
            // Label_Num
            // 
            this.Label_Num.AutoSize = true;
            this.Label_Num.Location = new System.Drawing.Point(107, 7);
            this.Label_Num.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Num.Name = "Label_Num";
            this.Label_Num.Size = new System.Drawing.Size(29, 12);
            this.Label_Num.TabIndex = 0;
            this.Label_Num.Text = "数量";
            // 
            // NumericUpDown_Num
            // 
            this.NumericUpDown_Num.Location = new System.Drawing.Point(137, 5);
            this.NumericUpDown_Num.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NumericUpDown_Num.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Num.Name = "NumericUpDown_Num";
            this.NumericUpDown_Num.Size = new System.Drawing.Size(42, 21);
            this.NumericUpDown_Num.TabIndex = 2;
            this.NumericUpDown_Num.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Label_QualityRange
            // 
            this.Label_QualityRange.AutoSize = true;
            this.Label_QualityRange.Location = new System.Drawing.Point(183, 8);
            this.Label_QualityRange.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_QualityRange.Name = "Label_QualityRange";
            this.Label_QualityRange.Size = new System.Drawing.Size(53, 12);
            this.Label_QualityRange.TabIndex = 3;
            this.Label_QualityRange.Text = "品质范围";
            // 
            // ComboBox_MinQuality
            // 
            this.ComboBox_MinQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_MinQuality.FormattingEnabled = true;
            this.ComboBox_MinQuality.Location = new System.Drawing.Point(238, 5);
            this.ComboBox_MinQuality.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ComboBox_MinQuality.Name = "ComboBox_MinQuality";
            this.ComboBox_MinQuality.Size = new System.Drawing.Size(98, 20);
            this.ComboBox_MinQuality.TabIndex = 4;
            // 
            // ComboBox_MaxQuality
            // 
            this.ComboBox_MaxQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_MaxQuality.FormattingEnabled = true;
            this.ComboBox_MaxQuality.Location = new System.Drawing.Point(356, 4);
            this.ComboBox_MaxQuality.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ComboBox_MaxQuality.Name = "ComboBox_MaxQuality";
            this.ComboBox_MaxQuality.Size = new System.Drawing.Size(98, 20);
            this.ComboBox_MaxQuality.TabIndex = 4;
            // 
            // Label_QualityTo
            // 
            this.Label_QualityTo.AutoSize = true;
            this.Label_QualityTo.Location = new System.Drawing.Point(340, 8);
            this.Label_QualityTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_QualityTo.Name = "Label_QualityTo";
            this.Label_QualityTo.Size = new System.Drawing.Size(11, 12);
            this.Label_QualityTo.TabIndex = 3;
            this.Label_QualityTo.Text = "~";
            // 
            // SynthesisItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ComboBox_MaxQuality);
            this.Controls.Add(this.ComboBox_MinQuality);
            this.Controls.Add(this.Label_QualityTo);
            this.Controls.Add(this.Label_QualityRange);
            this.Controls.Add(this.NumericUpDown_Num);
            this.Controls.Add(this.TextBox_SynthesisItem);
            this.Controls.Add(this.Label_Num);
            this.Controls.Add(this.Label_SynthesisItem);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SynthesisItemControl";
            this.Size = new System.Drawing.Size(454, 30);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Num)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_SynthesisItem;
        private System.Windows.Forms.TextBox TextBox_SynthesisItem;
        private System.Windows.Forms.Label Label_Num;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Num;
        private System.Windows.Forms.Label Label_QualityRange;
        private System.Windows.Forms.ComboBox ComboBox_MinQuality;
        private System.Windows.Forms.ComboBox ComboBox_MaxQuality;
        private System.Windows.Forms.Label Label_QualityTo;
    }
}
