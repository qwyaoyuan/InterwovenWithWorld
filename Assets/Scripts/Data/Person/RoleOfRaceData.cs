using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 种族的所有数据
/// </summary>
public class RoleOfRaceData : ILoadable<RoleOfRaceData>
{
    /// <summary>
    /// 保存种族数据的路径
    /// </summary>
    public static string dataFilePath = "Data/RoleOfRaceData/RoleOfRaceData";

    /// <summary>
    /// 每个种族的数据数组
    /// </summary>
    private RoleOfRaceInfoStruct[] roleOfRaceInfoStructArray;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(RoleOfRaceData.dataFilePath);//加载信息 
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        roleOfRaceInfoStructArray = DeSerializeNow<RoleOfRaceInfoStruct[]>(assetText);
        if (roleOfRaceInfoStructArray == null)
        {
            roleOfRaceInfoStructArray = new RoleOfRaceInfoStruct[0];
        }
    }

    /// <summary>
    /// 获取指定种族的属性
    /// </summary>
    /// <param name="roleOfRace"></param>
    /// <returns></returns>
    public RoleOfRaceInfoStruct this[RoleOfRace roleOfRace]
    {
        get
        {
            if (roleOfRaceInfoStructArray == null)
                return null;
            RoleOfRaceInfoStruct roleOfRaceInfoStruct = roleOfRaceInfoStructArray.FirstOrDefault(temp => temp.roleOfRace == roleOfRace);
            return roleOfRaceInfoStruct;
        }
#if UNITY_EDITOR
        set
        {
            if (roleOfRaceInfoStructArray == null)
                roleOfRaceInfoStructArray = new RoleOfRaceInfoStruct[0];
            List<RoleOfRaceInfoStruct> roleOfRaceInfoStructList = new List<RoleOfRaceInfoStruct>(roleOfRaceInfoStructArray);
            RoleOfRaceInfoStruct roleOfRaceInfoStruct = roleOfRaceInfoStructArray.FirstOrDefault(temp => temp.roleOfRace == roleOfRace);
            if (roleOfRaceInfoStruct == null)
            {
                roleOfRaceInfoStructList.Add(value);
            }
            else
            {
                roleOfRaceInfoStructList.Remove(roleOfRaceInfoStruct);
                roleOfRaceInfoStructList.Add(value);
            }
            roleOfRaceInfoStructArray = roleOfRaceInfoStructList.ToArray();
        }
#endif
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    public T DeSerializeNow<T>(string value) where T : class
    {
        T target = JsonConvert.DeserializeObject<T>(value);
        return target;
    }
}

/// <summary>
/// 每个种族的数据
/// </summary>
public class RoleOfRaceInfoStruct
{
    /// <summary>
    /// 种族类型
    /// </summary>
    public RoleOfRace roleOfRace;

