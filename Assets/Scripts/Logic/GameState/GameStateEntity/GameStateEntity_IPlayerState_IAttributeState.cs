using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IPlayerAttributeState接口的GameState类的一个分支实体
/// 下标0表示自身
/// </summary>
public partial class GameState
{
    /// <summary>
    /// 保存临时属性的字典
    /// </summary>
    private Dictionary<int, IAttributeState> iAttributeStateDic;
    /// <summary>
    /// 属性操作句柄的当前下标
    /// </summary>
    int iAttributeHandleIndex;

    /// <summary>
    /// 用于初始化角色属性的对象 
    /// </summary>
    partial void Start_IPlayerState_IAttribute()
    {
        iAttributeStateDic = new Dictionary<int, IAttributeState>();
        iAttributeHandleIndex = 0;
        //构建自身的基础属性(下标是0)
        CreateAttributeHandle(0);
        //构建技能的属性(注意技能的属性从负数开始)
        //有些技能只存在特殊效果,而且这些特殊效果不涉及这些属性,则这些特殊效果在具体的位置处理
        //被动技能  注:光环技能不需要初始化,因为光环是动态的
        CreateAttributeHandle(-(int)EnumSkillType.FS05);
        CreateAttributeHandle(-(int)EnumSkillType.FS06);
        CreateAttributeHandle(-(int)EnumSkillType.FS07);
        CreateAttributeHandle(-(int)EnumSkillType.FS08);
        CreateAttributeHandle(-(int)EnumSkillType.FS10);
        CreateAttributeHandle(-(int)EnumSkillType.YSX05);
        CreateAttributeHandle(-(int)EnumSkillType.YSX08);
        CreateAttributeHandle(-(int)EnumSkillType.MFS03);
        CreateAttributeHandle(-(int)EnumSkillType.MFS07);
        CreateAttributeHandle(-(int)EnumSkillType.SM01);
        CreateAttributeHandle(-(int)EnumSkillType.SM05);
        CreateAttributeHandle(-(int)EnumSkillType.MS07);
        CreateAttributeHandle(-(int)EnumSkillType.DFS01);
        CreateAttributeHandle(-(int)EnumSkillType.DFS02);
        CreateAttributeHandle(-(int)EnumSkillType.DSM01);
        CreateAttributeHandle(-(int)EnumSkillType.DSM09);
        CreateAttributeHandle(-(int)EnumSkillType.JS01);
        CreateAttributeHandle(-(int)EnumSkillType.JS02);
        CreateAttributeHandle(-(int)EnumSkillType.JH01);
        CreateAttributeHandle(-(int)EnumSkillType.JH02);
        CreateAttributeHandle(-(int)EnumSkillType.JH03);
    }

    #region 用于操纵附加状态的功能
    /// <summary>
    /// 创建一个状态句柄,返回句柄的id
    /// </summary>
    /// <returns>如果返回的句柄小于定于0则表示失败了(小于等于0的句柄被用于内部)</returns>
    public int CreateAttributeHandle()
    {
        int testCount = 0;//当前尝试次数
        ReTest:
        iAttributeHandleIndex++;
        testCount++;//每次执行尝试次数加1
        if (CreateAttributeHandle(iAttributeHandleIndex))
        {
            return iAttributeHandleIndex;
        }
        else if (testCount < 100)//如果尝试次数小于100则可以继续尝试
        {
            if (iAttributeHandleIndex == int.MaxValue)
            {
                iAttributeHandleIndex = 0;
                goto ReTest;
            }
        }
        return 0;
    }

    /// <summary>
    /// 使用指定的句柄id创建一个状态
    /// 如果存在这个句柄了,则不会创建(这个一般不要用)
    /// </summary>
    /// <param name="index">句柄ID</param>
    /// <returns>返回是否创建成功</returns>
    public bool CreateAttributeHandle(int index)
    {
        if (iAttributeStateDic == null)
            iAttributeStateDic = new Dictionary<int, IAttributeState>();
        if (iAttributeStateDic.ContainsKey(index))
            return false;
        IAttributeState iAttributeState = new AttributeStateAdditional();
        iAttributeStateDic.Add(index, iAttributeState);
        //添加回调
        iAttributeState.Registor<IAttributeState>((target, fieldName) =>
        {
            Call<IPlayerAttributeState>(fieldName);
        });
        return true;
    }

