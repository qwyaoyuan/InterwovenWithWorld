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
            this.EnumType_Release_Mode = new SkillDataFileEditor.EnumTypeComboBox();
            this.TextBox_Base_Mode = new System.Windows.Forms.TextBox();
            this.Label_Base_Mode = new System.Windows.Forms.Label();
            this.EnumType_Base_SkillType = new SkillDataFileEditor.EnumTypeComboBox();
            this.TextBox_Base_SkillType = new System.Windows.Forms.TextBox();
            this.Label_Base_SkillType = new System.Windows.Forms.Label();
            this.TextBox_Base_SkillName = new System.Windows.Forms.TextBox();
            this.TextBox_Base_SkillID = new System.Windows.Forms.TextBox();
            this.Label_Base_SKillName = new System.Windows.Forms.Label();
            this.Label_Base_SkillID = new System.Windows.Forms.Label();
            this.TabPage_SkillRelease = new System.Windows.Forms.TabPage();
            this.Label_SkillRelease_Particle = new System.Windows.Forms.Label();
            this.AutoArrayControl_SkillRelease_Particle = new SkillDataFileEditor.AutoArrayControl();
            this.TabPage_Attribute = new System.Windows.Forms.TabPage();
            this.Button_SetAttributePanel = new System.Windows.Forms.Button();
            this.TypeTextBox_Attribute_SkillMaxLevel = new SkillDataFileEditor.TypeTextBox();
            this.Panel_Attribute = new System.Windows.Forms.Panel();
            this.FlowLayoutPanel_Attribute = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_Attribute_SkillMaxLevel = new System.Windows.Forms.Label();
            this.TabPage_Other = new System.Windows.Forms.TabPage();
            this.ComboBox_Other_Item = new System.Windows.Forms.ComboBox();
            this.Button_Other_Add = new System.Windows.Forms.Button();
            this.Button_Other_Delete = new System.Windows.Forms.Button();
            this.Panel_Other = new System.Windows.Forms.Panel();
            this.FlowLayoutPanel_Other = new System.Windows.Forms.FlowLayoutPanel();
            this.Panel_Skill = new System.Windows.Forms.Panel();
            this.Button_SaveSkillToMemory = new System.Windows.Forms.Button();
            this.Button_DeleteSkill = new System.Windows.Forms.Button();
            this.Button_AddSkill = new System.Windows.Forms.Button();
            this.Button_NewProject = new System.Windows.Forms.Button();
            this.Button_SaveProject = new System.Windows.Forms.Button();
            this.Button_LoadProject = new System.Windows.Forms.Button();
            this.Label_SkillRelease_Belief = new System.Windows.Forms.Label();
            this.EnumTypeComboBox_SkillRelease_Belief = new SkillDataFileEditor.EnumTypeComboBox();
            this.TextBox_SkillRelease_Belief = new System.Windows.Forms.TextBox();
            this.Label_SkillRelease_Explan = new System.Windows.Forms.Label();
            this.TextBox_SkillRelease_Explan = new System.Windows.Forms.TextBox();
            this.Label_SkillRelease_Effect = new System.Windows.Forms.Label();
            this.AutoArrayControl_SkillRelease_Effect = new SkillDataFileEditor.AutoArrayControl();
            this.TabControl_Setting.SuspendLayout();
            this.TabPage_Base.SuspendLayout();
            this.TabPage_SkillRelease.SuspendLayout();
            this.TabPage_Attribute.SuspendLayout();
            this.Panel_Attribute.SuspendLayout();
            this.TabPage_Other.SuspendLayout();
            this.Panel_Other.SuspendLayout();
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
            this.TabControl_Setting.Controls.Add(this.TabPage_Other);
            this.TabControl_Setting.Location = new System.Drawing.Point(315, 51);
            this.TabControl_Setting.Name = "TabControl_Setting";
            this.TabControl_Setting.SelectedIndex = 0;
            this.TabControl_Setting.Size = new System.Drawing.Size(854, 618);
            this.TabControl_Setting.TabIndex = 1;
            // 
            // TabPage_Base
            // 
            this.TabPage_Base.Controls.Add(this.EnumType_Release_Mode);
            this.TabPage_Base.Controls.Add(this.TextBox_Base_Mode);
            this.TabPage_Base.Controls.Add(this.Label_Base_Mode);
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
            // EnumType_Release_Mode
            // 
            this.EnumType_Release_Mode.IsChangedValue = true;
            this.EnumType_Release_Mode.ListenControl = "TextBox_Base_Mode";
            this.EnumType_Release_Mode.Location = new System.Drawing.Point(281, 143);
            this.EnumType_Release_Mode.Name = "EnumType_Release_Mode";
            this.EnumType_Release_Mode.Size = new System.Drawing.Size(513, 32);
            this.EnumType_Release_Mode.TabIndex = 9;
            this.EnumType_Release_Mode.TextValue = "None";
            this.EnumType_Release_Mode.TypeTag = "EnumReleaseMode";
            this.EnumType_Release_Mode.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // TextBox_Base_Mode
            // 
            this.TextBox_Base_Mode.Location = new System.Drawing.Point(124, 146);
            this.TextBox_Base_Mode.Name = "TextBox_Base_Mode";
            this.TextBox_Base_Mode.ReadOnly = true;
            this.TextBox_Base_Mode.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_Mode.TabIndex = 8;
            this.TextBox_Base_Mode.Tag = "releaseMode";
            // 
            // Label_Base_Mode
            // 
            this.Label_Base_Mode.AutoSize = true;
            this.Label_Base_Mode.Location = new System.Drawing.Point(21, 149);
            this.Label_Base_Mode.Name = "Label_Base_Mode";
            this.Label_Base_Mode.Size = new System.Drawing.Size(97, 15);
            this.Label_Base_Mode.TabIndex = 7;
            this.Label_Base_Mode.Text = "技能释放方式";
            // 
            // EnumType_Base_SkillType
            // 
            this.EnumType_Base_SkillType.IsChangedValue = true;
            this.EnumType_Base_SkillType.ListenControl = "TextBox_Base_SkillType";
            this.EnumType_Base_SkillType.Location = new System.Drawing.Point(281, 98);
            this.EnumType_Base_SkillType.Name = "EnumType_Base_SkillType";
            this.EnumType_Base_SkillType.Size = new System.Drawing.Size(513, 32);
            this.EnumType_Base_SkillType.TabIndex = 6;
            this.EnumType_Base_SkillType.TextValue = "None";
            this.EnumType_Base_SkillType.TypeTag = "EnumSkillType";
            this.EnumType_Base_SkillType.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // TextBox_Base_SkillType
            // 
            this.TextBox_Base_SkillType.Location = new System.Drawing.Point(124, 100);
            this.TextBox_Base_SkillType.Name = "TextBox_Base_SkillType";
            this.TextBox_Base_SkillType.ReadOnly = true;
            this.TextBox_Base_SkillType.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillType.TabIndex = 5;
            this.TextBox_Base_SkillType.Tag = "skillType";
            // 
            // Label_Base_SkillType
            // 
            this.Label_Base_SkillType.AutoSize = true;
            this.Label_Base_SkillType.Location = new System.Drawing.Point(28, 103);
            this.Label_Base_SkillType.Name = "Label_Base_SkillType";
            this.Label_Base_SkillType.Size = new System.Drawing.Size(67, 15);
            this.Label_Base_SkillType.TabIndex = 4;
            this.Label_Base_SkillType.Text = "技能类型";
            // 
            // TextBox_Base_SkillName
            // 
            this.TextBox_Base_SkillName.Location = new System.Drawing.Point(124, 55);
            this.TextBox_Base_SkillName.Name = "TextBox_Base_SkillName";
            this.TextBox_Base_SkillName.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillName.TabIndex = 3;
            this.TextBox_Base_SkillName.Tag = "skillName";
            // 
            // TextBox_Base_SkillID
            // 
            this.TextBox_Base_SkillID.Location = new System.Drawing.Point(124, 14);
            this.TextBox_Base_SkillID.Name = "TextBox_Base_SkillID";
            this.TextBox_Base_SkillID.Size = new System.Drawing.Size(151, 25);
            this.TextBox_Base_SkillID.TabIndex = 2;
            this.TextBox_Base_SkillID.Tag = "skillID";
            // 
            // Label_Base_SKillName
            // 
            this.Label_Base_SKillName.AutoSize = true;
            this.Label_Base_SKillName.Location = new System.Drawing.Point(42, 58);
            this.Label_Base_SKillName.Name = "Label_Base_SKillName";
            this.Label_Base_SKillName.Size = new System.Drawing.Size(52, 15);
            this.Label_Base_SKillName.TabIndex = 1;
            this.Label_Base_SKillName.Text = "技能名";
            // 
            // Label_Base_SkillID
            // 
            this.Label_Base_SkillID.AutoSize = true;
            this.Label_Base_SkillID.Location = new System.Drawing.Point(42, 17);
            this.Label_Base_SkillID.Name = "Label_Base_SkillID";
            this.Label_Base_SkillID.Size = new System.Drawing.Size(53, 15);
            this.Label_Base_SkillID.TabIndex = 0;
            this.Label_Base_SkillID.Text = "技能ID";
            // 
            // TabPage_SkillRelease
            // 
            this.TabPage_SkillRelease.Controls.Add(this.AutoArrayControl_SkillRelease_Effect);
            this.TabPage_SkillRelease.Controls.Add(this.Label_SkillRelease_Effect);
            this.TabPage_SkillRelease.Controls.Add(this.TextBox_SkillRelease_Explan);
            this.TabPage_SkillRelease.Controls.Add(this.Label_SkillRelease_Explan);
            this.TabPage_SkillRelease.Controls.Add(this.TextBox_SkillRelease_Belief);
            this.TabPage_SkillRelease.Controls.Add(this.EnumTypeComboBox_SkillRelease_Belief);
            this.TabPage_SkillRelease.Controls.Add(this.Label_SkillRelease_Belief);
            this.TabPage_SkillRelease.Controls.Add(this.Label_SkillRelease_Particle);
            this.TabPage_SkillRelease.Controls.Add(this.AutoArrayControl_SkillRelease_Particle);
            this.TabPage_SkillRelease.Location = new System.Drawing.Point(4, 25);
            this.TabPage_SkillRelease.Name = "TabPage_SkillRelease";
            this.TabPage_SkillRelease.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_SkillRelease.Size = new System.Drawing.Size(846, 589);
            this.TabPage_SkillRelease.TabIndex = 0;
            this.TabPage_SkillRelease.Text = "技能效果";
            this.TabPage_SkillRelease.UseVisualStyleBackColor = true;
            // 
            // Label_SkillRelease_Particle
            // 
            this.Label_SkillRelease_Particle.AutoSize = true;
            this.Label_SkillRelease_Particle.Location = new System.Drawing.Point(21, 16);
            this.Label_SkillRelease_Particle.Name = "Label_SkillRelease_Particle";
            this.Label_SkillRelease_Particle.Size = new System.Drawing.Size(82, 15);
            this.Label_SkillRelease_Particle.TabIndex = 1;
            this.Label_SkillRelease_Particle.Text = "使用的粒子";
            // 
            // AutoArrayControl_SkillRelease_Particle
            // 
            this.AutoArrayControl_SkillRelease_Particle.ChildControlType = "SkillDataFileEditor.TypeTextBox";
            this.AutoArrayControl_SkillRelease_Particle.Count = 5;
            this.AutoArrayControl_SkillRelease_Particle.IsChangedValue = false;
            this.AutoArrayControl_SkillRelease_Particle.Location = new System.Drawing.Point(122, 6);
            this.AutoArrayControl_SkillRelease_Particle.Name = "AutoArrayControl_SkillRelease_Particle";
            this.AutoArrayControl_SkillRelease_Particle.Size = new System.Drawing.Size(225, 160);
            this.AutoArrayControl_SkillRelease_Particle.TabIndex = 0;
            this.AutoArrayControl_SkillRelease_Particle.Tag = "particleNames";
            this.AutoArrayControl_SkillRelease_Particle.TextValues = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.AutoArrayControl_SkillRelease_Particle.TypeTag = "System.String";
            // 
            // TabPage_Attribute
            // 
            this.TabPage_Attribute.Controls.Add(this.Button_SetAttributePanel);
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
            // Button_SetAttributePanel
            // 
            this.Button_SetAttributePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_SetAttributePanel.Location = new System.Drawing.Point(762, 11);
            this.Button_SetAttributePanel.Name = "Button_SetAttributePanel";
            this.Button_SetAttributePanel.Size = new System.Drawing.Size(75, 32);
            this.Button_SetAttributePanel.TabIndex = 5;
            this.Button_SetAttributePanel.Text = "配置";
            this.Button_SetAttributePanel.UseVisualStyleBackColor = true;
            this.Button_SetAttributePanel.Click += new System.EventHandler(this.Button_SetAttributePanel_Click);
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
            // TabPage_Other
            // 
            this.TabPage_Other.Controls.Add(this.ComboBox_Other_Item);
            this.TabPage_Other.Controls.Add(this.Button_Other_Add);
            this.TabPage_Other.Controls.Add(this.Button_Other_Delete);
            this.TabPage_Other.Controls.Add(this.Panel_Other);
            this.TabPage_Other.Location = new System.Drawing.Point(4, 25);
            this.TabPage_Other.Name = "TabPage_Other";
            this.TabPage_Other.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Other.Size = new System.Drawing.Size(846, 589);
            this.TabPage_Other.TabIndex = 3;
            this.TabPage_Other.Text = "其他";
            this.TabPage_Other.UseVisualStyleBackColor = true;
            // 
            // ComboBox_Other_Item
            // 
            this.ComboBox_Other_Item.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Other_Item.FormattingEnabled = true;
            this.ComboBox_Other_Item.Location = new System.Drawing.Point(502, 19);
            this.ComboBox_Other_Item.Name = "ComboBox_Other_Item";
            this.ComboBox_Other_Item.Size = new System.Drawing.Size(160, 23);
            this.ComboBox_Other_Item.TabIndex = 3;
            // 
            // Button_Other_Add
            // 
            this.Button_Other_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Other_Add.Location = new System.Drawing.Point(684, 15);
            this.Button_Other_Add.Name = "Button_Other_Add";
            this.Button_Other_Add.Size = new System.Drawing.Size(75, 33);
            this.Button_Other_Add.TabIndex = 2;
            this.Button_Other_Add.Text = "添加";
            this.Button_Other_Add.UseVisualStyleBackColor = true;
            this.Button_Other_Add.Click += new System.EventHandler(this.Button_Other_Add_Click);
            // 
            // Button_Other_Delete
            // 
            this.Button_Other_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Other_Delete.Location = new System.Drawing.Point(765, 15);
            this.Button_Other_Delete.Name = "Button_Other_Delete";
            this.Button_Other_Delete.Size = new System.Drawing.Size(75, 33);
            this.Button_Other_Delete.TabIndex = 1;
            this.Button_Other_Delete.Text = "移除";
            this.Button_Other_Delete.UseVisualStyleBackColor = true;
            this.Button_Other_Delete.Click += new System.EventHandler(this.Button_Other_Delete_Click);
            // 
            // Panel_Other
            // 
            this.Panel_Other.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Other.AutoScroll = true;
            this.Panel_Other.Controls.Add(this.FlowLayoutPanel_Other);
            this.Panel_Other.Location = new System.Drawing.Point(6, 54);
            this.Panel_Other.Name = "Panel_Other";
            this.Panel_Other.Size = new System.Drawing.Size(834, 528);
            this.Panel_Other.TabIndex = 0;
            // 
            // FlowLayoutPanel_Other
            // 
            this.FlowLayoutPanel_Other.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowLayoutPanel_Other.AutoSize = true;
            this.FlowLayoutPanel_Other.Location = new System.Drawing.Point(3, 3);
            this.FlowLayoutPanel_Other.Name = "FlowLayoutPanel_Other";
            this.FlowLayoutPanel_Other.Size = new System.Drawing.Size(828, 494);
            this.FlowLayoutPanel_Other.TabIndex = 0;
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
            this.Button_SaveSkillToMemory.Enabled = false;
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
            // Label_SkillRelease_Belief
            // 
            this.Label_SkillRelease_Belief.AutoSize = true;
            this.Label_SkillRelease_Belief.Location = new System.Drawing.Point(21, 180);
            this.Label_SkillRelease_Belief.Name = "Label_SkillRelease_Belief";
            this.Label_SkillRelease_Belief.Size = new System.Drawing.Size(67, 15);
            this.Label_SkillRelease_Belief.TabIndex = 2;
            this.Label_SkillRelease_Belief.Text = "信仰类型";
            // 
            // EnumTypeComboBox_SkillRelease_Belief
            // 
            this.EnumTypeComboBox_SkillRelease_Belief.IsChangedValue = true;
            this.EnumTypeComboBox_SkillRelease_Belief.ListenControl = "TextBox_SkillRelease_Belief";
            this.EnumTypeComboBox_SkillRelease_Belief.Location = new System.Drawing.Point(362, 180);
            this.EnumTypeComboBox_SkillRelease_Belief.Name = "EnumTypeComboBox_SkillRelease_Belief";
            this.EnumTypeComboBox_SkillRelease_Belief.Size = new System.Drawing.Size(422, 32);
            this.EnumTypeComboBox_SkillRelease_Belief.TabIndex = 3;
            this.EnumTypeComboBox_SkillRelease_Belief.Tag = "";
            this.EnumTypeComboBox_SkillRelease_Belief.TextValue = null;
            this.EnumTypeComboBox_SkillRelease_Belief.TypeTag = "EnumSkillBelief";
            this.EnumTypeComboBox_SkillRelease_Belief.SelectedChanged += new System.EventHandler<SkillDataFileEditor.EnumTypeCBOSelectedChangedEventArgs>(this.EnumTypeComboBox_SelectedChanged);
            // 
            // TextBox_SkillRelease_Belief
            // 
            this.TextBox_SkillRelease_Belief.Location = new System.Drawing.Point(122, 180);
            this.TextBox_SkillRelease_Belief.Name = "TextBox_SkillRelease_Belief";
            this.TextBox_SkillRelease_Belief.ReadOnly = true;
            this.TextBox_SkillRelease_Belief.Size = new System.Drawing.Size(225, 25);
            this.TextBox_SkillRelease_Belief.TabIndex = 4;
            // 
            // Label_SkillRelease_Explan
            // 
            this.Label_SkillRelease_Explan.AutoSize = true;
            this.Label_SkillRelease_Explan.Location = new System.Drawing.Point(21, 394);
            this.Label_SkillRelease_Explan.Name = "Label_SkillRelease_Explan";
            this.Label_SkillRelease_Explan.Size = new System.Drawing.Size(67, 15);
            this.Label_SkillRelease_Explan.TabIndex = 5;
            this.Label_SkillRelease_Explan.Text = "效果说明";
            // 
            // TextBox_SkillRelease_Explan
            // 
            this.TextBox_SkillRelease_Explan.Location = new System.Drawing.Point(122, 391);
            this.TextBox_SkillRelease_Explan.Multiline = true;
            this.TextBox_SkillRelease_Explan.Name = "TextBox_SkillRelease_Explan";
            this.TextBox_SkillRelease_Explan.Size = new System.Drawing.Size(704, 191);
            this.TextBox_SkillRelease_Explan.TabIndex = 6;
            this.TextBox_SkillRelease_Explan.Tag = "skillExplan";
            // 
            // Label_SkillRelease_Effect
            // 
            this.Label_SkillRelease_Effect.AutoSize = true;
            this.Label_SkillRelease_Effect.Location = new System.Drawing.Point(21, 228);
            this.Label_SkillRelease_Effect.Name = "Label_SkillRelease_Effect";
            this.Label_SkillRelease_Effect.Size = new System.Drawing.Size(67, 15);
            this.Label_SkillRelease_Effect.TabIndex = 7;
            this.Label_SkillRelease_Effect.Text = "技能效果";
            // 
            // AutoArrayControl_SkillRelease_Effect
            // 
            this.AutoArrayControl_SkillRelease_Effect.ChildControlType = "SkillDataFileEditor.EnumTypeComboBox";
            this.AutoArrayControl_SkillRelease_Effect.Count = 5;
            this.AutoArrayControl_SkillRelease_Effect.IsChangedValue = false;
            this.AutoArrayControl_SkillRelease_Effect.Location = new System.Drawing.Point(122, 218);
            this.AutoArrayControl_SkillRelease_Effect.Name = "AutoArrayControl_SkillRelease_Effect";
            this.AutoArrayControl_SkillRelease_Effect.Size = new System.Drawing.Size(704, 167);
            this.AutoArrayControl_SkillRelease_Effect.TabIndex = 8;
            this.AutoArrayControl_SkillRelease_Effect.Tag = "skillStatusEffect";
            this.AutoArrayControl_SkillRelease_Effect.TextValues = new string[] {
        null,
        null,
        null,
        null,
        null};
            this.AutoArrayControl_SkillRelease_Effect.TypeTag = "EnumStatusEffect";
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
            this.TabPage_Other.ResumeLayout(false);
            this.Panel_Other.ResumeLayout(false);
            this.Panel_Other.PerformLayout();
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
        private System.Windows.Forms.Label Label_Base_SkillType;
        private EnumTypeComboBox EnumType_Base_SkillType;
        private System.Windows.Forms.TextBox TextBox_Base_SkillType;
        private System.Windows.Forms.TabPage TabPage_Attribute;
        private System.Windows.Forms.Label Label_Attribute_SkillMaxLevel;
        private System.Windows.Forms.Panel Panel_Attribute;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Attribute;
        private TypeTextBox TypeTextBox_Attribute_SkillMaxLevel;
        private System.Windows.Forms.Button Button_SetAttributePanel;
        private System.Windows.Forms.TabPage TabPage_Other;
        private System.Windows.Forms.Panel Panel_Other;
        private System.Windows.Forms.Button Button_Other_Add;
        private System.Windows.Forms.Button Button_Other_Delete;
        private System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel_Other;
        private System.Windows.Forms.ComboBox ComboBox_Other_Item;
        private EnumTypeComboBox EnumType_Release_Mode;
        private System.Windows.Forms.TextBox TextBox_Base_Mode;
        private System.Windows.Forms.Label Label_Base_Mode;
        private AutoArrayControl AutoArrayControl_SkillRelease_Particle;
        private System.Windows.Forms.Label Label_SkillRelease_Particle;
        private System.Windows.Forms.Label Label_SkillRelease_Belief;
        private EnumTypeComboBox EnumTypeComboBox_SkillRelease_Belief;
        private System.Windows.Forms.TextBox TextBox_SkillRelease_Belief;
        private System.Windows.Forms.Label Label_SkillRelease_Explan;
        private System.Windows.Forms.TextBox TextBox_SkillRelease_Explan;
        private AutoArrayControl AutoArrayControl_SkillRelease_Effect;
        private System.Windows.Forms.Label Label_SkillRelease_Effect;
    }
}

