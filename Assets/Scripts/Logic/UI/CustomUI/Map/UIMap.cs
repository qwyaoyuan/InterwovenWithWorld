using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Map控件
/// 自定义控件
/// </summary>
public class UIMap : MonoBehaviour
{
    /// <summary>
    /// 地图显示图片的控件
    /// </summary>
    [SerializeField]
    private Image mapImage;

    /// <summary>
    /// 地图遮罩图片的控件
    /// </summary>
    [SerializeField]
    private Image maskImage;

    /// <summary>
    /// 地图手柄控件
    /// </summary>
    [SerializeField]
    private Image mapHandle;

    /// <summary>
    /// 地图图标结构集合
    /// </summary>
    private List<UIMapIconStruct> uiImapIconStructList;

    /// <summary>
    /// 场景的大小
    /// </summary>
    Rect sceneRect;
    /// <summary>
    /// 吸附的距离
    /// </summary>
    int stickOnPixel;
    /// <summary>
    /// 当前的缩放比率
    /// </summary>
    float scale;
    /// <summary>
    /// 最小的缩放比率
    /// </summary>
    float minScale = 0.05f;
    /// <summary>
    /// 最大的缩放比率
    /// </summary>
    readonly float maxScale = 1;
    /// <summary>
    /// 手柄触发的UI对象集合
    /// </summary>
    private List<GameObject> handleTriggerGameObjectList;

    /// <summary>
    /// 移动手柄状态
    /// </summary>
    private bool isMoveHandleState = false;
    /// <summary>
    /// 距离最近一次移动手柄的时间 
    /// </summary>
    private float lastMoveHandleTime = 0;
    /// <summary>
    /// 等待手柄不移动时常超过该值时吸附
    /// </summary>
    [SerializeField]
    private float WaitMoveHandleTimeStickOn = 0.2f;
    /// <summary>
    /// 碰触的图标
    /// </summary>
    private GameObject touchIconObj;
    /// <summary>
    /// 在地图上点击了鼠标
    /// </summary>
    public event Action<Vector2> ClickOnMap;

    private void Awake()
    {
        if (mapHandle)
        {
            MapHandleColliderCheck mapHandleColliderCheck = mapHandle.gameObject.AddComponent<MapHandleColliderCheck>();
            mapHandleColliderCheck.TriggerEnterHandle += UIMap_TriggerEnterHandle;
            mapHandleColliderCheck.TriggerExitHandle += MapHandleColliderCheck_TriggerExitHandle;
        }
        if (mapImage)
        {
            //添加点击事件
            EventTrigger eventTrigger = mapImage.gameObject.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(MapImage_Click);
            eventTrigger.triggers.Add(entry);
        }
        Init();
    }

    /// <summary>
    /// 是否使用手柄
    /// </summary>
    public bool UseHandle
    {
        get
        {
            return mapHandle.gameObject.activeSelf;
        }
        set
        {
            mapHandle.gameObject.SetActive(value);
            if (!value)
            {
                handleTriggerGameObjectList.Clear();
            }
        }
    }

    /// <summary>
    /// 检测手柄触发了离开事件
    /// </summary>
    /// <param name="obj"></param>
    private void MapHandleColliderCheck_TriggerExitHandle(Collider2D obj)
    {
        if (handleTriggerGameObjectList != null)
        {
            if (handleTriggerGameObjectList.Contains(obj.gameObject))
            {
                handleTriggerGameObjectList.Remove(obj.gameObject);
            }
        }
    }

    /// <summary>
    /// 检测手柄触发了进入事件
    /// </summary>
    /// <param name="obj"></param>
    private void UIMap_TriggerEnterHandle(Collider2D obj)
    {
        if (handleTriggerGameObjectList != null)
        {
            if (!handleTriggerGameObjectList.Contains(obj.gameObject))
            {
                handleTriggerGameObjectList.Add(obj.gameObject);
            }
        }
    }

