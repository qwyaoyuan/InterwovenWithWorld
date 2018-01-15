using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动管理器
/// 负责角色的移动
/// </summary>
public class MoveManager : IInput
{
    /// <summary>
    /// 移动管理器私有静态对象
    /// </summary>
    private static MoveManager instance;
    /// <summary>
    /// 移动管理器的单例对象
    /// </summary>
    public static MoveManager Instance
    {
        get
        {
            if (instance == null) instance = new MoveManager();
            return instance;
        }
    }
    /// <summary>
    /// 移动管理器的私有构造函数
    /// </summary>
    private MoveManager()
    {
        specialStateList_CantMove = new List<string>();
        buffOrDebuffStateList_Move = new Dictionary<string, float>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
        iAnimatorState = GameState.Instance.GetEntity<IAnimatorState>();
        GameState.Instance.Registor<ISpecialState>(CallbackSpecialState);
        GameState.Instance.Registor<IBuffState>(CallbackBuffState);
        GameState.Instance.Registor<IDebuffState>(CallbackDebuffState);
    }

    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 玩家的属性
    /// </summary>
    IPlayerAttributeState iPlayerAttributeState;

    /// <summary>
    /// 动画状态
    /// </summary>
    IAnimatorState iAnimatorState;

    /// <summary>
    /// 特殊状态集合(此处并不是全部的特殊状态,只有会引起无法移动的特殊状态才会被存放到这里)
    /// </summary>
    List<string> specialStateList_CantMove;

    /// <summary>
    /// buff和debuff状态字典(此处并不是全部的buff和debuff,只有会引起移动速度变化的状态才会被存放到这里)
    /// </summary>
    Dictionary<string, float> buffOrDebuffStateList_Move;

