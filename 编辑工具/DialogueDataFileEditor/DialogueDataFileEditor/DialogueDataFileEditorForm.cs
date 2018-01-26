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

namespace DialogueDataFileEditor
{
    public partial class DialogueDataFileEditorForm : Form
    {
        public DialogueDataFileEditorForm()
        {
            InitializeComponent();
            dialogueAnalysisData = new DialogueAnalysisData();
        }

        /// <summary>
        /// 搜索窗体
        /// </summary>
        SearchNodeForm searchNodeForm = null;

        /// <summary>
        /// 对话解析对象
        /// </summary>
        DialogueAnalysisData dialogueAnalysisData;

        /// <summary>
        /// 当前选中的控件
        /// </summary>
        ISelectedControl iSelectedControlNow;

        /// <summary>
        /// 方案文件路径
        /// </summary>
        string projectPath = "";
        /// <summary>
        /// 对话数据文件名
        /// </summary>
        string dialogueValueFileName = "";
        /// <summary>
        /// 对话关系文件名
        /// </summary>
        string dialogueConditionFileName = "";

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iSelectedControlNow == null)//此时添加顶层节点
            {
                CreateNewTopNode();
            }
            else//此时添加子节点
            {
                AddNewChildNode(iSelectedControlNow);
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iSelectedControlNow != null)
            {
                IDialoguePointID iDialoguePointID = iSelectedControlNow as IDialoguePointID;
                Control parentControl = iSelectedControlNow.GetParent();
                if (iDialoguePointID.GetType().Equals(typeof(DialogueConditionControl)))
                {
                    FlowLayoutPanel_Main.Controls.Remove(parentControl);
                    iSelectedControlNow = null;
                }
                else
                {
                    DialogueConditionControl dialogueConditionControl = parentControl.Controls.OfType<DialogueConditionControl>().FirstOrDefault();
                    if (dialogueConditionControl != null)
                    {
                        IDialoguePointID parentIDialoguePointID = GetParentDialoguePointID(iDialoguePointID, dialogueConditionControl);
                        if (parentIDialoguePointID != null)
                        {
                            parentIDialoguePointID.RemovNextPointID(iDialoguePointID);
                            RemoveDialoguePointIDControl(parentControl, iDialoguePointID);
                            List<Control> tagControlList = (parentIDialoguePointID as Control).Tag as List<Control>;
                            if (tagControlList != null)
                                tagControlList.Remove(iDialoguePointID as Control);
                            //移除所有子节点
                            iSelectedControlNow = null;
                            ResetControlRect(parentControl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除指定节点的控件
        /// </summary>
        /// <param name="control">父控件</param>
        /// <param name="iDialoguePointID">要移除的节点</param>
        private void RemoveDialoguePointIDControl(Control control, IDialoguePointID iDialoguePointID)
        {
            IDialoguePointID[] childs = iDialoguePointID.GetDialogueNextPointID;
            foreach (IDialoguePointID child in childs)
            {
                RemoveDialoguePointIDControl(control, child);
            }
            control.Controls.Remove(iDialoguePointID as Control);
        }

        /// <summary>
        /// 查找指定子节点的父节点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parents"></param>
        /// <returns></returns>
        private IDialoguePointID GetParentDialoguePointID(IDialoguePointID target, params IDialoguePointID[] parents)
        {
            foreach (IDialoguePointID item in parents)
            {
                if (item.GetDialogueNextPointID.Contains(target))
                    return item;
                IDialoguePointID result = GetParentDialoguePointID(target, item.GetDialogueNextPointID);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 创建一个顶级节点
        /// </summary>
        /// <param name="conditionData">关系数据</param>
        /// <param name="updateControl">是否立即更新</param>
        private DialogueConditionControl CreateNewTopNode(DialogueCondition conditionData = null, bool updateControl = true)
        {
            Panel p = new Panel();
            FlowLayoutPanel_Main.Controls.Add(p);
            DialogueConditionControl dialogueConditionControl = new DialogueConditionControl(conditionData);
            dialogueConditionControl.Size = dialogueConditionControl.StopSize;
            dialogueConditionControl.SetListenControlSelected(ListenControlSelected);
            dialogueConditionControl.SetLiestenOpenStop(AddListenOpenStop);
            dialogueConditionControl.Tag = new List<Control>();
            p.Controls.Add(dialogueConditionControl);
            p.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.Clear(p.BackColor);
                //绘制连接线
                Pen pen = new Pen(Color.Red);
                foreach (Control control in p.Controls.OfType<Control>())
                {
                    if (control.Tag == null)
                        continue;
                    Size parentSize = control.Size;
                    Point parentLocation = control.Location;
                    List<Control> childControls = control.Tag as List<Control>;
                    if (parentLocation.X >= 0 && parentLocation.Y >= 0)
                    {
                        foreach (Control childControl in childControls)
                        {
                            Size childSize = childControl.Size;
                            Point childLocation = childControl.Location;
                            if (childLocation.X >= 0 && childLocation.Y >= 0)
                            {
                                g.DrawBezier(pen,
                                    new Point(parentLocation.X + parentSize.Width, parentLocation.Y + parentSize.Height / 2),
                                    new Point(parentLocation.X + parentSize.Width + 20, parentLocation.Y + parentSize.Height / 2),
                                    new Point(childLocation.X - 20, childLocation.Y + childSize.Height / 2),
                                    new Point(childLocation.X, childLocation.Y + childSize.Height / 2));
                            }
                        }

                    }
                }
            };
            if (updateControl)
                ResetControlRect(p);
            if (searchNodeForm != null)
                searchNodeForm.NodeChanged = true;
            return dialogueConditionControl;
        }

        /// <summary>
        /// 添加一个子节点
        /// </summary>
        /// <param name="iSelectedControlNow">要添加子节点的节点</param>
        private DialoguePointControl AddNewChildNode(ISelectedControl iSelectedControlNow, DialogueValue dialogueValue = null, bool updateControl = true)
        {
            IDialoguePointID iDialoguePointID = iSelectedControlNow as IDialoguePointID;
            if (iDialoguePointID.GetType().Equals(typeof(DialogueConditionControl)))//顶层节点（关系关系）只能添加一个子节点（数据节点）
            {
                if (iDialoguePointID.GetDialogueNextPointID.Length > 0)
                {
                    return null;
                }
            }
            if (iDialoguePointID != null)
            {
                DialoguePointControl dialoguePointControl = new DialoguePointControl(dialogueValue);
                dialoguePointControl.Size = dialoguePointControl.StopSize;
                dialoguePointControl.SetListenControlSelected(ListenControlSelected);
                dialoguePointControl.SetLiestenOpenStop(AddListenOpenStop);
                dialoguePointControl.Tag = new List<Control>();
                Control iSelectedControl = iSelectedControlNow as Control;
                List<Control> childControl = iSelectedControl.Tag as List<Control>;
                childControl.Add(dialoguePointControl);
                Control parentControl = iSelectedControlNow.GetParent();
                parentControl.Controls.Add(dialoguePointControl);
                iDialoguePointID.AddNextPointID(dialoguePointControl);
                if (updateControl)
                    ResetControlRect(parentControl);
                if (searchNodeForm != null)
                    searchNodeForm.NodeChanged = true;
                return dialoguePointControl;
            }
            return null;
        }

        /// <summary>
        /// 重新设置控件尺寸
        /// </summary>
        /// <param name="parent"></param>
        private void ResetControlRect(Control parent)
        {
            if (parent == null)
                return;
            try
            {
                DialogueConditionControl topControl = parent.Controls.OfType<DialogueConditionControl>().FirstOrDefault();
                #region 构建一个深度结构并计算

                //初步构建
                List<IDialoguePointID> nextChilds = new List<IDialoguePointID>();
                List<List<DeepControlStruct>> deepControlStructLists = new List<List<DeepControlStruct>>();
                int tempDeep = 0;
                nextChilds.Add(topControl);
                while (nextChilds.Count > 0)
                {
                    IDialoguePointID[] tempChilds = nextChilds.ToArray();
                    nextChilds.Clear();
                    if (deepControlStructLists.Count == tempDeep)
                    {
                        List<DeepControlStruct> deepControlStructList = new List<DeepControlStruct>();
                        deepControlStructLists.Add(deepControlStructList);
                        deepControlStructList.AddRange(tempChilds.Select(temp => new DeepControlStruct() { deep = tempDeep, iOpenStop = temp as IOpenStop, childs = new List<DeepControlStruct>() }));
                    }
                    foreach (IDialoguePointID tempChild in tempChilds)
                    {
                        IOpenStop iOpenStop = tempChild as IOpenStop;
                        if (iOpenStop != null && iOpenStop.OpenStopState)//如果该节点是展开节点并且该节点时展开的
                        {
                            nextChilds.AddRange(tempChild.GetDialogueNextPointID);//用于下次计算
                        }
                    }
                    tempDeep++;
                }

                //计算父子关系
                for (int i = 0; i < deepControlStructLists.Count - 1; i++)
                {
                    foreach (DeepControlStruct deepControlStruct in deepControlStructLists[i])
                    {
                        IDialoguePointID iDialoguePointID = deepControlStruct.iOpenStop as IDialoguePointID;
                        IDialoguePointID[] childIDialoguePointIDs = iDialoguePointID.GetDialogueNextPointID;
                        DeepControlStruct[] childs = deepControlStructLists[i + 1].Where(temp => childIDialoguePointIDs.Contains(temp.iOpenStop as IDialoguePointID)).ToArray();
                        foreach (DeepControlStruct child in childs)
                        {
                            child.parent = deepControlStruct;
                            deepControlStruct.childs.Add(child);
                        }
                    }
                }
                //初始化对象的BoundY
                foreach (DeepControlStruct deepControlStruct in deepControlStructLists[0])
                {
                    deepControlStruct.InitBoundY();
                }
                //计算每个节点的x轴坐标
                int tempLocationX = 0;
                for (int i = 0; i < deepControlStructLists.Count; i++)
                {
                    int maxLocationX = 0;
                    foreach (DeepControlStruct deepControlStruct in deepControlStructLists[i])
                    {
                        deepControlStruct.location_x = tempLocationX;
                        int thisWidth = deepControlStruct.iOpenStop.OpenStopState ? deepControlStruct.iOpenStop.OpenSize.Width : deepControlStruct.iOpenStop.StopSize.Width;
                        maxLocationX = maxLocationX > thisWidth ? maxLocationX : thisWidth;
                    }
                    tempLocationX += maxLocationX + 50;
                }
                //计算每个节点的y轴坐标 
                foreach (List<DeepControlStruct> deepControlStructList in deepControlStructLists)
                {
                    int tempY = 0;
                    DeepControlStruct parentDeep = null;
                    foreach (DeepControlStruct deepControlStruct in deepControlStructList)
                    {
                        if (deepControlStruct.parent == null)
                        {
                            deepControlStruct.location_y = tempY;
                            tempY += deepControlStruct.boundY;
                        }
                        else
                        {
                            if (parentDeep != deepControlStruct.parent)
                            {
                                tempY = 0;
                                parentDeep = deepControlStruct.parent;
                            }
                            deepControlStruct.location_y = parentDeep.location_y + tempY;
                            tempY += deepControlStruct.boundY;
                        }
                    }
                }
                //设置父控件大小
                List<int> deepWidthList = new List<int>();
                deepWidthList = deepControlStructLists.Select(temp =>
                    temp.Select(deepControl => deepControl.iOpenStop).
                    Select(iOpenStop => iOpenStop.OpenStopState ? iOpenStop.OpenSize.Width : iOpenStop.StopSize.Width).
                    Max() + 50
                ).ToList();
                int panelWidth = deepWidthList.Sum();
                int panelHeight = deepControlStructLists[0].Select(temp => temp.boundY).Sum();
                parent.Size = new Size(panelWidth, panelHeight);
                //设置每个节点的大小以及隐藏节点的位置与大小
                Action<IDialoguePointID> HideChild = null;//隐藏子节点
                HideChild = (target) =>
                {
                    IDialoguePointID[] childs = target.GetDialogueNextPointID;
                    foreach (IDialoguePointID child in childs)
                    {
                        (child as Control).Location = new Point(-1000, -1000);
                        HideChild(child);
                    }
                };
                foreach (List<DeepControlStruct> deepControlStructList in deepControlStructLists)
                {
                    foreach (DeepControlStruct deepControlStruct in deepControlStructList)
                    {
                        Control thisControl = deepControlStruct.iOpenStop as Control;
                        thisControl.Location = new Point(deepControlStruct.location_x, deepControlStruct.location_y);
                        thisControl.Size = deepControlStruct.iOpenStop.OpenStopState ? deepControlStruct.iOpenStop.OpenSize : deepControlStruct.iOpenStop.StopSize;
                        if (!deepControlStruct.iOpenStop.OpenStopState)
                        {
                            HideChild(deepControlStruct.iOpenStop as IDialoguePointID);
                        }
                    }
                }
                //重置背景控件大小
                ResetBackRect();
                #endregion
            }
            catch { }
        }

        /// <summary>
        /// 重置背景控件大小
        /// </summary>
        private void ResetBackRect()
        {
            int width = 0;
            int height = 0;
            try
            {
                width = FlowLayoutPanel_Main.Controls.OfType<Panel>().Select(temp => temp.Size.Width + 3).Max();
            }
            catch { }
            try
            {
                height = FlowLayoutPanel_Main.Controls.OfType<Panel>().Select(temp => temp.Size.Height + 7).Sum();
            }
            catch { }
            width = width > Panel_Main.Size.Width ? width : Panel_Main.Width;
            height = height > Panel_Main.Size.Height ? height : Panel_Main.Height;
            FlowLayoutPanel_Main.Size = new Size(width, height);
            FlowLayoutPanel_Main.Controls.OfType<Panel>().ToList().ForEach(temp => temp.Refresh());
        }

        /// <summary>
        /// 监听控件被选择变化
        /// </summary>
        /// <param name="iSelectedControl"></param>
        private void ListenControlSelected(ISelectedControl iSelectedControl)
        {
            if (iSelectedControlNow != null)
                iSelectedControlNow.NoSelectedControl();
            iSelectedControlNow = iSelectedControl;
        }

        /// <summary>
        /// 监听展开收起变化
        /// </summary>
        /// <param name="iOpenStop">对象</param>
        /// <param name="state">当前状态（需要改变状态）</param>
        private void AddListenOpenStop(IOpenStop iOpenStop, bool state)
        {
            ISelectedControl targetSelectedContrl = iOpenStop as ISelectedControl;
            if (targetSelectedContrl != null)
            {
                if (state)
                    iOpenStop.StopControl(new Point(0, 0));
                else
                    iOpenStop.OpenControl(new Point(0, 0));
                ResetControlRect(targetSelectedContrl.GetParent());
            }
        }

        /// <summary>
        /// 鼠标点击主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowLayoutPanel_Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (iSelectedControlNow != null)
                    iSelectedControlNow.NoSelectedControl();
                iSelectedControlNow = null;
            }
        }

        /// <summary>
        /// 打开方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
#if DEBUG
            ofd.InitialDirectory = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\编辑工具\DialogueData";
#endif
            ofd.Filter = "对话文件|*.DialogueEditor";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                OpenProject(ofd.FileName);
            }
        }

        /// <summary>
        /// 新建方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新建方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProjectForm newProjectForm = new NewProjectForm();
            DialogResult dr = newProjectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //先创建其他两个文件
                string folderPath = Path.GetDirectoryName(newProjectForm.newFilePath);//父文件夹路径
                while (true)
                {
                    dialogueConditionFileName = Tools.GetRandomString(30, true, true, true, false, "");
                    if (!File.Exists(folderPath + "\\" + dialogueConditionFileName+ ".txt"))
                    {
                        File.Create(folderPath + "\\" + dialogueConditionFileName+ ".txt").Close();
                        break;
                    }
                }
                while (true)
                {
                    dialogueValueFileName = Tools.GetRandomString(30, true, true, true, false, "");
                    if (!File.Exists(folderPath + "\\" + dialogueValueFileName+ ".txt"))
                    {
                        File.Create(folderPath + "\\" + dialogueValueFileName+ ".txt").Close();
                        break;
                    }
                }
                File.WriteAllText(newProjectForm.newFilePath, dialogueConditionFileName + "\r\n" + dialogueValueFileName, Encoding.UTF8);
                OpenProject(newProjectForm.newFilePath);
            }
        }

