namespace SkillDataFileEditor
{
    partial class SetAttributeForm
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
            this.TabControl_SkillAttribute = new System.Windows.Forms.TabControl();
            this.Button_Add = new System.Windows.Forms.Button();
            this.Button_Subtract = new System.Windows.Forms.Button();
            this.Button_Save = new System.Windows.Forms.Button();
            this.Button_Left = new System.Windows.Forms.Button();
            this.Button_Right = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TabControl_SkillAttribute
            // 
            this.TabControl_SkillAttribute.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl_SkillAttribute.Location = new System.Drawing.Point(4, 1);
            this.TabControl_SkillAttribute.Name = "TabControl_SkillAttribute";
            this.TabControl_SkillAttribute.SelectedIndex = 0;
            this.TabControl_SkillAttribute.Size = new System.Drawing.Size(802, 562);
            this.TabControl_SkillAttribute.TabIndex = 0;
            // 
            // Button_Add
            // 
            this.Button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Add.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_Add.Location = new System.Drawing.Point(812, 12);
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(50, 50);
            this.Button_Add.TabIndex = 1;
            this.Button_Add.Text = "+";
            this.Button_Add.UseVisualStyleBackColor = true;
            this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
            // 
            // Button_Subtract
            // 
            this.Button_Subtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Subtract.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_Subtract.Location = new System.Drawing.Point(812, 68);
            this.Button_Subtract.Name = "Button_Subtract";
            this.Button_Subtract.Size = new System.Drawing.Size(50, 50);
            this.Button_Subtract.TabIndex = 2;
            this.Button_Subtract.Text = "-";
            this.Button_Subtract.UseVisualStyleBackColor = true;
            this.Button_Subtract.Click += new System.EventHandler(this.Button_Subtract_Click);
            // 
            // Button_Save
            // 
            this.Button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Save.Location = new System.Drawing.Point(812, 505);
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(50, 50);
            this.Button_Save.TabIndex = 3;
            this.Button_Save.Text = "✓";
            this.Button_Save.UseVisualStyleBackColor = true;
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Left
            // 
            this.Button_Left.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Left.Location = new System.Drawing.Point(812, 179);
            this.Button_Left.Name = "Button_Left";
            this.Button_Left.Size = new System.Drawing.Size(50, 50);
            this.Button_Left.TabIndex = 4;
            this.Button_Left.Text = "←";
            this.Button_Left.UseVisualStyleBackColor = true;
            this.Button_Left.Click += new System.EventHandler(this.Button_Left_Click);
            // 
            // Button_Right
            // 
            this.Button_Right.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Right.Location = new System.Drawing.Point(812, 235);
            this.Button_Right.Name = "Button_Right";
            this.Button_Right.Size = new System.Drawing.Size(50, 50);
            this.Button_Right.TabIndex = 4;
            this.Button_Right.Text = "→";
            this.Button_Right.UseVisualStyleBackColor = true;
            this.Button_Right.Click += new System.EventHandler(this.Button_Right_Click);
            // 
            // SetAttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 567);
            this.Controls.Add(this.Button_Right);
            this.Controls.Add(this.Button_Left);
            this.Controls.Add(this.Button_Save);
            this.Controls.Add(this.Button_Subtract);
            this.Controls.Add(this.Button_Add);
            this.Controls.Add(this.TabControl_SkillAttribute);
            this.Name = "SetAttributeForm";
            this.Text = "配置技能属性";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl_SkillAttribute;
        private System.Windows.Forms.Button Button_Add;
        private System.Windows.Forms.Button Button_Subtract;
        private System.Windows.Forms.Button Button_Save;
        private System.Windows.Forms.Button Button_Left;
        private System.Windows.Forms.Button Button_Right;
    }
}