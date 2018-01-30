using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

/// <summary>
/// 游戏状态的常量对象所在的类 (还包含一些对该常量操作的函数)
/// </summary>
public class GameStateConstValues
{
    #region 经验值与等级相关常量 
    /// <summary>
    /// 最大等级
    /// </summary>
    public const int MAXLEVEL = 50;

    #region 1-10
    /// <summary>
    /// 等级为1-10的时候经验参数A
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.A, 1, 10)]
    public const int LEVEL_1_10_EXPERIENCEPARAM_A = 100;
    /// <summary>
    /// 等级为1-10的时候经验参数B
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.B, 1, 10)]
    public const int LEVEL_1_10_EXPERIENCEPARAM_B = 200;

    #endregion

    #region 11-20
    /// <summary>
    /// 等级为11-20的时候经验参数A
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.A, 11, 20)]
    public const int LEVEL_11_20_EXPERIENCEPARAM_A = 150;
    /// <summary>
    /// 等级为11-20的时候经验参数B
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.B, 11, 20)]
    public const int LEVEL_11_20_EXPERIENCEPARAM_B = 600;

    #endregion

    #region 21-30
    /// <summary>
    /// 等级为21-30的时候经验参数A
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.A, 21, 30)]
    public const int LEVEL_21_30_EXPERIENCEPARAM_A = 300;
    /// <summary>
    /// 等级为21-30的时候经验参数B
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.B, 21, 30)]
    public const int LEVEL_21_30_EXPERIENCEPARAM_B = 1750;

    #endregion

    #region 31-40
    /// <summary>
    /// 等级为31-40的时候经验参数A
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.A, 31, 40)]
    public const int LEVEL_31_40_EXPERIENCEPARAM_A = 650;
    /// <summary>
    /// 等级为31-40的时候经验参数B
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.B, 31, 40)]
    public const int LEVEL_31_40_EXPERIENCEPARAM_B = 3250;

    #endregion

    #region 41-50
    /// <summary>
    /// 等级为41-50的时候经验参数A
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.A, 41, 40)]
    public const int LEVEL_41_50_EXPERIENCEPARAM_A = 1200;
    /// <summary>
    /// 等级为41-50的时候经验参数B
    /// </summary>
    [LevelExperienceParamAttribute(LevelExperienceParamAttribute.EnumLevelExperienceParamType.B, 41, 40)]
    public const int LEVEL_41_50_EXPERIENCEPARAM_B = 600;

    #endregion

    /// <summary>
    /// 获取该等级所需的升级经验
    /// </summary>
    /// <param name="level">等级</param>
    /// <returns></returns>
    public static int GetExperienceAtLevel(int level)
    {
        int result = int.MaxValue;
        if (level > 50)
            return int.MaxValue;
        Dictionary<FieldInfo, LevelExperienceParamAttribute> fieldDic = GetFieldAttributeInfo<LevelExperienceParamAttribute>();
        KeyValuePair<FieldInfo, LevelExperienceParamAttribute>[] fieldKVPs = fieldDic.Where(temp => temp.Value.MinLevel <= level && temp.Value.MaxLevel >= level).ToArray();
        if (fieldKVPs.Length == 2)//如果不等于请检查常量上的特性设置
        {
            FieldInfo fieldParamA = fieldKVPs.Where(temp => temp.Value.LevelExperienceParamType == LevelExperienceParamAttribute.EnumLevelExperienceParamType.A).Select(temp=>temp.Key).FirstOrDefault();
            FieldInfo fieldParamB = fieldKVPs.Where(temp => temp.Value.LevelExperienceParamType == LevelExperienceParamAttribute.EnumLevelExperienceParamType.B).Select(temp => temp.Key).FirstOrDefault();
            if (fieldParamA != null && fieldParamB != null)//如果有空的请检查常量上的特性设置
            {
                int paramA = (int)fieldParamA.GetValue(null);
                int paramB = (int)fieldParamB.GetValue(null);
                result = level * paramA + paramB;
            }
        }
        return result;
    }
    #endregion

    /// <summary>
    /// 获取该类的静态字段和对应特性的字典,该字段上有T类型的特性
    /// </summary>
    /// <typeparam name="T">特性类型</typeparam>
    /// <returns></returns>
    static Dictionary<FieldInfo, T> GetFieldAttributeInfo<T>() where T : Attribute
    {
        FieldInfo[] sourceFieldInfos = GetFieldInfo();
        Dictionary<FieldInfo, T> resultDic = new Dictionary<FieldInfo, T>();
        foreach (FieldInfo fieldInfo in sourceFieldInfos)
        {
            T att = fieldInfo.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();
            if (att == null)
                continue;
            if (resultDic.ContainsKey(fieldInfo))
                continue;
            resultDic.Add(fieldInfo, att);
        }
        return resultDic;
    }

    /// <summary>
    /// 获取该类的所有静态字段
    /// </summary>
    /// <returns></returns>
    static FieldInfo[] GetFieldInfo()
    {
        Type thisType = typeof(GameStateConstValues);
        FieldInfo[] fieldInfos = thisType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        return fieldInfos;
    }
}

/// <summary>
/// 等级经验相关参数的描述特性
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class LevelExperienceParamAttribute : Attribute
{
    /// <summary>
    /// 等级经验参数类型(A或B)
    /// </summary>
    public EnumLevelExperienceParamType LevelExperienceParamType { get; private set; }
    /// <summary>
    /// 区间的最小等级
    /// </summary>
    public int MinLevel { get; private set; }
    /// <summary>
    /// 区间的最大等级
    /// </summary>
    public int MaxLevel { get; private set; }

    /// <summary>
    /// 等级相关参数的描述特性
    /// 等级区间为闭区间(包含)
    /// </summary>
    /// <param name="levelExperienceParamType">参数类型A或B</param>
    /// <param name="minLevel">最小等级</param>
    /// <param name="maxLevel">最大等级</param>
    public LevelExperienceParamAttribute(EnumLevelExperienceParamType levelExperienceParamType, int minLevel, int maxLevel)
    {
        this.LevelExperienceParamType = levelExperienceParamType;
        this.MinLevel = minLevel;
        this.MaxLevel = maxLevel;
    }

    /// <summary>
    /// 等级经验参数类型
    /// </summary>
    public enum EnumLevelExperienceParamType
    {
        A,
        B
    }
}