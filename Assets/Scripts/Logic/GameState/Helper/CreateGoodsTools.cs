using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 生成物品工具
/// </summary>
public class CreateGoodsTools
{
    /// <summary>
    /// 创建物品并将物品保存到存档中
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="goodsMetaInfoMations"></param>
    /// <param name="baseGoodsType"></param>
    /// <param name="minQuality"></param>
    /// <param name="maxQuality"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static PlayGoods CreatePlayGoodsAddToPlayData(PlayerState playerState, GoodsMetaInfoMations goodsMetaInfoMations, EnumGoodsType baseGoodsType, EnumQualityType minQuality, EnumQualityType maxQuality, int count)
    {
        PlayGoods playGoods = new PlayGoods();
        Goods addGoods = goodsMetaInfoMations[baseGoodsType].Clone(true);//获取需要添加物品的原始属性
        //如果是道具或炼金物品则可以叠加,但是没有品质
        if (baseGoodsType > EnumGoodsType.Item && baseGoodsType < EnumGoodsType.SpecialItem)
        {
            int addGoodsID = playerState.PlayerAllGoods.Where(temp => temp.GoodsInfo.EnumGoodsType == baseGoodsType).Select(temp => temp.ID).FirstOrDefault();
            if (addGoodsID == 0)
            {
                addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
                playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.Package);
                playGoods.Count = count;
                playGoods.QualityType = EnumQualityType.White;
                playerState.PlayerAllGoods.Add(playGoods);
            }
            else
            {
                playGoods.Count += count;
            }
        }
        else
        {
            int addGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
            EnumQualityType qualityType = (EnumQualityType)UnityEngine.Random.Range((int)minQuality, (int)maxQuality + 1);
            float minRate = ((int)qualityType) * 0.2f + 0.8f;
            float maxRate = minRate + 0.2f;
            // 根据品质设置属性
            if (qualityType != EnumQualityType.Red)//如果是唯一的则不用改变属性
            {
                //将所有属性随机(根据品质)
                addGoods.goodsAbilities.ForEach(temp =>
                {
                    float min = temp.Value * minRate;
                    float max = temp.Value * maxRate;
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
                addGoods.goodsAbilities.RemoveAll(temp => temp.AbilibityKind == EnumGoodsAbility.Nothing);
                playGoods = new PlayGoods(addGoodsID, addGoods, GoodsLocation.Package);
                playGoods.QualityType = qualityType;
                playGoods.Count = 1;
                playerState.PlayerAllGoods.Add(playGoods);
            }
        }
        return playGoods;
    }
}

