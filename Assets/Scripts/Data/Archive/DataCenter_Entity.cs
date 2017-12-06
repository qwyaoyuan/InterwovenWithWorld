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
    /// 任务进度
    /// </summary>
    private RuntimeTasksData RuntimeTasks;

    /// <summary>
    /// 按键映射数据
    /// </summary>
    private KeyContactData KeyConatactData;



    public DataCenter()
    {
        PlayerState = new PlayerState();
        KeyConatactData = new KeyContactData();
        RuntimeTasks = new RuntimeTasksData();
    }


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
    /// 人物等级
    /// </summary>
    public int Level;

    /// <summary>
    /// 当前的经验值
    /// </summary>
    public int Experience;

    /// <summary>
    /// 自由点
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
    /// 专注
    /// </summary>
    public int Concentration;

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

    public PlayerState()
    {
        SkillPoint = new Dictionary<EnumSkillType, int>();
        RoleOfRaceRoute = new List<RoleOfRace>();
        PlayerAllGoods = new List<PlayGoods>();
        CombineSkills = new List<EnumSkillType[]>();
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
                    float data = maskDatArray[i, j]/255f;
                    colors[i * texture2D.height + j] = new Color(data, data, data);
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
                    mapDataArray[i, j] = (byte)(color.r * 255);
                }
            }
            if (SceneMapMaskDataDic.ContainsKey(sceneName))
            {
                SceneMapMaskDataDic[sceneName] = mapDataArray;
            }
        }
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
    public Sprite GetGoodsSprite()
    {
        if (cachedSprite == null)
            cachedSprite = Resources.Load<Sprite>(GoodsInfo.GoodsName);
        return cachedSprite;

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




