using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tasks
{
    //提供父任务、子任务、检索任务是否互斥、提供当前所有可以做的任务

    private static Tasks _instance;

    public static Tasks Instance
    {
        get
        {

            return _instance ?? (_instance = new Tasks());
        }
    }

    private Grapic<TaskInfo> Data;

    public Tasks()
    {
        LoadTasks(Resources.Load<TextAsset>("Data/Task/Tasks").text.Split(new string[]{ "\r\n" },StringSplitOptions.None));
    }

    private NameValueCollection exlucsionTaskDic = new NameValueCollection();
    /// <summary>
    /// 通过配置文件加载任务
    /// </summary>
    /// <param name="path"></param>
    private void LoadTasks(string[] allLines)
    {

        Data = new Grapic<TaskInfo>();

        for (int i = 0; i < allLines.Length; i += 4)
        {
            TaskInfo taskInfo = new TaskInfo();
            taskInfo.Deserialze(new string[] { allLines[i], allLines[i + 1], allLines[i + 2], allLines[i + 3] });
            Data.AllNodes.Add(taskInfo);
        }
        //填充

        for (int i = 0; i < Data.AllNodes.Count; i++)
        {
            if (Data.AllNodes[i].Children != null)
            {
                for (int j = 0; j < Data.AllNodes[i].Children.Count; j++)
                {
                    Data.AllNodes[i].Children[j] =
                        Data.AllNodes.Find(t => t.ID.Equals(Data.AllNodes[i].Children[j].ID));
                }
            }
            if (Data.AllNodes[i].Parents != null)
            {
                for (int j = 0; j < Data.AllNodes[i].Parents.Count; j++)
                {
                    Data.AllNodes[i].Parents[j] =
                        Data.AllNodes.Find(t => t.ID.Equals(Data.AllNodes[i].Parents[j].ID));
                }
            }
        }
        //设置头结点

        Data.RootNode = Data.AllNodes.Single(t => t.Parents == null);

        //头结点可达
        Data.RootNode.CanVisit = r => true;
        //其余结点全部不可达
        Data.AllNodes.Where(t => t.ID != Data.RootNode.ID).ToList().ForEach(t => t.CanVisit = tt => false);

        //获取所有互斥任务
        string[] allExclusiveTasks =
            Resources.Load<TextAsset>("Data/Task/ExclusiveTasks").text.Split(new string[] { "\r\n" }, StringSplitOptions.None); 
        foreach (var exclusiveTaskPair in allExclusiveTasks)
        {
            string[] taskpair = exclusiveTaskPair.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            exlucsionTaskDic.Add(taskpair[0], taskpair[1]);
        }

        //触发互斥任务不可达
        Data.AllNodes.ForEach(n => n.Stated += t =>
        {
            if (t.Parents == null || t.Parents.Count != 1) return;
            var sibling = t.Parents[0].Children.Where(c => c.ID != n.ID);
            if (exlucsionTaskDic.AllKeys.Contains<string>(t.ID.ToString()))
            {
                string[] exclusiveIds = exlucsionTaskDic.GetValues(t.ID.ToString());
                if (exclusiveIds == null) return;
                sibling.ToList().ForEach(s =>
                {
                    if (exclusiveIds.Contains(s.ID.ToString()))
                        s.CanVisit = ss => false;
                });
            }

        });


        //testCode
        //firstLevel
        //var firstTask = GetAllToDoList();
        //firstTask[0].IsOver = true;


        //var secondTasks = GetAllToDoList();
        //bool isexluseSive = IsTaskExclusive(secondTasks[0].ID, secondTasks[1].ID);

        //secondTasks[0].IsStart = true;
        //var now = GetAllToDoList();
        //secondTasks[0].IsOver = true;


        ////var thirdTasks = GetAllToDoList();
        ////thirdTasks[0].IsOver = true;

    }

    /// <summary>
    /// 两任务是否互斥
    /// </summary>
    /// <param name="id1">任务1id</param>
    /// <param name="id2">任务2id</param>
    /// <returns></returns>
    public bool IsTaskExclusive(int id1, int id2)
    {
        if (exlucsionTaskDic.AllKeys.Contains(id1.ToString()))
        {
            if (exlucsionTaskDic.GetValues(id1.ToString()).Contains(id2.ToString()))
                return true;
        }
        if (exlucsionTaskDic.AllKeys.Contains(id2.ToString()))
        {
            if (exlucsionTaskDic.GetValues(id2.ToString()).Contains(id1.ToString()))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取当前所有可做任务
    /// </summary>
    /// <returns></returns>
    public List<TaskInfo> GetAllToDoList()
    {
        return Data.GetLastFrameNodes().ToList();
    }
}

public class Grapic<T> where T : IGraphicNode<T>
{

    /// <summary>
    /// root task没有parent
    /// </summary>
    public T RootNode { get; set; }


    public Grapic(T root)
    {
        this.RootNode = root;
    }

    public Grapic()
    {

    }

    public List<T> AllNodes = new List<T>();

    /// <summary>
    /// BFS遍历
    /// </summary>
    /// <param name="action"></param>
    public void BFSTransfer(Action<T> action)
    {
        if (RootNode == null) return;
        List<T> hasTransferNode = new List<T>();
        Queue<T> remainNode = new Queue<T>();
        remainNode.Enqueue(RootNode);
        while (remainNode.Count > 0)
        {
            T currentTransferNode = remainNode.Dequeue();
            action(currentTransferNode);
            if (currentTransferNode.Children == null) continue;
            foreach (var child in currentTransferNode.Children)
            {
                if (!hasTransferNode.Contains(currentTransferNode) && (child.CanVisit == null || child.CanVisit(child)))
                {
                    hasTransferNode.Add(currentTransferNode);
                    remainNode.Enqueue(child);
                }
            }
        }

    }



    /// <summary>
    /// 获取所有节点
    /// </summary>
    /// <returns></returns>
    public T[] GetAllNode()
    {
        return AllNodes.ToArray();
    }

    /// <summary>
    /// DFS遍历
    /// </summary>
    /// <param name="action"></param>
    public void DFSTransfer(Action<T> action)
    {
        hasDFSNode.Clear();
        DFS(action, this.RootNode);
    }


    /// <summary>
    /// 得到最后一层节点
    /// </summary>
    /// <returns></returns>
    public T[] GetLastFrameNodes()
    {
        T[] allNode = GetAllNode();
        Func<T, bool> filter = node => node.CanVisit != null && node.CanVisit(node) && node.Children != null && node.Children.All(n => n != null && n.CanVisit != null && n.CanVisit(n) == false);
        return allNode.Where(filter).ToArray();
    }


    List<T> hasDFSNode = new List<T>();


    private void DFS(Action<T> action, T startNode)
    {
        if (startNode == null) return;
        action(startNode);
        hasDFSNode.Add(startNode);
        if (startNode.Children == null) return;
        foreach (T childNode in startNode.Children)
        {
            if (hasDFSNode.Contains(childNode) || (hasDFSNode != null && childNode.CanVisit != null && childNode.CanVisit(childNode) == false))
                continue;
            DFS(action, childNode);
        }
    }
}

public interface IGraphicNode<T>
{
    List<T> Children { get; set; }

    List<T> Parents { get; set; }

    Predicate<T> CanVisit { get; set; }

    void Serialize(StreamWriter sw);

    void Deserialze(string[] strs);

}

public class TaskInfo : IGraphicNode<TaskInfo>
{

    /// <summary>
    /// 任务ID
    /// </summary>
    public int ID { get; set; }

    private bool isStart;


    public event Action<TaskInfo> Stated;
    /// <summary>
    /// 是否开始,即接取任务
    /// </summary>
    public bool IsStart
    {
        get { return isStart; }
        set
        {
            if (value)
            {
                if (Stated != null)
                    Stated(this);
            }
        }
    }

    private bool isOver;


    /// <summary>
    /// 当前任务是否完成
    /// </summary>
    public bool IsOver
    {
        get { return isOver; }
        set
        {
            if (value)
            {
                if (this.Children != null)
                    this.Children.ForEach(t => t.CanVisit = tt => true);
            }
            isOver = value;
        }
    }



    public TaskInfo()
    {
        TaskNode = new TaskNode();
    }
    /// <summary>
    /// 任务节点
    /// </summary>
    public TaskNode TaskNode { get; set; }

    /// <summary>
    /// 当前节点的孩子们
    /// </summary>
    public List<TaskInfo> Children { get; set; }

    /// <summary>
    /// 当前节点的父亲们
    /// </summary>
    public List<TaskInfo> Parents { get; set; }

    /// <summary>
    /// 是否可以visit
    /// </summary>
    public Predicate<TaskInfo> CanVisit { get; set; }

    public void Serialize(StreamWriter sw)
    {
        sw.WriteLine(ID);
        //首先写入任务节点
        sw.WriteLine(TaskNode.ToString());
        //写入父亲：
        string parentStr = string.Empty;
        if (Parents == null || Parents.Count == 0)
        {
            sw.WriteLine();
        }
        else
        {
            for (int i = 0; i < Parents.Count; i++)
            {
                if (i == 0)
                    parentStr += Parents[i].ID;
                else
                    parentStr += "," + Parents[i].ID;
            }
            sw.WriteLine(parentStr);
        }
        //写入孩子：
        string childStr = string.Empty;
        if (Children == null || Children.Count == 0)
        {
            sw.WriteLine();
        }
        else
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == 0)
                    childStr += Children[i].ID;
                else
                    childStr += "," + Children[i].ID;
            }
            sw.WriteLine(childStr);
        }
    }


    public void Deserialze(string[] strs)
    {
        this.ID = int.Parse(strs[0]);
        this.TaskNode = new TaskNode();
        this.TaskNode.FromStr(strs[1]);
        //反序列化父亲,此时仅仅包含父亲id，父亲实体没有填充

        if (string.IsNullOrEmpty(strs[2]))
            this.Parents = null;
        else
        {
            string[] parents = strs[2].Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in parents)
            {
                TaskInfo t = new TaskInfo();
                t.ID = int.Parse(p);
                this.AddParent(t);
            }
        }
        //反序列化孩子，此时仅仅包含孩子id，孩子实体没有填充
        if (string.IsNullOrEmpty(strs[3]))
            this.Children = null;
        else
        {
            string[] childs = strs[3].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var c in childs)
            {
                TaskInfo t = new TaskInfo();
                t.ID = int.Parse(c);
                this.AddChild(t);
            }
        }

    }



    private void EnsureHasChildren()
    {
        if (Children == null)
            Children = new List<TaskInfo>();
    }

    private void EnsureHasParents()
    {
        if (Parents == null)
            Parents = new List<TaskInfo>();
    }

    /// <summary>
    /// 增加一个孩纸
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(TaskInfo child)
    {
        EnsureHasChildren();
        this.Children.Add(child);
    }

    /// <summary>
    /// 增加一个父亲
    /// </summary>
    /// <param name="parent"></param>
    public void AddParent(TaskInfo parent)
    {
        EnsureHasParents();
        this.Parents.Add(parent);
    }

    /// <summary>
    /// 增加孩子节点
    /// </summary>
    /// <param name="childs"></param>
    public void AddChildren(params TaskInfo[] childs)
    {
        foreach (var taskNode in childs)
        {
            AddChild(taskNode);
        }
    }

    /// <summary>
    /// 增加父亲节点
    /// </summary>
    /// <param name="parents"></param>
    public void AddParents(params  TaskInfo[] parents)
    {
        foreach (var taskNode in parents)
        {
            this.Parents.Add(taskNode);
        }
    }

    /// <summary>
    /// 移除孩子节点
    /// </summary>
    /// <param name="child"></param>
    public void RemoveChild(TaskInfo child)
    {
        if (this.Children == null || this.Children.Count <= 0) return;
        this.Children.RemoveAll(t => t.ID.Equals(child.ID));

    }

    /// <summary>
    /// 移除父亲节点
    /// </summary>
    /// <param name="parent"></param>
    public void RemoveParent(TaskInfo parent)
    {
        if (this.Parents == null || this.Parents.Count <= 0) return;
        this.Parents.RemoveAll(t => t.ID.Equals(parent.ID));
    }

    /// <summary>
    /// 移除孩子节点们
    /// </summary>
    /// <param name="childs"></param>
    public void RemoveChilds(params TaskInfo[] childs)
    {
        foreach (var taskNode in childs)
        {
            RemoveChild(taskNode);
        }
    }

    /// <summary>
    /// 移动父亲节点们
    /// </summary>
    /// <param name="parents"></param>
    public void RemoveParents(params TaskInfo[] parents)
    {
        foreach (var taskNode in parents)
        {
            RemoveParent(taskNode);
        }
    }



}

