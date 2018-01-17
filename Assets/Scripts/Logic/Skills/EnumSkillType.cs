using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 技能类型枚举
/// </summary>
[Serializable]
public enum EnumSkillType
{
    /// <summary>
    /// 没有技能
    /// </summary>
    [FieldExplan("空")]
    None = 0,

    #region 特殊技能
    #region 魔法
    /// <summary>
    /// 特殊的直接释放的魔法技能序列的开始
    /// </summary>
    SpecialMagicReleaseStart = 1,
    /// <summary>
    /// 大魔法
    /// </summary>
    [FieldExplan("大魔法")]
    DFS09 = 2,
    /// <summary>
    /// 信仰赋予
    /// </summary>
    [FieldExplan("信仰赋予")]
    ZHS04 = 3,
    /// <summary>
    /// 魔力扰乱
    /// </summary>
    [FieldExplan("魔力扰乱")]
    ZHS05 = 4,
    /// <summary>
    /// 魔偶操控
    /// </summary>
    [FieldExplan("魔偶操控")]
    MFS02 = 5,
    /// <summary>
    /// 精神集中
    /// </summary>
    [FieldExplan("精神集中")]
    DSM05 = 6,
    /// <summary>
    /// 体能突破
    /// </summary>
    [FieldExplan("体能突破")]
    DSM06 = 7,
    /// <summary>
    /// 传送
    /// </summary>
    [FieldExplan("传送")]
    DSM08 = 8,
    /// <summary>
    /// 后方支援
    /// </summary>
    [FieldExplan("后方支援")]
    MS05 = 9,
    /// <summary>
    /// 神秘信仰_降临
    /// </summary>
    [FieldExplan("神秘信仰_降临")]
    JS09 = 10,
    /// <summary>
    /// 神依
    /// </summary>
    [FieldExplan("神依")]
    JH09 = 11,
    /// <summary>
    /// 魔法释放
    /// </summary>
    [FieldExplan("魔法释放")]
    MagicRelease = 199,
    /// <summary>
    /// 特殊的直接释放的魔法技能序列的结束
    /// </summary>
    SpecialMagicReleaseEnd = 200,
    #endregion
    #region 物理
    /// <summary>
    /// 物理普通攻击
    /// </summary>
    [FieldExplan("普通攻击")]
    PhysicAttack = 201,
    /// <summary>
    /// 特殊的直接释放的物理技能序列的开始
    /// </summary>
    SpecialPhysicReleaseStart = 202,
    /// <summary>
    /// 魔剑士
    /// </summary>
    [FieldExplan("魔剑士")]
    ZS02 = 203,
    /// <summary>
    /// 魔矢
    /// </summary>
    [FieldExplan("魔矢")]
    GJS02 = 204,
    /// <summary>
    /// 风行
    /// </summary>
    [FieldExplan("风行")]
    YX02 = 205,
    /// <summary>
    /// 暗杀术
    /// </summary>
    [FieldExplan("暗杀术")]
    DZ03 = 206,
    /// <summary>
    /// 特殊的直接释放物理技能(动作)开始
    /// </summary>
    SpecialPhysicActionReleaseStart = 250,
    /// <summary>
    /// 重击
    /// </summary>
    [FieldExplan("重击")]
    WL01 = 251,
    /// <summary>
    /// 冲锋
    /// </summary>
    [FieldExplan("冲锋")]
    ZS03 = 252,
    /// <summary>
    /// 散射
    /// </summary>
    [FieldExplan("散射")]
    GJS03 = 253,
    /// <summary>
    /// 战吼
    /// </summary>
    [FieldExplan("战吼")]
    KZS03 = 254,
    /// <summary>
    /// 燕返
    /// </summary>
    [FieldExplan("燕返")]
    JAS03 = 255,
    /// <summary>
    /// 狙击术
    /// </summary>
    [FieldExplan("狙击术")]
    SSS03 = 256,
    /// <summary>
    /// 特殊的直接释放的物理技能序列的结束
    /// </summary>
    SpecialPhysicReleaseEnd = 299,
    #endregion
    #endregion

    #region 被动技能
    PassiveStart = 300,
    /// <summary>
    /// 入门法师
    /// </summary>
    [FieldExplan("入门法师")]
    FS05 = 301,

    /// <summary>
    /// 法术力场
    /// </summary>
    [FieldExplan("法术力场")]
    FS06 = 302,

