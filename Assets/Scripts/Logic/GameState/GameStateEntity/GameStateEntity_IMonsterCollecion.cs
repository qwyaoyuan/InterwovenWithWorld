using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了IMapState接口的GameState类的一个分支实体
/// </summary>
public partial class GameState : IMonsterCollection
{
    /// <summary>
    /// 本场景中的怪物集合
    /// </summary>
    List<GameObject> thisSceneMonsterObjList;

    /// <summary>
    /// 怪物集合的开始方法
    /// 用于注册场景切换
    /// </summary>
    partial void Start_IMonsterCollection()
    {
        thisSceneMonsterObjList = new List<GameObject>();
        GameState.Instance.Registor<IGameState>(IGameStateChanged_In_IMonsterCollection);
    }

    /// <summary>
    /// 游戏状态发生变化
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameStateChanged_In_IMonsterCollection(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, string>(temp => temp.SceneName)))//场景名发生变化说明切换了场景
        {
            //移除之前场景中的怪物
            if (thisSceneMonsterObjList != null && thisSceneMonsterObjList.Count > 0)
            {
                foreach (GameObject obj in thisSceneMonsterObjList)
                {
                    try
                    {
                        GameObject.Destroy(obj);
                    }
                    catch { }
                }
            }
            //加载该场景的怪物
            //.........
        }
    }

    /// <summary>
    /// 获取怪物,需要指定查询的中心对象(一般是玩家),查询的角度(一般是玩家的正方向),查询的距离
    /// </summary>
    /// <param name="centerObj">查询的中心对象(一般是玩家)</param>
    /// <param name="angle">查询的角度(一般是玩家的正方向)</param>
    /// <param name="distance">查询的距离</param>
    /// <returns></returns>
    public GameObject[] GetMonsters(GameObject centerObj, float angle, float distance)
    {
        if (thisSceneMonsterObjList == null || centerObj == null)
            return null;
        float distancePow = distance * distance;
        GameObject[] distancePathedArray = thisSceneMonsterObjList.Where(temp => temp != null && temp != centerObj)
            .Select(temp => new { obj = temp, dis = GetDistancePow(temp.transform.position, centerObj.transform.position) })
            .Where(temp => temp.dis < distancePow)
            .OrderBy(temp => temp.dis)
            .Select(temp => temp.obj)
            .ToArray();
        if (angle > 360 || angle < 0)//表示查询所有范围内的
            return distancePathedArray;
        else//表示了查询的角度
        {
            float halfAngle = angle / 2;
            Vector3 tempforward = centerObj.transform.forward;
            Vector2 forward2D = (new Vector2(tempforward.x, tempforward.z)).normalized;
            GameObject[] anglePathedArray = distancePathedArray.Select(temp => new { obj = temp, vec = temp.transform.position - centerObj.transform.position })
                .Where(temp =>
                {
                    Vector2 thisForward2D = (new Vector2(temp.vec.x, temp.vec.z)).normalized;
                    float thisAngle = Vector2.Angle(thisForward2D, forward2D);
                    return thisAngle < halfAngle;
                })
                .Select(temp => temp.obj).ToArray();
            return anglePathedArray;
        }
    }

    /// <summary>
    /// 获取距离,忽略y轴
    /// </summary>
    /// <param name="arg1">第一个位置</param>
    /// <param name="arg2">第二个位置</param>
    /// <returns></returns>
    private float GetDistancePow(Vector3 arg1, Vector3 arg2)
    {
        return Mathf.Pow(arg1.x - arg2.z, 2) + Mathf.Pow(arg1.z - arg2.z, 2);
    }
}

