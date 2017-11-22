using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// 提供任务信息
/// </summary>
public class MetaTasksData : ILoadable<MetaTasksData>
{

    private Grapic<MetaTaskInfo> Data;

    /// <summary
    /// 通过配置文件加载任务
    /// </summary>
    /// <param name="path"></param>
    private void LoadTasks()
    {

        Data = new Grapic<MetaTaskInfo>();

        Data.AllNodes = JsonConvert.DeserializeObject<List<MetaTaskInfo>>(Resources.Load<TextAsset>("Data/Task/MetaTasksData").text);
        //设置头结点

        Data.RootNode = Data.AllNodes.Single(t => t.Parents == null);

        //头结点可达
        Data.RootNode.CanVisit = true;
        //其余结点全部可达
        Data.AllNodes.Where(t => t.ID != Data.RootNode.ID).ToList().ForEach(t => t.CanVisit = true);

    }

    /// <summary>
    /// 根据任务id获取任务元信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MetaTaskInfo this[int id]
    {

        get { return Data.AllNodes.FirstOrDefault(mt => mt.ID == id); }
    }




    public void Load()
    {
        LoadTasks();
    }
}




public class MetaTaskInfo : IGraphicNode<MetaTaskInfo>
{

    /// <summary>
    /// 任务ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 任务进度
    /// </summary>
    public Enums.EnumTaskProgress TaskProgress { get; private set; }


    public MetaTaskInfo()
    {
        MetaTaskNode = new MetaTaskNode();
    }


    /// <summary>
    /// 任务节点
    /// </summary>
    [JsonProperty("TaskNode")]
    public MetaTaskNode MetaTaskNode { get; set; }

    /// <summary>
    /// 当前节点的孩子们
    /// </summary>
    public List<MetaTaskInfo> Children { get; set; }

    /// <summary>
    /// 当前节点的父亲们
    /// </summary>
    public List<MetaTaskInfo> Parents { get; set; }

    /// <summary>
    /// 是否可以visit
    /// </summary>
    public bool CanVisit { get; set; }

    /// <summary>
    /// 是否已经Visted完成
    /// </summary>
    public bool Visited { get; set; }




    private void EnsureHasChildren()
    {
        if (Children == null)
            Children = new List<MetaTaskInfo>();
    }



    private void EnsureHasParents()
    {
        if (Parents == null)
            Parents = new List<MetaTaskInfo>();
    }


    /// <summary>
    /// 增加一个孩纸
    /// </summary>
    /// <param name="child"></param>
    public void AddChild(MetaTaskInfo child)
    {
        EnsureHasChildren();
        this.Children.Add(child);
    }

    /// <summary>
    /// 增加一个父亲
    /// </summary>
    /// <param name="parent"></param>
    public void AddParent(MetaTaskInfo parent)
    {
        EnsureHasParents();
        this.Parents.Add(parent);
    }

    /// <summary>
    /// 增加孩子节点
    /// </summary>
    /// <param name="childs"></param>
    public void AddChildren(params MetaTaskInfo[] childs)
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
    public void AddParents(params MetaTaskInfo[] parents)
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
    public void RemoveChild(MetaTaskInfo child)
    {
        if (this.Children == null || this.Children.Count <= 0) return;
        this.Children.RemoveAll(t => t.ID.Equals(child.ID));

    }

    /// <summary>
    /// 移除父亲节点
    /// </summary>
    /// <param name="parent"></param>
    public void RemoveParent(MetaTaskInfo parent)
    {
        if (this.Parents == null || this.Parents.Count <= 0) return;
        this.Parents.RemoveAll(t => t.ID.Equals(parent.ID));
    }

    /// <summary>
    /// 移除孩子节点们
    /// </summary>
    /// <param name="childs"></param>
    public void RemoveChilds(params MetaTaskInfo[] childs)
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
    public void RemoveParents(params MetaTaskInfo[] parents)
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
public class MetaTaskNode
{
    /// <summary>
    /// 任务地点
    /// </summary>
    public Enums.TaskLocation TaskLocation { get; set; }

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitile { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public Enums.TaskType TaskType { get; set; }

    /// <summary>
    /// 等级限制,>=此等级可以开始任务
    /// </summary>
    public int LevelLimit { get; set; }

    /// <summary>
    /// 性格倾向
    /// </summary>
    public Enums.CharacterTendency ChaTendency { get; set; }

    /// <summary>
    /// 种族类型
    /// </summary>
    public RoleOfRace RoleOfRace { get; set; }



    /// <summary>
    /// 需要的声望
    /// </summary>
    [JsonProperty("Reputation")]
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
    /// 需要杀死的怪物数量
    /// </summary>
    [JsonProperty("KillMosterCount")]
    public Dictionary<int, int> NeedKillMonsterCount { get; set; }


    /// <summary>
    /// 需要获取物品的数量
    /// </summary>
    [JsonProperty("GetGoodsCount")]
    public Dictionary<int, int> NeedGetGoodsCount { get; set; }


    /// <summary>
    /// 需要到达指定区域 ,Vector.zero
    /// </summary>
    [JsonProperty("Position")]
    public Vector3  NeedArrivedPosition { get; set; }


    /// <summary>
    /// 经过的时间
    /// </summary>
    [JsonProperty("Time")]
    public int TimeLimit
    {
        get;
        set;
    }

}









