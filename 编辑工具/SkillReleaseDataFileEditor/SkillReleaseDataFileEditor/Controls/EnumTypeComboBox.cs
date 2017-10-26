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
    public partial class EnumTypeComboBox : UserControl, ITextValue, ITypeTag, IChanged
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
        /// 监听的控件
        /// </summary>
        private Control listenControl;
        /// <summary>
        /// 监听控件的字符串
        /// </summary>
        private string _listenControl;
        /// <summary>
        /// 监听的控件
        /// </summary>
        public string ListenControl
        {
            get
            {
                if (_listenControl != null)
                    return _listenControl;
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _listenControl = value;
                if (this.Parent == null)
                {
                    return;
                }
                //如果传入的值是空，则去除事件，同时赋值空
                if (string.IsNullOrEmpty(value))
                {
                    if (listenControl != null)
                    {
                        try
                        {
                            listenControl.TextChanged -= ListenControl_TextChanged;
                        }
                        catch { }
                        listenControl = null;
                    }
                }
                //如果传入的不为空
                else
                {

                    Control[] controlValues = this.Parent.Controls.Find(value, false);
                    //如果查找到了控件
                    if (controlValues.Length > 0)
                    {
                        //如果该控件已经是空则赋值
                        if (listenControl == null)
                        {
                            listenControl = controlValues[0];
                            listenControl.TextChanged += ListenControl_TextChanged;
                        }
                        else
                        {
                            try
                            {
                                listenControl.TextChanged -= ListenControl_TextChanged;
                            }
                            catch { }
                            listenControl = controlValues[0];
                            listenControl.TextChanged += ListenControl_TextChanged;
                        }
                    }
                    //如果没有查找到控件，则相当于赋值了空
                    else
                    {
                        if (listenControl != null)
                        {
                            try
                            {
                                listenControl.TextChanged -= ListenControl_TextChanged;
                            }
                            catch { }
                            listenControl = null;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 如果父物体发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnumTypeComboBox_ParentChanged(object sender, EventArgs e)
        {
            ListenControl = _listenControl;
        }
        /// <summary>
        /// 初始化监听控件
        /// </summary>
        public void InitListenControl()
        {
            ListenControl = _listenControl;
        }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string TextValue
        {
            get { return selectValue; }
            set { ComboBox_Main.Text = value; }
        }

        /// <summary>
        /// 是否改变了值
        /// </summary>
        private bool isChangedValue;
        /// <summary>
        /// 是否改变了值
        /// </summary>
        public bool IsChangedValue { get => isChangedValue; set => isChangedValue = value; }

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
        public event EventHandler<EnumTypeCBOSelectedChangedEventArgs> SelectedChanged;
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
                ListenControl = _listenControl;
                Label_Message.Text = messages[ComboBox_Main.SelectedIndex];
                selectValue = ComboBox_Main.Text;
                SelectedChanged?.Invoke(this, new EnumTypeCBOSelectedChangedEventArgs() { targetControl = this, selectText = selectValue });
                isChangedValue = true;
            }
            else Label_Message.Text = "";
        }

        /// <summary>
        /// 监听控件的文本发生变化的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListenControl_TextChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && !string.Equals(c.Text, ComboBox_Main.Text))
            {
                ComboBox_Main.Text = c.Text;
                if (!string.Equals(ComboBox_Main.Text, c.Text) && values != null && values.Length > 0)
                {
                    ComboBox_Main.Text = values[0];
                }
            }
        }


    }

    /// <summary>
    /// EnumTypeComboBox控件的消息
    /// </summary>
    public class EnumTypeCBOEventArgs : EventArgs
    {
        /// <summary>
        /// 消息来自的控件
        /// </summary>
        public EnumTypeComboBox targetControl;
    }

    /// <summary>
    /// EnumTypeComboBox控件选择项发生变化时触发
    /// </summary>
    public class EnumTypeCBOSelectedChangedEventArgs : EnumTypeCBOEventArgs
    {
        /// <summary>
        /// 选择的内容
        /// </summary>
        public string selectText;
    }
}
