using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            projectFilePath = "";
            skillAnalysisData = new SkillAnalysisData();
            Init();
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
        /// 初始化
        /// </summary>
        void Init()
        {
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
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "HP", Label = "HP", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 5 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "MP", Label = "MP", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 7 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "STR", Label = "STR", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 8 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "SE", Label = "SE", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 9 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "HP", Label = "HP", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 10 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "MP", Label = "MP", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 11 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "STR", Label = "STR", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 12 }), FlowLayoutPanel_Attribute);
            AddItemToPanel(GetAutoItemControl_SkillAttribute(new AutoItemControl.ItemStruct() { Tag = "SE", Label = "SE", ControlType = typeof(AutoArrayControl), TypeTag = "System.Int32", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = 13 }), FlowLayoutPanel_Attribute);
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
            opd.Filter = "技能编辑文件|*.SkillEditor";
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
            TextBox_Release_Mode.Text = "Magic_Bullet";
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
                    string value = File.ReadAllText(folderPath + "\\" + idToDatas[2], Encoding.UTF8);
                    values.Add(value);
                }
            }
            skillAnalysisData.AnalysisData(values.ToArray());

            Button_SaveProject.Enabled = true;
            Button_AddSkill.Enabled = false;
            Button_DeleteSkill.Enabled = false;
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
                    File.WriteAllText(folderPath + "\\" + tags[2], value, Encoding.UTF8);
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
                return changedCount > 0;
            }
            set
            {
                IChanged[] iChangeds = TabControl_Setting.TabPages.OfType<TabPage>().Select(temp => temp.Controls.OfType<Control>()).Combine().Select(temp => temp as IChanged).Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds)
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
            AutoItemControl_Release_Other.Tag = id;
            AutoItemControl_Release_Other.SkillAnalysisData = skillAnalysisData;
            AutoItemControl_Release_Other.Clear();

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
        }

        /// <summary>
        /// 保存当前选中树节点的数据
        /// </summary>
        private void SaveSelectNodeShowData()
        {
            if (selectNode == null || string.IsNullOrEmpty(projectFilePath))
                return;
            string id = selectNode.Name;
            AutoItemControl_Release_Other.Tag = id;
            AutoItemControl_Release_Other.SkillAnalysisData = skillAnalysisData;

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
        }

        /// <summary>
        /// 技能的释放方式发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_Release_Mode_TextChanged(object sender, EventArgs e)
        {
            AutoItemControl_Release_Other.Clear();
            EnumReleaseMode releaseMode = EnumReleaseMode.None;
            if (!string.IsNullOrEmpty(TextBox_Release_Mode.Text))
            {
                try
                {
                    releaseMode = (EnumReleaseMode)Enum.Parse(typeof(EnumReleaseMode), TextBox_Release_Mode.Text.Trim());
                }
                catch { }
            }
            //定义注册函数
            Func<EnumReleaseMode, AutoItemControl.ItemStruct> GetPowerRate = (type) => //添加注入魔法倍率
            {
                return new AutoItemControl.ItemStruct() { Label = "魔力注入倍率", ControlType = typeof(TypeTextBox), TypeTag = "System.Single", Tag = "powerRate" };
            };
            Func<EnumReleaseMode, AutoItemControl.ItemStruct> GetParticleName = (type) => //添加魔法的粒子
            {
                return new AutoItemControl.ItemStruct() { Label = "魔法粒子资源名", ControlType = typeof(TypeTextBox), TypeTag = "System.String", Tag = "particleName" };
            };
            Func<EnumReleaseMode, AutoItemControl.ItemStruct> GetParticleNames = (temp) => //添加多个魔法粒子
            {
                int count = temp == EnumReleaseMode.Magic_Bullet ? 2 : (temp == EnumReleaseMode.Magic_Pulse ? 2 : (temp == EnumReleaseMode.Magic_Vibrate ? 2 : 1));
                return new AutoItemControl.ItemStruct() { Label = "魔法粒子资源名列表", ControlType = typeof(AutoArrayControl), TypeTag = "System.String", Tag = "particleNames", IsArray = true, ChildControlType = "SkillDataFileEditor.TypeTextBox", ChildCount = count };
            };
            //定义添加函数
            Action<AutoItemControl, AutoItemControl.ItemStruct> AddItem = (control, data) =>
            {
                AutoItemControl.ItemStruct resultData = control.CreateItem();
                resultData.Label = data.Label;
                resultData.Tag = data.Tag;
                resultData.TypeTag = data.TypeTag;
                resultData.ControlType = data.ControlType;
                resultData.IsArray = data.IsArray;
                resultData.ChildControlType = data.ChildControlType;
                resultData.ChildCount = data.ChildCount;
            };
            //注册
            Dictionary<EnumReleaseMode, Func<EnumReleaseMode, AutoItemControl.ItemStruct>[]> releaseModeToControls = new Dictionary<EnumReleaseMode, Func<EnumReleaseMode, AutoItemControl.ItemStruct>[]>();
            releaseModeToControls.Add(EnumReleaseMode.None, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Bullet, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleNames });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Vibrate, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleNames });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Barrier, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleName });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Point, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleName });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Pulse, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleNames });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Buff, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleName });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Call, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { GetPowerRate, GetParticleName });
            releaseModeToControls.Add(EnumReleaseMode.Magic_Action, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { });
            releaseModeToControls.Add(EnumReleaseMode.Physics_Buff, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { });
            releaseModeToControls.Add(EnumReleaseMode.Physics_Attack, new Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] { });
            //调用
            Func<EnumReleaseMode, AutoItemControl.ItemStruct>[] result = releaseModeToControls[releaseMode];
            if (result != null)
            {
                foreach (Func<EnumReleaseMode, AutoItemControl.ItemStruct> item in result)
                {
                    AddItem(AutoItemControl_Release_Other, item(releaseMode));
                }
            }
            AutoItemControl_Release_Other.UpdateItems();
        }

        /// <summary>
        /// 技能属性的等级文本框发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeTextBox_Attribute_SkillMaxLevel_TypeTextChanged(object sender, TypeTextBoxEventArgs e)
        {

        }
    }
}
