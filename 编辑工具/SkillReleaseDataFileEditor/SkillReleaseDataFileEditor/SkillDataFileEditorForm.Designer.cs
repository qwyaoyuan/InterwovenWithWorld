namespace SkillDataFileEditor
{
    partial class SkillDataFileEditorForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TreeView_Skills = new System.Windows.Forms.TreeView();
            this.TabControl_Setting = new System.Windows.Forms.TabControl();
            this.TabPage_Base = new System.Windows.Forms.TabPage();
            this.Label_Base_SkillCombine = new System.Windows.Forms.Label();
            this.AutoArray_Base_SkillCombine = new SkillDataFileEditor.AutoArrayControl();
            this.EnumType_Base_SkillType = new SkillDataFileEditor.EnumTypeComboBox();
            this.TextBox_Base_SkillType = new System.Windows.Forms.TextBox();
            this.Label_Base_SkillType = new System.Windows.Forms.Label();
            this.TextBox_Base_SkillName = new System.Windows.Forms.TextBox();
            this.TextBox_Base_SkillID = new System.Windows.Forms.TextBox();
            this.Label_Base_SKillName = new System.Windows.Forms.Label();
            this.Label_Base_SkillID = new System.Windows.Forms.Label();
            this.TabPage_SkillRelease = new System.Windows.Forms.TabPage();
            this.AutoItemControl_Release_Other = new SkillDataFileEditor.AutoItemControl();
            this.EnumType_Release_Type = new SkillDataFileEditor.EnumTypeComboBox();
            this.EnumType_Release_Mode = new SkillDataFileEditor.EnumTypeComboBox();
            this.TextBox_Release_CombineType = new System.Windows.Forms.TextBox();
            this.Label_Release_CombineType = new System.Windows.Forms.Label();
            this.TextBox_Release_Mode = new System.Windows.Forms.TextBox();
            this.Label_Release_Mode = new System.Windows.Forms.Label();
            this.TabPage_Attribute = new System.Windows.Forms.TabPage();
            this.TypeTextBox_Attribute_SkillMaxLevel = new SkillDataFileEditor.TypeTextBox();
            this.Panel_Attribute = new System.Windows.Forms.Panel();
            this.FlowLayoutPanel_Attribute = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_Attribute_SkillMaxLevel = new System.Windows.Forms.Label();
            this.Panel_Skill = new System.Windows.Forms.Panel();
            this.Button_SaveSkillToMemory = new System.Windows.Forms.Button();
            this.Button_DeleteSkill = new System.Windows.Forms.Button();
            this.Button_AddSkill = new System.Windows.Forms.Button();
            this.Button_NewProject = new System.Windows.Forms.Button();
            this.Button_SaveProject = new System.Windows.Forms.Button();
            this.Button_LoadProject = new System.Windows.Forms.Button();
            this.TabControl_Setting.SuspendLayout();
            this.TabPage_Base.SuspendLayout();
            this.TabPage_SkillRelease.SuspendLayout();
            this.TabPage_Attribute.SuspendLayout();
            this.Panel_Attribute.SuspendLayout();
            this.Panel_Skill.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreeView_Skills
            // 
            this.TreeView_Skills.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Skills.Location = new System.Drawing.Point(1, 51);
            this.TreeView_Skills.Name = "TreeView_Skills";
            this.TreeView_Skills.Size = new System.Drawing.Size(308, 618);
            this.TreeView_Skills.TabIndex = 0;
            this.TreeView_Skills.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Skills_AfterSelect);
            // 
            // TabControl_Setting
            // 
            this.TabControl_Setting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl_Setting.Controls.Add(this.TabPage_Base);
            this.TabControl_Setting.Controls.Add(this.TabPage_SkillRelease);
            this.TabControl_Setting.Controls.Add(this.TabPage_Attribute);
            this.TabControl_Setting.Location = new System.Drawing.Point(315, 51);
            this.TabControl_Setting.Name = "TabControl_Setting";
            this.TabControl_Setting.SelectedIndex = 0;
            this.TabControl_Setting.Size = new System.Drawing.Size(854, 618);
            this.TabControl_Setting.TabIndex = 1;
            // 
            // TabPage_Base
            // 
            this.TabPage_Base.Controls.Add(this.Label_Base_SkillCombine);
            this.TabPage_Base.Controls.Add(this.AutoArray_Base_SkillCombine);
            this.TabPage_Base.Controls.Add(this.EnumType_Base_SkillType);
            this.TabPage_Base.Controls.Add(this.TextBox_Base_SkillType);
            this.TabPage_Base.Controls.Add(this.Label_Base_SkillType);
            this.TabPage_Base.Controls.Add(this.TextBox_Base_SkillName);
            this.TabPage_Base.Controls.Add(this.TextBox_Base_SkillID);
            this.TabPage_Base.Controls.Add(this.Label_Base_SKillName);
            this.TabPage_Base.Controls.Add(this.Label_Base_SkillID);
            this.TabPage_Base.Location = new System.Drawing.Point(4, 25);
            this.TabPage_Base.Name = "TabPage_Base";
            this.TabPage_Base.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Base.Size = new System.Drawing.Size(846, 589);
            this.TabPage_Base.TabIndex = 1;
            this.TabPage_Base.Text = "基础数据";
            this.TabPage_Base.UseVisualStyleBackColor = true;
            // 
            // Label_Base_SkillCombine
            // 
            this.Label_Base_SkillCombine.AutoSize = true;
            this.Label_Base_SkillCombine.Location = new System.Drawing.Point(36, 146);
            this.Label_Base_SkillCombine.Name = "Label_Base_SkillCombine";
            this.Label_Base_SkillCombine.Size = new System.Drawing.Size(37, 15);
            this.Label_Base_SkillCombine.TabIndex = 8;
            this.Label_Base_SkillCombine.Text = "组合";
            // 
            // AutoArray_Base_SkillCombine
            // 
            this.AutoArray_Base_SkillCombine.ChildControlType = "SkillDataFileEditor.EnumTypeComboBox";
            this.AutoArray_Base_SkillCombine.Count = 4;
            this.AutoArray_Base_SkillCombine.IsChangedValue = true;
            this.AutoArray_Base_SkillCombine.Location = new System.Drawing.Point(96, 135);
            this.AutoArray_Base_SkillCombine.Name = "AutoArray_Base_SkillCombine";
            this.AutoArray_Base_SkillCombine.Size = new System.Drawing.Size(670, 143);
            this.AutoArray_Base_SkillCombine.TabIndex = 7;
            this.AutoArray_Base_SkillCombine.Tag = "combinSkillTypes";
            this.AutoArray_Base_SkillCombine.TextValues = new string[] {
        "None",
        "None",
        "None",
        "None"};
            this.AutoArray_Base_SkillCombine.TypeTag = "EnumSkillType";
            // 
            // EnumType_Base_SkillType
            // 
            this.EnumType_Base_SkillType.IsChangedValue = true;
            this.EnumType_Base_SkillType.ListenControl = "TextBox_Base_SkillType";
            this.EnumType_Base_SkillType.Location = new System.Drawing.Point(260, 97);
            this.EnumType_Base_SkillType.Name = "EnumType_Base_SkillType";
            this.EnumType_Base_SkillType.Size = new System.Drawing.Size(513, 32);
            this.EnumType_Base_SkillType.TabIndex = 6;
            this.EnumType_Base_SkillType.TextValue = "None";
            this.EnumType_Base_SkillType.TypeTag = "EnumSkillType";
            this.EnumType_Base_SkillType.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // TextBox_Base_SkillType
            // 
            this.TextBox_Base_SkillType.Location = new System.Drawing.Point(103, 99);
            this.TextBox_Base_SkillType.Name = "TextBox_Base_SkillType";
            this.TextBox_Base_SkillType.ReadOnly = true;
            this.TextBox_Base_SkillType.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillType.TabIndex = 5;
            this.TextBox_Base_SkillType.Tag = "skillType";
            // 
            // Label_Base_SkillType
            // 
            this.Label_Base_SkillType.AutoSize = true;
            this.Label_Base_SkillType.Location = new System.Drawing.Point(7, 102);
            this.Label_Base_SkillType.Name = "Label_Base_SkillType";
            this.Label_Base_SkillType.Size = new System.Drawing.Size(67, 15);
            this.Label_Base_SkillType.TabIndex = 4;
            this.Label_Base_SkillType.Text = "技能类型";
            // 
            // TextBox_Base_SkillName
            // 
            this.TextBox_Base_SkillName.Location = new System.Drawing.Point(103, 54);
            this.TextBox_Base_SkillName.Name = "TextBox_Base_SkillName";
            this.TextBox_Base_SkillName.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillName.TabIndex = 3;
            this.TextBox_Base_SkillName.Tag = "skillName";
            // 
            // TextBox_Base_SkillID
            // 
            this.TextBox_Base_SkillID.Location = new System.Drawing.Point(103, 13);
            this.TextBox_Base_SkillID.Name = "TextBox_Base_SkillID";
            this.TextBox_Base_SkillID.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillID.TabIndex = 2;
            this.TextBox_Base_SkillID.Tag = "skillID";
            // 
            // Label_Base_SKillName
            // 
            this.Label_Base_SKillName.AutoSize = true;
            this.Label_Base_SKillName.Location = new System.Drawing.Point(21, 57);
            this.Label_Base_SKillName.Name = "Label_Base_SKillName";
            this.Label_Base_SKillName.Size = new System.Drawing.Size(52, 15);
            this.Label_Base_SKillName.TabIndex = 1;
            this.Label_Base_SKillName.Text = "技能名";
            // 
            // Label_Base_SkillID
            // 
            this.Label_Base_SkillID.AutoSize = true;
            this.Label_Base_SkillID.Location = new System.Drawing.Point(21, 16);
            this.Label_Base_SkillID.Name = "Label_Base_SkillID";
            this.Label_Base_SkillID.Size = new System.Drawing.Size(53, 15);
            this.Label_Base_SkillID.TabIndex = 0;
            this.Label_Base_SkillID.Text = "技能ID";
            // 
            // TabPage_SkillRelease
            // 
            this.TabPage_SkillRelease.Controls.Add(this.AutoItemControl_Release_Other);
            this.TabPage_SkillRelease.Controls.Add(this.EnumType_Release_Type);
            this.TabPage_SkillRelease.Controls.Add(this.EnumType_Release_Mode);
            this.TabPage_SkillRelease.Controls.Add(this.TextBox_Release_CombineType);
            this.TabPage_SkillRelease.Controls.Add(this.Label_Release_CombineType);
            this.TabPage_SkillRelease.Controls.Add(this.TextBox_Release_Mode);
            this.TabPage_SkillRelease.Controls.Add(this.Label_Release_Mode);
            this.TabPage_SkillRelease.Location = new System.Drawing.Point(4, 25);
            this.TabPage_SkillRelease.Name = "TabPage_SkillRelease";
            this.TabPage_SkillRelease.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_SkillRelease.Size = new System.Drawing.Size(846, 589);
            this.TabPage_SkillRelease.TabIndex = 0;
            this.TabPage_SkillRelease.Text = "技能释放";
            this.TabPage_SkillRelease.UseVisualStyleBackColor = true;
            // 
            // AutoItemControl_Release_Other
            // 
            this.AutoItemControl_Release_Other.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoItemControl_Release_Other.IsChangedValue = false;
            this.AutoItemControl_Release_Other.Location = new System.Drawing.Point(6, 90);
            this.AutoItemControl_Release_Other.Name = "AutoItemControl_Release_Other";
            this.AutoItemControl_Release_Other.Size = new System.Drawing.Size(837, 492);
            this.AutoItemControl_Release_Other.TabIndex = 6;
            // 
            // EnumType_Release_Type
            // 
            this.EnumType_Release_Type.IsChangedValue = true;
            this.EnumType_Release_Type.ListenControl = "TextBox_Release_CombineType";
            this.EnumType_Release_Type.Location = new System.Drawing.Point(237, 52);
            this.EnumType_Release_Type.Name = "EnumType_Release_Type";
            this.EnumType_Release_Type.Size = new System.Drawing.Size(513, 32);
            this.EnumType_Release_Type.TabIndex = 5;
            this.EnumType_Release_Type.TextValue = "Direct";
            this.EnumType_Release_Type.TypeTag = "EnumReleaseType";
            this.EnumType_Release_Type.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // EnumType_Release_Mode
            // 
            this.EnumType_Release_Mode.IsChangedValue = true;
            this.EnumType_Release_Mode.ListenControl = "TextBox_Release_Mode";
            this.EnumType_Release_Mode.Location = new System.Drawing.Point(237, 11);
            this.EnumType_Release_Mode.Name = "EnumType_Release_Mode";
            this.EnumType_Release_Mode.Size = new System.Drawing.Size(513, 32);
            this.EnumType_Release_Mode.TabIndex = 4;
            this.EnumType_Release_Mode.TextValue = "None";
            this.EnumType_Release_Mode.TypeTag = "EnumReleaseMode";
            this.EnumType_Release_Mode.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // TextBox_Release_CombineType
            // 
            this.TextBox_Release_CombineType.Location = new System.Drawing.Point(124, 54);
            this.TextBox_Release_CombineType.Name = "TextBox_Release_CombineType";
            this.TextBox_Release_CombineType.ReadOnly = true;
            this.TextBox_Release_CombineType.Size = new System.Drawing.Size(100, 25);
            this.TextBox_Release_CombineType.TabIndex = 3;
            this.TextBox_Release_CombineType.Tag = "releaseType";
            // 
            // Label_Release_CombineType
            // 
            this.Label_Release_CombineType.AutoSize = true;
            this.Label_Release_CombineType.Location = new System.Drawing.Point(21, 57);
            this.Label_Release_CombineType.Name = "Label_Release_CombineType";
            this.Label_Release_CombineType.Size = new System.Drawing.Size(97, 15);
            this.Label_Release_CombineType.TabIndex = 2;
            this.Label_Release_CombineType.Text = "技能组合类型";
            // 
            // TextBox_Release_Mode
            // 
            this.TextBox_Release_Mode.Location = new System.Drawing.Point(124, 13);
            this.TextBox_Release_Mode.Name = "TextBox_Release_Mode";
            this.TextBox_Release_Mode.ReadOnly = true;
            this.TextBox_Release_Mode.Size = new System.Drawing.Size(100, 25);
            this.TextBox_Release_Mode.TabIndex = 1;
            this.TextBox_Release_Mode.Tag = "releaseMode";
            this.TextBox_Release_Mode.TextChanged += new System.EventHandler(this.TextBox_Release_Mode_TextChanged);
            // 
            // Label_Release_Mode
            // 
            this.Label_Release_Mode.AutoSize = true;
            this.Label_Release_Mode.Location = new System.Drawing.Point(21, 16);
            this.Label_Release_Mode.Name = "Label_Release_Mode";
            this.Label_Release_Mode.Size = new System.Drawing.Size(97, 15);
            this.Label_Release_Mode.TabIndex = 0;
            this.Label_Release_Mode.Text = "技能释放方式";
            // 
            // TabPage_Attribute
            // 
            this.TabPage_Attribute.Controls.Add(this.TypeTextBox_Attribute_SkillMaxLevel);
            this.TabPage_Attribute.Controls.Add(this.Panel_Attribute);
            this.TabPage_Attribute.Controls.Add(this.Label_Attribute_SkillMaxLevel);
            this.TabPage_Attribute.Location = new System.Drawing.Point(4, 25);
            this.TabPage_Attribute.Name = "TabPage_Attribute";
            this.TabPage_Attribute.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Attribute.Size = new System.Drawing.Size(846, 589);
            this.TabPage_Attribute.TabIndex = 2;
            this.TabPage_Attribute.Text = "属性";
            this.TabPage_Attribute.UseVisualStyleBackColor = true;
            // 
            // TypeTextBox_Attribute_SkillMaxLevel
            // 
            this.TypeTextBox_Attribute_SkillMaxLevel.IsChangedValue = true;
            this.TypeTextBox_Attribute_SkillMaxLevel.Location = new System.Drawing.Point(90, 8);
            this.TypeTextBox_Attribute_SkillMaxLevel.Name = "TypeTextBox_Attribute_SkillMaxLevel";
            this.TypeTextBox_Attribute_SkillMaxLevel.Size = new System.Drawing.Size(150, 26);
            this.TypeTextBox_Attribute_SkillMaxLevel.TabIndex = 4;
            this.TypeTextBox_Attribute_SkillMaxLevel.Tag = "skillLevel";
            this.TypeTextBox_Attribute_SkillMaxLevel.TextValue = "0";
            this.TypeTextBox_Attribute_SkillMaxLevel.TypeTag = "Syste.Int32";
            this.TypeTextBox_Attribute_SkillMaxLevel.TypeTextChanged += new System.EventHandler<SkillDataFileEditor.TypeTextBoxEventArgs>(this.TypeTextBox_Attribute_SkillMaxLevel_TypeTextChanged);
            // 
            // Panel_Attribute
            // 
            this.Panel_Attribute.AutoScroll = true;
            this.Panel_Attribute.Controls.Add(this.FlowLayoutPanel_Attribute);
            this.Panel_Attribute.Location = new System.Drawing.Point(6, 49);
            this.Panel_Attribute.Name = "Panel_Attribute";
            this.Panel_Attribute.Size = new System.Drawing.Size(834, 533);
            this.Panel_Attribute.TabIndex = 3;
            // 
            // FlowLayoutPanel_Attribute
            // 
            this.FlowLayoutPanel_Attribute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowLayoutPanel_Attribute.AutoSize = true;
            this.FlowLayoutPanel_Attribute.Location = new System.Drawing.Point(3, 3);
            this.FlowLayoutPanel_Attribute.Name = "FlowLayoutPanel_Attribute";
            this.FlowLayoutPanel_Attribute.Size = new System.Drawing.Size(828, 527);
            this.FlowLayoutPanel_Attribute.TabIndex = 2;
            // 
            // Label_Attribute_SkillMaxLevel
            // 
            this.Label_Attribute_SkillMaxLevel.AutoSize = true;
            this.Label_Attribute_SkillMaxLevel.Location = new System.Drawing.Point(17, 14);
            this.Label_Attribute_SkillMaxLevel.Name = "Label_Attribute_SkillMaxLevel";
            this.Label_Attribute_SkillMaxLevel.Size = new System.Drawing.Size(67, 15);
            this.Label_Attribute_SkillMaxLevel.TabIndex = 0;
            this.Label_Attribute_SkillMaxLevel.Text = "最高等级";
            // 
            // Panel_Skill
            // 
            this.Panel_Skill.Controls.Add(this.Button_SaveSkillToMemory);
            this.Panel_Skill.Controls.Add(this.Button_DeleteSkill);
            this.Panel_Skill.Controls.Add(this.Button_AddSkill);
            this.Panel_Skill.Controls.Add(this.Button_NewProject);
            this.Panel_Skill.Controls.Add(this.Button_SaveProject);
            this.Panel_Skill.Controls.Add(this.Button_LoadProject);
            this.Panel_Skill.Location = new System.Drawing.Point(3, 0);
            this.Panel_Skill.Name = "Panel_Skill";
            this.Panel_Skill.Size = new System.Drawing.Size(1162, 45);
            this.Panel_Skill.TabIndex = 2;
            // 
            // Button_SaveSkillToMemory
            // 
            this.Button_SaveSkillToMemory.Location = new System.Drawing.Point(553, 6);
            this.Button_SaveSkillToMemory.Name = "Button_SaveSkillToMemory";
            this.Button_SaveSkillToMemory.Size = new System.Drawing.Size(130, 34);
            this.Button_SaveSkillToMemory.TabIndex = 5;
            this.Button_SaveSkillToMemory.Text = "保存修改到内存";
            this.Button_SaveSkillToMemory.UseVisualStyleBackColor = true;
            this.Button_SaveSkillToMemory.Click += new System.EventHandler(this.Button_SaveSkillToMemory_Click);
            // 
            // Button_DeleteSkill
            // 
            this.Button_DeleteSkill.Enabled = false;
            this.Button_DeleteSkill.Location = new System.Drawing.Point(459, 6);
            this.Button_DeleteSkill.Name = "Button_DeleteSkill";
            this.Button_DeleteSkill.Size = new System.Drawing.Size(88, 34);
            this.Button_DeleteSkill.TabIndex = 4;
            this.Button_DeleteSkill.Text = "删除技能";
            this.Button_DeleteSkill.UseVisualStyleBackColor = true;
            this.Button_DeleteSkill.Click += new System.EventHandler(this.Button_DeleteSkill_Click);
            // 
            // Button_AddSkill
            // 
            this.Button_AddSkill.Enabled = false;
            this.Button_AddSkill.Location = new System.Drawing.Point(365, 6);
            this.Button_AddSkill.Name = "Button_AddSkill";
            this.Button_AddSkill.Size = new System.Drawing.Size(88, 34);
            this.Button_AddSkill.TabIndex = 3;
            this.Button_AddSkill.Text = "添加技能";
            this.Button_AddSkill.UseVisualStyleBackColor = true;
            this.Button_AddSkill.Click += new System.EventHandler(this.Button_AddSkill_Click);
            // 
            // Button_NewProject
            // 
            this.Button_NewProject.Location = new System.Drawing.Point(217, 6);
            this.Button_NewProject.Name = "Button_NewProject";
            this.Button_NewProject.Size = new System.Drawing.Size(88, 34);
            this.Button_NewProject.TabIndex = 2;
            this.Button_NewProject.Text = "新建项目";
            this.Button_NewProject.UseVisualStyleBackColor = true;
            this.Button_NewProject.Click += new System.EventHandler(this.Button_NewProject_Click);
            // 
            // Button_SaveProject
            // 
            this.Button_SaveProject.Enabled = false;
            this.Button_SaveProject.Location = new System.Drawing.Point(110, 6);
            this.Button_SaveProject.Name = "Button_SaveProject";
            this.Button_SaveProject.Size = new System.Drawing.Size(88, 34);
            this.Button_SaveProject.TabIndex = 1;
            this.Button_SaveProject.Text = "保存项目";
            this.Button_SaveProject.UseVisualStyleBackColor = true;
            this.Button_SaveProject.Click += new System.EventHandler(this.Button_SaveProject_Click);
            // 
            // Button_LoadProject
            // 
            this.Button_LoadProject.Location = new System.Drawing.Point(3, 6);
            this.Button_LoadProject.Name = "Button_LoadProject";
            this.Button_LoadProject.Size = new System.Drawing.Size(88, 34);
            this.Button_LoadProject.TabIndex = 0;
            this.Button_LoadProject.Text = "加载项目";
            this.Button_LoadProject.UseVisualStyleBackColor = true;
            this.Button_LoadProject.Click += new System.EventHandler(this.Button_LoadProject_Click);
            // 
            // SkillDataFileEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 670);
            this.Controls.Add(this.Panel_Skill);
            this.Controls.Add(this.TabControl_Setting);
            this.Controls.Add(this.TreeView_Skills);
            this.Name = "SkillDataFileEditorForm";
            this.Text = "技能编辑器";
            this.TabControl_Setting.ResumeLayout(false);
            this.TabPage_Base.ResumeLayout(false);
            this.TabPage_Base.PerformLayout();
            this.TabPage_SkillRelease.ResumeLayout(false);
            this.TabPage_SkillRelease.PerformLayout();
            this.TabPage_Attribute.ResumeLayout(false);
            this.TabPage_Attribute.PerformLayout();
            this.Panel_Attribute.ResumeLayout(false);
            this.Panel_Attribute.PerformLayout();
            this.Panel_Skill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TreeView_Skills;
        private System.Windows.Forms.TabControl TabControl_Setting;
        private System.Windows.Forms.TabPage TabPage_SkillRelease;
        private System.Windows.Forms.Panel Panel_Skill;
        private System.Windows.Forms.Button Button_SaveProject;
        private System.Windows.Forms.Button Button_LoadProject;
        private System.Windows.Forms.Button Button_NewProject;
        private System.Windows.Forms.Button Button_DeleteSkill;
        private System.Windows.Forms.Button Button_AddSkill;
        private System.Windows.Forms.TabPage TabPage_Base;
        private System.Windows.Forms.Label Label_Base_SkillID;
        private System.Windows.Forms.Label Label_Base_SKillName;
        private System.Windows.Forms.TextBox TextBox_Base_SkillName;
        private System.Windows.Forms.TextBox TextBox_Base_SkillID;
        private System.Windows.Forms.Button Button_SaveSkillToMemory;
        private System.Windows.Forms.Label Label_Release_Mode;
        private System.Windows.Forms.TextBox TextBox_Release_Mode;
        private System.Windows.Forms.Label Label_Release_CombineType;
        private System.Windows.Forms.TextBox TextBox_Release_CombineType;
        private EnumTypeComboBox EnumType_Release_Mode;
        private EnumTypeComboBox EnumType_Release_Type;
        private System.Windows.Forms.Label Label_Base_SkillType;
        private EnumTypeComboBox EnumType_Base_SkillType;
        private System.Windows.Forms.TextBox TextBox_Base_SkillType;
        private AutoArrayControl AutoArray_Base_SkillCombine;
        private System.Windows.Forms.Label Label_Base_SkillCombine;
        private AutoItemControl AutoItemControl_Release_Other;
        private System.Windows.Forms.TabPage TabPage_Attribute;
        private System.Windows.Forms.Label Label_Attribute_SkillMaxLevel;
        private System.Windows.Forms.Panel Panel_Attribute;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Attribute;
        private TypeTextBox TypeTextBox_Attribute_SkillMaxLevel;
    }
}

