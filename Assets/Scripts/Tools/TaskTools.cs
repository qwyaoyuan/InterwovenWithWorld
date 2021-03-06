﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于执行任务的工具
/// </summary>
public class TaskTools : IEntrance
{
    /// <summary>
    /// 单例对象
    /// </summary>
    public static TaskTools Instance { get; private set; }

    /// <summary>
    /// 获取id
    /// </summary>
    int nextID = 0;

    /// <summary>
    /// 任务字典
    /// </summary>
    Dictionary<int, RunTaskStruct> taskDic;
    /// <summary>
    /// 任务操作手柄字典
    /// </summary>
    Dictionary<RunTaskStruct, RunTaskStruct.RunTaskStructHandle> taskHandleDic;

    /// <summary>
    /// 任务执行集合
    /// </summary>
    List<RunTaskStruct> taskList;

    /// <summary>
    /// 要移除的任务执行集合
    /// </summary>
    List<RunTaskStruct> removeTaskList;

    public void Start()
    {
        Instance = this;
        taskDic = new Dictionary<int, RunTaskStruct>();
        taskHandleDic = new Dictionary<RunTaskStruct, RunTaskStruct.RunTaskStructHandle>();
        taskList = new List<RunTaskStruct>();
        removeTaskList = new List<RunTaskStruct>();
    }

    public void Update()
    {
        if (taskList.Count <= 0)
            return;
        RunTaskStruct[] tempTaskArray = taskList.ToArray();
        foreach (RunTaskStruct runTaskStruct in tempTaskArray)
        {
            RunTaskStruct.RunTaskStructHandle runTaskStructHandle = null;
            if (taskHandleDic.TryGetValue(runTaskStruct, out runTaskStructHandle))
            {
                switch (runTaskStructHandle.RunTaskType)
                {
                    case EnumRunTaskType.Frame:
                        float nowFrame = runTaskStructHandle.NowFrame;
                        runTaskStructHandle.NowFrame -= runTaskStructHandle.SpeedRate;
                        if (nowFrame > 0)
                            continue;
                        else
                        {
                            if (!runTaskStructHandle.HasNext)//不存在下一次直接移除
                                removeTaskList.Add(runTaskStruct);
                            else if (runTaskStructHandle.Conditions != null && !runTaskStructHandle.Conditions())//存在则判断条件存在且条件不通过,移除
                                removeTaskList.Add(runTaskStruct);
                            if (runTaskStructHandle.Callback != null)
                                runTaskStructHandle.Callback();
                        }
                        break;
                    case EnumRunTaskType.Time:
                        float nowTime = runTaskStructHandle.NowTime;
                        runTaskStructHandle.NowTime -= runTaskStructHandle.SpeedRate * Time.deltaTime;
                        if (nowTime > 0)
                            continue;
                        else
                        {
                            if (!runTaskStructHandle.HasNext)//不存在下一次直接移除
                                removeTaskList.Add(runTaskStruct);
                            else if (runTaskStructHandle.Conditions != null && !runTaskStructHandle.Conditions())//存在则判断条件存在且条件不通过,移除
                                removeTaskList.Add(runTaskStruct);
                            if (runTaskStructHandle.Callback != null)
                                runTaskStructHandle.Callback();
                        }
                        break;
                }
            }
        }
        foreach (RunTaskStruct runTaskStruct in removeTaskList)
        {
            RunTaskStruct.RunTaskStructHandle runTaskStructHandle = null;
            if (taskHandleDic.TryGetValue(runTaskStruct, out runTaskStructHandle))
            { }
            taskList.Remove(runTaskStruct);
        }
        removeTaskList.Clear();
    }

    public void OnDestroy() { Instance = null; }

    /// <summary>
    /// 获取一个任务
    /// </summary>
    /// <returns></returns>
    public RunTaskStruct GetRunTaskStruct()
    {
        RunTaskStruct.RunTaskStructHandle runTaskStructHandle = null;
        RunTaskStruct runTaskStruct = new RunTaskStruct(nextID++, (handle) => runTaskStructHandle = handle);
        if (runTaskStructHandle != null)
        {
            taskHandleDic.Add(runTaskStruct, runTaskStructHandle);
            runTaskStructHandle.runTaskStateAction = RunTaskState;
        }
        taskDic.Add(runTaskStruct.id, runTaskStruct);
        return runTaskStruct;
    }

