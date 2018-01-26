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
            int key = SkillCombineStaticTools.GetCombineSkillKey(thisUsedSkills.Select(temp => temp.skillType));
            UIListItem thisUIListItem = uiCombineSkillList.NewItem();
            thisUIListItem.value = thisUsedSkills.Select(temp => temp.skillType).ToArray();
            thisUIListItem.childText.text = CombineSkillText(key);
        }
        if (playerState.CombineSkills.Count < 30)
        {
            for (int i = 0; i < 30 - playerState.CombineSkills.Count; i++)
            {
                UIListItem thisUIListItem = uiCombineSkillList.NewItem();
                thisUIListItem.value = new EnumSkillType[4];
                thisUIListItem.childText.text = "[None]";
            }
        }
        uiCombineSkillList.UpdateUI();
        //重设状态
        enumUISkillCombine = EnumUISkillCombine.CombineSkillItem;
        uiCombineSkillList.CanClickListItem = true;
        nowCombineSkillItem = uiCombineSkillList.FirstShowItem();
        uiCombineSkillList.ShowItem(nowCombineSkillItem);
        if (nowCombineSkillItem)
        {
            nowCombineSkillItem.childImage.enabled = true;
            nowCombineSkillItem.childImage.gameObject.SetActive(true);
            SetSkillCombineLatticeAndShowVadio((EnumSkillType[])nowCombineSkillItem.value);
        }
    }

    /// <summary>
    /// 融合技能的名字
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string CombineSkillText(int key)
    {
        EnumSkillType[] skills = SkillCombineStaticTools.GetCombineSkills(key);
        SkillBaseStruct[] thisUsedSkills = skillStructData_Base.SearchSkillDatas(temp => skills.Contains(temp.skillType));
        string combineSkillNames = SkillCombineStaticTools.GetCombineSkillsName(thisUsedSkills);
        if (string.IsNullOrEmpty(combineSkillNames))
            return "[None]";
        return combineSkillNames;
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        if (nowUISkilCombineLattice)
            nowUISkilCombineLattice.LostForcus();
        //保存数据
        UIListItem[] allUIListItems = uiCombineSkillList.GetAllImtes();
        playerState.CombineSkills.Clear();
        foreach (UIListItem uiListItem in allUIListItems)
        {
            if (uiListItem != null && uiListItem.value!=null && uiListItem.value.GetType().Equals(typeof(EnumSkillType[])))
            {
                EnumSkillType[] enumSkillTypes = uiListItem.value as EnumSkillType[];
                if (enumSkillTypes.Count(temp => temp == EnumSkillType.None) != 4)
                {
                    playerState.CombineSkills.Add(enumSkillTypes);
                }
            }
        }
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
                            uiCombineSkillList.CanClickListItem = false;
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
                                {
                                    nowCombineSkillItem.childImage.enabled = false;
                                    nowCombineSkillItem.childImage.gameObject.SetActive(false);
                                }
                                nowCombineSkillItem = tempArrays[index];
                                if (nowCombineSkillItem && nowCombineSkillItem.childImage)
                                {
                                    nowCombineSkillItem.childImage.enabled = true;
                                    nowCombineSkillItem.childImage.gameObject.SetActive(true);
                                }
                                if (nowCombineSkillItem)
                                {
                                    SetSkillCombineLatticeAndShowVadio((EnumSkillType[])nowCombineSkillItem.value);
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
                        uiCombineSkillList.CanClickListItem = true;
                        break;
                    case UIManager.KeyType.LEFT:
                    case UIManager.KeyType.RIGHT:
                        if (nowUISkilCombineLattice)
                        {
                            UIFocusSkillCombineLattice tempUIFocuesSkillCombineLattice =
                                uiFocusPath.GetNewNextFocus(nowUISkilCombineLattice, keyType == UIManager.KeyType.LEFT ? UIFocusPath.MoveType.LEFT : UIFocusPath.MoveType.RIGHT) as UIFocusSkillCombineLattice;
                                //uiFocusPath.GetNextFocus(nowUISkilCombineLattice, keyType == UIManager.KeyType.LEFT ? UIFocusPath.MoveType.LEFT : UIFocusPath.MoveType.RIGHT) as UIFocusSkillCombineLattice;
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
                                {
                                    nowSelectSkillItem.childImage.enabled = false;
                                    nowSelectSkillItem.childImage.gameObject.SetActive(false);
                                }
                                nowSelectSkillItem = tempArrays[index];
                                if (uiSelectSkillList && nowSelectSkillItem.childImage)
                                {
                                    nowSelectSkillItem.childImage.enabled = true;
                                    nowSelectSkillItem.childImage.gameObject.SetActive(true);
                                }
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
        if (nowSelectSkillItem && nowUISkilCombineLattice)
        {
            //判断当前锁定框的位置并修改技能
            int level = nowUISkilCombineLattice.Level;
            EnumSkillType[] thisCombineSkillTypes = nowCombineSkillItem.value as EnumSkillType[];
            EnumSkillType[] tempCombineSkillTypes = thisCombineSkillTypes.Clone() as EnumSkillType[];
            tempCombineSkillTypes[level - 1] = (EnumSkillType)nowSelectSkillItem.value;
            if (SkillCombineStaticTools.GetCanCombineSkills(tempCombineSkillTypes)//该框内是否可以使用该技能
                || tempCombineSkillTypes.Count(temp => temp == EnumSkillType.None) == 4//说明都是空的
                )
            {
                thisCombineSkillTypes[level - 1] = (EnumSkillType)nowSelectSkillItem.value;
                //修改组合框的图标
                SetSkillCombineLatticeAndShowVadio(thisCombineSkillTypes);
                //修改组合技能集合中选中技能的名字
                if (nowCombineSkillItem)
                {
                    int key = SkillCombineStaticTools.GetCombineSkillKey(thisCombineSkillTypes);
                    nowCombineSkillItem.childText.text = CombineSkillText(key);
                }
            }
        }
        //切换状态到技能框
        uiSelectSkillList.gameObject.SetActive(false);
        enumUISkillCombine = EnumUISkillCombine.CombineSkillLattice;
    }

    /// <summary>
    /// 设置技能组合框的图标
    /// 根据技能组合查找相关视频并显示
    /// </summary>
    /// <param name="skillTypes">组合技能的类型</param>
    private void SetSkillCombineLatticeAndShowVadio(EnumSkillType[] skillTypes)
    {
        //需要修改技能组合框的图标以及显示该技能的视频（如果没有则不播放）
        SkillBaseStruct[] thisUsedSkills = skillStructData_Base.SearchSkillDatas(temp => skillTypes.Contains(temp.skillType));
        UIFocusSkillCombineLattice[] uiFocus = uiFocusPath.NewUIFocusArray.Select(temp => temp as UIFocusSkillCombineLattice).ToArray();//uiFocusPath.UIFocuesArray.Select(temp => temp as UIFocusSkillCombineLattice).ToArray();
        for (int i = 0; i < uiFocus.Length; i++)
        {
            if (thisUsedSkills.Length > i)
            {
                uiFocus[i].SkillSprite = SkillSpriteData.GetSprite(thisUsedSkills[i].skillType);
            }
            else
            {
                uiFocus[i].SkillSprite = null;
            }
        }
        //throw new Exception("未实现查找视频");
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
            {
                nowCombineSkillItem.childImage.enabled = false;
                nowCombineSkillItem.childImage.gameObject.SetActive(false);
            }
            nowCombineSkillItem = combineSkillItem;
            if (nowCombineSkillItem && nowCombineSkillItem.childImage)
            {
                nowCombineSkillItem.childImage.enabled = true;
                nowCombineSkillItem.childImage.gameObject.SetActive(true);
            }
            SetSkillCombineLatticeAndShowVadio((EnumSkillType[])combineSkillItem.value);
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
        //当前的技能组合
        EnumSkillType[] nowSkillCombineTypes = (nowCombineSkillItem.value as EnumSkillType[]).Clone() as EnumSkillType[];
        //判断当前锁定框的位置
        int level = nowUISkilCombineLattice.Level;
        //通过位置和判断是否可以组合技能显示需要添加的技能
        EnumSkillType[] canSetSkillTypes = SkillCombineStaticTools.GetBaseSkillBackCombineSkillIndex(level);//该阶段的技能
        EnumSkillType[] addedPointSkillTypes = playerState.SkillPoint.Where(temp => temp.Value > 0).Select(temp => temp.Key).ToArray();//所有加点的技能
        List<EnumSkillType> mustShowSkillTypes = canSetSkillTypes.Intersect(addedPointSkillTypes).ToList(); //简略判断可以显示的技能
        //将现有技能组合判断是否可以使用技能
        mustShowSkillTypes.RemoveAll(temp =>
        {
            nowSkillCombineTypes[level - 1] = temp;
            return !SkillCombineStaticTools.GetCanCombineSkills(nowSkillCombineTypes);
        });
        mustShowSkillTypes.Insert(0, EnumSkillType.None);//第一个必须是None技能
        foreach (EnumSkillType mustShowSkillType in mustShowSkillTypes)
        {
            UIListItem uiListItem = uiSelectSkillList.NewItem();
            SkillBaseStruct tempSkillBaseStruct = skillStructData_Base.SearchSkillDatas(temp => temp.skillType == mustShowSkillType).FirstOrDefault();
            //uiListItem.childImage.sprite = tempSkillBaseStruct == null ? null : tempSkillBaseStruct.skillSprite;
            uiListItem.value = mustShowSkillType;
            uiListItem.childText.text = SkillCombineStaticTools.GetSingleSkillName(mustShowSkillType);
        }
        uiSelectSkillList.UpdateUI();
        //设置第一个技能高亮
        nowSelectSkillItem = uiSelectSkillList.FirstShowItem();
        if (nowSelectSkillItem)
        {
            nowSelectSkillItem.childImage.gameObject.SetActive(true);
            nowSelectSkillItem.childImage.enabled = true;
        }
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