/// <summary>
/// 任务节点
/// </summary>
public class TaskNode
{

    /// <summary>
    /// 任务类型
    /// </summary>
    public TaskType TaskType { get; set; }

    /// <summary>
    /// 等级限制,>=此等级可以开始任务
    /// </summary>
    public int LevelLimit { get; set; }

    /// <summary>
    /// 性格倾向
    /// </summary>
    public CharacterTendency ChaTendency { get; set; }

    /// <summary>
    /// 种族类型
    /// </summary>
    public RoleOfRace RoleOfRace { get; set; }

    /// <summary>
    /// 需要的声望
    /// </summary>
    public float NeedReputation { get; set; }

    /// <summary>
    /// 奖励物品
    /// </summary>
    public List<int> AwardGoods { get; set; }

    /// <summary>
    /// 奖励经验
    /// </summary>
    public int AwardExperience { get; set; }

    /// <summary>
    /// 奖励技能点
    /// </summary>
    public int AwardSkillPoint { get; set; }

    /// <summary>
    /// 奖励的声望
    /// </summary>
    public float AwardReputation { get; set; }

    /// <summary>
    /// 接取任务的npc的id
    /// </summary>
    public int ReceiveTaskNpcId { get; set; }

    /// <summary>
    /// 交付任务的npc的id
    /// </summary>
    public int DeliveryTaskNpcId { get; set; }

