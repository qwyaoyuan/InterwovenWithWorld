using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 技能类型枚举
/// </summary>
public enum EnumSkillType
{
    /// <summary>
    /// 没有技能
    /// </summary>
    None = 0,

    #region 特殊技能
    SpecialReleaseStart = 1,
    /// <summary>
    /// 大魔法
    /// </summary>
    [FieldExplan("大魔法")]
    DFS09,
    /// <summary>
    /// 信仰赋予
    /// </summary>
    [FieldExplan("信仰赋予")]
    ZHS04,
    /// <summary>
    /// 魔力扰乱
    /// </summary>
    [FieldExplan("魔力扰乱")]
    ZHS05,
    /// <summary>
    /// 魔偶操控
    /// </summary>
    [FieldExplan("魔偶操控")]
    MFS02,
    /// <summary>
    /// 精神集中
    /// </summary>
    [FieldExplan("精神集中")]
    DSM05,
    /// <summary>
    /// 体能突破
    /// </summary>
    [FieldExplan("体能突破")]
    DSM06,
    /// <summary>
    /// 传送
    /// </summary>
    [FieldExplan("传送")]
    DSM08,
    /// <summary>
    /// 后方支援
    /// </summary>
    [FieldExplan("后方支援")]
    MS05,
    /// <summary>
    /// 神秘信仰_降临
    /// </summary>
    [FieldExplan("神秘信仰_降临")]
    JS09,
    /// <summary>
    /// 神依
    /// </summary>
    [FieldExplan("神依")]
    JH09,
    /// <summary>
    /// 释放魔法
    /// </summary>
    MagicRelease = 199,
    SpecialReleaseEnd = 200,
    #endregion

    #region 被动技能
    PassiveStart = 300,
    /// <summary>
    /// 入门法师
    /// </summary>
    [FieldExplan("入门法师")]
    FS05,

    /// <summary>
    /// 法术力场
    /// </summary>
    [FieldExplan("法术力场")]
    FS06,

    /// <summary>
    /// 进阶法师
    /// </summary>
    [FieldExplan("进阶法师")]
    FS07,

    /// <summary>
    /// 法力之源
    /// </summary>
    [FieldExplan("法力之源")]
    FS08,

    /// <summary>
    /// 快速咏唱
    /// </summary>
    [FieldExplan("快速咏唱")]
    FS09,

    /// <summary>
    /// 高阶法师
    /// </summary>
    [FieldExplan("高阶法师")]
    FS10,

    /// <summary>
    /// 元素精通
    /// </summary>
    [FieldExplan("元素精通")]
    YSX05,

    /// <summary>
    /// 元素低语
    /// </summary>
    [FieldExplan("元素低语")]
    YSX08,

    /// <summary>
    /// 魔偶制成
    /// </summary>
    [FieldExplan("魔偶制成")]
    MFS01,

    /// <summary>
    /// 魔法精通
    /// </summary>
    [FieldExplan("魔法精通")]
    MFS03,

    /// <summary>
    /// 精灵交谈
    /// </summary>
    [FieldExplan("精灵交谈")]
    MFS07,

    /// <summary>
    /// 元素信仰基础
    /// </summary>
    [FieldExplan("元素信仰基础")]
    SM01,

    /// <summary>
    /// 初阶萨满
    /// </summary>
    [FieldExplan("初阶萨满")]
    SM05,

    /// <summary>
    /// 自然聆听
    /// </summary>
    [FieldExplan("自然聆听")]
    SM08,

    /// <summary>
    /// 神灵信奉
    /// </summary>
    [FieldExplan("神灵信奉")]
    MS01,

    /// <summary>
    /// 信徒
    /// </summary>
    [FieldExplan("信徒")]
    MS07,

    /// <summary>
    /// 大法师
    /// </summary>
    [FieldExplan("大法师")]
    DFS01,

    /// <summary>
    /// 法术精通
    /// </summary>
    [FieldExplan("法术精通")]
    DFS02,

    /// <summary>
    /// 附加元素
    /// </summary>
    [FieldExplan("附加元素")]
    DFS03,

    /// <summary>
    /// 元素力场
    /// </summary>
    [FieldExplan("元素力场")]
    DFS07,

    /// <summary>
    /// 高速咏唱
    /// </summary>
    [FieldExplan("高速咏唱")]
    DFS08,

    /// <summary>
    /// 高阶法术陷阱
    /// </summary>
    [FieldExplan("高阶法术陷阱")]
    ZHS01,

