using GoodsEditor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsProgram.CommonLoad
{
    /// <summary>
    /// 物品信息的元数据
    /// </summary>
    public class GoodsMetaInfoMations
    {

        Dictionary<int, Goods> allGoodsInfo = new Dictionary<int, Goods>();


        public void Load()
        {
            allGoodsInfo = JsonConvert.DeserializeObject<Dictionary<int, Goods>>(File.ReadAllText("allGoodsAbility.goosInfo"));
        }


        /// <summary>
        /// 根据物品枚举获取基本信息
        /// </summary>
        /// <param name="enumGoodsType"></param>
        /// <returns></returns>
        public Goods this[EnumGoodsType enumGoodsType]
        {

            get
            {

                int intType = (int)enumGoodsType;
                if (allGoodsInfo.ContainsKey(intType))
                {
                    return allGoodsInfo[intType];
                }
                else
                {
                    return null;
                }
            }

        }


    }
}