    /// <summary>
    /// 进阶法师
    /// </summary>
    [FieldExplan("进阶法师")]
    FS07 = 303,

    /// <summary>
    /// 法力之源
    /// </summary>
    [FieldExplan("法力之源")]
    FS08 = 304,

    /// <summary>
    /// 快速咏唱
    /// </summary>
    [FieldExplan("快速咏唱")]
    FS09 = 305,

    /// <summary>
    /// 高阶法师
    /// </summary>
    [FieldExplan("高阶法师")]
    FS10 = 306,

    /// <summary>
    /// 元素精通
    /// </summary>
    [FieldExplan("元素精通")]
    YSX05 = 307,

    /// <summary>
    /// 元素低语
    /// </summary>
    [FieldExplan("元素低语")]
    YSX08 = 308,

    /// <summary>
    /// 魔偶制成
    /// </summary>
    [FieldExplan("魔偶制成")]
    MFS01 = 309,

    /// <summary>
    /// 魔法精通
    /// </summary>
    [FieldExplan("魔法精通")]
    MFS03 = 310,

    /// <summary>
    /// 精灵交谈
    /// </summary>
    [FieldExplan("精灵交谈")]
    MFS07 = 311,

    /// <summary>
    /// 元素信仰基础
    /// </summary>
    [FieldExplan("元素信仰基础")]
    SM01 = 312,

    /// <summary>
    /// 初阶萨满
    /// </summary>
    [FieldExplan("初阶萨满")]
    SM05 = 313,

    /// <summary>
    /// 自然聆听
    /// </summary>
    [FieldExplan("自然聆听")]
    SM08 = 314,

    /// <summary>
    /// 神灵信奉
    /// </summary>
    [FieldExplan("神灵信奉")]
    MS01 = 315,

    /// <summary>
    /// 信徒
    /// </summary>
    [FieldExplan("信徒")]
    MS07 = 316,

    /// <summary>
    /// 大法师
    /// </summary>
    [FieldExplan("大法师")]
    DFS01 = 317,

    /// <summary>
    /// 法术精通
    /// </summary>
    [FieldExplan("法术精通")]
    DFS02 = 318,

    /// <summary>
    /// 附加元素
    /// </summary>
    [FieldExplan("附加元素")]
    DFS03 = 319,

    /// <summary>
    /// 元素力场
    /// </summary>
    [FieldExplan("元素力场")]
    DFS07 = 320,

    /// <summary>
    /// 高速咏唱
    /// </summary>
    [FieldExplan("高速咏唱")]
    DFS08 = 321,

    /// <summary>
    /// 高阶法术陷阱
    /// </summary>
    [FieldExplan("高阶法术陷阱")]
    ZHS01 = 322,

    /// <summary>
    /// 高阶魔偶
    /// </summary>
    [FieldExplan("高阶魔偶")]
    ZHS03 = 323,

    /// <summary>
    /// 魔偶指令
    /// </summary>
    [FieldExplan("魔偶指令")]
    ZHS06 = 324,

    /// <summary>
    /// 操控者
    /// </summary>
    [FieldExplan("操控者")]
    ZHS07 = 325,

    /// <summary>
    /// 神圣元素
    /// </summary>
    [FieldExplan("神圣元素")]
    DSM01 = 326,

    /// <summary>
    /// 萨满之矛
    /// </summary>
    [FieldExplan("萨满之矛")]
    DSM07 = 327,

    /// <summary>
    /// 萨满精神
    /// </summary>
    [FieldExplan("萨满精神")]
    DSM09 = 328,

    /// <summary>
    /// 光明祭司
    /// </summary>
    [FieldExplan("光明祭司")]
    JS01 = 329,

    /// <summary>
    /// 黑暗祭司
    /// </summary>
    [FieldExplan("黑暗祭司")]
    JS02 = 330,

    /// <summary>
    /// 神秘信仰
    /// </summary>
    [FieldExplan("神秘信仰")]
    JS06 = 331,

    /// <summary>
    /// 高阶生物信仰
    /// </summary>
    [FieldExplan("高阶生物信仰")]
    JH01 = 332,

    /// <summary>
    /// 高阶自然信仰
    /// </summary>
    [FieldExplan("高阶自然信仰")]
    JH02 = 333,

    /// <summary>
    /// 神灵笃信
    /// </summary>
    [FieldExplan("神灵笃信")]
    JH03 = 334,

