namespace SynthesisDataFileEditor
{
    partial class DataEditorForm
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
            this.TreeView_ItemType = new System.Windows.Forms.TreeView();
            this.Label_ID = new System.Windows.Forms.Label();
            this.TextBox_ID = new System.Windows.Forms.TextBox();
            this.Label_Name = new System.Windows.Forms.Label();
            this.TextBox_Name = new System.Windows.Forms.TextBox();
            this.Label_SynthesisType = new System.Windows.Forms.Label();
            this.TextBox_SynthesisType = new System.Windows.Forms.TextBox();
            this.Label_SynthesisItem = new System.Windows.Forms.Label();
            this.TextBox_SynthesisItem = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TreeView_ItemType
            // 
            this.TreeView_ItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_ItemType.Location = new System.Drawing.Point(5, 4);
            this.TreeView_ItemType.Name = "TreeView_ItemType";
            this.TreeView_ItemType.Size = new System.Drawing.Size(359, 519);
            this.TreeView_ItemType.TabIndex = 0;
            // 
            // Label_ID
            // 
            this.Label_ID.AutoSize = true;
            this.Label_ID.Location = new System.Drawing.Point(370, 9);
            this.Label_ID.Name = "Label_ID";
            this.Label_ID.Size = new System.Drawing.Size(23, 15);
            this.Label_ID.TabIndex = 1;
            this.Label_ID.Text = "ID";
            // 
            // TextBox_ID
            // 
            this.TextBox_ID.Location = new System.Drawing.Point(451, 6);
            this.TextBox_ID.Name = "TextBox_ID";
            this.TextBox_ID.ReadOnly = true;
            this.TextBox_ID.Size = new System.Drawing.Size(100, 25);
            this.TextBox_ID.TabIndex = 2;
            // 
            // Label_Name
            // 
            this.Label_Name.AutoSize = true;
            this.Label_Name.Location = new System.Drawing.Point(621, 9);
            this.Label_Name.Name = "Label_Name";
            this.Label_Name.Size = new System.Drawing.Size(52, 15);
            this.Label_Name.TabIndex = 1;
            this.Label_Name.Text = "合成名";
            // 
            // TextBox_Name
            // 
            this.TextBox_Name.Location = new System.Drawing.Point(702, 6);
            this.TextBox_Name.Name = "TextBox_Name";
            this.TextBox_Name.Size = new System.Drawing.Size(100, 25);
            this.TextBox_Name.TabIndex = 2;
            // 
            // Label_SynthesisType
            // 
            this.Label_SynthesisType.AutoSize = true;
            this.Label_SynthesisType.Location = new System.Drawing.Point(370, 40);
            this.Label_SynthesisType.Name = "Label_SynthesisType";
            this.Label_SynthesisType.Size = new System.Drawing.Size(67, 15);
            this.Label_SynthesisType.TabIndex = 1;
            this.Label_SynthesisType.Text = "合成类别";
            // 
            // TextBox_SynthesisType
            // 
            this.TextBox_SynthesisType.Location = new System.Drawing.Point(451, 37);
            this.TextBox_SynthesisType.Name = "TextBox_SynthesisType";
            this.TextBox_SynthesisType.ReadOnly = true;
            this.TextBox_SynthesisType.Size = new System.Drawing.Size(100, 25);
            this.TextBox_SynthesisType.TabIndex = 2;
            // 
            // Label_SynthesisItem
            // 
            this.Label_SynthesisItem.AutoSize = true;
            this.Label_SynthesisItem.Location = new System.Drawing.Point(621, 43);
            this.Label_SynthesisItem.Name = "Label_SynthesisItem";
            this.Label_SynthesisItem.Size = new System.Drawing.Size(82, 15);
            this.Label_SynthesisItem.TabIndex = 1;
            this.Label_SynthesisItem.Text = "具体的类目";
            // 
            // TextBox_SynthesisItem
            // 
            this.TextBox_SynthesisItem.Location = new System.Drawing.Point(702, 40);
            this.TextBox_SynthesisItem.Name = "TextBox_SynthesisItem";
            this.TextBox_SynthesisItem.ReadOnly = true;
            this.TextBox_SynthesisItem.Size = new System.Drawing.Size(100, 25);
            this.TextBox_SynthesisItem.TabIndex = 2;
            // 
            // DataEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 527);
            this.Controls.Add(this.TextBox_Name);
            this.Controls.Add(this.Label_Name);
            this.Controls.Add(this.TextBox_SynthesisItem);
            this.Controls.Add(this.Label_SynthesisItem);
            this.Controls.Add(this.TextBox_SynthesisType);
            this.Controls.Add(this.Label_SynthesisType);
            this.Controls.Add(this.TextBox_ID);
            this.Controls.Add(this.Label_ID);
            this.Controls.Add(this.TreeView_ItemType);
            this.Name = "DataEditorForm";
            this.Text = "编辑数据";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView TreeView_ItemType;
        private System.Windows.Forms.Label Label_ID;
        private System.Windows.Forms.TextBox TextBox_ID;
        private System.Windows.Forms.Label Label_Name;
        private System.Windows.Forms.TextBox TextBox_Name;
        private System.Windows.Forms.Label Label_SynthesisType;
        private System.Windows.Forms.TextBox TextBox_SynthesisType;
        private System.Windows.Forms.Label Label_SynthesisItem;
        private System.Windows.Forms.TextBox TextBox_SynthesisItem;
    }
}