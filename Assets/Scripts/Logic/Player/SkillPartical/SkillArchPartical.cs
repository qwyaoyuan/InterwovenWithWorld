using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能---弓箭粒子控制
/// </summary>
public class SkillArchPartical : MonoBehaviour
{
    /// <summary>
    /// 跟随时间(受攻击速度影响)
    /// </summary>
    public float followTime;

    /// <summary>
    /// 发射速度(不受攻击速度影响)
    /// </summary>
    public float shotSpeed;

    /// <summary>
    /// 角色属性
    /// </summary>
    public IAttributeState iAttributeState;

    /// <summary>
    /// 攻击速度
    /// </summary>
    private float attackSpeed;

    /// <summary>
    /// 运行时间
    /// </summary>
    private float runTime;

    /// <summary>
    /// 是否销毁了
    /// </summary>
    private bool destory;

    /// <summary>
    /// 父容器
    /// </summary>
    GameObject parentObj;

    private void Start()
    {
        parentObj = gameObject.transform.parent.gameObject;
        if (iAttributeState == null)
        {
            iAttributeState = GameState.Instance.GetEntity<IAttributeState>();
        }
        if (iAttributeState != null)
        {
            attackSpeed = iAttributeState.AttackSpeed;
            iAttributeState.Registor<IAttributeState>(IAttributeStateChanged);
        }
    }

    /// <summary>
    /// 监听属性发生变化,主要是监听速度
    /// </summary>
    /// <param name="iAttribute"></param>
    /// <param name="fieldName"></param>
    private void IAttributeStateChanged(IAttributeState iAttribute, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IAttributeState, float>(temp => temp.AttackSpeed)))
        {
            attackSpeed = iAttributeState.AttackSpeed;
        }
    }

    private void Update()
    {
        float tempRunTime = runTime;
        runTime += Time.deltaTime * attackSpeed;
        if (tempRunTime < followTime && runTime >= followTime)
        {
            //将该对象从父对象中移除
            transform.SetParent(null);
            //发射
            StartCoroutine(Shot());
        }
        if (parentObj == null && !destory)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Shot()
    {
        while (!destory)
        {
            transform.position += transform.forward * shotSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        destory = true;
    }
}