    /// <summary>
    /// 神灵信仰
    /// </summary>
    [FieldExplan("神灵信仰")]
    JH06 = 335,
    /// <summary>
    /// 圣物
    /// </summary>
    [FieldExplan("圣物")]
    JH07 = 336,


    //--------------------下面的是物理被动--------------------//
    /// <summary>
    /// 武器掌握
    /// </summary>
    [FieldExplan("武器掌握")]
    WL02 = 401,
    /// <summary>
    /// 快速反应
    /// </summary>
    [FieldExplan("快速反应")]
    WL03 = 402,
    /// <summary>
    /// 身体训练
    /// </summary>
    [FieldExplan("身体训练")]
    WL04=403,
    /// <summary>
    /// 白刃
    /// </summary>
    [FieldExplan("白刃")]
    ZS01=404,
    /// <summary>
    /// 近战强化
    /// </summary>
    [FieldExplan("近战强化")]
    ZS04,
    PassiveEnd = 500,
    /// <summary>
    /// 游击
    /// </summary>
    [FieldExplan("游击")]
    GJS01 = 501,
    /// <summary>
    /// 远程强化
    /// </summary>
    [FieldExplan("远程强化")]
    GJS04 = 502,
    /// <summary>
    /// 坚韧狂暴
    /// </summary>
    [FieldExplan("坚韧狂暴")]
    KZS01 = 503,
    /// <summary>
    /// 近战专精
    /// </summary>
    [FieldExplan("近战专精")]
    KZS02 = 504,
    /// <summary>
    /// 日益精进
    /// </summary>
    [FieldExplan("日益精进")]
    JAS01 = 505,
    /// <summary>
    /// 剑意
    /// </summary>
    [FieldExplan("剑意")]
    JAS02 = 506,
    /// <summary>
    /// 灵动敏锐
    /// </summary>
    [FieldExplan("灵动敏锐")]
    YX01 = 507,
    /// <summary>
    /// 后撤步
    /// </summary>
    [FieldExplan("后撤步")]
    YX03 = 508,
    /// <summary>
    /// 巧手夺宝
    /// </summary>
    [FieldExplan("巧手夺宝")]
    DZ01  =509,
    /// <summary>
    /// 巧手
    /// </summary>
    [FieldExplan("巧手")]
    DZ02 = 510,
    /// <summary>
    /// 远程专精
    /// </summary>
    [FieldExplan("远程专精")]
    SSS01 = 511,
    /// <summary>
    /// 鹰眼
    /// </summary>
    [FieldExplan("鹰眼")]
    SSS02 = 512,
    #endregion

    #region 光环技能
    SpecialCircleStart = 600,
    /// <summary>
    /// 祝福光环
    /// </summary>
    [FieldExplan("祝福光环")]
    MS04 = 601,
    /// <summary>
    /// 敬畏光环
    /// </summary>
    [FieldExplan("敬畏光环")]
    JH05 = 602,
    /// <summary>
    /// 神佑光环
    /// </summary>
    [FieldExplan("神佑光环")]
    JS05 = 603,
    SpecialCircleEnd = 800,
    #endregion

