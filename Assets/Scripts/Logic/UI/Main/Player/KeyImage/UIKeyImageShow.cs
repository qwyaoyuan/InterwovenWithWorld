using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主界面的按钮显示UI
/// </summary>
public class UIKeyImageShow : MonoBehaviour
{

    /// <summary>
    /// 主要用于指定ABXY上向左右八个按键
    /// </summary>
    [Range(1, 8, order = 1)]
    [SerializeField]
    private int InputType;

    /// <summary>
    /// 该键的显示图片控件
    /// </summary>
    Image keyImage;

    /// <summary>
    /// 按键与技能物品之间的对应关系对象
    /// </summary>
    KeyContactData keyContactData;

    /// <summary>
    /// 技能结构数据
    /// </summary>
    SkillStructData skillStructData;

    /// <summary>
    /// 玩家的存档状态
    /// </summary>
    PlayerState playerState;

    bool r1Press;

    bool r2Press;

    bool l1Press;

    bool l2Press;

    void Start()
    {
        keyImage = GetComponent<Image>();
        keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        Show();
    }

    void OnEnable()
    {
        UIManager.Instance.KeyPressHandle += Instance_KeyPressHandle;
    }

    void OnDisable()
    {
        UIManager.Instance.KeyPressHandle -= Instance_KeyPressHandle;
    }

    private void Instance_KeyPressHandle(UIManager.KeyType arg1, Vector2 arg2)
    {
        if (keyContactData != null)
        {
            switch (arg1)
            {
                case UIManager.KeyType.R1:
                    r1Press = true;
                    break;
                case UIManager.KeyType.R2:
                    r2Press = true;
                    break;
                case UIManager.KeyType.L1:
                    l1Press = true;
                    break;
                case UIManager.KeyType.L2:
                    l2Press = true;
                    break;
            }
            Show();
        }
    }

    private void Show()
    {
        if (skillStructData == null || playerState == null || keyImage == null)
            return;
        KeyContactStruct keyContactStruct = default(KeyContactStruct);
        //这里存在优化空间
        if (r1Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.RB).FirstOrDefault();
        else if (r2Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.RT).FirstOrDefault();
        else if (l1Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.LB).FirstOrDefault();
        else if (l2Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.LT).FirstOrDefault();

        switch (keyContactStruct.keyContactType)
        {
            case EnumKeyContactType.None:
                keyImage.sprite = null;
                break;
            case EnumKeyContactType.Skill:
                int skillKey = keyContactStruct.id;
                if (skillKey > (int)EnumSkillType.MagicCombinedStart)
                    keyImage.sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, skillKey);
                else
                    keyImage.sprite = skillStructData.SearchSkillDatas(temp => (int)temp.skillType == skillKey).Select(temp => temp.skillSprite).FirstOrDefault();
                break;
            case EnumKeyContactType.Prap:
                PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => (int)temp.GoodsInfo.EnumGoodsType == keyContactStruct.id).FirstOrDefault();
                if (playGoods != null)
                {
                    keyImage.sprite = playGoods.GetGoodsSprite();
                }
                else
                    keyImage.sprite = null;
                break;
            case EnumKeyContactType.Action:
                keyImage.sprite = null;
                break;
        }
        keyImage.enabled = keyImage.sprite != null;
        r1Press = false;
        r2Press = false;
        l1Press = false;
        l2Press = false;
    }
}