    #region 基础
#if UNITY_EDITOR
    [JsonIgnore] [FieldExplan("-----------------基础--------------------")] public string Split1;
#endif
    /// <summary>
    /// 基础力量
    /// </summary>
    [FieldExplan("基础力量")] public int basePower;
    /// <summary>
    /// 基础精神
    /// </summary>
    [FieldExplan("基础精神")] public int baseMental;
    /// <summary>
    /// 基础敏捷
    /// </summary>
    [FieldExplan("基础敏捷")] public int baseQuick;
    /// <summary>
    /// 基础生命上限
    /// </summary>
    [FieldExplan("基础生命上限")] public int baseHP;
    /// <summary>
    /// 基础魔法上限
    /// </summary>
    [FieldExplan("基础魔法上限")] public int baseMana;
    /// <summary>
    /// 基础生命回复速度
    /// </summary>
    [FieldExplan("基础生命回复速度")] public float baseHPRecovery;
    /// <summary>
    /// 基础魔法回复速度
    /// </summary>
    [FieldExplan("基础魔法回复速度")] public float baseManaRecovery;
    /// <summary>
    /// 基础闪避
    /// </summary>
    [FieldExplan("基础闪避")] public float baseEvadeRate;
    /// <summary>
    /// 基础命中
    /// </summary>
    [FieldExplan("基础命中")] public float baseHitRate;
    /// <summary>
    /// 基础暴击
    /// </summary>
    [FieldExplan("基础暴击")] public float baseCritRate;
    /// <summary>
    /// 基础攻击速度
    /// </summary>
    [FieldExplan("基础攻击速度")] public float baseAttackSpeed;
    /// <summary>
    /// 基础最大耗魔上限
    /// </summary>
    [FieldExplan("基础最大耗魔上限")] public float baseMaxUseMana;
    /// <summary>
    /// 基础移动速度
    /// </summary>
    [FieldExplan("基础移动速度")] public float baseMoveSpeed;
    /// <summary>
    /// 基础物理伤害
    /// </summary>
    [FieldExplan("基础物理伤害")] public int basePhysicDamage;
    /// <summary>
    /// 基础物理护甲
    /// </summary>
    [FieldExplan("基础物理护甲")] public int basePhysicDefense;
    /// <summary>
    /// 基础被发现几率
    /// </summary>
    [FieldExplan("基础被发现几率")] public float baseSight;
    /// <summary>
    /// 基础冷却时间减少率
    /// </summary>
    [FieldExplan("基础冷却时间减少率")] public float baseCoolingTime;
    /// <summary>
    /// 基础暴击伤害倍率(1.5)
    /// </summary>
    [FieldExplan("基础暴击伤害倍率(1.5)")] public float baseCritHurt;
    /// <summary>
    /// 基础的暴击伤害减少率
    /// </summary>
    [FieldExplan("基础的暴击伤害减少率")] public float baseCritHurtDef;
    /// <summary>
    /// 基础光明信仰强度
    /// </summary>
    [FieldExplan("基础光明信仰强度")] public int baseLightStrength;
    /// <summary>
    /// 基础黑暗信仰强度
    /// </summary>
    [FieldExplan("基础黑暗信仰强度")] public int baseDarkStrength;
    /// <summary>
    /// 基础生物信仰强度
    /// </summary>
    [FieldExplan("基础生物信仰强度")] public int baseLifeStrength;
    /// <summary>
    /// 基础自然信仰强度
    /// </summary>
    [FieldExplan("基础自然信仰强度")] public int baseNaturalStrength;
    /// <summary>
    /// 基础魔法亲和性
    /// </summary>
    [FieldExplan("基础魔法亲和性")] public int baseMagicFit;
    /// <summary>
    /// 异常状态抗性(特殊状态抗性)
    /// </summary>
    [FieldExplan("异常状态抗性(特殊状态抗性)")] public int baseAbnormalStateResistance;
    #endregion