    #region 魔法组合技能
    MagicStart = 1000,
    #region 需要结合的技能(1阶段)
    MagicCombinedLevel1Start = 1000,
    /// <summary>
    /// 奥术弹
    /// </summary>
    [FieldExplan("奥术弹")]
    FS01 = 1001,
    /// <summary>
    /// 魔力屏障
    /// </summary>
    [FieldExplan("魔力屏障")]
    FS03 = 1002,
    /// <summary>
    /// 奥术震荡
    /// </summary>
    [FieldExplan("奥术震荡")]
    FS02 = 1003,
    /// <summary>
    /// 魔力导向
    /// </summary>
    [FieldExplan("魔力导向")]
    FS04 = 1004,
    /// <summary>
    /// 魔力脉冲
    /// </summary>
    [FieldExplan("魔力脉冲")]
    MFS05 = 1005,
    MagicCombinedLevel1End = 1100,
    #endregion
    #region 需要结合的技能(2阶段)
    MagicCombinedLevel2Start = 1100,
    /// <summary>
    /// 火元素
    /// </summary>
    [FieldExplan("火元素")]
    YSX01 = 1101,
    /// <summary>
    /// 水元素
    /// </summary>
    [FieldExplan("水元素")]
    YSX02 = 1102,
    /// <summary>
    /// 土元素
    /// </summary>
    [FieldExplan("土元素")]
    YSX03 = 1103,
    /// <summary>
    /// 风元素
    /// </summary>
    [FieldExplan("风元素")]
    YSX04 = 1104,
    /// <summary>
    /// 冰元素
    /// </summary>
    [FieldExplan("冰元素")]
    SM06 = 1105,
    /// <summary>
    /// 雷元素
    /// </summary>
    [FieldExplan("雷元素")]
    SM07 = 1106,
    /// <summary>
    /// 光明元素
    /// </summary>
    [FieldExplan("光明元素")]
    DSM03 = 1107,
    /// <summary>
    /// 黑暗元素
    /// </summary>
    [FieldExplan("黑暗元素")]
    DSM04 = 1108,
    /// <summary>
    /// 光明信仰基础
    /// </summary>
    [FieldExplan("光明信仰基础")]
    XYX01 = 1109,
    /// <summary>
    /// 光明信仰基础_对友军
    /// </summary>
    [FieldExplan("光明信仰基础_对友军")]
    XYX01_Self = 1110,
    /// <summary>
    /// 光明信仰基础_对敌军
    /// </summary>
    [FieldExplan("光明信仰基础_对敌军")]
    XYX01_Target = 1111,
    /// <summary>
    /// 黑暗信仰基础
    /// </summary>
    [FieldExplan("黑暗信仰基础")]
    XYX02 = 1112,
    /// <summary>
    /// 黑暗信仰基础_对友军
    /// </summary>
    [FieldExplan("黑暗信仰基础_对友军")]
    XYX02_Self = 1113,
    /// <summary>
    /// 黑暗信仰基础_对敌军
    /// </summary>
    [FieldExplan("黑暗信仰基础_对敌军")]
    XYX02_Target = 1114,
    /// <summary>
    /// 生物信仰基础
    /// </summary>
    [FieldExplan("生物信仰基础")]
    XYX03 = 1115,
    /// <summary>
    /// 生物信仰基础_对友军
    /// </summary>
    [FieldExplan("生物信仰基础_对友军")]
    XYX03_Self = 1116,
    /// <summary>
    /// 生物信仰基础_对敌军
    /// </summary>
    [FieldExplan("生物信仰基础_对敌军")]
    XYX03_Target = 1117,
    /// <summary>
    /// 生物信仰基础_无指向
    /// </summary>
    [FieldExplan("生物信仰基础_无指向")]
    XYX03_None = 1118,
    /// <summary>
    /// 自然信仰基础
    /// </summary>
    [FieldExplan("自然信仰基础")]
    XYX04 = 1119,
    /// <summary>
    /// 自然信仰基础_对敌军
    /// </summary>
    [FieldExplan("自然信仰基础_对敌军")]
    XYX04_Target = 1120,
    MagicCombinedLevel2End = 1200,
    #endregion
    #region 需要结合的技能(3阶段)
    MagicCombinedLevel3Start = 1200,
    /// <summary>
    /// 连续魔力导向
    /// </summary>
    [FieldExplan("连续魔力导向")]
    MFS06 = 1201,
    /// <summary>
    /// 双重法术
    /// </summary>
    [FieldExplan("双重法术")]
    MFS08 = 1202,
    /// <summary>
    /// 法术陷阱
    /// </summary>
    [FieldExplan("法术陷阱")]
    MFS04 = 1203,
    /// <summary>
    /// 元素精炼
    /// </summary>
    [FieldExplan("元素精炼")]
    YSX07 = 1204,
    /// <summary>
    /// 元素驻留
    /// </summary>
    [FieldExplan("元素驻留")]
    YSX06 = 1205,
    /// <summary>
    /// 黑暗信仰_死体操控
    /// </summary>
    [FieldExplan("黑暗信仰_死体操控")]
    ZHS02 = 1206,
    /// <summary>
    /// 魔力虹吸
    /// </summary>
    [FieldExplan("魔力虹吸")]
    SM03 = 1207,
    /// <summary>
    /// 生命虹吸
    /// </summary>
    [FieldExplan("生命虹吸")]
    SM02 = 1208,
    /// <summary>
    /// 生物信仰_召唤
    /// </summary>
    [FieldExplan("生物信仰_召唤")]
    SM04 = 1209,
    /// <summary>
    /// 光明信仰_净化
    /// </summary>
    [FieldExplan("光明信仰_净化")]
    XYX05 = 1210,
    /// <summary>
    /// 黑暗信仰_凋零
    /// </summary>
    [FieldExplan("黑暗信仰_凋零")]
    XYX08 = 1211,
    /// <summary>
    /// 生物信仰_活力
    /// </summary>
    [FieldExplan("生物信仰_活力")]
    XYX06 = 1212,
    /// <summary>
    /// 自然信仰_自然之力
    /// </summary>
    [FieldExplan("自然信仰_自然之力")]
    XYX07 = 1213,
    /// <summary>
    /// 神秘信仰_特化
    /// </summary>
    [FieldExplan("神秘信仰_特化")]
    JS07 = 1214,
    /// <summary>
    /// 光明信仰_圣光
    /// </summary>
    [FieldExplan("光明信仰_圣光")]
    JS03 = 1215,
    /// <summary>
    /// 黑暗信仰_魔笛
    /// </summary>
    [FieldExplan("黑暗信仰_魔笛")]
    JS04 = 1216,
    /// <summary>
    /// 光明信仰_神迹
    /// </summary>
    [FieldExplan("光明信仰_神迹")]
    MS02 = 1217,
    /// <summary>
    /// 黑暗信仰_瘟疫
    /// </summary>
    [FieldExplan("黑暗信仰_瘟疫")]
    MS03 = 1218,
    /// <summary>
    /// 崇拜信仰_信仰之翼
    /// </summary>
    [FieldExplan("崇拜信仰_信仰之翼")]
    JH04 = 1219,
    MagicCombinedLevel3End = 1300,
    #endregion
    #region 需要结合的技能(4阶段)
    MagicCombinedLevel4Start = 1300,
    /// <summary>
    /// 纯净元素
    /// </summary>
    [FieldExplan("纯净元素")]
    DFS05 = 1301,
    /// <summary>
    /// 神速咏唱
    /// </summary>
    [FieldExplan("神速咏唱")]
    DFS04 = 1302,
    /// <summary>
    /// 元素爆破
    /// </summary>
    [FieldExplan("元素爆破(魔力导向)")]
    DFS06 = 1303,
    /// <summary>
    /// 精灵呼唤
    /// </summary>
    [FieldExplan("精灵呼唤(精灵交谈)")]
    ZHS09 = 1304,
    /// <summary>
    /// 生物信仰_呼唤
    /// </summary>
    [FieldExplan("生物信仰_呼唤")]
    ZHS08 = 1305,
    /// <summary>
    /// 信仰冲击
    /// </summary>
    [FieldExplan("信仰冲击")]
    MS08 = 1306,
    /// <summary>
    /// 医者自医
    /// </summary>
    [FieldExplan("医者自医")]
    MS06 = 1307,
    /// <summary>
    /// 信仰感召
    /// </summary>
    [FieldExplan("信仰感召")]
    JS08 = 1308,
    /// <summary>
    /// 崇拜信仰_传承
    /// </summary>
    [FieldExplan("崇拜信仰_传承")]
    JH08 = 1309,
    /// <summary>
    /// 虹吸增幅
    /// </summary>
    [FieldExplan("虹吸增幅")]
    DSM02 = 1310,
    MagicCombinedLevel4End = 1400,
    #endregion

