using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑的入口
/// </summary>
public class MainEntrance : MonoBehaviour
{
    /// <summary>
    /// 入口集合
    /// </summary>
    List<IEntrance> iEntranceList;

    /// <summary>
    /// 如果单例对象
    /// </summary>
    public static MainEntrance Instance;

    void Awake()
    {
        iEntranceList = new List<IEntrance>();
        Instance = this;
    }

    /// <summary>
    /// 一些基础单例类的注册
    /// </summary>
    void Start()
    {
        Registor(new TaskTools());
        Registor(new SkillDealHandle());
        Registor(new GameState());
        Registor(new InputControl());
    }

    void Update()
    {
        foreach (IEntrance iEntrace in iEntranceList)
        {
            iEntrace.Update();
        }
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="iEntrance">入口对象</param>
    public void Registor(IEntrance iEntrance)
    {
        if (!iEntranceList.Contains(iEntrance))
        {
            iEntranceList.Add(iEntrance);
            iEntrance.Start();
        }
    }

    /// <summary>
    /// 接触注册
    /// </summary>
    /// <param name="iEntrance">入口对象</param>
    public void Unregistor(IEntrance iEntrance)
    {
        if (iEntranceList.Contains(iEntrance))
        {
            iEntranceList.Remove(iEntrance);
            iEntrance.OnDestroy();
        }
    }
}
