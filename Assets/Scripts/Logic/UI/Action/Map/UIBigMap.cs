using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大地图
/// </summary>
public class UIBigMap : MonoBehaviour
{
    /// <summary>
    /// 显示设置面板
    /// </summary>
    [SerializeField]
    RectTransform showSettingPanel;

    /// <summary>
    /// 地图控件
    /// </summary>
    [SerializeField]
    UIMap uiMapControl;

    /// <summary>
    /// 解释面板
    /// </summary>
    [SerializeField]
    RectTransform explanPanel;

    /// <summary>
    /// 图标大小
    /// </summary>
    public Vector2 iconSize = new Vector2(30, 30);

    /// <summary>
    /// 标签路径
    /// 显示哪些UI
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前选中的对象
    /// </summary>
    UIFocus nowUIFocus;
    /// <summary>
    /// 大地图的操作状态
    /// </summary>
    EnumBigMapOperateState bigMapOperateState;

    /// <summary>
    /// 标记对象
    /// </summary>
    UIMapIconStruct flagIconStruct;

    /// <summary>
    /// 地图状态
    /// </summary>
    IMapState iMapState;
    /// <summary>
    /// npc数据
    /// </summary>
    NPCData npcData;
    /// <summary>
    /// 游戏状态对象
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;
    /// <summary>
    /// 当前任务状态
    /// </summary>
    INowTaskState iNowTaskState;

    /// <summary>
    /// 地图图标对象集合
    /// </summary>
    List<UIMapIconStruct> uiMapIconStructList;

    /// <summary>
    /// 任务运行时数据
    /// </summary>
    RuntimeTasksData runTimeTasksData;

