using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在角色生成后初始化角色数据到GameState的状态中
/// </summary>
public class InitPlayerState : MonoBehaviour
{
    /// <summary>
    /// 玩家摄像机
    /// </summary>
    public Camera playerCamera;

    /// <summary>
    /// 伤害检测脚本
    /// </summary>
    public PhysicSkillInjuryDetection physicSkillInjuryDetection;

    void Awake()
    {
        GameState.Instance.PlayerObj = gameObject;
        GameState.Instance.PlayerCamera = playerCamera;
        GameState.Instance.MoveSpeed = 8;
        GameState.Instance.CameraRotateSpeed = new Vector2(50, 50);
        GameState.Instance.CameraYAngleRange = new Vector2(20, 160);
        GameState.Instance.CameraDistanceOfPlayer = 10;
        GameState.Instance.CameraArmOffsetZ = 1;
        GameState.Instance.CameraPosOffsetY = 7;
        GameState.Instance.CameraPosOffsetZ = 6;
        GameState.Instance.ViewModel = EnumViewModel.Solid;//固定摄像机模式
        GameState.Instance.GameRunType = EnumGameRunType.Safe;
        GameState.Instance.PhysicSkillInjuryDetection = physicSkillInjuryDetection;
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            GameState.Instance.ViewModel = EnumViewModel.Solid;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GameState.Instance.HP += 100;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameState.Instance.HP -= 100;
        }
    }

 
}
