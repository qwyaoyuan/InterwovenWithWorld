using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 物理技能伤害检测脚本
/// </summary>
public class PhysicSkillInjuryDetection : MonoBehaviour
{
    /// <summary>
    /// 检测范围集合数组
    /// </summary>
    [SerializeField]
    [Tooltip("检测范围集合数组")]
    CheckStructCollection[] checkStructCollectionArray;
    /// <summary>
    /// 攻击检测的层,如果内部设置则使用内部的
    /// </summary>
    [SerializeField]
    [Tooltip("攻击检测的层,如果内部设置则使用内部的")]
    LayerMask attackLayerMask;

    #region 用于检测的临时变量
    /// <summary>
    /// 根据攻击速度来驱动检测时间
    /// 外面设置的检测时间是在正常的速度下设置的
    /// </summary>
    float AttackSpeed = 1;
    /// <summary>
    /// 检测到后的回调函数
    /// </summary>
    Action<int, GameObject> CheckResultAction;
    /// <summary>
    /// 临时的检测层对象
    /// </summary>
    LayerMask? tempAttackLayerMask;
    /// <summary>
    /// 检测所示使用的结构
    /// </summary>
    CheckStructCollection checkStructCollection;
    /// <summary>
    /// 技能内部次序对应检测到的对象集合
    /// </summary>
    Dictionary<int, List<GameObject>> skillOrderToObjList;
    /// <summary>
    /// 当前的检测时间
    /// </summary>
    float nowCheckTime;
    /// <summary>
    /// 攻击的检测对象缓存
    /// </summary>
    RaycastHit[] attackRaycastHitArray;
    #endregion

    private void Awake()
    {
        attackRaycastHitArray = new RaycastHit[20];
    }

    private void Update()
    {
        if (checkStructCollection != null && skillOrderToObjList != null && CheckResultAction != null)//开始检测
        {
            nowCheckTime += Time.deltaTime * AttackSpeed;//根据攻击速度乘以对应的时间
            //取出复合当前时间范围的检测
            CheckStruct[] checkStructArray = checkStructCollection.CheckStructs.Where(temp => temp.StartCheckTime < nowCheckTime && (temp.StartCheckTime + temp.DurationCheck > nowCheckTime)).ToArray();
            foreach (CheckStruct checkStruct in checkStructArray)
            {
                if (!skillOrderToObjList.ContainsKey(checkStruct.InnerDamageOrder))
                    skillOrderToObjList.Add(checkStruct.InnerDamageOrder, new List<GameObject>());
                Transform colliderTrans = checkStruct.Collider.transform;
                BoxCollider boxCollider = checkStruct.Collider;
                LayerMask layerMask = tempAttackLayerMask != null ? tempAttackLayerMask.Value : attackLayerMask;
                int count = Physics.BoxCastNonAlloc(
                    colliderTrans.TransformPoint(boxCollider.center),
                    colliderTrans.TransformVector(boxCollider.size),
                    colliderTrans.TransformDirection(colliderTrans.forward),
                    attackRaycastHitArray,
                    colliderTrans.rotation,
                    0,
                    layerMask,
                    QueryTriggerInteraction.Collide);
                for (int i = 0; i < count; i++)
                {
                    RaycastHit rch = attackRaycastHitArray[i];
                    attackRaycastHitArray[i] = default(RaycastHit);
                    if (!skillOrderToObjList[checkStruct.InnerDamageOrder].Contains(rch.collider.gameObject))
                    {
                        skillOrderToObjList[checkStruct.InnerDamageOrder].Add(rch.collider.gameObject);
                        CheckResultAction(checkStruct.InnerDamageOrder, rch.collider.gameObject);
                    }
                }
            }
            //判断之后是否还有检测的东西
            int canCheckCount = checkStructCollection.CheckStructs.Count(temp => temp.StartCheckTime + temp.DurationCheck > nowCheckTime);
            if (canCheckCount <= 0)//如果都检测完了则初始化
            {
                checkStructCollection = null;
                nowCheckTime = 0;
                skillOrderToObjList = null;
                AttackSpeed = 0;
                CheckResultAction = null;
                tempAttackLayerMask = null;
            }
        }
    }

    /// <summary>
    /// 检测攻击
    /// </summary>
    /// <param name="skillType">技能类型</param>
    /// <param name="attackSpeed">攻击速度</param>
    /// <param name="attackLayerMask">内部设置的检测层</param>
    /// <param name="CheckResultAction">检测到碰撞对象后的回调</param>
    public void CheckAttack(EnumSkillType skillType, float attackSpeed, LayerMask? attackLayerMask, Action<int, GameObject> CheckResultAction)
    {
        this.CheckResultAction = CheckResultAction;
        this.AttackSpeed = attackSpeed;
        if (attackLayerMask != null)
            tempAttackLayerMask = attackLayerMask.Value;
        else
            tempAttackLayerMask = null;
        if (checkStructCollectionArray != null)
            checkStructCollection = checkStructCollectionArray.Where(temp => temp.SkillType == skillType).FirstOrDefault();
        else
            checkStructCollection = null;
        nowCheckTime = 0;
        skillOrderToObjList = new Dictionary<int, List<GameObject>>();
    }

    /// <summary>
    /// 技能的检测范围集合结构
    /// </summary>
    [Serializable]
    public class CheckStructCollection
    {
        /// <summary>
        /// 该技能的类型
        /// </summary>
        [Tooltip("该技能的类型")]
        public EnumSkillType SkillType;

        /// <summary>
        /// 检测范围数组
        /// </summary>
        [Tooltip("检测范围数组")]
        public CheckStruct[] CheckStructs;
    }


    /// <summary>
    /// 技能的检测范围结构
    /// </summary>
    [Serializable]
    public class CheckStruct
    {
        /// <summary>
        /// 该技能内部伤害的顺序(多段伤害情况)
        /// </summary>
        [Tooltip("该技能内部伤害的顺序(多段伤害情况)")]
        public int InnerDamageOrder;
        /// <summary>
        /// 该检测的检测的触发对象
        /// </summary>
        [Tooltip("该检测的检测的触发对象")]
        public BoxCollider Collider;
        /// <summary>
        /// 该检测的检测时间(技能释放开始从零计时)
        /// </summary>
        [Tooltip("该检测的检测时间(技能释放开始从零计时)")]
        public float StartCheckTime;
        /// <summary>
        /// 持续检测的时间
        /// </summary>
        [Tooltip("持续检测的时间")]
        public float DurationCheck;
    }
}

