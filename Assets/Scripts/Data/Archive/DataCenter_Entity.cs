using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public partial class DataCenter
{
    /// <summary>
    /// 玩家状态
    /// </summary>
    private PlayerState PlayerState;

    /// <summary>
    /// 商人的状态
    /// </summary>
    private BusinessmanStates BusinessmanStates;

    /// <summary>
    /// 任务进度
    /// </summary>
    private RuntimeTasksData RuntimeTasks;

    /// <summary>
    /// 按键映射数据
    /// </summary>
    private KeyContactData KeyConatactData;

    /// <summary>
    /// 游戏运行过的状态
    /// </summary>
    private GameRunnedState gameRunnedState;

    /// <summary>
    /// 新版任务进度
    /// </summary>
    private TaskMap.RunTimeTaskData RuntimeTaskMap;

    /// <summary>
    /// 游戏运行时的状态数据
    /// </summary>
    private GameRunningStateData GameRunningStaetData;

    public DataCenter()
    {
        PlayerState = new PlayerState();
        KeyConatactData = new KeyContactData();
        RuntimeTasks = new RuntimeTasksData();
        gameRunnedState = new GameRunnedState();
        RuntimeTaskMap = new TaskMap.RunTimeTaskData();
        BusinessmanStates = new BusinessmanStates();
        GameRunningStaetData = new GameRunningStateData();
    }


}

/// <summary>
/// 游戏运行过的状态
/// </summary>
public class GameRunnedState
{
    /// <summary>
    /// 启动次数
    /// 从1-42949之间
    /// </summary>
    public int StartTimes;
}

/// <summary>
/// 玩家状态
/// </summary>
public class PlayerState
{
    /// <summary>
    /// 玩家所在的场景
    /// </summary>
    public string Scene;

    /// <summary>
    /// 玩家的位置
    /// </summary>
    public Vector3 Location;

    /// <summary>
    /// 角色名字
    /// </summary>
    public string PlayerName;

    /// <summary>
    /// 最近一次点击路牌的ID
    /// </summary>
    public int StreetID;

    /// <summary>
    /// 最忌一次点击路牌时所在的场景
    /// </summary>
    public string StreetScene;

    /// <summary>
    /// 人物等级
    /// </summary>
    public int Level;

    /// <summary>
    /// 当前的经验值
    /// </summary>
    public int Experience;

    /// <summary>
    /// 钱
    /// </summary>
    public int Sprice;

    /// <summary>
    /// 技能点
    /// </summary>
    public int FreedomPoint;

    /// <summary>
    /// 技能点
    /// </summary>
    public Dictionary<EnumSkillType, int> SkillPoint;


    /// <summary>
    /// 组合技能的数据
    /// </summary>
    public List<EnumSkillType[]> CombineSkills;


    /// <summary>
    /// 种族路线
    /// </summary>
    public List<RoleOfRace> RoleOfRaceRoute;

    /// <summary>
    /// 属性点
    /// </summary>
    public int PropertyPoint;

    /// <summary>
    /// 力量
    /// </summary>
    public int Strength;

    /// <summary>
    /// 精神
    /// </summary>
    public int Spirit;

    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility;

    /// <summary>
    /// 声望
    /// </summary>
    public int Reputation;

    /// <summary>
    /// 场景与对应地图的遮罩数据字典
    /// </summary>
    public Dictionary<string, byte[,]> SceneMapMaskDataDic;

    /// <summary>
    /// 场景与对应地图的遮罩图片信息字典
    /// </summary>
    [JsonIgnore]
    private Dictionary<string, Sprite> sceneMapMaskSpriteDic;

    /// <summary>
    /// 玩家所有物品
    /// </summary>
    public List<PlayGoods> PlayerAllGoods;

    /// <summary>
    /// 可以显示的词条集合
    /// </summary>
    public List<int> EntryEnableList;

    /// <summary>
    /// 玩家的行动路线
    /// </summary>
    public List<Vector2> movePathList;

    public PlayerState()
    {
        SkillPoint = new Dictionary<EnumSkillType, int>();
        RoleOfRaceRoute = new List<RoleOfRace>();
        PlayerAllGoods = new List<PlayGoods>();
        CombineSkills = new List<EnumSkillType[]>();
        EntryEnableList = new List<int>();
        movePathList = new List<Vector2>();
    }

