using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    public partial class AddOtherAttributeForm : Form
    {
        public AddOtherAttributeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 要添加的属性
        /// </summary>
        public AutoItemControl.ItemStruct attributeItemStruct;

        /// <summary>
        /// 点击确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OK_Click(object sender, EventArgs e)
        {
            Action SaveAction = () =>
            {
                attributeItemStruct = new AutoItemControl.ItemStruct();
                attributeItemStruct.Label = SetAttributeControl_Main.TextBox_Label.Text;
                attributeItemStruct.Tag = SetAttributeControl_Main.TextBox_Tag.Text;
                attributeItemStruct.TypeTag = SetAttributeControl_Main.TextBox_TypeTag.Text;
                attributeItemStruct.ControlType = Type.GetType(SetAttributeControl_Main.TextBox_ControlType.Text, false);
                attributeItemStruct.IsArray = bool.Parse( SetAttributeControl_Main.TextBox_IsArray.Text);
                if (attributeItemStruct.IsArray)
                {
                    attributeItemStruct.ChildCount = int.Parse(SetAttributeControl_Main.TextBox_ChildCount.Text);
                    attributeItemStruct.ChildControlType = SetAttributeControl_Main.TextBox_ChildControlType.Text;
                }
                else
                {
                    attributeItemStruct.ChildCount = 0;
                    attributeItemStruct.ChildControlType = "";
                }
                DialogResult = DialogResult.OK;
            };
            if (string.IsNullOrEmpty(SetAttributeControl_Main.TextBox_Label.Text))
            {
                MessageBox.Show("Label不能为空");
                return;
            }
            if (string.IsNullOrEmpty(SetAttributeControl_Main.TextBox_Tag.Text))
            {
                MessageBox.Show("Tag不能为空");
                return;
            }
            if (!SkillDataFileEditorForm.CanUseThisTag(SetAttributeControl_Main.TextBox_Tag.Text))
            {
                MessageBox.Show("该Tag不可用");
                return;
            }

            bool isArray;
            if (bool.TryParse(SetAttributeControl_Main.TextBox_IsArray.Text, out isArray))
            {
                if (isArray)//数组
                {
                    if (string.Equals(SetAttributeControl_Main.TextBox_ControlType.Text?.Trim(), "SkillDataFileEditor.AutoArrayControl"))
                    {
                        int childCount;
                        if (!int.TryParse(SetAttributeControl_Main.TextBox_ChildCount.Text, out childCount))
                        {
                            MessageBox.Show("ChildCount必须是一个整数");
                        }
                        try
                        {
                            Type childControlType = Type.GetType(SetAttributeControl_Main.TextBox_ChildControlType.Text);
                            if (!childControlType.IsSubclassOf(typeof(Control)))
                            {
                                MessageBox.Show("ChildControlType必须是一个控件");
                            }
                            //开始保存
                            SaveAction();
                        }
                        catch
                        {
                            MessageBox.Show("ChildControlType必须是一个具体的类型");
                        }
                    }
                    else
                    {
                        MessageBox.Show("如果IsArray是True,则此处的类型必须是SkillDataFileEditor.AutoArrayControl");
                        return;
                    }
                }
                else//非数组
                {
                    Type typeTag = null;
                    try { typeTag = Type.GetType(SetAttributeControl_Main.TextBox_TypeTag.Text); } catch { }
                    if (typeTag == null)
                    {
                        MessageBox.Show("TypeTag必须是具体的类型");
                        return;
                    }
                    //开始保存
                    SaveAction();
                }
            }
            else
            {
                MessageBox.Show("IsArray必须是可以转换成Bool值");
                return;
            }
        }
    }
}
