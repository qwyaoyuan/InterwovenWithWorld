using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

public class RuntimeTasksData
{
    //提供父任务、子任务、检索任务是否互斥、提供当前所有可以做的任务


    private Grapic<RunTimeTaskInfo> Data;


    [JsonIgnore]
    private NameValueCollection exlucsionTaskDic = new NameValueCollection();

    /// <summary
    /// 通过配置文件加载任务
    /// </summary>
    /// <param name="path"></param>
    private void LoadTasks()
    {

        //首次读取
        if (Data == null)
        {
            Data = new Grapic<RunTimeTaskInfo>();
            Data.AllNodes = JsonConvert.DeserializeObject<List<RunTimeTaskInfo>>(Resources.Load<TextAsset>("Data/Task/TasksInitialData").text);
        }


        Data.RootNode = Data.AllNodes.Single(t => t.Parents == null);

        //头结点可达
        Data.RootNode.CanVisit = true;


        //获取所有互斥任务
        string[] allExclusiveTasks =
            Resources.Load<TextAsset>("Data/Task/ExclusiveTasks").text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

        foreach (var exclusiveTaskPair in allExclusiveTasks)
        {
            string[] taskpair = exclusiveTaskPair.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            exlucsionTaskDic.Add(taskpair[0], taskpair[1]);
        }

        //触发互斥任务不可达
        Data.AllNodes.ForEach(n => n.Overd += t =>
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
                        s.CanVisit = false;
                });
            }

        });


    }


    /// <summary>
    /// 序列化完成后挂事件
    /// </summary>
    /// <param name="context"></param>
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        LoadTasks();
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
    public List<RunTimeTaskInfo> GetAllToDoList()
    {
        return Data.GetLastFrameNodes().ToList();
    }


    /// <summary>
    /// 根据npcId获取任务列表
    /// </summary>
    /// <param name="npcId"></param>
    /// <returns></returns>
    public List<RunTimeTaskInfo> GetTasksWithNPC(int npcId)
    {
        return GetAllToDoList().Where(t => t.RunTimeTaskNode.ReceiveTaskNpcId == npcId).ToList();
    }





    public RuntimeTasksData()
    {

    }
}




public class RunTimeTaskInfo : IGraphicNode<RunTimeTaskInfo>
{


    /// <summary>
    /// 任务ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 任务进度
    /// </summary>
    public Enums.EnumTaskProgress TaskProgress { get; private set; }

    private bool isStart;


