using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 实现了IPlayerState->IDamage接口的GameState类的一个分支实体
/// </summary>
public partial class GameState
{
    /// <summary>
    /// 根据技能获取粒子的初始化数据
    /// </summary>
    /// <param name="playerObj">对象</param>
    /// <param name="skills">技能数组</param>
    /// <returns></returns>
    public ParticalInitParamData[] GetParticalInitParamData(GameObject playerObj, params SkillBaseStruct[] skills)
    {
        List<ParticalInitParamData> resultList = new List<ParticalInitParamData>();
        ParticalInitParamData particalInitParamData = default(ParticalInitParamData);
        //这三个是基础数据
        particalInitParamData.lifeTime = 5;
        particalInitParamData.checkCollisionIntervalTime = 1;
        particalInitParamData.targetObjs = new GameObject[0];

        //下面的是变化数据
        //颜色
        SkillBaseStruct combine_secondSkill = skills.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel2Start && temp.skillType < EnumSkillType.MagicCombinedLevel2End);
        if (combine_secondSkill != null)
        {
            switch (combine_secondSkill.skillType)
            {
                case EnumSkillType.YSX01://火元素
                    particalInitParamData.color = Color.red;
                    break;
                case EnumSkillType.YSX02://水元素
                    particalInitParamData.color = Color.blue;
                    break;
                case EnumSkillType.YSX03://土元素
                    particalInitParamData.color = Color.yellow;
                    break;
                case EnumSkillType.YSX04://风元素
                    particalInitParamData.color = Color.green;
                    break;
                case EnumSkillType.SM06://冰元素
                    particalInitParamData.color = Color.cyan;
                    break;
                case EnumSkillType.SM07://雷元素
                    particalInitParamData.color = new Color(0.5f, 0, 0.5f, 1);
                    break;
                case EnumSkillType.DSM03://光明元素
                    particalInitParamData.color = Color.white;
                    break;
                case EnumSkillType.DSM04://黑暗元素
                    particalInitParamData.color = Color.black;
                    break;
            }
        }
        else
            particalInitParamData.color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
        //技能最基础表现形式
        SkillBaseStruct combine_firstSkill = skills.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel1Start && temp.skillType < EnumSkillType.MagicCombinedLevel1End);
        IMonsterCollection iMonsterCollection = GameState.Instance.GetEntity<IMonsterCollection>();
        GameObject selectTargetObj = null;//魔力导向选中的对象
        if (combine_firstSkill != null)
        {
            switch (combine_secondSkill.skillType)
            {
                case EnumSkillType.FS01://奥数弹
                    particalInitParamData.range = 20;//表示距离
                    break;
                case EnumSkillType.FS02://奥数震荡
                    particalInitParamData.range = 1;//表示比例
                    break;
                case EnumSkillType.FS03://魔力屏障
                    particalInitParamData.range = 1;//表示比例
                    break;
                case EnumSkillType.FS04://魔力导向
                    //查找前方45度方位内距离自己最近的怪物
                    GameObject[] selectObjs = iMonsterCollection.GetMonsters(playerObj, 45, 20);
                    if (selectObjs != null && selectObjs.Length > 0)
                        selectTargetObj = selectObjs[0];
                    if (selectTargetObj)
                        particalInitParamData.range = Vector3.Distance(selectTargetObj.transform.position, playerObj.transform.position);//表示距离
                    else
                        particalInitParamData.range = 10;
                    break;
                case EnumSkillType.MFS05://魔力脉冲
                    particalInitParamData.range = 1;//表示比例
                    break;
            }
        }
        resultList.Add(particalInitParamData);
        //第三阶段的连续魔力导向有点特殊
        SkillBaseStruct combine_thirdSkill = skills.FirstOrDefault(temp => temp.skillType > EnumSkillType.MagicCombinedLevel3Start && temp.skillType < EnumSkillType.MagicCombinedLevel3End);
        if (combine_thirdSkill != null && combine_thirdSkill.skillType == EnumSkillType.MFS06)
        {
            //查找周围距离查找到的怪物的最近的怪物
            if (selectTargetObj)
            {
                GameObject[] nextObjs = iMonsterCollection.GetMonsters(playerObj, -1, 10);
                Queue<GameObject> queueNextObj = new Queue<GameObject>();
                queueNextObj.Enqueue(playerObj);
                foreach (var item in nextObjs)
                {
                    queueNextObj.Enqueue(item);
                }
                while (queueNextObj.Count > 1)
                {
                    GameObject firstObj = queueNextObj.Dequeue();//第一个怪物
                    GameObject secondObj = queueNextObj.Peek();//第二个怪物
                    ParticalInitParamData temp_particalInitParamData = particalInitParamData;
                    temp_particalInitParamData.position = firstObj.transform.position + Vector3.up;
                    temp_particalInitParamData.forward = (secondObj.transform.position - firstObj.transform.position).normalized;
                    temp_particalInitParamData.range = Vector3.Distance(firstObj.transform.position, secondObj.transform.position);
                    resultList.Add(temp_particalInitParamData);
                }
            }
        }

        return resultList.ToArray();
    }
}

