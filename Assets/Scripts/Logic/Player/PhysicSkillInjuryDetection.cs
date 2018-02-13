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
    Func<int, GameObject,bool> CheckResultAction;
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
            bool hasNextCheck = true;//是否存在下次一的检测
            float lastCheckTime = nowCheckTime;
            nowCheckTime += Time.deltaTime * AttackSpeed;//根据攻击速度乘以对应的时间
            //当时间到时生成粒子
            SoildSkillPartical[] soildSkillParticals = checkStructCollection.SoildSkillParticals.Where(temp => temp.StartParticalTime > lastCheckTime && temp.StartParticalTime < nowCheckTime).ToArray();
            foreach (SoildSkillPartical soildSkillPartical in soildSkillParticals)
            {
                if (soildSkillPartical.ParticalPrefab == null)
                    continue;
                GameObject createPartical = GameObject.Instantiate<GameObject>(soildSkillPartical.ParticalPrefab);
                if (soildSkillPartical.ShowTarget != null)
                {
                    switch (soildSkillPartical.ShowType)
                    {
                        case SoildSkillPartical.EnumShowType.Soild:
                            createPartical.transform.position = transform.position;
                            createPartical.transform.forward = transform.forward;
                            break;
                        case SoildSkillPartical.EnumShowType.Follow:
                            createPartical.transform.SetParent(soildSkillPartical.ShowTarget);
                            createPartical.transform.localPosition = Vector3.zero;
                            createPartical.transform.localEulerAngles = Vector3.zero;
                            break;
                    }
                }
                else
                {
                    createPartical.transform.position = transform.position;
                    createPartical.transform.forward = transform.forward;
                }
                float stillTime = soildSkillPartical.Duration / (AttackSpeed > 0 ? AttackSpeed : 1);
                ParticleSystem particleSystem = createPartical.GetComponent<ParticleSystem>();
                if (particleSystem)
                {
                    ParticleSystem.MainModule mainModule = particleSystem.main;
                    mainModule.duration = stillTime;
                    particleSystem.Play();
                }
                GameObject.Destroy(createPartical, 5);
            }
            //取出复合当前时间范围的检测
            CheckStruct[] checkStructArray = checkStructCollection.CheckStructs.Where(temp => temp.StartCheckTime < nowCheckTime && (temp.StartCheckTime + temp.DurationCheck > nowCheckTime)).ToArray();
            foreach (CheckStruct checkStruct in checkStructArray)
            {
                if (!skillOrderToObjList.ContainsKey(checkStruct.InnerDamageOrder))
                    skillOrderToObjList.Add(checkStruct.InnerDamageOrder, new List<GameObject>());
                PhysicSkillInjuryDetection_Check physicSkillInjuryDetection_Check = checkStruct.Collider.gameObject.GetComponent<PhysicSkillInjuryDetection_Check>();
                if (physicSkillInjuryDetection_Check == null)
                    continue;
                GameObject[] targetObjs = physicSkillInjuryDetection_Check.GetTargets;
                if (targetObjs.Length > 0)
                {
                    foreach (GameObject targetObj in targetObjs)
                    {
                        if (!skillOrderToObjList[checkStruct.InnerDamageOrder].Contains(targetObj))
                        {
                            skillOrderToObjList[checkStruct.InnerDamageOrder].Add(targetObj);
                            hasNextCheck = CheckResultAction(checkStruct.InnerDamageOrder, targetObj);//返回是否存在下一次的检测
                            //生成(命中)粒子
                            //从对象身上获取偏差值数据,暂时为0
                            if (checkStruct.ParticalPrefab != null)
                            {
                                GameObject partaicalObj = GameObject.Instantiate<GameObject>(checkStruct.ParticalPrefab);
                                Destroy(partaicalObj, 5);
                            }
                        }
                    }
                }
            }
            //判断之后是否还有检测的东西
            int canCheckCount = checkStructCollection.CheckStructs.Count(temp => temp.StartCheckTime + temp.DurationCheck > nowCheckTime);
            if (canCheckCount <= 0 || !hasNextCheck)//如果都检测完了则初始化 如果没有下次检测了也要初始化
            {
                checkStructCollection.CheckStructs.ToList().ForEach(temp =>
                {
                    temp.Collider.gameObject.SetActive(false);
                });
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
    /// <param name="WeaponType">武器类型</param>
    /// <param name="attackSpeed">攻击速度</param>
    /// <param name="attackLayerMask">内部设置的检测层</param>
    /// <param name="CheckResultAction">检测到碰撞对象后的回调</param>
    /// <param name="otherCheck">其他的检测</param>
    public void CheckAttack(EnumSkillType skillType, EnumWeaponTypeByPlayerState WeaponType, float attackSpeed, LayerMask? attackLayerMask, Func<int, GameObject,bool> CheckResultAction, int otherCheck = 0)
    {
        this.CheckResultAction = CheckResultAction;
        this.AttackSpeed = attackSpeed;
        if (attackLayerMask != null)
            tempAttackLayerMask = attackLayerMask.Value;
        else
            tempAttackLayerMask = null;
        if (checkStructCollectionArray != null)
        {
            checkStructCollection = checkStructCollectionArray
                .Where(temp => temp.SkillType == skillType)//判断技能类型
                .Where(temp => temp.WeaponType == WeaponType || temp.WeaponType == EnumWeaponTypeByPlayerState.None)//判断武器类型(如果设置的是None表示什么武器都可以)
                .Where(temp => (temp.OtherCheck == 0 || otherCheck == 0) ? true : temp.OtherCheck == otherCheck)//判断是否存在其他的检测,并判断是否通过检测
                .FirstOrDefault();
        }
        else
            checkStructCollection = null;
        if (checkStructCollection != null)
        {
            //进行设置
            checkStructCollection.CheckStructs.ToList().ForEach(temp =>
            {
                temp.Collider.gameObject.SetActive(true);//打开检测对象
                //查找检测脚本
                PhysicSkillInjuryDetection_Check physicSkillInjuryDetection_Check = temp.Collider.gameObject.GetComponent<PhysicSkillInjuryDetection_Check>();
                //如果为空则添加
                if (physicSkillInjuryDetection_Check == null)
                {
                    physicSkillInjuryDetection_Check = temp.Collider.gameObject.AddComponent<PhysicSkillInjuryDetection_Check>();
                }
                //设置检测层
                physicSkillInjuryDetection_Check.checkMask = tempAttackLayerMask != null ? tempAttackLayerMask.Value : this.attackLayerMask;
            });
        }
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
        /// 武器类型
        /// </summary>
        public EnumWeaponTypeByPlayerState WeaponType;

        /// <summary>
        /// 其他位置的检测(注意如果是0则表示不用检测,外部输入0也是同理)
        /// </summary>
        public int OtherCheck;

        /// <summary>
        /// 检测范围数组
        /// </summary>
        [Tooltip("检测范围数组")]
        public CheckStruct[] CheckStructs;

        /// <summary>
        /// 固定的粒子对象
        /// </summary>
        [Tooltip("固定的粒子对象")]
        public SoildSkillPartical[] SoildSkillParticals;
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
        /// <summary>
        /// 粒子预设提(命中)
        /// </summary>
        [Tooltip("粒子预设提(命中)")]
        public GameObject ParticalPrefab;
    }

    /// <summary>
    /// 固定的粒子对象
    /// </summary>
    [Serializable]
    public class SoildSkillPartical
    {
        /// <summary>
        /// 该技能粒子的开始时间(技能释放开始从零计时)
        /// </summary>
        [Tooltip("该技能粒子的开始时间(技能释放开始从零计时)")]
        public float StartParticalTime;
        /// <summary>
        /// 该技能粒子的时间
        /// </summary>
        [Tooltip("该技能粒子的持续时间")]
        public float Duration;
        /// <summary>
        /// 粒子预设提
        /// </summary>
        [Tooltip("粒子预设提")]
        public GameObject ParticalPrefab;
        /// <summary>
        /// 粒子的显示方式
        /// </summary>
        [Tooltip("粒子的显示方式")]
        public EnumShowType ShowType;
        /// <summary>
        /// 目标
        /// \r\n
        /// 如果选择Follow则设置子物体跟随,如果选择Soild则使用一次该位置,如果为null则使用本脚本的位置
        /// </summary>
        [Tooltip("如果选择Follow则设置子物体跟随,如果选择Soild则使用一次该位置,如果为null则使用本脚本的位置")]
        public Transform ShowTarget;

        /// <summary>
        /// 粒子的显示方式枚举
        /// </summary>
        public enum EnumShowType
        {
            /// <summary>
            /// 固定
            /// </summary>
            Soild,
            /// <summary>
            /// 跟随
            /// </summary>
            Follow
        }
    }

    /// <summary>
    /// 用于检测的子脚本
    /// </summary>
    public class PhysicSkillInjuryDetection_Check : MonoBehaviour
    {
        /// <summary>
        /// 检测层
        /// </summary>
        public LayerMask checkMask;

        /// <summary>
        /// 对象集合
        /// </summary>
        List<GameObject> targetList;
        /// <summary>
        /// 是否取出了数据
        /// </summary>
        bool popuped;

        /// <summary>
        /// 获取目标
        /// </summary>
        public GameObject[] GetTargets
        {
            get
            {
                if (targetList == null)
                    return new GameObject[0];
                popuped = true;
                return targetList.ToArray();
            }
        }

        private void Awake()
        {
            targetList = new List<GameObject>();
        }

        private void OnEnable()
        {
            if (targetList != null)
                targetList.Clear();
        }

        private void OnDisable()
        {
            if (targetList != null)
                targetList.Clear();
        }

        private void LateUpdate()
        {
            if (targetList != null)
                if (popuped)
                    targetList.Clear();
        }

        private void OnTriggerStay(Collider other)
        {
            int layer = (int)Mathf.Pow(2, other.gameObject.layer);
            if (checkMask.value == (layer | checkMask.value))
                if (targetList != null)
                {
                    if (!targetList.Contains(other.gameObject))
                        targetList.Add(other.gameObject);
                }
        }
    }
}

