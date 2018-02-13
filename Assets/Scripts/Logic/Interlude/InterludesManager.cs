using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 过场控制器
/// </summary>
public class InterludesManager : IEntrance
{

    /// <summary>
    /// 游戏状态对象 
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 玩家状态对象
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 黑背景土坯那
    /// </summary>
    Texture2D blackTexture;

    /// <summary>
    /// 执行任务过程对象
    /// 主要用于协助检测对话
    /// </summary>
    RunTaskStruct runTaskStruct;

    /// <summary>
    /// 对话数据对象
    /// </summary>
    DialogueStructData dialogueStructData;

    /// <summary>
    /// npcid对应显示文字的游戏对象字典
    /// </summary>
    Dictionary<int, GameObject> npcIDToShowObjDic;

    /// <summary>
    /// npcid对应对话显示控制数据字典
    /// </summary>
    Dictionary<int, TalkShowStruct> npcIDToTalkShowStructDic;

    /// <summary>
    /// 显示对话的例子
    /// </summary>
    GameObject talkShowExplanObj;

    /// <summary>
    /// npc数据
    /// </summary>
    NPCData npcData;

    public void Start()
    {
        npcIDToTalkShowStructDic = new Dictionary<int, TalkShowStruct>();
        npcIDToShowObjDic = new Dictionary<int, GameObject>();
        GameState.Instance.Registor<INowTaskState>(INowTaskStateChanged);
        iGameState = GameState.Instance.GetEntity<IGameState>();
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        blackTexture = Resources.Load<Texture2D>("Sprites/black");
        dialogueStructData = DataCenter.Instance.GetMetaData<DialogueStructData>();
        talkShowExplanObj = Resources.Load<GameObject>("UI/NPCTalkCanvas");
        npcData = DataCenter.Instance.GetMetaData<NPCData>();
        //获取一个执行任务过程对象
        runTaskStruct = TaskTools.Instance.GetRunTaskStruct();
    }

    public void OnDestroy()
    {
        TaskTools.Instance.RemoveRunTaskStruct(runTaskStruct);
    }

    /// <summary>
    /// 运行过程的携程
    /// </summary>
    IEnumerator interludesEnumerator;

    /// <summary>
    /// onGUI的alpha通道值
    /// </summary>
    float onGUIAlpha = 0;

    private void INowTaskStateChanged(INowTaskState iNowTaskState, string targetName)
    {
        //接受任务时调用
        if (string.Equals(targetName, GameState.Instance.GetFieldName<INowTaskState, int>(temp => temp.StartTask)))
        {
            InterludesData interludesData = DataCenter.Instance.GetMetaData<InterludesData>();
            InterludesItemStruct interludesItemStruct = interludesData.GetInterludesItemStructByTaskID(iNowTaskState.StartTask);
            if (interludesItemStruct != null)
            {
                StartInterludes(interludesItemStruct);
            }
            else
            {
                Debug.Log("该任务没有过场动画");
            }
        }
        //完成任务时调用
        else if (string.Equals(targetName, GameState.Instance.GetFieldName<INowTaskState, int>(temp => temp.OverTaskID)))
        {
            InterludesData interludesData = DataCenter.Instance.GetMetaData<InterludesData>();
            InterludesItemStruct interludesItemStruct = interludesData.GetInterludesItemStructByTaskID(iNowTaskState.OverTaskID, InterludesItemStruct.EnumInterludesShowTime.Over);
            if (interludesItemStruct != null)
            {
                StartInterludes(interludesItemStruct);
            }
            else
            {
                Debug.Log("该任务没有过场动画");
            }
        }

    }

    /// <summary>
    /// 开始过场动画
    /// </summary>
    /// <param name="interludesItemStruct"></param>
    private void StartInterludes(InterludesItemStruct interludesItemStruct)
    {
        if (interludesEnumerator == null && interludesItemStruct != null)
        {
            iGameState.PushEnumGameRunType(EnumGameRunType.Interludes);
            interludesEnumerator = RunningInterludes(interludesItemStruct);
        }
    }

