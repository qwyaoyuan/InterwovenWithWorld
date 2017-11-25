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
    /// 当前的场景名 
    /// </summary>
    private string _SceneName;
    /// <summary>
    /// 当前的场景名
    /// </summary>
    public string SceneName
    {
        get { return _SceneName == null ? "" : _SceneName; }
        set
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