    #region 成长
#if UNITY_EDITOR
    [JsonIgnore] [FieldExplan("-----------------成长--------------------")] public string Split2;
#endif
    /// <summary>
    /// 力量成长值
    /// </summary>
    [FieldExplan("力量成长值")] public float additionPower;
    /// <summary>
    /// 精神成长值
    /// </summary>
    [FieldExplan("精神成长值")] public float additionMental;
    /// <summary>
    /// 敏捷成长值
    /// </summary>
    [FieldExplan("敏捷成长值")] public float additionQuick;
    /// <summary>
    /// 生命上限成长值
    /// </summary>
    [FieldExplan("生命上限成长值")] public float additionHP;
    /// <summary>
    /// 魔法上限成长值
    /// </summary>
    [FieldExplan("魔法上限成长值")] public float additionMana;
    /// <summary>
    /// 基础物理伤害成长值
    /// </summary>
    [FieldExplan("基础物理伤害成长值")] public float additionPhysicDamage;
    /// <summary>
    /// 基础物理防御成长值
    /// </summary>
    [FieldExplan("基础物理防御成长值")] public float additionPhysicDefense;
    /// <summary>
    /// 每次升级增加的属性点数
    /// </summary>
    [FieldExplan("每次升级增加的属性点数")] public int additionAttributePoint;
    /// <summary>
    /// 每次升级增加的技能点数
    /// </summary>
    [FieldExplan("每次升级增加的技能点数")] public int additionSkillPoint;
    /// <summary>
    /// 每点敏捷对闪避率的提升率
    /// </summary>
    [FieldExplan("每点敏捷对闪避率的提升率")] public float additionQuickToEvadeRate;
    /// <summary>
    /// 每点敏捷对命中率的提升 
    /// </summary>
    [FieldExplan("每点敏捷对命中率的提升")] public float additionQuickToHitRate;
    /// <summary>
    /// 每点敏捷对暴击率的提升
    /// </summary>
    [FieldExplan("每点敏捷对暴击率的提升")] public float additionQuickToCritRate;
    /// <summary>
    /// 每点敏捷对攻击速度的提升
    /// </summary>
    [FieldExplan("每点敏捷对攻击速度的提升")] public float additionQuickToAttackSpeed;
    /// <summary>
    /// 每点敏捷对移动速度的提升
    /// </summary>
    [FieldExplan("每点敏捷对移动速度的提升")] public float additionQuickToMoveSpeed;
    /// <summary>
    /// 每点精神对法力上限的提升
    /// </summary>
    [FieldExplan("每点精神对法力上限的提升")] public float additionMentalToMana;
    /// <summary>
    /// 每点精神对魔法攻击力的提升
    /// </summary>
    [FieldExplan("每点精神对魔法攻击力的提升")] public float additionMentalToMagicAttacking;
    /// <summary>
    /// 每点精神对最大耗魔上限的提升
    /// </summary>
    [FieldExplan("每点精神对最大耗魔上限的提升")] public float additionMentalToMaxUseMana;
    /// <summary>
    /// 每点精神对魔法防御力的提升
    /// </summary>
    [FieldExplan("每点精神对魔法防御力的提升")] public float additionMentalToMagicResistance;
    /// <summary>
    /// 每点精神对魔法回复速度的提升
    /// </summary>
    [FieldExplan("每点精神对魔法回复速度的提升")] public float additionMentalToManaRecovery;
    /// <summary>
    /// 每点力量对血量上限的提升
    /// </summary>
    [FieldExplan("每点力量对血量上限的提升")] public float additionPowerToHP;
    /// <summary>
    /// 每点力量对物理攻击力的提升
    /// </summary>
    [FieldExplan("每点力量对物理攻击力的提升")] public float additionPowerToPhysicAttacking;
    /// <summary>
    /// 每点力量对异常状态抗性(特殊状态抗性)的提升
    /// </summary>
    [FieldExplan("每点力量对异常状态抗性")] public float additionPowerToAbnormalStateResistance;
    /// <summary>
    /// 每点力量对物理防御力的提升
    /// </summary>
    [FieldExplan("每点力量对物理防御力的提升")] public float additionPowerToPhysicsResistance;
    /// <summary>
    /// 每点力量对生命回复速度的提升
    /// </summary>
    [FieldExplan("每点力量对生命回复速度的提升")] public float additionPowerToHPRecovery;
    #endregion

    #region 公式参数   
#if UNITY_EDITOR
    [JsonIgnore] [FieldExplan("-----------------公式参数--------------------")] public string Split3;
#endif
    /// <summary>
    /// 物理攻击力转换为伤害倍率的系数
    /// </summary>
    [FieldExplan("物理攻击力转换为伤害倍率的系数")] public float physicAttackToDamageRateRatio;
    /// <summary>
    /// 敏捷转换为最低伤害值的系数
    /// </summary>
    [FieldExplan("敏捷转换为最低伤害值的系数")] public float physicQuickToMinDamageRatio;
    /// <summary>
    /// 物理防御力转换为减伤倍率系数
    /// </summary>
    [FieldExplan("物理防御力转换为减伤倍率系数")] public float physicDefenseToHurtRateRatio;
    /// <summary>
    /// 敏捷转换为豁免伤害值的系数
    /// </summary>
    [FieldExplan("敏捷转换为豁免伤害值的系数")] public float physicQuickToHurtExemptRatio;
    /// <summary>
    /// 魔法攻击力转化为伤害倍率的系数
    /// </summary>
    [FieldExplan("魔法攻击力转化为伤害倍率的系数")] public float magicAttackToDamageRateRatio;
    /// <summary>
    /// 魔法防御力转换为减伤倍率系数
    /// </summary>
    [FieldExplan("物理防御力转换为减伤倍率系数")] public float magicDefenseToHurtRateRatio;
    #endregion

}


