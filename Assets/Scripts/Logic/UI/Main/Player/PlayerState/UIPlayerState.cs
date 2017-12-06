using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 主UI界面的玩家状态显示(血量 蓝条 buff debuff 特殊状态)
/// </summary>
public class UIPlayerState : MonoBehaviour
{
    /// <summary>
    /// 显示血条的图片
    /// </summary>
    public Image hpValueImage;

    /// <summary>
    /// 显示蓝条的图片
    /// </summary>
    public Image mpValueImage;

    /// <summary>
    /// 显示血量具体数值的文本
    /// </summary>
    public Text hpValueText;

    /// <summary>
    /// 显示蓝量具体数值的文本
    /// </summary>
    public Text mpValueText;

    /// <summary>
    /// 用于排列状态图标的组件
    /// </summary>
    public GridLayoutGroup stateGrid;

    /// <summary>
    /// 状态图标的例子对象
    /// </summary>
    public GameObject stateExamplePrefab;
    /// <summary>
    /// 状态枚举值对应显示图标字典
    /// </summary>
    private Dictionary<EnumStatusEffect, UIPlayerState_State> statusEffectToObjDic;
    /// <summary>
    /// 通过字段名查找对应状态的字典
    /// </summary>
    private Dictionary<string, Func<IBaseState, BuffState>> GetBuffStateByNameDic;


    void Start()
    {
        stateExamplePrefab.AddComponent<UIPlayerState_State>();//给预设添加脚本(以后的对象就不用添加了)
        GetBuffStateByNameDic = new Dictionary<string, Func<IBaseState, BuffState>>();
        statusEffectToObjDic = new Dictionary<EnumStatusEffect, UIPlayerState_State>();
        GameState.Instance.Registor<IBuffState>(IState_Callback);
        GameState.Instance.Registor<IDebuffState>(IState_Callback);
        GameState.Instance.Registor<ISpecialState>(IState_Callback);
        GameState.Instance.Registor<IAttributeState>(IAttributeState_Callback);
        IAttributeState iAttributeState = GameState.Instance.GetEntity<IAttributeState>();
        ChangeHPShow(iAttributeState);
        ChangeMPShow(iAttributeState);
    }

    /// <summary>
    /// 状态发生变化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="iState"></param>
    /// <param name="fieldName"></param>
    private void IState_Callback<T>(T iState, string fieldName) where T : IBaseState
    {
        BuffState buffState;
        if (TryGetBuff(iState, fieldName, out buffState))
        {
            if (!statusEffectToObjDic.ContainsKey(buffState.statusEffect))
            {
                GameObject stateObj = GameObject.Instantiate(stateExamplePrefab);
                stateObj.transform.SetParent(stateGrid.transform);
                UIPlayerState_State uiPlayerState_State = stateObj.GetComponent<UIPlayerState_State>();
                statusEffectToObjDic.Add(buffState.statusEffect, uiPlayerState_State);
            }
            if (statusEffectToObjDic.ContainsKey(buffState.statusEffect))
            {
                if (statusEffectToObjDic[buffState.statusEffect].SetState(buffState))
                {
                    //如果发生了显示变化则更新组件的排序
                    stateGrid.SetLayoutHorizontal();
                    stateGrid.SetLayoutVertical();
                }
            }
        }
    }

    /// <summary>
    /// 尝试获取BuffState对象
    /// </summary>
    /// <param name="iBaseState">接口对象</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="buffState">要返回的对象</param>
    /// <returns>是否取到了BuffState对象</returns>
    private bool TryGetBuff(IBaseState iBaseState, string fieldName, out BuffState buffState)
    {
        if (!GetBuffStateByNameDic.ContainsKey(fieldName))
        {
            if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Chaofeng))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Chaofeng);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Hunluan))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Hunluan);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Jiangzhi))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Jiangzhi);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Kongju))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Kongju);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Meihuo))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Meihuo);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Xuanyun))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Xuanyun);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Zhimang))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Zhimang);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Jingu))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Jingu);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Jinmo))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Jinmo);
            else if (fieldName == GameState.Instance.GetFieldName<ISpecialState, BuffState>(temp => temp.Mabi))
                GetBuffStateByNameDic.Add(fieldName, temp => ((ISpecialState)temp).Mabi);

            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Huoli))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Huoli);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Jiasu))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Jiasu);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Jinghua))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Jinghua);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Minjie))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Minjie);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Qiangli))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Qiangli);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Qusan))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Qusan);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.Ruizhi))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).Ruizhi);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.XixueWuli))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).XixueWuli);
            else if (fieldName == GameState.Instance.GetFieldName<IBuffState, BuffState>(temp => temp.XixueMofa))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IBuffState)temp).XixueMofa);

            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Bingdong))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Bingdong);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Chidun))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Chidun);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Dianran))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Dianran);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Diaoling))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Diaoling);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Jiansu))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Jiansu);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Mihuo))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Mihuo);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Wuli))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Wuli);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Xuruo))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Xuruo);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Zhongdu))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Zhongdu);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.Zuzhou))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).Zuzhou);
            else if (fieldName == GameState.Instance.GetFieldName<IDebuffState, BuffState>(temp => temp.LiuXue))
                GetBuffStateByNameDic.Add(fieldName, temp => ((IDebuffState)temp).LiuXue);
        }
        if (GetBuffStateByNameDic.ContainsKey(fieldName))
        {
            buffState = GetBuffStateByNameDic[fieldName](iBaseState);
            return true;
        }
        buffState = default(BuffState);
        return false;
    }

    /// <summary>
    /// 当玩家属性发生变化时的回调
    /// </summary>
    /// <param name="iAttributeState"></param>
    /// <param name="fieldName"></param>
    private void IAttributeState_Callback(IAttributeState iAttributeState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAttributeState, float>(temp => temp.HP))||
            string.Equals(fieldName, GameState.Instance.GetFieldName<IAttributeState, float>(temp => temp.MaxHP)))
        {
            ChangeHPShow(iAttributeState);
        }
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAttributeState, float>(temp => temp.Mana)) ||
           string.Equals(fieldName, GameState.Instance.GetFieldName<IAttributeState, float>(temp => temp.MaxMana)))
        {
            ChangeMPShow(iAttributeState);
        }
    }

    /// <summary>
    /// 改变血条
    /// </summary>
    /// <param name="iAttributeState"></param>
    private void ChangeHPShow(IAttributeState iAttributeState)
    {
        float bili = iAttributeState.HP / iAttributeState.MaxHP;
        bili = Mathf.Clamp(bili, 0, 1);
        hpValueImage.fillAmount = bili;
        hpValueText.text = (int)iAttributeState.HP + "/" + (int)iAttributeState.MaxHP;
    }

    /// <summary>
    /// 改变魔法条
    /// </summary>
    /// <param name="iAttributeState"></param>
    private void ChangeMPShow(IAttributeState iAttributeState)
    {
        float bili = iAttributeState.Mana / iAttributeState.MaxMana;
        bili = Mathf.Clamp(bili, 0, 1);
        mpValueImage.fillAmount = bili;
        mpValueText.text = (int)iAttributeState.Mana + "/" + (int)iAttributeState.MaxMana;
    }
}

