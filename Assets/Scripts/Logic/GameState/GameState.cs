﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// 游戏状态
/// </summary>
public partial class GameState : IEntrance, IBaseState
{
    /// <summary>
    /// 单例对象
    /// </summary>
    public static GameState Instance;

    /// <summary>
    /// 回调字典
    /// </summary>
    Dictionary<Type, List<KeyValuePair<object, Action<IBaseState, string>>>> callBackDic;

    /// <summary>
    /// 玩家状态
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// 运行时任务对象
    /// </summary>
    RuntimeTasksData runtimeTaskData;
    /// <summary>
    /// 等级数据
    /// </summary>
    LevelData levelData;

    public void Start()
    {
        Instance = this;
        callBackDic = new Dictionary<Type, List<KeyValuePair<object, Action<IBaseState, string>>>>();
        Type[] allType = GetType().GetInterfaces();
        foreach (Type type in allType)
        {
            if (type.IsInterface
                && !Type.Equals(type, typeof(IBaseState))
                && type.GetInterface(typeof(IBaseState).Name) != null)
            {
                List<KeyValuePair<object, Action<IBaseState, string>>> callBackList = new List<KeyValuePair<object, Action<IBaseState, string>>>();
                callBackDic.Add(type, callBackList);
            }
        }
    }

    /// <summary>
    /// 在加载存档后调用来初始化数据
    /// </summary>
    public void LoadArchive()
    {
        //初始化共有数据
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        runtimeTaskData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        levelData = DataCenter.Instance.GetMetaData<LevelData>();
        //其他的加载初始化
        Load_INowTaskState();
        LoadIPlayerState();
    }

    public void Update()
    {
        Update_INowTaskState();
    }

    /// <summary>
    /// 注册监听状态改变回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="CallbackAction"></param>
    public void Registor<T>(Action<T, string> CallbackAction) where T : IBaseState
    {
        if (CallbackAction == null)
            return;
        if (typeof(T).IsInterface
            && !Type.Equals(typeof(T), typeof(IBaseState))
            && typeof(T).GetInterface(typeof(IBaseState).Name) != null)
        {
            if (!callBackDic.ContainsKey(typeof(T)))
                return;
            List<KeyValuePair<object, Action<IBaseState, string>>> callBackList = callBackDic[typeof(T)];
            if (callBackList != null && !callBackList.Select(temp => temp.Key).Contains(CallbackAction))
            {
                Action<IBaseState, string> tempAction = (iBaseState, fieldName) =>
                {
                    try
                    {
                        T t = (T)iBaseState;
                        CallbackAction(t, fieldName);
                    }
                    catch { }
                };
                callBackList.Add(new KeyValuePair<object, Action<IBaseState, string>>(CallbackAction, tempAction));
            }
        }
    }

    /// <summary>
    /// 移除注册
    /// </summary>
    /// <param name="d"></param>
    public void UnRegistor<T>(Action<T, string> CallbackAction)
    {
        if (CallbackAction == null)
            return;
        foreach (KeyValuePair<Type, List<KeyValuePair<object, Action<IBaseState, string>>>> item in callBackDic)
        {
            List<KeyValuePair<object, Action<IBaseState, string>>> actionList = item.Value;
            int index = actionList.Select(temp => temp.Key).ToList().IndexOf(CallbackAction);
            if (index > -1)
            {
                actionList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 获取实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetEntity<T>() where T : IBaseState
    {
        return (T)(object)this;
    }

    /// <summary>
    /// 获取字段名
    /// 也可以传入一个函数,但是注意如果函数和自动重名则会认为这是一个
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="expr"></param>
    /// <returns></returns>
    public string GetFieldName<T, U>(Expression<Func<T, U>> expr)
    {
        string propertyName = string.Empty;
        if (expr.Body is MemberExpression)
        {
            propertyName = ((MemberExpression)expr.Body).Member.Name;
        }
        else if (expr.Body is UnaryExpression)
        {
            Expression expressionOperand = (expr.Body as UnaryExpression).Operand;
            if (expressionOperand is MethodCallExpression)
            {
                MethodCallExpression methodCallExpression = expressionOperand as MethodCallExpression;
                if (methodCallExpression.Arguments.Count == 3)
                {
                    Expression expression1 = methodCallExpression.Arguments[2];
                    MethodInfo methodInfo = (expression1 as ConstantExpression).Value as MethodInfo;
                    if (methodInfo != null)
                    {
                        propertyName = methodInfo.Name;
                    }
                }
            }
        }
        return propertyName;
    }

    /// <summary>
    /// 回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fieldName"></param>
    private void Call<T, U>(Expression<Func<T, U>> expr) where T : IBaseState
    {
        string propertyName = string.Empty;
        if (expr.Body is MemberExpression)
        {
            propertyName = ((MemberExpression)expr.Body).Member.Name;
            List<KeyValuePair<object, Action<IBaseState, string>>> actionList = null;
            if (callBackDic.TryGetValue(typeof(T), out actionList) && actionList != null)
            {
                foreach (KeyValuePair<object, Action<IBaseState, string>> item in actionList)
                {
                    try
                    {
                        item.Value(this, propertyName);
                    }
                    catch { }
                }
            }
        }
    }

    public void OnDestroy()
    {

    }

    #region 声明函数分部
    /// <summary>
    /// (任务)加载函数调用时调用
    /// </summary>
    partial void Load_INowTaskState();

    /// <summary>
    /// 任务的更新函数
    /// </summary>
    partial void Update_INowTaskState();

    /// <summary>
    /// (玩家状态)加载函数时调用
    /// </summary>
    partial void LoadIPlayerState();

    #endregion
}

/// <summary>
/// 基础状态，所有的状态接口都必须继承自本接口
/// </summary>
public interface IBaseState
{
    /// <summary>
    /// 注册监听状态改变回调
    /// </summary>
    /// <param name="target"></param>
    /// <param name="CallBackAction"></param>
    void Registor<T>(Action<T, string> CallBackAction) where T : IBaseState;
    /// <summary>
    /// 移除注册
    /// </summary>
    /// <param name="CallbackAction"></param>
    void UnRegistor<T>(Action<T, string> CallbackAction);
    /// <summary>
    /// 获取字段或属性名 
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    string GetFieldName<T, U>(Expression<Func<T, U>> expr);
}
