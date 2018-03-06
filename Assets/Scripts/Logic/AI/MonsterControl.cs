using NodeCanvas.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物控制器
/// </summary>
public class MonsterControl : MonoBehaviour
{
    /// <summary>
    /// 相同组的对象(包括自己)
    /// </summary>
    public List<GameObject> SameGroupObjList;
    /// <summary>
    /// 怪物的数据对象
    /// </summary>
    public MonsterDataInfo monsterDataInfo;

    Blackboard blackboard;

    /// <summary>
    /// 不要初始化
    /// </summary>
#if UNITY_EDITOR
    public bool DontInit;
#endif

    /// <summary>
    /// 销毁携程对象
    /// </summary>
    Coroutine destroyCoroutine;

    /// <summary>
    /// 更新位置到地面并初始化导航网格
    /// </summary>
    public void Start()
    {
#if UNITY_EDITOR
        if (DontInit)
            return;
#endif
        Vector3 rayStart = transform.position + Vector3.up * 100;
        Ray ray = new Ray(rayStart, Vector3.down);
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray);
        RaycastHit rayCastHit = rayCastHits.FirstOrDefault(temp => string.Equals(temp.transform.tag, "Terrain"));
        if (rayCastHit.collider != null)
        {
            transform.position = rayCastHit.point + Vector3.up * monsterDataInfo.Offset;
        }
        NavMeshAgent navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.radius = 0.2f;
        navMeshAgent.height = 1.8f;
        navMeshAgent.baseOffset = 0;
        blackboard = GetComponent<Blackboard>();
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        if (blackboard != null)
        {
            if (monsterDataInfo.MonsterBaseAttribute != null)
                blackboard.SetValue("HP", monsterDataInfo.MonsterBaseAttribute.HP);
            switch (monsterDataInfo.AIType)
            {
                case EnumMonsterAIType.Trigger:
                    blackboard.SetValue("StartPoint", transform.position);
                    blackboard.SetValue("Target", iPlayerState.PlayerObj);
                    break;
                case EnumMonsterAIType.GoOnPatrol:
                    //巡逻点
                    List<Vector3> goOnPatrolList = new List<Vector3>();
                    Vector3 result = Vector3.zero;
                    for (int i = 0; i < 2; i++)
                    {
                        if (GetPointAtTerrainPosition(monsterDataInfo.Center + RandomVector3(monsterDataInfo.Range), out result))
                            goOnPatrolList.Add(result);
                    }
                    blackboard.SetValue("GoOnPatrolPointList", goOnPatrolList);
                    break;
                case EnumMonsterAIType.Boss:
                    blackboard.SetValue("StartPoint", transform.position);
                    //blackboard.SetValue("Target", iPlayerState.PlayerObj);
                    break;
            }
        }

        //给对象添加一个子物体,子物体设置触发(这个不是用来检测攻击的而是用来检测发现目标的)
        SphereCollider thisSphereCollider = GetComponent<SphereCollider>();
        if (thisSphereCollider != null)
        {
            GameObject childCheckObj = new GameObject();
            childCheckObj.transform.SetParent(transform);
            MonsterCheckPlayer monsterCheckPlayer = childCheckObj.AddComponent<MonsterCheckPlayer>();
            monsterCheckPlayer.TriggerEnter = _OnTriggerEnter;
            monsterCheckPlayer.TriggerExit = _OnTriggerExit;
            childCheckObj.transform.localPosition = Vector3.zero;
            SphereCollider childSphereCollider = childCheckObj.AddComponent<SphereCollider>();
            childSphereCollider.radius = thisSphereCollider.radius;
            thisSphereCollider.enabled = false;
            childSphereCollider.isTrigger = true;
        }

