using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public EnumKeyContactType keyLatticeType;

    /// <summary>
    /// 对应的id
    /// </summary>
    public int id;

    /// <summary>
    /// 玩家存档对象
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// 技能元数据
    /// </summary>
    SkillStructData skillStructData;

    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
    }

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
        switch (keyLatticeType)
        {
            case EnumKeyContactType.Skill:
                if (id > (int)EnumSkillType.MagicCombinedStart)//组合技能
                {
                    SetTargetImage(SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, id));
                }
                else//单一的技能
                {
                   SkillBaseStruct skillBaseStruct =  skillStructData.SearchSkillDatas(temp => temp.skillType == (EnumSkillType)id).FirstOrDefault();
                    if (skillBaseStruct != null)
                    {
                        SetTargetImage(skillBaseStruct.skillSprite);
                    }
                    else SetTargetImage(null);
                }
                break;
            case EnumKeyContactType.Prap:
                PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.ID == id).FirstOrDefault();
                SetTargetImage(playGoods.GoodsInfo.Sprite);
                break;
            default:
                SetTargetImage(null);
                break;
        }
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

    ///// <summary>
    ///// 格子存放的类型
    ///// </summary>
    //public enum EnumKeyLatticeType
    //{
    //    /// <summary>
    //    /// 技能
    //    /// </summary>
    //    Skill,
    //    /// <summary>
    //    /// 道具
    //    /// </summary>
    //    Item
    //}
}
