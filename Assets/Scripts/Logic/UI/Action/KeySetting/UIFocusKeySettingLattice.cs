using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按键设置UI界面中的技能格子
/// </summary>
public class UIFocusKeySettingLattice : UIFocus
{

    /// <summary>
    /// 按键类型1
    /// 和2进行组合
    /// </summary>
    [SerializeField]
    private EnumInputType input1;
    /// <summary>
    /// 按键类型2
    /// 和1进行组合
    /// </summary>
    [SerializeField]
    private EnumInputType input2;

    /// <summary>
    /// 边框图片对象
    /// </summary>
    [SerializeField]
    private Image imageLine;

    /// <summary>
    /// 图标图片对象
    /// </summary>
    [SerializeField]
    private Image targetImage;

    /// <summary>
    /// 按键格子存放的类型
    /// </summary>
    public EnumKeyLatticeType keyLatticeType;

    /// <summary>
    /// 对应的id
    /// </summary>
    public int id;

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        imageLine.enabled = false;
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        imageLine.enabled = true;
    }

    /// <summary>
    /// 根据指定的类型和id设置图标
    /// </summary>
    public void InitShow()
    {
        //SetTargetImage()
    }

    /// <summary>
    /// 设置对象的图标
    /// </summary>
    /// <param name="sprite"></param>
    public void SetTargetImage(Sprite sprite)
    {
        if (sprite != null)
        {
            targetImage.sprite = sprite;
            targetImage.enabled = true;
        }
        else
        {
            targetImage.sprite = null;
            targetImage.enabled = false;
        }
    }

    /// <summary>
    /// 获取输入
    /// </summary>
    /// <returns></returns>
    public int GetKeySettingInput()
    {
        int int1 = (int)input1;
        int int2 = (int)input2;
        int end = int1 | int2;
        return end;
    }

    /// <summary>
    /// 格子存放的类型
    /// </summary>
    public enum EnumKeyLatticeType
    {
        /// <summary>
        /// 技能
        /// </summary>
        Skill,
        /// <summary>
        /// 道具
        /// </summary>
        Item
    }
}