    /// <summary>
    /// 过场动画运行时携程(只能返回true或false)
    /// </summary>
    /// <returns></returns>
    IEnumerator RunningInterludes(InterludesItemStruct interludesItemStruct)
    {
        float runTime = 0;
        CanvasGroup canvasGroup = null;
        if (iGameState.InterludesPanel != null)
        {
            canvasGroup = iGameState.InterludesPanel.GetComponent<CanvasGroup>();
        }
        iGameState.InterludesPanel.enabled = true;
        //开场先显示黑幕
        while (runTime < 1)
        {
            runTime += Time.deltaTime;
            if (canvasGroup)
            {
                canvasGroup.alpha = runTime;
            }
            yield return false;
        }
        canvasGroup.alpha = 1;
        //执行数据的读取和初始化操作
        iGameState.ActionPanel.enabled = false;
        iGameState.SettingPanel.enabled = false;
        iGameState.MainPanel.enabled = false;
        iGameState.InterludesCamera.gameObject.SetActive(true);
        iGameState.InterludesCamera.transform.position = iPlayerState.PlayerCamera.transform.position;
        iGameState.InterludesCamera.transform.eulerAngles = iPlayerState.PlayerCamera.transform.eulerAngles;
        //将黑幕隐藏同时开始控制播放
        bool interludesIsOver = false;
        runTime = 0;
        List<GameObject> rubbishList = new List<GameObject>();//用于存放临时生成的垃圾
        int interludesIndex = -1;//当前的过场项目下标
        Action WaitOverAction = null;//等待持续时间结束委托对象
        while (runTime < 1 || !interludesIsOver || WaitOverAction != null)
        {
            runTime += Time.deltaTime;
            if (canvasGroup)
            {
                float alpha = Mathf.Clamp(1 - runTime, 0, 1);
                canvasGroup.alpha = alpha;
            }
            if (!interludesIsOver)
            {
                //具体的控制逻辑
                interludesIsOver = true;
                interludesIndex++;
                runTaskStruct.StopTask();
                if (interludesIndex < interludesItemStruct.InterludesDataInfo.Datas.Count)//如果还有过场数据则取出来
                {
                    InterludesDataInfo.ItemData itemData = interludesItemStruct.InterludesDataInfo.Datas[interludesIndex];
                    float keepTime = itemData.baseKeepTime;
                    //设置持续时间委托对象的实体
                    WaitOverAction = () =>
                    {
                        keepTime -= Time.deltaTime;
                        if (keepTime < 0)
                        {
                            interludesIsOver = false;
                            WaitOverAction = null;
                        }
                    };
                    //根据不同的类型将数值传递给指定对象
                    //如果是对话则传递给对话控制
                    if (string.Equals(itemData.GetType().Name, typeof(InterludesDataInfo.ItemData_Talk).Name))
                    {
                        //获取对话
                        InterludesDataInfo.ItemData_Talk itemData_talk = itemData as InterludesDataInfo.ItemData_Talk;
                        DialogueCondition talkCondition = dialogueStructData.SearchDialogueConditionByID(itemData_talk.TalkID);
                        ShowTalk(talkCondition);
                        runTaskStruct.StartTask(0f, CheckTalk, -1, true);
                    }
                    else if (string.Equals(itemData.GetType().Name, typeof(InterludesDataInfo.ItemData_CameraPathAnimation).Name))
                    {
                        //获取镜头动画
                        InterludesDataInfo.ItemData_CameraPathAnimation itemData_CameraPathAnimation = itemData as InterludesDataInfo.ItemData_CameraPathAnimation;
                        if (itemData_CameraPathAnimation.ObjPrefab)
                        {
                            GameObject cameraPathAniamtionObj = GameObject.Instantiate<GameObject>(itemData_CameraPathAnimation.ObjPrefab);
                            rubbishList.Add(cameraPathAniamtionObj);
                            CameraPathAnimator cameraPathAniamtor = cameraPathAniamtionObj.GetComponent<CameraPathAnimator>();
                            cameraPathAniamtor.animationObject = iGameState.InterludesCamera.transform;
                            cameraPathAniamtor.Play();
                        }
                    }
                }
                else
                {
                    WaitOverAction = null;
                }
            }
            if (WaitOverAction != null)
                WaitOverAction();
            yield return false;
        }
        //闭幕前显示黑幕
        while (runTime < 1)
        {
            runTime += Time.deltaTime;
            if (canvasGroup)
            {
                canvasGroup.alpha = runTime;
            }
            yield return false;
        }
        canvasGroup.alpha = 1;
        //初始化摄像机等数据
        iGameState.ActionPanel.enabled = true;
        iGameState.SettingPanel.enabled = true;
        iGameState.MainPanel.enabled = true;
        iGameState.InterludesCamera.gameObject.SetActive(false);
        foreach (var item in rubbishList)
        {
            GameObject.Destroy(item);
        }
        //将黑幕隐藏
        runTime = 0;
        while (runTime < 1)
        {
            runTime += Time.deltaTime;
            if (canvasGroup)
            {
                canvasGroup.alpha = 1 - runTime;
            }
            yield return false;
        }
        canvasGroup.alpha = 0;
        iGameState.InterludesPanel.enabled = false;
    }

