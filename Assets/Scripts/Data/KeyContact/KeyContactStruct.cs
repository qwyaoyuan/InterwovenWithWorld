using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按键与技能或物品之间关系的结构
/// </summary>
public struct KeyContactStruct 
{
    /// <summary>
    /// 按下该键的类型
    /// 如果是功能按键则按功能键提取信息
    /// 如果是技能则按技能释放
    /// 如果是道具则按道具选择使用
    /// </summary>
    public EnumKeyContactType keyContactType;

    /// <summary>
    /// 对应的id
    /// 如果是技能则对应的是技能的枚举值
    /// 如果是功能,暂定 1:采集 2:交谈 3:功能
    /// </summary>
    public int id;

    /// <summary>
    /// 按键id
    /// </summary>
    public int key;

    /// <summary>
    /// 名字
    /// </summary>
    public string name;

    /// <summary>
    /// 对应的图片名(暂时只用于Action类型,技能道具请使用id检索)
    /// </summary>
    [JsonProperty]
    private string _SpriteName;
    /// <summary>
    /// 对应的图片(暂时只用于Action类型,技能道具请使用id检索)
    /// </summary>
    [JsonIgnore]
    private Sprite _Sprite;
    /// <summary>
    /// 对应的图片(暂时只用于Action类型,技能道具请使用id检索)
    /// </summary>
    [JsonIgnore]
    public Sprite Sprite
    {
        get
        {
            if (_Sprite == null)
            {
                _Sprite = SpriteManager.GetSrpite(_SpriteName);
            }
            return _Sprite;
        }
        set
        {
            _Sprite = value;
            _SpriteName = SpriteManager.GetName(value);
        }
    }

}
