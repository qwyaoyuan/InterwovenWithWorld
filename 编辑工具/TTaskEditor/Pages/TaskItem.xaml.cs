using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TTaskEditor.Data;
using TTaskEditor.Helper;

namespace TTaskEditor.Pages
{
    /// <summary>
    /// Interaction logic for TaskItem.xaml
    /// </summary>
    public partial class TaskItem : UserControl
    {
        public TaskItem()
        {
            InitializeComponent();
            this.PreviewMouseLeftButtonDown += TaskItem_MouseDown;
            string taskTitle = "任务:" + RandomId.GetRandomId();
            this.TitleExpander.Header = taskTitle;
        }

        /// <summary>
        /// 点击后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationState.MouseState == MouseState.MakeTranstion)
            {
                TitleExpander.IsExpanded = false;
                ApplicationState.TransationToElement = TitleExpander;
                e.Handled = true;
            }
        }

        public int TaskID
        {
            get
            {
                string id = this.TitleExpander.Header.ToString()
                  .Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
                return int.Parse(id);
            }
        }

        /// <summary>
        /// 与其他窗口建立关系链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeTransation(object sender, RoutedEventArgs e)
        {
            ApplicationState.MouseState = MouseState.MakeTranstion;
            ApplicationState.TransationFromElement = TitleExpander;
        }

