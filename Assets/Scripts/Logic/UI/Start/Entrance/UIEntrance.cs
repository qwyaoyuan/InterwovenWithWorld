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
    /// 遮罩图片
    /// </summary>
    public Image maskImage;
    /// <summary>
    /// 过渡时间
    /// </summary>
    public float crossoverTime;

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
    /// ui路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的焦点
    /// </summary>
    UIFocus nowFocus;
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
        uiFocusPath = GetComponent<UIFocusPath>();
        if (settingCanvasPrefab)
        {
            GameObject settingGameObject = Instantiate(settingCanvasPrefab);
            settingCanvas = settingGameObject.GetComponent<Canvas>();
            settingGameObject.SetActive(false);
        }
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        StartCoroutine(CrossoverMaskImageAlpha(1));

        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }
    }

    private void Update()
    {
        if (!CanGetInput)
            return;

        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }

        if (uiFocusPath)
        {
            Action<UIFocusPath.MoveType> ThisAction = (moveType) =>
            {
                UIFocus next = uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowFocus = next;
                    nowFocus.SetForcus();
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
                if (nowFocus)
                {
                    Button button = nowFocus.GetComponent<Button>();
                    button.onClick.Invoke();
                }
            }
        }
    }

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
        if (nowFocus == null && uiFocusPath)
        {
            nowFocus = uiFocusPath.GetFirstFocus();
            if (nowFocus)
                nowFocus.SetForcus();
        }
        if (uiFocusPath)
        {
            //判断键位
            Action<UIFocusPath.MoveType> MoveFocusAction = (moveType) =>
            {
                UIFocus next = uiFocusPath.GetNextFocus(nowFocus, moveType, true);
                if (next)
                {
                    nowFocus = next;
                    nowFocus.SetForcus();
                }
            };
            switch (keyType)
            {
                case UIManager.KeyType.A:
                    if (nowFocus)
                    {
                        Button nowButton = nowFocus.GetComponent<Button>();
                        if (nowButton)
                        {
                            nowButton.onClick.Invoke();
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

    /// <summary>
    /// 切换到指定场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    private void ChangeScene(string sceneName)
    {
        StartCoroutine(CrossoverMaskImageAlpha(0, () => { }));
    }

    #region ui上的事件
    /// <summary>
    /// 继续按钮按下事件
    /// </summary>
    public void ContinueButtonClick()
    {
        if (isLoadedScene)
            return;
        //切换场景
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();

        List<Archive> archiveList = DataCenter.Instance.GetAllArchive();//获取所有存档名 
        Archive defaultArchive = archiveList.FirstOrDefault(temp => temp.ID == 1);
        if (defaultArchive != null)
        {
            DataCenter.Instance.LoadArchive(1);//加载第一个存档
            GetArchiveData();
        }
    }

    /// <summary>
    /// 新游戏按钮按下事件
    /// </summary>
    public void NewGameButtonClick()
    {
        if (isLoadedScene)
            return;
        //切换场景
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
        List<Archive> archiveList = DataCenter.Instance.GetAllArchive();//获取所有存档名 
        Archive defaultArchive = archiveList.FirstOrDefault(temp => temp.ID == 1);
        if (defaultArchive == null)
        {
            DataCenter.Instance.Save(1, "存档", "默认存档");
            DataCenter.Instance.LoadArchive(1);
            GetArchiveData();
        }
    }

    /// <summary>
    /// 设置按钮按下事件
    /// </summary>
    public void SettingButtonClick()
    {
        if (isLoadedScene)
            return;
        nowFocus = EventSystem.current.currentSelectedGameObject.GetComponent<UIFocus>();
        //显示设置面板
        if (settingCanvas)
        {
            settingCanvas.gameObject.SetActive(true);
        }
    }
    #endregion

    /// <summary>
    /// 将当前存档数据放入运行时数据中,同时跳转场景
    /// </summary>
    private void GetArchiveData()
    {
        //加载数据
        GameState.Instance.LoadArchive();
        PlayerState playerState = DataCenter.Instance.GetEntity<PlayerState>();
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
        KeyContactData keyContactData = DataCenter.Instance.GetEntity<KeyContactData>();
        keyContactData.SetKeyContactStruct((int)EnumInputType.A, new KeyContactStruct() { id = 199, key = (int)EnumInputType.A, keyContactType = EnumKeyContactType.Skill, name = "释放魔法" });
        keyContactData.SetKeyContactStruct((int)EnumInputType.Up, new KeyContactStruct() {id = 1001,key = (int)EnumInputType.Up,keyContactType= EnumKeyContactType.Skill,name = "奥术弹" });
        keyContactData.SetKeyContactStruct((int)EnumInputType.Right, new KeyContactStruct() { id = 1101, key = (int)EnumInputType.Right, keyContactType = EnumKeyContactType.Skill, name = "火元素" });
        keyContactData.SetKeyContactStruct((int)EnumInputType.Left, new KeyContactStruct() { id = 1004, key = (int)EnumInputType.Left, keyContactType = EnumKeyContactType.Skill, name = "魔力导向" });
        keyContactData.SetKeyContactStruct((int)EnumInputType.Down, new KeyContactStruct() { id = 1201, key = (int)EnumInputType.Down, keyContactType = EnumKeyContactType.Skill, name = "连续魔力导向" });
        keyContactData.SetKeyContactStruct((int)EnumInputType.X, new KeyContactStruct() { id = 201, key = (int)EnumInputType.X, keyContactType = EnumKeyContactType.Skill, name = "普通攻击" });
        /**********/
        //切换场景
        if (string.IsNullOrEmpty(playerState.Scene))
        {
            playerState.Scene = "中央王国";
            playerState.Location = new Vector3(813.9f, 36.64f, 909.7f);
        }
        isLoadedScene = true;
        IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
        iGameState.ChangedScene(playerState.Scene, playerState.Location, result => isLoadedScene = false);

    }


}