    /// <summary>
    /// 高阶魔偶
    /// </summary>
    [FieldExplan("高阶魔偶")]
    ZHS03,

    /// <summary>
    /// 魔偶指令
    /// </summary>
    [FieldExplan("魔偶指令")]
    ZHS06,

    /// <summary>
    /// 操控者
    /// </summary>
    [FieldExplan("操控者")]
    ZHS07,

    /// <summary>
    /// 神圣元素
    /// </summary>
    [FieldExplan("神圣元素")]
    DSM01,

    /// <summary>
    /// 萨满之矛
    /// </summary>
    [FieldExplan("萨满之矛")]
    DSM07,

    /// <summary>
    /// 萨满精神
    /// </summary>
    [FieldExplan("萨满精神")]
    DSM09,

    /// <summary>
    /// 光明祭司
    /// </summary>
    [FieldExplan("光明祭司")]
    JS01,

    /// <summary>
    /// 黑暗祭司
    /// </summary>
    [FieldExplan("黑暗祭司")]
    JS02,

    /// <summary>
    /// 神秘信仰
    /// </summary>
    [FieldExplan("神秘信仰")]
    JS06,

    /// <summary>
    /// 高阶生物信仰
    /// </summary>
    [FieldExplan("高阶生物信仰")]
    JH01,

    /// <summary>
    /// 高阶自然信仰
    /// </summary>
    [FieldExplan("高阶自然信仰")]
    JH02,

    /// <summary>
    /// 神灵笃信
    /// </summary>
    [FieldExplan("神灵笃信")]
    JH03,

    /// <summary>
    /// 神灵信仰
    /// </summary>
    [FieldExplan("神灵信仰")]
    JH06,
    /// <summary>
    /// 圣物
    /// </summary>
    [FieldExplan("圣物")]
    JH07,
    PassiveEnd = 500,
    #endregion

    #region 光环技能
    SpecialCircleStart = 600,
    /// <summary>
    /// 祝福光环
    /// </summary>
    [FieldExplan("祝福光环")]
    MS04,
    /// <summary>
    /// 敬畏光环
    /// </summary>
    [FieldExplan("敬畏光环")]
    JH05,
    /// <summary>
    /// 神佑光环
    /// </summary>
    [FieldExplan("神佑光环")]
    JS05,
    SpecialCircleEnd = 800,
    #endregion