    /*
    #region 测试代码
    private void Start()
    {
        InitMap(mapImage.sprite, new Rect(100, 100, 100, 100), 0.2f);
        tempStruct = AddIcon(mapImage.sprite, new Vector2(20, 20), new Vector2(130, 130));
        ClickOnMap += UIMap_ClickOnMap;
    }

    private void UIMap_ClickOnMap(Vector2 obj)
    {
        Vector2 terrainPos = obj;
        Vector2 scenePos = TranslatePosTerrainToScene(terrainPos);
        Debug.Log(terrainPos+" "+ scenePos);

        UIMapIconStruct uiMapIconStruct = GetTouchOrClickIcon();
        if (!uiMapIconStruct)
            if (!tempStruct1)
                tempStruct1 = AddIcon(mapImage.sprite, new Vector2(20, 20), obj);
            else
            {
                SetIconPos(tempStruct1, obj);
            }
    }

    UIMapIconStruct tempStruct;
    UIMapIconStruct tempStruct1;
    private void Update()
    {
        //缩放
        float add = Input.GetAxis("Mouse ScrollWheel") / 20;
        Scale += add;
        //移动
        Vector2 moveDelat = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            moveDelat.x -= 10;
        if (Input.GetKey(KeyCode.D))
            moveDelat.x += 10;
        if (Input.GetKey(KeyCode.W))
            moveDelat.y += 10;
        if (Input.GetKey(KeyCode.S))
            moveDelat.y -= 10;
        MoveHandle(moveDelat);
        //定位
        if (Input.GetKey(KeyCode.Return))
        {
            MoveToTerrainPoint(new Vector2(200, 200));
        }
        //定位到图标
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            MoveToIconStruct(tempStruct);
        }
    }
    #endregion
    */

