using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

/// <summary>
/// 用于记录组合技能是否可以向下组合的结构
/// </summary>
public class SkillCombineNextNodeStruct
{
    /// <summary>
    /// 当前阶段
    /// </summary>
    int level;
    /// <summary>
    /// 当前技能
    /// </summary>
    public EnumSkillType enumSkillType { get; private set; }
    /// <summary>
    /// 父节点
    /// </summary>
    public SkillCombineNextNodeStruct parent { get; private set; }
    /// <summary>
    /// 下面的阶段
    /// </summary>
    public List<SkillCombineNextNodeStruct> nextNodes { get; private set; }

    public SkillCombineNextNodeStruct(EnumSkillType enumSkillType, SkillCombineNextNodeStruct parent)
    {
        this.enumSkillType = enumSkillType;
        this.parent = parent;
        nextNodes = new List<SkillCombineNextNodeStruct>();
        //设置当前技能阶段
        if (enumSkillType > EnumSkillType.MagicCombinedLevel1Start && enumSkillType < EnumSkillType.MagicCombinedLevel1End)
            level = 1;
        else if (enumSkillType > EnumSkillType.MagicCombinedLevel2Start && enumSkillType < EnumSkillType.MagicCombinedLevel2End)
            level = 2;
        else if (enumSkillType > EnumSkillType.MagicCombinedLevel3Start && enumSkillType < EnumSkillType.MagicCombinedLevel3End)
            level = 3;
        else if (enumSkillType > EnumSkillType.MagicCombinedLevel4Start && enumSkillType < EnumSkillType.MagicCombinedLevel4End)
            level = 4;
    }

