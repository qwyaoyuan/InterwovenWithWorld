using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DialogueDataFileEditor
{
    /// <summary>
    /// 表示对话节点的控件
    /// </summary>
    public partial class DialoguePointControl : UserControl, IDialoguePointID, IOpenStop,ISelectedControl
    {
        /// <summary>
        /// 对话数据对象
        /// </summary>
        DialogueValue dialogueValue;

        /// <summary>
        /// 子节点集合
        /// </summary>
        List<IDialoguePointID> childIDList;

        public DialoguePointControl() : this(null)
        { }

        public DialoguePointControl(DialogueValue dialogueValue)
        {
            InitializeComponent();
            childIDList = new List<IDialoguePointID>();
            if (dialogueValue == null)
            {
                this.dialogueValue = new DialogueValue();
                this.dialogueValue.dialogueID = IDCreator.Instance.GetNextID();
                this.dialogueValue.npcID = -1;
                this.dialogueValue.voiceID = -1;
                this.dialogueValue.titleValue = "";
                this.dialogueValue.showValue = "";
                this.dialogueValue.otherValue = "";
            }
            else
            {
                this.dialogueValue = dialogueValue;
            }
            Init();
        }

        private void Init()
        {
            TextBox_ID.Text = dialogueValue.dialogueID.ToString();
            TextBox_NPCID.Text = dialogueValue.npcID.ToString();
            TextBox_VoiceID.Text = dialogueValue.voiceID.ToString();
            TextBox_TitleValue.Text = dialogueValue.titleValue?.ToString();
            TextBox_ShowValue.Text = dialogueValue.showValue?.ToString();
            TextBox_OtherValue.Text = dialogueValue.otherValue?.ToString();

            TextBox_NPCID.TextChanged += (sender, e) =>
            {
                int value;
                if (int.TryParse(TextBox_NPCID.Text, out value))
                {
                    dialogueValue.npcID = value >= -1 ? value : -1;
                    if (value < -1)
                        TextBox_NPCID.Text = "-1";
                }
                else TextBox_NPCID.Text = dialogueValue.npcID.ToString();
            };
            TextBox_VoiceID.TextChanged += (sender, e) =>
            {
                int value;
                if (int.TryParse(TextBox_VoiceID.Text, out value))
                {
                    dialogueValue.voiceID = value >= -1 ? value : -1;
                    if (value < -1)
                        TextBox_VoiceID.Text = "-1";
                }
                else TextBox_VoiceID.Text = dialogueValue.voiceID.ToString();
            };
            TextBox_TitleValue.TextChanged += (sender, e) => 
            {
                dialogueValue.titleValue = TextBox_TitleValue.Text;
            };
            TextBox_ShowValue.TextChanged += (sender, e) => 
            {
                dialogueValue.showValue = TextBox_ShowValue.Text;
            };
            TextBox_OtherValue.TextChanged += (sender, e) => 
            {
                dialogueValue.otherValue = TextBox_OtherValue.Text;
            };
        }

        /// <summary>
        /// 获取对话数据
        /// </summary>
        /// <returns></returns>
        public DialogueValue GetDialogueValue()
        {
            return dialogueValue;
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
        public Size OpenSize => new Size(240,265);
        /// <summary>
        /// 收起后的大小 
        /// </summary>
        public Size StopSize => new Size(240, 28);
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
        private void DialoguePointControl_MouseClick(object sender, MouseEventArgs e)
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
