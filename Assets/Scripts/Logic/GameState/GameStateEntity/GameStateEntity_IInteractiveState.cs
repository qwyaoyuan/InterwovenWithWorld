using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 实现了IInteractiveState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IInteractiveState
{

    /// <summary>
    /// 主线的中间过度展示
    /// </summary>
    GameObject _InterludeObj;
    /// <summary>
    /// 主线的中间过度展示
    /// </summary>
    public GameObject InterludeObj
    {
        get { return _InterludeObj; }
        set
        {
            GameObject tempInterludeObj = _InterludeObj;
            _InterludeObj = value;
            if (_InterludeObj != tempInterludeObj && _InterludeObj != null)
                Call<IInteractiveState, GameObject>(temp => temp.InterludeObj);
        }
    }

    /// <summary>
    /// 支线的选择展示
    /// </summary>
    GameObject _QueryObj;
    /// <summary>
    /// 支线的选择展示
    /// </summary>
    public GameObject QueryObj
    {
        get { return _QueryObj; }
        set
        {
            GameObject tempQueryObj = _QueryObj;
            _QueryObj = value;
            if (_QueryObj != tempQueryObj && _QueryObj != null)
                Call<IInteractiveState, GameObject>(temp => temp.QueryObj);
        }
    }

    /// <summary>
    /// 点击的的NPC
    /// 在设置时判断使用那个UI来展示
    /// </summary>
    int _ClickInteractiveNPCID;
    /// <summary>
    /// 点击的的NPC(用于交任务或者展开选择任务)
    /// 在设置时判断使用那个UI来展示
    /// </summary>
    public int ClickInteractiveNPCID
    {
        get { return _ClickInteractiveNPCID; }
        set
        {
            switch (GameRunType)
            {
                case EnumGameRunType.Safe:
                case EnumGameRunType.Unsafa:
                    _ClickInteractiveNPCID = value;
                    //先判断是否完成了任务
                    bool checkNPCTaskResult = CheckNowTask(EnumCheckTaskType.NPC, _ClickInteractiveNPCID);
                    if (checkNPCTaskResult)//如果任务完成了则不用后面的检测了
                        return;
                    //如果任务没有完成则需要后面的检测
                    //接取任务的npcid与点击npcid相同并且存在的任务没有被完成也没有被接取
                    TaskMap.RunTimeTaskInfo[] runTimeTaskInfos = runtimeTaskData.GetAllToDoList()
                        .Where(temp => temp.TaskInfoStruct.ReceiveTaskNpcId == _ClickInteractiveNPCID && temp.IsOver == false && temp.IsStart == false)
                        .ToArray();
                    //存在主线任务则展开主线
                    if (runTimeTaskInfos.Where(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Main).Count() > 0)
                    {
                        if (InterludeObj != null)
                            InterludeObj.SetActive(true);
                        return;
                    }
                    //如果存在支线则展开支线
                    if (runTimeTaskInfos.Where(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Spur).Count() > 0)
                    {
                        if (QueryObj != null)
                            QueryObj.SetActive(true);
                        return;
                    }
                    //如果存在重复任务则展开重复任务
                    if (runTimeTaskInfos.Where(temp => temp.TaskInfoStruct.TaskType == TaskMap.Enums.EnumTaskType.Repeat).Count() > 0)
                    {
                        if (QueryObj != null)
                            QueryObj.SetActive(true);
                        return;
                    }
                    //如果不存在任何任务则在任务头顶显示文字
                    break;
            }
        }
    }

    /// <summary>
    /// 合成面板
    /// </summary>
    GameObject _SynthesisObj;
    /// <summary>
    /// 合成面板
    /// </summary>
    public GameObject SynthesisObj
    {
        get { return _SynthesisObj; }
        set
        {
            GameObject tempSynthesisObj = _SynthesisObj;
            _SynthesisObj = value;
            if (_SynthesisObj != tempSynthesisObj && _SynthesisObj != null)
                Call<IInteractiveState, GameObject>(temp => temp.SynthesisObj);
        }
    }

    /// <summary>
    /// 点击的采集点ID
    /// </summary>
    private int _ClickInteractiveStuffID;
    /// <summary>
    ///点击的采集点ID
    /// </summary>
    public int ClickInteractiveStuffID
    {
        get { return _ClickInteractiveStuffID; }
        set
        {
            switch (GameRunType)
            {
                case EnumGameRunType.Safe:
                case EnumGameRunType.Unsafa:
                    _ClickInteractiveStuffID = value;
                    //点击一次获得一个物品
                    StuffData stuffData = DataCenter.Instance.GetMetaData<StuffData>();
                    IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
                    PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
                    if (stuffData == null || iGameState == null || playerState == null)
                        return;
                    //首先判断采集点是否存在物品
                    StuffDataInfo stuffDataInfo = stuffData.GetStuffDataInfo(iGameState.SceneName, _ClickInteractiveStuffID);
                    if (stuffDataInfo == null)
                        return;
                    int residualCount = stuffDataInfo.ResidualCount();
                    if (residualCount > 0)
                    {
                        stuffDataInfo.Collect(residualCount);
                        stuffDataInfo.Update();
                        PlayGoods playGoods = playerState.PlayerAllGoods.FirstOrDefault(temp => temp.GoodsInfo.EnumGoodsType == stuffDataInfo.StuffType);
                        if (playGoods == null)
                        {
                            int id = NowTimeToID.GetNowID(DataCenter.Instance.GetMetaData<GameRunnedState>());
                            if (playerState.PlayerAllGoods.Count(temp => temp.ID == id) == 0)
                            {
                                GoodsMetaInfoMations goodsMetaInfoMations = DataCenter.Instance.GetMetaData<GoodsMetaInfoMations>();
                                Goods goods = goodsMetaInfoMations[stuffDataInfo.StuffType];
                                if (goods != null)
                                {
                                    playGoods = new PlayGoods(id, goods.Clone(true), GoodsLocation.Package);
                                    playerState.PlayerAllGoods.Add(playGoods);
                                }
                            }
                        }
                        if (playGoods != null)
                        {
                            playGoods.Count += residualCount;
                        }
                    }
                    break;
            }
        }
    }
}