    /// <summary>
    /// 是否可以使用这个组合
    /// 使用穷举法
    /// </summary>
    /// <returns></returns>
    public bool CanUseThisCombine()
    {
        //主要用于检测当前的技能链组合是否可行
        //在三阶段魔法中
        if (level == 3)
        {
            //连续魔力导向不能只能和魔力导向进行组合,并且不能和光明元素与黑暗元素进行组合
            if (enumSkillType == EnumSkillType.MFS06)
            {
                if (parent.parent.enumSkillType != EnumSkillType.FS04)
                    return false;
                if (parent.enumSkillType == EnumSkillType.DSM03 ||
                    parent.enumSkillType == EnumSkillType.DSM04)
                    return false;
            }
        }
        //在四阶段中
        if (level == 4)
        {
            //精灵呼唤不可以和冰元素 雷元素 光明元素 黑暗元素组合
            if (enumSkillType == EnumSkillType.ZHS09)
            {
                if (parent.parent.enumSkillType == EnumSkillType.SM06 ||
                    parent.parent.enumSkillType == EnumSkillType.SM07 ||
                    parent.parent.enumSkillType == EnumSkillType.DSM03 ||
                    parent.parent.enumSkillType == EnumSkillType.DSM04)
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 设置下一个阶段
    /// </summary>
    public void SetNext()
    {
        EnumSkillType[] level_SkillTypes = null;
        if (level == 1)//当前1阶段选取2阶段
        {
            level_SkillTypes = Enum.GetValues(typeof(EnumSkillType))
                .OfType<EnumSkillType>()
                .Where(temp => temp > EnumSkillType.MagicCombinedLevel2Start && temp < EnumSkillType.MagicCombinedLevel2End)
                .ToArray();
        }
        else if (level == 2)//当前2阶段选取3阶段
        {
            level_SkillTypes = Enum.GetValues(typeof(EnumSkillType))
                .OfType<EnumSkillType>()
                .Where(temp => temp > EnumSkillType.MagicCombinedLevel3Start && temp < EnumSkillType.MagicCombinedLevel3End)
                .ToArray();
        }
        else if (level == 3)//当前3阶段选取4阶段
        {
            level_SkillTypes = Enum.GetValues(typeof(EnumSkillType))
                .OfType<EnumSkillType>()
                .Where(temp => temp > EnumSkillType.MagicCombinedLevel4Start && temp < EnumSkillType.MagicCombinedLevel4End)
                .ToArray();
        }
        if (level_SkillTypes != null)
        {
            foreach (EnumSkillType enumSkillType in level_SkillTypes)
            {
                SkillCombineNextNodeStruct skillCombineNextNodeStruct = new SkillCombineNextNodeStruct(enumSkillType, this);
                if (skillCombineNextNodeStruct.CanUseThisCombine())
                {
                    nextNodes.Add(skillCombineNextNodeStruct);
                    //设置下一个
                    skillCombineNextNodeStruct.SetNext();
                }
            }
        }
    }

    /// <summary>
    /// 这是否是一个技能组合
    /// </summary>
    /// <param name="skillTypes"></param>
    /// <returns></returns>
    public bool ThisIsACombineSkills(List<EnumSkillType> skillTypes)
    {
        if (skillTypes.Contains(enumSkillType))
        {
            skillTypes.Remove(enumSkillType);
            if (skillTypes.Count > 0)
            {
                int rightCount = nextNodes.Select(temp => temp.ThisIsACombineSkills(skillTypes)).Count(temp => temp);
                return rightCount > 0;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取组合技能的名
    /// </summary>
    /// <param name="tempVar"></param>
    /// <param name="defaultName">初始名字(外部调用时不需要传入)</param>
    /// <returns></returns>
    public string GetCombineSkillName(List<KeyValuePair<EnumSkillType, string>> tempList, string defaultName)
    {
        if (tempList.Count(temp => temp.Key == enumSkillType) == 0)
            return "";
        else tempList.RemoveAll(temp => temp.Key == enumSkillType);
        //1阶段
        if (level == 1)
        {
            switch (enumSkillType)
            {
                case EnumSkillType.FS01:
                    defaultName = "奥数弹";
                    break;
                case EnumSkillType.FS03:
                    defaultName = "魔力屏障";
                    break;
                case EnumSkillType.FS02:
                    defaultName = "奥术震荡";
                    break;
                case EnumSkillType.FS04:
                    defaultName = "魔力导向";
                    break;
                case EnumSkillType.FS05:
                    defaultName = "魔力脉冲";
                    break;
            }
        }
        //2阶段
        else if (level == 2)
        {
            switch (enumSkillType)
            {
                case EnumSkillType.YSX01://火元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "火球";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "火屏障";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "炎舞";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "炎爆术";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "火枪贯通";
                            break;
                    }
                    break;
                case EnumSkillType.YSX02://水元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "水箭";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "水墙术";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "雨雾";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "水幕";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "水啸";
                            break;
                    }
                    break;
                case EnumSkillType.YSX03://土元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "土爆术";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "土盾";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "岩枪术";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "陨落";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "大地咆哮";
                            break;
                    }
                    break;
                case EnumSkillType.YSX04://风元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "风刃";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "风屏障";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "旋风术";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "风蚀术";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "风暴";
                            break;
                    }
                    break;
                case EnumSkillType.SM06://冰元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "冰矢";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "冰墙术";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "暴风雪";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "冰结咒";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "寒霜吐息";
                            break;
                    }
                    break;
                case EnumSkillType.SM07://雷元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "闪电球";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "雷屏障";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "雷暴";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "雷击术";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "闪电冲击";
                            break;
                    }
                    break;
                case EnumSkillType.DSM03://光明元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "光球";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "光屏障";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "光域";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "光明冲击";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "极光";
                            break;
                    }
                    break;
                case EnumSkillType.DSM04://黑暗元素
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.FS01:
                            defaultName = "黑刃";
                            break;
                        case EnumSkillType.FS03:
                            defaultName = "暗屏障";
                            break;
                        case EnumSkillType.FS02:
                            defaultName = "暗域";
                            break;
                        case EnumSkillType.FS04:
                            defaultName = "黑暗侵蚀";
                            break;
                        case EnumSkillType.FS05:
                            defaultName = "暗流";
                            break;
                    }
                    break;
            }
        }
        //3阶段
        else if (level == 3)
        {
            switch (enumSkillType)
            {
                case EnumSkillType.MFS06://连续魔力导向
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.YSX01:
                            defaultName = "火龙链";
                            break;
                        case EnumSkillType.YSX02:
                            defaultName = "水流冲击";
                            break;
                        case EnumSkillType.YSX03:
                            defaultName = "地裂";
                            break;
                        case EnumSkillType.YSX04:
                            defaultName = "疾风暴走";
                            break;
                        case EnumSkillType.SM06:
                            defaultName = "寒霜路径";
                            break;
                        case EnumSkillType.SM07:
                            defaultName = "闪电链";
                            break;
                    }
                    break;
                case EnumSkillType.MFS08://双重法术
                    defaultName = "双重" + defaultName;
                    break;
                case EnumSkillType.MFS04://法术陷阱
                    defaultName += "陷阱";
                    break;
                case EnumSkillType.YSX07://元素精炼
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.YSX01:
                            defaultName = "强焱" + defaultName;
                            break;
                        case EnumSkillType.YSX02:
                            defaultName = "幻海" + defaultName;
                            break;
                        case EnumSkillType.YSX03:
                            defaultName = "地灵" + defaultName;
                            break;
                        case EnumSkillType.YSX04:
                            defaultName = "龙卷" + defaultName;
                            break;
                        case EnumSkillType.SM06:
                            defaultName = "玄冰" + defaultName;
                            break;
                        case EnumSkillType.SM07:
                            defaultName = "霹雳" + defaultName;
                            break;
                        case EnumSkillType.DSM03:
                            defaultName = "神圣" + defaultName;
                            break;
                        case EnumSkillType.DSM04:
                            defaultName = "死亡" + defaultName;
                            break;
                    }
                    break;
                case EnumSkillType.YSX06://元素驻留
                    switch (parent.enumSkillType)
                    {
                        case EnumSkillType.YSX01:
                            defaultName = "爆燃" + defaultName;
                            break;
                        case EnumSkillType.YSX02:
                            defaultName = "涟漪" + defaultName;
                            break;
                        case EnumSkillType.YSX03:
                            defaultName = "震裂" + defaultName;
                            break;
                        case EnumSkillType.YSX04:
                            defaultName = "缠魂" + defaultName;
                            break;
                        case EnumSkillType.SM06:
                            defaultName = "霜冻" + defaultName;
                            break;
                        case EnumSkillType.SM07:
                            defaultName = "天惩" + defaultName;
                            break;
                        case EnumSkillType.DSM03:
                            defaultName = "神谴" + defaultName;
                            break;
                        case EnumSkillType.DSM04:
                            defaultName = "诅咒" + defaultName;
                            break;
                    }
                    break;
            }
        }
        //4阶段
        else if (level == 4)
        {
            switch (enumSkillType)
            {
                case EnumSkillType.ZHS09://精灵呼唤
                    switch (parent.parent.enumSkillType)
                    {
                        case EnumSkillType.YSX01://火元素
                            defaultName += "_火精灵呼唤";
                            break;
                        case EnumSkillType.YSX02://水元素
                            defaultName += "_水精灵呼唤";
                            break;
                        case EnumSkillType.YSX03://土元素
                            defaultName += "_土精灵呼唤";
                            break;
                        case EnumSkillType.YSX04://风元素
                            defaultName += "_风精灵呼唤";
                            break;
                    }
                    break;
                case EnumSkillType.DFS05://纯净元素
                    defaultName += "_风精灵呼唤";
                    break;
                case EnumSkillType.DFS06:
                    defaultName += "元素爆破";
                    break;
                case EnumSkillType.DFS04://神速咏唱
                    defaultName += "神速";
                    break;
            }
        }
        if (tempList.Count > 0 && nextNodes.Count > 0)
        {
            defaultName = nextNodes.Select(temp => temp.GetCombineSkillName(tempList, defaultName)).Where(temp => !string.IsNullOrEmpty(temp)).FirstOrDefault();
        }
        return defaultName;
    }
}