        /// <summary>
        /// 具体的打开方案类
        /// </summary>
        /// <param name="projectPath">方案文件路径</param>
        private void OpenProject(string projectPath)
        {
            if (searchNodeForm != null)
                searchNodeForm.MustClose();
            searchNodeForm = new SearchNodeForm();
            searchNodeForm.ShowSearchPanel = ShowSearchPanel;
            searchNodeForm.BasePanel = FlowLayoutPanel_Main;
            this.projectPath = projectPath;
            FlowLayoutPanel_Main.Controls.Clear();
            dialogueConditionFileName = "";//对话条件文件名
            dialogueValueFileName = "";//对话数据文件名
            using (StreamReader fs = new StreamReader(this.projectPath, Encoding.UTF8))
            {
                dialogueConditionFileName = fs.ReadLine() ;
                dialogueValueFileName = fs.ReadLine();
            }
            string folderPath = Path.GetDirectoryName(this.projectPath);
            string dialogueConditionStr = "";
            string dialogueValueStr = "";
            if (!string.IsNullOrEmpty(dialogueConditionFileName) && File.Exists(folderPath + "\\" + dialogueConditionFileName+ ".txt"))
            {
                dialogueConditionStr = File.ReadAllText(folderPath + "\\" + dialogueConditionFileName+ ".txt", Encoding.UTF8);
            }
            if (!string.IsNullOrEmpty(dialogueValueFileName) && File.Exists(folderPath + "\\" + dialogueValueFileName+ ".txt"))
            {
                dialogueValueStr = File.ReadAllText(folderPath + "\\" + dialogueValueFileName+ ".txt", Encoding.UTF8);
            }
            if (!string.IsNullOrEmpty(dialogueConditionFileName) && !string.IsNullOrEmpty(dialogueValueFileName))
            {
                int maxID = dialogueAnalysisData.ReadData(dialogueConditionStr, dialogueValueStr);
                IDCreator.Instance.Init(maxID + 1);
                CreateControlByData();
                添加节点ToolStripMenuItem.Enabled = true;
                删除节点ToolStripMenuItem.Enabled = true;
                保存方案ToolStripMenuItem.Enabled = true;
                搜索节点ToolStripMenuItem.Enabled = true;
                展开节点ToolStripMenuItem.Enabled = true;
                收起节点ToolStripMenuItem.Enabled = true;
            }
        }

