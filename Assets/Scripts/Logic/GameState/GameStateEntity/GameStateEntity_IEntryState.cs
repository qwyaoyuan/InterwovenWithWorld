using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 实现了IEntryState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IEntryState
{
    /// <summary>
    /// 词条集合对象 
    /// </summary>
    EntryData entryData;

    /// <summary>
    /// ID对应的词条数据字典
    /// </summary>
    Dictionary<int, EntryDataInfo> idToEntryDataInfoDic;

    /// <summary>
    /// 词条解耦实现对象的加载函数
    /// </summary>
    partial void Load_IEntryState()
    {
        entryData = DataCenter.Instance.GetMetaData<EntryData>();
        idToEntryDataInfoDic = new Dictionary<int, EntryDataInfo>();
        EntryDataInfo[] roots = entryData.GetTops();
        SetEntryDataInfoDic(roots);
        EntryStateInit();
        //注册
        GameState.Instance.UnRegistor<INowTaskState>(INowTaskStateChange_Entry);
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChange_Entry);
    }

    /// <summary>
    /// 设置id对应词条数据字典
    /// </summary>
    /// <param name="targets"></param>
    private void SetEntryDataInfoDic(EntryDataInfo[] targets)
    {
        foreach (EntryDataInfo target in targets)
        {
            if (!idToEntryDataInfoDic.ContainsKey(target.ID))
            {
                idToEntryDataInfoDic.Add(target.ID, target);
            }
            EntryDataInfo[] childs = entryData.GetNexts(target);
            if (childs != null && childs.Length > 0)
                SetEntryDataInfoDic(childs);
        }
    }

    /// <summary>
    /// 词条状态的一次更新
    /// </summary>
    private void EntryStateInit()
    {

        foreach (KeyValuePair<int, EntryDataInfo> item in idToEntryDataInfoDic)
        {
            if (item.Value.UnlockDick.Count == 0)
            {
                if (!playerState.EntryEnableList.Contains(item.Value.ID))
                    playerState.EntryEnableList.Add(item.Value.ID);
            }
            else
            {
                var taskChecks = item.Value.UnlockDick.Where(temp => temp.Key == EntryDataInfo.EnumEntryUnlockType.OverTask).ToArray();
                foreach (var taskCheck in taskChecks)
                {
                    TaskMap.RunTimeTaskInfo taskInfo = runtimeTaskData.GetTasksWithID(taskCheck.Value, false);
                    if (taskInfo.IsOver && !playerState.EntryEnableList.Contains(item.Value.ID))
                    {
                        playerState.EntryEnableList.Add(item.Value.ID);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 任务状态发生变化(词条相关)
    /// </summary>
    /// <param name="iNowTaskState"></param>
    /// <param name="fieldName"></param>
    private void INowTaskStateChange_Entry(INowTaskState iNowTaskState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<INowTaskState, int>(temp => temp.OverTaskID)))
        {
            CheckEntryTaskOver(iNowTaskState.OverTaskID);
        }
    }

    /// <summary>
    /// 交互关系发生变化(词条相关)
    /// </summary>
    /// <param name="iInteractiveState"></param>
    /// <param name="fieldName"></param>
    private void IInteractiveStateChange_Entry(IInteractiveState iInteractiveState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IInteractiveState, int>(temp => temp.ClickInteractiveNPCID)))
        {
            CheckEntryClickNPC(iInteractiveState.ClickInteractiveNPCID);
        }
    }

    public void CheckEntryClickNPC(int npcID)
    {
        foreach (KeyValuePair<int, EntryDataInfo> item in idToEntryDataInfoDic)
        {
            if (item.Value.UnlockDick.ContainsKey(EntryDataInfo.EnumEntryUnlockType.ClickNPC))
            {
                if (item.Value.UnlockDick[EntryDataInfo.EnumEntryUnlockType.ClickNPC] == npcID)
                {
                    if (!playerState.EntryEnableList.Contains(item.Value.ID))
                        playerState.EntryEnableList.Add(item.Value.ID);
                }
            }
        }
    }

    public void CheckEntryKillMonster(EnumMonsterType monsterType)
    {

    }

    public void CheckEntryTaskOver(int taskID)
    {
        foreach (KeyValuePair<int, EntryDataInfo> item in idToEntryDataInfoDic)
        {
            if (item.Value.UnlockDick.ContainsKey(EntryDataInfo.EnumEntryUnlockType.OverTask))
            {
                if (item.Value.UnlockDick[EntryDataInfo.EnumEntryUnlockType.OverTask] == taskID)
                {
                    if (!playerState.EntryEnableList.Contains(item.Value.ID))
                        playerState.EntryEnableList.Add(item.Value.ID);
                }
            }
        }
    }
}

