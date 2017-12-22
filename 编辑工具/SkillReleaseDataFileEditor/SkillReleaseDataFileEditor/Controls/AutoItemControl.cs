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
    public partial class AutoItemControl : UserControl, IChanged
    {
        public AutoItemControl()
        {
            InitializeComponent();
            items = new List<ItemStruct>();
        }

        List<ItemStruct> items;
        /// <summary>
        /// 内部的控件
        /// </summary>
        public ItemStruct[] Items
        {
            get { return items.ToArray(); }
        }
        /// <summary>
        /// 控件的个数
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }
        /// <summary>
        /// 获取指定下标控件的对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ItemStruct this[int index]
        {
            get
            {
                if (index < Count)
                    return items[index];
                else return null;
            }
        }
        /// <summary>
        /// 技能数据对象
        /// </summary>
        private SkillAnalysisData skillAnalysisData;
        /// <summary>
        /// 设置技能数据对象
        /// </summary>
        public SkillAnalysisData SkillAnalysisData
        {
            set
            {
                skillAnalysisData = value;
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
                int changedLength = FlowLayoutPanel_Release_Other.Controls.OfType<Panel>()
                    .Select(temp => temp.Controls.Find("ItemControl", false).FirstOrDefault())
                    .Where(
                    temp =>
                    {
                        if (temp == null)
                            return false;
                        IChanged iChanged = temp as IChanged;
                        if (iChanged == null)
                            return false;
                        return iChanged.IsChangedValue;
                    }).Count();
                return isChangedValue || changedLength > 0;
            }
            set
            {
                isChangedValue = value;
                IChanged[] iChangeds = FlowLayoutPanel_Release_Other.Controls.OfType<Panel>()
                    .Select(temp => temp.Controls.Find("ItemControl", false).FirstOrDefault())
                    .Where(temp => temp != null)
                    .Select(temp => temp as IChanged)
                    .Where(temp => temp != null).ToArray();
                foreach (IChanged iChanged in iChangeds)
                {
                    iChanged.IsChangedValue = value;
                }
            }
        }

        /// <summary>
        /// 创建一个对象
        /// 注意只能使用这种方法创建
        /// </summary>
        /// <returns></returns>
        public ItemStruct CreateItem()
        {
            ItemStruct create = new ItemStruct();
            create.ChangedHandle += Create_ChangedHandle;
            Panel ItemPanel = new Panel();
            ItemPanel.Size = new Size(1, 1);
            ItemPanel.Tag = create;
            FlowLayoutPanel_Release_Other.Controls.Add(ItemPanel);
            items.Add(create);
            return create;
        }

        /// <summary>
        /// 移除一个指定下标的对象
        /// </summary>
        /// <param name="index">对象的下标</param>
        /// <returns></returns>
        public bool RemoveItem(int index)
        {
            if (index < Count && index >= 0)
            {
                Panel[] panels = FlowLayoutPanel_Release_Other.Controls.OfType<Panel>().Where(temp => object.Equals(temp.Tag, items[index])).ToArray();
                if (panels.Length > 0)
                {
                    items.RemoveAt(index);
                    FlowLayoutPanel_Release_Other.Controls.Remove(panels[0]);
                    UpdateItems();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// 移除一个指定的对象
        /// </summary>
        /// <param name="item">对象</param>
        /// <returns></returns>
        public bool RemoveItem(ItemStruct item)
        {
            int index = items.IndexOf(item);
            return RemoveItem(index);
        }

        /// <summary>
        /// 清理控件
        /// </summary>
        public void Clear()
        {
            items.Clear();
            FlowLayoutPanel_Release_Other.Controls.Clear();
        }

        /// <summary>
        /// 保存函数
        /// </summary>
        public void SaveData()
        {
            if (this.Tag == null || skillAnalysisData == null)
                return;
            string id = (string)this.Tag;
            Panel[] panels = FlowLayoutPanel_Release_Other.Controls.OfType<Panel>().ToArray();
            foreach (Panel panel in panels)
            {
                ItemStruct itemStruct = panel.Tag as ItemStruct;
                if (itemStruct == null || string.IsNullOrEmpty(itemStruct.Tag))
                    continue;
                //判断条件
                Control control = panel.Controls.Find("ItemControl", false).FirstOrDefault();
                if (control != null)
                {
                    //是否是数组
                    if (itemStruct.IsArray)
                    {
                        IChildControlType iChildControlType = control as IChildControlType;
                        if (iChildControlType != null)
                        {
                            skillAnalysisData.SetValues(id, itemStruct.Tag, iChildControlType.TextValues);
                        }
                    }
                    else
                    {
                        ITextValue iTextValue = control as ITextValue;
                        if (iTextValue != null)
                        {
                            skillAnalysisData.SetValue(id, itemStruct.Tag, iTextValue.TextValue);
                        }
                        else
                        {
                            skillAnalysisData.SetValue(id, itemStruct.Tag, control.Text);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发生改变后更改
        /// </summary>
        /// <param name="obj"></param>
        private void Create_ChangedHandle(ItemStruct obj)
        {
            UpdateItems();
        }

        /// <summary>
        /// 更新Item
        /// </summary>
        /// <param name="mustUpdateData">必须更新数据</param>
        public void UpdateItems(bool mustUpdateData = false)
        {
            Panel[] panels = FlowLayoutPanel_Release_Other.Controls.OfType<Panel>().ToArray();
            int outHeight = panels.Length * 3 * 2;
            foreach (Panel panel in panels)
            {
                ItemStruct itemStruct = panel.Tag as ItemStruct;
                if (itemStruct == null)
                    FlowLayoutPanel_Release_Other.Controls.Remove(panel);
                else
                {
                    //判断条件
                    if (itemStruct.ControlType != null && !string.IsNullOrEmpty(itemStruct.Label))
                    {
                        int maxHeight = 1;
                        //设置控件的说明
                        Label tempLabelControl = panel.Controls.OfType<Label>().Where(temp => string.Equals(temp.Name, "Label_Show")).FirstOrDefault();
                        if (tempLabelControl == null)
                        {
                            tempLabelControl = new Label();
                            panel.Controls.Add(tempLabelControl);
                            tempLabelControl.Name = "Label_Show";
                            tempLabelControl.Text = itemStruct.Label;
                            tempLabelControl.AutoSize = true;
                            tempLabelControl.MaximumSize = new Size(150, 0);
                            //tempLabelControl.Width = panel.Size.Width / 2;
                            tempLabelControl.Location = new Point(3, 3);
                        }
                        else
                        {
                            tempLabelControl.Text = itemStruct.Label;
                        }
                        maxHeight = tempLabelControl.Height;
                        //设置控件的子控件
                        Control control = panel.Controls.Find("ItemControl", false).FirstOrDefault();
                        int controlHeight = 30;
                        if (control == null && itemStruct.ControlType.IsSubclassOf(typeof(Control)))
                        {
                            control = Activator.CreateInstance(itemStruct.ControlType) as Control;
                            panel.Controls.Add(control);
                            control.Name = "ItemControl";
                            control.Location = new Point(9 + tempLabelControl.Width, 3);
                        }
                        if (control != null)
                        {
                            control.Tag = itemStruct.Tag;
                            ITypeTag iTypeTag = control as ITypeTag;
                            if (iTypeTag != null)
                            {
                                if (!string.Equals(iTypeTag.TypeTag, itemStruct.TypeTag) || mustUpdateData)
                                {
                                    iTypeTag.TypeTag = itemStruct.TypeTag;
                                    //是否不是数组
                                    if (!itemStruct.IsArray)
                                    {
                                        ITextValue iTextValue = control as ITextValue;
                                        if (skillAnalysisData != null && this.Tag != null)
                                        {
                                            string tempValue = skillAnalysisData.GetValue<string>(this.Tag.ToString(), itemStruct.Tag);
                                            if (iTextValue != null)
                                            {
                                                iTextValue.TextValue = tempValue;
                                            }
                                            else
                                            {
                                                control.Text = tempValue;
                                            }
                                            mustUpdateData = false;
                                        }
                                    }
                                }
                            }
                            IChildControlType iChildControlType = control as IChildControlType;
                            if (iChildControlType != null)
                            {
                                controlHeight *= itemStruct.ChildCount;
                                if (!string.Equals(iChildControlType.ChildControlType, itemStruct.ChildControlType) || mustUpdateData)
                                {
                                    iChildControlType.TextValues = new string[itemStruct.ChildCount];
                                    iChildControlType.ChildControlType = itemStruct.ChildControlType;
                                    //是否是数组
                                    if (itemStruct.IsArray)
                                    {
                                        if (skillAnalysisData != null && this.Tag != null)
                                        {
                                            string[] tempValues = skillAnalysisData.GetValues<string>(this.Tag.ToString(), itemStruct.Tag);
                                            if (tempValues == null)
                                                tempValues = new string[0];
                                            string[] resultValues = new string[itemStruct.ChildCount];
                                            int index = 0;
                                            while (index < tempValues.Length && index < resultValues.Length)
                                            {
                                                resultValues[index] = tempValues[index];
                                                index++;
                                            }
                                            iChildControlType.TextValues = resultValues;
                                            mustUpdateData = false;
                                        }
                                    }
                                }
                                else if (iChildControlType.TextValues.Length != itemStruct.ChildCount || mustUpdateData)
                                {
                                    string[] tempValues = null;
                                    if (this.Tag != null)
                                    {
                                        tempValues = skillAnalysisData.GetValues<string>(this.Tag.ToString(), itemStruct.Tag);
                                    }
                                    if (tempValues == null)
                                        tempValues = new string[0];
                                    string[] resultValues = new string[itemStruct.ChildCount];
                                    string[] nowValues = iChildControlType.TextValues;
                                    for (int i = 0; i < itemStruct.ChildCount; i++)
                                    {
                                        if (i < nowValues.Length)
                                        {
                                            resultValues[i] = nowValues[i];
                                            continue;
                                        }
                                        if (i < tempValues.Length)
                                        {
                                            resultValues[i] = tempValues[i];
                                        }
                                    }
                                    iChildControlType.TextValues = resultValues;
                                    mustUpdateData = false;
                                }
                            }
                            maxHeight = maxHeight > controlHeight ? maxHeight : controlHeight;
                        }

                        //设置父控件大小
                        panel.Size = new Size(FlowLayoutPanel_Release_Other.Width - 7, maxHeight + 6);
                        outHeight += maxHeight + 6;
                        //设置自身控件的大小
                        control.Size = new Size(panel.Size.Width - (12 + tempLabelControl.Width), controlHeight);
                    }
                    else
                    {
                        panel.Controls.Clear();
                        panel.Size = new Size(1, 1);
                    }
                }
            }
            FlowLayoutPanel_Release_Other.Size = new Size(FlowLayoutPanel_Release_Other.Size.Width, outHeight);
        }

        /// <summary>
        /// 里面每个控件的数据结构
        /// </summary>
        public class ItemStruct
        {
            /// <summary>
            /// 控件的说明
            /// </summary>
            private string label;
            /// <summary>
            /// 控件的tag的赋值
            /// 一般用于关联字段
            /// </summary>
            private string tag;
            /// <summary>
            /// 控件的tyepTag赋值
            /// </summary>
            private string typeTag;
            /// <summary>
            /// 控件的类型(不能是Lable)
            /// </summary>
            private Type controlType;
            /// <summary>
            /// 是否是数组
            /// </summary>
            private bool isArray;
            /// <summary>
            /// 数组中每个元素的控件类型
            /// </summary>
            private string childControlType;
            /// <summary>
            /// 数组的长度
            /// </summary>
            private int childCount;
            /// <summary>
            /// 是否有做出改变
            /// </summary>
            private bool changed;
            /// <summary>
            /// 控件的说明
            /// </summary>
            public string Label
            {
                get { return label; }
                set
                {
                    label = value;
                    changed = true;
                }
            }
            /// <summary>
            /// 控件的Tag的赋值
            /// 一般用于关联字段
            /// </summary>
            public string Tag
            {
                get { return tag; }
                set
                {
                    changed = true;
                    tag = value;
                }
            }
            /// <summary>
            /// 控件的TypeTag赋值
            /// </summary>
            public string TypeTag
            {
                get { return typeTag; }
                set
                {
                    changed = true;
                    typeTag = value;
                }
            }
            /// <summary>
            /// 控件的类型(不能是Lable)
            /// 如果时数组，则此时的控件类型必须是AutoArrayControl
            /// </summary>
            public Type ControlType
            {
                get { return controlType; }
                set
                {
                    changed = true;
                    controlType = value;
                }
            }
            /// <summary>
            /// 是否是数组
            /// </summary>
            public bool IsArray
            {
                get { return isArray; }
                set
                {
                    changed = true;
                    isArray = value;
                }
            }
            /// <summary>
            /// 数组中每个元素的控件类型
            /// </summary>
            public string ChildControlType
            {
                get { return childControlType; }
                set
                {
                    changed = true;
                    childControlType = value;
                }
            }
            /// <summary>
            /// 数组的长度
            /// </summary>
            public int ChildCount
            {
                get { return childCount; }
                set
                {
                    changed = true;
                    childCount = value;
                }
            }

            /// <summary>
            /// 数据发生变化时改变
            /// </summary>
            public event Action<ItemStruct> ChangedHandle;
            /// <summary>
            /// 更新
            /// </summary>
            public void Update()
            {
                if (changed && ChangedHandle != null)
                {
                    changed = false;
                    ChangedHandle(this);
                }
            }
        }
    }
}