        #region 根据现有数据创建控件 
        /// <summary>
        /// 根据现有数据创建控件
        /// </summary>
        private void CreateControlByData()
        {
            int[] dialogueConditionIDs = dialogueAnalysisData.GetDialogueConditionAllID;
            foreach (int dialogueConditionID in dialogueConditionIDs)
            {
                DialogueCondition dialogueCondition = dialogueAnalysisData.GetDialogueConditionIDByID(dialogueConditionID);
                if (dialogueCondition == null)
                    continue;
                DialogueConditionControl dialogueConditionControl = CreateNewTopNode(dialogueCondition, false);
                DialoguePoint dialoguePoint = dialogueCondition.topPoint;
                CreateControlByData(dialogueConditionControl, dialoguePoint);
                ResetControlRect(dialogueConditionControl.Parent);
                dialogueConditionControl.Parent.Refresh();
            }
            ResetBackRect();
        }

        /// <summary>
        /// 根据现有数据创建控件 
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="dialoguePoints">节点关系对象</param>
        private void CreateControlByData(IDialoguePointID parent, params DialoguePoint[] dialoguePoints)
        {
            foreach (DialoguePoint dialoguePoint in dialoguePoints)
            {
                DialogueValue dialogueValue = dialogueAnalysisData.GetDialoguePointByID(dialoguePoint.dialogueID);
                if (dialogueValue == null)
                    continue;
                DialoguePointControl dialoguePointControl = AddNewChildNode(parent as ISelectedControl, dialogueValue, false);//添加一个子节点
                if (dialoguePoint.childDialoguePoints != null && dialoguePoint.childDialoguePoints.Length > 0)
                    CreateControlByData(dialoguePointControl, dialoguePoint.childDialoguePoints);
            }
        }
        #endregion

