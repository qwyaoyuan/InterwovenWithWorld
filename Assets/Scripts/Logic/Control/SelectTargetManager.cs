using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 选择目标管理器 
/// 负责选择目标
/// </summary>
public class SelectTargetManager : IInput
{
    /// <summary>
    /// 选择目标管理器私有静态对象 
    /// </summary>
    private static SelectTargetManager instance;
    /// <summary>
    /// 选择目标管理器的但离对象
    /// </summary>
    public static SelectTargetManager Instance;

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;
    /// <summary>
    /// 怪物集合对象
    /// </summary>
    IMonsterCollection iMonsterCollection;

    private SelectTargetManager()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        iMonsterCollection = GameState.Instance.GetEntity<IMonsterCollection>();
        //注册对选择模式发生变化的监听
        iGameState.Registor<IGameState>(IGameState_Changed);
    }

    /// <summary>
    /// IGameState对象发生变化
    /// </summary>
    /// <param name="iGamaState"></param>
    /// <param name="fieldName"></param>
    private void IGameState_Changed(IGameState iGamaState, string fieldName)
    {
        if (string.Equals(fieldName, iGamaState.GetFieldName<IGameState, EnumSelectTargetModel>(temp => temp.SelectTargetModel)))
        {
            switch (iGameState.SelectTargetModel)
            {
                case EnumSelectTargetModel.None:
                case EnumSelectTargetModel.SelectMonster:
                    iPlayerState.SelectObj = null;
                    break;
                case EnumSelectTargetModel.SelectSelf:
                    iPlayerState.SelectObj = iPlayerState.PlayerObj;
                    break;
            }
        }
    }

    /// <summary>
    /// 选择持续时间
    /// </summary>
    float selectTime;

    /// <summary>
    /// 如果当选择目标是空的话,则选择一个怪物
    /// </summary>
    private void Update()
    {
        selectTime -= Time.deltaTime;
        if (selectTime < 0 && iGameState.SelectTargetModel == EnumSelectTargetModel.SelectMonster && iPlayerState.SelectObj == null && iPlayerState.PlayerObj != null)
        {
            selectTime = 2;//两秒检测一次
            //选择一个最近的怪物
            GameObject selectObj = iMonsterCollection.GetMonsters(iPlayerState.PlayerObj, -1, 20).FirstOrDefault();
            iPlayerState.SelectObj = selectObj;
        }
    }

    public void KeyDown(int key)
    {

    }

    public void KeyPress(int key)
    {

    }

    public void KeyUp(int key)
    {
        //如果按下了R3则切换状态
        if (key == (int)EnumInputType.R3)
        {
            switch (iGameState.SelectTargetModel)
            {
                case EnumSelectTargetModel.None:
                    iGameState.SelectTargetModel = EnumSelectTargetModel.SelectMonster;
                    break;
                case EnumSelectTargetModel.SelectMonster:
                    iGameState.SelectTargetModel = EnumSelectTargetModel.SelectSelf;
                    break;
                case EnumSelectTargetModel.SelectSelf:
                    iGameState.SelectTargetModel = EnumSelectTargetModel.None;
                    break;
            }
        }
    }

    public void Move(Vector2 forward)
    {

    }

    /// <summary>
    /// 是否更改了View按键
    /// </summary>
    bool changedView;

    public void View(Vector2 view)
    {
        if (view.magnitude > 0.6f && changedView == false)//按键数值大于0.6f并且之前并没有按下按键
        {
            changedView = true;//表示按下了按键
            if (iGameState.SelectTargetModel == EnumSelectTargetModel.SelectMonster && iPlayerState.PlayerObj != null)
            {
                GameObject[] selectObjs = iMonsterCollection.GetMonsters(iPlayerState.PlayerObj, -1, 20);
                if (selectObjs.Length > 0)
                {
                    float value = view.x + view.y;
                    if (value >= 0)//寻找相对较远的怪物
                    {
                        if (iPlayerState.SelectObj != null)
                        {
                            int index = selectObjs.ToList().IndexOf(iPlayerState.SelectObj);
                            if (index < 0)
                                iPlayerState.SelectObj = selectObjs.LastOrDefault();
                            else if (index < selectObjs.Length - 1)
                            {
                                iPlayerState.SelectObj = selectObjs[index + 1];
                            }
                        }
                        else
                        {
                            iPlayerState.SelectObj = selectObjs.LastOrDefault();
                        }
                    }
                    else//寻找相对较近的怪物
                    {
                        if (iPlayerState.SelectObj != null)
                        {
                            int index = selectObjs.ToList().IndexOf(iPlayerState.SelectObj);
                            if (index < 0)
                                iPlayerState.SelectObj = selectObjs.FirstOrDefault();
                            else if (index > 1)
                            {
                                iPlayerState.SelectObj = selectObjs[index - 1];
                            }
                        }
                        else
                        {
                            iPlayerState.SelectObj = selectObjs.FirstOrDefault();
                        }
                    }
                }
            }
        }
        if (view.magnitude < 0.2f)//如果松开了View键
        {
            changedView = false;//表示松开了View键
        }

    }
}

