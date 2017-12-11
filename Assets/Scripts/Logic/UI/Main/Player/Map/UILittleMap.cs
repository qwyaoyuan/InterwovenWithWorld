using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小地图
/// </summary>
public class UILittleMap : MonoBehaviour
{
    /// <summary>
    /// 地图控件 
    /// </summary>
    public UIMap uiMapControl;
    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;
    /// <summary>
    /// 游戏状态对象
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 地图状态
    /// </summary>
    IMapState iMapState;
    /// <summary>
    /// npc数据
    /// </summary>
    NPCData npcData;
    /// <summary>
    /// 当前任务状态
    /// </summary>
    INowTaskState iNowTaskState;

    /// <summary>
    /// 任务运行时数据
    /// </summary>
    RuntimeTasksData runTimeTasksData;

    void Start()
    {
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iMapState = GameState.Instance.GetEntity<IMapState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        npcData = DataCenter.Instance.GetMetaData<NPCData>();
        runTimeTasksData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        ResetMap();
        GameState.Instance.Registor<IGameState>(IGamgeStateChaged);
    }

    private void IGamgeStateChaged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, string>(temp => temp.SceneName)))
            ResetMap();
    }

    /// <summary>
    /// 重新设置地图
    /// </summary>
    private void ResetMap()
    {
        if (iMapState.MapBackSprite == null)
            return;
        //根据场景初始化地图
        uiMapControl.InitMap(iMapState.MapBackSprite, iMapState.MaskMapSprite, iMapState.MapRectAtScene, 0.05f);
        //重回全地图的图标
        //npc与路牌
        NPCDataInfo[] npcDataInfos = npcData.GetNPCDataInfos(iGameState.SceneName);
        foreach (NPCDataInfo npcDataInfo in npcDataInfos)
        {
            UIMapIconStruct uiMapIconStruct = uiMapControl.AddIcon(npcDataInfo.NPCSprite, new Vector2(10, 10), new Vector2(npcDataInfo.NPCLocation.x, npcDataInfo.NPCLocation.z));
            object[] innerValue = new object[]
            {
                 npcDataInfo.NPCType!= EnumNPCType.Street?EnumBigMapIconCheck.Action : EnumBigMapIconCheck.Street,
                npcDataInfo
            };
            uiMapIconStruct.value = innerValue;
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
        }
        //根据玩家位置设置地图中心位置
        MoveToCenter();
    }

    void Update()
    {
        MoveToCenter();
    }

    void MoveToCenter()
    {
        if (iPlayerState.PlayerObj == null)
            return;
        //始终移动地图中心到玩家处
        Vector2 playerLocation = new Vector2(iPlayerState.PlayerObj.transform.position.x, iPlayerState.PlayerObj.transform.position.z);
        uiMapControl.MoveToTerrainPoint(playerLocation);
    }
}
