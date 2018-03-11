using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示怪物血条
/// </summary>
public class UIShowMonsterHP : MonoBehaviour
{
    /// <summary>
    /// ui的整体对象
    /// </summary>
    [SerializeField]
    private GameObject UIObj;
    /// <summary>
    ///  显示名字的文本对象
    /// </summary>
    [SerializeField]
    private Text NameText;
    /// <summary>
    /// 显示生命条进度的图像对象
    /// </summary>
    [SerializeField]
    private Image LifeImage;
    /// <summary>
    /// 显示buff的父窗体对象
    /// </summary>
    [SerializeField]
    private RectTransform BuffParentRectrans;
    /// <summary>
    /// buff的例子预设体
    /// 最外层显示buff图片,内部一个Image对象显示时间进度
    /// </summary>
    [SerializeField]
    private GameObject BuffPrefab;

    IGameState iGameState;

    IPlayerState iPlayerState;

    /// <summary>
    /// 显示buff的结构
    /// </summary>
    private List<BuffShowStruct> buffShowStructList;

    /// <summary>
    /// 目标
    /// </summary>
    private MonsterHPUIStruct target;

    private void Start()
    {
        buffShowStructList = new List<BuffShowStruct>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        GameState.Instance.Registor<IGameState>(IGameState_Changed);
        UIObj.SetActive(false);
    }

    private void OnDisable()
    {
        GameState.Instance.UnRegistor<IGameState>(IGameState_Changed);
    }

    private void Update()
    {
        if (target.monsterObj != null && iPlayerState != null && iPlayerState.PlayerObj != null)
        {
            if (target.nowHP <= 0 || Vector3.Distance(target.monsterObj.transform.position, iPlayerState.PlayerObj.transform.position) > 50)
            {         
                //重置
                foreach (BuffShowStruct buffShowStruct in buffShowStructList)
                {
                    GameObject.Destroy(buffShowStruct.buffTarget);
                }
                buffShowStructList.Clear();
                target = default(MonsterHPUIStruct);
                //隐藏 
                UIObj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 游戏状态发生变化,主要用于检测怪物HP变化
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameState_Changed(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IGameState, MonsterHPUIStruct>(temp => temp.ShowMonsterHP)))
        {
            //处理异常状态
            if (target.monsterObj != null && target.monsterObj == iGameState.ShowMonsterHP.monsterObj)//如果相同只需要查看buff就行了(暂时不处理buff)
            {
                target = iGameState.ShowMonsterHP;
                if (target.statusDatas != null)
                {
                    foreach (var statusData in target.statusDatas)
                    {
                        if (statusData == null)
                            continue;
                        if (buffShowStructList.Count(temp => temp.statusData.EffectType == statusData.EffectType) > 0)
                            continue;
                        AddNewStatus(statusData);
                    }
                }
            }
            else
            {
                target = iGameState.ShowMonsterHP;
                //重置
                foreach (BuffShowStruct buffShowStruct in buffShowStructList)
                {
                    GameObject.Destroy(buffShowStruct.buffTarget);
                }
                buffShowStructList.Clear();
                //添加
                if (target.statusDatas != null)
                {
                    foreach (var statusData in target.statusDatas)
                    {
                        AddNewStatus(statusData);
                    }
                }
            }
            //处理其他 
            NameText.text = target.monsterName;
            LifeImage.fillAmount = target.nowHP / target.maxHP;

            UIObj.SetActive(true);
        }
    }

    /// <summary>
    /// 添加新的异常状态(其实这么写是有问题的,因为使用的是会变化的本次攻击的数据,而不是对象身上的常量数据 )
    /// </summary>
    /// <param name="statusData"></param>
    private void AddNewStatus(StatusDataInfo.StatusLevelDataInfo statusData)
    {
        //不存在测创建
        GameObject createBuffStateObj = GameObject.Instantiate<GameObject>(BuffPrefab);
        Transform childTrans = createBuffStateObj.transform.GetChild(0);
        Image timeImage = childTrans.GetComponent<Image>();
        float usedMana = Mathf.Clamp(target.thisUsedMana, statusData.MinMana, statusData.MaxMana);
        buffShowStructList.Add(new BuffShowStruct()
        {
            buffTarget = createBuffStateObj,
            maxTime = statusData.DurationCuvre.Evaluate(usedMana),
            nowTime = 0,
            statusData = statusData,
            timeImge = timeImage
        });
    }

    /// <summary>
    ///显示buff的结构
    /// </summary>
    public class BuffShowStruct
    {
        /// <summary>
        /// buff的目标
        /// </summary>
        public GameObject buffTarget;
        /// <summary>
        /// 用于处理时间的Image
        /// </summary>
        public Image timeImge;
        /// <summary>
        /// 异常数据
        /// </summary>
        public StatusDataInfo.StatusLevelDataInfo statusData;
        /// <summary>
        /// 初始持续时间
        /// </summary>
        public float maxTime;
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float nowTime;
    }
}