    /// <summary>
    /// 获取指定场景的地图遮罩
    /// </summary>
    /// <param name="sceneName">指定的场景名</param>
    /// <param name="targetSceneMapSprite">指定场景的地图精灵</param>
    /// <returns></returns>
    public Sprite GetSceneMapMaskSprite(string sceneName, Sprite targetSceneMapSprite)
    {
        if (SceneMapMaskDataDic == null)
            SceneMapMaskDataDic = new Dictionary<string, byte[,]>();
        if (!SceneMapMaskDataDic.ContainsKey(sceneName))
        {
            byte[,] maskDataArray = new byte[(int)targetSceneMapSprite.rect.width, (int)targetSceneMapSprite.rect.height];
            SceneMapMaskDataDic.Add(sceneName, maskDataArray);
            for (int i = 0; i < maskDataArray.GetLength(0); i++)
            {
                for (int j = 0; j < maskDataArray.GetLength(1); j++)
                {
                    maskDataArray[i, j] = 255;
                }
            }
        }
        if (sceneMapMaskSpriteDic == null)
            sceneMapMaskSpriteDic = new Dictionary<string, Sprite>();
        if (!sceneMapMaskSpriteDic.ContainsKey(sceneName))
        {
            byte[,] maskDatArray = SceneMapMaskDataDic[sceneName];
            Texture2D texture2D = new Texture2D(maskDatArray.GetLength(0), maskDatArray.GetLength(1));
            Color[] colors = new Color[texture2D.width * texture2D.height];
            for (int i = 0; i < texture2D.width; i++)
            {
                for (int j = 0; j < texture2D.height; j++)
                {
                    float data = maskDatArray[i, j] / 255f;
                    colors[i * texture2D.height + j] = new Color(0, 0, 0, data);
                }
            }
            texture2D.SetPixels(colors);
            texture2D.Apply();
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            sceneMapMaskSpriteDic.Add(sceneName, sprite);
        }
        return sceneMapMaskSpriteDic[sceneName];
    }

    /// <summary>
    /// 保存指定场景的地图遮罩图片的数据
    /// </summary>
    /// <param name="sceneName">指定的场景</param>
    public void SaveGetSceneMapMaskData(string sceneName)
    {
        UpdateMapMaskData(sceneName);
        if (sceneMapMaskSpriteDic != null && sceneMapMaskSpriteDic.ContainsKey(sceneName))
        {
            Sprite sprite = sceneMapMaskSpriteDic[sceneName];
            Texture2D texture2D = sprite.texture;
            byte[,] mapDataArray = new byte[texture2D.width, texture2D.height];
            Color[] colors = texture2D.GetPixels();
            for (int i = 0; i < texture2D.width; i++)
            {
                for (int j = 0; j < texture2D.height; j++)
                {
                    Color color = colors[i * texture2D.height + j];
                    mapDataArray[i, j] = (byte)(color.a * 255);
                }
            }
            if (SceneMapMaskDataDic.ContainsKey(sceneName))
            {
                SceneMapMaskDataDic[sceneName] = mapDataArray;
            }
        }
    }

    /// <summary>
    /// 设置玩家的行动路线
    /// </summary>
    /// <param name="movePaths">玩家在地图上的坐标</param>
    public void SetPlayerMovePath(string sceneName, params Vector2[] movePaths)
    {
        foreach (Vector2 now in movePaths)
        {
            bool add = true;
            foreach (Vector2 item in movePathList)
            {
                if (Vector2.Distance(item, now) < 10f)
                {
                    add = false;
                    break;
                }
            }
            if (add)
            {
                movePathList.Add(now);
            }
        }
        if (movePathList.Count > 2)
            UpdateMapMaskData(sceneName);
    }

    /// <summary>
    /// 根据设置的玩家的行动路线更新遮罩地图
    /// </summary>
    public void UpdateMapMaskData(string sceneName)
    {
        if (!sceneMapMaskSpriteDic.ContainsKey(sceneName))
            return;

        if (movePathList.Count > 0)
        {
            Sprite thisSprite = sceneMapMaskSpriteDic[sceneName];
            Texture2D texture2D = thisSprite.texture;
            int width = texture2D.width;
            int height = texture2D.height;
            foreach (Vector2 movePath in movePathList)
            {
                int centerX = (int)movePath.x;
                int centerY = (int)movePath.y;
                Rect rectRange = new Rect(0, 0, 0, 0);
                rectRange.xMin = (centerX - 20) > 0 ? (centerX - 20) : 0;
                rectRange.yMin = (centerY - 20) > 0 ? (centerY - 20) : 0;
                rectRange.xMax = (centerX + 20) < width ? (centerX + 20) : (width - 1);
                rectRange.yMax = (centerY + 20) < height ? (centerY + 20) : (height - 1);
                Color[] colors = texture2D.GetPixels((int)rectRange.xMin, (int)rectRange.yMin, (int)rectRange.width, (int)rectRange.height);
                for (int i = centerX - 20; i < centerX + 20; i++)
                {
                    for (int j = centerY - 20; j < centerY + 20; j++)
                    {
                        float distance = Mathf.Pow(centerX - i, 2) + Mathf.Pow(centerY - j, 2);
                        if (i >= 0 && j >= 0 && i <= width && j <= height && distance < 400)
                        {
                            int index = (int)((j - rectRange.yMin) * rectRange.width + (i - rectRange.xMin));
                            float rade = 0;
                            if (distance > 50)
                            {
                                rade = 1- (400f - distance) / 350f;
                                rade = Mathf.Pow(rade, 0.5f);
                            }
                            rade = Mathf.Clamp(rade, 0, 1);
                            if (colors[index].a > rade)
                                colors[index] = new Color(0, 0, 0, rade);
                        }
                    }
                }
                texture2D.SetPixels((int)rectRange.xMin, (int)rectRange.yMin, (int)rectRange.width, (int)rectRange.height, colors);
            }
            texture2D.Apply();
        }
        movePathList.Clear();
    }
}