    /// <summary>
    /// 杀死某怪物指定数量
    /// </summary>
    public Dictionary<int, int> KillMonsterAssignCount { get; set; }

    /// <summary>
    /// 获取物品指定数量
    /// </summary>
    public Dictionary<int, int> GetGoodsAssignCount { get; set; }

    /// <summary>
    /// 到达指定区域 ,Vector.zero
    /// </summary>
    public Vector3 ArriveAssignPosition { get; set; }


    /// <summary>
    /// 时间限制
    /// </summary>
    public int TimeLimit
    {
        get;
        set;
    }


    public override string ToString()
    {
        string awardGoodsStr = string.Empty;
        for (int i = 0; i < AwardGoods.Count; i++)
        {
            if (i == awardGoodsStr.Length - 1)
                awardGoodsStr += AwardGoods[i].ToString();
        }
        Func<Dictionary<int, int>.Enumerator, string> toStr = dicE =>
        {
            string str = string.Empty;
            if (dicE.MoveNext())
            {
                str += dicE.Current.Key + "," + dicE.Current.Value;
            }
            while (dicE.MoveNext())
            {
                str += "," + dicE.Current.Key + "," +
                       dicE.Current.Value;
            }
            return str;
        };

        var killMosterEnumerator = KillMonsterAssignCount.GetEnumerator();
        var getGoodsAssignCountEnumerator = GetGoodsAssignCount.GetEnumerator();
        string killMosterStr = toStr(killMosterEnumerator);
        string getGoodsAssignCountStr = toStr(getGoodsAssignCountEnumerator);

        string spliter = "|";
        string nodeStr = (int)TaskType + spliter + LevelLimit + spliter + (int)ChaTendency +
                         spliter + (int)RoleOfRace
                         + spliter + NeedReputation + spliter + awardGoodsStr + spliter + AwardExperience + spliter +
                         AwardSkillPoint + spliter + AwardReputation + spliter + ReceiveTaskNpcId + spliter +
                         DeliveryTaskNpcId + spliter + killMosterStr
                         + spliter + getGoodsAssignCountStr + spliter + ArriveAssignPosition.ToString() + spliter +
                         TimeLimit;


        return nodeStr;
    }


