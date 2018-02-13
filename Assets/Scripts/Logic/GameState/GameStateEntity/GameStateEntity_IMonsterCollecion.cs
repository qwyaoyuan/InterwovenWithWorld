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
    /// 更新间隔时间
    /// </summary>
    public const float UpdateTime = 3;
    /// <summary>
    /// 最远的区域离开距离
    /// </summary>
    public const float MaxRangeLeaveDistance = 50;

    /// <summary>
    /// 本场景中的可以检测的怪物集合
    /// </summary>
    List<GameObject> thisSceneCanCheckMonsterObjList;

    /// <summary>
    /// 本场景中的可以检测的怪物字典
    /// </summary>
    Dictionary<MonsterDataInfo, List<GameObject>> thisSceneCanCheckMonsterObjDic;

    /// <summary>
    /// 当前所有的怪物字典
    /// </summary>
    Dictionary<MonsterDataInfo, List<GameObject>> allMonsterObjDic;

    /// <summary>
    /// 该场景的怪物信息数组
    /// </summary>
    MonsterDataInfo[] monsterDataInfos;

    /// <summary>
    /// 运行任务结构(检测怪物)
    /// </summary>
    RunTaskStruct runTaskStruct_CheckMonster;

    /// <summary>
    /// 怪物集合的开始方法
    /// 用于注册场景切换
    /// </summary>
    partial void Start_IMonsterCollection()
    {
        thisSceneCanCheckMonsterObjList = new List<GameObject>();
        allMonsterObjDic = new Dictionary<MonsterDataInfo, List<GameObject>>();
        runTaskStruct_CheckMonster = TaskTools.Instance.GetRunTaskStruct();
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
            if (allMonsterObjDic != null)
            {
                foreach (KeyValuePair<MonsterDataInfo, List<GameObject>> item in allMonsterObjDic)
                {
                    if (item.Value != null)
                    {
                        foreach (GameObject obj in item.Value)
                        {
                            try
                            {
                                GameObject.Destroy(obj);
                            }
                            catch { }
                        }
                    }
                }
            }
            allMonsterObjDic = new Dictionary<MonsterDataInfo, List<GameObject>>();
            thisSceneCanCheckMonsterObjDic = new Dictionary<MonsterDataInfo, List<GameObject>>();
            thisSceneCanCheckMonsterObjList = new List<GameObject>();
            //加载该场景的怪物配置
            MonsterData monsterData = DataCenter.Instance.GetMetaData<MonsterData>();
            monsterDataInfos = monsterData.GetMonsterDataInfos(iGameState.SceneName);
            //创建检测
            if (monsterDataInfos != null)
            {
                runTaskStruct_CheckMonster.InitTask();
                runTaskStruct_CheckMonster.SetSpeed(1);
                runTaskStruct_CheckMonster.StartTask(UpdateTime, CheckMonsterDataInfoState, 0, true);
            }
        }
    }

    /// <summary>
    /// 检测怪物数据的状态
    /// 和玩家位置进行对比,用于创建移动或设置怪物AI
    /// </summary>
    private void CheckMonsterDataInfoState()
    {
        if (monsterDataInfos == null || thisSceneCanCheckMonsterObjDic == null || allMonsterObjDic == null || thisSceneCanCheckMonsterObjList == null)
            return;
        IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        if (iPlayerState.PlayerObj == null)
            return;
        //判断大区域
        bool objListChanged = false;//对象集合是否发生了变化

        //检测触发区域是否会发生变化
        foreach (MonsterDataInfo monsterDataInfo in monsterDataInfos)
        {
            float distance = Vector2.Distance(
                new Vector2(monsterDataInfo.Center.x, monsterDataInfo.Center.z)
                , new Vector2(iPlayerState.PlayerObj.transform.position.x, iPlayerState.PlayerObj.transform.position.z));
            if (distance > monsterDataInfo.Range + MaxRangeLeaveDistance)
            {
                if (thisSceneCanCheckMonsterObjDic.ContainsKey(monsterDataInfo))
                {
                    thisSceneCanCheckMonsterObjDic.Remove(monsterDataInfo);
                    objListChanged = true;
                }
            }
            else if (distance <= monsterDataInfo.Range)
            {
                if (!thisSceneCanCheckMonsterObjDic.ContainsKey(monsterDataInfo))
                {
                    if (!allMonsterObjDic.ContainsKey(monsterDataInfo))
                        allMonsterObjDic.Add(monsterDataInfo, new List<GameObject>());
                    thisSceneCanCheckMonsterObjDic.Add(monsterDataInfo, allMonsterObjDic[monsterDataInfo]);
                    objListChanged = true;
                }
            }
        }

        //移除所有对象字典空元素
        foreach (KeyValuePair<MonsterDataInfo, List<GameObject>> tempData in allMonsterObjDic)
        {
            tempData.Value.RemoveAll(temp => temp == null);
        }

        //检测触发区域内的对象生成情况
        foreach (KeyValuePair<MonsterDataInfo, List<GameObject>> tempData in thisSceneCanCheckMonsterObjDic)
        {
            MonsterDataInfo monsterDataInfo = tempData.Key;
            switch (monsterDataInfo.AIType)
            {
                case EnumMonsterAIType.Trigger://触发型(更新时间到了并且在触发区域被且没有任何怪物时生成)
                    MonsterAIData_Trigger monsterAIData_Trigger = monsterDataInfo.AIData as MonsterAIData_Trigger;
                    if (monsterAIData_Trigger != null && tempData.Value.Count == 0 && monsterAIData_Trigger.NowUpdateTime <= 0)
                    {
                        monsterAIData_Trigger.NowUpdateTime = monsterAIData_Trigger.UpdateTime;
                        float distance = Vector2.Distance(
                            new Vector2(monsterDataInfo.Center.x, monsterDataInfo.Center.z)
                            , new Vector2(iPlayerState.PlayerObj.transform.position.x, iPlayerState.PlayerObj.transform.position.z));
                        if (distance < monsterAIData_Trigger.TriggerRange && monsterDataInfo.MonsterPrefab != null)
                        {
                            //生成怪物
                            for (int i = 0; i < monsterAIData_Trigger.Count; i++)
                            {
                                GameObject createObj = GameObject.Instantiate<GameObject>(monsterDataInfo.MonsterPrefab);
                                MonsterControl monsterControl = createObj.AddComponent<MonsterControl>();
                                monsterControl.SameGroupObjList = tempData.Value;
                                monsterControl.monsterDataInfo = tempData.Key;
                                tempData.Value.Add(createObj);
                                //设置位置(MonsterControl内部会进行y轴的设置)
                                createObj.transform.position = new Vector3(
                                    UnityEngine.Random.Range(monsterDataInfo.Center.x - monsterAIData_Trigger.TriggerRange, monsterDataInfo.Center.x + monsterAIData_Trigger.TriggerRange),
                                    monsterDataInfo.Center.y,
                                    UnityEngine.Random.Range(monsterDataInfo.Center.z - monsterAIData_Trigger.TriggerRange, monsterDataInfo.Center.z + monsterAIData_Trigger.TriggerRange));
                            }
                            objListChanged = true;
                        }
                    }
                    break;
                case EnumMonsterAIType.GoOnPatrol://巡逻型(更新时间到了并且怪物集合数量少与最大数量时生成)
                    MonsterAIData_GoOnPatrol monsterAIData_GoOnPatrol = monsterDataInfo.AIData as MonsterAIData_GoOnPatrol;
                    if (monsterAIData_GoOnPatrol != null && tempData.Value.Count < monsterAIData_GoOnPatrol.Count && monsterAIData_GoOnPatrol.NowUpdateTime <= 0)
                    {
                        monsterAIData_GoOnPatrol.NowUpdateTime = monsterAIData_GoOnPatrol.UpdateTime;
                        //生成怪物
                        int createCount = monsterAIData_GoOnPatrol.Count - tempData.Value.Count;
                        for (int i = 0; i < createCount; i++)
                        {
                            GameObject createObj = GameObject.Instantiate<GameObject>(monsterDataInfo.MonsterPrefab);
                            MonsterControl monsterControl = createObj.AddComponent<MonsterControl>();
                            monsterControl.SameGroupObjList = tempData.Value;
                            monsterControl.monsterDataInfo = tempData.Key;
                            tempData.Value.Add(createObj);
                            //设置位置(MonsterControl内部会进行y轴的设置)
                            createObj.transform.position = new Vector3(
                                UnityEngine.Random.Range(monsterDataInfo.Center.x - tempData.Key.Range, monsterDataInfo.Center.x + tempData.Key.Range),
                                monsterDataInfo.Center.y,
                                UnityEngine.Random.Range(monsterDataInfo.Center.z - tempData.Key.Range, monsterDataInfo.Center.z + tempData.Key.Range));
                        }
                        objListChanged = true;
                    }
                    break;
                case EnumMonsterAIType.Boss://boss型(更新时间到了并且怪物没有生成是生成)
                    MonsterAIData_Boss monsterAIData_Boss = monsterDataInfo.AIData as MonsterAIData_Boss;
                    if (monsterAIData_Boss != null && tempData.Value.Count == 0 && monsterAIData_Boss.NowUpdateTime <= 0)
                    {
                        monsterAIData_Boss.NowUpdateTime = monsterAIData_Boss.UpdateTime;
                        //生成怪物
                        GameObject createObj = GameObject.Instantiate<GameObject>(monsterDataInfo.MonsterPrefab);
                        MonsterControl monsterControl = createObj.AddComponent<MonsterControl>();
                        monsterControl.SameGroupObjList = tempData.Value;
                        monsterControl.monsterDataInfo = tempData.Key;
                        tempData.Value.Add(createObj);
                        //设置位置(MonsterControl内部会进行y轴的设置)
                        createObj.transform.position = new Vector3(
                               monsterDataInfo.Center.x,
                               monsterDataInfo.Center.y,
                              monsterDataInfo.Center.z);
                        objListChanged = true;
                    }
                    break;
            }
        }

        //刷新时间
        foreach (KeyValuePair<MonsterDataInfo, List<GameObject>> tempData in allMonsterObjDic)
        {
            switch (tempData.Key.AIType)
            {
                case EnumMonsterAIType.Trigger:
                    if (tempData.Value.Count == 0 && tempData.Key.AIData != null)
                        tempData.Key.AIData.NowUpdateTime -= UpdateTime;
                    break;
                case EnumMonsterAIType.GoOnPatrol:
                    MonsterAIData_GoOnPatrol monsterAIData_GoOnPatrol = tempData.Key.AIData as MonsterAIData_GoOnPatrol;
                    if (monsterAIData_GoOnPatrol != null && tempData.Value.Count < monsterAIData_GoOnPatrol.Count)
                        monsterAIData_GoOnPatrol.NowUpdateTime -= UpdateTime;
                    break;
                case EnumMonsterAIType.Boss:
                    if (tempData.Value.Count == 0 && tempData.Key.AIData != null)
                        tempData.Key.AIData.NowUpdateTime -= UpdateTime;
                    break;
            }
        }

        //重新构建检测集合
        if (objListChanged)
        {
            thisSceneCanCheckMonsterObjList.Clear();
            foreach (KeyValuePair<MonsterDataInfo, List<GameObject>> tempData in thisSceneCanCheckMonsterObjDic)
            {
                thisSceneCanCheckMonsterObjList.AddRange(tempData.Value);
            }
        }

        //移除检测集合的空元素
        thisSceneCanCheckMonsterObjList.RemoveAll(temp => temp == null);
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
        if (thisSceneCanCheckMonsterObjList == null || centerObj == null)
            return null;
        float distancePow = distance * distance;
        GameObject[] distancePathedArray = thisSceneCanCheckMonsterObjList.Where(temp => temp != null && !GameObject.Equals(temp, centerObj))
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
        return Mathf.Pow(arg1.x - arg2.x, 2) + Mathf.Pow(arg1.z - arg2.z, 2);
    }
}

