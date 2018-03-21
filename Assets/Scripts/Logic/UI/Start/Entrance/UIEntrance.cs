using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 入口界面UI
/// </summary>
public class UIEntrance : MonoBehaviour
{
    /// <summary>
    /// 开始面板
    /// </summary>
    public RectTransform startRect;
    /// <summary>
    /// 创建面板
    /// </summary>
    public RectTransform createRect;
    /// <summary>
    /// 进入面板
    /// </summary>
    public RectTransform enterRect;

    /// <summary>
    /// 遮罩图片
    /// </summary>
    public Image maskImage;
    /// <summary>
    /// 过渡时间
    /// </summary>
    public float crossoverTime;

    /// <summary>
    /// 开始场景中的游戏状态
    /// </summary>
    EnumEntranceType entranceType;

    #region 开始面板上的按钮以及他们的ui路径
    /// <summary>
    /// 继续游戏
    /// </summary>
    public RectTransform continueRect;
    /// <summary>
    /// 新游戏
    /// </summary>
    public RectTransform newGameRect;
    /// <summary>
    /// 设置
    /// </summary>
    public RectTransform optionRect;
    /// <summary>
    /// 开始面板的UI焦点路径
    /// </summary>
    public UIFocusPath startUIFocusPath;
    /// <summary>
    /// 当前的开始面板的焦点
    /// </summary>
    UIFocus nowStartFocus;
    #endregion

    #region 创建面板的按钮以及他们的UI路径
    /// <summary>
    /// 种族选择面板 
    /// </summary>
    public RectTransform roleOfRaceRect;
    /// <summary>
    /// 名字输入面板
    /// </summary>
    public RectTransform nameInputRect;
    /// <summary>
    /// 种族选择面板的ui路径
    /// </summary>
    public UIFocusPath roleOfRaceUIFocusPath;
    /// <summary>
    /// 当前的种族选择面板的焦点
    /// </summary>
    UIFocus nowRoleOfRaceFocus;
    /// <summary>
    /// 名字输入面板的ui路径
    /// </summary>
    public UIFocusPath nameInputUIFocusPath;
    /// <summary>
    /// 当前的名字输入面板的焦点
    /// </summary>
    UIFocus nowNameInputFocus;

    /// <summary>
    /// 显示角色名的文本框
    /// </summary>
    public Text playerNameText;
    /// <summary>
    /// 显示角色等级的文本框 
    /// </summary>
    public Text playerLevelText;
    /// <summary>
    /// 显示角色种族的文本框
    /// </summary>
    public Text playerRoleOfRaceText;
    /// <summary>
    /// 角色创建的位置
    /// </summary>
    public Transform playerCreateTrans;
    #endregion

    /// <summary>
    /// 设置面板预设体
    /// </summary>
    [SerializeField]
    private GameObject settingCanvasPrefab;

    /// <summary>
    /// 设置面板
    /// </summary>
    private Canvas settingCanvas;

    /// <summary>
    /// 是否正在加载场景
    /// </summary>
    bool isLoadedScene;

    /// <summary>
    /// 自身是否可以使用输入
    /// </summary>
    private bool CanGetInput
    {
        get
        {
            if (settingCanvas)
            {
                return !settingCanvas.gameObject.activeSelf;
            }
            else return true;
        }
    }

