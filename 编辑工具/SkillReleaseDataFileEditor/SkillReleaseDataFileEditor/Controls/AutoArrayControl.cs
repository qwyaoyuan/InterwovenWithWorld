using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    public partial class AutoArrayControl : UserControl, ITypeTag, IChildControlType, IChanged
    {
        public AutoArrayControl()
        {
            InitializeComponent();
        }

        //SkillDataFileEditor.EnumTypeComboBox

        /// <summary>
        /// 子控件类型
        /// </summary>
        private Type childControlType;
        /// <summary>
        /// 子控件类型
        /// </summary>
        public string ChildControlType
        {
            get
            {
                if (childControlType == null)
                    return "";
                else return childControlType.FullName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    childControlType = null;
                    FlowLayoutPanel_Main.Controls.Clear();
                }
                else
                {
                    Type t = Type.GetType(value, false);
                    if (t != null && t.IsSubclassOf(typeof(Control)) && typeof(ITextValue).IsAssignableFrom(t) && typeof(ITypeTag).IsAssignableFrom(t))
                    {
                        if (!Type.Equals(childControlType, value))
                        {
                            FlowLayoutPanel_Main.Controls.Clear();
                            childControlType = t;
                        }
                    }
                    else
                    {
                        childControlType = null;
                        FlowLayoutPanel_Main.Controls.Clear();
                    }
                }
                UpdateControl();
            }
        }



        /// <summary>
        /// 元素长度
        /// </summary>
        private int count;
        /// <summary>
        /// 元素长度
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                int temp = count;
                count = value;
                if (temp != count)
                    UpdateControl();
            }
        }

        /// <summary>
        /// 类型Tag
        /// </summary>
        private string typeTag;
        /// <summary>
        /// 类型Tag
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
                FlowLayoutPanel_Main.Controls.OfType<ITypeTag>().ToList().ForEach(temp => temp.TypeTag = TypeTag);
            }
        }

        /// <summary>
        /// 获取或设置数组内容
        /// </summary>
        public string[] TextValues
        {
            set
            {
                if (value == null || value.Length == 0)
                    Count = 0;
                else
                {
                    count = value.Length;
                    Control[] controls = FlowLayoutPanel_Main.Controls.OfType<Control>().ToArray();
                    int index = 0;
                    while (index < count && index < controls.Length)
                    {
                        ITextValue iTextValue = controls[index] as ITextValue;
                        if (iTextValue != null)
                        {
                            iTextValue.TextValue = value[index];
                        }
                        else
                        {
                            controls[index].Text = value[index];
                        }
                        index++;
                    }
                }
            }
            get
            {
                string[] values = new string[count];
                for (int i = 0; i < count; i++)
                {
                    if (i >= FlowLayoutPanel_Main.Controls.Count)
                        break;
                    ITextValue iTextValue = (ITextValue)FlowLayoutPanel_Main.Controls[i];
                    if (iTextValue != null) values[i] = iTextValue.TextValue;
                    else values[i] = FlowLayoutPanel_Main.Controls[i].Text;
                }
                return values;
            }
        }

        /// <summary>
        /// 是否改变了值
        /// </summary>
        private bool isChangedValue;
        /// <summary>
        /// 是否改变了值
        /// </summary>
        public bool IsChangedValue
        {
            get
            {
                int changedLength = FlowLayoutPanel_Main.Controls.OfType<Control>().Where(
                    temp =>
                    {
                        IChanged iChanged = temp as IChanged;
                        if (iChanged != null)
                            return iChanged.IsChangedValue;
                        return false;
                    }).Count();
                return isChangedValue || changedLength > 0;
            }
            set
            {
                isChangedValue = value;
                IChanged[] iChangeds = FlowLayoutPanel_Main.Controls.OfType<Control>().Select(temp => temp as IChanged).Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds)
                {
                    iChanged.IsChangedValue = value;
                }
            }
        }

        /// <summary>
        /// 更新控件
        /// </summary>
        private void UpdateControl()
        {
            if (count == 0 || childControlType == null)
            {
                if (this.Size.Width != 100 || this.Size.Height != 100)
                    this.Size = new Size(100, 100);
            }
            else
            {
                while (FlowLayoutPanel_Main.Controls.Count > count)
                {
                    FlowLayoutPanel_Main.Controls.RemoveAt(FlowLayoutPanel_Main.Controls.Count - 1);
                }
                while (FlowLayoutPanel_Main.Controls.Count < count)
                {
                    object createObj = Activator.CreateInstance(childControlType);
                    Control createControl = createObj as Control;
                    ITypeTag iTypeTag = createObj as ITypeTag;
                    if (createControl != null)
                    {
                        FlowLayoutPanel_Main.Controls.Add(createControl);
                    }
                    if (iTypeTag != null)
                    {
                        iTypeTag.TypeTag = typeTag;
                    }
                    else return;
                }
                int width = FlowLayoutPanel_Main.Size.Width - Margin.Left * 2;
                int height = (FlowLayoutPanel_Main.Size.Height - Margin.Top * count * 2) / count;
                for (int i = 0; i < count; i++)
                {
                    FlowLayoutPanel_Main.Controls[i].Size = new Size(width, height);
                }
            }
        }

        /// <summary>
        /// 面大大小发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowLayoutPanel_Main_SizeChanged(object sender, EventArgs e)
        {
            UpdateControl();
        }

    }
}