    /// <summary>
    /// 监听special状态变化回调
    /// </summary>
    /// <param name="iSpecialState"></param>
    /// <param name="specialName"></param>
    private void CallbackSpecialState(ISpecialState iSpecialState, string specialName)
    {
        Action<BuffState, string> CheckGameState = (tempBuffState, buffStateName) =>
         {
             if (tempBuffState.Time > 0)//如果存在异常则添加到集合中
             {
                 if (!specialStateList_CantMove.Contains(buffStateName))//如果不存在再添加,否则会重复添加
                     specialStateList_CantMove.Add(buffStateName);
             }
             else//如果异常时间到了则从集合中移除
             {
                 if (specialStateList_CantMove.Contains(buffStateName))//如果存在再移除
                     specialStateList_CantMove.Remove(buffStateName);
             }
         };
        if (string.Equals(specialName, GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Jiangzhi)))//如果是僵直
            CheckGameState(iSpecialState.Jiangzhi, specialName);
        if (string.Equals(specialName, GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Xuanyun)))//如果是眩晕
            CheckGameState(iSpecialState.Xuanyun, specialName);
        if (string.Equals(specialName, GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Jingu)))//如果是禁锢
            CheckGameState(iSpecialState.Jingu, specialName);
        if (string.Equals(specialName, GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Mabi)))//如果是麻痹
            CheckGameState(iSpecialState.Mabi, specialName);

    }

    /// <summary>
    /// 将变化后的buff设置到或从buffOrDebuffStateList_Move字典中移除
    /// </summary>
    /// <param name="tempBuffState"></param>
    /// <param name="buffStateName"></param>
    [Obsolete("因为速度已经整合到状态中,不需要在这里计算了")]
    private void SetStateListMove(BuffState tempBuffState, string buffStateName)
    {
        // 重新计算移动速度加成值(衰减值)
        Action ReCalculateMoveSpeed = () => { };

        if (tempBuffState.Time > 0)//如果存在buff则添加到集合中
        {
        }
        else//如果异常时间到了则从集合中移除
        {
            if (buffOrDebuffStateList_Move.ContainsKey(buffStateName))//如果存在再移除
            {
                buffOrDebuffStateList_Move.Remove(buffStateName);
                ReCalculateMoveSpeed();
            }
        }
    }

    /// <summary>
    /// 监听buff状态变化回调
    /// </summary>
    /// <param name="iBuffState"></param>
    /// <param name="buffName"></param>
    private void CallbackBuffState(IBuffState iBuffState, string buffName)
    {
        if (string.Equals(iBuffState, GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Jiasu)))//如果是加速
            SetStateListMove(iBuffState.Jiasu, buffName);
        if (string.Equals(iBuffState, GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Minjie)))//如果是敏捷
            SetStateListMove(iBuffState.Minjie, buffName);
    }

    /// <summary>
    /// 监听debuff状态变化回调
    /// </summary>
    /// <param name="iDebuffState"></param>
    /// <param name="debuffName"></param>
    private void CallbackDebuffState(IDebuffState iDebuffState, string debuffName)
    {
        if (string.Equals(iDebuffState, GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Bingdong)))//如果是冰冻
            SetStateListMove(iDebuffState.Bingdong, debuffName);
        if (string.Equals(iDebuffState, GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Chidun)))//如果是迟钝
            SetStateListMove(iDebuffState.Chidun, debuffName);
        if (string.Equals(iDebuffState, GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Jiansu)))//如果是减速
            SetStateListMove(iDebuffState.Jiansu, debuffName);
    }

    /// <summary>
    /// 检测是否可以移动
    /// </summary>
    /// <returns></returns>
    private bool CheckCanMoveState()
    {
        if (!iPlayerState.PlayerObj)
            return false;
        if (iGameState.GameRunType != EnumGameRunType.Safe && iGameState.GameRunType != EnumGameRunType.Unsafa)//如果不是在安全区或者非安全区(对话剧情等状态)时,不可以移动
            return false;
        if (specialStateList_CantMove.Count > 0)//如果存在特殊状态无法移动,则不可以移动
            return false;
        return true;
    }

    public void KeyPress(int key) { }

    public void KeyUp(int key) { }

    public void KeyDown(int key) { }

    /// <summary>
    /// 移动角色(摄像机在角色的子物体内)
    /// </summary>
    /// <param name="forward"></param>
    public void Move(Vector2 forward)
    {
        if (CheckCanMoveState()
            && (!iAnimatorState.IsMagicActionState || (iAnimatorState.IsMagicActionState && iAnimatorState.MagicAnimatorType == EnumMagicAnimatorType.Sing))
            && !iAnimatorState.IsPhycisActionState
            && !iAnimatorState.IsSkillState)
        {
            float moveSpeed = iPlayerAttributeState.MoveSpeed;//移动速度(向前移动  向后移动0.7  左右移动0.3) 
            if (iAnimatorState.IsMagicActionState && iAnimatorState.MagicAnimatorType == EnumMagicAnimatorType.Sing)
                moveSpeed *= 0.3f;
            float strength = Mathf.Pow(forward.sqrMagnitude, 0.5f);//速度的力度
            float animForwardAngle = 0;//0前方向 1,-1后方向 0.5右方向 -0.5左方向
            if (Mathf.Abs(forward.x) < 0.1f)//没有左右方向的移动
                if (forward.y > 0)//前方向
                {
                    moveSpeed *= strength;
                    animForwardAngle = 0;
                }
                else//后方向
                {
                    moveSpeed *= -strength * 0.5f;
                    animForwardAngle = 1;
                }
            else
            {
                float angle = Vector2.Angle(new Vector2(0, 1), forward);
                if (forward.y >= 0)//前方向
                {
                    moveSpeed = Mathf.Lerp(moveSpeed, moveSpeed * 0.3f, angle / 90);
                    if (forward.x > 0)
                        animForwardAngle = 0.5f * angle / 90;
                    else
                        animForwardAngle = -0.5f * angle / 90;
                }
                else//后方向
                {
                    moveSpeed = Mathf.Lerp(moveSpeed * 0.7f, moveSpeed * 0.3f, (angle - 90) / 90);
                    if (forward.x > 0)
                        animForwardAngle = 0.5f + 0.5f * (angle - 90) / 90;
                    else
                        animForwardAngle = -0.5f - 0.5f * (angle - 90) / 90;
                }
                moveSpeed *= strength * (Mathf.Abs(forward.x) / forward.x);
            }
            moveSpeed = Mathf.Abs(moveSpeed);//取速度的绝对值
            animForwardAngle *= 180;
            //设置动画的状态
            iAnimatorState.AnimatorMoveSpeed = Mathf.Pow(Vector2.SqrMagnitude(forward), 0.5f);//  moveSpeed / iPlayerAttributeState.MoveSpeed;
            iAnimatorState.MoveAnimatorVectorType = (int)animForwardAngle;
            CharacterController characterController = iPlayerState.PlayerObj.GetComponent<CharacterController>();
            //处理对象移动
            if (moveSpeed > 0)
            {
                forward *= moveSpeed * Time.deltaTime;
                Vector3 self_forward = iPlayerState.PlayerObj.transform.TransformDirection(new Vector3(forward.x, 0, forward.y));
                self_forward.y = 0;
                characterController.Move(self_forward);
            }
            characterController.Move(Vector3.down * Time.deltaTime * 10);//持续添加重力
        }
    }

    /// <summary>
    /// 旋转摄像机以及旋转操纵角色(摄像机在角色的子物体内)
    /// </summary>
    /// <param name="view"></param>
    public void View(Vector2 view)
    {
        if (CheckCanMoveState())
        {
            view.x = -view.x;//左右方向调整
            view.y = -view.y;//上下方向调整

            //左右旋转的话旋转整体
            float leftRight = view.x;//左右移动 正表示右方向 负表示左方向
            iPlayerState.PlayerObj.transform.Rotate(Vector3.up, -leftRight * iGameState.CameraRotateSpeed.x * Time.deltaTime, Space.World);
            //上下旋转的话旋转摄像机
            if (iPlayerState.PlayerCamera)
            {
                float upDown = -view.y;//上下移动 (+)向上移动角度变小 (-)向下移动角度变大
                float mustRotateAngle = upDown * iGameState.CameraRotateSpeed.y * Time.deltaTime;
                //计算当前的夹角
                float nowAngle = Vector3.Angle(iPlayerState.PlayerObj.transform.up, iPlayerState.PlayerCamera.transform.position - iPlayerState.PlayerObj.transform.position);
                float mustAngle = nowAngle + mustRotateAngle;//需要旋转到的角度
                float endAngle = Mathf.Clamp(mustAngle, iGameState.CameraYAngleRange.x, iGameState.CameraYAngleRange.y);//将角度限定
                float rotateAngle = nowAngle - endAngle;//需要旋转的角度
                iPlayerState.PlayerCamera.transform.RotateAround(iPlayerState.PlayerObj.transform.position, iPlayerState.PlayerObj.transform.right, rotateAngle);
            }
        }
    }


}
