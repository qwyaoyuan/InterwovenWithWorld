using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IGameState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IGameState
{

    #region IGameState

    /// <summary>
    /// 游戏运行状态
    /// </summary>
    private EnumGameRunType _GameRunType;
    /// <summary>
    /// 游戏运行状态
    /// </summary>
    public EnumGameRunType GameRunType
    {
        get { return _GameRunType; }
        set
        {
            EnumGameRunType tempGameRunType = _GameRunType;
            _GameRunType = value;
            if (tempGameRunType != _GameRunType)
                Call<IGameState, EnumGameRunType>(temp => temp.GameRunType);
        }
    }

    /// <summary>
    /// 更改场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="playerLocation">玩家的位置</param>
    /// <param name="LoadResultAction">加载结果回调</param>
    public void ChangedScene(string sceneName, Vector3 playerLocation, Action<bool> LoadResultAction = null)
    {
        //如果要切换的场景不是当前场景则加载场景
        if (SceneName != sceneName)
        {
            UIChangeScene.Instance.LoadScene(sceneName, result =>
            {
                //自身调用初始化数据
                Debug.Log("需要调用自身的函数实现数据的初始化,如地图显示,npc位置等一系列的数据");
                //加载地图的图片以及遮罩资源
                IMapState iMapState = GetEntity<IMapState>();
                MapData mapData = DataCenter.Instance.GetMetaData<MapData>();
                MapDataInfo mapDataInfo = mapData[sceneName];
                if (mapDataInfo != null)
                {
                    mapDataInfo.Load();
                    iMapState.MapBackSprite = mapDataInfo.MapSprite;
                    Sprite mapMaskSprite = playerState.GetSceneMapMaskSprite(sceneName, mapDataInfo.MapSprite);
                    iMapState.MaskMapSprite = mapMaskSprite;
                    iMapState.MapRectAtScene = mapDataInfo.SceneRect;
                }
                //初始化npc与npc的位置
                NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
                NPCDataInfo[] npcDataInfos = npcData.GetNPCDataInfos(sceneName);
                foreach (NPCDataInfo npcDataInfo in npcDataInfos)
                {
                    npcDataInfo.Load();//切换场景是有时候会导致游戏对象被删除,需要重新Load
                }
                //创建玩家操纵的游戏对象
                GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
                GameObject.Instantiate(playerPrefab);
                //更改过后修改玩家位置
                PlayerObj.transform.position = playerLocation;
                //回调
                if (LoadResultAction != null)
                    LoadResultAction(result);
            });
            SceneName = sceneName;
        }
        else//如果不需要切换场景则直接更改玩家位置即可
        {
            //更改过后修改玩家位置
            PlayerObj.transform.position = playerLocation;
        }
    }

    /// <summary>
    /// 当前的场景名 
    /// </summary>
    private string _SceneName;
    /// <summary>
    /// 当前的场景名
    /// </summary>
    public string SceneName
    {
        get { return _SceneName == null ? "" : _SceneName; }
        private set
        {
            string tempSceneName = _SceneName;
            _SceneName = value;
            if (!string.Equals(tempSceneName, _SceneName))
                Call<IGameState, string>(temp => temp.SceneName);
        }
    }

    /// <summary>
    /// 镜头的移动速度
    /// </summary>
    private Vector2 _CameraRotateSpeed;
    /// <summary>
    /// 镜头的移动速度
    /// </summary>
    public Vector2 CameraRotateSpeed
    {
        get { return _CameraRotateSpeed; }
        set
        {
            Vector2 tempCameraRotateSpeed = _CameraRotateSpeed;
            _CameraRotateSpeed = value;
            if (!Vector2.Equals(tempCameraRotateSpeed, _CameraRotateSpeed))
                Call<IGameState, Vector2>(temp => temp.CameraRotateSpeed);
        }
    }

    /// <summary>
    /// 镜头与对象的Y轴夹角范围
    /// </summary>
    private Vector2 _CameraYAngleRange;
    /// <summary>
    /// 镜头与对象的夹角范围 
    /// </summary>
    public Vector2 CameraYAngleRange
    {
        get { return _CameraYAngleRange; }
        set
        {
            Vector2 tempCameraYAngleRange = _CameraYAngleRange;
            _CameraYAngleRange = value;
            if (_CameraYAngleRange.x > _CameraYAngleRange.y)
            {
                float tempX = _CameraYAngleRange.x;
                _CameraYAngleRange.x = _CameraYAngleRange.y;
                _CameraYAngleRange.y = tempX;
            }
            if (!Vector2.Equals(_CameraYAngleRange, tempCameraYAngleRange))
                Call<IGameState, Vector2>(temp => temp.CameraYAngleRange);
        }
    }


    #endregion

}
