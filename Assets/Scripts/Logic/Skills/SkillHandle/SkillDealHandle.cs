using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能处理类
/// </summary>
public class SkillDealHandle : MonoBehaviour, IEntrance
{
    /// <summary>
    /// 技能处理类的单例对象
    /// </summary>
    public static SkillDealHandle Instance;

    /// <summary>
    /// 运行任务对象
    /// </summary>
    RunTaskStruct runTaskStruct;

    /// <summary>
    /// 此时是否可以解决技能
    /// </summary>
    public bool CanDealSkill
    {
        get { return false; }
    }

    public void Start()
    {
        Instance = this;
        runTaskStruct = TaskTools.Instance.GetRunTaskStruct();
    }

    public void Update()
    {
        //判断是否处于被控制状态，如果是则重置任务
        //--------------------------
        if (false)
        {
            runTaskStruct.InitTask();
            this.skillBaseStructs = null;
            this.readTheArticleOver = false;
            this.loosenTheReleaseKey = false;
            this.savingMagicPowerTime = 0;
        }
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// 接下来要释放的技能
    /// </summary>
    SkillBaseStruct[] skillBaseStructs;
    /// <summary>
    /// 读条是否结束
    /// </summary>
    bool readTheArticleOver;
    /// <summary>
    /// 是否松开释放键
    /// </summary>
    bool loosenTheReleaseKey;
    /// <summary>
    /// 魔力储蓄时间
    /// </summary>
    float savingMagicPowerTime;
    /// <summary>
    /// 处理组合技能(开始)，此时开始读条
    /// </summary>
    /// <param name="skillBaseStructs">技能数组</param>
    public void BeginCombineSkill(SkillBaseStruct[] skillBaseStructs)
    {
        if (skillBaseStructs == null)
        {
            this.skillBaseStructs = skillBaseStructs;
            this.readTheArticleOver = false;
            this.loosenTheReleaseKey = false;
            float readTheArticleTime = 2;//计算读条时间
            runTaskStruct.StartTask(readTheArticleTime, () =>
            {
                readTheArticleOver = true;
                ReleaseCombineSkill();
            });
            //展现读条动作
            //--------------------------
        }
    }

    /// <summary>
    /// 释放组合技能
    /// </summary>
    void ReleaseCombineSkill()
    {
        if (skillBaseStructs != null && readTheArticleOver && loosenTheReleaseKey)
        {
            //释放技能
            //--------------------------
            //释放技能动作
            //--------------------------
            //初始化数据
            skillBaseStructs = null;
            readTheArticleOver = false;
            loosenTheReleaseKey = false;
            savingMagicPowerTime = 0;
        }
    }

    /// <summary>
    /// 结束组合技能(结束),此时只设置一个开关，只有读条完毕才可以释放技能
    /// </summary>
    /// <param name="savingMagicPowerTime">储蓄魔力时间</param>
    public void EndCombineSkill(float savingMagicPowerTime)
    {
        this.savingMagicPowerTime = savingMagicPowerTime;
        loosenTheReleaseKey = true;
        ReleaseCombineSkill();
    }

    /// <summary>
    /// 直接释放技能
    /// </summary>
    /// <param name="skillBaseStruct">直接释放技能</param>
    public void DirectReleaseSkill(SkillBaseStruct skillBaseStruct)
    {
        //检测当前状态，是否正在释放读条技能，并且处于非被控制状态
        if (true && skillBaseStructs == null)
        {
            //释放技能
            //-------------------
            //技能动作 
            //-------------------
        }
    }
}
