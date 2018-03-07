using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 处理功能交互中宝箱类型的类
/// </summary>
public class ActionInteractiveDataInfoMono_TreasureBox : ActionInteractiveDataInfoMono
{
    /// <summary>
    /// 宝箱的状态
    /// </summary>
    public bool treasureBoxState;

    /// <summary>
    /// 动画组件
    /// </summary>
    Animator animator;

    protected override void InnerAwake()
    {
        base.InnerAwake();
        //获取动画组件
        animator = GetComponent<Animator>();
    }

    public override void LoadData(object obj, Action<object> SaveDataAction)
    {
        base.LoadData(obj, SaveDataAction);
        if (obj != null && obj.GetType().Equals(typeof(bool)))
        {
            treasureBoxState = (bool)obj;
            //设置动画状态
            if (animator != null)
            {
                animator.SetBool("TreasureBox", treasureBoxState);
            }
        }
    }

    public override void ActionTrigger()
    {
        base.ActionTrigger();
        if (!treasureBoxState)
        {
            //触发宝箱
            treasureBoxState = true;
            if (animator != null)
            {
                animator.SetBool("TreasureBox", treasureBoxState);
            }
            //保存状态
            SaveDataAction(true);
            //发放道具
            Dictionary<EnumGoodsType, ActionInteractiveDataInfo.TreasureBoxStruct> dataDic = ActionInteractiveDataInfo.OtherValue as Dictionary<EnumGoodsType, ActionInteractiveDataInfo.TreasureBoxStruct>;
            if (dataDic != null)
            {
                //实例化数据并填入存档中
                GoodsMetaInfoMations goodsMetaInfoMations = DataCenter.Instance.GetMetaData<GoodsMetaInfoMations>();
                PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
                foreach (KeyValuePair<EnumGoodsType, ActionInteractiveDataInfo.TreasureBoxStruct> data in dataDic)
                {
                    EnumGoodsType baseGoodsType = data.Key;
                    EnumQualityType qualityTypeMin = data.Value.MinQualityType;
                    EnumQualityType qualityTypeMax = data.Value.MaxQualityType;
                    Goods addGoods = goodsMetaInfoMations[baseGoodsType].Clone(true);//获取需要添加物品的原始属性
                    //如果是道具或炼金物品则可以叠加,但是没有品质
                    if (baseGoodsType > EnumGoodsType.Item && baseGoodsType < EnumGoodsType.SpecialItem)
                    {
                        //尝试从玩家道具中获取id,如果没有则新建一个(这个id并没有什么卵用)
                        int addGoodsID = playerState.PlayerAllGoods.Where(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType).Select(temp => temp.ID).FirstOrDefault();
                        if (addGoodsID == 0)
                        {
                            addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
                            PlayGoods playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.Package);
                            playGoods.QualityType = EnumQualityType.White;
                            playGoods.Count = data.Value.Count;
                            playerState.PlayerAllGoods.Add(playGoods);
                        }
                        else
                        {
                            PlayGoods playGoods = playerState.PlayerAllGoods.FirstOrDefault(temp => temp.ID == addGoodsID);
                            playGoods.Count += data.Value.Count;
                        }
                    }
                    //如果不是道具和炼金物品则不可以叠加,但是存在品质
                    else
                    {
                        int addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
                        EnumQualityType qualityType = (EnumQualityType)UnityEngine.Random.Range((int)qualityTypeMin, (int)qualityTypeMax + 1);
                        // 根据品质设置属性
                        if (qualityType != EnumQualityType.Red)//如果是唯一的则不用改变属性
                        {
                            //将所有属性随机
                            addGoods.goodsAbilities.ForEach(temp =>
                            {
                                float min = temp.Value * 0.8f;
                                float max = temp.Value * 1.2f;
                                temp.Value = (int)UnityEngine.Random.Range(min, max);
                            });
                            //取出用于分割固定属性以及随机属性的条目
                            GoodsAbility goodsAbility_nothing = addGoods.goodsAbilities.FirstOrDefault(temp => temp.AbilibityKind == EnumGoodsAbility.Nothing);
                            int index = -1;
                            if (goodsAbility_nothing != null)
                                index = addGoods.goodsAbilities.IndexOf(goodsAbility_nothing);
                            //存在分割属性
                            if (index > -1 && index + 1 < addGoods.goodsAbilities.Count)
                            {
                                //取出分割项后的可变项
                                List<EnumGoodsAbility> randomAbilitys = Enumerable.Range(index + 1, addGoods.goodsAbilities.Count - index - 1).
                                    Select(temp => addGoods.goodsAbilities[temp]).Select(temp => temp.AbilibityKind).ToList();
                                int saveAbilityCount = (int)qualityType;//可保留的属性
                                while (randomAbilitys.Count > saveAbilityCount)
                                {
                                    int removeIndex = UnityEngine.Random.Range(0, randomAbilitys.Count);
                                    EnumGoodsAbility removeGoodsAbility = randomAbilitys[removeIndex];
                                    addGoods.goodsAbilities.RemoveAll(temp => temp.AbilibityKind == removeGoodsAbility);
                                    randomAbilitys.RemoveAt(removeIndex);
                                }
                            }
                        }
                        addGoods.goodsAbilities.RemoveAll(temp => temp.AbilibityKind == EnumGoodsAbility.Nothing);
                        PlayGoods playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.Package);
                        playGoods.QualityType = qualityType;
                        playGoods.Count = 1;
                        playerState.PlayerAllGoods.Add(playGoods);
                    }
                }

            }
        }
    }
}

