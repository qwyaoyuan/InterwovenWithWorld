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
    /// 战斗时的音乐结构
    /// </summary>
    public SceneToAudioStruct[] UnsafeAudioStructs;

    /// <summary>
    /// 正常时的背景音乐结构
    /// </summary>
    public SceneToAudioStruct[] SafeAudioStructs;

    /// <summary>
    /// 背景音乐
    /// </summary>
    private AudioClip[] backAudioClips;

    /// <summary>
    /// 战斗音乐
    /// </summary>
    private AudioClip[] battleAudioClips;

    /// <summary>
    /// 物理技能音效
    /// </summary>
    public PhysicSkillAudioStruct[] physicSkillAudios;

    /// <summary>
    /// 普通攻击音效
    /// </summary>
    public PhysicNormalAudioStruct[] physicNormalAudios;

    /// <summary>
    /// 物理命中音效
    /// </summary>
    public PhysicHitAudioStruct[] physicHitAudios;

    /// <summary>
    /// 魔法命中音效
    /// </summary>
    public MagicHitAudioStruct[] magicHitAduios;

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
            GameState.Instance.Registor<IDamage>(IDamageStateChagned);
            IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
            backAudioClips = SafeAudioStructs.Where(temp => temp.sceneName == iGameState.SceneName).Select(temp => temp.audioClips).FirstOrDefault();
            battleAudioClips = UnsafeAudioStructs.Where(temp => temp.sceneName == iGameState.SceneName).Select(temp => temp.audioClips).FirstOrDefault();
        }
        StartCoroutine("PlaySafe");
        StartCoroutine("PlayUnsafe");
    }

    private void OnDestroy()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.UnRegistor<IGameState>(IGameStateChanged);
            GameState.Instance.UnRegistor<IAnimatorState>(IAnimatorStateChanged);
            GameState.Instance.UnRegistor<IDamage>(IDamageStateChagned);

        }
    }

    /// <summary>
    /// 播放安全时的音乐
    /// </summary>
    /// <returns></returns>
    IEnumerator PlaySafe()
    {
        while (backAudioClips == null)
            yield return null;
        ReSelect:
        AudioClip audioClip = backAudioClips[UnityEngine.Random.Range(0, backAudioClips.Length)];
        if (audioClip == null)
        {
            yield return null;
            goto ReSelect;
        }
        audioSources[0].clip = audioClip;
        float length = audioClip.length;
        audioSources[0].Play();
        while (length > 0)
        {
            length -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine("PlaySafe");
    }

    /// <summary>
    /// 播放战斗时时的音乐
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayUnsafe()
    {
        while (backAudioClips == null)
            yield return null;
        ReSelect:
        AudioClip audioClip = battleAudioClips[UnityEngine.Random.Range(0, battleAudioClips.Length)];
        if (audioClip == null)
        {
            yield return null;
            goto ReSelect;
        }
        audioSources[1].clip = audioClip;
        float length = audioClip.length;
        audioSources[1].Play();
        while (length > 0)
        {
            length -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine("PlayUnsafe");
    }

    /// <summary>
    /// 用于检测战斗音乐
    /// </summary>
    /// <param name="iGameState"></param>
    /// <param name="fieldName"></param>
    private void IGameStateChanged(IGameState iGameState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IGameState, string>(temp => temp.SceneName)))
        {
            backAudioClips = SafeAudioStructs.Where(temp => temp.sceneName == iGameState.SceneName).Select(temp => temp.audioClips).FirstOrDefault();
            battleAudioClips = UnsafeAudioStructs.Where(temp => temp.sceneName == iGameState.SceneName).Select(temp => temp.audioClips).FirstOrDefault();
            StopCoroutine("PlaySafe");
            StartCoroutine("PlaySafe");
            StopCoroutine("PlayUnsafe");
            StartCoroutine("PlayUnsafe");
        }
        else if (string.Equals(fieldName, GameState.Instance.GetFieldName<IGameState, EnumGameRunType>(temp => temp.GameRunType)))
        {
            if (iGameState.GameRunType == EnumGameRunType.Unsafa && battleAudioClips.Length > 0)
            {
                audioSources[1].volume = 1;
                audioSources[0].volume = 0;
            }
            else if(iGameState.GameRunType == EnumGameRunType.Safe)
            {
                audioSources[1].volume = 0;
                audioSources[0].volume = 1;
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
                PlayGoods playGoods = playerState.PlayerAllGoods.Where(temp => temp.GoodsLocation == GoodsLocation.Wearing && temp.leftRightArms != null && temp.leftRightArms.Value == false).FirstOrDefault();
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
    /// 检测伤害(显示命中声音)
    /// </summary>
    /// <param name="iDamage"></param>
    /// <param name="fieldName"></param>
    private void IDamageStateChagned(IDamage iDamage, string fieldName)
    {
        if (string.Equals(fieldName, GameState.GetFieldNameStatic<IDamage, int>(temp => temp.WeaponPhysicHit)))
        {
            EnumWeaponTypeByPlayerState weaponType = (EnumWeaponTypeByPlayerState)iDamage.WeaponPhysicHit;
            PhysicHitAudioStruct physicHitAudioStruct = physicHitAudios.FirstOrDefault(temp => temp.WeaponType == weaponType);
            if (physicHitAudioStruct != null)
            {
                audioSources[3].clip = physicHitAudioStruct.Clip;
                audioSources[3].Play();
            }
        }
        else if (string.Equals(fieldName, GameState.GetFieldNameStatic<IDamage, int>(temp => temp.MagicTypeHit)))
        {
            if (iDamage.MagicTypeHit >= (int)EnumSkillType.MagicCombinedLevel2Start && iDamage.MagicTypeHit < (int)EnumSkillType.MagicCombinedLevel2End)
            {
                MagicHitAudioStruct magicHitAudioStruct = magicHitAduios.FirstOrDefault(temp => temp.SkillType == iDamage.MagicTypeHit);
                if (magicHitAudioStruct != null)
                {
                    audioSources[3].clip = magicHitAudioStruct.Clip;
                    audioSources[3].Play();
                }
            }
        }
    }

    /// <summary>
    /// 场景对应音乐结构
    /// </summary>
    /// </summary>
    [Serializable]
    public class SceneToAudioStruct
    {
        /// <summary>
        /// 场景名
        /// </summary>
        public string sceneName;

        /// <summary>
        /// 音乐剪辑
        /// </summary>
        public AudioClip[] audioClips;
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

    /// <summary>
    /// 物理技能攻击命中的音频结构
    /// </summary>
    [Serializable]
    public class PhysicHitAudioStruct
    {
        /// <summary>
        /// 武器类型 
        /// </summary>
        public EnumWeaponTypeByPlayerState WeaponType;

        /// <summary>
        /// 声音剪辑
        /// </summary>
        public AudioClip Clip;
    }

    /// <summary>
    /// 魔法攻击命中的音频结构(根据二阶段的元素类型划分)
    /// </summary>
    [Serializable]
    public class MagicHitAudioStruct
    {
        /// <summary>
        /// 技能类型选区范围是1100-1199,1100表示如果查找不到时使用的默认选择 
        /// </summary>
        [Range(1100, 1199)]
        public int SkillType;
        /// <summary>
        /// 声音剪辑
        /// </summary>
        public AudioClip Clip;
    }
}