    /// <summary>
    /// 用于处理手柄靠近后的吸附效果
    /// </summary>
    private void LateUpdate()
    {
        if (UseHandle && handleTriggerGameObjectList != null && handleTriggerGameObjectList.Count > 0)
        {
            lastMoveHandleTime += Time.deltaTime;
            if (lastMoveHandleTime > WaitMoveHandleTimeStickOn && isMoveHandleState)
            {
                isMoveHandleState = false;
                //计算手柄应该移动到的位置
                UIMapIconStruct uiMapIconStruct = NearIconOfHandle();
                if (uiMapIconStruct != null)
                {
                    Vector2 iconPos = uiMapIconStruct.GetComponent<RectTransform>().position;
                    Vector2 handlePos = mapHandle.GetComponent<RectTransform>().position;
                    if (Vector2.Distance(iconPos, handlePos) < stickOnPixel)
                    {
                        Vector2 offset = iconPos - handlePos;
                        MoveHandleInner(offset);
                        touchIconObj = uiMapIconStruct.gameObject;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Init()
    {
        handleTriggerGameObjectList = new List<GameObject>();
        if (mapImage != null)
        {
            int childsCount = mapImage.transform.childCount;
            if (childsCount > 0)
            {
                Transform[] childTranses = Enumerable.Range(0, childsCount).Select(temp => mapImage.transform.GetChild(temp)).ToArray();
                foreach (Transform childTrans in childTranses)
                {
                    Destroy(childTrans.gameObject);
                }
            }
            //    for (int i = childsCount - 1; i >= 0; i++)
            //{
            //    Transform child = mapImage.transform.GetChild(i);
            //    DestroyImmediate(child.gameObject);
            //}
        }
        uiImapIconStructList = new List<UIMapIconStruct>();
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    /// <param name="mapSprite">背景地图的图片</param>
    /// <param name="maskSprite">地图遮罩的图片</param>
    /// <param name="sceneRect">该地图表示场景的大小</param>
    /// <param name="stickOnPixel">操控手柄吸附距离</param>
    /// <param name="scale">缩放比例</param>
    /// <param name="minScale">最小的缩放比例</param>
    public void InitMap(Sprite mapSprite,Sprite maskSprite, Rect sceneRect, float scale = 0.2f, float minScale = 0.05f, int stickOnPixel = 50)
    {
        RectTransform mapRectTrans = GetComponent<RectTransform>();
        float mapWidth = mapRectTrans.rect.width;
        float mapHeight = mapRectTrans.rect.height;
        Init();
        if (mapImage)
        {
            mapImage.sprite = mapSprite;
            float mapWHR = mapWidth / mapHeight;
            float spriteWidth = mapSprite.rect.width;
            float spriteHeight = mapSprite.rect.height;
            float spriteWHR = spriteWidth / spriteHeight;
            if (mapWHR > spriteWHR)//依照宽度为基准
            {
                spriteWidth = mapWidth;
                spriteHeight = spriteWidth / spriteWHR;
            }
            else//依照高度为基准
            {
                spriteHeight = mapHeight;
                spriteWidth = spriteHeight * spriteWHR;
            }
            mapImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            mapImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            mapImage.rectTransform.offsetMin = new Vector2(-spriteWidth / 2, -spriteHeight / 2);
            mapImage.rectTransform.offsetMax = new Vector2(spriteWidth / 2, spriteHeight / 2);
            this.minScale = minScale >= 0.01f && minScale < 0.9f ? minScale : 0.01f;
            this.scale = 1;
            Scale = scale;
        }
        if (maskSprite)
        {
            maskImage.sprite = maskSprite;
            maskImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            maskImage.color = new Color(1, 1, 1, 0);
        }
        this.sceneRect = sceneRect;
        this.stickOnPixel = stickOnPixel;
        if (mapHandle)
        {
            CircleCollider2D circleCollider2D = mapHandle.GetComponent<CircleCollider2D>();
            circleCollider2D.radius = stickOnPixel / 2;
            float mapHandleWidht = mapHandle.rectTransform.rect.width;
            float mapHandleHeight = mapHandle.rectTransform.rect.height;
            mapHandle.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            mapHandle.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            mapHandle.rectTransform.offsetMin = new Vector2(-mapHandleWidht / 2, -mapHandleHeight / 2);
            mapHandle.rectTransform.offsetMax = new Vector2(mapHandleWidht / 2, mapHandleHeight / 2);
        }
    }

    /// <summary>
    /// 设置或获取缩放比例
    /// </summary>
    public float Scale
    {
        get
        {
            return scale;
        }
        set
        {
            if (value <= maxScale && value >= minScale)
                if (mapImage)
                {
                    float factor = scale / value;
                    Vector2 offsetMin = mapImage.rectTransform.offsetMin;
                    Vector2 offsetMax = mapImage.rectTransform.offsetMax;
                    offsetMin *= factor;
                    offsetMax *= factor;
                    MoveBackMap(offsetMin, offsetMax);
                    scale = value;
                }
        }
    }

    /// <summary>
    /// 移动背景地图
    /// </summary>
    /// <param name="offsetMin"></param>
    /// <param name="offsetMax"></param>
    private void MoveBackMap(Vector2 offsetMin, Vector2 offsetMax)
    {
        if (mapImage)
        {
            RectTransform mapRectTrans = GetComponent<RectTransform>();
            float mapWidth = mapRectTrans.rect.width;
            float mapHeight = mapRectTrans.rect.height;
            if (offsetMin.x > -mapWidth / 2)
            {
                float offset = offsetMin.x - (-mapWidth / 2);
                offsetMin.x -= offset;
                offsetMax.x -= offset;
            }
            else if (offsetMax.x < mapWidth / 2)
            {
                float offset = mapWidth / 2 - offsetMax.x;
                offsetMin.x += offset;
                offsetMax.x += offset;
            }
            if (offsetMin.y > -mapHeight / 2)
            {
                float offset = offsetMin.y - (-mapHeight / 2);
                offsetMin.y -= offset;
                offsetMax.y -= offset;
            }
            else if (offsetMax.y < mapHeight / 2)
            {
                float offset = mapHeight / 2 - offsetMax.y;
                offsetMin.y += offset;
                offsetMax.y += offset;
            }
            mapImage.rectTransform.offsetMin = offsetMin;
            mapImage.rectTransform.offsetMax = offsetMax;
        }
    }

    /// <summary>
    /// 移动手柄
    /// </summary>
    /// <param name="moveDelta"></param>
    public void MoveHandle(Vector2 moveDelta)
    {
        if (UseHandle && !moveDelta.Equals(Vector2.zero))
        {
            isMoveHandleState = true;
            lastMoveHandleTime = 0;
            touchIconObj = null;
            MoveHandleInner(moveDelta);
        }
    }

    /// <summary>
    /// 内部的移动手柄
    /// </summary>
    /// <param name="moveDelta">移动的像素</param>
    private void MoveHandleInner(Vector2 moveDelta)
    {
        RectTransform mapRectTrans = GetComponent<RectTransform>();
        float mapWidth = mapRectTrans.rect.width;
        float mapHeight = mapRectTrans.rect.height;
        if (mapHandle)
        {
            float mapHandleWidth = mapHandle.rectTransform.rect.width;
            float mapHandleHeight = mapHandle.rectTransform.rect.height;
            Vector2 mapHandleOffsetMin = mapHandle.rectTransform.offsetMin;
            Vector2 mapHandleOffsetMax = mapHandle.rectTransform.offsetMax;
            mapHandleOffsetMin += moveDelta;
            mapHandleOffsetMax += moveDelta;
            Vector2 mapMoveDelta = Vector2.zero;
            if (mapHandleOffsetMin.x < -mapWidth / 2)
            {
                mapMoveDelta.x = (-mapWidth / 2) - mapHandleOffsetMin.x;
                mapHandleOffsetMin.x = -mapWidth / 2;
                mapHandleOffsetMax.x = mapHandleOffsetMin.x + mapHandleWidth;
            }
            else if (mapHandleOffsetMax.x > mapWidth / 2)
            {
                mapMoveDelta.x = -(mapHandleOffsetMax.x - mapWidth / 2);
                mapHandleOffsetMax.x = mapWidth / 2;
                mapHandleOffsetMin.x = mapHandleOffsetMax.x - mapHandleWidth;
            }
            if (mapHandleOffsetMin.y < -mapHeight / 2)
            {
                mapMoveDelta.y = (-mapHeight / 2) - mapHandleOffsetMin.y;
                mapHandleOffsetMin.y = -mapHeight / 2;
                mapHandleOffsetMax.y = mapHandleOffsetMin.y + mapHandleHeight;
            }
            else if (mapHandleOffsetMax.y > mapHeight / 2)
            {
                mapMoveDelta.y = -(mapHandleOffsetMax.y - mapHeight / 2);
                mapHandleOffsetMax.y = mapHeight / 2;
                mapHandleOffsetMin.y = mapHandleOffsetMax.y - mapHandleHeight;
            }
            mapHandle.rectTransform.offsetMin = mapHandleOffsetMin;
            mapHandle.rectTransform.offsetMax = mapHandleOffsetMax;
            //移动地图
            if (mapImage)
            {
                Vector2 mapImageOffsetMin = mapImage.rectTransform.offsetMin;
                Vector2 mapImageOffsetMax = mapImage.rectTransform.offsetMax;
                mapImageOffsetMin += mapMoveDelta;
                mapImageOffsetMax += mapMoveDelta;
                MoveBackMap(mapImageOffsetMin, mapImageOffsetMax);
            }
        }
    }

    /// <summary>
    /// 移动到地图指定位置(场景坐标)
    /// </summary>
    /// <param name="pointAtTerrain">场景坐标</param>
    public void MoveToTerrainPoint(Vector2 pointAtTerrain)
    {
        RectTransform mapRectTrans = GetComponent<RectTransform>();
        float mapWidth = mapRectTrans.rect.width;
        float mapHeight = mapRectTrans.rect.height;
        if (mapImage)
        {
            if (mapImage)
            {
                float mapImageWidth = mapImage.rectTransform.rect.width;
                float mapImageHeight = mapImage.rectTransform.rect.height;
                float percentageX = (pointAtTerrain.x - sceneRect.xMin) / sceneRect.width;
                float percentageY = (pointAtTerrain.y - sceneRect.yMin) / sceneRect.height;
                if (percentageX >= 0 && percentageX <= 1 && percentageY >= 0 && percentageY <= 1)//表明该点在地图上
                {
                    Vector2 offsetMin = new Vector2(-mapImageWidth * percentageX, -mapImageHeight * percentageY);
                    Vector2 offsetMax = new Vector2(offsetMin.x + mapImageWidth, offsetMin.y + mapImageHeight);
                    MoveBackMap(offsetMin, offsetMax);
                }
            }
        }
    }

    /// <summary>
    /// 定位到指定图标
    /// </summary>
    /// <param name="iconStruct"></param>
    public void MoveToIconStruct(UIMapIconStruct iconStruct)
    {
        if (!iconStruct)
            return;
        RectTransform iconTrans = iconStruct.GetComponent<RectTransform>();
        Vector2 anchor = iconTrans.anchorMin;
        Vector2 terrainPoint = new Vector2(sceneRect.xMin + sceneRect.width * anchor.x, sceneRect.yMin + sceneRect.height * anchor.y);
        MoveToTerrainPoint(terrainPoint);
    }

    /// <summary>
    /// 添加一个Icon
    /// </summary>
    /// <param name="iconSprite">图标精灵</param>
    /// <param name="pointAtTerrain">图标在地图中的位置</param>
    /// <param name="size">图标大小</param>
    /// <returns></returns>
    public UIMapIconStruct AddIcon(Sprite iconSprite, Vector2 size, Vector2 pointAtTerrain)
    {
        if (mapImage)
        {
            RectTransform mapImageRect = mapImage.rectTransform;
            GameObject mapIconObj = new GameObject();
            mapIconObj.transform.SetParent(mapImageRect);
            //添加ui组件并设置大小和位置
            RectTransform mapIconRect = mapIconObj.AddComponent<RectTransform>();
            Vector2 anchor = new Vector2((pointAtTerrain.x - sceneRect.xMin) / sceneRect.width, (pointAtTerrain.y - sceneRect.yMin) / sceneRect.height);
            mapIconRect.anchorMin = anchor;
            mapIconRect.anchorMax = anchor;
            mapIconRect.offsetMin = -size / 2;
            mapIconRect.offsetMax = size / 2;
            //添加触发器
            CircleCollider2D collider2D = mapIconObj.AddComponent<CircleCollider2D>();
            collider2D.radius = size.x > size.y ? (size.x / 2) : (size.y / 2);
            collider2D.isTrigger = true;
            //添加图片
            Image mapIconImage = mapIconObj.AddComponent<Image>();
            mapIconImage.sprite = iconSprite;
            mapIconImage.color = Color.white;
            mapIconImage.type = Image.Type.Simple;
            //添加点击事件
            EventTrigger eventTrigger = mapIconObj.AddComponent<EventTrigger>();
            eventTrigger.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(MapIcon_Click);
            eventTrigger.triggers.Add(entry);
            //添加脚本
            UIMapIconStruct uiMapIconStruct = mapIconObj.AddComponent<UIMapIconStruct>();
            uiImapIconStructList.Add(uiMapIconStruct);
            return uiMapIconStruct;
        }
        return null;
    }

    /// <summary>
    /// 鼠标点击图标时触发
    /// </summary>
    /// <param name="e"></param>
    private void MapIcon_Click(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe != null)
        {
            UIMapIconStruct uiMapIconStruct = null;
            if (pe.pointerCurrentRaycast.gameObject && (uiMapIconStruct = pe.pointerCurrentRaycast.gameObject.GetComponent<UIMapIconStruct>()) != null)
            {
                touchIconObj = uiMapIconStruct.gameObject;
                if (ClickOnMap != null)
                    ClickOnMap(TranslatePosSceneToTerrain(pe.currentInputModule.input.mousePosition));
            }
        }
    }

    /// <summary>
    /// 鼠标点击地图时触发
    /// </summary>
    /// <param name="e"></param>
    private void MapImage_Click(BaseEventData e)
    {
        PointerEventData pe = e as PointerEventData;
        if (pe != null)
        {
            if (pe.pointerCurrentRaycast.gameObject && pe.pointerCurrentRaycast.gameObject.Equals(mapImage.gameObject))
            {
                touchIconObj = null;
                if (ClickOnMap != null)
                    ClickOnMap(TranslatePosSceneToTerrain(pe.currentInputModule.input.mousePosition));
            }
        }
    }

    /// <summary>
    /// 移除一个Icon
    /// </summary>
    /// <param name="mapIconStruct">图标对象</param>
    public void RemoveIcon(UIMapIconStruct mapIconStruct)
    {
        if (uiImapIconStructList.Contains(mapIconStruct))
        {
            uiImapIconStructList.Remove(mapIconStruct);
        }
        if (mapIconStruct)
        {
            if (handleTriggerGameObjectList.Contains(mapIconStruct.gameObject))
                handleTriggerGameObjectList.Remove(mapIconStruct.gameObject);
            if (touchIconObj && touchIconObj.Equals(mapIconStruct.gameObject))
                touchIconObj = null;
            DestroyImmediate(mapIconStruct.gameObject);
        }
    }

    /// <summary>
    /// 设置图标的可见性
    /// </summary>
    /// <param name="mapIconStruct">图标对象</param>
    /// <param name="visiable">是否可见</param>
    public void SetIconVissable(UIMapIconStruct mapIconStruct, bool visiable)
    {
        if (mapIconStruct)
        {
            mapIconStruct.gameObject.SetActive(visiable);
            if (visiable)
            {
                if (UseHandle && handleTriggerGameObjectList != null && handleTriggerGameObjectList.Contains(mapIconStruct.gameObject))
                {
                    handleTriggerGameObjectList.Remove(mapIconStruct.gameObject);
                }
                if (touchIconObj && touchIconObj.Equals(mapIconStruct.gameObject))
                    touchIconObj = null;
            }
            else
            {
                if (UseHandle)
                {
                    Vector2 iconPos = mapIconStruct.GetComponent<RectTransform>().position;
                    Vector2 handlePos = mapHandle.GetComponent<RectTransform>().position;
                    if (Vector2.Distance(iconPos, handlePos) < stickOnPixel)
                    {
                        if (!handleTriggerGameObjectList.Contains(mapIconStruct.gameObject))
                        {
                            handleTriggerGameObjectList.Add(mapIconStruct.gameObject);
                            touchIconObj = null;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 设置图标的大小
    /// </summary>
    /// <param name="mapIconStruct">图标对象</param>
    /// <param name="size">像素大小</param>
    public void SetIconSize(UIMapIconStruct mapIconStruct, Vector2 size)
    {
        if (mapIconStruct)
        {
            RectTransform mapIconTrans = mapIconStruct.GetComponent<RectTransform>();
            mapIconTrans.offsetMin = new Vector2(-size.x, -size.y);
            mapIconTrans.offsetMax = new Vector2(size.x, size.y);
        }
    }

    /// <summary>
    /// 设置图标在地图上的位置
    /// </summary>
    /// <param name="mapIconStruct">图标对象</param>
    /// <param name="pointAtTerrain">图标在场景中的位置</param>
    public void SetIconPos(UIMapIconStruct mapIconStruct, Vector2 pointAtTerrain)
    {
        if (mapIconStruct)
        {
            Vector2 anchor = new Vector2((pointAtTerrain.x - sceneRect.xMin) / sceneRect.width, (pointAtTerrain.y - sceneRect.yMin) / sceneRect.height);
            RectTransform mapIconTrans = mapIconStruct.GetComponent<RectTransform>();
            mapIconTrans.anchorMin = anchor;
            mapIconTrans.anchorMax = anchor;
        }
    }

    /// <summary>
    /// 获取触碰或点击的图标
    /// </summary>
    /// <returns></returns>
    public UIMapIconStruct GetTouchOrClickIcon()
    {
        if (UseHandle)
        {
            if (touchIconObj != null)
                return touchIconObj.GetComponent<UIMapIconStruct>();
            else if (handleTriggerGameObjectList != null && handleTriggerGameObjectList.Count > 0)
            {
                UIMapIconStruct uiMapIconStruct = NearIconOfHandle();
                if (uiMapIconStruct != null)
                {
                    touchIconObj = uiMapIconStruct.gameObject;
                    return uiMapIconStruct;
                }
                return null;
            }
            else
                return null;
        }
        else
        {
            if (touchIconObj != null)
                return touchIconObj.GetComponent<UIMapIconStruct>();
            return null;
        }
    }

    /// <summary>
    /// 获取手柄转换到地图上以后的位置
    /// 如果不是手柄模式则返回Vector.Zero
    /// </summary>
    /// <returns></returns>
    public Vector2 GetHandlePosInTerrain()
    {
        if (UseHandle)
        {
            if (mapHandle)
                return TranslatePosSceneToTerrain(mapHandle.rectTransform.position);
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 将屏幕坐标转换成地图坐标
    /// </summary>
    /// <param name="scenePos"></param>
    /// <returns></returns>
    public Vector2 TranslatePosSceneToTerrain(Vector2 scenePos)
    {
        if (mapImage)
        {
            Vector2 mapImageCenter = mapImage.rectTransform.position;
            Vector2 offsetMin = mapImage.rectTransform.offsetMin;
            Vector2 offsetMax = mapImage.rectTransform.offsetMax;
            Vector2 nowPosOffset = scenePos - mapImageCenter;
            Vector2 nowPosAtMapImage = nowPosOffset + new Vector2((offsetMax.x - offsetMin.x) / 2, (offsetMax.y - offsetMin.y) / 2);
            Vector2 nowAnchorAtMapImage = new Vector2(nowPosAtMapImage.x / (offsetMax.x - offsetMin.x), nowPosAtMapImage.y / (offsetMax.y - offsetMin.y));
            Vector2 nowPosAtTerrain = new Vector2(sceneRect.xMin + sceneRect.width * nowAnchorAtMapImage.x, sceneRect.yMin + sceneRect.height * nowAnchorAtMapImage.y);
            return nowPosAtTerrain;
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 将地图坐标转换成屏幕坐标
    /// </summary>
    /// <param name="terrainPos"></param>
    /// <returns></returns>
    public Vector2 TranslatePosTerrainToScene(Vector2 terrainPos)
    {
        if (mapImage)
        {
            Vector2 nowAnchorAtMapImage = new Vector2((terrainPos.x - sceneRect.xMin) / sceneRect.width, (terrainPos.y - sceneRect.yMin) / sceneRect.height);
            Vector2 mapImageCenter = mapImage.rectTransform.position;
            Vector2 offsetMin = mapImage.rectTransform.offsetMin;
            Vector2 offsetMax = mapImage.rectTransform.offsetMax;
            Vector2 nowPosAtMapImage = new Vector2(nowAnchorAtMapImage.x * (offsetMax.x - offsetMin.x), nowAnchorAtMapImage.y * (offsetMax.y - offsetMin.y))
                - new Vector2((offsetMax.x - offsetMin.x) / 2, (offsetMax.y - offsetMin.y) / 2);
            Vector2 nowPosAtScene = nowPosAtMapImage + mapImageCenter;
            return nowPosAtScene;
        }
        return new Vector2(-10000, -10000);
    }

    /// <summary>
    /// 获取地图显示的区域(返回的是场景坐标)
    /// </summary>
    /// <returns></returns>
    public Rect GetShowRectInTerrain()
    {
        if (mapImage)
        {
            RectTransform mapRectTrans = GetComponent<RectTransform>();//地图控件
            float mapWidth = mapRectTrans.rect.width;// 地图控件的宽度
            float mapHeight = mapRectTrans.rect.height;//地图控件的高度
            Vector2 mapPos = mapRectTrans.position;//地图控件的位置
            Vector2 leftDownScenePos = new Vector2(mapPos.x - mapWidth / 2, mapPos.y - mapHeight / 2);
            Vector3 rightUpScenePos = new Vector3(mapPos.x + mapWidth / 2, mapPos.y + mapHeight / 2);
            Vector2 leftDownTerrainPos = TranslatePosSceneToTerrain(leftDownScenePos);
            Vector2 rightUpTerrainPos = TranslatePosSceneToTerrain(rightUpScenePos);
            return new Rect(leftDownTerrainPos, rightUpTerrainPos - leftDownTerrainPos);
        }
        return new Rect(1,1,1,1);
    }

    /// <summary>
    /// 获取距离手柄最近的图标
    /// 只有在使用手柄式才会有效
    /// </summary>
    /// <returns></returns>
    private UIMapIconStruct NearIconOfHandle()
    {
        if (UseHandle && handleTriggerGameObjectList != null && handleTriggerGameObjectList.Count > 0)
        {
            Vector2 mapHandleCenter = new Vector2(mapHandle.rectTransform.position.x, mapHandle.rectTransform.position.y);
            UIMapIconStruct uiMapIconStruct = handleTriggerGameObjectList.
                Where(temp => temp != null).
                Select(temp => temp.GetComponent<RectTransform>()).
                Select(temp => new { trans = temp, pos = new Vector2(temp.position.x, temp.position.y) }).
                Select(temp => new { trans = temp.trans, dis = Vector2.Distance(mapHandleCenter, temp.pos) }).
                OrderBy(temp => temp.dis).
                FirstOrDefault().trans.GetComponent<UIMapIconStruct>();
            return uiMapIconStruct;
        }
        else return null;
    }

    /// <summary>
    /// 获取图标在场景中位置
    /// </summary>
    /// <param name="uiMapIconStruct">图标对象</param>
    /// <returns></returns>
    public Vector2 GetIconPosInTerrain(UIMapIconStruct uiMapIconStruct)
    {
        if (uiMapIconStruct)
        {
            return TranslatePosSceneToTerrain(uiMapIconStruct.GetComponent<RectTransform>().position);
        }
        return new Vector2(-10000, -10000);
    }
}


/// <summary>
/// 检测手柄触发所使用的类
/// </summary>
public class MapHandleColliderCheck : MonoBehaviour
{
    /// <summary>
    /// 触发了进入事件
    /// </summary>
    public event Action<Collider2D> TriggerEnterHandle;

    /// <summary>
    /// 触发了离开事件
    /// </summary>
    public event Action<Collider2D> TriggerExitHandle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerEnterHandle != null)
            TriggerEnterHandle(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerExitHandle != null)
            TriggerExitHandle(collision);
    }
}

/// <summary>
/// 地图图标结构
/// </summary>
public class UIMapIconStruct : MonoBehaviour
{
    /// <summary>
    /// 对象id
    /// </summary>
    public int id;

    /// <summary>
    /// 对象数据
    /// </summary>
    public object value;
}