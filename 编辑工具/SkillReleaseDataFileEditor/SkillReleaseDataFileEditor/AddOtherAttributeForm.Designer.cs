namespace SkillDataFileEditor
{
    partial class AddOtherAttributeForm
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
            this.SetAttributeControl_Main = new SkillDataFileEditor.SetAttributeControl();
            this.Button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SetAttributeControl_Main
            // 
            this.SetAttributeControl_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetAttributeControl_Main.Location = new System.Drawing.Point(2, 61);
            this.SetAttributeControl_Main.Name = "SetAttributeControl_Main";
            this.SetAttributeControl_Main.Size = new System.Drawing.Size(779, 505);
            this.SetAttributeControl_Main.TabIndex = 0;
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(42, 12);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 36);
            this.Button_OK.TabIndex = 1;
            this.Button_OK.Text = "确认";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // AddOtherAttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 567);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.SetAttributeControl_Main);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 614);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 614);
            this.Name = "AddOtherAttributeForm";
            this.Text = "添加一个设置项";
            this.ResumeLayout(false);

        }

        #endregion

        private SetAttributeControl SetAttributeControl_Main;
        private System.Windows.Forms.Button Button_OK;
    }
}