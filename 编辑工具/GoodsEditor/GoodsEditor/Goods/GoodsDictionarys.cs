using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsEditor
{
    /// <summary>
    /// 所有物品名称管理类
    /// </summary>
    class GoodsDictionarys
    {
        #region 构造
        /// <summary>
        /// 物品种类名字典
        /// </summary>
        private Dictionary<int, string> GoodsType_dic = new Dictionary<int, string>();
        /// <summary>
        /// 物品属性说明字典
        /// </summary>
        private Dictionary<int, string> GoodsNature_dic = new Dictionary<int, string>();
        /// <summary>
        /// 物品名称管理类单例
        /// </summary>
        private static GoodsDictionarys instance;
        /// <summary>
        /// 物品名称管理类初始化
        /// </summary>
        private GoodsDictionarys()
        {
            #region 素材类
            GoodsType_dic.Add(1, "素材类");

            GoodsType_dic.Add(11, "矿石类");
            GoodsType_dic.Add(1110, "矿石");
            GoodsType_dic.Add(1111, "宝石");
            GoodsType_dic.Add(1112, "铸锭");

            GoodsType_dic.Add(12, "生物素材类");
            GoodsType_dic.Add(1210, "兽皮");
            GoodsType_dic.Add(1211, "兽爪");
            GoodsType_dic.Add(1212, "兽骨");
            GoodsType_dic.Add(1213, "兽肉");

            GoodsType_dic.Add(13, "自然素材类");
            GoodsType_dic.Add(1310, "草药");
            GoodsType_dic.Add(1311, "木材");
            GoodsType_dic.Add(1312, "果实");

            GoodsType_dic.Add(14, "特殊素材类");
            GoodsType_dic.Add(1410, "掉落特殊素材");
            GoodsType_dic.Add(1411, "任务特殊素材");
            #endregion 素材类
            #region 装备类
            GoodsType_dic.Add(2, "装备类");

            GoodsType_dic.Add(21, "武器类");
            GoodsType_dic.Add(2110, "单手剑");
            GoodsType_dic.Add(2111, "双手剑");
            GoodsType_dic.Add(2112, "弓");
            GoodsType_dic.Add(2113, "弩");
            GoodsType_dic.Add(2114, "盾");
            GoodsType_dic.Add(2115, "匕首");
            GoodsType_dic.Add(2116, "长杖");
            GoodsType_dic.Add(2117, "短杖");
            GoodsType_dic.Add(2118, "水晶球");

            GoodsType_dic.Add(22, "头盔类");
            GoodsType_dic.Add(2210, "头盔");
            GoodsType_dic.Add(2211, "头环");
            GoodsType_dic.Add(2212, "兜帽");

            GoodsType_dic.Add(23, "盔甲类");
            GoodsType_dic.Add(2310, "重甲");
            GoodsType_dic.Add(2311, "皮甲");
            GoodsType_dic.Add(2312, "法袍");

            GoodsType_dic.Add(24, "靴子类");
            GoodsType_dic.Add(2410, "甲靴");
            GoodsType_dic.Add(2411, "皮靴");
            GoodsType_dic.Add(2412, "布靴");

            GoodsType_dic.Add(25, "饰品类");
            GoodsType_dic.Add(2510, "项链");
            GoodsType_dic.Add(2511, "戒指");
            GoodsType_dic.Add(2512, "护身符");
            GoodsType_dic.Add(2513, "勋章");
            #endregion
            #region 道具类
            GoodsType_dic.Add(3, "道具类");

            GoodsType_dic.Add(31, "投掷道具类");
            GoodsType_dic.Add(3110, "炸弹");
            GoodsType_dic.Add(3111, "飞行道具");

            GoodsType_dic.Add(32, "魔偶类");
            GoodsType_dic.Add(3210, "可制作魔偶");
            GoodsType_dic.Add(3211, "不可制作魔偶");

            GoodsType_dic.Add(33, "陷阱类");
            GoodsType_dic.Add(3310, "固定点陷阱");
            GoodsType_dic.Add(3311, "特殊用法陷阱");

            #endregion
            #region 炼金药剂类
            GoodsType_dic.Add(4, "炼金药剂类");

            GoodsType_dic.Add(41, "酒类");
            GoodsType_dic.Add(4110, "普通酒");

            GoodsType_dic.Add(42, "恢复药剂类");
            GoodsType_dic.Add(4210, "恢复药剂");

            GoodsType_dic.Add(43, "强化药剂类");
            GoodsType_dic.Add(4310, "强化药剂");

            GoodsType_dic.Add(44, "附魔药剂类");
            GoodsType_dic.Add(4410, "附魔药剂");

            GoodsType_dic.Add(45, "功能药剂类");
            GoodsType_dic.Add(4510, "功能药剂");

            #endregion
            #region 特殊物品类
            GoodsType_dic.Add(5, "特殊物品类");

            GoodsType_dic.Add(51, "书籍类");
            GoodsType_dic.Add(5110, "故事书");
            GoodsType_dic.Add(5111, "技能书");
            GoodsType_dic.Add(5112, "信笺");

            #endregion

            #region 物品属性类型字典
            GoodsNature_dic.Add(0, "None");
            GoodsNature_dic.Add(10, "生命值附加:获得额外指定数值的生命值");
            GoodsNature_dic.Add(20, "法力值附加:获得额外指定数值的法力值");
            GoodsNature_dic.Add(30, "灵巧值附加:获得额外指定数值的灵巧值");
            GoodsNature_dic.Add(40, "专注值附加:获得额外指定数值的专注值");
            GoodsNature_dic.Add(50, "意志值附加:获得额外指定数值的意志值");
            GoodsNature_dic.Add(60, "力量值附加:获得额外指定数值的力量值");

            GoodsNature_dic.Add(10000, "None");
            #endregion
        }


        //基础类型,功能类型,物品具体类型
        public Dictionary<string, int> GoodsBaseType { get { return GoodsType_dic.Where(kv => kv.Key.ToString().Length == 1).ToDictionary(kv => kv.Value, kv => kv.Key); } }



        /// <summary>
        /// 获取功能类型
        /// </summary>
        /// <param name="goodsBaseType"></param>
        /// <returns></returns>
        public Dictionary<int, string> GoodsFuncType(int goodsBaseType)
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            foreach (var goodsTypeKv in GoodsType_dic)
            {
                if (goodsTypeKv.Key.ToString().Take(1).Select(c => int.Parse(c.ToString())).First() == goodsBaseType && goodsTypeKv.Key.ToString().Length == 2)
                    ret.Add(goodsTypeKv.Key, goodsTypeKv.Value);
            }

            return ret;
        }

        /// <summary>
        /// 获取物品具体类型
        /// </summary>
        /// <param name="goodsFuncType"></param>
        /// <returns></returns>
        public Dictionary<int, string> GoodSpecialType(int goodsFuncType)
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            foreach (var goodsTypeKv in GoodsType_dic)
            {
               
                if (goodsTypeKv.Key.ToString().Length == 4 &&  goodsTypeKv.Key/ 100 == goodsFuncType)
                    ret.Add(goodsTypeKv.Key, goodsTypeKv.Value);
            }

            return ret;
        }


        /// <summary>
        /// 物品名称管理类单例获取
        /// </summary>
        /// <returns></returns>
        public static GoodsDictionarys Instance()
        {
            if (instance == null)
                instance = new GoodsDictionarys();
            return instance;
        }
        #endregion

        /// <summary>
        /// 物品类型名称获取方法
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetGoodsType(int ID)
        {
            string type = "";
            string type1 = "未找到类型枚举";
            string type2 = "未找到类型枚举";
            string type3 = "未找到类型枚举";

            if (GoodsType_dic.ContainsKey(ID / 1000000))
            {
                type1 = GoodsType_dic[ID / 1000000];
            }
            if (GoodsType_dic.ContainsKey(ID / 100000))
            {
                type2 = GoodsType_dic[ID / 100000];
            }
            if (GoodsType_dic.ContainsKey(ID / 1000))
            {
                type3 = GoodsType_dic[ID / 1000];
            }

            type = type1 + "," + type2 + "," + type3;
            return type;
        }
        /// <summary>
        /// 物品属性说明获取方法
        /// </summary>
        /// <param name="NatureEnum"></param>
        /// <returns></returns>
        public string GetGoodsNatrue(int NatureEnum)
        {
            string NatureComplain = "属性说明";
            NatureComplain = GoodsNature_dic[NatureEnum];
            return NatureComplain;
        }
    }
}