    /// <summary>
    /// 运行任务的状态
    /// </summary>
    /// <param name="target">运行任务的对象</param>
    /// <param name="state">设置任务的状态</param>
    private void RunTaskState(RunTaskStruct target, bool state)
    {
        if (state)
        {
            if (!taskList.Contains(target))
            {
                taskList.Add(target);
            }
        }
        else
        {
            taskList.Remove(target);
        }
    }

    /// <summary>
    /// 移除指定的任务
    /// 可以直接传入任务对象
    /// </summary>
    /// <param name="id">任务的id</param>
    public void RemoveRunTaskStruct(int id)
    {
        if (id < 0 || !taskDic.ContainsKey(id))
            return;
        RunTaskStruct runTaskStruct = taskDic[id];
        taskDic.Remove(id);
        if (taskHandleDic.ContainsKey(runTaskStruct))
            taskHandleDic.Remove(runTaskStruct);
        taskList.Remove(runTaskStruct);
    }
}

/// <summary>
/// 执行任务结构 
/// </summary>
public class RunTaskStruct
{
    /// <summary>
    /// 任务的id
    /// </summary>
    public readonly int id;

    /// <summary>
    /// 执行任务的类型
    /// </summary>
    public EnumRunTaskType RunTaskType { get; private set; }

    /// <summary>
    /// 速度倍率
    /// </summary>
    float speedRate;

    /// <summary>
    /// 操作手柄
    /// </summary>
    RunTaskStructHandle runTaskStructHandle;

    /// <summary>
    /// 时间
    /// </summary>
    float time;

    /// <summary>
    ///  帧数
    /// </summary>
    float frame;

    /// <summary>
    /// 回调函数
    /// </summary>
    Action Callback;

    /// <summary>
    /// 执行次数
    /// </summary>
    int executionCount;

    /// <summary>
    /// 判断执行条件函数
    /// </summary>
    Func<bool> Conditions;

    /// <summary>
    /// 构造函数 
    /// </summary>
    /// <param name="id">任务的id</param>
    /// <param name="ReturnHanel">任务的操作手柄设置</param>
    public RunTaskStruct(
        int id,
        Action<RunTaskStructHandle> ReturnHanel)
    {
        this.id = id;
        speedRate = 1;
        runTaskStructHandle = new RunTaskStructHandle(this);
        ReturnHanel(runTaskStructHandle);
    }

    /// <summary>
    /// 设置速度倍率
    /// </summary>
    /// <param name="speedRate">速度倍率</param>
    public void SetSpeed(float speedRate)
    {
        this.speedRate = speedRate;
    }

