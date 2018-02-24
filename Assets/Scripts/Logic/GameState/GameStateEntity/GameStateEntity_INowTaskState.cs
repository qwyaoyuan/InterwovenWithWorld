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
    List<TaskMap.RunTimeTaskInfo> runTimeTaskInfos_Start;
    /// <summary>
    /// 运行时的任务对象数组(等待接取的)
    /// </summary>
    List<TaskMap.RunTimeTaskInfo> runTimeTaskInfos_Wait;

    /// <summary>
    /// 检测位置的正在执行任务字典
    /// </summary>
    Dictionary<int, TaskMap.RunTimeTaskInfo> checkPostionRunTimeDic;
    /// <summary>
    /// 检测NPC的正在执行任务字典
    /// </summary>
    Dictionary<int, TaskMap.RunTimeTaskInfo> checkNPCRunTimeDic;
    /// <summary>
    /// 检测击杀怪物的正在执行任务字典
    /// </summary>
    Dictionary<int, TaskMap.RunTimeTaskInfo> checkMonsterRunTimeDic;
    /// <summary>
    /// 检测物品的则会能够在执行任务字典
    /// </summary>
    Dictionary<int, TaskMap.RunTimeTaskInfo> checkGoodsRunTimeDic;

    /// <summary>
    /// 检测移除的id集合
    /// </summary>
    List<int> checkRemoveIDList;

    /// <summary>
    /// 任务信息对象 
    /// </summary>
    //MetaTasksData metaTasksData;

    ///<summary>
    ///游戏状态对象
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 交互对象
    /// </summary>
    IInteractiveState iInteractiveState;

    /// <summary>
    /// 任务事件处理接口
    /// </summary>
    INowTaskStateEvent iNowTaskStateEvent;

    /// <summary>
    /// 任务接口实现对象的加载函数 
    /// </summary>
    partial void Load_INowTaskState()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iNowTaskStateEvent = GameState.Instance.GetEntity<INowTaskStateEvent>();
        //metaTasksData = DataCenter.Instance.GetMetaData<MetaTasksData>();
        //当前可以做的任务,包括正在做的和可以接取的
        List<TaskMap.RunTimeTaskInfo> todoList = runtimeTaskData.GetAllToDoList();
        //从中将其分类
        runTimeTaskInfos_Start = todoList.Where(temp => temp.IsStart).ToList();
        runTimeTaskInfos_Wait = todoList.Where(temp => !temp.IsStart).ToList();
        //将正在做的任务分类(方便检测)
        checkPostionRunTimeDic = new Dictionary<int, TaskMap.RunTimeTaskInfo>();
        checkNPCRunTimeDic = new Dictionary<int, TaskMap.RunTimeTaskInfo>();
        checkMonsterRunTimeDic = new Dictionary<int, TaskMap.RunTimeTaskInfo>();
        checkGoodsRunTimeDic = new Dictionary<int, TaskMap.RunTimeTaskInfo>();
        runTimeTaskInfos_Start.ForEach(temp => SetStartTaskCheckClassify(temp));
        //触发事件
        foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in todoList)
        {
            iNowTaskStateEvent.TriggeringEvents(runTimeTaskInfo);
        }
    }

    /// <summary>
    /// 设置正在执行的任务的检测分类
    /// </summary>
    /// <param name="runTimeTaskInfo">正在执行的任务</param>
    void SetStartTaskCheckClassify(TaskMap.RunTimeTaskInfo runTimeTaskInfo)
    {
        if (runTimeTaskInfo == null || runTimeTaskInfo.IsOver)
            return;
        //NPC检测(是否存在交任务的npc)
        if (runTimeTaskInfo.TaskInfoStruct.DeliveryTaskNpcId > 0)
            checkNPCRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
        //位置检测(与NPC检测互斥,如果存在交任务的npc则不检测位置(但是使用其中的场景))
        else if (runTimeTaskInfo.TaskInfoStruct.DeliveryTaskLocation != null)
            checkPostionRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
        //击杀怪物检测
        if (runTimeTaskInfo.TaskInfoStruct.NeedKillMonsterCount != null && runTimeTaskInfo.TaskInfoStruct.NeedKillMonsterCount.Count(temp => temp.Value > 0) > 0)
        {
            if (runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount == null)
                runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount = new Dictionary<EnumMonsterType, int>();
            bool mustAdd = false;
            foreach (KeyValuePair<EnumMonsterType, int> item in runTimeTaskInfo.TaskInfoStruct.NeedKillMonsterCount)
            {
                if (!runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount.ContainsKey(item.Key))
                    runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount.Add(item.Key, 0);
                if (runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount[item.Key] < item.Value)
                {
                    mustAdd = true;
                }
            }
            if (mustAdd)
                checkMonsterRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo);
        }
        //获取物品检测
        if (runTimeTaskInfo.TaskInfoStruct.NeedGetGoodsCount != null && runTimeTaskInfo.TaskInfoStruct.NeedGetGoodsCount.Count(temp => temp.Value > 0) > 0)
        {
            if (runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount == null)
                runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount = new Dictionary<EnumGoodsType, int>();
            bool mustAdd = false;
            foreach (KeyValuePair<EnumGoodsType, int> item in runTimeTaskInfo.TaskInfoStruct.NeedGetGoodsCount)
            {
                if (!runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount.ContainsKey(item.Key))
                    runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount.Add(item.Key, 0);
                if (runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount[item.Key] < item.Value)
                {
                    mustAdd = true;
                }
            }
            if (mustAdd)
                checkGoodsRunTimeDic.Add(runTimeTaskInfo.ID, runTimeTaskInfo); ;
        }
    }

    /// <summary>
    /// 任务接口实现对象的更新函数
    /// </summary>
    partial void Update_INowTaskState()
    {
        if (iGameState != null &&
            (iGameState.GameRunType == EnumGameRunType.Safe || iGameState.GameRunType == EnumGameRunType.Unsafa) &&
            iInteractiveState != null && iInteractiveState.CanInterlude)
        {
            //检测可以完成的任务
            CheckNowTask(EnumCheckTaskType.Position);
            //如果此时没有主线但是有未接取的主线,则持续检测
            if (runTimeTaskInfos_Start != null && runTimeTaskInfos_Wait != null
                && runTimeTaskInfos_Start.Count(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Main) == 0
                && runTimeTaskInfos_Wait.Count(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Main) > 0)
            {
                //检测新任务
                CheckNewTask();
            }
        }
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
    List<TaskMap.RunTimeTaskInfo> checkPostionDicTempList;

    /// <summary>
    /// 检测任务(关于角色位置)
    /// </summary>
    bool CheckNowTaskPostion()
    {
        if (checkPostionRunTimeDic == null || PlayerObj == null)
            return false;
        if (checkPostionDicTempList == null)
            checkPostionDicTempList = new List<TaskMap.RunTimeTaskInfo>();
        if (checkPostionRunTimeDic.Count > 0)
        {
            checkPostionDicTempList.Clear();
            Vector3 playerNowPos = PlayerObj.transform.position;
            playerNowPos.y = 0;//忽略y轴
            foreach (KeyValuePair<int, TaskMap.RunTimeTaskInfo> checkPostionRunTime in checkPostionRunTimeDic)
            {
                //先判断场景是否一致
                if (!string.IsNullOrEmpty(SceneName) && string.Equals(checkPostionRunTime.Value.TaskInfoStruct.DeliveryTaskLocation.SceneName, SceneName))
                {
                    Vector3 targetPos = checkPostionRunTime.Value.TaskInfoStruct.DeliveryTaskLocation.ArrivedCenterPos;
                    targetPos.y = 0;
                    Vector3 offsetDis = targetPos - playerNowPos;
                    float sqrDis = Vector3.SqrMagnitude(offsetDis);
                    if (sqrDis < Mathf.Pow(checkPostionRunTime.Value.TaskInfoStruct.DeliveryTaskLocation.Radius, 2))
                    {
                        checkPostionDicTempList.Add(checkPostionRunTime.Value);
                    }
                }
            }
            if (checkPostionDicTempList.Count > 0)
            {
                //如果检测出已经完成则判断是否还有其他检测未完成,如果未完成则不要移除(这个是特例,和点击npc完成任务是互斥的)
                foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in checkPostionDicTempList)
                {
                    checkPostionRunTimeDic.Remove(runTimeTaskInfo.ID);//临时的移除
                    if (!HasCheckTaskID(runTimeTaskInfo.ID))//如果存在其他检测则此处重新添加上(检测位置是特例,因为有可能需要杀完怪后到达指定地点,提前到达是无效的)
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
        if (runTimeTaskInfos_Start.Count > 0)
        {
            if (checkRemoveIDList == null)
                checkRemoveIDList = new List<int>();
            checkRemoveIDList.Clear();
            foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in runTimeTaskInfos_Start)
            {
                if (checkPostionRunTimeDic.ContainsKey(runTimeTaskInfo.ID))
                    continue;
                if (checkNPCRunTimeDic.ContainsKey(runTimeTaskInfo.ID))
                    continue;
                if (checkMonsterRunTimeDic.ContainsKey(runTimeTaskInfo.ID))
                    continue;
                if (checkGoodsRunTimeDic.ContainsKey(runTimeTaskInfo.ID))
                    continue;
                checkRemoveIDList.Add(runTimeTaskInfo.ID);
            }
            if (checkRemoveIDList.Count > 0)
            {
                foreach (int checkRemoveID in checkRemoveIDList)
                {
                    OverTaskID = checkRemoveID;
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 检测NPC数组所用的临时集合
    /// </summary>
    List<TaskMap.RunTimeTaskInfo> checkNPCDicTempList;

    /// <summary>
    /// 检测任务(关于NPC)
    /// </summary>
    /// <param name="npcID">NPC的id,在点击npc并结束对话的时候调用</param>
    bool CheckNowTaskNPC(int npcID)
    {
        if (checkNPCRunTimeDic == null)
            return false;
        if (checkNPCDicTempList == null)
            checkNPCDicTempList = new List<TaskMap.RunTimeTaskInfo>();
        if (checkNPCRunTimeDic.Count > 0)
        {
            checkNPCDicTempList.Clear();
            foreach (KeyValuePair<int, TaskMap.RunTimeTaskInfo> checkNPCRunTime in checkNPCRunTimeDic)
            {
                if (checkNPCRunTime.Value.TaskInfoStruct.DeliveryTaskNpcId == npcID)
                {
                    if (checkNPCRunTime.Value.TaskInfoStruct.DeliveryTaskLocation == null || (checkNPCRunTime.Value.TaskInfoStruct.DeliveryTaskLocation != null && string.Equals(checkNPCRunTime.Value.TaskInfoStruct.DeliveryTaskLocation.SceneName, SceneName)))
                        checkNPCDicTempList.Add(checkNPCRunTime.Value);
                }
            }
            if (checkNPCDicTempList.Count > 0)
            {
                //如果检测出已经完成则判断是否还有其他检测未完成,如果未完成则不要移除(这个是特例,和到达指定地点完成任务是互斥的)
                foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in checkNPCDicTempList)
                {
                    checkNPCRunTimeDic.Remove(runTimeTaskInfo.ID);//临时的移除
                    if (!HasCheckTaskID(runTimeTaskInfo.ID))//如果存在其他检测则此处重新添加上(检测npc是特例,因为有可能需要杀完怪后才能提交,提前点击是无效的)
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
    List<TaskMap.RunTimeTaskInfo> checkMonsterDicTempList;

    /// <summary>
    /// 检测任务(关于击杀怪物)
    /// </summary>
    /// <param name="monsterID">怪物的id</param>
    bool CheckNowTaskMonster(int monsterID)
    {
        if (checkMonsterRunTimeDic == null)
            return false;
        if (checkMonsterDicTempList == null)
            checkMonsterDicTempList = new List<TaskMap.RunTimeTaskInfo>();
        if (checkMonsterRunTimeDic.Count > 0)
        {
            checkMonsterDicTempList.Clear();
            foreach (KeyValuePair<int, TaskMap.RunTimeTaskInfo> checkMonsterRunTime in checkMonsterRunTimeDic)
            {
                TaskMap.RunTimeTaskInfo runTimeTaskInfo = checkMonsterRunTime.Value;
                if (!runTimeTaskInfo.TaskInfoStruct.NeedKillMonsterCount.ContainsKey((EnumMonsterType)monsterID))//如果该任务不需要判断该怪物
                    continue;
                runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount[(EnumMonsterType)monsterID] += 1;//击杀怪物数量加1
                bool isOver = true;
                foreach (KeyValuePair<EnumMonsterType, int> item in runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount)
                {
                    if (runTimeTaskInfo.TaskInfoStruct.GameKillMonsterCount[item.Key] < item.Value)
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
                foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in checkMonsterDicTempList)
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
    List<TaskMap.RunTimeTaskInfo> checkGoodsDicTempList;

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
            checkGoodsDicTempList = new List<TaskMap.RunTimeTaskInfo>();
        if (checkGoodsRunTimeDic.Count > 0)
        {
            checkGoodsDicTempList.Clear();
            foreach (KeyValuePair<int, TaskMap.RunTimeTaskInfo> checkGoodsRunTime in checkGoodsRunTimeDic)
            {
                TaskMap.RunTimeTaskInfo runTimeTaskInfo = checkGoodsRunTime.Value;
                if (!runTimeTaskInfo.TaskInfoStruct.NeedGetGoodsCount.ContainsKey((EnumGoodsType)goodsID))//如果该任务不需要判断该物品
                    continue;
                PlayGoods playerGoods = playerState.PlayerAllGoods.Where(temp => (int)temp.GoodsInfo.EnumGoodsType == goodsID).FirstOrDefault();
                if (playerGoods == null)
                    continue;
                runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount[(EnumGoodsType)goodsID] = playerGoods.Count;
                bool isOver = true;
                foreach (KeyValuePair<EnumGoodsType, int> item in runTimeTaskInfo.TaskInfoStruct.NeedGetGoodsCount)
                {
                    if (runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount[item.Key] < item.Value)
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
                foreach (TaskMap.RunTimeTaskInfo runTimeTaskInfo in checkGoodsDicTempList)
                {
                    //如果检测出已完成则移除
                    checkGoodsRunTimeDic.Remove(runTimeTaskInfo.ID);
                    if (!HasCheckTaskID(runTimeTaskInfo.ID))//如果不存在检测项了则直接完成任务
                    {
                        OverTaskID = runTimeTaskInfo.ID;
                        //完成以后从物品栏中移除指定的物品
                        foreach (KeyValuePair<EnumGoodsType, int> item in runTimeTaskInfo.TaskInfoStruct.GameGetGoodsCount)
                        {
                            playerState.PlayerAllGoods
                                .Where(temp => temp.GoodsInfo.EnumGoodsType == item.Key).ToList()
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
    /// 如果检测到该任务已经失败;如果检测到任务已经完成
    /// </summary>
    /// <param name="taskID">任务id</param>
    /// <param name="all">是否将人物从等待接取集合中移除吗</param>
    private void RemoveTaskByIDAtDicAndList(int taskID, bool all = false)
    {
        if (!all)//如果不从等待接取集合中移除
        {
            TaskMap.RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == taskID).FirstOrDefault();
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
    int _StartTask = -1;
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
                TaskMap.RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Wait.Where(temp => temp.ID == _StartTask).FirstOrDefault();
                if (runTimeTaskInfo != null)
                {
                    runTimeTaskInfos_Wait.RemoveAll(temp => temp.ID == runTimeTaskInfo.ID);
                    runTimeTaskInfos_Start.Add(runTimeTaskInfo);
                    runTimeTaskInfo.IsStart = true;
                    SetStartTaskCheckClassify(runTimeTaskInfo);
                    Call<INowTaskState, int>(temp => temp.StartTask);
                    //触发事件
                    iNowTaskStateEvent.TriggeringEvents(runTimeTaskInfo);
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
                TaskMap.RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == _OverTaskID).FirstOrDefault();
                if (runTimeTaskInfo == null)
                    return;
                //内部处理完成后的事项
                //奖励物品

                //奖励经验

                //奖励技能点

                //奖励声望

                //任务完成后的后续
                RemoveTaskByIDAtDicAndList(_OverTaskID, true);//从数据中移除
                runTimeTaskInfo.IsOver = true;
                List<TaskMap.RunTimeTaskInfo> todoList = runtimeTaskData.GetAllToDoList();
                //因为可能存在互斥任务,该任务完成后其他任务可能就失败了,因此这里检测剩下的任务是否存在与当前任务
                List<int> mustRemoveMutexTaskList = new List<int>();//需要移除的互斥的任务
                foreach (TaskMap.RunTimeTaskInfo nowTaskInfo in runTimeTaskInfos_Start)//正在执行的
                {
                    if (todoList.Count(temp => temp.ID == nowTaskInfo.ID) == 0)
                        mustRemoveMutexTaskList.Add(nowTaskInfo.ID);
                }
                foreach (TaskMap.RunTimeTaskInfo nowTaskInfo in runTimeTaskInfos_Wait)//等待接取的
                {
                    if (todoList.Count(temp => temp.ID == nowTaskInfo.ID) == 0)
                        mustRemoveMutexTaskList.Add(nowTaskInfo.ID);
                }
                foreach (int mustRemoveMutexTaskID in mustRemoveMutexTaskList)//移除需要移除的互斥任务
                {
                    RemoveTaskByIDAtDicAndList(mustRemoveMutexTaskID, true);
                }
                //检测可以接取的任务
                CheckNewTask();
                //调用通知外部任务完成了
                Call<INowTaskState, int>(temp => temp.OverTaskID);
                //触发事件
                iNowTaskStateEvent.TriggeringEvents(runTimeTaskInfo);
                foreach (TaskMap.RunTimeTaskInfo todoRunTimeTaskInfo in todoList)
                {
                    iNowTaskStateEvent.TriggeringEvents(todoRunTimeTaskInfo);
                }
            }
        }
    }

    /// <summary>
    /// 检测当前可以直接接取的任务
    /// </summary>
    private void CheckNewTask()
    {
        if (PlayerObj == null)
            return;
        List<TaskMap.RunTimeTaskInfo> todoList = runtimeTaskData.GetAllToDoList();
        //检测新的任务
        foreach (TaskMap.RunTimeTaskInfo todoTaskInfo in todoList)
        {
            if (!todoTaskInfo.IsStart)//只用处理未开始的
            {
                if (runTimeTaskInfos_Wait.Count(temp => temp.ID == todoTaskInfo.ID) <= 0)//不管是否可以直接接取都先将该数据存放到等待接取集合中,如果可以直接接取
                {
                    runTimeTaskInfos_Wait.Add(todoTaskInfo);
                }
                //如果可以直接接取则接取该任务
                if (todoTaskInfo.TaskInfoStruct.ReceiveTaskNpcId <= 0)//直接接取
                {
                    bool canStart = false;
                    if (todoTaskInfo.TaskInfoStruct.ReceiveTaskLocation == null)
                        canStart = true;
                    else
                    {
                        Vector3 playerNowPos = PlayerObj.transform.position;
                        playerNowPos.y = 0;//忽略y轴
                        Vector3 targetPos = todoTaskInfo.TaskInfoStruct.ReceiveTaskLocation.ArrivedCenterPos;
                        targetPos.y = 0;
                        Vector3 offsetDis = targetPos - playerNowPos;
                        float sqrDis = Vector3.SqrMagnitude(offsetDis);
                        if (sqrDis < Mathf.Pow(todoTaskInfo.TaskInfoStruct.ReceiveTaskLocation.Radius, 2))
                            canStart = true;
                    }
                    if (canStart)
                    {
                        //if (todoTaskInfo.TaskInfoStruct.NeedShowTalk)
                        //{
                        //    //调用对话框,让对话框完成后实现接取
                        //    IInteractiveState iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
                        //    if (iInteractiveState.InterludeObj != null)
                        //    {
                        //        iInteractiveState.InterludeObj.SetActive(true);
                        //    }
                        //}
                        //else
                        //{
                        StartTask = todoTaskInfo.ID;
                        //}
                        //StartTask = todoTaskInfo.ID;
                        //todoTaskInfo.IsStart = true;
                        ////将任务添加到分类中
                        //runTimeTaskInfos_Start.Add(todoTaskInfo);
                        //runTimeTaskInfos_Wait.RemoveAll(temp => temp.ID == todoTaskInfo.ID);
                        //SetStartTaskCheckClassify(todoTaskInfo);
                    }
                }
                //else//如果不是直接接取的
                //{
                //    //判断是否存在于等待截取集合中,不存在则添加
                //    if (runTimeTaskInfos_Wait.Count(temp => temp.ID == todoTaskInfo.ID) <= 0)
                //    {
                //        runTimeTaskInfos_Wait.Add(todoTaskInfo);
                //    }
                //}
            }
        }
    }

    /// <summary>
    /// 放弃任务
    /// </summary>
    /// <param name="taskID"></param>
    public void GiveUPTask(int taskID)
    {
        TaskMap.RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos_Start.Where(temp => temp.ID == taskID && temp.TaskInfoStruct.TaskType != TaskMap.Enums.EnumTaskType.Main).FirstOrDefault();
        if (runTimeTaskInfo != null)
        {
            RemoveTaskByIDAtDicAndList(runTimeTaskInfo.ID);
            runTimeTaskInfo.IsStart = false;
            //触发事件
            iNowTaskStateEvent.TriggeringEvents(runTimeTaskInfo);
        }
    }

    /// <summary>
    /// 获取制定场景中的未接取任务,如果场景名为空,则返回所有未接取任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public TaskMap.RunTimeTaskInfo[] GetWaitTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        if (string.IsNullOrEmpty(scene))
            return runTimeTaskInfos_Wait.ToArray();
        else
            return runTimeTaskInfos_Wait.Where(temp =>
                ((temp.TaskInfoStruct.ReceiveTaskLocation == null || string.Equals(scene, temp.TaskInfoStruct.ReceiveTaskLocation.SceneName)) && npcData.GetNPCDataInfo(scene, temp.TaskInfoStruct.ReceiveTaskNpcId) != null) ||
                (temp.TaskInfoStruct.ReceiveTaskLocation != null && string.Equals(scene, temp.TaskInfoStruct.ReceiveTaskLocation.SceneName))
            ).ToArray();
    }

    /// <summary>
    /// 获取指定场景中的正在执行的任务,如果场景名为空,则返回所有正在执行的任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public TaskMap.RunTimeTaskInfo[] GetStartTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        if (string.IsNullOrEmpty(scene))
            return runTimeTaskInfos_Start.ToArray();
        else
            return runTimeTaskInfos_Start.Where(temp =>
            ((temp.TaskInfoStruct.DeliveryTaskLocation == null || string.Equals(scene, temp.TaskInfoStruct.DeliveryTaskLocation.SceneName)) && npcData.GetNPCDataInfo(scene, temp.TaskInfoStruct.DeliveryTaskNpcId) != null) ||
              (temp.TaskInfoStruct.DeliveryTaskLocation != null && string.Equals(scene, temp.TaskInfoStruct.DeliveryTaskLocation.SceneName))
            ).ToArray();
    }

    /// <summary>
    /// 获取指定场景中的条件达成但是没有交付的任务,如果场景名为空,则会返回所有条件达成但是没有交付的任务
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public TaskMap.RunTimeTaskInfo[] GetEndTask(string scene)
    {
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        IEnumerable<TaskMap.RunTimeTaskInfo> taskInfos = runTimeTaskInfos_Start
            .Where(temp => !checkMonsterRunTimeDic.ContainsKey(temp.ID) && !checkGoodsRunTimeDic.ContainsKey(temp.ID));
        if (string.IsNullOrEmpty(scene))
            return taskInfos.ToArray();
        else
            return taskInfos.Where(temp =>
                ((temp.TaskInfoStruct.DeliveryTaskLocation == null || string.Equals(scene, temp.TaskInfoStruct.DeliveryTaskLocation.SceneName)) && npcData.GetNPCDataInfo(scene, temp.TaskInfoStruct.DeliveryTaskNpcId) != null) ||
                (temp.TaskInfoStruct.DeliveryTaskLocation != null && string.Equals(scene, temp.TaskInfoStruct.DeliveryTaskLocation.SceneName))
            ).ToArray();
    }

}