    public void FromStr(string str)
    {
        string[] nodeStr = str.Split(new char[] { '|' });

        TaskType = (TaskType)int.Parse(nodeStr[0]);
        LevelLimit = int.Parse(nodeStr[1]);
        ChaTendency = (CharacterTendency)int.Parse(nodeStr[2]);
        RoleOfRace = (global::RoleOfRace)int.Parse(nodeStr[3]);
        NeedReputation = float.Parse(nodeStr[4]);
        string awardGoodStr = nodeStr[5];
        AwardGoods = new List<int>();
        foreach (var awardGood in awardGoodStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            AwardGoods.Add(Int32.Parse(awardGood));
        }
        AwardExperience = int.Parse(nodeStr[6]);
        AwardSkillPoint = int.Parse(nodeStr[7]);
        AwardReputation = float.Parse(nodeStr[8]);
        ReceiveTaskNpcId = int.Parse(nodeStr[9]);
        DeliveryTaskNpcId = int.Parse(nodeStr[10]);
        string killMonsterAssignCountStr = nodeStr[11];
        string[] killMosterStrs = killMonsterAssignCountStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        KillMonsterAssignCount = new Dictionary<int, int>();
        for (int i = 0; i < killMosterStrs.Length; i += 2)
        {
            KillMonsterAssignCount.Add(int.Parse(killMosterStrs[i]), int.Parse(killMosterStrs[i + 1]));
        }
        string getGoodsAssignCountStr = nodeStr[12];
        string[] getGoodsAssignCountStrs = getGoodsAssignCountStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        GetGoodsAssignCount = new Dictionary<int, int>();
        for (int i = 0; i < getGoodsAssignCountStrs.Length; i += 2)
        {
            GetGoodsAssignCount.Add(int.Parse(getGoodsAssignCountStrs[i]), int.Parse(getGoodsAssignCountStrs[i + 1]));
        }
        string arriveAssignPositionstr = nodeStr[13];
        string[] arriveAssignPositionStrs = arriveAssignPositionstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        ArriveAssignPosition = new Vector3(0, 0, 0);
        if (arriveAssignPositionStrs.Length == 3)
        {
            var arriveAssignPosition = ArriveAssignPosition;
            arriveAssignPosition.x = float.Parse(arriveAssignPositionStrs[0]);
            arriveAssignPosition.y = float.Parse(arriveAssignPositionStrs[1]);
            arriveAssignPosition.z = float.Parse(arriveAssignPositionStrs[2]);
            ArriveAssignPosition = arriveAssignPosition;
        }
        TimeLimit = int.Parse(nodeStr[14]);
    }

}


/// <summary>
/// 任务类型
/// </summary>
public enum TaskType
{
    /// <summary>
    /// 主线任务
    /// </summary>
    PrincipalLine,

    /// <summary>
    /// 支线任务
    /// </summary>
    BranchLine,

    /// <summary>
    /// 重复性任务
    /// </summary>
    Repeat,

    /// <summary>
    /// 随机任务
    /// </summary>
    Random
}

/// <summary>
/// 性格倾向
/// </summary>
public enum CharacterTendency
{

    None,
    /// <summary>
    /// 好杀戮的
    /// </summary>
    Slaughterous,

    /// <summary>
    /// 和平的
    /// </summary>
    Peaceable,
}





