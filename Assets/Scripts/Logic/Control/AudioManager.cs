using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
///  声音管理器(切换场景后更换)
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 音频播放组件
    /// </summary>
    AudioSource[] audioSources;

    /// <summary>
    /// 背景音乐
    /// </summary>
    public AudioClip[] backAudioClips;

    /// <summary>
    /// 战斗音乐
    /// </summary>
    public AudioClip[] battleAudioClips;

    /// <summary>
    /// 物理技能音效
    /// </summary>
    public PhysicSkillAudioStruct[] physicSkillAudios;

    /// <summary>
    /// 普通攻击音效
    /// </summary>
    public PhysicNormalAudioStruct[] physicNormalAudios;

    /// <summary>
    /// 攻击命中的声音
    /// </summary>
    public AudioClip hitAudios;

    void Start()
    {
        audioSources = new AudioSource[4];//第一个播放背景音乐 第二个播放战斗音乐 第三个播放物理技能音效 第四个播放命中音效
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].spatialBlend = 0;//2D效果
            audioSources[i].loop = false;//默认不循环
        }
        audioSources[0].loop = true;
        audioSources[1].loop = true;

        if (GameState.Instance != null)
        {
            GameState.Instance.Registor<IGameState>(IGameStateChanged);
            GameState.Instance.Registor<IAnimatorState>(IAnimatorStateChanged);
            GameState.Instance.Registor<IPlayerState>(IPlayerStateChanged);
        }

        StartCoroutine(PlayBackAudio());
    }

    IEnumerator PlayBackAudio()
    {
        AudioClip audioClip = backAudioClips[UnityEngine.Random.Range(0, backAudioClips.Length)];
        if (audioClip != null)
        {
            audioSources[0].clip = audioClip;
            audioSources[0].Play();
            yield return new WaitForSeconds(audioClip.length - 1f);
            StartCoroutine(PlayBackAudio());
        }
    }

    private void OnDestroy()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.UnRegistor<IGameState>(IGameStateChanged);
            GameState.Instance.UnRegistor<IAnimatorState>(IAnimatorStateChanged);
            GameState.Instance.UnRegistor<IPlayerState>(IPlayerStateChanged);
        }
    }

    /// <summary>
    /// 用于检测战斗音乐
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameStateChanged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, EnumGameRunType>(temp => temp.GameRunType)))
        {
            if (iGameState.GameRunType == EnumGameRunType.Unsafa && battleAudioClips.Length > 0)
            {
                audioSources[1].clip = battleAudioClips[UnityEngine.Random.Range(0, battleAudioClips.Length - 1)];
                audioSources[1].Play();
            }
            else
            {
                audioSources[1].Stop();
            }
        }
    }

    /// <summary>
    /// 用于检测物理技能和普通攻击
    /// </summary>
    /// <param name="iAnimatorState"></param>
    /// <param name="fieldName"></param>
    private void IAnimatorStateChanged(IAnimatorState iAnimatorState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IAnimatorState, EnumPhysicAnimatorType>(temp => temp.PhysicAnimatorType)))
        {
            if (iAnimatorState.PhysicAnimatorType == EnumPhysicAnimatorType.Skill)
            {
                PhysicSkillAudioStruct physicSkillAudioStruct = physicSkillAudios.FirstOrDefault(temp => temp.SkillType == iAnimatorState.PhysicAnimatorSkillType);
                if (physicSkillAudioStruct != null)
                {
                    audioSources[2].clip = physicSkillAudioStruct.Clip;
                    audioSources[2].Play();
                }
            }
            else if (iAnimatorState.PhysicAnimatorType == EnumPhysicAnimatorType.Normal)
            {
                PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
                PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing && temp.leftRightArms != null && temp.leftRightArms.Value == false).First();
                if (playGoods != null)
                {
                    EnumGoodsType enumGoodsType = playGoods.GoodsInfo.EnumGoodsType;
                    //与1000相除求出剔除了具体类型后的上层一分类
                    //然后与100求余计算出具体的武器分类
                    //因为武器的分类是从10开始的因此减去10
                    //动画的第一个分类留空为0表示空手,因此加1
                    int num = (((int)enumGoodsType) / 1000) % 100 - 10 + 1;
                    PhysicNormalAudioStruct physicNormalAudioStruct = physicNormalAudios.FirstOrDefault(temp => temp.WeaponNum == num);
                    if (physicNormalAudioStruct != null)
                    {
                        if (iAnimatorState.PhycisActionNowType == 0)
                            audioSources[2].clip = physicNormalAudioStruct.Clip1;
                        else if (iAnimatorState.PhycisActionNowType == 1)
                            audioSources[2].clip = physicNormalAudioStruct.Clip2;
                        else if (iAnimatorState.PhycisActionNowType == 2)
                            audioSources[2].clip = physicNormalAudioStruct.Clip3;
                        audioSources[2].Play();
                    }
                }


            }
        }

    }

    /// <summary>
    /// 用于检测攻击到目标
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="fieldName"></param>
    private void IPlayerStateChanged(IPlayerState iPlayerState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerState, MonsterControl>(temp => temp.HitMonsterTarget)))
        {
            if (hitAudios != null)
            {
                audioSources[3].clip = hitAudios;
                audioSources[3].Play();
            }
        }
    }

    /// <summary>
    /// 技能攻击的音频结构
    /// </summary>
    [Serializable]
    public class PhysicSkillAudioStruct
    {
        /// <summary>
        /// 技能类型
        /// </summary>
        public EnumSkillType SkillType;
        /// <summary>
        /// 声音剪辑
        /// </summary>
        public AudioClip Clip;
    }

    /// <summary>
    /// 普通攻击的音频结构
    /// </summary>
    [Serializable]
    public class PhysicNormalAudioStruct
    {
        /// <summary>
        /// 武器编号
        /// 规则如下:
        /// 与1000相除求出剔除了具体类型后的上层一分类
        /// 然后与100求余计算出具体的武器分类
        /// 因为武器的分类是从10开始的因此减去10
        /// 动画的第一个分类留空为0表示空手,因此加1
        /// </summary>
        public int WeaponNum;

        /// <summary>
        /// 声音剪辑1
        /// </summary>
        public AudioClip Clip1;
        /// <summary>
        /// 声音剪辑2
        /// </summary>
        public AudioClip Clip2;
        /// <summary>
        /// 声音剪辑3
        /// </summary>
        public AudioClip Clip3;
    }
}
