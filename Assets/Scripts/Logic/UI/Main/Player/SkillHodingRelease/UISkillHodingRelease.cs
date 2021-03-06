﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能蓄力与释放界面 
/// </summary>
public class UISkillHodingRelease : MonoBehaviour
{
    /// <summary>
    /// 第一个技能的图片对象
    /// </summary>
    public Transform firstImage;
    /// <summary>
    /// 第二个技能的图片对象
    /// </summary>
    public Transform secondImage;
    /// <summary>
    /// 第三个技能的图片对象
    /// </summary>
    public Transform thirdImage;
    /// <summary>
    /// 第四个技能的图片对象
    /// </summary>
    public Transform fourthImage;
    /// <summary>
    /// 第一个技能的冷却进度条
    /// </summary>
    public Transform firstInnerImage;
    /// <summary>
    /// 第二个技能的冷却进度条
    /// </summary>
    public Transform secondInnerImage;
    /// <summary>
    /// 第三个技能的冷却进度条
    /// </summary>
    public Transform thirdInnerImage;
    /// <summary>
    /// 第四个技能的冷却进度条
    /// </summary>
    public Transform fourthInnerImage;

    /// <summary>
    /// 蓄力图片
    /// </summary>
    public Image hodingImage;

    /// <summary>
    /// 用来播放动画的对象
    /// </summary>
    public List<Image> animationImageList;
    /// <summary>
    /// 序列帧图片
    /// </summary>
    public List<Sprite> animationSpriteList;
    /// <summary>
    /// 播放的间隔时间
    /// </summary>
    public float intervalTime = 0.15f;

    /// <summary>
    /// 当前时间
    /// </summary>
    float nowTime = 0;
    /// <summary>
    /// 动画播放到的下标
    /// </summary>
    int animationIndex = -1;

    /// <summary>
    /// 技能结构对象
    /// </summary>
    SkillStructData skillStructData;

    /// <summary>
    /// 玩家技能状态对象
    /// </summary>
    ISkillState iSkillState;
    /// <summary>
    /// 玩家属性状态
    /// </summary>
    IAttributeState iAttributeState;

    private void Start()
    {
        GameState.Instance.Registor<ISkillState>(ISkillStateChanged);
        GameState.Instance.Registor<IAttributeState>(IAttributeStateChanged);
        skillStructData = DataCenter.Instance.GetMetaData<SkillStructData>();
        iSkillState = GameState.Instance.GetEntity<ISkillState>();
        iAttributeState = GameState.Instance.GetEntity<IAttributeState>();
    }