/// <summary>
/// 技能融合工具类
/// </summary>
public static class SkillCombineStaticTools
{
    /// <summary>
    /// 组合的值对应粒子名配置文件路径
    /// </summary>
    public static string combineSkillKeyToParticalNameFilePath = "Data/Skill/CombinePartical/CombinePartical";

    /// <summary>
    /// 组合技能的值对应粒子名字典
    /// </summary>
    private static Dictionary<int, string> combineSkillKeyToParticalNameDic;

    /// <summary>
    /// 组合技能对应图片的缓存字典
    /// </summary>
    private static Dictionary<int, Sprite> combineSpriteDic;

    /// <summary>
    /// 可以使用的组合技能集合
    /// 注意一阶段的技能也是组合技能,也是从1亿开始的数字
    /// </summary>
    private static List<SkillCombineNextNodeStruct> canUseCombineSkillList;

    static SkillCombineStaticTools()
    {
        combineSpriteDic = new Dictionary<int, Sprite>();
        //设置可以使用的组合技能
        canUseCombineSkillList = new List<SkillCombineNextNodeStruct>();
        //添加一阶段(后续阶段自动添加)
        EnumSkillType[] level_SkillTypes = Enum.GetValues(typeof(EnumSkillType))
                .OfType<EnumSkillType>()
                .Where(temp => temp > EnumSkillType.MagicCombinedLevel1Start && temp < EnumSkillType.MagicCombinedLevel1End)
                .ToArray();
        foreach (EnumSkillType enumSkillType in level_SkillTypes)
        {
            SkillCombineNextNodeStruct skillCombineNExtNodeStruct = new SkillCombineNextNodeStruct(enumSkillType, null);
            canUseCombineSkillList.Add(skillCombineNExtNodeStruct);
            skillCombineNExtNodeStruct.SetNext();
        }
        //添加组合技能值对应的粒子名
        try
        {
            TextAsset textAsset = Resources.Load<TextAsset>(combineSkillKeyToParticalNameFilePath);
            string assetText = Encoding.UTF8.GetString(textAsset.bytes);
            combineSkillKeyToParticalNameDic = JsonConvert.DeserializeObject<Dictionary<int, string>>(assetText, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        }
        catch
        {
            combineSkillKeyToParticalNameDic = new Dictionary<int, string>();
        }
    }

    /// <summary>
    /// 获取组合技能的技能图片
    /// </summary>
    /// <param name="skillStructData">技能元数据对象</param>
    /// <param name="key">技能id</param>
    /// <returns></returns>
    public static Sprite GetCombineSkillSprite(SkillStructData skillStructData, int key)
    {
        if (combineSpriteDic.ContainsKey(key))
            return combineSpriteDic[key];
        EnumSkillType[] skills = SkillCombineStaticTools.GetCombineSkills(key);
        SkillBaseStruct[] thisUsedSkills = skillStructData.SearchSkillDatas(temp => skills.Contains(temp.skillType));
        Sprite[] sprites = thisUsedSkills.Select(temp => temp.skillSprite).ToArray();
        if (sprites == null || sprites.Length == 0)
            return null;
        var sizes = sprites.Select(temp => new { width = temp.textureRect.width , height = temp.textureRect.height });
        int width = (int)sizes.OrderBy(temp => temp.width).FirstOrDefault().width;
        int height = (int)sizes.OrderBy(temp => temp.height).FirstOrDefault().height;
        if (width == 0 || height == 0)
            return null;
        //进行组合
        if (sprites.Length == 1)//只有一个技能表示该技能本身
        {
            combineSpriteDic.Add(key, sprites[0]);
            return combineSpriteDic[key];
        }
        Texture2D texture2D = new Texture2D(width, height);
        if (sprites.Length == 2)//两个技能左右分开
        {

            //第一个技能
            Sprite firstSprite = sprites[0];
            Rect firstRect = firstSprite.rect;
            Color[] firstColors = firstSprite.texture.GetPixels((int)firstRect.x, (int)firstRect.y, (int)firstRect.width, (int)firstRect.height);
            Color[] firstEndColors = firstColors.CompressionColors((int)firstRect.width, (int)firstRect.height, width, height / 2);
            texture2D.SetPixels(0, 0, width, height / 2, firstEndColors);
            //第二个技能
            Sprite secondSprite = sprites[1];
            Rect secondRect = secondSprite.rect;
            Color[] secondColors = secondSprite.texture.GetPixels((int)secondRect.x, (int)secondRect.y, (int)secondRect.width, (int)secondRect.height);
            Color[] secondEndColors = secondColors.CompressionColors((int)secondRect.width, (int)secondRect.height, width, height / 2);
            texture2D.SetPixels(0, height / 2, width, height / 2, secondEndColors);
        }
        else if (sprites.Length >= 3)//三个技能或四个技能则分成四块分布
        {
            //第一个技能
            Sprite firstSprite = sprites[0];
            Rect firstRect = firstSprite.rect;
            Color[] firstColors = firstSprite.texture.GetPixels((int)firstRect.x, (int)firstRect.y, (int)firstRect.width, (int)firstRect.height);
            Color[] firstEndColors = firstColors.CompressionColors((int)firstRect.width, (int)firstRect.height, width / 2, height / 2);
            texture2D.SetPixels(0, 0, width / 2, height / 2, firstEndColors);
            //第二个技能
            Sprite secondSprite = sprites[0];
            Rect secondRect = secondSprite.rect;
            Color[] secondColors = secondSprite.texture.GetPixels((int)secondRect.x, (int)secondRect.y, (int)secondRect.width, (int)secondRect.height);
            Color[] secondEndColors = secondColors.CompressionColors((int)secondRect.width, (int)secondRect.height, width / 2, height / 2);
            texture2D.SetPixels(width / 2, height / 2, width / 2, height / 2, secondEndColors);
            //第三个技能
            Sprite thirdSprite = sprites[0];
            Rect thirdRect = thirdSprite.rect;
            Color[] thirdColors = thirdSprite.texture.GetPixels((int)thirdRect.x, (int)thirdRect.y, (int)thirdRect.width, (int)thirdRect.height);
            Color[] thirdEndColors = thirdColors.CompressionColors((int)thirdRect.width, (int)thirdRect.height, width / 2, height / 2);
            texture2D.SetPixels(0, height / 2, width / 2, height / 2, thirdEndColors);
            //第四个技能
            if (sprites.Length >= 4)
            {
                Sprite fourthSprite = sprites[0];
                Rect fourthRect = fourthSprite.rect;
                Color[] fourthColors = fourthSprite.texture.GetPixels((int)fourthRect.x, (int)fourthRect.y, (int)fourthRect.width, (int)fourthRect.height);
                Color[] fourthEndColors = fourthColors.CompressionColors((int)fourthRect.width, (int)fourthRect.height, width / 2, height / 2);
                texture2D.SetPixels(width / 2, 0, width / 2, height / 2, fourthEndColors);
            }
        }
        texture2D.Apply();
        Sprite createSprite = Sprite.Create(texture2D, new Rect(0, 0, width, height), Vector2.zero);
        combineSpriteDic.Add(key, createSprite);
        return combineSpriteDic[key];
    }

    /// <summary>
    /// 转换压缩图片
    /// </summary>
    /// <param name="sourceColors"></param>
    /// <param name="sourceWidth"></param>
    /// <param name="sourceHeight"></param>
    /// <param name="targetWidth"></param>
    /// <param name="targetHeight"></param>
    /// <returns></returns>
    public static Color[] CompressionColors(this Color[] sourceColors, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
    {
        Color[,] tempSourceColors = new Color[sourceWidth, sourceHeight];
        for (int i = 0; i < sourceWidth; i++)
        {
            for (int j = 0; j < sourceHeight; j++)
            {
                tempSourceColors[sourceWidth, sourceHeight] = sourceColors[j * sourceWidth + i];
            }
        }
        Color[,] endSourceColors = new Color[targetWidth, targetHeight];
        float biliWidth = (float)sourceWidth / (float)targetWidth;
        float biliHeight = (float)sourceHeight / (float)targetHeight;
        for (int i = 0; i < targetWidth; i++)
        {
            int indexWidth = (int)(biliWidth * i);
            indexWidth = Mathf.Clamp(indexWidth, 0, sourceWidth - 1);
            for (int j = 0; j < targetHeight; j++)
            {
                int indexHeight = (int)(biliWidth * i);
                indexHeight = Mathf.Clamp(indexHeight, 0, sourceHeight - 1);
                endSourceColors[i, j] = tempSourceColors[indexWidth, indexHeight];
            }
        }
        Color[] resultColors = new Color[targetWidth * targetHeight];
        for (int i = 0; i < targetWidth; i++)
        {
            for (int j = 0; j < targetHeight; j++)
            {
                resultColors[targetWidth * j + i] = endSourceColors[i, j];
            }
        }
        return resultColors;
    }

    /// <summary>
    /// 获取技能的组合值
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public static int GetCombineSkillKey(IEnumerable<EnumSkillType> skills)
    {
        int[] transDataArray = new int[4];
        EnumSkillType[] enumSkillTypes = skills.ToArray();
        for (int i = 0; i < 4; i++)
        {
            if (i < skills.Count())
            {
                EnumSkillType enumSkillType = enumSkillTypes[i];
                int value = (int)enumSkillType;
                int index = -1;
                if (enumSkillType > EnumSkillType.MagicCombinedLevel1Start && enumSkillType < EnumSkillType.MagicCombinedLevel1End)
                {
                    index = 0;
                    value = value - (int)EnumSkillType.MagicCombinedLevel1Start;
                }
                else if (enumSkillType > EnumSkillType.MagicCombinedLevel2Start && enumSkillType < EnumSkillType.MagicCombinedLevel2End)
                {
                    index = 1;
                    value = value - (int)EnumSkillType.MagicCombinedLevel2Start;
                    value *= (int)Mathf.Pow(100, 1);
                }
                else if (enumSkillType > EnumSkillType.MagicCombinedLevel3Start && enumSkillType < EnumSkillType.MagicCombinedLevel3End)
                {
                    index = 2;
                    value = value - (int)EnumSkillType.MagicCombinedLevel3Start;
                    value *= (int)Mathf.Pow(100, 2);
                }
                else if (enumSkillType > EnumSkillType.MagicCombinedLevel4Start && enumSkillType < EnumSkillType.MagicCombinedLevel4End)
                {
                    index = 3;
                    value = value - (int)EnumSkillType.MagicCombinedLevel4Start;
                    value *= (int)Mathf.Pow(100, 3);
                }
                else if (enumSkillType != EnumSkillType.None)
                {
                    return value;
                }
                if (index > -1)
                    transDataArray[index] = value;
            }
        }
        int result = transDataArray.Sum() + (int)EnumSkillType.MagicCombinedStart;
        return result;
    }

    /// <summary>
    /// 获取技能的组合值
    /// </summary>
    /// <param name="skillBaseStructs"></param>
    /// <returns></returns>
    public static int GetCombineSkillKey(IEnumerable<SkillBaseStruct> skillBaseStructs)
    {
        return GetCombineSkillKey(skillBaseStructs.Where(temp => temp != null).Select(temp => temp.skillType));
    }

    /// <summary>
    /// 获取组合值组合的技能
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static EnumSkillType[] GetCombineSkills(int key)
    {
        if (key < (int)EnumSkillType.MagicCombinedStart)
        {
            return new EnumSkillType[] { (EnumSkillType)key };
        }
        int level1 = key % 100; key /= 100;
        int level2 = key % 100; key /= 100;
        int level3 = key % 100; key /= 100;
        int level4 = key % 100;
        List<EnumSkillType> resultList = new List<EnumSkillType>();
        if (level1 != 0)
            resultList.Add((EnumSkillType)(level1 + (int)EnumSkillType.MagicCombinedLevel1Start));
        if (level2 != 0)
            resultList.Add((EnumSkillType)(level2 + (int)EnumSkillType.MagicCombinedLevel2Start));
        if (level3 != 0)
            resultList.Add((EnumSkillType)(level3 + (int)EnumSkillType.MagicCombinedLevel3Start));
        if (level4 != 0)
            resultList.Add((EnumSkillType)(level4 + (int)EnumSkillType.MagicCombinedLevel4Start));
        return resultList.ToArray();
    }

    /// <summary>
    /// 获取是否可以组合技能
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public static bool GetCanCombineSkills(params EnumSkillType[] skills)
    {
        List<EnumSkillType> skillList = skills.Where(temp => temp > EnumSkillType.MagicCombinedLevel1Start && temp < EnumSkillType.MagicCombinedLevel4End).ToList();
        int rightCount = canUseCombineSkillList.Select(temp => temp.ThisIsACombineSkills(skillList)).Count(temp => temp);
        return rightCount > 0;
    }

    /// <summary>
    /// 通过组合技能的阶段来获取所有该阶段的技能
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static EnumSkillType[] GetBaseSkillBackCombineSkillIndex(int index)
    {
        switch (index)
        {
            case 1:
                return Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Where(temp => temp > EnumSkillType.MagicCombinedLevel1Start && temp < EnumSkillType.MagicCombinedLevel1End).ToArray();
            case 2:
                return Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Where(temp => temp > EnumSkillType.MagicCombinedLevel2Start && temp < EnumSkillType.MagicCombinedLevel2End).ToArray();
            case 3:
                return Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Where(temp => temp > EnumSkillType.MagicCombinedLevel3Start && temp < EnumSkillType.MagicCombinedLevel3End).ToArray();
            case 4:
                return Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Where(temp => temp > EnumSkillType.MagicCombinedLevel4Start && temp < EnumSkillType.MagicCombinedLevel4End).ToArray();
            default:
                return new EnumSkillType[0];
        }
    }

    /// <summary>
    /// 通过组合技能数组来获取该组合的技能名
    /// </summary>
    /// <param name="skillBaseStructs"></param>
    /// <returns></returns>
    public static string GetCombineSkillsName(IEnumerable<SkillBaseStruct> skillBaseStructs)
    {
        if (skillBaseStructs.Count() == 1)
        {
            SkillBaseStruct skillBaseStruct = skillBaseStructs.FirstOrDefault();
            if (skillBaseStruct != null)
            {
                if (skillBaseStruct.skillType < EnumSkillType.MagicStart)
                {
                    return skillBaseStruct.skillName;
                }
            }
        }
        List<KeyValuePair<EnumSkillType, string>> tempList = skillBaseStructs.Select(temp => new KeyValuePair<EnumSkillType, string>(temp.skillType, temp.skillName)).ToList();
        string combineSkillName = canUseCombineSkillList.Select(temp => temp.GetCombineSkillName(tempList, "")).Where(temp => !string.IsNullOrEmpty(temp)).FirstOrDefault();
        return combineSkillName == null ? "" : combineSkillName;
    }

    /// <summary>
    /// 通过组合技能类型来获取该组合的技能名
    /// </summary>
    /// <param name="enumSkillTypes"></param>
    /// <returns></returns>
    public static string GetCombineSkillsName(IEnumerable<EnumSkillType> enumSkillTypes)
    {
        SkillStructData skillStructData_Base = DataCenter.Instance.GetMetaData<SkillStructData>();
        if (skillStructData_Base != null)
        {
            SkillBaseStruct[] thisUsedSkills = skillStructData_Base.SearchSkillDatas(temp => enumSkillTypes.Contains(temp.skillType));
            return GetCombineSkillsName(thisUsedSkills);
        }
        return "";
    }

    /// <summary>
    /// 获取单一技能的名字(反射枚举获取)
    /// </summary>
    /// <param name="skillBaseStruct"></param>
    /// <returns></returns>
    public static string GetSingleSkillName(SkillBaseStruct skillBaseStruct)
    {
        if (skillBaseStruct == null)
            return "空";
        EnumSkillType enumSkillType = skillBaseStruct.skillType;
        Type t = typeof(EnumSkillType);
        FieldInfo fieldInfo = t.GetField(enumSkillType.ToString());
        if (fieldInfo != null)
        {
            FieldExplanAttribute fieldExplan = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplan != null)
            {
                return fieldExplan.GetExplan();
            }
        }
        return "空";
    }

    /// <summary>
    /// 获取单一技能的名字(反射枚举获取)
    /// </summary>
    /// <param name="enumSkillType"></param>
    /// <returns></returns>
    public static string GetSingleSkillName(EnumSkillType enumSkillType)
    {
        SkillStructData skillStructData_Base = DataCenter.Instance.GetMetaData<SkillStructData>();
        if (skillStructData_Base != null)
        {
            SkillBaseStruct[] thisUsedSkills = skillStructData_Base.SearchSkillDatas(temp => enumSkillType == temp.skillType);
            return GetSingleSkillName(thisUsedSkills.FirstOrDefault());
        }
        return "空";
    }

    /// <summary>
    /// 获取组合值组合技能的粒子
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static GameObject GetCombineSkillsPartical(int key)
    {
        EnumSkillType[] enumSkillTypes = GetCombineSkills(key);
        if (combineSkillKeyToParticalNameDic != null && combineSkillKeyToParticalNameDic.ContainsKey(key))
        {
            string particalName = combineSkillKeyToParticalNameDic[key];
            return ParticalManager.GetPartical(particalName);
        }
        return null;
    }
}
