using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// 表示对话条件的控件
    /// </summary>
    public partial class DialogueConditionControl : UserControl, IDialoguePointID, IOpenStop,ISelectedControl
    {
        /// <summary>
        /// 对话条件对象
        /// </summary>
        DialogueCondition dialogueCondition;

        /// <summary>
        /// 子节点集合
        /// </summary>
        List<IDialoguePointID> childIDList;

        public DialogueConditionControl() : this(null)
        { }

        public DialogueConditionControl(DialogueCondition dialogueCondition)
        {
            InitializeComponent();
            childIDList = new List<IDialoguePointID>();
            if (dialogueCondition == null)
            {
                this.dialogueCondition = new DialogueCondition();
                this.dialogueCondition.text = "";
                this.dialogueCondition.enumDialogueType = EnumDialogueType.Normal;
                this.dialogueCondition.maxLevel = 99;
                this.dialogueCondition.minLevel = -1;
                this.dialogueCondition.overTask = -1;
                this.dialogueCondition.thisTask = -1;
                this.dialogueCondition.minGoodAndEvil = -9999;
                this.dialogueCondition.maxGoodAndEvil = 9999;
                this.dialogueCondition.race = RoleOfRace.None;
            }
            else
            {
                this.dialogueCondition = dialogueCondition;
                if (string.IsNullOrEmpty(this.dialogueCondition.text))
                    this.dialogueCondition.text = "";
            }
            Init();
        }

        private void Init()
        {
            Action<ComboBox, Type> InitComboBox = (cbo, t) =>
            {
                Dictionary<FieldInfo, FieldExplanAttribute> tempDic = t.GetFields()
                    .Where(temp => !string.Equals(temp.Name, "value__"))
                    .Select(temp => new { attr = temp.GetCustomAttributes(typeof(FieldExplanAttribute), false).FirstOrDefault(), field = temp })
                    .Where(temp => temp.attr != null)
                    .Select(temp => new { attr = (FieldExplanAttribute)temp.attr, field = temp.field })
                    .ToDictionary(temp => temp.field, temp => temp.attr);
                DataTable dt = new DataTable();
                dt.Columns.Add("Enum");
                dt.Columns.Add("Text");
                foreach (KeyValuePair<FieldInfo, FieldExplanAttribute> item in tempDic)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = item.Key.Name;
                    dr[1] = item.Value.GetExplan();
                    dt.Rows.Add(dr);
                }
                cbo.DataSource = dt;
                cbo.DisplayMember = "Text";
                cbo.ValueMember = "Enum";
            };

            InitComboBox(ComboBox_DialogueType, typeof(EnumDialogueType));
            InitComboBox(ComboBox_RoleOfRace, typeof(RoleOfRace));

            TextBox_Text.Text = dialogueCondition.text;
            TextBox_TouchNPCID.Text = dialogueCondition.touchNPCID.ToString();
            ComboBox_DialogueType.SelectedValue = dialogueCondition.enumDialogueType;
            TextBox_OverTask.Text = dialogueCondition.overTask.ToString();
            TextBox_ThisTask.Text = dialogueCondition.thisTask.ToString();
            NumericUpDown_Level_Min.Value = dialogueCondition.minLevel;
            NumericUpDown_Level_Max.Value = dialogueCondition.maxLevel;
            NumericUpDown_GoodAndEvil_Min.Value = dialogueCondition.minGoodAndEvil;
            NumericUpDown_GoodAndEvil_Max.Value = dialogueCondition.maxGoodAndEvil;
            ComboBox_RoleOfRace.SelectedValue = dialogueCondition.race;

            TextBox_Text.TextChanged += (sender, e) =>
            {
                dialogueCondition.text = TextBox_Text.Text;
            };
            TextBox_TouchNPCID.TextChanged += (sender, e) =>
            {
                int value;
                if (int.TryParse(TextBox_TouchNPCID.Text, out value))
                {
                    dialogueCondition.touchNPCID = value >= -1 ? value : -1;
                    if (value < -1)
                        TextBox_TouchNPCID.Text = "-1";
                }
                else
                    TextBox_TouchNPCID.Text = dialogueCondition.touchNPCID.ToString();

            };
            TextBox_OverTask.TextChanged += (sender, e) => 
            {
                int value;
                if (int.TryParse(TextBox_OverTask.Text, out value))
                {
                    dialogueCondition.overTask = value >= -1 ? value : -1;
                    if (value < -1)
                        TextBox_OverTask.Text = "-1";
                }
                else
                    TextBox_OverTask.Text = dialogueCondition.overTask.ToString();
            };
            TextBox_ThisTask.TextChanged += (sender, e) => 
            {
                int value;
                if (int.TryParse(TextBox_ThisTask.Text, out value))
                {
                    dialogueCondition.thisTask = value > -1 ? value : -1;
                    if (value < -1)
                        TextBox_ThisTask.Text = "-1";
                }
                else
                    TextBox_ThisTask.Text = dialogueCondition.thisTask.ToString();
            };
            ComboBox_DialogueType.SelectedIndexChanged += (sender, e) =>
            {
                try
                {
                    dialogueCondition.enumDialogueType = (EnumDialogueType)Enum.Parse(typeof(EnumDialogueType), ComboBox_DialogueType.SelectedValue.ToString());
                }
                catch { }
            };
            ComboBox_RoleOfRace.SelectedIndexChanged += (sender, e) =>
            {
                try
                {
                    dialogueCondition.race = (RoleOfRace)Enum.Parse(typeof(RoleOfRace), ComboBox_RoleOfRace.SelectedValue.ToString());
                }
                catch { }
            };
            NumericUpDown_Level_Min.ValueChanged += (sender, e) => { dialogueCondition.minLevel = (int)NumericUpDown_Level_Min.Value; };
            NumericUpDown_Level_Max.ValueChanged += (sender, e) => { dialogueCondition.maxLevel = (int)NumericUpDown_Level_Max.Value; };
            NumericUpDown_GoodAndEvil_Min.ValueChanged += (sender, e) => { dialogueCondition.minGoodAndEvil = (int)NumericUpDown_GoodAndEvil_Min.Value; };
            NumericUpDown_GoodAndEvil_Max.ValueChanged += (sender, e) => { dialogueCondition.maxGoodAndEvil = (int)NumericUpDown_GoodAndEvil_Max.Value; };
        }

        /// <summary>
        /// 获取对话条件
        /// </summary>
        /// <returns></returns>
        public DialogueCondition GetDialogueCondition()
        {
            return dialogueCondition;
        }

        #region 节点相关

        /// <summary>
        /// 获取当前节点的子节点
        /// </summary>
        public IDialoguePointID[] GetDialogueNextPointID
        {
            get
            {
                return childIDList.ToArray();
            }
        }

        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="child">子节点</param>
        /// <returns></returns>
        public void AddNextPointID(IDialoguePointID child)
        {
            if (!childIDList.Contains(child))
                childIDList.Add(child);
        }

        /// <summary>
        /// 移除一个子节点
        /// </summary>
        /// <param name="child">子节点</param>
        /// <returns></returns>
        public void RemovNextPointID(IDialoguePointID child)
        {
            if (childIDList.Contains(child))
                childIDList.RemoveAll(temp => temp.Equals(child));
        }

        #endregion


        #region 控件大小位置相关
        /// <summary>
        /// 控件的展开收起状态
        /// </summary>
        bool openStopState;
        /// <summary>
        /// 展开或收起变化发生时触发
        /// </summary>
        Action<IOpenStop, bool> OpenStopChanged;
        /// <summary>
        /// 获取控件的展开收起状态
        /// </summary>
        public bool OpenStopState
        {
            get { return openStopState; }
        }
        /// <summary>
        /// 展开后的大小
        /// </summary>
        public Size OpenSize => new Size(200, 225);
        /// <summary>
        /// 收起后的大小 
        /// </summary>
        public Size StopSize => new Size(200, 33);
        /// <summary>
        /// 展开控件
        /// </summary>
        /// <param name="location">控件位置</param>
        public void OpenControl(Point location)
        {
            this.Size = OpenSize;
            this.Location = location;
            openStopState = true;
            Button_OpenStop.Text = "-";
        }
        /// <summary>
        /// 收起控件
        /// </summary>
        /// <param name="location">控件位置 </param>
        public void StopControl(Point location)
        {
            this.Size = StopSize;
            this.Location = location;
            openStopState = false;
            Button_OpenStop.Text = "+";
        }
        /// <summary>
        /// 注册对象的展开收起发生变化事件
        /// </summary>
        /// <param name="OpenStopChanged"></param>
        public void SetLiestenOpenStop(Action<IOpenStop, bool> OpenStopChanged)
        {
            this.OpenStopChanged = OpenStopChanged;
        }
        /// <summary>
        /// 展开或收起控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenStop_Click(object sender, EventArgs e)
        {
            OpenStopChanged?.Invoke(this, openStopState);
        }
        #endregion


        #region 控件选中相关
        /// <summary>
        /// 当前是否是选中状态
        /// </summary>
        bool isSelectControl;
        /// <summary>
        /// 控件被选中时触发
        /// </summary>
        Action<ISelectedControl> ListenSelectedControl;
        /// <summary>
        /// 注册监听控件被选中方法
        /// </summary>
        /// <param name="ListenSelectedControl">委托函数</param>
        public void SetListenControlSelected(Action<ISelectedControl> ListenSelectedControl)
        {
            this.ListenSelectedControl = ListenSelectedControl;
        }

        /// <summary>
        /// 取消选中控件
        /// </summary>
        public void NoSelectedControl()
        {
            if (isSelectControl)
            {
                isSelectControl = false;
                this.BackColor = DefaultBackColor;
            }
        }

        /// <summary>
        /// 选中控件
        /// </summary>
        public void SelectedControl()
        {
            if (!isSelectControl)
            {
                isSelectControl = true;
                this.BackColor = Color.Green;
            }
        }

        /// <summary>
        /// 鼠标在控件上点击时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogueConditionControl_MouseClick(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.Green;
            if (!isSelectControl)
            {
                isSelectControl = true;
                ListenSelectedControl?.Invoke(this);
            }
        }

        /// <summary>
        /// 获取父控件
        /// </summary>
        /// <returns></returns>
        public Control GetParent()
        {
            return this.Parent;
        }

        #endregion


    }
}