    /// <summary>
    /// 玩家属性发生更改 
    /// </summary>
    /// <param name="iAttribute"></param>
    /// <param name="fieldName"></param>
    private void IAttributeStateChanged(IAttributeState iAttribute, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAttributeState, float>(temp => temp.ExemptionChantingTime)))//咏唱时间改变
        {
            ChangeHoldingImage();
        }
    }

    /// <summary>
    /// 技能状态发生变化
    /// </summary>
    /// <param name="iSkillState"></param>
    /// <param name="fieldName"></param>
    private void ISkillStateChanged(ISkillState iSkillState, string fieldName)
    {
        //组合技能改变
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, SkillBaseStruct[]>(temp => temp.CombineSkills)))
        {
            ChangeSpriteLattice();
        }
        //释放技能
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, bool>(temp => temp.IsSkillStartHolding)))
        {
            ChangeHoldingImage();
        }
        //蓄力时间
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, float>(temp => temp.SkillStartHoldingTime)))
        {
            ChangeHoldingImage();
        }
        //公共冷却时间
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, float>(temp => temp.PublicCoolingTime)))
        {
            ChangeSpriteLattice();
        }
        //技能冷却时间
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<ISkillState, Func<int, float>>(temp => temp.GetSkillRuntimeCoolingTime)))
        {
            ChangeSpriteLattice();
        }
    }

    /// <summary>
    /// 更新显示
    /// </summary>
    private void ChangeSpriteLattice()
    {
        SkillBaseStruct[] skillBaseStructs = iSkillState.CombineSkills;
        Action<Transform, Sprite> SetImageSprite = (trans, sprite) =>//设置技能的图片显示
        {
            if (trans)
            {
                Image image = trans.GetComponent<Image>();
                if (image)
                {
                    image.sprite = sprite;
                    image.enabled = sprite != null;
                }
            }
        };
        Action<Transform, float> SetImageInnerState = (trans, coolingTime) =>//设置技能的冷却显示
        {
            if (trans)
            {
                Image image = trans.GetComponent<Image>();
                if (image)
                {
                    image.fillAmount = Mathf.Clamp(coolingTime, 0, 1);
                }
            }
        };
        if (skillBaseStructs != null)
        {
            int skillID = SkillCombineStaticTools.GetCombineSkillKey(skillBaseStructs);
            float skillCoolingTime = iSkillState.GetSkillRuntimeCoolingTime(skillID);//本技能的冷却时间
            float maxTime = skillCoolingTime > iSkillState.PublicCoolingTime ? (skillCoolingTime / iSkillState.GetSkillMaxCoolingTime(skillID)) : iSkillState.PublicCoolingTime;//选取最大的时间作为显示时间
            if (skillBaseStructs.Length > 0 && skillBaseStructs[0] != null)
            {
                Sprite sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, (int)skillBaseStructs[0].skillType);
                SetImageSprite(firstImage, sprite);
                SetImageInnerState(firstInnerImage, maxTime);
            }
            else//清理第一个框
            {
                SetImageSprite(firstImage, null);
                SetImageInnerState(firstInnerImage, 0);
            }

            if (skillBaseStructs.Length > 1 && skillBaseStructs[1] != null)
            {
                Sprite sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, (int)skillBaseStructs[1].skillType);
                SetImageSprite(secondImage, sprite);
                SetImageInnerState(secondInnerImage, maxTime);
            }
            else//清理第二个框
            {
                SetImageSprite(secondImage, null);
                SetImageInnerState(secondInnerImage, 0);
            }

            if (skillBaseStructs.Length > 2 && skillBaseStructs[2] != null)
            {
                Sprite sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, (int)skillBaseStructs[2].skillType);
                SetImageSprite(thirdImage, sprite);
                SetImageInnerState(thirdInnerImage, maxTime);
            }
            else//清理第三个框
            {
                SetImageSprite(thirdImage, null);
                SetImageInnerState(thirdInnerImage, 0);
            }

            if (skillBaseStructs.Length > 3 && skillBaseStructs[3] != null)
            {
                Sprite sprite = SkillCombineStaticTools.GetCombineSkillSprite(skillStructData, (int)skillBaseStructs[3].skillType);
                SetImageSprite(fourthImage, sprite);
                SetImageInnerState(fourthInnerImage, maxTime);
            }
            else//清理第四个框
            {
                SetImageSprite(fourthImage, null);
                SetImageInnerState(fourthInnerImage, 0);
            }
        }
        else
        {
            SetImageSprite(firstImage, null);
            SetImageSprite(secondImage, null);
            SetImageSprite(thirdImage, null);
            SetImageSprite(fourthImage, null);
            SetImageInnerState(firstInnerImage, 0);
            SetImageInnerState(secondInnerImage, 0);
            SetImageInnerState(thirdInnerImage, 0);
            SetImageInnerState(fourthInnerImage, 0);
        }
    }

    /// <summary>
    /// 更改蓄力图
    /// </summary>
    private void ChangeHoldingImage()
    {
        hodingImage.enabled = iSkillState.IsSkillStartHolding;
        if (iSkillState.IsSkillStartHolding)
        {

            float bili = iSkillState.SkillStartHoldingTime / GameState.BaseSkillStartHoldingTime;
            bili = Mathf.Clamp(bili, 0, 1);
            hodingImage.fillAmount = bili;
        }
        else
        {
            hodingImage.fillAmount = 0;
        }
    }

    void Update()
    {
        nowTime -= Time.deltaTime;
        if (nowTime < 0)
        {
            nowTime = intervalTime;
            PlaySprite();
        }
    }

    /// <summary>
    /// 播放序列帧动画
    /// </summary>
    void PlaySprite()
    {
        if (animationIndex < 0)
            animationIndex = 0;
        if (animationIndex < animationSpriteList.Count)
        {
            Sprite thisSprite = animationSpriteList[animationIndex];
            foreach (var item in animationImageList)
            {
                item.sprite = thisSprite;
            }
        }
        animationIndex++;
        if (animationIndex >= animationSpriteList.Count)
            animationIndex = -1;
    }
}