    /// <summary>
    /// 通过句柄获取对应的状态对象
    /// </summary>
    /// <param name="handle">状态句柄</param>
    /// <returns></returns>
    public IAttributeState GetAttribute(int handle)
    {
        if (iAttributeStateDic != null && iAttributeStateDic.ContainsKey(handle))
            return iAttributeStateDic[handle];
        return null;
    }

    /// <summary>
    /// 获取合计的属性
    /// </summary>
    /// <returns></returns>
    public IAttributeState GetResultAttribute()
    {
        AttributeStateAdditional result = new AttributeStateAdditional();
        //使用反射进行数据整合
        Type t = typeof(IAttributeState);
        PropertyInfo[] infos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo info in infos)
        {
            MethodInfo setInfo = info.GetSetMethod();
            MethodInfo getInfo = info.GetGetMethod();
            object value = getInfo.Invoke(this, null);
            setInfo.Invoke(result, new object[] { value });
        }
        return result;
    }

    /// <summary>
    /// 移除一个状态,注意只能移除大于零的句柄
    /// </summary>
    /// <param name="handle"></param>
    public void RemoveAttribute(int handle)
    {
        if (handle > 0 && iAttributeStateDic != null && iAttributeStateDic.ContainsKey(handle))
        {
            iAttributeStateDic.Remove(handle);
            Call<IPlayerAttributeState, Action<int>>(temp => temp.RemoveAttribute);//通知属性变动
        }
    }

    #endregion

    /// <summary>
    /// 没有任何作用
    /// </summary>
    public void Init()
    { }

    #region IAttributeState 属性状态
    #region 基础属性
    /// <summary>
    /// 敏捷
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Quick
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Quick).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Quick = value;
            }
        }
    }

    /// <summary>
    /// 专注
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Dedicated
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Dedicated).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Dedicated = value;
            }
        }
    }

    /// <summary>
    /// 精神
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Mental
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Mental).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Mental = value;
            }
        }
    }

    /// <summary>
    /// 力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Power
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Power).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Power = value;
            }
        }
    }
    #endregion
    #region 常规属性
    /// <summary>
    /// 血量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float HP
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.HP).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.HP = value;
            }
        }
    }

    /// <summary>
    /// 最大血量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxHP
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxHP).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxHP = value;
            }
        }
    }

    /// <summary>
    /// 魔力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float Mana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.Mana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.Mana = value;
            }
        }
    }

    /// <summary>
    /// 最大魔力量
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxMana).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxMana = value;
            }
        }
    }

    /// <summary>
    /// 最大耗魔上限
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MaxUseMana
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MaxUseMana).Sum();
        }

        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MaxUseMana = value;
            }
        }
    }

    /// <summary>
    /// 视野范围
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float View
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.View).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.View = value;
            }
        }
    }

    /// <summary>
    /// 移动速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MoveSpeed
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MoveSpeed).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MoveSpeed = value;
            }
        }
    }

    /// <summary>
    /// 攻击速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float AttackSpeed
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.AttackSpeed).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.AttackSpeed = value;
            }
        }
    }

    /// <summary>
    /// 命中率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float HitRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.HitRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.HitRate = value;
            }
        }
    }

    /// <summary>
    /// 闪避率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float EvadeRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EvadeRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EvadeRate = value;
            }
        }
    }

    /// <summary>
    /// 暴击率
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float CritRate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.CritRate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.CritRate = value;
            }
        }
    }
    #endregion
    #region 回复
    /// <summary>
    /// 生命恢复速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float LifeRecovery
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LifeRecovery).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LifeRecovery = value;
            }
        }
    }

    /// <summary>
    /// 法力恢复速度
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ManaRecovery
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ManaRecovery).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ManaRecovery = value;
            }
        }
    }
    #endregion
    #region 攻击与防御属性
    /// <summary>
    /// 道具攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ItemAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ItemAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ItemAttacking = value;
            }
        }
    }

    /// <summary>
    /// 魔法攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicAttacking = value;
            }
        }
    }

    /// <summary>
    /// 物理攻击力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsAttacking
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsAttacking).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsAttacking = value;
            }
        }
    }

    /// <summary>
    /// 魔法附加伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicAdditionalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicAdditionalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicAdditionalDamage = value;
            }
        }
    }

    /// <summary>
    /// 物理伤害附加
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsAdditionalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsAdditionalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsAdditionalDamage = value;
            }
        }
    }

    /// <summary>
    /// 魔法攻击穿透
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicPenetrate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicPenetrate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicPenetrate = value;
            }
        }
    }

    /// <summary>
    /// 物理攻击穿透
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsPenetrate
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsPenetrate).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsPenetrate = value;
            }
        }
    }

    /// <summary>
    /// 魔法最终伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicFinalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicFinalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicFinalDamage = value;
            }
        }
    }

    /// <summary>
    /// 物理最终伤害
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsFinalDamage
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsFinalDamage).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsFinalDamage = value;
            }
        }
    }

    /// <summary>
    /// 元素亲和
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float ElementAffine
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.ElementAffine).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ElementAffine = value;
            }
        }
    }

    /// <summary>
    /// 魔法亲和
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicAffine
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicAffine).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicAffine = value;
            }
        }
    }

    /// <summary>
    /// 魔法抗性（魔法防御）
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float MagicResistance
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.MagicResistance).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.MagicResistance = value;
            }
        }
    }

    /// <summary>
    /// 物理抗性（物理防御）
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float PhysicsResistance
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.PhysicsResistance).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.PhysicsResistance = value;
            }
        }
    }

    /// <summary>
    /// 元素抗性
    /// </summary>
    float[] _ElementResistances;
    public float[] ElementResistances
    {
        get
        {
            if (iAttributeStateDic != null)
            {
                var tempDatas = iAttributeStateDic.Values.Select(temp => temp.ElementResistances).Where(temp => temp.Length != 0);
                var lengthData = tempDatas.Select(temp => temp.Length).Distinct();
                if (lengthData.Count() == 1 && lengthData.First() > 0)
                {
                    int length = lengthData.First();
                    float[] result = new float[length];
                    foreach (var _tempData in tempDatas)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            result[i] += _tempData[i];
                        }
                    }
                }
            }
            return new float[0];
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.ElementResistances = value;
            }
        }
    }

    /// <summary>
    /// 状态抗性
    /// </summary>
    float[] _StateResistances;
    /// <summary>
    /// 状态抗性
    /// </summary>
    public float[] StateResistances
    {
        get
        {
            if (iAttributeStateDic != null)
            {
                var tempDatas = iAttributeStateDic.Values.Select(temp => temp.StateResistances).Where(temp => temp.Length != 0);
                var lengthData = tempDatas.Select(temp => temp.Length).Distinct();
                if (lengthData.Count() == 1 && lengthData.First() > 0)
                {
                    int length = lengthData.First();
                    float[] result = new float[length];
                    foreach (var _tempData in tempDatas)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            result[i] += _tempData[i];
                        }
                    }
                }
            }
            return new float[0];
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.StateResistances = value;
            }
        }
    }

    /// <summary>
    /// 特效影响力
    /// 注意:获取的是整合后的属性,而设置的是自身的属性 
    /// </summary>
    public float EffectResideTime
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.EffectResideTime).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.EffectResideTime = value;
            }
        }
    }

    /// <summary>
    /// 光明信仰强度
    /// </summary>
    public float LightFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LightFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LightFaith = value;
            }
        }
    }

    /// <summary>
    /// 黑信仰强度
    /// </summary>
    public float DarkFaith
    {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.DarkFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.DarkFaith = value;
            }
        }
    }

    /// <summary>
    /// 生物信仰强度
    /// </summary>
    public float LifeFaith {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.LifeFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.LifeFaith = value;
            }
        }
    }

    /// <summary>
    /// 自然信仰强度
    /// </summary>
    public float NaturalFaith {
        get
        {
            if (iAttributeStateDic == null)
                return 0;
            return iAttributeStateDic.Values.Select(temp => temp.NaturalFaith).Sum();
        }
        set
        {
            IAttributeState iAttributeBaseState = GetAttribute(0);
            if (iAttributeBaseState != null)
            {
                iAttributeBaseState.NaturalFaith = value;
            }
        }
    }
    #endregion
    #endregion

}