        /// <summary>
        /// 将选择的控件移动到父控件指定位置以显示控件
        /// </summary>
        /// <param name="panel">选择显示的控件</param>
        private void ShowSearchPanel(Panel panel)
        {
            if (panel != null)
            {
                int locationY = panel.Location.Y;
                int allHeight = FlowLayoutPanel_Main.Size.Height;
                int windowHeigth = Panel_Main.Size.Height;
                float bili = (float)locationY / (float)allHeight;
                Panel_Main.VerticalScroll.Value = (int)(Panel_Main.VerticalScroll.Maximum * bili);
                Panel_Main.VerticalScroll.Value = (int)(Panel_Main.VerticalScroll.Maximum * bili);
                DialogueConditionControl dialogueConditionControl = panel.Controls.OfType<DialogueConditionControl>().FirstOrDefault();
                if (dialogueConditionControl != null)
                {
                    if (iSelectedControlNow != null)
                        iSelectedControlNow.NoSelectedControl();
                    dialogueConditionControl.SelectedControl();
                    iSelectedControlNow = dialogueConditionControl;
                }
            }
        }

        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存方案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //重建对话关系数据集合以及对话数据字典
            List<DialogueCondition> dialogueConditionList = new List<DialogueCondition>();
            Dictionary<int, DialogueValue> dialogueValueDic = new Dictionary<int, DialogueValue>();
            foreach (Panel panel in FlowLayoutPanel_Main.Controls.OfType<Panel>())
            {
                DialogueConditionControl dialogueConditionContorl = panel.Controls.OfType<DialogueConditionControl>().FirstOrDefault();
                if (dialogueConditionContorl == null)
                    continue;
                //对话条件
                DialogueCondition dialogueCondition = dialogueConditionContorl.GetDialogueCondition();
                //对话关系
                if (dialogueConditionContorl.Tag != null
                    && dialogueConditionContorl.Tag.GetType().Equals(typeof(List<Control>))
                    && (dialogueConditionContorl.Tag as List<Control>).Count > 0)
                {
                    List<Control> childControlList = dialogueConditionContorl.Tag as List<Control>;
                    DialoguePointControl topPointControl = childControlList[0] as DialoguePointControl;
                    if (topPointControl != null)
                    {
                        DialoguePoint topPoint = new DialoguePoint();
                        topPoint.dialogueID = topPointControl.GetDialogueValue().dialogueID;
                        if (childControlList[0].Tag != null
                            && childControlList[0].Tag.GetType().Equals(typeof(List<Control>))
                            && (childControlList[0].Tag as List<Control>).Count > 0)
                            SetDialoguePointData_Child(topPoint, (childControlList[0].Tag as List<Control>).ToArray());
                        dialogueCondition.topPoint = topPoint;
                    }
                }
                else
                {
                    dialogueCondition.topPoint = null;
                }
                dialogueConditionList.Add(dialogueCondition);
                //对话数据
                foreach (DialoguePointControl dialoguePointControl in panel.Controls.OfType<DialoguePointControl>())
                {
                    DialoguePointControl topPointControl = dialoguePointControl as DialoguePointControl;
                    if (topPointControl != null)
                    {
                        if (!dialogueValueDic.ContainsKey(topPointControl.GetDialogueValue().dialogueID))
                        {
                            dialogueValueDic.Add(topPointControl.GetDialogueValue().dialogueID, topPointControl.GetDialogueValue());
                        }
                    }
                }
            }
            dialogueAnalysisData.ResetData(dialogueConditionList, dialogueValueDic);
            string dialogueConditionStr, dialogueValueStr;
            dialogueAnalysisData.GetData(out dialogueConditionStr, out dialogueValueStr);
            //保存
            string folderPath = Path.GetDirectoryName(projectPath);
            File.WriteAllText(folderPath + "\\" + dialogueConditionFileName+ ".txt", dialogueConditionStr, Encoding.UTF8);
            File.WriteAllText(folderPath + "\\" + dialogueValueFileName+ ".txt", dialogueValueStr, Encoding.UTF8);
        }

        /// <summary>
        /// 设置（重建）对话节点的子节点数据
        /// </summary>
        /// <param name="parent">对话节点对象</param>
        /// <param name="childControls">子控件</param>
        private void SetDialoguePointData_Child(DialoguePoint parent, params Control[] childControls)
        {
            List<DialoguePoint> tempDialoguePointList = new List<DialoguePoint>();
            foreach (Control childControl in childControls)
            {
                DialoguePointControl pointControl = childControl as DialoguePointControl;
                if (pointControl == null)
                    continue;
                DialoguePoint childPoint = new DialoguePoint();
                childPoint.dialogueID = pointControl.GetDialogueValue().dialogueID;
                tempDialoguePointList.Add(childPoint);
                if (childControl.Tag != null
                    && childControl.Tag.GetType().Equals(typeof(List<Control>))
                    && (childControl.Tag as List<Control>).Count > 0)
                {
                    SetDialoguePointData_Child(childPoint, (childControl.Tag as List<Control>).ToArray());
                }
            }
            parent.childDialoguePoints = tempDialoguePointList.ToArray();
        }

        /// <summary>
        /// 搜索节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 搜索节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchNodeForm.Show(this);
        }

        /// <summary>
        /// 展开深层节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 展开节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iSelectedControlNow != null)
            {
                iSelectedControlNow.GetParent().Controls.OfType<IOpenStop>().ToList().ForEach(temp => temp.OpenControl(new Point(0, 0)));
                ResetControlRect(iSelectedControlNow.GetParent());
            }
        }

        /// <summary>
        /// 收起深层节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 收起节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iSelectedControlNow != null)
            {
                iSelectedControlNow.GetParent().Controls.OfType<IOpenStop>().ToList().ForEach(temp => temp.StopControl(new Point(0, 0)));
                ResetControlRect(iSelectedControlNow.GetParent());
            }
        }

        /// <summary>
        /// 关闭时关闭另一个窗体 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DialogueDataFileEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (searchNodeForm != null)
            {
                searchNodeForm.MustClose();
                searchNodeForm = null;
                this.Close();
            }
        }
    }

    /// <summary>
    /// 控件深度结构
    /// </summary>
    public class DeepControlStruct
    {
        /// <summary>
        /// 该控件的层次深度
        /// </summary>
        public int deep;
        /// <summary>
        /// x轴位置
        /// </summary>
        public int location_x;
        /// <summary>
        /// y轴坐标 
        /// </summary>
        public int location_y;
        /// <summary>
        /// 计算包含子节点在内的高度
        /// </summary>
        public int boundY = -1;
        /// <summary>
        /// 子控件
        /// </summary>
        public List<DeepControlStruct> childs;
        /// <summary>
        /// 父控件
        /// </summary>
        public DeepControlStruct parent;
        /// <summary>
        /// 展开或收起对象
        /// </summary>
        public IOpenStop iOpenStop;
        /// <summary>
        /// 计算
        /// </summary>
        public void InitBoundY()
        {
            int childY = 0;
            foreach (DeepControlStruct child in childs)
            {
                if (child.boundY < 0)
                    child.InitBoundY();
                childY += child.boundY + 15;
            }
            int thisHeight = iOpenStop.OpenStopState ? iOpenStop.OpenSize.Height : iOpenStop.StopSize.Height;
            boundY = childY > thisHeight ? childY : thisHeight;
        }
    }
}