    /// <summary>
    /// 显示对话
    /// </summary>
    /// <param name="dialogueCondition">对话的文本</param>
    private void ShowTalk(DialogueCondition dialogueCondition)
    {
        TalkShowStruct talkShowStruct = new TalkShowStruct();
        Queue<int> targetTaskIDs = new Queue<int>();
        Queue<DialoguePoint> targetTaskPoints = new Queue<DialoguePoint>();
        PushDialoguePointTask(dialogueCondition.topPoint, targetTaskIDs, targetTaskPoints);
        float maxTime = 0;
        foreach (DialoguePoint targetTaskPoint in targetTaskPoints)
        {
            DialogueValue dialogueValue = dialogueStructData.SearchDialogueValueByID(targetTaskPoint.dialogueID);
            if (dialogueValue == null)
                continue;
            string otherValue = dialogueValue.otherValue;
            if (string.IsNullOrEmpty(otherValue))
                continue;
            string[] splitsValue = otherValue.Split(',');
            if (splitsValue.Length == 2)
            {
                float startTime = 0;//开始时间
                float intervalTime = 0;//持续时间
                if (!(float.TryParse(splitsValue[0], out startTime) && float.TryParse(splitsValue[1], out intervalTime)))
                    continue;
                float endTime = startTime + intervalTime;
                maxTime = maxTime > endTime ? maxTime : endTime;
                talkShowStruct.pointToCreateTime.Add(targetTaskPoint, startTime);
                talkShowStruct.pointToIntervalTime.Add(targetTaskPoint, intervalTime);
            }
        }
        talkShowStruct.maxTime = maxTime;
        npcIDToTalkShowStructDic.Add(dialogueCondition.touchNPCID, talkShowStruct);
    }

    /// <summary>
    /// 将任务id push到队列中
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetTaskIDs"></param>
    /// <param name="targetTaskPoints"></param>
    private void PushDialoguePointTask(DialoguePoint source, Queue<int> targetTaskIDs = null, Queue<DialoguePoint> targetTaskPoints = null)
    {
        DialogueValue dialogueValue = dialogueStructData.SearchDialogueValueByID(source.dialogueID);
        if (targetTaskPoints != null && !targetTaskPoints.Contains(source))
            targetTaskPoints.Enqueue(source);
        int taskID;
        if (!string.IsNullOrEmpty(dialogueValue.otherValue) && int.TryParse(dialogueValue.otherValue, out taskID))
        {
            if (targetTaskIDs != null && !targetTaskIDs.Contains(taskID))
                targetTaskIDs.Enqueue(taskID);
        }
        if (source.childDialoguePoints != null && source.childDialoguePoints.Length > 0)//如果存在子节点
        {
            foreach (DialoguePoint childPoint in source.childDialoguePoints)
            {
                PushDialoguePointTask(childPoint, targetTaskIDs, targetTaskPoints);
            }
        }
    }


