using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 技能加点UI
/// </summary>
public class UISkillLittle : MonoBehaviour
{
    /// <summary>
    /// 技能加点面板的路径
    /// </summary>
    UIFocusPath skillFocusPath;

    /// <summary>
    /// 当前的焦点技能
    /// </summary>
    UIFocusSkillLittleLattice nowSkillLattice;

    /// <summary>
    /// 玩家存档状态
    /// </summary>
    PlayerState playerState;

    /// <summary>
    /// 玩家运行时状态
    /// </summary>
    IPlayerState iPlayerStateRun;

    /// <summary>
    /// 技能的元数据
    /// </summary>
    SkillStructData skillStructData;

    /// <summary>
    /// 技能显示的面板
    /// </summary>
    [SerializeField]
    RectTransform skillsShowPanel;

    void Awake()
    {
        skillFocusPath = GetComponent<UIFocusPath>();
        if (skillFocusPath)
        {
            //给每个技能加一个点击事件，处理鼠标点击获取焦点
            UIFocus[] uiSkillLatticeArray = skillFocusPath.NewUIFocusArray;//skillFocusPath.UIFocuesArray;
            foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
            {
                if (!uiSkillLattice)
                    continue;
                EventTrigger eventTrigger = uiSkillLattice.gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers = new List<EventTrigger.Entry>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback = new EventTrigger.TriggerEvent();
                entry.callback.AddListener(ClickSkillLattice);
                eventTrigger.triggers.Add(entry);
            }
        }
    }

    private void Start()
    {
        //初始化技能控件状态
        UIFocus[] uiSkillLatticeArray = skillFocusPath.NewUIFocusArray;//skillFocusPath.UIFocuesArray;
        foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
        {
            if (!uiSkillLattice)
                continue;
            UIFocusSkillLittleLattice currentSkillLattice = uiSkillLattice as UIFocusSkillLittleLattice;
            if (currentSkillLattice)
            {
                currentSkillLattice.SkillLittleSettingState = false;
                currentSkillLattice.InitSkillShow(UpdatePreconditionState);
            }
        }
        UpdatePreconditionState();
    }

