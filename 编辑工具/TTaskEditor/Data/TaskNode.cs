using System;
using System.Collections.Generic;

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
            if (i == awardGoodsStr.Length-1)
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
        string nodeStr =  (int)TaskType + spliter + LevelLimit + spliter + (int)ChaTendency +
                         spliter + (int)RoleOfRace
                         + spliter + NeedReputation + spliter + awardGoodsStr + spliter+AwardExperience+spliter+
                         AwardSkillPoint+spliter+AwardReputation+spliter+ ReceiveTaskNpcId + spliter +
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
        foreach (var awardGood in awardGoodStr.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
        {
            AwardGoods.Add(Int32.Parse(awardGood));
        }
        AwardExperience = int.Parse(nodeStr[6]);
        AwardSkillPoint = int.Parse(nodeStr[7]);
        AwardReputation = float.Parse(nodeStr[8]);
        ReceiveTaskNpcId = int.Parse(nodeStr[9]);
        DeliveryTaskNpcId = int.Parse(nodeStr[10]);
        string killMonsterAssignCountStr = nodeStr[11];
        string[] killMosterStrs = killMonsterAssignCountStr.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
        KillMonsterAssignCount = new Dictionary<int, int>();
        for (int i = 0; i < killMosterStrs.Length; i += 2)
        {
            KillMonsterAssignCount.Add(int.Parse(killMosterStrs[i]), int.Parse(killMosterStrs[i + 1]));
        }
        string getGoodsAssignCountStr = nodeStr[12];
        string[] getGoodsAssignCountStrs = getGoodsAssignCountStr.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
        GetGoodsAssignCount = new Dictionary<int, int>();
        for (int i = 0; i < getGoodsAssignCountStrs.Length; i += 2)
        {
            GetGoodsAssignCount.Add(int.Parse(getGoodsAssignCountStrs[i]), int.Parse(getGoodsAssignCountStrs[i + 1]));
        }
        string arriveAssignPositionstr = nodeStr[13];
        string[] arriveAssignPositionStrs = arriveAssignPositionstr.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
        ArriveAssignPosition = new Vector3(0, 0, 0);
        if (arriveAssignPositionStrs.Length == 3)
        {
            ArriveAssignPosition.X = float.Parse(arriveAssignPositionStrs[0]);
            ArriveAssignPosition.Y = float.Parse(arriveAssignPositionStrs[1]);
            ArriveAssignPosition.Z = float.Parse(arriveAssignPositionStrs[2]);
        }
        TimeLimit = int.Parse(nodeStr[14]);
    }

}

public class Vector3
{
    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }


    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return X.ToString() + "," + Y.ToString() + "," + Z.ToString();
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