    #region 魔法技能
    MagicStart = 1000,
    #region 需要结合的技能(1阶段)
    MagicCombinedLevel1Start = 1000,
    /// <summary>
    /// 奥术弹
    /// </summary>
    [FieldExplan("奥术弹")]
    FS01,
    /// <summary>
    /// 魔力屏障
    /// </summary>
    [FieldExplan("魔力屏障")]
    FS03,
    /// <summary>
    /// 奥术震荡
    /// </summary>
    [FieldExplan("奥术震荡")]
    FS02,
    /// <summary>
    /// 魔力导向
    /// </summary>
    [FieldExplan("魔力导向")]
    FS04,
    /// <summary>
    /// 魔力脉冲
    /// </summary>
    [FieldExplan("魔力脉冲")]
    MFS05,
    MagicCombinedLevel1End = 1100,
    #endregion
    #region 需要结合的技能(2阶段)
    MagicCombinedLevel2Start = 1100,
    /// <summary>
    /// 火元素
    /// </summary>
    [FieldExplan("火元素")]
    YSX01,
    /// <summary>
    /// 水元素
    /// </summary>
    [FieldExplan("水元素")]
    YSX02,
    /// <summary>
    /// 土元素
    /// </summary>
    [FieldExplan("土元素")]
    YSX03,
    /// <summary>
    /// 风元素
    /// </summary>
    [FieldExplan("风元素")]
    YSX04,
    /// <summary>
    /// 冰元素
    /// </summary>
    [FieldExplan("冰元素")]
    SM06,
    /// <summary>
    /// 雷元素
    /// </summary>
    [FieldExplan("雷元素")]
    SM07,
    /// <summary>
    /// 光明元素
    /// </summary>
    [FieldExplan("光明元素")]
    DSM03,
    /// <summary>
    /// 黑暗元素
    /// </summary>
    [FieldExplan("黑暗元素")]
    DSM04,
    /// <summary>
    /// 光明信仰基础
    /// </summary>
    [FieldExplan("光明信仰基础")]
    XYX01,
    /// <summary>
    /// 黑暗信仰基础
    /// </summary>
    [FieldExplan("黑暗信仰基础")]
    XYX02,
    /// <summary>
    /// 生物信仰基础
    /// </summary>
    [FieldExplan("生物信仰基础")]
    XYX03,
    /// <summary>
    /// 自然信仰基础
    /// </summary>
    [FieldExplan("自然信仰基础")]
    XYX04,
    MagicCombinedLevel2End = 1200,
    #endregion
    #region 需要结合的技能(3阶段)
    MagicCombinedLevel3Start = 1200,
    /// <summary>
    /// 连续魔力导向
    /// </summary>
    [FieldExplan("连续魔力导向")]
    MFS06,
    /// <summary>
    /// 双重法术
    /// </summary>
    [FieldExplan("双重法术")]
    MFS08,
    /// <summary>
    /// 法术陷阱
    /// </summary>
    [FieldExplan("法术陷阱")]
    MFS04,
    /// <summary>
    /// 元素精炼
    /// </summary>
    [FieldExplan("元素精炼")]
    YSX07,
    /// <summary>
    /// 元素驻留
    /// </summary>
    [FieldExplan("元素驻留")]
    YSX06,
    /// <summary>
    /// 黑暗信仰_死体操控
    /// </summary>
    [FieldExplan("黑暗信仰_死体操控")]
    ZHS02,
    /// <summary>
    /// 魔力虹吸
    /// </summary>
    [FieldExplan("魔力虹吸")]
    SM03,
    /// <summary>
    /// 生命虹吸
    /// </summary>
    [FieldExplan("生命虹吸")]
    SM02,
    /// <summary>
    /// 生物信仰_召唤
    /// </summary>
    [FieldExplan("生物信仰_召唤")]
    SM04,
    /// <summary>
    /// 光明信仰_净化
    /// </summary>
    [FieldExplan("光明信仰_净化")]
    XYX05,
    /// <summary>
    /// 黑暗信仰_凋零
    /// </summary>
    [FieldExplan("黑暗信仰_凋零")]
    XYX08,
    /// <summary>
    /// 生物信仰_活力
    /// </summary>
    [FieldExplan("生物信仰_活力")]
    XYX06,
    /// <summary>
    /// 自然信仰_自然之力
    /// </summary>
    [FieldExplan("自然信仰_自然之力")]
    XYX07,
    /// <summary>
    /// 神秘信仰_特化
    /// </summary>
    [FieldExplan("神秘信仰_特化")]
    JS07,
    /// <summary>
    /// 光明信仰_圣光
    /// </summary>
    [FieldExplan("光明信仰_圣光")]
    JS03,
    /// <summary>
    /// 黑暗信仰_魔笛
    /// </summary>
    [FieldExplan("黑暗信仰_魔笛")]
    JS04,
    /// <summary>
    /// 光明信仰_神迹
    /// </summary>
    [FieldExplan("光明信仰_神迹")]
    MS02,
    /// <summary>
    /// 黑暗信仰_瘟疫
    /// </summary>
    [FieldExplan("黑暗信仰_瘟疫")]
    MS03,
    /// <summary>
    /// 崇拜信仰_信仰之翼
    /// </summary>
    [FieldExplan("崇拜信仰_信仰之翼")]
    JH04,
    MagicCombinedLevel3End = 1300,
    #endregion
    #region 需要结合的技能(4阶段)
    MagicCombinedLevel4Start = 1300,
    /// <summary>
    /// 纯净元素
    /// </summary>
    [FieldExplan("纯净元素")]
    DFS05,
    /// <summary>
    /// 神速咏唱
    /// </summary>
    [FieldExplan("神速咏唱")]
    DFS04,
    /// <summary>
    /// 元素爆破
    /// </summary>
    [FieldExplan("元素爆破(魔力导向)")]
    DFS06,
    /// <summary>
    /// 精灵呼唤
    /// </summary>
    [FieldExplan("精灵呼唤(精灵交谈)")]
    ZHS09,
    /// <summary>
    /// 生物信仰_呼唤
    /// </summary>
    [FieldExplan("生物信仰_呼唤")]
    ZHS08,
    /// <summary>
    /// 信仰冲击
    /// </summary>
    [FieldExplan("信仰冲击")]
    MS08,
    /// <summary>
    /// 医者自医
    /// </summary>
    [FieldExplan("医者自医")]
    MS06,
    /// <summary>
    /// 信仰感召
    /// </summary>
    [FieldExplan("信仰感召")]
    JS08,
    /// <summary>
    /// 崇拜信仰_传承
    /// </summary>
    [FieldExplan("崇拜信仰_传承")]
    JH08,
    /// <summary>
    /// 虹吸增幅
    /// </summary>
    [FieldExplan("虹吸增幅")]
    DSM02,
    MagicCombinedLevel4End = 1400,
    #endregion