/// <summary>
/// 用于挂载在显示玩家当前buff debuff 特殊状态的图标对象上
/// </summary>
public class UIPlayerState_State : MonoBehaviour
{
    /// <summary>
    /// 用于显示剩余时间的文本(单位是秒)
    /// </summary>
    Text timeText;
    /// <summary>
    /// 显示状态概述(细节)的文本
    /// </summary>
    Text explaneText;
    /// <summary>
    /// 状态图标
    /// </summary>
    Image stateImage;

    /// <summary>
    /// 该图标表示的状态
    /// </summary>
    EnumStatusEffect statusEffect;

    /// <summary>
    /// 状态数据对象
    /// </summary>
    StatusData statusData;

    private void Awake()
    {
        statusData = DataCenter.Instance.GetMetaData<StatusData>();
        stateImage = GetComponent<Image>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTrans = transform.GetChild(i);
            if (childTrans.name.Equals("Time"))
            {
                timeText = childTrans.GetComponent<Text>();
            }
            else if (childTrans.name.Equals("Explane"))
            {
                explaneText = childTrans.GetComponent<Text>();
            }
        }
        //添加事件
        if (explaneText != null)
        {
            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry entry_Enter = new EventTrigger.Entry();
            entry_Enter.eventID = EventTriggerType.PointerEnter;
            entry_Enter.callback = new EventTrigger.TriggerEvent();
            entry_Enter.callback.AddListener(temp => explaneText.gameObject.SetActive(true));
            eventTrigger.triggers.Add(entry_Enter);

            EventTrigger.Entry entry_Exit = new EventTrigger.Entry();
            entry_Exit.eventID = EventTriggerType.PointerExit;
            entry_Exit.callback = new EventTrigger.TriggerEvent();
            entry_Exit.callback.AddListener(temp => explaneText.gameObject.SetActive(false));
            eventTrigger.triggers.Add(entry_Exit);
        }
    }

    /// <summary>
    /// 设置状态
    /// 返回设置状态后游戏对象的显示是否发生变化
    /// </summary>
    /// <param name="buffState"></param>
    /// <returns></returns>
    public bool SetState(BuffState buffState)
    {
        if (buffState.statusEffect != statusEffect)//重新设置图标
        {
            statusEffect = buffState.statusEffect;
            StatusDataInfo statusDataInfo = statusData[buffState.statusEffect];
            if (statusDataInfo != null)
            {
                statusDataInfo.Load();
                stateImage.sprite = statusDataInfo.StatusSprite;
                explaneText.text = statusDataInfo.StatusExplane + "(" + statusDataInfo[buffState.level].LevelExplane + ")";
            }
            else
            {
                stateImage.sprite = null;
                explaneText.text = "";
            }
        }
        if (buffState.Time > 0)
        {
            int shi = (int)buffState.Time / 3600;
            int fen = ((int)buffState.Time % 3600) / 60;
            int miao = ((int)buffState.Time % 3600) % 60;
            timeText.text = shi > 0 ? (shi + ":" + fen + ":" + miao) :
                (fen > 0 ? (fen + ":" + miao) : miao.ToString());
        }
        bool lastActive = gameObject.activeSelf;
        gameObject.SetActive(buffState.Time > 0);
        return lastActive == (buffState.Time > 0);
    }



}