/// <summary>
/// 玩家的物品
/// </summary>
public class PlayGoods
{
    /// <summary>
    /// 物品的id(并非是物品的类型,是指具体物品的唯一标识)
    /// </summary>
    private int id;

    /// <summary>
    /// 物品信息
    /// </summary>
    public Goods GoodsInfo { get; set; }
    /// <summary>
    /// 物品所在位置
    /// </summary>
    public GoodsLocation GoodsLocation { get; set; }
    /// <summary>
    /// 主要适用于药品以及需要堆叠的物品(比如任务物品),当该项为0时,则需要销毁
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 物品的品质 
    /// </summary>
    public EnumQualityType QualityType { get; set; }

    /// <summary>
    /// 物品的id(并非是物品的类型,是指具体物品的唯一标识)
    /// </summary>
    public int ID { get { return id; } }

    #region 特殊的属性
    /// <summary>
    /// 这是左手还是右手武器 (主要是在类型为装备中的武器判断时使用)
    /// 左手true  右手false  null表示没有装备
    /// </summary>
    public bool? leftRightArms;

    #endregion

    public PlayGoods()
    {
        GoodsInfo = new Goods();
        GoodsLocation = GoodsLocation.None;
    }
    public PlayGoods(int id, Goods goodsInfo, GoodsLocation location)
    {
        this.id = id;
        this.GoodsInfo = goodsInfo;
        this.GoodsLocation = location;
    }

    [JsonIgnore]
    private Sprite cachedSprite;

    /// <summary>
    /// 获取物品的图标,此函数是假定我们的物品Sprite的名称和物品的名称是一致的
    /// </summary>
    /// <returns></returns>
    [JsonIgnore]
    public Sprite GetGoodsSprite
    {
        get
        {
            if (cachedSprite == null)
                cachedSprite = SpriteManager.GetSrpite(GoodsInfo.SpriteName); //Resources.Load<Sprite>(GoodsInfo.GoodsName);
            return cachedSprite;
        }
    }
}

/// <summary>
/// 物品位置
/// </summary>
public enum GoodsLocation
{
    /// <summary>
    /// 穿戴中
    /// </summary>
    Wearing,
    /// <summary>
    /// 包裹
    /// </summary>
    Package,
    /// <summary>
    /// 仓库
    /// </summary>
    warehouse,

    None,
}

/// <summary>
/// 商人的数据集合
/// </summary>
public class BusinessmanStates
{
    /// <summary>
    /// 商人集合
    /// </summary>
    public List<Businessman> BusinessmanList;

    public BusinessmanStates()
    {
        BusinessmanList = new List<Businessman>();
    }
}

/// <summary>
/// 商人的数据
/// </summary>
public class Businessman
{
    /// <summary>
    /// 商人的基础数据
    /// </summary>
    public BusinessmanDataInfo BusinessManDataInfo;

    /// <summary>
    /// 卖给商人并被商人保存的道具
    /// </summary>
    public List<PlayGoods> SellPropsList;

    /// <summary>
    /// 商人的基础道具
    /// </summary>
    public List<PlayGoods> BaseList;

    /// <summary>
    /// 商人的id
    /// </summary>
    public int BusinessmanID;

    /// <summary>
    /// 商人所在的场景
    /// </summary>
    public string BusinessmanScene;

    public Businessman()
    {
        SellPropsList = new List<PlayGoods>();
        BaseList = new List<PlayGoods>();
    }
}

/// <summary>
/// 游戏运行时的状态数据
/// </summary>
public class GameRunningStateData
{
    /// <summary>
    /// 大都图是否可用
    /// </summary>
    public bool CanBigMap;
}