    /// <summary>
    /// 检测对话
    /// </summary>
    private void CheckTalk()
    {
        //创建新的
        var touchIDs = npcIDToTalkShowStructDic.Keys.OfType<int>().ToArray();
        foreach (int touchID in touchIDs)
        {
            TalkShowStruct talkShowStruct = npcIDToTalkShowStructDic[touchID];
            float nextTime = talkShowStruct.nowTime + Time.deltaTime;
            var selectCreates = talkShowStruct.pointToCreateTime.Where(temp => temp.Value >= talkShowStruct.nowTime && temp.Value < nextTime).ToArray();
            talkShowStruct.nowTime = nextTime;
            foreach (var selectCreate in selectCreates)
            {
                DialogueValue dialogueValue = dialogueStructData.SearchDialogueValueByID(selectCreate.Key.dialogueID);
                if (dialogueValue == null)
                    continue;
                int thisDialogueNPCID = dialogueValue.npcID;//在哪个npc头顶显示
                string thisDialogueValue = dialogueValue.showValue;//显示的文字
                float intervalTime = talkShowStruct.pointToIntervalTime[selectCreate.Key];//显示的持续时间
                if (!npcIDToShowObjDic.ContainsKey(thisDialogueNPCID))
                {
                    GameObject createObj = GameObject.Instantiate<GameObject>(talkShowExplanObj);//这是一个UI面板(这个面板一经创建就不会消失了,但是正常情况下是隐藏的,只有当存在显示文字的时候才会显示)
                    npcIDToShowObjDic.Add(thisDialogueNPCID, createObj);
                    //设置位置
                    NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, thisDialogueNPCID);
                    npcDataInfo.Load();
                    if (npcDataInfo != null)
                    {
                        createObj.transform.position = npcDataInfo.NPCObj.transform.position + npcDataInfo.TalkShowOffset;
                    }
                }
                GameObject talkUIObj = npcIDToShowObjDic[thisDialogueNPCID];//取出用于显示文字的ui面板
                UITalkShowControl uiTalkShowControl = talkUIObj.GetComponent<UITalkShowControl>();
                if (uiTalkShowControl != null)
                    uiTalkShowControl.AddNewTalk(thisDialogueValue, intervalTime);

            }
            if (talkShowStruct.nowTime > talkShowStruct.maxTime)
            {
                npcIDToTalkShowStructDic.Remove(touchID);
            }
        }
        if (npcIDToTalkShowStructDic.Count <= 0)
            runTaskStruct.StopTask();
    }

    public void Update()
    {
        if (interludesEnumerator != null)
        {
            if (interludesEnumerator.MoveNext())
            {
                if (object.Equals(interludesEnumerator.Current, true))
                {
                    interludesEnumerator = null;
                    if (iGameState.GameRunType == EnumGameRunType.Interludes)
                        iGameState.PopEnumGameRunType();
                }
            }
            else
            {
                interludesEnumerator = null;
                if (iGameState.GameRunType == EnumGameRunType.Interludes)
                    iGameState.PopEnumGameRunType();
            }
        }
    }


    class TalkShowStruct
    {
        /// <summary>
        /// 节点对应的创建时间字典
        /// </summary>
        public Dictionary<DialoguePoint, float> pointToCreateTime;
        /// <summary>
        /// 节点对应的持续时间字典
        /// </summary>
        public Dictionary<DialoguePoint, float> pointToIntervalTime;
        /// <summary>
        /// 计算出最大存在时间,超时则移除该对象
        /// </summary>
        public float maxTime;
        /// <summary>
        /// 当前对话运转的时间
        /// </summary>
        public float nowTime;

        public TalkShowStruct()
        {
            pointToCreateTime = new Dictionary<DialoguePoint, float>();
            pointToIntervalTime = new Dictionary<DialoguePoint, float>();
        }
    }
}
