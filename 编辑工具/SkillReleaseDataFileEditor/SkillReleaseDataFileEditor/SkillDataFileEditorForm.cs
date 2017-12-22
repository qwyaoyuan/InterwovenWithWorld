using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    public partial class SkillDataFileEditorForm : Form, IChanged
    {
        public SkillDataFileEditorForm()
        {
            InitializeComponent();
            otherTabPage = TabPage_Other;
            projectFilePath = "";
            skillAnalysisData = new SkillAnalysisData();
            InitSkillAttributePanel();
        }


        /// <summary>
        /// 工程文件路径
        /// 工程文件仅仅保存指定id的技能对应的文件名
        /// </summary>
        string projectFilePath;
        /// <summary>
        /// 技能数据解析对象
        /// </summary>
        SkillAnalysisData skillAnalysisData;
        /// <summary>
        /// 选择的树节点
        /// </summary>
        TreeNode selectNode;
        /// <summary>
        /// 属性面板所使用的Tag（字段名）
        /// </summary>
        static string[] attributeTags;
        /// <summary>
        /// 其他的设置面板
        /// </summary>
        static TabPage otherTabPage;
        /// <summary>
        /// 初始化技能属性面板
        /// </summary>
        void InitSkillAttributePanel()
        {
            List<AutoItemControl.ItemStruct> skillAttributeItemStructs = new List<AutoItemControl.ItemStruct>();
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string directoryPath = Path.GetDirectoryName(exePath);
            string attributeSettingPath = directoryPath + "\\SkillAttribute.Setting";
            if (File.Exists(attributeSettingPath))
            {
                using (StreamReader sr = new StreamReader(attributeSettingPath, Encoding.UTF8))
                {
                    string readLine = null;
                    string[] itemsSplit = new string[] { "^^^" };
                    string[] valueSplit = new string[] { ":::" };
                    while ((readLine = sr.ReadLine()) != null)
                    {
                        string[] Items = readLine.Split(itemsSplit, StringSplitOptions.RemoveEmptyEntries);
                        if (Items.Length == 7)
                        {
                            AutoItemControl.ItemStruct skillAttributeItemStruct = new AutoItemControl.ItemStruct();
                            foreach (string item in Items)
                            {
                                string[] values = item.Split(valueSplit, StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length != 2)
                                {
                                    string[] tempValues = new string[2];
                                    for (int i = 0; i < 2; i++)
                                    {
                                        tempValues[i] = "";
                                    }
                                    int index = 0;
                                    while (index < tempValues.Length && index < values.Length)
                                    {
                                        tempValues[index] = values[index];
                                        index++;
                                    }
                                }
                                switch (values[0].Trim())
                                {
                                    case "Label":
                                        skillAttributeItemStruct.Label = values[1].Trim();
                                        break;
                                    case "Tag":
                                        skillAttributeItemStruct.Tag = values[1].Trim();
                                        break;
                                    case "TypeTag":
                                        skillAttributeItemStruct.TypeTag = values[1].Trim();
                                        break;
                                    case "ControlType":
                                        try { skillAttributeItemStruct.ControlType = Type.GetType(values[1].Trim()); } catch { }
                                        break;
                                    case "IsArray":
                                        try { skillAttributeItemStruct.IsArray = bool.Parse(values[1].Trim()); } catch { }
                                        break;
                                    case "ChildControlType":
                                        skillAttributeItemStruct.ChildControlType = values[1].Trim();
                                        break;
                                    case "ChildCount":
                                        try { skillAttributeItemStruct.ChildCount = int.Parse(values[1].Trim()); } catch { }
                                        break;
                                }
                            }
                            skillAttributeItemStructs.Add(skillAttributeItemStruct);
                        }
                    }
                }
            }

            FlowLayoutPanel_Attribute.Controls.Clear();
            //在技能属性（根据技能等级设置属性）面板添加控件
            Func<AutoItemControl.ItemStruct, AutoItemControl> GetAutoItemControl_SkillAttribute = (_itemStruct) =>
            {
                AutoItemControl autoItemControl = new AutoItemControl();
                autoItemControl.SkillAnalysisData = skillAnalysisData;
                autoItemControl.Size = new Size(Panel_Attribute.Size.Width / 2 - 25, Panel_Attribute.Height / 2);
                autoItemControl.Margin = new Padding(3, 3, 3, 25);
                AutoItemControl.ItemStruct itemStruct = autoItemControl.CreateItem();
                itemStruct.Tag = _itemStruct.Tag;
                itemStruct.Label = _itemStruct.Label;
                itemStruct.TypeTag = _itemStruct.TypeTag;
                itemStruct.ControlType = _itemStruct.ControlType;
                itemStruct.IsArray = _itemStruct.IsArray;
                itemStruct.ChildControlType = _itemStruct.ChildControlType;
                itemStruct.ChildCount = _itemStruct.ChildCount;
                return autoItemControl;
            };

            Action<AutoItemControl, FlowLayoutPanel> AddItemToPanel = (autoItemControl, flowLayoutPanel) =>
            {
                flowLayoutPanel.Controls.Add(autoItemControl);
                autoItemControl.UpdateItems();
            };

            foreach (AutoItemControl.ItemStruct itemStruct in skillAttributeItemStructs)
            {
                AddItemToPanel(GetAutoItemControl_SkillAttribute(itemStruct), FlowLayoutPanel_Attribute);
            }
            TreeView_Skills.SelectedNode = selectNode = null;
            attributeTags = skillAttributeItemStructs.Select(temp => temp.Tag).ToArray();
        }

        /// <summary>
        /// 是否可以使用这个Tag
        /// </summary>
        /// <param name="tag">目标Tag</param>
        /// <returns></returns>
        public static bool CanUseThisTag(string tag)
        {
            tag = tag?.Trim();
            switch (tag)
            {
                case "OtherFieldSet"://该字段用于保存Other面板中设置的属性
                case "skillID":
                case "skillName":
                case "skillType":
                case "releaseType":
                case "releaseMode":
                case "combinSkillTypes":
                case "powerRate":
                case "particleName":
                case "skillLevel":
                    return false;
            }
            if (attributeTags.Contains(tag))
                return false;
            try
            {
                string[] ids = otherTabPage.Controls.OfType<Panel>().First().Controls.OfType<FlowLayoutPanel>().First().Controls.OfType<AutoItemControl>().Select(temp => temp.Items[0].Tag).ToArray();
                if (ids.Contains(tag))
                    return false;
            }
            catch { }
            return true;
        }

        /// <summary>
        /// 新建一个项目文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_NewProject_Click(object sender, EventArgs e)
        {
            NewProjectForm newProjectForm = new NewProjectForm();
            DialogResult dr = newProjectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                projectFilePath = newProjectForm.newFilePath;
                OpenProject();
            }
        }

        /// <summary>
        /// 打开一个项目文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_LoadProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.InitialDirectory = NewProjectForm.lastPath;
            opd.Filter = "技能编辑文件|*.SkillEditor|技能编辑文件(文本)|*.txt";
            DialogResult dr = opd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                projectFilePath = opd.FileName;
                OpenProject();
            }
        }

        /// <summary>
        /// 清理数据与控件
        /// </summary>
        private void ClearData()
        {
            selectNode = null;
            TreeView_Skills.Nodes.Clear();
            skillAnalysisData.Clear();
            TextBox_Base_Mode.Text = "Magic_Bullet";
        }

        /// <summary>
        /// 打开工程
        /// </summary>
        private void OpenProject()
        {
            ClearData();
            string[] splits = new string[] { "^^^" };
            using (StreamReader fs = new StreamReader(projectFilePath, Encoding.UTF8))
            {
                string readLine = null;
                while ((readLine = fs.ReadLine()) != null)
                {
                    readLine = readLine.Trim();
                    if (string.IsNullOrEmpty(readLine))
                        continue;
                    // id 技能名 文件名
                    string[] idToDatas = readLine.Split(splits, StringSplitOptions.RemoveEmptyEntries);
                    if (idToDatas.Length != 3)
                        continue;
                    TreeNode treeNode = new TreeNode(idToDatas[1]);
                    treeNode.Name = idToDatas[0];
                    treeNode.Tag = idToDatas;
                    TreeView_Skills.Nodes.Add(treeNode);
                }
            }
            List<string> values = new List<string>();
            string folderPath = Path.GetDirectoryName(projectFilePath);
            foreach (TreeNode treeNode in TreeView_Skills.Nodes)
            {
                string[] idToDatas = treeNode.Tag as string[];
                if (idToDatas != null)
                {
                    string value = File.ReadAllText(folderPath + "\\" + idToDatas[2] + ".txt", Encoding.UTF8);
                    values.Add(value);
                }
            }
            skillAnalysisData.AnalysisData(values.ToArray());

            foreach (TreeNode treeNode in TreeView_Skills.Nodes)
            {
                string[] idToDatas = treeNode.Tag as string[];
                treeNode.Text = idToDatas[0] + "~" + idToDatas[1] + "~" + skillAnalysisData.GetValue<string>(idToDatas[0], "releaseMode");
            }

            Button_SaveProject.Enabled = true;
            Button_AddSkill.Enabled = true;
            Button_DeleteSkill.Enabled = true;
            Button_SaveSkillToMemory.Enabled = true;
            IsChangedValue = false;
        }

        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddSkill_Click(object sender, EventArgs e)
        {
            AddSkillForm addSkillForm = new AddSkillForm();
            DialogResult dr = addSkillForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string skillID = addSkillForm.skillID;
                string skillName = addSkillForm.skillName;
                string skillFileName = Tools.GetRandomString(30, true, true, true, false, "");
                string folderPath = Path.GetDirectoryName(projectFilePath);
                TreeNode[] checkNodesByID = TreeView_Skills.Nodes.Find(skillID, false);
                int selectCount = TreeView_Skills.Nodes.OfType<TreeNode>().Where(
                    temp =>
                    {
                        string[] tags = (string[])temp.Tag;
                        return string.Equals(tags[2], skillFileName);
                    }).Count();
                if (checkNodesByID.Length > 0 || selectCount > 0)
                {
                    return;
                }
                skillAnalysisData.AddID(skillID);
                skillAnalysisData.SetValue(skillID, "skillID", skillID);
                skillAnalysisData.SetValue(skillID, "skillName", skillName);
                TreeNode treeNode = new TreeNode(skillName);
                treeNode.Name = skillID;
                treeNode.Tag = new string[] { skillID, skillName, skillFileName };
                TreeView_Skills.Nodes.Add(treeNode);
                TreeView_Skills.SelectedNode = treeNode;
            }
        }

        /// <summary>
        /// 删除技能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DeleteSkill_Click(object sender, EventArgs e)
        {
            TreeNode selectNode = this.selectNode;
            this.selectNode = null;
            if (selectNode != null)
            {
                DialogResult dr = MessageBox.Show("是否删除选中技能?", "警告！", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    TreeView_Skills.Nodes.Remove(selectNode);
                    skillAnalysisData.RemoveID(selectNode.Name);
                    string folderPath = Path.GetDirectoryName(projectFilePath);
                    string fileName = ((string[])selectNode.Tag)[2];
                    if (File.Exists(folderPath + "\\" + fileName))
                        File.Delete(folderPath + "\\" + fileName);
                }
            }
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SaveProject_Click(object sender, EventArgs e)
        {
            if (File.Exists(projectFilePath))
            {
                string projectValue = "";
                string[] splits = new string[] { "^^^" };
                string folderPath = Path.GetDirectoryName(projectFilePath);
                foreach (TreeNode treeNode in TreeView_Skills.Nodes)
                {
                    string[] tags = (string[])treeNode.Tag;
                    foreach (string tag in tags)
                    {
                        projectValue += tag + splits[0];
                    }
                    projectValue += "\r\n";
                    //每个文件的数据
                    string value = skillAnalysisData.Disanalysis(tags[0]);
                    File.WriteAllText(folderPath + "\\" + tags[2] + ".txt", value, Encoding.UTF8);
                }
                File.WriteAllText(projectFilePath, projectValue, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 保存当前修改到内存中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SaveSkillToMemory_Click(object sender, EventArgs e)
        {
            //保存当前数据
            SaveSelectNodeShowData();
            IsChangedValue = false;
        }

        #region 数据控件的事件与函数

        /// <summary>
        /// 查找指定的控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="root">根控件</param>
        /// <param name="name">查找的控件名</param>
        /// <returns></returns>
        private T FindControl<T>(Control root, string name) where T : Control
        {
            try
            {
                Control[] controls = root.Controls.Find(name, true).Where(temp => temp.GetType().Equals(typeof(T))).ToArray();
                if (controls.Length > 0)
                    return (T)controls[0];
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 枚举下拉列表选择项发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnumTypeComboBox_SelectedChanged(object sender, EnumTypeCBOSelectedChangedEventArgs e)
        {
            TextBox textBox = FindControl<TextBox>(TabControl_Setting, (string)e.targetControl.ListenControl);
            if (textBox != null)
            {
                textBox.Text = e.selectText;
            }
        }

        #endregion

        /// <summary>
        /// 是否改变了值
        /// </summary>
        public bool IsChangedValue
        {
            get
            {
                int changedCount = TabControl_Setting.TabPages.OfType<TabPage>().Select(temp => temp.Controls.OfType<Control>()).Combine().Select(temp => temp as IChanged).
                    Where(temp =>
                    {
                        if (temp == null)
                            return false;
                        return temp.IsChangedValue;
                    }).Count();
                int changedCount_Attribute = FlowLayoutPanel_Attribute.Controls.OfType<AutoItemControl>().Where(temp => temp.IsChangedValue).Count();
                int changedCount_Other = FlowLayoutPanel_Other.Controls.OfType<AutoItemControl>().Where(temp => temp.IsChangedValue).Count();
                return changedCount > 0 || changedCount_Attribute > 0 || changedCount_Other > 0;
            }
            set
            {
                IChanged[] iChangeds = TabControl_Setting.TabPages.OfType<TabPage>().Select(temp => temp.Controls.OfType<Control>()).Combine().Select(temp => temp as IChanged).Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds)
                {
                    iChanged.IsChangedValue = value;
                }
                IChanged[] iChangeds_Attribute = FlowLayoutPanel_Attribute.Controls.OfType<AutoItemControl>().Select(temp => temp as IChanged).Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds_Attribute)
                {
                    iChanged.IsChangedValue = value;
                }
                IChanged[] iChangeds_Other = FlowLayoutPanel_Other.Controls.OfType<AutoItemControl>().Select(temp => temp as IChanged).Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds_Other)
                {
                    iChanged.IsChangedValue = value;
                }
            }
        }

        /// <summary>
        /// 树列表选择项发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_Skills_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsChangedValue)
            {
                DialogResult dr = MessageBox.Show("是否保存上一个技能的数据？如不保存将会导致数据丢失", "警告！", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    //保存当前数据
                    SaveSelectNodeShowData();
                }
            }
            selectNode = e.Node;
            //设置新节点数据
            SetSelectNodeShowData();
            IsChangedValue = false;
        }

        /// <summary>
        /// 设置当前选中树节点应该显示的数据
        /// </summary>
        private void SetSelectNodeShowData()
        {
            if (selectNode == null || string.IsNullOrEmpty(projectFilePath))
                return;
            string id = selectNode.Name;
            FlowLayoutPanel_Other.Controls.Clear();

            IEnumerable<TabPage> allTabPage = TabControl_Setting.TabPages.OfType<TabPage>();
            EnumTypeComboBox[] enumTypeComboBoxs = allTabPage.Select(temp => temp.Controls.OfType<EnumTypeComboBox>()).Combine().ToArray();
            foreach (EnumTypeComboBox enumTypeComboBox in enumTypeComboBoxs)
            {
                enumTypeComboBox.InitListenControl();
                string fieldName = (string)enumTypeComboBox.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                    enumTypeComboBox.TextValue = skillAnalysisData.GetValue<string>(id, fieldName);
            }
            TextBox[] textBoxs = allTabPage.Select(temp => temp.Controls.OfType<TextBox>()).Combine().ToArray();
            foreach (TextBox textBox in textBoxs)
            {
                string fieldName = (string)textBox.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    textBox.Text = skillAnalysisData.GetValue<string>(id, fieldName);
                }
            }
            AutoArrayControl[] autoArrayControls = allTabPage.Select(temp => temp.Controls.OfType<AutoArrayControl>()).Combine().ToArray();
            foreach (AutoArrayControl autoArrayControl in autoArrayControls)
            {
                string fieldName = (string)autoArrayControl.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    string[] getValues = skillAnalysisData.GetValues<string>(id, fieldName);
                    string[] nowValues = autoArrayControl.TextValues;
                    int index = 0;
                    while (index < getValues.Length && index < nowValues.Length)
                    {
                        nowValues[index] = getValues[index];
                        index++;
                    }
                    autoArrayControl.TextValues = nowValues;
                }
            }
            TypeTextBox[] typeTextBoxs = allTabPage.Select(temp => temp.Controls.OfType<TypeTextBox>()).Combine().ToArray();
            foreach (TypeTextBox typeTextBox in typeTextBoxs)
            {
                string fieldName = (string)typeTextBox.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    typeTextBox.TextValue = skillAnalysisData.GetValue<string>(id, fieldName);
                }
            }
            AutoItemControl[] autoItemControls = allTabPage.Select(temp => temp.Controls.OfType<AutoItemControl>()).Combine().ToArray();
            foreach (AutoItemControl autoItemControl in autoItemControls)
            {
                autoItemControl.UpdateItems();
            }
            //处理技能属性（根据等级附加属性）
            AutoItemControl[] attributeSkillLevelControls = FlowLayoutPanel_Attribute.Controls.OfType<AutoItemControl>().ToArray();
            foreach (AutoItemControl attributeSkillLevelControl in attributeSkillLevelControls)
            {
                attributeSkillLevelControl.SkillAnalysisData = skillAnalysisData;
                attributeSkillLevelControl.Tag = id;
                attributeSkillLevelControl.UpdateItems(true);
            }
            //处理其他属性
            string[] oldOtherSettings = skillAnalysisData.GetValues<string>(id, "OtherFieldSet");
            string[] otherSplit = new string[] { "***" };
            ComboBox_Other_Item.Items.Clear();
            Func<string, string> JCFZFunc = (target) => //解除封装
            {
                if (target.Length < 2)
                    return "";
                target = target.Remove(0, 1);
                target = target.Remove(target.Length - 1, 1);
                return target;
            };
            foreach (string oldOtherSetting in oldOtherSettings)
            {
                string[] otherFieldSets = oldOtherSetting.Split(otherSplit, StringSplitOptions.RemoveEmptyEntries);
                if (otherFieldSets.Length == 7)
                {
                    AutoItemControl autoItemContorl = new AutoItemControl();
                    autoItemContorl.SkillAnalysisData = skillAnalysisData;
                    autoItemContorl.Tag = id;
                    AutoItemControl.ItemStruct itemStruct = autoItemContorl.CreateItem();
                    itemStruct.Label = JCFZFunc(otherFieldSets[0]);
                    itemStruct.Tag = JCFZFunc(otherFieldSets[1]);
                    Type controlType = null;
                    try { controlType = Type.GetType(JCFZFunc(otherFieldSets[2])); } catch { }
                    itemStruct.ControlType = controlType;
                    itemStruct.TypeTag = JCFZFunc(otherFieldSets[3]);
                    bool isArray = false;
                    try { isArray = bool.Parse(JCFZFunc(otherFieldSets[4])); } catch { }
                    itemStruct.IsArray = isArray;
                    int childCount = 0;
                    try { childCount = int.Parse(JCFZFunc(otherFieldSets[5])); } catch { }
                    itemStruct.ChildCount = childCount;
                    itemStruct.ChildControlType = JCFZFunc(otherFieldSets[6]);

                    autoItemContorl.Size = new Size(Panel_Other.Size.Width / 2 - 7, Panel_Other.Size.Height / 3);
                    FlowLayoutPanel_Other.Controls.Add(autoItemContorl);
                    autoItemContorl.UpdateItems(true);
                    autoItemContorl.IsChangedValue = false;

                    ComboBox_Other_Item.Items.Add(itemStruct.Tag);
                }
            }
        }

        /// <summary>
        /// 保存当前选中树节点的数据
        /// </summary>
        private void SaveSelectNodeShowData()
        {
            if (selectNode == null || string.IsNullOrEmpty(projectFilePath))
                return;
            string[] idToDatas = selectNode.Tag as string[];
            string id = idToDatas[0];

            IEnumerable<TabPage> allTabPage = TabControl_Setting.TabPages.OfType<TabPage>();
            TextBox[] textBoxs = allTabPage.Select(temp => temp.Controls.OfType<TextBox>()).Combine().ToArray();
            foreach (TextBox textBox in textBoxs)
            {
                string fieldName = (string)textBox.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    skillAnalysisData.SetValue(id, fieldName, textBox.Text);
                }
            }
            AutoArrayControl[] autoArrayControls = allTabPage.Select(temp => temp.Controls.OfType<AutoArrayControl>()).Combine().ToArray();
            foreach (AutoArrayControl autoArrayControl in autoArrayControls)
            {
                string fieldName = (string)autoArrayControl.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    skillAnalysisData.SetValues<string>(id, fieldName, autoArrayControl.TextValues);
                }
            }
            TypeTextBox[] typeTextBoxs = allTabPage.Select(temp => temp.Controls.OfType<TypeTextBox>()).Combine().ToArray();
            foreach (TypeTextBox typeTextBox in typeTextBoxs)
            {
                string fieldName = (string)typeTextBox.Tag;
                if (!string.IsNullOrEmpty(fieldName))
                {
                    skillAnalysisData.SetValue<string>(id, fieldName, typeTextBox.TextValue);
                }
            }
            AutoItemControl[] autoItemControls = allTabPage.Select(temp => temp.Controls.OfType<AutoItemControl>()).Combine().ToArray();
            foreach (AutoItemControl autoItemControl in autoItemControls)
            {
                autoItemControl.SaveData();
            }
            //处理技能属性（根据等级附加属性）
            AutoItemControl[] attributeSkillLevelControls = FlowLayoutPanel_Attribute.Controls.OfType<AutoItemControl>().ToArray();
            foreach (AutoItemControl attributeSkillLevelControl in attributeSkillLevelControls)
            {
                attributeSkillLevelControl.SkillAnalysisData = skillAnalysisData;
                attributeSkillLevelControl.Tag = id;
                attributeSkillLevelControl.SaveData();
            }
            //处理其他属性
            AutoItemControl[] attributeOtherControls = FlowLayoutPanel_Other.Controls.OfType<AutoItemControl>().ToArray();
            foreach (AutoItemControl attributeOtherControl in attributeOtherControls)
            {
                attributeOtherControl.SkillAnalysisData = skillAnalysisData;
                attributeOtherControl.Tag = id;
                attributeOtherControl.SaveData();
            }
            selectNode.Text = idToDatas[0] + "~" + idToDatas[1] + "~" + skillAnalysisData.GetValue<string>(idToDatas[0], "releaseMode");
        }

        /// <summary>
        /// 技能属性的等级文本框发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeTextBox_Attribute_SkillMaxLevel_TypeTextChanged(object sender, TypeTextBoxEventArgs e)
        {
            int count = 0;
            int.TryParse(e.text, out count);
            AutoItemControl[] attributeSkillLevelControls = FlowLayoutPanel_Attribute.Controls.OfType<AutoItemControl>().ToArray();
            foreach (AutoItemControl attributeSkillLevelControl in attributeSkillLevelControls)
            {
                if (attributeSkillLevelControl.Count > 0)
                {
                    attributeSkillLevelControl[0].ChildCount = count;
                    attributeSkillLevelControl.UpdateItems();
                }
            }
        }

        /// <summary>
        /// 配置技能属性面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SetAttributePanel_Click(object sender, EventArgs e)
        {
            SetAttributeForm setAttributeForm = new SetAttributeForm();
            setAttributeForm.ShowDialog();
            InitSkillAttributePanel();
        }

        /// <summary>
        /// 添加一个Item到Other选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Other_Add_Click(object sender, EventArgs e)
        {
            if (selectNode == null || string.IsNullOrEmpty(projectFilePath))
                return;

            AddOtherAttributeForm addOtherAttributeForm = new AddOtherAttributeForm();
            DialogResult dr = addOtherAttributeForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                AutoItemControl autoItemContorl = new AutoItemControl();
                autoItemContorl.SkillAnalysisData = skillAnalysisData;
                TreeNode selectNode = TreeView_Skills.SelectedNode;
                if (selectNode != null)
                {
                    try
                    {
                        string[] values = selectNode.Tag as string[];
                        autoItemContorl.Tag = values[0];
                    }
                    catch { }
                }
                AutoItemControl.ItemStruct itemStruct = autoItemContorl.CreateItem();
                itemStruct.Label = addOtherAttributeForm.attributeItemStruct.Label;
                itemStruct.Tag = addOtherAttributeForm.attributeItemStruct.Tag;
                itemStruct.ControlType = addOtherAttributeForm.attributeItemStruct.ControlType;
                itemStruct.TypeTag = addOtherAttributeForm.attributeItemStruct.TypeTag;
                itemStruct.IsArray = addOtherAttributeForm.attributeItemStruct.IsArray;
                itemStruct.ChildCount = addOtherAttributeForm.attributeItemStruct.ChildCount;
                itemStruct.ChildControlType = addOtherAttributeForm.attributeItemStruct.ChildControlType;
                autoItemContorl.Size = new Size(Panel_Other.Size.Width / 2 - 7, Panel_Other.Size.Height / 3);
                FlowLayoutPanel_Other.Controls.Add(autoItemContorl);
                autoItemContorl.UpdateItems();
                autoItemContorl.IsChangedValue = true;
                if (skillAnalysisData != null && selectNode != null)//将本次设置的内容保存到OtherFieldSet字段中
                {
                    string[] tagValues = selectNode.Tag as string[];
                    if (tagValues != null && tagValues.Length > 0)
                    {
                        string[] oldOtherSettings = skillAnalysisData.GetValues<string>(tagValues[0], "OtherFieldSet");
                        if (oldOtherSettings == null)
                            oldOtherSettings = new string[0];
                        string[] newOtherSettings = new string[oldOtherSettings.Length + 1];
                        Array.Copy(oldOtherSettings, newOtherSettings, oldOtherSettings.Length);
                        string split = "***";
                        Func<object, string> FZFunc = (target) =>//封装[]
                        {
                            if (target == null)
                                return "[]";
                            if (object.Equals(target.GetType(), typeof(Type)))
                            {
                                Type t = target as Type;
                                return "[" + t.FullName + "]";
                            }
                            return "[" + target.ToString() + "]";
                        };
                        string otherFieldSet =
                            FZFunc(itemStruct.Label) + split +
                            FZFunc(itemStruct.Tag) + split +
                            FZFunc(itemStruct.ControlType) + split +
                            FZFunc(itemStruct.TypeTag) + split +
                            FZFunc(itemStruct.IsArray) + split +
                            FZFunc(itemStruct.ChildCount) + split +
                            FZFunc(itemStruct.ChildControlType);
                        newOtherSettings[newOtherSettings.Length - 1] = otherFieldSet;
                        skillAnalysisData.SetValues<string>(tagValues[0], "OtherFieldSet", newOtherSettings);
                    }
                }
                //添加到下拉列表中
                ComboBox_Other_Item.Items.Add(itemStruct.Tag);
            }
        }

        /// <summary>
        /// 从Other选项卡中移除一个Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Other_Delete_Click(object sender, EventArgs e)
        {
            if (selectNode == null || string.IsNullOrEmpty(projectFilePath))
                return;

            int selectIndex = ComboBox_Other_Item.SelectedIndex;
            if (selectIndex < 0)
                return;
            //选择的关联对象
            string selectTag = ComboBox_Other_Item.Items[selectIndex].ToString();
            try
            {
                AutoItemControl autoItemControl = FlowLayoutPanel_Other.Controls.OfType<AutoItemControl>().
                    Select(temp => new { control = temp, tag = temp.Items[0].Tag }).
                    Where(temp => string.Equals(temp.tag, selectTag)).
                    Select(temp => temp.control).FirstOrDefault();
                if (autoItemControl != null)
                {
                    FlowLayoutPanel_Other.Controls.Remove(autoItemControl);
                    ComboBox_Other_Item.Items.RemoveAt(selectIndex);
                    if (selectNode != null)
                    {
                        string[] selectNodeValues = selectNode.Tag as string[];
                        if (selectNodeValues != null && selectNodeValues.Length > 0)
                            skillAnalysisData.RemoveValue(selectNodeValues[0], selectTag);
                        if (skillAnalysisData != null && selectNode != null)//将本次删除更改后的内容保存到OtherFieldSet字段中
                        {
                            string[] tagValues = selectNode.Tag as string[];
                            if (tagValues != null && tagValues.Length > 0)
                            {
                                string[] oldOtherSettings = skillAnalysisData.GetValues<string>(tagValues[0], "OtherFieldSet");
                                if (oldOtherSettings == null)
                                    oldOtherSettings = new string[0];
                                List<string> newOtherSettings = new List<string>();
                                string[] split = new string[] { "***" };
                                Func<string, string> JCFZFunc = (target) => //解除封装
                                {
                                    if (target.Length < 2)
                                        return "";
                                    target = target.Remove(0, 1);
                                    target = target.Remove(target.Length - 1, 1);
                                    return target;
                                };
                                foreach (string oldOtherSetting in oldOtherSettings)
                                {
                                    string[] otherFieldSets = oldOtherSetting.Split(split, StringSplitOptions.RemoveEmptyEntries);
                                    if (otherFieldSets.Length == 7)
                                    {
                                        string thisID = JCFZFunc(otherFieldSets[1]);
                                        if (!string.Equals(thisID, selectTag))
                                        {
                                            newOtherSettings.Add(oldOtherSetting);
                                        }
                                    }
                                }
                                skillAnalysisData.SetValues<string>(tagValues[0], "OtherFieldSet", newOtherSettings.ToArray());
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("移除失败");
            }
        }

        private void SkillDataFileEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("请保证保存了修改内容再关闭此窗体", "关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);//触发事件进行提示
            if (dr == DialogResult.No)
            {
                e.Cancel = true;//不退
            }
            else
            {
                e.Cancel = false;//退
            }
        }
    }
}