    /// <summary>
    /// 点击技能图标
    /// </summary>
    /// <param name="e"></param>
    private void ClickSkillLattice(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe.pointerCurrentRaycast.gameObject != null)
        {
            UIFocusSkillLittleLattice currentSkillLattice = UITools.FindTargetPopup<UIFocusSkillLittleLattice>(pe.pointerCurrentRaycast.gameObject.transform);
            if (currentSkillLattice)
            {
                nowSkillLattice.LostForcus();
                nowSkillLattice = currentSkillLattice;
                nowSkillLattice.SetForcus();
                nowSkillLattice.SkillLittleSettingState = true;
            }
        }
    }

    /// <summary>
    /// 当显示时初始化技能数据
    /// </summary>
    private void OnEnable()
    {
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        iPlayerStateRun = GameState.Instance.GetEntity<IPlayerState>();
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        UIFocusSkillLittleLattice.tempUseSkillPoint = 0;
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        UIManager.Instance.KeyPressHandle += Instance_KeyPressHandle;
        if (skillFocusPath)
        {
            nowSkillLattice = skillFocusPath.GetFirstFocus() as UIFocusSkillLittleLattice;
            if (nowSkillLattice)
                nowSkillLattice.SetForcus();
            UIFocus[] uiSkillLatticeArray = skillFocusPath.NewUIFocusArray; //skillFocusPath.UIFocuesArray;
            foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
            {
                if (!uiSkillLattice)
                    continue;
                UIFocusSkillLittleLattice uiFocusSkillLittleLattice = uiSkillLattice as UIFocusSkillLittleLattice;
                if (uiFocusSkillLittleLattice)
                    uiFocusSkillLittleLattice.InitSkillShow(UpdatePreconditionState);
            }
            UIFocusSkillLittleLattice.skillPointDic = null;
            UIFocusSkillLittleLattice.zonePointDic = null;
            UpdatePreconditionState();
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        if (nowSkillLattice)
        {
            nowSkillLattice.LostForcus();
        }
        //在退出时保存技能
        bool skillChanged = false;
        UIFocus[] uiSkillLatticeArray = skillFocusPath.NewUIFocusArray;//skillFocusPath.UIFocuesArray;
        foreach (UIFocus uiSkillLattice in uiSkillLatticeArray)
        {
            UIFocusSkillLittleLattice _uiFocus = uiSkillLattice as UIFocusSkillLittleLattice;
            if (playerState.SkillPoint.ContainsKey((EnumSkillType)_uiFocus.skillID))
            {
                if (playerState.SkillPoint[(EnumSkillType)_uiFocus.skillID] != _uiFocus.TempPoint)
                    skillChanged = true;
                playerState.SkillPoint[(EnumSkillType)_uiFocus.skillID] = _uiFocus.TempPoint;
            }
            else
            {
                playerState.SkillPoint.Add((EnumSkillType)_uiFocus.skillID, _uiFocus.TempPoint);
                skillChanged = true;
            }
        }
        Action<EnumSkillType, int> SetEnumSkillPointAction = (enumSkillType, point) => 
        {
            if (playerState.SkillPoint.ContainsKey(enumSkillType))
                playerState.SkillPoint[enumSkillType] = point;
            else playerState.SkillPoint.Add(enumSkillType, point);
        };
        //信仰一的点数需要特殊处理
        if (playerState.SkillPoint.ContainsKey(EnumSkillType.XYX01))
        {
            SetEnumSkillPointAction(EnumSkillType.XYX01_Self, playerState.SkillPoint[EnumSkillType.XYX01]);
            SetEnumSkillPointAction(EnumSkillType.XYX01_Target, playerState.SkillPoint[EnumSkillType.XYX01]);
        }
        if (playerState.SkillPoint.ContainsKey(EnumSkillType.XYX02))
        {
            SetEnumSkillPointAction(EnumSkillType.XYX02_Self, playerState.SkillPoint[EnumSkillType.XYX02]);
            SetEnumSkillPointAction(EnumSkillType.XYX02_Target, playerState.SkillPoint[EnumSkillType.XYX02]);
        }
        if (playerState.SkillPoint.ContainsKey(EnumSkillType.XYX03))
        {
            SetEnumSkillPointAction(EnumSkillType.XYX03_Self, playerState.SkillPoint[EnumSkillType.XYX03]);
            SetEnumSkillPointAction(EnumSkillType.XYX03_Target, playerState.SkillPoint[EnumSkillType.XYX03]);
            SetEnumSkillPointAction(EnumSkillType.XYX03_None, playerState.SkillPoint[EnumSkillType.XYX03]);
        }
        if (playerState.SkillPoint.ContainsKey(EnumSkillType.XYX04))
        {
            SetEnumSkillPointAction(EnumSkillType.XYX04_Target, playerState.SkillPoint[EnumSkillType.XYX04]);
        }
        //如果更改则调用事件
        if (skillChanged)
        {
            iPlayerStateRun.SkillLevelChanged = true;
        }
        UIFocusSkillLittleLattice.tempUseSkillPoint = 0;
    }

    /// <summary>
    /// 接收输入
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (!nowSkillLattice && skillFocusPath)
            nowSkillLattice = skillFocusPath.GetFirstFocus() as UIFocusSkillLittleLattice;
        Action<UIFocusPath.MoveType> CanMoveNextEndAction = (moveType) =>
        {
            if (nowSkillLattice.CanMoveNext(moveType))//可以移动则移动
            {
                if (skillFocusPath)
                {
                    UIFocusSkillLittleLattice currentSkillLattice = skillFocusPath.GetNewNextFocus(nowSkillLattice, moveType) as UIFocusSkillLittleLattice;//skillFocusPath.GetNextFocus(nowSkillLattice, moveType) as UIFocusSkillLittleLattice;
                    if (currentSkillLattice != null && !object.Equals(currentSkillLattice, nowSkillLattice))
                    {
                        nowSkillLattice.LostForcus();
                        nowSkillLattice = currentSkillLattice;
                        nowSkillLattice.SetForcus();
                    }
                }
            }
            else //不可以移动则内部处理
            {
                nowSkillLattice.MoveChild(moveType);
            }
        };
        if (nowSkillLattice)
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    CanMoveNextEndAction(UIFocusPath.MoveType.OK);
                    break;
                case UIManager.KeyType.LEFT:
                    CanMoveNextEndAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    CanMoveNextEndAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    CanMoveNextEndAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    CanMoveNextEndAction(UIFocusPath.MoveType.DOWN);
                    break;

            }
    }

    /// <summary>
    /// 接收持续按下输入
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="value"></param>
    private void Instance_KeyPressHandle(UIManager.KeyType keyType, Vector2 value)
    {
        if (keyType == UIManager.KeyType.RIGHT_ROCKER)
        {
            float x = -value.x * 50;//大于0表示向右推摇杆
            float y = -value.y * 50;//大于0表示向上推摇杆
            if (skillsShowPanel != null)
            {
                float maxX = skillsShowPanel.offsetMax.x + x;
                float minX = skillsShowPanel.offsetMin.x + x;
                if (maxX < 0 || minX > 0)
                    x = 0;
                float maxY = skillsShowPanel.offsetMax.y + y;
                float minY = skillsShowPanel.offsetMin.y + y;
                if (maxY < 0 || minY > 0)
                    y = 0;
                skillsShowPanel.offsetMax += new Vector2(x, y);
                skillsShowPanel.offsetMin += new Vector2(x, y);

            }
        }
    }

    /// <summary>
    /// 更新前置条件状态
    /// </summary>
    private void UpdatePreconditionState()
    {
        //如果对象是空的则使用存档中的数据进行计算
        if (UIFocusSkillLittleLattice.skillPointDic == null || UIFocusSkillLittleLattice.zonePointDic == null)
        {
            //计算已经加的技能点
            UIFocusSkillLittleLattice.skillPointDic = new Dictionary<EnumSkillType, int>();
            foreach (EnumSkillType skillType in Enum.GetValues(typeof(EnumSkillType)))
            {
                if (!UIFocusSkillLittleLattice.skillPointDic.ContainsKey(skillType))
                    UIFocusSkillLittleLattice.skillPointDic.Add(skillType, playerState.SkillPoint.Where(temp => temp.Key == skillType).Sum(temp => temp.Value));
            }
            //计算每组已经加的技能点的总数
            UIFocusSkillLittleLattice.zonePointDic = new Dictionary<EnumSkillZone, int>();
            foreach (EnumSkillZone skillZone in Enum.GetValues(typeof(EnumSkillZone)))
            {
                int point = skillStructData.SearchSkillDatas(temp => temp.skillZones.Contains(skillZone))//首先查询出该组的技能
                    .Select(temp =>
                    {
                        int p = 0;
                        UIFocusSkillLittleLattice.skillPointDic.TryGetValue(temp.skillType, out p);
                        return p;
                    }).Sum();
                UIFocusSkillLittleLattice.zonePointDic.Add(skillZone, point);
            }
        }
        //如果对象不是空则使用UI界面中的临时数据进行计算
        else if (nowSkillLattice)
        {
            int tempSkillPoint = nowSkillLattice.TempPoint;
            EnumSkillType skillType = (EnumSkillType)nowSkillLattice.skillID;
            if (UIFocusSkillLittleLattice.skillPointDic.ContainsKey(skillType))
                UIFocusSkillLittleLattice.skillPointDic[skillType] = tempSkillPoint;
            else
                UIFocusSkillLittleLattice.skillPointDic.Add(skillType, tempSkillPoint);
            EnumSkillZone[] skillZones = new EnumSkillZone[0];
            try
            {
                skillZones = skillStructData.SearchSkillDatas(temp => temp.skillType == skillType).FirstOrDefault().skillZones;
            }
            catch { }
            var tempZonePoints = skillFocusPath.NewUIFocusArray.Select(temp=>temp as UIFocusSkillLittleLattice)// skillFocusPath.UIFocuesArray.Select(temp => temp as UIFocusSkillLittleLattice)
                .Where(temp => temp != null)//从集合中便利所有控件
                .Select(temp =>//从中选出控件代表的技能以及该技能所在的组
                new
                {
                    type = (EnumSkillType)temp.skillID,
                    zone = skillStructData.SearchSkillDatas(typeData => typeData.skillType == (EnumSkillType)temp.skillID)
                                .Select(innerTemp => innerTemp.skillZones).FirstOrDefault()
                })
                .Where(temp => temp.zone != null && temp.zone.Intersect(skillZones).Count() > 0)//两者存在交集则选择此项目
                .Select(temp =>//选出该技能与本次加点技能对应的组
                new
                {
                    skill = playerState.SkillPoint.Where(typeData => typeData.Key == temp.type).FirstOrDefault(),
                    zones = temp.zone.Intersect(skillZones)
                });
            Dictionary<EnumSkillZone, int> tempZonePointDic = new Dictionary<EnumSkillZone, int>();//临时的组加点
            foreach (var tempZonePoint in tempZonePoints)
            {
                foreach (EnumSkillZone zone in tempZonePoint.zones)
                {
                    if (tempZonePointDic.ContainsKey(zone))
                        tempZonePointDic[zone] += tempZonePoint.skill.Value;
                    else tempZonePointDic.Add(zone, tempZonePoint.skill.Value);
                }
            }
            foreach (var itemZonePoint in tempZonePointDic)
            {
                if (UIFocusSkillLittleLattice.zonePointDic.ContainsKey(itemZonePoint.Key))
                    UIFocusSkillLittleLattice.zonePointDic[itemZonePoint.Key] = itemZonePoint.Value;
                else
                    UIFocusSkillLittleLattice.zonePointDic.Add(itemZonePoint.Key, itemZonePoint.Value);
            }

        }

        if (UIFocusSkillLittleLattice.skillMustPointDic == null)
            UIFocusSkillLittleLattice.skillMustPointDic = new Dictionary<EnumSkillType, int>();
        if (UIFocusSkillLittleLattice.zoneMustPointDic == null)
            UIFocusSkillLittleLattice.zoneMustPointDic = new Dictionary<EnumSkillZone, int>();
        UIFocusSkillLittleLattice.skillMustPointDic.Clear();
        UIFocusSkillLittleLattice.zoneMustPointDic.Clear();
        var tempStructData = skillFocusPath.NewUIFocusArray.Select(temp=>temp as UIFocusSkillLittleLattice)// skillFocusPath.UIFocuesArray.Select(temp => temp as UIFocusSkillLittleLattice)
            .Where(temp => temp.TempPoint > 0)
            .Select(temp => (EnumSkillType)temp.skillID)
            .Select(temp => skillStructData.SearchSkillDatas(tempSkill => tempSkill.skillType == temp).FirstOrDefault())
            .Where(temp => temp != null)
            .Select(temp => temp.skillPrecondition);
        IEnumerable<KeyValuePair<EnumSkillType, int>[]> skillTypesDic = tempStructData.Select(temp => temp.mustSkillPointDic.ToArray());
        foreach (var item1 in skillTypesDic)
        {
            foreach (var item2 in item1)
            {
                if (UIFocusSkillLittleLattice.skillMustPointDic.ContainsKey(item2.Key))
                {
                    if (UIFocusSkillLittleLattice.skillMustPointDic[item2.Key] < item2.Value)
                        UIFocusSkillLittleLattice.skillMustPointDic[item2.Key] = item2.Value;
                }
                else
                    UIFocusSkillLittleLattice.skillMustPointDic.Add(item2.Key, item2.Value);
            }
        }
        IEnumerable<KeyValuePair<EnumSkillZone, int>[]> skillZonesDic = tempStructData.Select(temp => temp.mustSkillZonePointDic.ToArray());
        foreach (var item1 in skillZonesDic)
        {
            foreach (var item2 in item1)
            {
                if (UIFocusSkillLittleLattice.zoneMustPointDic.ContainsKey(item2.Key))
                {
                    if (UIFocusSkillLittleLattice.zoneMustPointDic[item2.Key] < item2.Value)
                        UIFocusSkillLittleLattice.zoneMustPointDic[item2.Key] = item2.Value;
                }
                else
                    UIFocusSkillLittleLattice.zoneMustPointDic.Add(item2.Key, item2.Value);
            }
        }
    }
}
