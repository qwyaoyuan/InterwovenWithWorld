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
    public partial class SetAttributeForm : Form
    {
        public SetAttributeForm()
        {
            InitializeComponent();
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string directoryPath = Path.GetDirectoryName(exePath);
            attributeSettingPath = directoryPath + "\\SkillAttribute.Setting";
            LoadFile();
        }

        /// <summary>
        /// 属性设置文件路径
        /// </summary>
        string attributeSettingPath;
        /// <summary>
        /// 加载
        /// </summary>
        public void LoadFile()
        {
            List<AutoItemControl.ItemStruct> skillAttributeItemStructs = new List<AutoItemControl.ItemStruct>();
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

            foreach (AutoItemControl.ItemStruct skillAttributeItemStruct in skillAttributeItemStructs)
            {
                TabPage tabPage = new TabPage();
                TabControl_SkillAttribute.TabPages.Add(tabPage);
                SetAttributeControl setAttributeControl = new SetAttributeControl();
                tabPage.Controls.Add(setAttributeControl);
                setAttributeControl.Dock = DockStyle.Fill;
                setAttributeControl.TextBox_Label.TextChanged += (sender, e) => { tabPage.Text = "Name:" + setAttributeControl.TextBox_Label.Text; };
                setAttributeControl.TextBox_Label.Text = skillAttributeItemStruct.Label;
                setAttributeControl.TextBox_Tag.Text = skillAttributeItemStruct.Tag;
                setAttributeControl.TextBox_TypeTag.Text = skillAttributeItemStruct.TypeTag;
                setAttributeControl.TextBox_ControlType.Text = skillAttributeItemStruct.ControlType?.FullName;
                setAttributeControl.TextBox_IsArray.Text = skillAttributeItemStruct.IsArray.ToString();
                setAttributeControl.TextBox_ChildCount.Text = skillAttributeItemStruct.ChildCount.ToString();
                setAttributeControl.TextBox_ChildControlType.Text = skillAttributeItemStruct.ChildControlType;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Save_Click(object sender, EventArgs e)
        {
            if (!File.Exists(attributeSettingPath))
            {
                File.Create(attributeSettingPath).Close();
            }
            string valueStr = "";
            foreach (TabPage tabPage in TabControl_SkillAttribute.TabPages.OfType<TabPage>())
            {
                SetAttributeControl setAttributeControl = tabPage.Controls.OfType<SetAttributeControl>().FirstOrDefault();
                if (setAttributeControl == null)
                    continue;
                foreach (TextBox textBox in setAttributeControl.Controls.OfType<TextBox>())
                {
                    if (textBox.Tag != null && !string.IsNullOrEmpty(textBox.Tag.ToString().Trim()))
                    {
                        string itemName = textBox.Tag.ToString().Trim();
                        string itemValue = textBox.Text == null ? "" : textBox.Text;
                        valueStr += itemName + ":::" + itemValue + "^^^";
                    }
                }
                valueStr += "\r\n";
            }
            File.WriteAllText(attributeSettingPath, valueStr, Encoding.UTF8);
            MessageBox.Show("保存成功");
        }

        /// <summary>
        /// 标签页左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Left_Click(object sender, EventArgs e)
        {
            int selectIndex = TabControl_SkillAttribute.SelectedIndex;
            TabPage selectTabPage = TabControl_SkillAttribute.SelectedTab;
            if (selectIndex > 0)
            {
                TabControl_SkillAttribute.TabPages.Remove(selectTabPage);
                TabControl_SkillAttribute.TabPages.Insert(selectIndex - 1, selectTabPage);
                TabControl_SkillAttribute.SelectedIndex = selectIndex - 1;
            }
        }

        /// <summary>
        /// 标签页右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Right_Click(object sender, EventArgs e)
        {
            int selectIndex = TabControl_SkillAttribute.SelectedIndex;
            TabPage selectTabPage = TabControl_SkillAttribute.SelectedTab;
            if (selectIndex > -1 && selectIndex < TabControl_SkillAttribute.TabCount - 1)
            {
                TabControl_SkillAttribute.TabPages.Remove(selectTabPage);
                TabControl_SkillAttribute.TabPages.Insert(selectIndex + 1, selectTabPage);
                TabControl_SkillAttribute.SelectedIndex = selectIndex + 1;
            }

        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "Name:";
            TabControl_SkillAttribute.TabPages.Add(tabPage);
            SetAttributeControl setAttributeControl = new SetAttributeControl();
            tabPage.Controls.Add(setAttributeControl);
            setAttributeControl.Dock = DockStyle.Fill;
            setAttributeControl.TextBox_Label.TextChanged += (_sender, _e) => { tabPage.Text = "Name:" + setAttributeControl.TextBox_Label.Text; };
            TabControl_SkillAttribute.SelectedIndex = TabControl_SkillAttribute.TabCount - 1;
        }

        /// <summary>
        /// 移除一个标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Subtract_Click(object sender, EventArgs e)
        {
            int selectIndex = TabControl_SkillAttribute.SelectedIndex;
            if (selectIndex > -1)
            {
                DialogResult dr = MessageBox.Show("是否移除该项设置？", "警告！", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    TabControl_SkillAttribute.TabPages.RemoveAt(selectIndex);
                }
            }
        }
    }
}