    [JsonIgnore]
    public  Action<RunTimeTaskInfo> Started;
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
                TaskProgress = Enums.EnumTaskProgress.Started;
                if (Started != null)
                    Started(this);
            }
        }
    }



    private bool isOver;

    [JsonIgnore]
    public  Action<RunTimeTaskInfo> Overd;

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
                TaskProgress = Enums.EnumTaskProgress.Sucessed;
                if (this.Children != null)
                    this.Children.ForEach(t => t.CanVisit = true);
                if (Overd != null)
                    Overd(this);
            }
            isOver = value;

            Visited = value;
        }
    }



    /// <summary>
    /// 放弃任务,主线任务不可放弃,支线任务可以放弃
    /// </summary>
    /// <returns></returns>
    public bool GiveUpTask()
    {
        if (RunTimeTaskNode.TaskType == Enums.TaskType.PrincipalLine)
            return false;
        else
            return true;
    }

    public RunTimeTaskInfo()
    {
        RunTimeTaskNode = new RunTimeTaskNode();
    }


    /// <summary>
    /// 任务节点
    /// </summary>
    [JsonProperty("TaskNode")]
    public RunTimeTaskNode RunTimeTaskNode { get; set; }

    /// <summary>
    /// 当前节点的孩子们
    /// </summary>
    public List<RunTimeTaskInfo> Children { get; set; }

    /// <summary>
    /// 当前节点的父亲们
    /// </summary>
    public List<RunTimeTaskInfo> Parents { get; set; }

    /// <summary>
    /// 是否可以visit
    /// </summary>
    public bool CanVisit { get; set; }

    /// <summary>
    /// 是否已经Visited过了
    /// </summary>
    public bool Visited { get; set; }


    private void EnsureHasChildren()
    {
        if (Children == null)
            Children = new List<RunTimeTaskInfo>();
    }

    private void EnsureHasParents()
    {
        if (Parents == null)
            Parents = new List<RunTimeTaskInfo>();
    }

    /// <summary>
    /// 增加一个孩纸
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(RunTimeTaskInfo child)
    {
        EnsureHasChildren();
        this.Children.Add(child);
    }

    /// <summary>
    /// 增加一个父亲
    /// </summary>
    /// <param name="parent"></param>
    public void AddParent(RunTimeTaskInfo parent)
    {
        EnsureHasParents();
        this.Parents.Add(parent);
    }

    /// <summary>
    /// 增加孩子节点
    /// </summary>
    /// <param name="childs"></param>
    public void AddChildren(params RunTimeTaskInfo[] childs)
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
    public void AddParents(params RunTimeTaskInfo[] parents)
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
    public void RemoveChild(RunTimeTaskInfo child)
    {
        if (this.Children == null || this.Children.Count <= 0) return;
        this.Children.RemoveAll(t => t.ID.Equals(child.ID));

    }

    /// <summary>
    /// 移除父亲节点
    /// </summary>
    /// <param name="parent"></param>
    public void RemoveParent(RunTimeTaskInfo parent)
    {
        if (this.Parents == null || this.Parents.Count <= 0) return;
        this.Parents.RemoveAll(t => t.ID.Equals(parent.ID));
    }

    /// <summary>
    /// 移除孩子节点们
    /// </summary>
    /// <param name="childs"></param>
    public void RemoveChilds(params RunTimeTaskInfo[] childs)
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
    public void RemoveParents(params RunTimeTaskInfo[] parents)
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
public class RunTimeTaskNode
{
    /// <summary>
    /// 任务地点
    /// </summary>
    private Enums.TaskLocation TaskLocation { get; set; }

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitile { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public Enums.TaskType TaskType { get; private set; }

    /// <summary>
    /// 等级限制,>=此等级可以开始任务
    /// </summary>
    private int LevelLimit { get; set; }

    /// <summary>
    /// 性格倾向
    /// </summary>
    private Enums.CharacterTendency ChaTendency { get; set; }

    /// <summary>
    /// 种族类型
    /// </summary>
    private RoleOfRace RoleOfRace { get; set; }

    /// <summary>
    /// 需要的声望
    /// </summary>
    private float NeedReputation { get; set; }


    /// <summary>
    /// 奖励物品
    /// </summary>
    private List<int> AwardGoods { get; set; }

    /// <summary>
    /// 奖励经验
    /// </summary>
    private int AwardExperience { get; set; }

    /// <summary>
    /// 奖励技能点
    /// </summary>
    private int AwardSkillPoint { get; set; }

    /// <summary>
    /// 奖励的声望
    /// </summary>
    private float AwardReputation { get; set; }

    /// <summary>
    /// 接取任务的npc的id
    /// </summary>
    public int ReceiveTaskNpcId { get; private set; }

    /// <summary>
    /// 交付任务的npc的id
    /// </summary>
    public int DeliveryTaskNpcId { get; private set; }


    /// <summary>
    /// 杀死某怪物指定数量
    /// </summary>
    [JsonProperty("KillMosterCount")]
    public Dictionary<int, int> HaveKilledMonsterCount { get; set; }


    /// <summary>
    /// 已经获取的物品数量
    /// </summary>
    [JsonProperty("GetGoodsCount")]
    public Dictionary<int, int> HaveGetGoodsCount { get; set; }


    /// <summary>
    /// 到达指定区域 ,Vector.zero
    /// </summary>
    [JsonProperty("Position")]
    public Vector3 NowArrivedPosition { get; set; }


    /// <summary>
    /// 已经经过的时间
    /// </summary>
    [JsonProperty("Time")]
    public int TimeElasped
    {
        get;
        set;
    }



}







