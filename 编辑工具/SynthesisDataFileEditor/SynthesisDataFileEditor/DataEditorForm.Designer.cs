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
            this.GroupBox_From = new System.Windows.Forms.GroupBox();
            this.Label_Time = new System.Windows.Forms.Label();
            this.NumericUpDown_Time = new System.Windows.Forms.NumericUpDown();
            this.Label_Time_Minute = new System.Windows.Forms.Label();
            this.Label_FromTo = new System.Windows.Forms.Label();
            this.GroupBox_To = new System.Windows.Forms.GroupBox();
            this.Button_GiveFrom = new System.Windows.Forms.Button();
            this.Button_GiveTo = new System.Windows.Forms.Button();
            this.Label_Level = new System.Windows.Forms.Label();
            this.NumericUpDown_Level = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Level)).BeginInit();
            this.SuspendLayout();
            // 
            // TreeView_ItemType
            // 
            this.TreeView_ItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_ItemType.HideSelection = false;
            this.TreeView_ItemType.Location = new System.Drawing.Point(4, 3);
            this.TreeView_ItemType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TreeView_ItemType.Name = "TreeView_ItemType";
            this.TreeView_ItemType.Size = new System.Drawing.Size(270, 449);
            this.TreeView_ItemType.TabIndex = 0;
            this.TreeView_ItemType.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_ItemType_ItemDrag);
            // 
            // Label_ID
            // 
            this.Label_ID.AutoSize = true;
            this.Label_ID.Location = new System.Drawing.Point(278, 7);
            this.Label_ID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_ID.Name = "Label_ID";
            this.Label_ID.Size = new System.Drawing.Size(17, 12);
            this.Label_ID.TabIndex = 1;
            this.Label_ID.Text = "ID";
            // 
            // TextBox_ID
            // 
            this.TextBox_ID.Location = new System.Drawing.Point(338, 5);
            this.TextBox_ID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextBox_ID.Name = "TextBox_ID";
            this.TextBox_ID.ReadOnly = true;
            this.TextBox_ID.Size = new System.Drawing.Size(76, 21);
            this.TextBox_ID.TabIndex = 2;
            // 
            // Label_Name
            // 
            this.Label_Name.AutoSize = true;
            this.Label_Name.Location = new System.Drawing.Point(466, 7);
            this.Label_Name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Name.Name = "Label_Name";
            this.Label_Name.Size = new System.Drawing.Size(41, 12);
            this.Label_Name.TabIndex = 1;
            this.Label_Name.Text = "合成名";
            // 
            // TextBox_Name
            // 
            this.TextBox_Name.Location = new System.Drawing.Point(526, 5);
            this.TextBox_Name.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextBox_Name.Name = "TextBox_Name";
            this.TextBox_Name.Size = new System.Drawing.Size(76, 21);
            this.TextBox_Name.TabIndex = 2;
            // 
            // Label_SynthesisType
            // 
            this.Label_SynthesisType.AutoSize = true;
            this.Label_SynthesisType.Location = new System.Drawing.Point(278, 32);
            this.Label_SynthesisType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_SynthesisType.Name = "Label_SynthesisType";
            this.Label_SynthesisType.Size = new System.Drawing.Size(53, 12);
            this.Label_SynthesisType.TabIndex = 1;
            this.Label_SynthesisType.Text = "合成类别";
            // 
            // TextBox_SynthesisType
            // 
            this.TextBox_SynthesisType.Location = new System.Drawing.Point(338, 30);
            this.TextBox_SynthesisType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextBox_SynthesisType.Name = "TextBox_SynthesisType";
            this.TextBox_SynthesisType.ReadOnly = true;
            this.TextBox_SynthesisType.Size = new System.Drawing.Size(76, 21);
            this.TextBox_SynthesisType.TabIndex = 2;
            // 
            // Label_SynthesisItem
            // 
            this.Label_SynthesisItem.AutoSize = true;
            this.Label_SynthesisItem.Location = new System.Drawing.Point(466, 34);
            this.Label_SynthesisItem.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_SynthesisItem.Name = "Label_SynthesisItem";
            this.Label_SynthesisItem.Size = new System.Drawing.Size(65, 12);
            this.Label_SynthesisItem.TabIndex = 1;
            this.Label_SynthesisItem.Text = "具体的类目";
            // 
            // TextBox_SynthesisItem
            // 
            this.TextBox_SynthesisItem.Location = new System.Drawing.Point(526, 32);
            this.TextBox_SynthesisItem.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextBox_SynthesisItem.Name = "TextBox_SynthesisItem";
            this.TextBox_SynthesisItem.ReadOnly = true;
            this.TextBox_SynthesisItem.Size = new System.Drawing.Size(76, 21);
            this.TextBox_SynthesisItem.TabIndex = 2;
            // 
            // GroupBox_From
            // 
            this.GroupBox_From.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox_From.Location = new System.Drawing.Point(300, 93);
            this.GroupBox_From.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBox_From.Name = "GroupBox_From";
            this.GroupBox_From.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBox_From.Size = new System.Drawing.Size(523, 285);
            this.GroupBox_From.TabIndex = 3;
            this.GroupBox_From.TabStop = false;
            this.GroupBox_From.Text = "合成材料";
            this.GroupBox_From.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupBox_From_DragDrop);
            this.GroupBox_From.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupBox_From_DragEnter);
            // 
            // Label_Time
            // 
            this.Label_Time.AutoSize = true;
            this.Label_Time.Location = new System.Drawing.Point(278, 60);
            this.Label_Time.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Time.Name = "Label_Time";
            this.Label_Time.Size = new System.Drawing.Size(53, 12);
            this.Label_Time.TabIndex = 1;
            this.Label_Time.Text = "合成时间";
            // 
            // NumericUpDown_Time
            // 
            this.NumericUpDown_Time.Location = new System.Drawing.Point(338, 54);
            this.NumericUpDown_Time.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NumericUpDown_Time.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NumericUpDown_Time.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Time.Name = "NumericUpDown_Time";
            this.NumericUpDown_Time.Size = new System.Drawing.Size(75, 21);
            this.NumericUpDown_Time.TabIndex = 4;
            this.NumericUpDown_Time.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Label_Time_Minute
            // 
            this.Label_Time_Minute.AutoSize = true;
            this.Label_Time_Minute.Location = new System.Drawing.Point(418, 60);
            this.Label_Time_Minute.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Time_Minute.Name = "Label_Time_Minute";
            this.Label_Time_Minute.Size = new System.Drawing.Size(29, 12);
            this.Label_Time_Minute.TabIndex = 1;
            this.Label_Time_Minute.Text = "分钟";
            // 
            // Label_FromTo
            // 
            this.Label_FromTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Label_FromTo.AutoSize = true;
            this.Label_FromTo.Location = new System.Drawing.Point(300, 380);
            this.Label_FromTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_FromTo.Name = "Label_FromTo";
            this.Label_FromTo.Size = new System.Drawing.Size(29, 12);
            this.Label_FromTo.TabIndex = 5;
            this.Label_FromTo.Text = "↓↓";
            // 
            // GroupBox_To
            // 
            this.GroupBox_To.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox_To.Location = new System.Drawing.Point(300, 402);
            this.GroupBox_To.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBox_To.Name = "GroupBox_To";
            this.GroupBox_To.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GroupBox_To.Size = new System.Drawing.Size(523, 50);
            this.GroupBox_To.TabIndex = 6;
            this.GroupBox_To.TabStop = false;
            this.GroupBox_To.Text = "合成物品";
            this.GroupBox_To.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupBox_To_DragDrop);
            this.GroupBox_To.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupBox_To_DragEnter);
            // 
            // Button_GiveFrom
            // 
            this.Button_GiveFrom.Location = new System.Drawing.Point(277, 93);
            this.Button_GiveFrom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Button_GiveFrom.Name = "Button_GiveFrom";
            this.Button_GiveFrom.Size = new System.Drawing.Size(18, 18);
            this.Button_GiveFrom.TabIndex = 7;
            this.Button_GiveFrom.Text = "→";
            this.Button_GiveFrom.UseVisualStyleBackColor = true;
            this.Button_GiveFrom.Click += new System.EventHandler(this.Button_GiveFrom_Click);
            // 
            // Button_GiveTo
            // 
            this.Button_GiveTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Button_GiveTo.Location = new System.Drawing.Point(278, 402);
            this.Button_GiveTo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Button_GiveTo.Name = "Button_GiveTo";
            this.Button_GiveTo.Size = new System.Drawing.Size(18, 18);
            this.Button_GiveTo.TabIndex = 7;
            this.Button_GiveTo.Text = "→";
            this.Button_GiveTo.UseVisualStyleBackColor = true;
            this.Button_GiveTo.Click += new System.EventHandler(this.Button_GiveTo_Click);
            // 
            // Label_Level
            // 
            this.Label_Level.AutoSize = true;
            this.Label_Level.Location = new System.Drawing.Point(466, 62);
            this.Label_Level.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Level.Name = "Label_Level";
            this.Label_Level.Size = new System.Drawing.Size(65, 12);
            this.Label_Level.TabIndex = 1;
            this.Label_Level.Text = "可合成等级";
            // 
            // NumericUpDown_Level
            // 
            this.NumericUpDown_Level.Location = new System.Drawing.Point(526, 57);
            this.NumericUpDown_Level.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NumericUpDown_Level.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDown_Level.Name = "NumericUpDown_Level";
            this.NumericUpDown_Level.Size = new System.Drawing.Size(75, 21);
            this.NumericUpDown_Level.TabIndex = 4;
            // 
            // DataEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 454);
            this.Controls.Add(this.Button_GiveTo);
            this.Controls.Add(this.Button_GiveFrom);
            this.Controls.Add(this.GroupBox_To);
            this.Controls.Add(this.Label_FromTo);
            this.Controls.Add(this.NumericUpDown_Level);
            this.Controls.Add(this.NumericUpDown_Time);
            this.Controls.Add(this.GroupBox_From);
            this.Controls.Add(this.TextBox_Name);
            this.Controls.Add(this.Label_Name);
            this.Controls.Add(this.TextBox_SynthesisItem);
            this.Controls.Add(this.Label_SynthesisItem);
            this.Controls.Add(this.TextBox_SynthesisType);
            this.Controls.Add(this.Label_SynthesisType);
            this.Controls.Add(this.TextBox_ID);
            this.Controls.Add(this.Label_Time_Minute);
            this.Controls.Add(this.Label_Level);
            this.Controls.Add(this.Label_Time);
            this.Controls.Add(this.Label_ID);
            this.Controls.Add(this.TreeView_ItemType);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DataEditorForm";
            this.Text = "编辑数据";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataEditorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Level)).EndInit();
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
        private System.Windows.Forms.GroupBox GroupBox_From;
        private System.Windows.Forms.Label Label_Time;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Time;
        private System.Windows.Forms.Label Label_Time_Minute;
        private System.Windows.Forms.Label Label_FromTo;
        private System.Windows.Forms.GroupBox GroupBox_To;
        private System.Windows.Forms.Button Button_GiveFrom;
        private System.Windows.Forms.Button Button_GiveTo;
        private System.Windows.Forms.Label Label_Level;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Level;
    }
}