    /*组合技能的方法是：各个阶段减去它们的开始枚举数值，根据不同的阶段乘以100的n次方，然后相加，最终结果加1亿*/
    MagicCombinedStart = 100000000,

    EndMagic = 200000000,
    #endregion

}


/// <summary>
/// 技能分组
/// 主要用于判断前置条件
/// </summary>
public enum EnumSkillZone
{
    None,
    /// <summary>
    /// 魔法基础组(所有魔法技能都是改组的)
    /// </summary>
    [FieldExplan("魔法基础组(所有魔法技能都是改组的)")]
    MF_Zone,
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
    /// 元素+信仰
    /// </summary>
    [FieldExplan("元素+信仰")]
    YS_XY_MF_Zone,
    /// <summary>
    /// 魔法师+萨满
    /// </summary>
    [FieldExplan("魔法师+萨满")]
    MFS_SM_MF_Zone,
    /// <summary>
    /// 萨满+牧师
    /// </summary>
    [FieldExplan("萨满+牧师")]
    SM_MS_MF_Zone,


    /// <summary>
    /// 物理组
    /// </summary>
    [FieldExplan("物理组")]
    WL_Zone,
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
    /// 战士+弓箭手
    /// </summary>
    [FieldExplan("战士+弓箭手")]
    ZS_GJS_WL_Zone,
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