        public MetaTaskInfo MetaTaskInfo
        {
            get
            {
                MetaTaskInfo metaTaskInfo = new MetaTaskInfo();
                string id = this.TitleExpander.Header.ToString()
                    .Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
                metaTaskInfo.ID = int.Parse(id);
                //metaTaskInfo.MetaTaskNode.ArriveAssignPosition
                if (string.IsNullOrEmpty(GoToAssgedPos.Text))
                {
                    metaTaskInfo.MetaTaskNode.ArriveAssignPosition = new Vector3(0, 0, 0);

                }
                else
                {
                    try
                    {
                        string[] strs = GoToAssgedPos.Text.Split(new char[] { ',' });
                        metaTaskInfo.MetaTaskNode.ArriveAssignPosition = new Vector3(float.Parse(strs[0]), float.Parse(strs[1]),
                            float.Parse(strs[2]));
                    }
                    catch
                    {
                        MessageBox.Show("位置格式有误,格式应该为x,y,z");
                        return null;
                    }
                }
                try
                {
                    metaTaskInfo.MetaTaskNode.AwardExperience = int.Parse(AwardExperience.Text);
                }
                catch
                {
                    MessageBox.Show("奖励经验格式有误，应该是整型");
                    return null;
                }
                if (string.IsNullOrEmpty(AwardThing.Text))
                    metaTaskInfo.MetaTaskNode.AwardGoods = new List<int>();
                else
                {
                    try
                    {
                        string[] strs = AwardThing.Text.Split(new char[] { ',' });
                        metaTaskInfo.MetaTaskNode.AwardGoods = new List<int>();

                        foreach (var award in strs)
                        {
                            metaTaskInfo.MetaTaskNode.AwardGoods.Add(int.Parse(award));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("奖励物品格式有误有误,格式应该为物品id1,物品id2,物品id3");
                        return null;
                    }
                }
                try
                {
                    metaTaskInfo.MetaTaskNode.AwardReputation = float.Parse(AwardReputation.Text);
                }
                catch
                {
                    MessageBox.Show("奖励声望格式有误");
                    return null;
                }
                try
                {
                    metaTaskInfo.MetaTaskNode.AwardSkillPoint = int.Parse(SkillPoint.Text);
                }
                catch
                {
                    MessageBox.Show("奖励技能点格式有误");
                    return null;
                }

                metaTaskInfo.MetaTaskNode.ChaTendency = (CharacterTendency)CharaTendency.SelectedIndex;
                if (string.IsNullOrEmpty(DeliveryTaskNpcId.Text))
                    metaTaskInfo.MetaTaskNode.DeliveryTaskNpcId = -1;
                else
                {
                    try
                    {
                        metaTaskInfo.MetaTaskNode.DeliveryTaskNpcId = int.Parse(DeliveryTaskNpcId.Text);
                    }
                    catch
                    {
                        MessageBox.Show("交付任务的npcid输入有误");
                        return null;
                    }
                }


                if (String.IsNullOrEmpty(GetAssignedThing.Text))
                    metaTaskInfo.MetaTaskNode.GetGoodsAssignCount = new Dictionary<int, int>();
                else
                {
                    try
                    {
                        string[] strs = GetAssignedThing.Text.Split(new char[] { ',' });
                        metaTaskInfo.MetaTaskNode.GetGoodsAssignCount = new Dictionary<int, int>();
                        for (int i = 0; i < strs.Length; i += 2)
                        {
                            metaTaskInfo.MetaTaskNode.GetGoodsAssignCount.Add(int.Parse(strs[i]), int.Parse(strs[i + 1]));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("获取指定物品的格式有误，应该是  物品id,数量,物品id,数量");
                        return null;
                    }
                }
                if (string.IsNullOrEmpty(KillAssignedMosetrCount.Text))
                    metaTaskInfo.MetaTaskNode.KillMonsterAssignCount = new Dictionary<int, int>();
                else
                {
                    try
                    {
                        string[] strs = KillAssignedMosetrCount.Text.Split(new char[] { ',' });
                        metaTaskInfo.MetaTaskNode.KillMonsterAssignCount = new Dictionary<int, int>();
                        for (int i = 0; i < strs.Length; i += 2)
                        {
                            metaTaskInfo.MetaTaskNode.KillMonsterAssignCount.Add(int.Parse(strs[i]), int.Parse(strs[i + 1]));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("杀死怪物指定数量格式有误，应该是  怪物id,数量,怪物id,数量");
                        return null;
                    }
                }
                try
                {
                    metaTaskInfo.MetaTaskNode.LevelLimit = int.Parse(LvlLimit.Text);

                }

                catch
                {
                    MessageBox.Show("级别限制输入有误,应为整型数字");
                    return null;
                }
                metaTaskInfo.MetaTaskNode.TaskType = (TaskType)TaskType.SelectedIndex;
                metaTaskInfo.MetaTaskNode.RoleOfRace = (RoleOfRace)RoleOfRace.SelectedIndex;
                try
                {
                    metaTaskInfo.MetaTaskNode.NeedReputation = float.Parse(NeedReputation.Text);
                }
                catch
                {
                    MessageBox.Show("需要的声望格式输入有误,float类型");
                    return null;
                }
                if (string.IsNullOrEmpty(ReceiveTaskNPCID.Text))
                    metaTaskInfo.MetaTaskNode.ReceiveTaskNpcId = -1;
                else
                {
                    try
                    {
                        metaTaskInfo.MetaTaskNode.ReceiveTaskNpcId = int.Parse(ReceiveTaskNPCID.Text);
                    }
                    catch
                    {
                        MessageBox.Show("接收任务的npcid输入有误，整型");
                        return null;
                    }
                }

                if (string.IsNullOrEmpty(TimeLimit.Text))
                    metaTaskInfo.MetaTaskNode.TimeLimit = -1;
                else
                {
                    try
                    {
                        metaTaskInfo.MetaTaskNode.TimeLimit = int.Parse(TimeLimit.Text);
                    }
                    catch
                    {
                        MessageBox.Show("级别限制输入有误，整型");
                        return null;
                    }
                }
                //任务场景名
                if (string.IsNullOrEmpty(TaskSceneName.Text))
                {
                    MessageBox.Show("场景名称没输入");
                    return null;

                }
                string sceneName = TaskSceneName.Text;
                string[] scenePosStrs = TaskPosition.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                float tempnum;
                if (scenePosStrs.Length != 3 || scenePosStrs.Any(pos => !float.TryParse(pos, out tempnum)))
                {
                    MessageBox.Show("场景坐标输入有误");
                    return null;
                }

                //任务场景内坐标
                Vector3 scenePos = new Vector3(float.Parse(scenePosStrs[0]), float.Parse(scenePosStrs[1]), float.Parse(scenePosStrs[2]));

                //任务场景内浮动范围
                if (!Tool.IsInt(TaskPositionRadius.Text))
                {
                    MessageBox.Show("场景内浮动坐标填写有误");
                    return null;
                }
                metaTaskInfo.MetaTaskNode.TaskLocation = new TaskLocation() { SceneName = sceneName, ArrivedCenterPos = scenePos, Radius = int.Parse(TaskPositionRadius.Text) };
                if (string.IsNullOrEmpty(TaskTitle.Text))
                {
                    MessageBox.Show("任务标题没有填写");
                    return null;
                }
                metaTaskInfo.MetaTaskNode.TaskTitile = TaskTitle.Text;
                //任务标题
                return metaTaskInfo;
            }
            set
            {
                this.TitleExpander.Header = value.ID;
                string[] nodePropities = value.MetaTaskNode.ToString().Split(new char[] { '|' });
                this.TaskType.SelectedIndex = int.Parse(nodePropities[0]);
                this.LvlLimit.Text = nodePropities[1];
                this.CharaTendency.SelectedIndex = int.Parse(nodePropities[2]);
                this.RoleOfRace.SelectedIndex = int.Parse(nodePropities[3]);
                this.NeedReputation.Text = nodePropities[4];
                this.AwardThing.Text = nodePropities[5];
                this.AwardExperience.Text = nodePropities[6];
                this.SkillPoint.Text = nodePropities[7];
                this.AwardReputation.Text = nodePropities[8];
                this.ReceiveTaskNPCID.Text = nodePropities[9];
                this.DeliveryTaskNpcId.Text = nodePropities[10];
                this.KillAssignedMosetrCount.Text = nodePropities[11];
                this.GetAssignedThing.Text = nodePropities[12];
                this.GoToAssgedPos.Text = nodePropities[13];
                this.TimeLimit.Text = nodePropities[14];
            }
        }
    }
}