    void Start()
    {
        entranceType = EnumEntranceType.Start;

        playerCreateTrans = GameObject.Find("PlayerInit").transform;

        if (settingCanvasPrefab)
        {
            GameObject settingGameObject = Instantiate(settingCanvasPrefab);
            settingCanvas = settingGameObject.GetComponent<Canvas>();
            settingGameObject.SetActive(false);
        }
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        StartCoroutine(CrossoverMaskImageAlpha(0));

        //判断现在是否有默认存档,如果有则显示继续,如果没有则显示新建
        List<Archive> archiveList = DataCenter.Instance.GetAllArchive();//获取所有存档名 
        Archive defaultArchive = archiveList.FirstOrDefault(temp => temp.ID == 1);
        UIFocus continueFoucs = startUIFocusPath.NewUIFocusArray.FirstOrDefault(temp => string.Equals(temp.Tag, "Continue"));
        UIFocus newFocus = startUIFocusPath.NewUIFocusArray.FirstOrDefault(temp => string.Equals(temp.Tag, "New"));
        if (defaultArchive == null)
        {
            if (continueFoucs)
                continueFoucs.gameObject.SetActive(false);
            if (newFocus)
                newFocus.gameObject.SetActive(true);
        }
        else
        {
            if (continueFoucs)
                continueFoucs.gameObject.SetActive(true);
            if (newFocus)
                newFocus.gameObject.SetActive(false);
        }
        //设置当前选中
        if (nowStartFocus == null && startUIFocusPath)
        {
            nowStartFocus = startUIFocusPath.GetFirstFocus();
            if (nowStartFocus)
                nowStartFocus.SetForcus();
        }

    }

    /*
    private void Update()
    {
        if (!CanGetInput)
            return;

        switch (entranceType)
        {
            case EnumEntranceType.Start:
                StartType_KeyboardUpdate();
                break;
            case EnumEntranceType.Create_SelectRoleOfRace:
                break;
            case EnumEntranceType.Create_InputName:
                break;
            case EnumEntranceType.Enter:
                break;
        }
    }

    /// <summary>
    /// 开始状态时的键盘输入检测
    /// </summary>
    private void StartType_KeyboardUpdate()
    {
        if (nowStartFocus == null && startUIFocusPath)
        {
            nowStartFocus = startUIFocusPath.GetFirstFocus();
            if (nowStartFocus)
                nowStartFocus.SetForcus();
        }
        if (startUIFocusPath)
        {
            Action<UIFocusPath.MoveType> ThisAction = (moveType) =>
            {
                UIFocus next = startUIFocusPath.GetNewNextFocus(nowStartFocus, moveType);
                if (next)
                {
                    nowStartFocus = next;
                    nowStartFocus.SetForcus();
                }
            };
            if (Input.GetKeyUp(KeyCode.A))
                ThisAction(UIFocusPath.MoveType.LEFT);
            if (Input.GetKeyUp(KeyCode.W))
                ThisAction(UIFocusPath.MoveType.UP);
            if (Input.GetKeyUp(KeyCode.D))
                ThisAction(UIFocusPath.MoveType.RIGHT);
            if (Input.GetKeyUp(KeyCode.S))
                ThisAction(UIFocusPath.MoveType.DOWN);
            if (Input.GetKeyUp(KeyCode.X))
            {
                if (nowStartFocus)
                {
                    Button button = nowStartFocus.GetComponent<Button>();
                    button.onClick.Invoke();
                }
            }
        }
    }
    */



