using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现了IGameState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IGameState
{

    /// <summary>
    /// 在加载存档后调用来初始化数据
    /// </summary>
    public void LoadArchive()
    {
        //初始化共有数据
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        runtimeTaskData = DataCenter.Instance.GetEntity<TaskMap.RunTimeTaskData>();
        levelData = DataCenter.Instance.GetMetaData<LevelData>();
        //其他的加载初始化
        Load_INowTaskState();
        Load_IPlayerState();
        Load_IGameState();
        Load_IPlayerState_ISkillState();
        Load_INowTaskStateEvent();
        Load_IEntryState();
        //通知存档加载
        Call<IGameState, Action>(temp => temp.LoadArchive);
    }

    partial void Load_IGameState()
    {
        _GameRunTypeStack = new Stack<EnumGameRunType>();
        _GameRunTypeStack.Push(EnumGameRunType.Start);
    }

    #region IGameState

    /// <summary>
    /// 游戏状态堆栈
    /// </summary>
    private Stack<EnumGameRunType> _GameRunTypeStack;
    /// <summary>
    /// 游戏运行状态
    /// </summary>
    public EnumGameRunType GameRunType
    {
        get
        {
            return _GameRunTypeStack.Peek();

        }
        set
        {
            EnumGameRunType tempGameRunType = GameRunType;//当前的顶层状态
            EnumGameRunType _GameRunType = value;//要直接设置的状态
            _GameRunTypeStack.Clear();
            _GameRunTypeStack.Push(_GameRunType);
            if (tempGameRunType != _GameRunType)
                Call<IGameState, EnumGameRunType>(temp => temp.GameRunType);
        }
    }

    /// <summary>
    /// 将一个游戏运行状态压入栈中
    /// </summary>
    /// <param name="enumGameRunType"></param>
    public void PushEnumGameRunType(EnumGameRunType enumGameRunType)
    {
        if (GameRunType != enumGameRunType)
        {
            _GameRunTypeStack.Push(enumGameRunType);
            Call<IGameState, EnumGameRunType>(temp => temp.GameRunType);
        }
    }

    /// <summary>
    /// 提出最顶层的一个状态
    /// </summary>
    public EnumGameRunType PopEnumGameRunType()
    {
        if (_GameRunTypeStack.Count <= 1)
        {
            return _GameRunTypeStack.Peek();
        }
        EnumGameRunType oldRunType = _GameRunTypeStack.Pop();
        Call<IGameState, EnumGameRunType>(temp => temp.GameRunType);
        return oldRunType;
    }


    /// <summary>
    /// 更改场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="playerLocation">玩家的位置</param>
    /// <param name="LoadResultAction">加载结果回调</param>
    public void ChangedScene(string sceneName, Vector3 playerLocation, Action<bool> LoadResultAction = null)
    {
        GameRunType = EnumGameRunType.Start;//切换场景时当前的状态为开始状态
        //如果要切换的场景不是当前场景则加载场景
        if (SceneName != sceneName)
        {
            //保存原来地形的遮罩图
            if (!string.IsNullOrEmpty(SceneName))
                playerState.SaveGetSceneMapMaskData(SceneName);
            //开始加载
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
                //初始化采集点与采集点的位置
                StuffData stuffData = DataCenter.Instance.GetMetaData<StuffData>();
                StuffDataInfo[] stuffDataInfos = stuffData.GetStuffDataInfos(sceneName);
                foreach (StuffDataInfo stuffDataInfo in stuffDataInfos)
                {
                    stuffDataInfo.Load();//切换场景是有时候会导致游戏对象被删除,需要重新Load
                }
                //创建UI
                GameObject mainCanvasPrefab = Resources.Load<GameObject>("UI/MainCanvas");
                GameObject.Instantiate(mainCanvasPrefab);
                //创建玩家操纵的游戏对象
                GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
                GameObject.Instantiate(playerPrefab);
                //更改过后修改玩家位置
                PlayerObj.transform.position = playerLocation;
                //回调
                if (LoadResultAction != null)
                    LoadResultAction(result);
                SceneName = sceneName;
                GameRunType = EnumGameRunType.Safe;
                //场景变换回调
                Call<IGameState, Action<string, Vector3, Action<bool>>>(temp => temp.ChangedScene);
            });
        }
        else//如果不需要切换场景则直接更改玩家位置即可
        {
            UIChangeScene.Instance.MovePlayer(3, result =>
            {
                GameRunType = EnumGameRunType.Safe;
                //场景变换回调
                Call<IGameState, Action<string, Vector3, Action<bool>>>(temp => temp.ChangedScene);
            });
            //更改过后修改玩家位置
            PlayerObj.transform.position = playerLocation;
            //关闭当前按显示的UI
            IInteractiveState iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
            iInteractiveState.ActionObj.SetActive(false);
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
    /// 镜头的移动速度(如果当前模式是第三人称模式)
    /// </summary>
    private Vector2 _CameraRotateSpeed;
    /// <summary>
    /// 镜头的移动速度(如果当前模式是第三人称模式)
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
    /// 镜头与对象的Y轴夹角范围(如果当前模式是第三人称模式)
    /// </summary>
    private Vector2 _CameraYAngleRange;
    /// <summary>
    /// 镜头与对象的夹角范围 (如果当前模式是第三人称模式)
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

    /// <summary>
    /// 镜头与对象的距离(如果当前模式是第三人称模式)
    /// </summary>
    private float _CameraDistanceOfPlayer;
    /// <summary>
    /// 镜头与对象的距离(如果当前模式是第三人称模式)
    /// </summary>
    public float CameraDistanceOfPlayer
    {
        get { return _CameraDistanceOfPlayer; }
        set
        {
            float tempCameraDistanceOfPlayer = _CameraDistanceOfPlayer;
            _CameraDistanceOfPlayer = value;
            if (tempCameraDistanceOfPlayer != _CameraDistanceOfPlayer)
            {
                Call<IGameState, float>(temp => temp.CameraDistanceOfPlayer);
            }
        }
    }

    /// <summary>
    /// 镜头朝向目标时向Z轴方向的偏差(如果当前模式是固定视角模式)
    /// </summary>
    private float _CameraArmOffsetZ;
    /// <summary>
    /// 镜头朝向目标时向Z轴方向的偏差(如果当前模式是固定视角模式)
    /// </summary>
    public float CameraArmOffsetZ
    {
        get { return _CameraArmOffsetZ; }
        set
        {
            float tempCameraArmOffsetZ = _CameraArmOffsetZ;
            _CameraArmOffsetZ = value;
            if (tempCameraArmOffsetZ != _CameraArmOffsetZ)
                Call<IGameState, float>(temp => temp.CameraArmOffsetZ);
        }
    }

    /// <summary>
    /// 镜头与目标在Z轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    private float _CameraPosOffsetZ;
    /// <summary>
    /// 镜头与目标在Z轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    public float CameraPosOffsetZ
    {
        get { return _CameraPosOffsetZ; }
        set
        {
            float tempCameraPosOffsetZ = _CameraPosOffsetZ;
            _CameraPosOffsetZ = value;
            if (_CameraPosOffsetZ != tempCameraPosOffsetZ)
                Call<IGameState, float>(temp => temp.CameraPosOffsetZ);
        }
    }

    /// <summary>
    /// 镜头与目标在Y轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    private float _CameraPosOffsetY;
    /// <summary>
    /// 镜头与目标在Y轴方向上的偏差(如果当前模式是固定视角模式)
    /// </summary>
    public float CameraPosOffsetY
    {
        get { return _CameraPosOffsetY; }
        set
        {
            float tempCameraPosOffsetY = _CameraPosOffsetY;
            _CameraPosOffsetY = value;
            if (_CameraPosOffsetY != tempCameraPosOffsetY)
                Call<IGameState, float>(temp => temp.CameraPosOffsetY);
        }
    }

    /// <summary>
    /// 选择目标的模式
    /// </summary>
    private EnumSelectTargetModel _SelectTargetModel;
    /// <summary>
    /// 选择目标的模式
    /// </summary>
    public EnumSelectTargetModel SelectTargetModel
    {
        get { return _SelectTargetModel; }
        set
        {
            EnumSelectTargetModel tempSelectTargetModel = _SelectTargetModel;
            _SelectTargetModel = value;
            if (tempSelectTargetModel != _SelectTargetModel)
                Call<IGameState, EnumSelectTargetModel>(temp => temp.SelectTargetModel);
        }
    }

    /// <summary>
    /// 游戏的视角模式
    /// </summary>
    private EnumViewModel _ViewModel;
    /// <summary>
    /// 游戏的视角模式
    /// </summary>
    public EnumViewModel ViewModel
    {
        get { return _ViewModel; }
        set
        {
            EnumViewModel tempViewModel = _ViewModel;
            _ViewModel = value;
            if (tempViewModel != _ViewModel)
                Call<IGameState, EnumViewModel>(temp => temp.ViewModel);
        }
    }

    /// <summary>
    /// 设置面板的UI
    /// </summary>
    private Canvas _SettingPanel;
    /// <summary>
    /// 设置面板的UI
    /// </summary>
    public Canvas SettingPanel
    {
        get { return _SettingPanel; }
        set
        {
            Canvas tempSettingPanel = _SettingPanel;
            _SettingPanel = value;
            if (tempSettingPanel != _SettingPanel)
                Call<IGameState, Canvas>(temp => temp.SettingPanel);
        }
    }

    /// <summary>
    /// 功能面板的UI
    /// </summary>
    private Canvas _ActionPanel;
    /// <summary>
    /// 功能面板的UI 
    /// </summary>
    public Canvas ActionPanel
    {
        get { return _ActionPanel; }
        set
        {
            Canvas tempActionPanel = _ActionPanel;
            _ActionPanel = value;
            if (_ActionPanel != tempActionPanel)
                Call<IGameState, Canvas>(temp => temp.ActionPanel);
        }
    }

    /// <summary>
    /// 主面板的UI
    /// </summary>
    private Canvas _MainPanel;
    /// <summary>
    /// 主面板的UI
    /// </summary>
    public Canvas MainPanel
    {
        get { return _MainPanel; }
        set
        {
            Canvas tempMainPanel = _MainPanel;
            _MainPanel = value;
            if (_MainPanel != tempMainPanel)
                Call<IGameState, Canvas>(temp => temp.MainPanel);
        }
    }

    private Canvas _InterludesPanel;
    /// <summary>
    /// 过场动画幕布的UI
    /// </summary>
    public Canvas InterludesPanel
    {
        get { return _InterludesPanel; }
        set
        {
            Canvas tempInterludesPanel = _InterludesPanel;
            _InterludesPanel = value;
            if (_InterludesPanel != tempInterludesPanel)
                Call<IGameState, Canvas>(temp => temp.InterludesPanel);
        }
    }

    /// <summary>
    /// 过场动画用到的摄像机
    /// </summary>
    private Camera _InterludesCamera;
    /// <summary>
    /// 过场动画用到的摄像机
    /// </summary>
    public Camera InterludesCamera
    {
        get { return _InterludesCamera; }
        set
        {
            Camera tempInterludesCamera = _InterludesCamera;
            _InterludesCamera = value;
            if (_InterludesCamera != tempInterludesCamera)
                Call<IGameState, Camera>(temp => temp.InterludesCamera);
        }
    }
    #endregion

}