    /// <summary>
    /// 获取剩余的等待（帧数或时间）
    /// 如果当前是按帧模式，返回还有多少帧
    /// 如果当前是按时间模式，则返回还有多少秒
    /// 如果当前已经初始化了则返回0
    /// </summary>
    public float SurplusWait
    {
        get
        {
            switch (RunTaskType)
            {
                case EnumRunTaskType.Frame:
                    return frame;
                case EnumRunTaskType.Time:
                    return time;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// 初始化任务
    /// 该函数会清空所有的设置（时间 帧 执行类型 速率 回调）
    /// </summary>
    public void InitTask()
    {
        runTaskStructHandle.StopTask();
        frame = 0;
        time = 0;
        RunTaskType = EnumRunTaskType.None;
        speedRate = 1;
        Callback = null;
    }

    /// <summary>
    /// 开始执行任务（安帧计算）
    /// </summary>
    /// <param name="frame">等待帧数</param>
    /// <param name="Callback">回调函数</param>
    /// <param name="executionCount">执行次数,大于零表示自行次数,小于等于0表示不限制次数</param>
    /// <param name="executeNow">是否立即执行</param>
    /// <param name="Conditions">判断执行条件,如果返回为真并且存在下次调用则下次会继续调用,否则下次将不再调用</param>
    public void StartTask(int? frame = null, Action Callback = null, int executionCount = 1, bool executeNow = true, Func<bool> Conditions = null)
    {
        if (frame != null)
        {
            this.frame = frame.Value;
            RunTaskType = EnumRunTaskType.Frame;
        }
        if (Callback != null)
            this.Callback = Callback;
        this.executionCount = executionCount;
        this.Conditions = Conditions;
        runTaskStructHandle.StartTask(executeNow);
    }

    /// <summary>
    /// 开始执行任务（按时间计算）
    /// </summary>
    /// <param name="time">等待时间</param>
    /// <param name="Callback">回调函数</param>
    /// <param name="executionCount">执行次数,大于零表示自行次数,小于等于0表示不限制次数</param>
    /// <param name="executeNow">是否立即执行</param>
    /// <param name="Conditions">判断执行条件,如果返回为真并且存在下次调用则下次会继续调用,否则下次将不再调用</param>
    public void StartTask(float? time = null, Action Callback = null, int executionCount = 1, bool executeNow = true, Func<bool> Conditions = null)
    {
        if (time != null)
        {
            this.time = time.Value;
            RunTaskType = EnumRunTaskType.Time;
        }
        if (Callback != null)
            this.Callback = Callback;
        this.executionCount = executionCount;
        this.Conditions = Conditions;
        runTaskStructHandle.StartTask(executeNow);
    }


    /// <summary>
    /// 暂停执行任务
    /// </summary>
    public void StopTask()
    {
        runTaskStructHandle.StopTask();
    }

    /// <summary>
    /// 将执行任务结构的id取出来
    /// </summary>
    /// <param name="target">对象</param>
    public static implicit operator int(RunTaskStruct target)
    {
        return target != null ? target.id : -1;
    }

    /// <summary>
    /// 执行任务结构的操作手柄
    /// </summary>
    public class RunTaskStructHandle
    {

        /// <summary>
        /// 临时时间
        /// </summary>
        float tempTime;
        /// <summary>
        /// 临时帧
        /// </summary>
        float tempFrame;
        /// <summary>
        /// 目标
        /// </summary>
        RunTaskStruct target;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">目标</param>
        public RunTaskStructHandle(RunTaskStruct target)
        {
            this.target = target;
        }

        /// <summary>
        /// 任务状态
        /// </summary>
        public Action<RunTaskStruct, bool> runTaskStateAction;

        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="executeNow">是否立即执行</param>
        public void StartTask(bool executeNow)
        {
            if (runTaskStateAction != null)
                runTaskStateAction(target, true);
            if (executeNow)
            {
                tempTime = -1;
                tempFrame = -1;
            }
            else
            {
                tempTime = target.time;
                tempFrame = target.frame;
            }
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        public void StopTask()
        {
            if (runTaskStateAction != null)
                runTaskStateAction(target, false);
        }

        /// <summary>
        /// 获取速度倍率
        /// </summary>
        /// <returns></returns>
        public float SpeedRate
        {
            get
            {
                return target.speedRate;
            }
        }

        /// <summary>
        /// 获取或设置当前时间
        /// </summary>
        public float NowTime
        {
            get
            {
                float _tempTime = tempTime;
                if (tempTime < 0)
                    tempTime = target.time;
                return _tempTime;
            }
            set
            {
                tempTime = value;
            }
        }

        /// <summary>
        /// 获取或设置当前帧
        /// </summary>
        public float NowFrame
        {
            get
            {
                float _tempFrame = tempTime;
                if (tempFrame < 0)
                    tempFrame = target.frame;
                return _tempFrame;
            }
            set
            {
                tempFrame = value;
            }
        }

        /// <summary>
        /// 获取任务类型
        /// </summary>
        /// <returns></returns>
        public EnumRunTaskType RunTaskType
        {
            get
            {
                return target.RunTaskType;
            }
        }

        /// <summary>
        /// 获取回调函数
        /// </summary>
        public Action Callback
        {
            get
            {
                return target.Callback;
            }
        }

        /// <summary>
        /// 获取判断条件函数
        /// </summary>
        public Func<bool> Conditions
        {
            get
            {
                return target.Conditions;
            }
        }

        /// <summary>
        /// 获取是否存在下一次调用
        /// </summary>
        public bool HasNext
        {
            get
            {
                if (target.executionCount > 0)//当前值大于0
                {
                    target.executionCount--;//调用次数减1
                    if (target.executionCount <= 0)//如果减去后的值小于等于0,则没有下一次调用
                        return false;
                    else//否则还可以继续调用
                        return true;
                }
                else //如果当前值小于0则一直存在下一次调用
                    return true;
            }
        }
    }
}

/// <summary>
/// 执行任务的类型
/// </summary>
public enum EnumRunTaskType
{
    /// <summary>
    /// 不执行的任务
    /// </summary>
    None,
    /// <summary>
    /// 按帧
    /// </summary>
    Frame,
    /// <summary>
    /// 按时间
    /// </summary>
    Time,
}