    private void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
        uiMapControl.ClickOnMap += UiMapControl_ClickOnMap;
    }

    /// <summary>
    /// 在地图上点击了鼠标
    /// </summary>
    /// <param name="terrainPos"></param>
    private void UiMapControl_ClickOnMap(Vector2 terrainPos)
    {
        ActionAtTerrain(terrainPos);
    }

    /// <summary>
    /// 控件激活时
    /// </summary>
    private void OnEnable()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        iMapState = GameState.Instance.GetEntity<IMapState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        npcData = DataCenter.Instance.GetMetaData<NPCData>();
        runTimeTasksData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        bigMapOperateState = EnumBigMapOperateState.OperateMap;
        showSettingPanel.gameObject.SetActive(false);
        ResetMap();
    }

    /// <summary>
    /// 控件隐藏时
    /// </summary>
    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    /// <summary>
    /// 重新设置地图
    /// </summary>
    private void ResetMap()
    {
        //根据场景初始化地图
        uiMapControl.InitMap(iMapState.MapBackSprite, iMapState.MaskMapSprite, iMapState.MapRectAtScene);
        //重绘全地图的图标
        uiMapIconStructList = new List<UIMapIconStruct>();
        //npc与路牌
        NPCDataInfo[] npcDataInfos = npcData.GetNPCDataInfos(iGameState.SceneName);
        foreach (NPCDataInfo npcDataInfo in npcDataInfos)
        {
            UIMapIconStruct uiMapIconStruct = uiMapControl.AddIcon(npcDataInfo.NPCSprite, new Vector2(20, 20), new Vector2(npcDataInfo.NPCLocation.x, npcDataInfo.NPCLocation.z));
            object[] innerValue = new object[]
            {
                npcDataInfo.NPCType!= EnumNPCType.Street?EnumBigMapIconCheck.Action : EnumBigMapIconCheck.Street,
                npcDataInfo
            };
            uiMapIconStruct.value = innerValue;
            uiMapIconStructList.Add(uiMapIconStruct);
        }
        //任务
        //等待接取的任务
        RunTimeTaskInfo[] runTimeTaskInfos_Wait = iNowTaskState.GetWaitTask(iGameState.SceneName);
        foreach (RunTimeTaskInfo runTimeTaskInfo in runTimeTaskInfos_Wait)
        {
            NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, runTimeTaskInfo.RunTimeTaskNode.ReceiveTaskNpcId);//接取任务的NPC
            if (npcDataInfo == null)
                continue;
            //需要传入一个金色的叹号
            UIMapIconStruct uiMapIconStruct = uiMapControl.AddIcon(null, new Vector2(20, 30),
                new Vector2(npcDataInfo.NPCLocation.x, npcDataInfo.NPCLocation.z));
            object[] innerValue = new object[]
            {
                EnumBigMapIconCheck.Task,
                runTimeTaskInfo,
                0//0表示等待接取,1表示正在执行,2表示已经完成
            };
            uiMapIconStruct.value = innerValue;
            uiMapIconStructList.Add(uiMapIconStruct);
        }
        //正在执行的任务
        RunTimeTaskInfo[] runTimeTaskInfos_Start = iNowTaskState.GetStartTask(iGameState.SceneName);
        foreach (RunTimeTaskInfo runTimeTaskInfo in runTimeTaskInfos_Start)
        {
            Vector2 targetPosition = Vector2.zero;
            if (runTimeTaskInfo.RunTimeTaskNode.NowArrivedPosition != Vector3.zero)
                targetPosition = new Vector2(runTimeTaskInfo.RunTimeTaskNode.NowArrivedPosition.x, runTimeTaskInfo.RunTimeTaskNode.NowArrivedPosition.z);
            else
            {
                NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, runTimeTaskInfo.RunTimeTaskNode.DeliveryTaskNpcId);//交付任务的NPC
                if (npcDataInfo != null)
                    targetPosition = new Vector2(npcDataInfo.NPCLocation.x, npcDataInfo.NPCLocation.z);
            }
            //需要传入一个白色的的问号
            UIMapIconStruct uiMapIconStruct = uiMapControl.AddIcon(null, new Vector2(20, 30), targetPosition);
            object[] innerValue = new object[]
            {
                EnumBigMapIconCheck.Task,
                runTimeTaskInfo,
                1//0表示等待接取,1表示正在执行,2表示已经完成
            };
            uiMapIconStruct.value = innerValue;
            uiMapIconStructList.Add(uiMapIconStruct);
        }
        //条件达成但是没有交付的任务
        RunTimeTaskInfo[] runTimeTaskInfos_End = iNowTaskState.GetStartTask(iGameState.SceneName);
        foreach (RunTimeTaskInfo runTimeTaskInfo in runTimeTaskInfos_End)
        {
            NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, runTimeTaskInfo.RunTimeTaskNode.DeliveryTaskNpcId);//交付任务的NPC
            if (npcDataInfo == null)
                continue;
            //需要传入一个金色的问号
            UIMapIconStruct uiMapIconStruct = uiMapControl.AddIcon(null, new Vector2(20, 30),
                new Vector2(npcDataInfo.NPCLocation.x, npcDataInfo.NPCLocation.z));
            object[] innerValue = new object[]
            {
                EnumBigMapIconCheck.Task,
                runTimeTaskInfo,
                2//0表示等待接取,1表示正在执行,2表示已经完成
            };
            uiMapIconStruct.value = innerValue;
            uiMapIconStructList.Add(uiMapIconStruct);
        }
        //根据玩家位置设置地图中心位置
        Vector2 playerLocation = new Vector2(iPlayerState.PlayerObj.transform.position.x, iPlayerState.PlayerObj.transform.position.z);
        uiMapControl.MoveToTerrainPoint(playerLocation);
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (bigMapOperateState)
        {
            case EnumBigMapOperateState.OperateMap://操作地图
                switch (keyType)
                {
                    case UIManager.KeyType.A:
                        Vector2 handlePos = uiMapControl.GetHandlePosInTerrain();
                        ActionAtTerrain(handlePos);
                        break;
                    case UIManager.KeyType.Y:
                        bigMapOperateState = EnumBigMapOperateState.CheckSetting;
                        showSettingPanel.gameObject.SetActive(true);
                        if (uiFocusPath)
                            nowUIFocus = uiFocusPath.GetFirstFocus();
                        if (nowUIFocus)
                            nowUIFocus.SetForcus();
                        break;
                    case UIManager.KeyType.LEFT_ROCKER:
                        uiMapControl.MoveHandle(rockValue);
                        break;
                }
                break;
            case EnumBigMapOperateState.CheckSetting://操作设置
                if (!nowUIFocus)
                    if (uiFocusPath)
                        nowUIFocus = uiFocusPath.GetFirstFocus();
                if (nowUIFocus)
                {
                    Action<UIFocusPath.MoveType> MoveUIFocusAction = (moveType) =>
                    {
                        UIFocus nextUIFocus = uiFocusPath.GetNextFocus(nowUIFocus, moveType);
                        if (nextUIFocus != null)
                        {
                            nowUIFocus.LostForcus();
                            nowUIFocus = nextUIFocus;
                            nowUIFocus.SetForcus();
                        }
                    };
                    switch (keyType)
                    {
                        case UIManager.KeyType.A:
                            if (nowUIFocus.GetType().Equals(typeof(UIFocusButton)))
                            {
                                ((UIFocusButton)nowUIFocus).ClickThisButton();
                            }
                            else if (nowUIFocus.GetType().Equals(typeof(UIFocusToggle)))
                            {
                                ((UIFocusToggle)nowUIFocus).MoveChild(UIFocusPath.MoveType.OK);
                            }
                            break;
                        case UIManager.KeyType.B:
                            ExitSetting_Click();
                            break;
                        case UIManager.KeyType.LEFT:
                            MoveUIFocusAction(UIFocusPath.MoveType.LEFT);
                            break;
                        case UIManager.KeyType.RIGHT:
                            MoveUIFocusAction(UIFocusPath.MoveType.RIGHT);
                            break;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 点击退出设置按钮
    /// </summary>
    public void ExitSetting_Click()
    {
        if (nowUIFocus)
            nowUIFocus.LostForcus();
        bigMapOperateState = EnumBigMapOperateState.OperateMap;
    }

    /// <summary>
    /// 在地图相应的场景坐标点击或按下按钮触发的功能
    /// 1点击路牌标记可以进行传送
    /// 2点击地图或者任务功能npcboss等标记位置
    /// </summary>
    /// <param name="terrainPos"></param>
    private void ActionAtTerrain(Vector2 terrainPos)
    {
        //清理当前的标记图标
        Action ClearFlagIconStruct = () =>
        {
            if (!flagIconStruct)
                return;
            try
            {
                object[] innerValue = (object[])flagIconStruct.value;
                EnumBigMapIconCheck enumBigMapIconCheck = (EnumBigMapIconCheck)innerValue[0];
                if (enumBigMapIconCheck == EnumBigMapIconCheck.Flag)//如果该标记已经是标记则移除该图标
                {
                    uiMapControl.RemoveIcon(flagIconStruct);
                    flagIconStruct = null;
                }
            }
            catch { }
        };

        UIMapIconStruct uiMapIconStruct = uiMapControl.GetTouchOrClickIcon();
        if (uiMapIconStruct)//获取不为空则判断类型
        {
            try
            {
                object[] innerValue = (object[])flagIconStruct.value;
                EnumBigMapIconCheck enumBigMapIconCheck = (EnumBigMapIconCheck)innerValue[0];
                switch (enumBigMapIconCheck)
                {
                    case EnumBigMapIconCheck.Task:
                    case EnumBigMapIconCheck.Action:
                    case EnumBigMapIconCheck.Boss:
                        //重设图标
                        ClearFlagIconStruct();
                        flagIconStruct = uiMapIconStruct;
                        break;
                    case EnumBigMapIconCheck.Flag:
                        //清理图标 
                        ClearFlagIconStruct();
                        uiMapControl.RemoveIcon(uiMapIconStruct);
                        break;
                    case EnumBigMapIconCheck.Street:
                        //传送
                        {
                            NPCDataInfo npcDataInfo = (NPCDataInfo)innerValue[1];
                            iGameState.ChangedScene(npcDataInfo.SceneName, npcDataInfo.NPCLocation + Vector3.one);
                        }
                        break;
                }
            }
            catch { }
        }
        else//获取为空则设置标记
        {
            if (flagIconStruct)//如果当前标记不等于空
            {
                ClearFlagIconStruct();
                //在该处设置图标 
                Sprite flagSprit = null;
                flagIconStruct = uiMapControl.AddIcon(flagSprit, iconSize, terrainPos);
                flagIconStruct.value = new object[] { EnumBigMapIconCheck.Flag, null };
            }
        }
    }

    /// <summary>
    /// 大地图的操作状态枚举
    /// </summary>
    enum EnumBigMapOperateState
    {
        /// <summary>
        /// 操作地图状态
        /// </summary>
        OperateMap,
        /// <summary>
        /// 选择设置状态
        /// </summary>
        CheckSetting
    }
}