    /*组合技能的方法是：各个阶段减去它们的开始枚举数值，根据不同的阶段乘以100的n次方，然后相加，最终结果加1亿*/

    EndMagic = 200000000,
    #endregion



    /*物理技能*/
    PhysicsStart = 200000002,
}


/// <summary>
/// 技能分组
/// 主要用于判断前置条件
/// </summary>
public enum EnumSkillZone
{
    None,

    /// <summary>
    /// 法术组
    /// </summary>
    [FieldExplan("法术组")]
    FS_MF_Zone,
    /// <summary>
    /// 元素系
    /// </summary>
    [FieldExplan("元素系")]
    YS_MF_Zone,
    /// <summary>
    /// 信仰系
    /// </summary>
    [FieldExplan("信仰系")]
    XY_MF_Zone,
    /// <summary>
    /// 魔法师
    /// </summary>
    [FieldExplan("魔法师")]
    MFS_MF_Zone,
    /// <summary>
    /// 萨满
    /// </summary>
    [FieldExplan("萨满")]
    SM_MF_Zone,
    /// <summary>
    /// 牧师
    /// </summary>
    [FieldExplan("牧师")]
    MS_MF_Zone,
    /// <summary>
    /// 大法师 
    /// </summary>
    [FieldExplan("大法师")]
    DFS_MF_Zone,
    /// <summary>
    /// 召唤师
    /// </summary>
    [FieldExplan("召唤师")]
    ZHS_MF_Zone,
    /// <summary>
    /// 大萨满
    /// </summary>
    [FieldExplan("大萨满")]
    DSM_MF_Zone,
    /// <summary>
    /// 祭司
    /// </summary>
    [FieldExplan("祭司")]
    JS_MF_Zone,
    /// <summary>
    /// 教皇
    /// </summary>
    [FieldExplan("教皇")]
    JH_MF_Zone,

    /// <summary>
    /// 战士
    /// </summary>
    [FieldExplan("战士")]
    ZS_WL_Zone,
    /// <summary>
    /// 弓箭手
    /// </summary>
    [FieldExplan("弓箭手")]
    GJS_WL_Zone,
    /// <summary>
    /// 狂战士
    /// </summary>
    [FieldExplan("狂战士")]
    KZS_WL_Zone,
    /// <summary>
    /// 剑士
    /// </summary>
    [FieldExplan("剑士")]
    JS_WL_Zone,
    /// <summary>
    /// 游侠
    /// </summary>
    [FieldExplan("游侠")]
    YX_WL_Zone,
    /// <summary>
    /// 盗贼 
    /// </summary>
    [FieldExplan("盗贼")]
    DZ_WL_Zone,
    /// <summary>
    /// 神射手
    /// </summary>
    [FieldExplan("神射手")]
    SSS_WL_Zone,
}

/// <summary>
/// 技能融合工具类
/// </summary>
public static class SkillCombineStaticTools
{
    /// <summary>
    /// 获取技能的组合值
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public static int GetCombineSkillKey(IEnumerable<EnumSkillType> skills)
    {
        throw new Exception("未实现");
    }

    /// <summary>
    /// 获取组合值组合的技能
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static EnumSkillType[] GetCombineSkills(int key)
    {
        throw new Exception("未实现");
    }

    /// <summary>
    /// 获取是否可以组合技能
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public static bool GetCanCombineSkills(params EnumSkillType[] skills)
    {
        throw new Exception("未实现");
    }

    /// <summary>
    /// 通过组合技能的阶段来获取所有该阶段的技能
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static EnumSkillType[] GetBaseSkillBackCombineSkillIndex(int index)
    {
        throw new Exception("未实现");
    }

    /// <summary>
    /// 通过组合技能数组来获取该组合的技能名
    /// </summary>
    /// <param name="skillBaseStructs"></param>
    /// <returns></returns>
    public static string GetCombineSkillsName(IEnumerable<SkillBaseStruct> skillBaseStructs)
    {
        throw new Exception("未实现");
    }
}