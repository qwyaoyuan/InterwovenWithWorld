using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 技能组合UI
/// </summary>
public class UISkillCombine : MonoBehaviour
{
    /// <summary>
    /// 技能组合框路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的技能组合框
    /// </summary>
    UIFocusSkillCombineLattice nowUISkilCombineLattice;
    /// <summary>
    /// 组合后的技能集合
    /// </summary>
    [SerializeField]
    private UIList uiCombineSkillList;
    /// <summary>
    /// 选择技能集合
    /// </summary>
    [SerializeField]
    private UIList uiSelectSkillList;

    /// <summary>
    /// 技能组合UI界面的状态
    /// </summary>
    EnumUISkillCombine enumUISkillCombine;

    /// <summary>
    /// 当前组合技能集合的选中条目
    /// </summary>
    UIListItem nowCombineSkillItem;
    /// <summary>
    /// 当前选择技能集合的选中条目
    /// </summary>
    UIListItem nowSelectSkillItem;
    /// <summary>
    /// 技能对象
    /// </summary>
    SkillStructData skillStructData_Base;
    /// <summary>
    /// 玩家状态
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// 组合技能对应图片的缓存字典
    /// </summary>
    public Dictionary<int, Texture> combineTextDic;

    private void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
        combineTextDic = new Dictionary<int, Texture>();
        uiCombineSkillList.ItemClickHandle += CombineSkillList_ItemClickHandle;
        uiSelectSkillList.ItemClickHandle += SelectSkillList_ItemClickHandle;
    }

    private void OnEnable()
    {
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        //重新载入数据
        skillStructData_Base = DataCenter.Instance.GetMetaData<SkillStructData>();//元数据
        playerState = DataCenter.Instance.GetEntity<PlayerState>();//玩家状态
        uiCombineSkillList.Init();
        foreach (EnumSkillType[] skillTypes in playerState.CombineSkills)
        {
            SkillBaseStruct[] thisUsedSkills = skillStructData_Base.SearchSkillDatas(temp => skillTypes.Contains(temp.skillType));
            int combineSkillKey = SkillCombineStaticTools.GetCombineSkillKey(thisUsedSkills.Select(temp => temp.skillType).ToArray());
            Sprite combineSkillSprite = CombineSprite(thisUsedSkills.Select(temp => temp.skillSprite).ToArray(), combineSkillKey);
            UIListItem thisUIListItem = uiCombineSkillList.NewItem();
            thisUIListItem.childImage.sprite = combineSkillSprite;
            thisUIListItem.value = thisUsedSkills.Select(temp => temp.skillType).ToArray();
        }
        if (playerState.CombineSkills.Count < 30)
        {
            for (int i = 0; i < 30 - playerState.CombineSkills.Count; i++)
            {
                UIListItem thisUIListItem = uiCombineSkillList.NewItem();
                thisUIListItem.childImage.sprite = null;
                thisUIListItem.value = new EnumSkillType[4];
            }
        }
        uiCombineSkillList.UpdateUI();
        //重设状态
        enumUISkillCombine = EnumUISkillCombine.CombineSkillItem;
        nowCombineSkillItem = uiCombineSkillList.FirstShowItem();
        uiCombineSkillList.ShowItem(nowCombineSkillItem);
    }

    /// <summary>
    /// 组合技能的技能图片
    /// </summary>
    /// <param name="sprites">分技能的精灵</param>
    /// <param name="key">该技能的组合值</param>
    /// <returns></returns>
    private Sprite CombineSprite(Sprite[] sprites, int key)
    {
        if (sprites == null || sprites.Length == 0)
            return null;
        var sizes = sprites.Select(temp => new { width = temp.bounds.size.x, height = temp.bounds.size.y });
        float width = sizes.OrderBy(temp => temp.width).FirstOrDefault().width;
        float height = sizes.OrderBy(temp => temp.height).FirstOrDefault().height;
        if (width == 0 || height == 0)
            return null;
        return null;
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        if (nowUISkilCombineLattice)
            nowUISkilCombineLattice.LostForcus();
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (enumUISkillCombine)
        {
            case EnumUISkillCombine.CombineSkillItem://处于左侧状态
                switch (keyType)
                {
                    case UIManager.KeyType.A:
                        if (nowCombineSkillItem)
                        {
                            //设置技能框的边框显示
                            nowUISkilCombineLattice = uiFocusPath.GetFirstFocus<UIFocusSkillCombineLattice>();
                            if (nowUISkilCombineLattice)
                                nowUISkilCombineLattice.SetForcus();
                            //同时将模式更改为CombineSkillLattice
                            enumUISkillCombine = EnumUISkillCombine.CombineSkillLattice;
                        }
                        break;
                    case UIManager.KeyType.UP:
                    case UIManager.KeyType.DOWN:
                        {
                            UIListItem[] tempArrays = uiCombineSkillList.GetAllImtes();
                            int index = 0;
                            if (nowCombineSkillItem)
                                index = tempArrays.ToList().IndexOf(nowCombineSkillItem);
                            if (index < 0)
                                index = 0;
                            switch (keyType)
                            {
                                case UIManager.KeyType.UP: index--; break;
                                case UIManager.KeyType.DOWN: index++; break;
                            }
                            index = Mathf.Clamp(index, 0, tempArrays.Length - 1);
                            if (index < tempArrays.Length)
                            {
                                uiCombineSkillList.ShowItem(tempArrays[index]);
                                if (nowCombineSkillItem && nowCombineSkillItem.childImage)
                                    nowCombineSkillItem.childImage.enabled = false;
                                nowCombineSkillItem = tempArrays[index];
                                if (nowCombineSkillItem && nowCombineSkillItem.childImage)
                                    nowCombineSkillItem.childImage.enabled = true;
                                if (nowCombineSkillItem)
                                {
                                    SetSkillCombineLatticeAndShowVadio((int)nowCombineSkillItem.value);
                                }
                            }
                        }
                        break;
                }
                break;
            case EnumUISkillCombine.CombineSkillLattice://处于右侧状态
                switch (keyType)
                {
                    case UIManager.KeyType.A:
                        SetSelectSkill();
                        break;
                    case UIManager.KeyType.B:
                        if (nowUISkilCombineLattice)
                            nowUISkilCombineLattice.LostForcus();
                        enumUISkillCombine = EnumUISkillCombine.CombineSkillItem;//切换模式到组合技能集合
                        break;
                    case UIManager.KeyType.LEFT:
                    case UIManager.KeyType.RIGHT:
                        if (nowUISkilCombineLattice)
                        {
                            UIFocusSkillCombineLattice tempUIFocuesSkillCombineLattice =
                                uiFocusPath.GetNextFocus(nowUISkilCombineLattice, keyType == UIManager.KeyType.LEFT ? UIFocusPath.MoveType.LEFT : UIFocusPath.MoveType.RIGHT) as UIFocusSkillCombineLattice;
                            if (tempUIFocuesSkillCombineLattice)
                            {
                                nowUISkilCombineLattice.LostForcus();
                                nowUISkilCombineLattice = tempUIFocuesSkillCombineLattice;
                                nowUISkilCombineLattice.SetForcus();
                            }
                        }
                        break;
                }
                break;
            case EnumUISkillCombine.CombineSkillSelect://处于选择技能状态
                switch (keyType)
                {
                    case UIManager.KeyType.A:
                        ResetCombineSkillLattice();
                        break;
                    case UIManager.KeyType.B:
                        //切换状态
                        uiSelectSkillList.gameObject.SetActive(false);
                        enumUISkillCombine = EnumUISkillCombine.CombineSkillLattice;
                        break;
                    case UIManager.KeyType.UP:
                    case UIManager.KeyType.DOWN:
                        {
                            UIListItem[] tempArrays = uiSelectSkillList.GetAllImtes();
                            int index = 0;
                            if (nowCombineSkillItem)
                                index = tempArrays.ToList().IndexOf(nowSelectSkillItem);
                            if (index < 0)
                                index = 0;
                            switch (keyType)
                            {
                                case UIManager.KeyType.UP: index--; break;
                                case UIManager.KeyType.DOWN: index++; break;
                            }
                            index = Mathf.Clamp(index, 0, tempArrays.Length - 1);
                            if (index < tempArrays.Length)
                            {
                                uiSelectSkillList.ShowItem(tempArrays[index]);
                                if (uiSelectSkillList && nowSelectSkillItem.childImage)
                                    nowSelectSkillItem.childImage.enabled = false;
                                nowSelectSkillItem = tempArrays[index];
                                if (uiSelectSkillList && nowSelectSkillItem.childImage)
                                    nowSelectSkillItem.childImage.enabled = true;
                            }
                        }
                        break;
                }
                break;
        }
    }


    /// <summary>
    /// 选择技能集合列表点击
    /// </summary>
    /// <param name="mouseType"></param>
    /// <param name="selectSkillItem"></param>
    private void SelectSkillList_ItemClickHandle(UIList.ItemClickMouseType mouseType, UIListItem selectSkillItem)
    {
        nowSelectSkillItem = selectSkillItem;
        ResetCombineSkillLattice();
    }

    /// <summary>
    /// 重置当前选中技能的选择技能
    /// </summary>
    private void ResetCombineSkillLattice()
    {
        //重置技能
        if (nowSelectSkillItem)
        {
            //需要修改图标以及修改组合技能集合中选中技能的名字
            //--------------------------//         
        }
        //切换状态到技能框
        uiSelectSkillList.gameObject.SetActive(false);
        enumUISkillCombine = EnumUISkillCombine.CombineSkillLattice;
    }

    /// <summary>
    /// 设置技能组合框的图标
    /// 根据技能组合查找相关视频并显示
    /// </summary>
    /// <param name="id">组合技能的id</param>
    private void SetSkillCombineLatticeAndShowVadio(int id)
    {
        //需要修改技能组合框的图标以及显示该技能的视频（如果没有则不播放）
        //---------------------------//
    }

    /// <summary>
    /// 组合技能集合列表项点击
    /// </summary>
    /// <param name="mouseType"></param>
    /// <param name="combineSkillItem"></param>
    private void CombineSkillList_ItemClickHandle(UIList.ItemClickMouseType mouseType, UIListItem combineSkillItem)
    {
        if (enumUISkillCombine == EnumUISkillCombine.CombineSkillItem || enumUISkillCombine == EnumUISkillCombine.CombineSkillLattice)
        {
            if (!combineSkillItem)
                return;
            if (nowCombineSkillItem && nowCombineSkillItem.childImage)
                nowCombineSkillItem.childImage.enabled = false;
            nowCombineSkillItem = combineSkillItem;
            if (nowCombineSkillItem && nowCombineSkillItem.childImage)
                nowCombineSkillItem.childImage.enabled = true;
            SetSkillCombineLatticeAndShowVadio((int)combineSkillItem.value);
        }
    }

    /// <summary>
    /// 当点击组合技能框时
    /// </summary>
    public void CombineSkillLatticeClick(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe == null)
            return;
        UIFocusSkillCombineLattice target = UITools.FindTargetPopup<UIFocusSkillCombineLattice>(pe.pointerCurrentRaycast.gameObject.transform);
        if (enumUISkillCombine == EnumUISkillCombine.CombineSkillItem || enumUISkillCombine == EnumUISkillCombine.CombineSkillLattice)
        {
            if (target)
            {
                if (nowUISkilCombineLattice)
                    nowUISkilCombineLattice.LostForcus();
                nowUISkilCombineLattice = target;
                nowUISkilCombineLattice.SetForcus();
                SetSelectSkill();
            }
        }
    }

    /// <summary>
    /// 根据当前的技能框项显示技能选择
    /// </summary>
    private void SetSelectSkill()
    {
        uiSelectSkillList.gameObject.SetActive(true);
        //根据当前的技能框，选择技能的种类来初始化技能选择集合并显示该集合
        uiSelectSkillList.Init();
        //需要选中的技能框以及已经存放的技能显示可以放入的技能
        //-----------------------//
        int index = uiFocusPath.UIFocuesArray.ToList().IndexOf(nowUISkilCombineLattice);
        //判断是否可以组合技能
        //-----------------------//


        //设置第一个技能高亮
        nowSelectSkillItem = uiSelectSkillList.FirstShowItem();
        if (nowSelectSkillItem)
            nowSelectSkillItem.childImage.enabled = true;
        //然后切换状态到技能选择 
        enumUISkillCombine = EnumUISkillCombine.CombineSkillSelect;
    }

    /// <summary>
    /// 技能组合界面的状态
    /// </summary>
    private enum EnumUISkillCombine
    {
        /// <summary>
        /// 组合技能集合中
        /// </summary>
        CombineSkillItem,
        /// <summary>
        /// 组合技能框
        /// </summary>
        CombineSkillLattice,
        /// <summary>
        /// 组合技能选择
        /// </summary>
        CombineSkillSelect
    }
}