        //注册监听
        if (monsterDataInfo != null && monsterDataInfo.MonsterBaseAttribute != null)
        {
            monsterDataInfo.MonsterBaseAttribute.Registor<IAttributeState>(IAttribute_Changed);
        }
    }


    /// <summary>
    /// 监听属性变化
    /// </summary>
    /// <param name="iAttribute"></param>
    /// <param name="fieldName"></param>
    private void IAttribute_Changed(IAttributeState iAttribute, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IAttributeState, float>(temp => temp.HP)))
        {
            if (blackboard != null)
                blackboard.SetValue("HP", iAttribute.HP);
            if (iAttribute.HP <= 0 && destroyCoroutine == null)
            {
                destroyCoroutine = StartCoroutine(DestoryThis());
            }
        }
    }

    /// <summary>
    /// 等待一定时间销毁
    /// </summary>
    /// <returns></returns>
    IEnumerator DestoryThis()
    {
        yield return null;
        //取消注册(其实也可以不用管)
        if (monsterDataInfo != null && monsterDataInfo.MonsterBaseAttribute != null)
        {
            monsterDataInfo.MonsterBaseAttribute.UnRegistor<IAttributeState>(IAttribute_Changed);
        }
        //通知杀死了某种怪物
        INowTaskState iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        if (iNowTaskState != null && monsterDataInfo != null)
        {
            iNowTaskState.CheckNowTask(EnumCheckTaskType.Monster, (int)monsterDataInfo.MonsterType);
        }
        //移除组件
        NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
            GameObject.Destroy(navMeshAgent);
        Rigidbody rigibody = GetComponent<Rigidbody>();
        if (rigibody != null)
            GameObject.Destroy(rigibody);
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider != null)
                GameObject.Destroy(collider);
        }
        if (monsterDataInfo != null)
            yield return new WaitForSeconds(monsterDataInfo.DeathAnimTime);
        else
            yield return new WaitForSeconds(3);
        float animatorTime = 0;
        float maxAnimatorTime = monsterDataInfo != null ? monsterDataInfo.DeathAnimTime : 3;
        while (animatorTime < maxAnimatorTime)
        {
            animatorTime += Time.deltaTime;
            gameObject.transform.position -= Vector3.up * Time.deltaTime * 0.5f;
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }


    /// <summary>
    /// 随机一个vector3
    /// </summary>
    /// <param name="maxRandom">最大的随机值</param>
    /// <returns></returns>
    private Vector3 RandomVector3(float maxRandom)
    {
        return new Vector3(UnityEngine.Random.Range(-maxRandom, maxRandom), UnityEngine.Random.Range(-maxRandom, maxRandom), UnityEngine.Random.Range(-maxRandom, maxRandom));
    }

    /// <summary>
    ///获取点在地形上的投射(返回是否可以直接打通到地面)
    /// </summary>
    /// <param name="source">原始点</param>
    /// <param name="result">返回点</param>
    /// <returns>是否可以直通地面</returns>
    private bool GetPointAtTerrainPosition(Vector3 source, out Vector3 result)
    {
        Vector3 startPoint = source + Vector3.up * 500;
        Ray ray = new Ray(startPoint, Vector3.down);
        RaycastHit _rch = Physics.RaycastAll(ray)
            .Where(temp => string.Equals(temp.transform.tag, "Terrain"))
            .FirstOrDefault();
        result = _rch.point;
        if (_rch.collider == null)
        {
            return false;
        }
        NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(_rch.point, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }

    private void _OnTriggerEnter(Collider other)
    {
        string checkTag = blackboard.GetValue<string>("PlayerTag");
        if (string.Equals(other.tag, checkTag))
        {
            blackboard.SetValue("TempTarget", other.gameObject);
        }
    }

    private void _OnTriggerExit(Collider other)
    {
        string checkTag = blackboard.GetValue<string>("PlayerTag");
        if (string.Equals(other.tag, checkTag))
        {
            blackboard.SetValue("TempTarget", null);
        }
    }


    #region 给外部提供的函数
    /// <summary>
    /// 获取怪物的当前属性 
    /// </summary>
    /// <returns></returns>
    public IAttributeState GetMonsterAttributeState()
    {
        return monsterDataInfo.MonsterBaseAttribute.Clone();
    }

    /// <summary>
    /// 给与攻击(用于被攻击反馈)
    /// </summary>
    public void GiveHit()
    {
        //暂时为所有时刻都处于可以被攻击状态
        if (blackboard != null)
            blackboard.SetValue("GetHit", true);
    }
    #endregion
}

/// <summary>
/// 用于检测触发
/// </summary>
public class MonsterCheckPlayer : MonoBehaviour
{
    public Action<Collider> TriggerEnter;
    public Action<Collider> TriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerEnter != null)
            TriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerExit != null)
            TriggerExit(other);
    }
}
