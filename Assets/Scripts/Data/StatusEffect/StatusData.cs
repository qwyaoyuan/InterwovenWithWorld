using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// buff debuff 特殊状态的数据
/// </summary>
public class StatusData : ILoadable<StatusData>
{
    /// <summary>
    /// 保存状态数据信息的文件路径 
    /// </summary>
    public static string dataFilePath = "Data/Status/Status";

    /// <summary>
    /// 数据字典
    /// </summary>
    Dictionary<EnumStatusEffect, StatusDataInfo> dataDic;

    public void Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(StatusData.dataFilePath);
        string assetText = Encoding.UTF8.GetString(textAsset.bytes);
        dataDic = DeSerializeNow<Dictionary<EnumStatusEffect, StatusDataInfo>>(assetText);
    }

    /// <summary>
    /// 获取指定状态枚举的数组
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public StatusDataInfo this[EnumStatusEffect statusEffect]
    {
        get
        {
            if (dataDic != null && dataDic.ContainsKey(statusEffect))
                return dataDic[statusEffect];
            return null;
        }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    private T DeSerializeNow<T>(string value) where T : class
    {
        try
        {
            T target = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return target;
        }
        catch { }
        return null;
    }
}

/// <summary>
/// buff debuff 特殊状态的对应数据
/// </summary>
[Serializable]
public class StatusDataInfo
{
    /// <summary>
    /// 用于查找状态图标的唯一id
    /// </summary>
    [JsonProperty]
    private string statusSpriteID;
    [JsonIgnore]
    private Sprite statusSprite;
    /// <summary>
    /// 状态显示的图标(主要是在血条状态ui)
    /// </summary>
    [JsonIgnore]
    public Sprite StatusSprite { get { return statusSprite; } }

    [JsonProperty]
    private string statusExplane;
    /// <summary>
    /// 数据的说明
    /// </summary>
    public string StatusExplane { get { return statusExplane; } }

    /// <summary>
    /// 等级对应的数据字典
    /// </summary>
    [JsonProperty]
    private Dictionary<int, StatusLevelDataInfo> levelToDataDic;

    public StatusDataInfo()
    {
        levelToDataDic = new Dictionary<int, StatusLevelDataInfo>();
    }

    public void Load()
    {
        if (statusSprite == null)
            if (!string.IsNullOrEmpty(statusSpriteID))
                statusSprite = SpriteManager.GetSrpite(statusSpriteID);
    }

    /// <summary>
    /// 获取最大的状态等级
    /// </summary>
    [JsonIgnore]
    public int MaxLevel
    {
        get
        {
            if (levelToDataDic.Count > 0)
                return levelToDataDic.Max(temp => temp.Key);
            return 0;
        }
    }

    /// <summary>
    /// 获取当前等级的状态数据
    /// </summary>
    /// <param name="level">等级</param>
    /// <returns></returns>
    [JsonIgnore]
    public StatusLevelDataInfo this[int level]
    {
        get
        {
            level = Mathf.Clamp(level, 0, MaxLevel);
            if (levelToDataDic.ContainsKey(level))
                return levelToDataDic[level];
            return levelToDataDic.Select(temp => temp.Value).FirstOrDefault();
        }
    }

    [Serializable]
    public class StatusLevelDataInfo
    {
        /// <summary>
        /// 状态在当前等级的说明
        /// </summary>
        public string LevelExplane { get; set; }

        /// <summary>
        /// 状态对应的数据
        /// </summary>
        public Dictionary<EnumStatusAction, StatusActionDataInfo_Base> StatusActionDataInfoDic;

        public StatusLevelDataInfo()
        {
            StatusActionDataInfoDic = new Dictionary<EnumStatusAction, StatusActionDataInfo_Base>();
        }

    }
}


/// <summary>
/// 状态效果的基础数据结构
/// 如果有些是特殊状态且不需要数据则没有对应的类
/// </summary>
[Serializable] public class StatusActionDataInfo_Base
{ }
/// <summary>
/// 移动速度变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_MoveSpeed : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 攻击速度变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_AttackSpeed : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 抗性变化（主要对应的是各种Buff状态的抗性）
/// </summary>
[Serializable]
public class StatusActionDataInfo_StateResistance : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 攻击其他目标伤害变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_HitOtherHurt : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
    /// <summary>
    /// 目标
    /// </summary>
    [JsonIgnore] public int TargetID;
}
/// <summary>
/// 攻击指定目标伤害变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_HitTargetHurt : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
    /// <summary>
    /// 目标
    /// </summary>
    [JsonIgnore] public int TargetID;
}
/// <summary>
/// 生命值变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_Life : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(每秒)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 元素抗性变化（主要适用于防御计算）
/// </summary>
[Serializable]
public class StatusActionDataInfo_ElementResistance : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 元素亲和性变化(主要适用于攻击计算)
/// </summary>
[Serializable]
public class StatusActionDataInfo_ElementAffine : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法亲和性变化(计算魔法攻击)
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicAffine : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 生命恢复速度变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_LifeRecoverySpeed : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 法力恢复速度变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_ManaRecoverySpeed : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 命中率变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_HitRate : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 闪避率变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_EvadeRate : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 暴击率变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_CritRate : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 僵直
/// </summary>
[Serializable]
public class StatusActionDataInfo_Stiff : StatusActionDataInfo_Base
{
    /// <summary>
    /// 是否是僵直状态(被攻击后取消僵直效果),但是不影响其他效果
    /// </summary>
    public bool IsStiffState;
}
/// <summary>
/// 提高经验变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_ImproveExperience : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 视野变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_View : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(米)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 属性变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_AttributeChange : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法攻击造成吸血效果
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicSuckBlood : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(伤害百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 物理攻击造成吸血效果
/// </summary>
[Serializable]
public class StatusActionDataInfo_PhysicsSuckBlood : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(伤害百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法抗性变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicResisitance : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 物理抗性变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_PhysicsResisitance : StatusActionDataInfo_Base
{
    [FieldExplan("变化量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法攻击穿透(无视指定的魔法抗性)
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicPenetrate : StatusActionDataInfo_Base
{
    [FieldExplan("无视魔法抗性量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 物理攻击穿透（无视指定的物理抗性）
/// </summary>
[Serializable]
public class StatusActionDataInfo_PhysicsPenetrate : StatusActionDataInfo_Base
{
    [FieldExplan("无视物理抗性量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法伤害附加(最终结算时附加)
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicAdditionalDamage : StatusActionDataInfo_Base
{
    [FieldExplan("附加量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 物理伤害附加(最终结算时附加)
/// </summary>
[Serializable]
public class StatusActionDataInfo_PhysicsAdditionalDamage : StatusActionDataInfo_Base
{
    [FieldExplan("附加量")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 魔法最终伤害变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_MagicFinalDamage : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
/// <summary>
/// 物理最终伤害变化
/// </summary>
[Serializable]
public class StatusActionDataInfo_PhysicsFinalDamage : StatusActionDataInfo_Base
{
    [FieldExplan("变化量(百分比)")]
    [JsonProperty]
    public int Variation;
}
