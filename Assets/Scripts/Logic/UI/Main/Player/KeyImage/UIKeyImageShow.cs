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
    /// 内部图片(用于显示冷却)
    /// </summary>
    public Image innerImage;

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

    /// <summary>
    /// 技能状态
    /// </summary>
    ISkillState iSkillState;

    /// <summary>
    /// 玩家状态
    /// </summary>
    IPlayerState iPlayerState;

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
        iSkillState = GameState.Instance.GetEntity<ISkillState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
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

    private void LateUpdate()
    {
        Show();
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
        }
    }

    private void Show()
    {
        if (skillStructData == null || playerState == null || keyImage == null)
            return;
        KeyContactStruct keyContactStruct = default(KeyContactStruct);
        //这里存在优化空间
        if (r1Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.RB, null, iPlayerState.KeyContactDataZone).FirstOrDefault();
        else if (r2Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.RT, null, iPlayerState.KeyContactDataZone).FirstOrDefault();
        else if (l1Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.LB, null, iPlayerState.KeyContactDataZone).FirstOrDefault();
        else if (l2Press)
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType | (int)EnumInputType.LT, null, iPlayerState.KeyContactDataZone).FirstOrDefault();
        else
            keyContactStruct = keyContactData.GetKeyContactStruct(InputType, null, iPlayerState.KeyContactDataZone).FirstOrDefault();

        switch (keyContactStruct.keyContactType)
        {
            case EnumKeyContactType.None:
                keyImage.sprite = null;
                innerImage.fillAmount = 0;
                break;
            case EnumKeyContactType.Skill:
                int skillKey = keyContactStruct.id;
                if (skillKey > (int)EnumSkillType.MagicCombinedStart)
                    keyImage.sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, skillKey);
                else
                    keyImage.sprite = skillStructData.SearchSkillDatas(temp => (int)temp.skillType == skillKey).Select(temp => temp.skillSprite).FirstOrDefault();
                //这里还需要判断是不是没有冷却的技能,比如魔法释放 普通攻击等
                if (skillKey != (int)EnumSkillType.MagicRelease && skillKey != (int)EnumSkillType.PhysicAttack)
                    innerImage.fillAmount = Mathf.Clamp(iSkillState.PublicCoolingTime / 1, 0, 1);
                else
                    innerImage.fillAmount = 0;
                break;
            case EnumKeyContactType.Prap:
                PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => (int)temp.GoodsInfo.EnumGoodsType == keyContactStruct.id).FirstOrDefault();
                if (playGoods != null)
                {
                    keyImage.sprite = playGoods.GetGoodsSprite;
                }
                else
                    keyImage.sprite = null;
                innerImage.fillAmount = 0;
                break;
            case EnumKeyContactType.Action:
                keyImage.sprite = keyContactStruct.Sprite;
                innerImage.fillAmount = 0;
                break;
        }
        keyImage.enabled = keyImage.sprite != null;
        r1Press = false;
        r2Press = false;
        l1Press = false;
        l2Press = false;
    }
}
