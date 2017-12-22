using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 组合技能的格子
/// </summary>
public class UIFocusSkillCombineLattice : UIFocus
{
    [Range(1, 4, order = 1)]
    [SerializeField]
    private int level = 1;
    /// <summary>
    /// 背景图片
    /// </summary>
    [SerializeField]
    private Image backLineImage;
    /// <summary>
    /// 技能图片
    /// </summary>
    [SerializeField]
    private Image skillImage;

    /// <summary>
    /// 获取技能的阶段
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
    }

    /// <summary>
    /// 设置技能的图标
    /// </summary>
    public Sprite SkillSprite
    {
        set
        {
            if (skillImage)
            {
                skillImage.sprite = value;
                skillImage.gameObject.SetActive(value != null);
            }
        }
        get
        {
            return skillImage == null ? null : skillImage.sprite;
        }
    }

    /// <summary>
    /// 设置焦点
    /// </summary>
    public override void SetForcus()
    {
        backLineImage.enabled = true;
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public override void LostForcus()
    {
        backLineImage.enabled = false;
    }

}
