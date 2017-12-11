using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 实现了INowTaskState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : INowTaskState
{

    /// <summary>
    /// 运行时的任务对象数组(正在执行的)
    /// </summary>
    List<RunTimeTaskInfo> runTimeTaskInfos_Start;
    /// <summary>
    /// 运行时的任务对象数组(等待接取的)
    /// </summary>
    List<RunTimeTaskInfo> runTimeTaskInfos_Wait;

    /// <summary>
    /// 检测位置的正在执行任务字典
    /// </summary>
    Dictionary<int, RunTimeTaskInfo> checkPostionRunTimeDic;
    /// <summary>
    /// 检测NPC的正在执行任务字典
    /// </summary>
    Dictionary<int, RunTimeTaskInfo> checkNPCRunTimeDic;
    /// <summary>
    /// 检测击杀怪物的正在执行任务字典
    /// </summary>
    Dictionary<int, RunTimeTaskInfo> checkMonsterRunTimeDic;
    /// <summary>
    /// 检测物品的则会能够在执行任务字典
    /// </summary>
    Dictionary<int, RunTimeTaskInfo> checkGoodsRunTimeDic;

    /// <summary>
    /// 任务信息对象 
    /// </summary>
    MetaTasksData metaTasksData;

    /// <summary>
    /// 任务接口实现对象的加载函数 
    /// </summary>
    partial void Load_INowTaskState()
    {
        metaTasksData = DataCenter.Instance.GetMetaData<MetaTasksData>();
        //当前可以做的任务,包括正在做的和可以接取的
        List<RunTimeTaskInfo> todoList = runtimeTaskData.GetAllToDoList();
        //从中将其分类
        runTimeTaskInfos_Start = todoList.Where(temp => temp.IsStart).ToList();
        runTimeTaskInfos_Wait = todoList.Where(temp => !temp.IsStart).ToList();
        //将正在做的任务分类(方便检测)
        checkPostionRunTimeDic = new Dictionary<int, RunTimeTaskInfo>();
        checkNPCRunTimeDic = new Dictionary<int, RunTimeTaskInfo>();
        checkMonsterRunTimeDic = new Dictionary<int, RunTimeTaskInfo>();
        checkGoodsRunTimeDic = new Dictionary<int, RunTimeTaskInfo>();
        runTimeTaskInfos_Start.ForEach(temp => SetStartTaskCheckClassify(temp));
    }

    /// <summary>
    /// 设置正在执行的任务的检测分类
    /// </summary>
    /// <param name="runTimeTaskInfo">正在执行的任务</param>
    void SetStartTaskCheckClassify(RunTimeTaskInfo runTimeTaskInfo)
    {
        if (runTimeTaskInfo == null || runTimeTaskInfo.IsOver)
            return;
        //NPC检测(是否存在交任务的npc)
        if (runTimeTaskInfo.RunTimeTaskNode.DeliveryTaskNpcId >= 0)
            checkNPCRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
        //位置检测(与NPC检测互斥,如果存在交任务的npc则不检测位置)
        else if (runTimeTaskInfo.RunTimeTaskNode.NowArrivedPosition != Vector3.zero)
            checkPostionRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
        //击杀怪物检测
        if (runTimeTaskInfo.RunTimeTaskNode.HaveKilledMonsterCount.Count > 0)
        {
            MetaTaskInfo metaTaskInfo = metaTasksData[runTimeTaskInfo.ID];
            foreach (KeyValuePair<int, int> item in metaTaskInfo.MetaTaskNode.NeedKillMonsterCount)
            {
                if (runTimeTaskInfo.RunTimeTaskNode.HaveKilledMonsterCount[item.Key] < item.Value)
                {
                    checkMonsterRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
                    break;
                }
            }
        }
        //获取物品检测
        if (runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount.Count > 0)
        {
            MetaTaskInfo metaTaskInfo = metaTasksData[runTimeTaskInfo.ID];
            foreach (KeyValuePair<int, int> item in metaTaskInfo.MetaTaskNode.NeedGetGoodsCount)
            {
                if (runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount[item.Key] < item.Value)
                {
                    checkGoodsRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 任务接口实现对象的更新函数
    /// </summary>
    partial void Update_INowTaskState()
    {
        CheckNowTask(EnumCheckTaskType.Position);
    }

    public bool CheckNowTask(EnumCheckTaskType checkTaskType, int value = -1)
    {
        switch (checkTaskType)
        {
            case EnumCheckTaskType.Position:
                return CheckNowTaskPostion();
            case EnumCheckTaskType.Monster:
                if (value != -1)
                    return CheckNowTaskMonster(value);
                break;
            case EnumCheckTaskType.Goods:
                if (value != -1)
                    return CheckNowTaskGoods(value);
                break;
            case EnumCheckTaskType.NPC:
                if (value != -1)
                    return CheckNowTaskNPC(value);
                break;
        }
        return false;
    }

    /// <summary>
    /// 检测位置数组所用的临时集合
    /// </summary>
    List<RunTimeTaskInfo> checkPostionDicTempList;

    /// <summary>
    /// 检测任务(关于角色位置)
    /// </summary>
    bool CheckNowTaskPostion()
    {
        if (checkPostionRunTimeDic == null)
            return false;
        if (checkPostionDicTempList == null)
            checkPostionDicTempList = new List<RunTimeTaskInfo>();
        if (checkPostionRunTimeDic.Count > 0)
        {
            checkPostionDicTempList.Clear();
            Vector3 playerNowPos = PlayerObj.transform.position;
            playerNowPos.y = 0;//忽略y轴
            foreach (KeyValuePair<int, RunTimeTaskInfo> checkPostionRunTime in checkPostionRunTimeDic)
            {
                //先判断场景是否一致
                if (!string.IsNullOrEmpty(SceneName) && string.Equals(checkPostionRunTime.Value.RunTimeTaskNode.TaskLocation.SceneName, SceneName))
                {
                    Vector3 targetPos = checkPostionRunTime.Value.RunTimeTaskNode.TaskLocation.ArrivedCenterPos;
                    targetPos.y = 0;
                    Vector3 offsetDis = targetPos - playerNowPos;
                    float sqrDis = Vector3.SqrMagnitude(offsetDis);
                    if (sqrDis < Mathf.Pow(checkPostionRunTime.Value.RunTimeTaskNode.TaskLocation.Radius, 2))
                    {
                        checkPostionDicTempList.Add(checkPostionRunTime.Value);
                    }
                }
            }
            if (checkPostionDicTempList.Count > 0)
            {
                //如果检测出已经完成则判断是否还有其他检测未完成,如果未完成则不要移除(这个是特例,和点击npc完成任务是互斥的)
                foreach (RunTimeTaskInfo runTimeTaskInfo in checkPostionDicTempList)
                {
                    checkPostionRunTimeDic.Remove(runTimeTaskInfo.ID);//临时的移除
                    if (HasCheckTaskID(runTimeTaskInfo.ID))//如果存在其他检测则此处重新添加上(检测位置是特例,因为有可能需要杀完怪后到达指定地点,提前到达是无效的)
                    {
                        checkPostionRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
                    }
                    else//如果不存在其他的检测了,则此任务完成
                    {
                        OverTaskID = runTimeTaskInfo.ID;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测NPC数组所用的临时集合
    /// </summary>
    List<RunTimeTaskInfo> checkNPCDicTempList;

    /// <summary>
    /// 检测任务(关于NPC)
    /// </summary>
    /// <param name="npcID">NPC的id,在点击npc并结束对话的时候调用</param>
    bool CheckNowTaskNPC(int npcID)
    {
        if (checkNPCRunTimeDic == null)
            return false;
        if (checkNPCDicTempList == null)
            checkNPCDicTempList = new List<RunTimeTaskInfo>();
        if (checkNPCRunTimeDic.Count > 0)
        {
            checkNPCDicTempList.Clear();
            foreach (KeyValuePair<int, RunTimeTaskInfo> checkNPCRunTime in checkNPCRunTimeDic)
            {
                if (checkNPCRunTime.Value.RunTimeTaskNode.DeliveryTaskNpcId == npcID)
                {
                    checkNPCDicTempList.Add(checkNPCRunTime.Value);
                }
            }
            if (checkNPCDicTempList.Count > 0)
            {
                //如果检测出已经完成则判断是否还有其他检测未完成,如果未完成则不要移除(这个是特例,和到达指定地点完成任务是互斥的)
                foreach (RunTimeTaskInfo runTimeTaskInfo in checkNPCDicTempList)
                {
                    checkNPCRunTimeDic.Remove(runTimeTaskInfo.ID);//临时的移除
                    if (HasCheckTaskID(runTimeTaskInfo.ID))//如果存在其他检测则此处重新添加上(检测npc是特例,因为有可能需要杀完怪后才能提交,提前点击是无效的)
                    {
                        checkNPCRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
                    }
                    else//如果不存在其他的检测了,则此任务完成
                    {
                        OverTaskID = runTimeTaskInfo.ID;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测击杀怪物数组所用的临时集合
    /// </summary>
    List<RunTimeTaskInfo> checkMonsterDicTempList;

    /// <summary>
    /// 检测任务(关于击杀怪物)
    /// </summary>
    /// <param name="monsterID">怪物的id</param>
    bool CheckNowTaskMonster(int monsterID)
    {
        if (checkMonsterRunTimeDic == null)
            return false;
        if (checkMonsterDicTempList == null)
            checkMonsterDicTempList = new List<RunTimeTaskInfo>();
        if (checkMonsterRunTimeDic.Count > 0)
        {
            checkMonsterDicTempList.Clear();
            foreach (KeyValuePair<int, RunTimeTaskInfo> checkMonsterRunTime in checkMonsterRunTimeDic)
            {
                RunTimeTaskInfo runTimeTaskInfo = checkMonsterRunTime.Value;
                if (!runTimeTaskInfo.RunTimeTaskNode.HaveKilledMonsterCount.ContainsKey(monsterID))//如果该任务不需要判断该怪物
                    continue;
                MetaTaskInfo metaTaskInfo = metaTasksData[checkMonsterRunTime.Key];
                runTimeTaskInfo.RunTimeTaskNode.HaveKilledMonsterCount[monsterID] += 1;//击杀怪物数量加1
                bool isOver = true;
                foreach (KeyValuePair<int, int> item in metaTaskInfo.MetaTaskNode.NeedKillMonsterCount)
                {
                    if (runTimeTaskInfo.RunTimeTaskNode.HaveKilledMonsterCount[item.Key] < item.Value)
                    {
                        isOver = false;
                        break;
                    }
                }
                if (isOver)
                    checkMonsterDicTempList.Add(checkMonsterRunTime.Value);
            }
            if (checkMonsterDicTempList.Count > 0)
            {
                foreach (RunTimeTaskInfo runTimeTaskInfo in checkMonsterDicTempList)
                {
                    //如果检测出已经完成则移除
                    checkMonsterRunTimeDic.Remove(runTimeTaskInfo.ID);
                    if (!HasCheckTaskID(runTimeTaskInfo.ID))//如果不存在检测项了则直接完成任务
                    {
                        OverTaskID = runTimeTaskInfo.ID;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测物品数组所用的临时集合
    /// </summary>
    List<RunTimeTaskInfo> checkGoodsDicTempList;

    /// <summary>
    /// 检测任务(关于物品)
    /// 物品在外部进行填入(这里只处理检测和移除,物品的生成不做处理)
    /// </summary>
    /// <param name="goodsID">变动了的物品的类型id(EnumGoodsType枚举),内部会判断物品类型,不论是丢弃或者增加</param>
    bool CheckNowTaskGoods(int goodsID)
    {
        if (checkGoodsRunTimeDic == null)
            return false;
        if (checkGoodsDicTempList == null)
            checkGoodsDicTempList = new List<RunTimeTaskInfo>();
        if (checkGoodsRunTimeDic.Count > 0)
        {
            checkGoodsDicTempList.Clear();
            foreach (KeyValuePair<int, RunTimeTaskInfo> checkGoodsRunTime in checkGoodsRunTimeDic)
            {
                RunTimeTaskInfo runTimeTaskInfo = checkGoodsRunTime.Value;
                if (!runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount.ContainsKey(goodsID))//如果该任务不需要判断该物品
                    continue;
                PlayGoods playerGoods = playerState.PlayerAllGoods.Where(temp => (int)temp.GoodsInfo.EnumGoodsType == goodsID).FirstOrDefault();
                if (playerGoods == null)
                    continue;
                runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount[goodsID] = playerGoods.Count;
                MetaTaskInfo metaTaskInfo = metaTasksData[checkGoodsRunTime.Key];
                bool isOver = true;
                foreach (KeyValuePair<int, int> item in metaTaskInfo.MetaTaskNode.NeedGetGoodsCount)
                {
                    if (runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount[item.Key] < item.Value)
                    {
                        isOver = false;
                        break;
                    }
                }
                if (isOver)
                    checkGoodsDicTempList.Add(checkGoodsRunTime.Value);
            }
            if (checkGoodsDicTempList.Count > 0)
            {
                foreach (RunTimeTaskInfo runTimeTaskInfo in checkGoodsDicTempList)
                {
                    //如果检测出已完成则移除
                    checkGoodsRunTimeDic.Remove(runTimeTaskInfo.ID);
                    if (!HasCheckTaskID(runTimeTaskInfo.ID))//如果不存在检测项了则直接完成任务
                    {
                        OverTaskID = runTimeTaskInfo.ID;
                        //完成以后从物品栏中移除指定的物品
                        foreach (KeyValuePair<int, int> item in runTimeTaskInfo.RunTimeTaskNode.HaveGetGoodsCount)
                        {
                            playerState.PlayerAllGoods
                                .Where(temp => (int)temp.GoodsInfo.EnumGoodsType == item.Key).ToList()
                                .ForEach(temp => temp.Count -= item.Value);
                            playerState.PlayerAllGoods.RemoveAll(temp => temp.Count <= 0);
                        }
                        GoodsChanged = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否存在检测的任务ID
    /// </summary>
    /// <param name="checkTaskID">检测的任务ID</param>
    /// <returns></returns>
    private bool HasCheckTaskID(int checkTaskID)
    {
        if (checkPostionRunTimeDic.ContainsKey(checkTaskID))
            return false;
        if (checkNPCRunTimeDic.ContainsKey(checkTaskID))
            return false;
        if (checkMonsterRunTimeDic.ContainsKey(checkTaskID))
            return false;
        if (checkGoodsRunTimeDic.ContainsKey(checkTaskID))
            return false;
        return true;
    }

    /// <summary>
    /// 在所有的检测字典以及集合中移除指定的任务 (主要使用与完成以及放弃任务)
    /// 如果检测到该任务已经失败
    /// </summary>
    /// <param name="taskID">任务id</param>
    /// <param name="all">是否将人物从等待接取集合中移除吗</param>
    private void RemoveTaskByIDAtDicAndList(int taskID, bool all = false)
    {
        if (!all)//如果不从等待接取集合中移除
        {
            RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == taskID).FirstOrDefault();
            if (runTimeTaskInfo != null)
            {
                if (runTimeTaskInfos_Wait.Count(temp => temp.ID == taskID) <= 0)
                {
                    runTimeTaskInfos_Wait.Add(runTimeTaskInfo);
                }
                runTimeTaskInfos_Start.Remove(runTimeTaskInfo);
            }
        }
        else//如果从等待集合中移除
        {
            runTimeTaskInfos_Start.RemoveAll(temp => temp.ID == taskID);
            runTimeTaskInfos_Wait.RemoveAll(temp => temp.ID == taskID);
        }
        //从字典中移除 
        checkPostionRunTimeDic.Remove(taskID);
        checkNPCRunTimeDic.Remove(taskID);
        checkMonsterRunTimeDic.Remove(taskID);
        checkGoodsRunTimeDic.Remove(taskID);
    }

    /// <summary>
    /// 开始任务
    /// </summary>
    int _StartTask;
    /// <summary>
    /// 开始任务
    /// </summary>
    public int StartTask
    {
        get { return _StartTask; }
        set
        {
            int tempStartTask = _StartTask;
            _StartTask = value;
            if (tempStartTask != _StartTask)
            {
                RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Wait.Where(temp => temp.ID == _StartTask).FirstOrDefault();
                if (runTimeTaskInfo != null)
                {
                    runTimeTaskInfos_Wait.Remove(runTimeTaskInfo);
                    runTimeTaskInfos_Start.Add(runTimeTaskInfo);
                    SetStartTaskCheckClassify(runTimeTaskInfo);
                    Call<INowTaskState, int>(temp => temp.StartTask);
                }
            }
        }
    }

    /// <summary>
    /// 任务完成的id
    /// </summary>
    int _OverTaskID;
    /// <summary>
    /// 任务完成的id
    /// </summary>
    public int OverTaskID
    {
        get { return _OverTaskID; }
        private set
        {
            int tempOverTaskID = _OverTaskID;
            _OverTaskID = value;
            if (tempOverTaskID != _OverTaskID)
            {
                RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == _OverTaskID).FirstOrDefault();
                if (runTimeTaskInfo == null)
                    return;
                //内部处理完成后的事项
                //奖励物品

                //奖励经验

                //奖励技能点

                //奖励声望

                //任务完成后的后续
                RemoveTaskByIDAtDicAndList(_OverTaskID);//从数据中移除
                runTimeTaskInfo.IsOver = true;
                List<RunTimeTaskInfo> todoList = runtimeTaskData.GetAllToDoList();
                //因为可能存在互斥任务,该任务完成后其他任务可能就失败了,因此这里检测剩下的任务是否存在与当前任务
                List<int> mustRemoveMutexTaskList = new List<int>();//需要移除的互斥的任务
                foreach (RunTimeTaskInfo nowTaskInfo in runTimeTaskInfos_Start)//正在执行的
                {
                    if (todoList.Count(temp => temp.ID == nowTaskInfo.ID) == 0)
                        mustRemoveMutexTaskList.Add(nowTaskInfo.ID);
                }
                foreach (RunTimeTaskInfo nowTaskInfo in runTimeTaskInfos_Wait)//等待接取的
                {
                    if (todoList.Count(temp => temp.ID == nowTaskInfo.ID) == 0)
                        mustRemoveMutexTaskList.Add(nowTaskInfo.ID);
                }
                foreach (int mustRemoveMutexTaskID in mustRemoveMutexTaskList)//移除需要移除的互斥任务
                {
                    RemoveTaskByIDAtDicAndList(mustRemoveMutexTaskID, true);
                }
                //检测新的任务
                foreach (RunTimeTaskInfo todoTaskInfo in todoList)
                {
                    if (!todoTaskInfo.IsStart)//只用处理未开始的
                    {
                        if (todoTaskInfo.RunTimeTaskNode.ReceiveTaskNpcId < 0)//直接接取
                        {
                            todoTaskInfo.IsStart = true;
                            //将任务添加到分类中
                            runTimeTaskInfos_Start.Add(todoTaskInfo);
                            SetStartTaskCheckClassify(todoTaskInfo);
                        }
                        else//如果不是直接接取的
                        {
                            //判断是否存在于等待截取集合中,不存在则添加
                            if (runTimeTaskInfos_Wait.Count(temp => temp.ID == todoTaskInfo.ID) <= 0)
                            {
                                runTimeTaskInfos_Wait.Add(todoTaskInfo);
                            }
                        }
                    }
                }
                //调用通知外部任务完成了
                Call<INowTaskState, int>(temp => temp.OverTaskID);
            }
        }
    }

    /// <summary>
    /// 放弃任务
    /// </summary>
    /// <param name="taskID"></param>
    public void GiveUPTask(int taskID)
    {
        RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == taskID && temp.RunTimeTaskNode.TaskType != Enums.TaskType.PrincipalLine).FirstOrDefault();
        if (runTimeTaskInfo != null)
        {
            RemoveTaskByIDAtDicAndList(runTimeTaskInfo.ID);
            runTimeTaskInfo.GiveUpTask();
        }
    }

    /// <summary>
    /// 获取制定场景中的未接取任务,如果场景名为空,则返回所有未接取任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public RunTimeTaskInfo[] GetWaitTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        if (string.IsNullOrEmpty(scene))
            return runTimeTaskInfos_Wait.ToArray();
        else
            return runTimeTaskInfos_Wait.Where(temp => npcData.GetNPCDataInfo(scene, temp.RunTimeTaskNode.ReceiveTaskNpcId) != null).ToArray();
    }

    /// <summary>
    /// 获取指定场景中的正在执行的任务,如果场景名为空,则返回所有正在执行的任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public RunTimeTaskInfo[] GetStartTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        if (string.IsNullOrEmpty(scene))
            return runTimeTaskInfos_Start.ToArray();
        else
            return runTimeTaskInfos_Start.Where(temp => npcData.GetNPCDataInfo(scene, temp.RunTimeTaskNode.ReceiveTaskNpcId) != null).ToArray();
    }

    /// <summary>
    /// 获取指定场景中的条件达成但是没有交付的任务,如果场景名为空,则会返回所有条件达成但是没有交付的任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public RunTimeTaskInfo[] GetEndTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        IEnumerable<RunTimeTaskInfo> taskInfos = runTimeTaskInfos_Start
            .Where(temp => !checkMonsterRunTimeDic.ContainsKey(temp.ID) && !checkGoodsRunTimeDic.ContainsKey(temp.ID));
        if (string.IsNullOrEmpty(scene))
            return taskInfos.ToArray();
        else
            return taskInfos.Where(temp => npcData.GetNPCDataInfo(scene, temp.RunTimeTaskNode.ReceiveTaskNpcId) != null).ToArray();
    }
}
