using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace SkillDataFileEditor
{
    public partial class EnumTypeComboBox : UserControl
    {
        public EnumTypeComboBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 类型标记
        /// </summary>
        private string typeTag;
        /// <summary>
        /// 类型标记
        /// </summary>
        public string TypeTag
        {
            get
            {
                return typeTag;
            }
            set
            {
                typeTag = value;
                SetList();
            }
        }

        /// <summary>
        /// 值数组
        /// </summary>
        public string[] values;
        /// <summary>
        /// 描述数组
        /// </summary>
        public string[] messages;

        /// <summary>
        /// 选择内容发生变化是触发
        /// </summary>
        public event EventHandler SelectedChanged;
        /// <summary>
        /// 选择的项
        /// </summary>
        public string selectValue;

        /// <summary>
        /// 设置集合
        /// </summary>
        private void SetList()
        {
            ComboBox_Main.Items.Clear();
            values = null;
            messages = null;
            if (!string.IsNullOrEmpty(typeTag))
            {
                Type type = Type.GetType(typeTag, false);
                if (type != null && type.IsEnum)
                {
                    FieldInfo[] infos = type.GetFields().Where(temp => temp.FieldType.Equals(type)).ToArray();
                    values = new string[infos.Length];
                    messages = new string[infos.Length];
                    for (int i = 0; i < infos.Length; i++)
                    {
                        values[i] = infos[i].Name;
                        object[] attributes = infos[i].GetCustomAttributes(false).Where(temp => temp.GetType().Equals(typeof(FieldExplanAttribute))).ToArray();
                        if (attributes.Length > 0)
                        {
                            FieldExplanAttribute fieldExplanAttribute = (FieldExplanAttribute)attributes[0];
                            messages[i] = fieldExplanAttribute.GetExplan();
                        }
                        else messages[i] = "未找到说明!";
                    }
                }
            }
            if (values != null)
            {
                foreach (string value in values)
                {
                    ComboBox_Main.Items.Add(value);
                }
                if (values.Length > 0)
                    ComboBox_Main.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 下拉列表选择项发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_Main.SelectedIndex > -1 && messages != null && messages.Length > ComboBox_Main.SelectedIndex)
            {
                Label_Message.Text = messages[ComboBox_Main.SelectedIndex];
                selectValue = Label_Message.Text;
                SelectedChanged?.Invoke(this, new EventArgs());
            }
            else Label_Message.Text = "";
        }
    }
}
