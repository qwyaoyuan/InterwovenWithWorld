using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 基础的交互脚本,实现了文字显示
/// </summary>
public abstract class InteractiveBaseMono : MonoBehaviour, IObjInteractive
{
    /// <summary>
    /// 伤害字体相对对象的偏差值
    /// </summary>
    [SerializeField]
    private float _HurtFontOffset;

    /// <summary>
    /// 游戏状态对象
    /// </summary>
    protected IGameState iGameState;

    /// <summary>
    /// Awake 
    /// </summary>
    protected virtual void Awake()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
    }

    /// <summary>
    /// 伤害字体相对对象的偏差值
    /// </summary>
    public float HurtFontOffset
    {
        get
        {
            return _HurtFontOffset;
        }
    }

    public T GetEntity<T>() where T : IBaseState
    {
        return default(T);
    }

    public abstract CalculateHurt.Result GiveAttackHurtStruct(AttackHurtStruct attackHurtStruct);

    /// <summary>
    /// 显示伤害
    /// </summary>
    /// <param name="hurtResult"></param>
    /// <param name="targetObj"></param>
    public void ShowHurt(CalculateHurt.Result hurtResult,GameObject targetObj )
    {
        iGameState.ShowHurtFont = new HurtFontStruct()
        {
            Hurt = hurtResult.hurt,
            IsCrit = hurtResult.IsCrit,
            Offset = HurtFontOffset,
            TargetObj = targetObj
        };
    }
}