    void OnDestroy()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (!CanGetInput)
            return;
        switch (entranceType)
        {
            case EnumEntranceType.Start:
                StartType_HandleUpdate(keyType, rockValue);
                break;
            case EnumEntranceType.Create_SelectRoleOfRace:
                CreateSelectRoleOfRaceType_HandleUpdate(keyType, rockValue);
                break;
            case EnumEntranceType.Create_InputName:
                CreateNameInputType_HandleUpdate(keyType, rockValue);
                break;
            case EnumEntranceType.Enter:
                EnterType_HandleUpDate(keyType, rockValue);
                break;
        }
    }

    /// <summary>
    /// 开始状态时的手柄输入检测
    /// </summary>
    /// <param name="keyType">按键类型</param>
    /// <param name="rockValue">摇杆</param>
    private void StartType_HandleUpdate(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (nowStartFocus == null && startUIFocusPath)
        {
            nowStartFocus = startUIFocusPath.GetFirstFocus();
            if (nowStartFocus)
                nowStartFocus.SetForcus();
        }
        if (startUIFocusPath)
        {
            //判断键位
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                UIFocus next = startUIFocusPath.GetNewNextFocus(nowStartFocus, moveType);// uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowStartFocus.LostForcus();
                    nowStartFocus = next;
                    nowStartFocus.SetForcus();
                }
            };
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    if (nowStartFocus)
                    {
                        UIFocusButton uiFocusButton = nowStartFocus as UIFocusButton;
                        if (uiFocusButton)
                        {
                            uiFocusButton.ClickThisButton();
                        }
                    }
                    break;
                case UIManager.KeyType.LEFT:
                    MoveFocusAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    MoveFocusAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    MoveFocusAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    MoveFocusAction(UIFocusPath.MoveType.DOWN);
                    break;
            }
        }
    }

    /// <summary>
    /// 创建时选择种族状态的手柄输入检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void CreateSelectRoleOfRaceType_HandleUpdate(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (nowRoleOfRaceFocus == null && roleOfRaceUIFocusPath)
        {
            nowRoleOfRaceFocus = roleOfRaceUIFocusPath.GetFirstFocus();
            if (nowRoleOfRaceFocus)
                nowRoleOfRaceFocus.SetForcus();
        }
        if (nowRoleOfRaceFocus)
        {
            //判断键位
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                UIFocus next = roleOfRaceUIFocusPath.GetNewNextFocus(nowRoleOfRaceFocus, moveType);// uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowRoleOfRaceFocus = next;
                    CreateSelectRoleOfRaceUpdateNowFocus();
                }
            };
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    if (nowRoleOfRaceFocus)
                    {
                        UIFocusButton uiFocusButton = nowRoleOfRaceFocus as UIFocusButton;
                        if (uiFocusButton)
                        {
                            uiFocusButton.ClickThisButton();
                        }
                    }
                    break;
                case UIManager.KeyType.LEFT:
                    MoveFocusAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    MoveFocusAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    MoveFocusAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    MoveFocusAction(UIFocusPath.MoveType.DOWN);
                    break;
            }
        }
    }

    /// <summary>
    /// 创建时输入名字状态的手柄输入检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void CreateNameInputType_HandleUpdate(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (nowNameInputFocus == null && nameInputUIFocusPath)
        {
            nowNameInputFocus = nameInputUIFocusPath.GetFirstFocus();
            if (nowNameInputFocus)
                nowNameInputFocus.SetForcus();
        }
        if (nowNameInputFocus)
        {
            //判断键位
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                UIFocus next = nameInputUIFocusPath.GetNewNextFocus(nowNameInputFocus, moveType);// uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowNameInputFocus.LostForcus();
                    nowNameInputFocus = next;
                    nowNameInputFocus.SetForcus();
                }
            };
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    if (nowNameInputFocus)
                    {
                        UIFocusButton uiFocusButton = nowNameInputFocus as UIFocusButton;
                        UIFocusInputField uiFocusInputField = nowNameInputFocus as UIFocusInputField;
                        if (uiFocusButton)
                        {
                            uiFocusButton.ClickThisButton();
                        }
                        else if (uiFocusInputField)
                        {
                            uiFocusInputField.EnterInputField();
                        }
                    }
                    break;
                case UIManager.KeyType.LEFT:
                    MoveFocusAction(UIFocusPath.MoveType.LEFT);
                    break;
                case UIManager.KeyType.RIGHT:
                    MoveFocusAction(UIFocusPath.MoveType.RIGHT);
                    break;
                case UIManager.KeyType.UP:
                    MoveFocusAction(UIFocusPath.MoveType.UP);
                    break;
                case UIManager.KeyType.DOWN:
                    MoveFocusAction(UIFocusPath.MoveType.DOWN);
                    break;
            }
        }
    }

    /// <summary>
    /// 进入状态时手柄输入检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void EnterType_HandleUpDate(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (keyType == UIManager.KeyType.A)
        {
            //进入游戏
            GetArchiveData();
        }
    }

    /// <summary>
    /// 过渡携程
    /// </summary>
    /// <param name="value">要过渡到的值</param>
    /// <param name="callBack">回调</param>
    /// <returns></returns>
    IEnumerator CrossoverMaskImageAlpha(int value, Action callBack = null)
    {
        value = Mathf.Clamp(value, 0, 1);
        float startValue = maskImage.color.a;
        float offset = value - startValue;
        if (crossoverTime > 0)
        {
            float interval = offset / crossoverTime;
            Color nowColor = maskImage.color;
            float tempCrossoverTime = crossoverTime;
            while ((tempCrossoverTime -= Time.deltaTime) >= 0)
            {
                nowColor.a += interval;
                maskImage.color = nowColor;
                yield return null;
            }
            nowColor.a = value;
            maskImage.color = nowColor;
        }
        if (callBack != null)
            callBack();
    }

    ///// <summary>
    ///// 切换到指定场景
    ///// </summary>
    ///// <param name="sceneName">场景名</param>
    //private void ChangeScene(string sceneName)
    //{
    //    StartCoroutine(CrossoverMaskImageAlpha(0, () => { }));
    //}

    #region 功能类
    /// <summary>
    /// 继续按钮按下事件
    /// </summary>
    public void ContinueButtonClick()
    {
        if (isLoadedScene || entranceType != EnumEntranceType.Start)
            return;
        //切换场景
        nowStartFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();

        List<Archive> archiveList = DataCenter.Instance.GetAllArchive();//获取所有存档名 
        Archive defaultArchive = archiveList.FirstOrDefault(temp => temp.ID == 1);
        if (defaultArchive != null)
        {
            DataCenter.Instance.LoadArchive(1);//加载第一个存档
            //隐藏该面板显示进入面板
            entranceType = EnumEntranceType.Transition;
            StartCoroutine(CrossoverMaskImageAlpha(1, () =>
            {
                startRect.gameObject.SetActive(false);
                enterRect.gameObject.SetActive(true);
                InitEnterRect();
                StartCoroutine(CrossoverMaskImageAlpha(0, () =>
                {
                    entranceType = EnumEntranceType.Enter;
                }));
            }));
            #region 老版的加载
            //DataCenter.Instance.LoadArchive(1);//加载第一个存档
            //GetArchiveData();
            #endregion
        }
    }

    /// <summary>
    /// 新游戏按钮按下事件
    /// </summary>
    public void NewGameButtonClick()
    {
        if (isLoadedScene || entranceType != EnumEntranceType.Start)
            return;
        //切换场景
        nowStartFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
        List<Archive> archiveList = DataCenter.Instance.GetAllArchive();//获取所有存档名 
        Archive defaultArchive = archiveList.FirstOrDefault(temp => temp.ID == 1);
        if (defaultArchive == null)
        {
            DataCenter.Instance.Save(1, "存档", "默认存档");
            DataCenter.Instance.LoadArchive(1);
            //设置默认数据
            //设置默认的按键 1:1_183
            KeyContactData keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
            //设置采集键
            Sprite collectSprite = SpriteManager.GetSrpite("1:1_183");
            keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { keyContactType = EnumKeyContactType.Action, name = "采集", Sprite = collectSprite, id = 1 }, EnumKeyContactDataZone.Collect);
            //设置对话键
            Sprite talkSprite = SpriteManager.GetSrpite("1:1_165");
            keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { keyContactType = EnumKeyContactType.Action, name = "交谈", Sprite = talkSprite, id = 2 }, EnumKeyContactDataZone.Dialogue);
            //设置功能键
            Sprite actionSprite = SpriteManager.GetSrpite("1:1_183");
            keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { keyContactType = EnumKeyContactType.Action, name = "交互", Sprite = actionSprite, id = 3 }, EnumKeyContactDataZone.Action);
            //设置默认的等级
            PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
            playerState.Level = 1;
            playerState.Experience = 0;
            playerState.RoleOfRaceRoute = new List<RoleOfRace>();
            //再次保存
            DataCenter.Instance.Save(1, "存档", "默认存档");

            #region 测试用添加的数据
            /*测试代码*/
            //添加技能点
            EnumSkillType[] enumSkillTypes = Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Where(temp =>
                (temp < EnumSkillType.MagicCombinedLevel1End && temp > EnumSkillType.MagicCombinedLevel1Start)
                || (temp < EnumSkillType.MagicCombinedLevel2End && temp > EnumSkillType.MagicCombinedLevel2Start)
                || (temp < EnumSkillType.MagicCombinedLevel3End && temp > EnumSkillType.MagicCombinedLevel3Start)
                || (temp < EnumSkillType.MagicCombinedLevel4End && temp > EnumSkillType.MagicCombinedLevel4Start)
            ).ToArray();
            enumSkillTypes = Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().Distinct().ToArray();//所有技能加1
            foreach (var item in enumSkillTypes)
            {
                playerState.SkillPoint.Add(item, 1);
            }
            //playerState.SkillPoint.Add(EnumSkillType.MagicRelease, 1);
            playerState.FreedomPoint = 2;
            //添加按键
            keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
            keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { id = 199, key = (int)EnumInputType.A, keyContactType = EnumKeyContactType.Skill, name = "释放魔法" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.Up, new KeyContactStruct() { id = 1001, key = (int)EnumInputType.Up, keyContactType = EnumKeyContactType.Skill, name = "奥术弹" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.Right, new KeyContactStruct() { id = 1101, key = (int)EnumInputType.Right, keyContactType = EnumKeyContactType.Skill, name = "火元素" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.Left, new KeyContactStruct() { id = 1004, key = (int)EnumInputType.Left, keyContactType = EnumKeyContactType.Skill, name = "魔力导向" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.Down, new KeyContactStruct() { id = 1201, key = (int)EnumInputType.Down, keyContactType = EnumKeyContactType.Skill, name = "连续魔力导向" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.X, new KeyContactStruct() { id = 201, key = (int)EnumInputType.X, keyContactType = EnumKeyContactType.Skill, name = "普通攻击" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.Y, new KeyContactStruct() { id = (int)EnumSkillType.JAS03, /*key = (int)EnumInputType.Y,*/ keyContactType = EnumKeyContactType.Skill, name = "燕返" });
            keyContactData.SetKeyContactStruct((int)EnumInputType.B, new KeyContactStruct() { id = (int)EnumSkillType.ZS03, keyContactType = EnumKeyContactType.Skill, name = "冲锋" });
            int key = (int)(EnumInputType.A | EnumInputType.LB);
            keyContactData.SetKeyContactStruct(key, new KeyContactStruct() { id = (int)EnumSkillType.KZS03, key = key, keyContactType = EnumKeyContactType.Skill, name = "战吼" });
            //添加装备
            //int playGoodsID = NowTimeToID.GetNowID(DataCenter.Instance.GetEntity<GameRunnedState>());
            //Goods goods = new Goods(EnumGoodsType.XJJ, "测试武器", 1, 10, "123");
            //goods.goodsAbilities.Add(new GoodsAbility() { AbilibityKind = EnumGoodsAbility.ASPD, Value = 10 });
            //goods.SpriteName = "1:1_226";

            //PlayGoods playGoods = new PlayGoods(playGoodsID, goods, GoodsLocation.Package);
            //playerState.PlayerAllGoods.Add(playGoods);
            //playerState.Sprice = 10000;
            /*****测试代码结束*****/

            #endregion

            //隐藏该面板显示创建面板的种族选择面板
            entranceType = EnumEntranceType.Transition;
            StartCoroutine(CrossoverMaskImageAlpha(1, () =>
            {
                startRect.gameObject.SetActive(false);
                createRect.gameObject.SetActive(true);
                roleOfRaceRect.gameObject.SetActive(true);
                nameInputRect.gameObject.SetActive(false);
                StartCoroutine(CrossoverMaskImageAlpha(0, () =>
                {
                    entranceType = EnumEntranceType.Create_SelectRoleOfRace;
                    InitCreateSelectRoleOfRace();
                }));
            }));
            #region 老版的直接加载
            //DataCenter.Instance.Save(1, "存档", "默认存档");
            //DataCenter.Instance.LoadArchive(1);
            ////设置默认数据
            ////设置默认的按键 1:1_183
            //KeyContactData keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
            ////设置采集键
            //Sprite collectSprite = SpriteManager.GetSrpite("1:1_183");
            //keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { keyContactType = EnumKeyContactType.Action, name = "采集", Sprite = collectSprite, id = 1 }, EnumKeyContactDataZone.Collect);
            ////设置对话键
            //Sprite talkSprite = SpriteManager.GetSrpite("1:1_165");
            //keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { keyContactType = EnumKeyContactType.Action, name = "交谈", Sprite = talkSprite, id = 2 }, EnumKeyContactDataZone.Dialogue);
            ////设置默认的等级
            //PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
            //playerState.Level = 1;
            //playerState.Experience = 0;
            //playerState.RoleOfRaceRoute = RoleOfRaceHelper.GetAllEvolvePath(RoleOfRace.Human).FirstOrDefault();
            ////再次保存
            //DataCenter.Instance.Save(1, "存档", "默认存档");
            ////获取数据并切换场景
            //GetArchiveData();
            #endregion
        }
    }

    /// <summary>
    /// 设置按钮按下事件
    /// </summary>
    public void SettingButtonClick()
    {
        if (isLoadedScene)
            return;
        nowStartFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
        //显示设置面板
        if (settingCanvas)
        {
            settingCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 初始化新建时的选择种族
    /// </summary>
    private void InitCreateSelectRoleOfRace()
    {
        nowRoleOfRaceFocus = roleOfRaceUIFocusPath.GetFirstFocus();
        CreateSelectRoleOfRaceUpdateNowFocus();
    }

    /// <summary>
    /// 新建时选择种族,更新此时的种族
    /// </summary>
    private void CreateSelectRoleOfRaceUpdateNowFocus()
    {
        if (nowRoleOfRaceFocus == null)
            return;
        UIFocus[] allRoleOfRaceFocuses = roleOfRaceUIFocusPath.NewUIFocusArray.Where(temp => temp.gameObject.activeSelf).ToArray();
        foreach (UIFocus roleOfRaceFoucs in allRoleOfRaceFocuses)
        {
            RectTransform rectTrans = roleOfRaceFoucs.GetComponent<RectTransform>();
            float width = rectTrans.rect.width;
            float minY = rectTrans.offsetMin.y;
            float maxY = rectTrans.offsetMax.y;
            if (roleOfRaceFoucs == nowRoleOfRaceFocus)
            {
                rectTrans.offsetMin = new Vector2(70, minY);
                rectTrans.offsetMax = new Vector2(width + 70, maxY);
                roleOfRaceFoucs.SetForcus();
            }
            else
            {
                rectTrans.offsetMin = new Vector2(20, minY);
                rectTrans.offsetMax = new Vector2(width + 20, maxY);
                roleOfRaceFoucs.LostForcus();
            }
        }
    }

    /// <summary>
    /// 新建时选择种族中的种族按钮被点击
    /// </summary>
    /// <param name="uiFocus"></param>
    public void CreateSelectRoleOfRaceButtonClick(UIFocus uiFocus)
    {
        if (nowRoleOfRaceFocus == uiFocus && uiFocus != null)
        {
            CreateSelectRoleOfRaceSelect(uiFocus.Tag);
        }
        else
        {
            nowRoleOfRaceFocus = uiFocus;
            CreateSelectRoleOfRaceUpdateNowFocus();
        }
    }

    /// <summary>
    /// 选择该种族 
    /// </summary>
    /// <param name="roleOfRaceStr"></param>
    private void CreateSelectRoleOfRaceSelect(string roleOfRaceStr)
    {
        RoleOfRace enumRoleOfRace = (RoleOfRace)Enum.Parse(typeof(RoleOfRace), roleOfRaceStr);
        //选择该种族
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        playerState.RoleOfRaceRoute = new List<RoleOfRace>();
        playerState.RoleOfRaceRoute.Add(enumRoleOfRace);
        //再次保存
        DataCenter.Instance.Save(1, "存档", "默认存档");
        //切换到输入名字状态
        entranceType = EnumEntranceType.Create_InputName;
        //显示输入名字面板
        roleOfRaceRect.gameObject.SetActive(false);
        nameInputRect.gameObject.SetActive(true);
        //初始化
        InitCreateNameInput();
    }

    /// <summary>
    /// 初始化新建时的输入姓名
    /// </summary>
    private void InitCreateNameInput()
    {
        nowNameInputFocus = nameInputUIFocusPath.GetFirstFocus();
        nowNameInputFocus = nameInputUIFocusPath.GetFirstFocus();
        nowNameInputFocus.SetForcus();

    }

    /// <summary>
    /// 输入名字的按钮点击时
    /// </summary>
    /// <param name="uiFocus"></param>
    public void CreateNameInputButtonClick(UIFocus uiFocus)
    {
        if (uiFocus)
        {
            switch (uiFocus.Tag)
            {
                case "OK":
                    {
                        UIFocusInputField uiFocusInputField = nameInputUIFocusPath.GetFirstFocus<UIFocusInputField>();
                        if (uiFocusInputField)
                        {
                            InputField inputField = uiFocusInputField.GetComponent<InputField>();
                            if (inputField)
                            {
                                if (!string.IsNullOrEmpty(inputField.text))
                                {
                                    PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
                                    playerState.PlayerName = inputField.text;
                                    //再次保存
                                    DataCenter.Instance.Save(1, "存档", "默认存档");
                                    //隐藏该面板显示进入面板
                                    entranceType = EnumEntranceType.Transition;
                                    nameInputRect.gameObject.SetActive(false);
                                    StartCoroutine(CrossoverMaskImageAlpha(1, () =>
                                    {
                                        createRect.gameObject.SetActive(false);
                                        enterRect.gameObject.SetActive(true);
                                        InitEnterRect();
                                        StartCoroutine(CrossoverMaskImageAlpha(0, () =>
                                        {
                                            entranceType = EnumEntranceType.Enter;
                                        }));
                                    }));
                                }
                            }
                        }
                    }
                    break;
                case "Cancel":
                    roleOfRaceRect.gameObject.SetActive(true);
                    nameInputRect.gameObject.SetActive(false);
                    entranceType = EnumEntranceType.Create_SelectRoleOfRace;
                    InitCreateSelectRoleOfRace();
                    break;
            }
        }
    }

    /// <summary>
    /// 初始化进入面板
    /// </summary>
    private void InitEnterRect()
    {
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        playerNameText.text = "角色:   " + playerState.PlayerName;
        playerLevelText.text = "等级:   " + playerState.Level;
        playerRoleOfRaceText.text = "种族:   " + playerState.RoleOfRaceRoute.LastOrDefault();
        //显示角色
        if (playerCreateTrans != null)
        {
            //暂时不管种族
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Start/Player_Start");
            GameObject playerCreate = GameObject.Instantiate(playerPrefab);
            playerCreate.transform.position = playerCreateTrans.position;
            playerCreate.transform.rotation = playerCreateTrans.rotation;
        }
    }

    /// <summary>
    /// 进入按钮点击
    /// </summary>
    public void EnterButtonClick()
    {
        if (entranceType == EnumEntranceType.Enter)
        {
            GetArchiveData();
        }
    }

    #endregion

    /// <summary>
    /// 将当前存档数据放入运行时数据中,同时跳转场景
    /// </summary>
    private void GetArchiveData()
    {
        entranceType = EnumEntranceType.Transition;

        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
        #region  测试代码开始
        //playerState.PlayerAllGoods.Clear();
        //if (playerState.PlayerAllGoods.Count(temp => temp.ID == 10000) == 0)
        //{
        //    playerState.PlayerAllGoods.Add(new PlayGoods(10000, new Goods(EnumGoodsType.HFYJ, "恢复药剂", 1, 100, "恢复药剂")
        //    {
        //        goodsAbilities = new List<GoodsAbility>(new GoodsAbility[] { new GoodsAbility() { AbilibityKind = EnumGoodsAbility.HPRect_Rate, Level = 1, Value = 10 } })
        //    }, GoodsLocation.Package)
        //    {
        //        QualityType = EnumQualityType.White,
        //        Count = 10
        //    });
        //}
        ////添加属性点
        //playerState.PropertyPoint = 10;
        ////添加一个弓
        //playerState.PlayerAllGoods.Add(new PlayGoods(100000, new Goods(EnumGoodsType.WGCSG, "王国长杉弓", 1, 100, "王国长杉弓"), GoodsLocation.Package)
        //{
        //    leftRightArms = true,
        //    QualityType = EnumQualityType.Blue,
        //    Count = 1
        //});
        ////添加一个剑
        //playerState.PlayerAllGoods.Add(new PlayGoods(100001, new Goods(EnumGoodsType.TJ, "铁剑", 1, 100, "铁剑")
        //{
        //    goodsAbilities = new List<GoodsAbility>(new GoodsAbility[] { new GoodsAbility() { AbilibityKind = EnumGoodsAbility.EquipATK, Level = 1, Value = 60 } })
        //}, GoodsLocation.Package)
        //{
        //    leftRightArms = true,
        //    QualityType = EnumQualityType.Blue,
        //    Count = 1
        //});
        ////添加一个巨剑
        //playerState.PlayerAllGoods.Add(new PlayGoods(100002, new Goods(EnumGoodsType.YGDJ, "精钢大剑", 1, 100, "精钢大剑"), GoodsLocation.Wearing)
        //{
        //    leftRightArms = true,
        //    QualityType = EnumQualityType.Blue,
        //    Count = 1
        //});
        ////测试代码结束

        #endregion


        //通知运行时数据状态中心加载数据
        GameState.Instance.LoadArchive();
        //切换场景
        if (string.IsNullOrEmpty(playerState.Scene))
        {
            playerState.Scene = "中央王国";
            playerState.Location = new Vector3(813.9f, 36.64f, 909.7f);
        }
        if (playerState.StreetID <= 0)
        {
            playerState.StreetID = 2;
            playerState.StreetScene = "中央王国";
        }
        isLoadedScene = true;
        IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
        iGameState.ChangedScene(playerState.Scene, playerState.Location, result => isLoadedScene = false);
    }


    /// <summary>
    /// 此时的游戏状态
    /// </summary>
    public enum EnumEntranceType
    {
        /// <summary>
        /// 起始状态
        /// </summary>
        Start,
        /// <summary>
        /// 创建游戏状态选择种族步骤
        /// </summary>
        Create_SelectRoleOfRace,
        /// <summary>
        /// 创建游戏状态输入名字状态
        /// </summary>
        Create_InputName,
        /// <summary>
        /// 准备完毕准备进入游戏状态
        /// </summary>
        Enter,
        /// <summary>
        /// 过渡状态
        /// </summary>
        Transition
    }
}
