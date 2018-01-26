using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  用于展示人物对话
/// </summary>
public class UITalkShow : MonoBehaviour
{

    /// <summary>
    /// 显示对话的例子
    /// </summary>
    [SerializeField]
    GameObject talkShowExplanObj;
    /// <summary>
    /// 交互接口对象
    /// </summary>
    IInteractiveState iInteractiveState;
    /// <summary>
    /// 对话数据对象
    /// </summary>
    DialogueStructData dialogueStructData;
    /// <summary>
    /// 运行时任务管理对象 
    /// </summary>
    TaskMap.RunTimeTaskData runtimeTasksData;
    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;
    /// <summary>
    /// 玩家存档状态
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// npc数据
    /// </summary>
    NPCData npcData;

    /// <summary>
    /// npcid对应对话显示控制数据字典
    /// </summary>
    Dictionary<int, TalkShowStruct> npcIDToTalkShowStructDic;

    /// <summary>
    /// npcid对应显示文字的游戏对象字典
    /// </summary>
    Dictionary<int, GameObject> npcIDToShowObjDic;

    void Start()
    {
        npcIDToTalkShowStructDic = new Dictionary<int, TalkShowStruct>();
        npcIDToShowObjDic = new Dictionary<int, GameObject>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        dialogueStructData = DataCenter.Instance.GetMetaData<DialogueStructData>();
        runtimeTasksData = DataCenter.Instance.GetEntity<TaskMap.RunTimeTaskData>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        npcData = DataCenter.Instance.GetMetaData<NPCData>();
    }


    private void Update()
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
    }

    /// <summary>
    /// 接收到了点击npc的消息
    /// </summary>
    public void ClickInteractiveNPCID()
    {
        if (npcData == null || playerState == null || iInteractiveState == null || npcIDToTalkShowStructDic == null)
            return;
        int thisClickNPCID = iInteractiveState.ClickInteractiveNPCID;
        if (npcIDToTalkShowStructDic.ContainsKey(thisClickNPCID))
            return;
        //取出对话
        DialogueCondition[] allTalkCondition = dialogueStructData.SearchDialogueConditionsByNPCID(thisClickNPCID, temp =>
            temp.enumDialogueType == EnumDialogueType.Normal && //判断是否是对话
            (temp.minLevel < 0 || temp.minLevel <= playerState.Level) && //判断最小等级是否复合
            (temp.maxLevel < 0 || temp.maxLevel >= playerState.Level) && //判断最大等级是否复合
            (temp.overTask < -1 || (runtimeTasksData.GetTasksWithID(temp.overTask, false) != null && (runtimeTasksData.GetTasksWithID(temp.overTask, false).TaskInfoStruct.TaskProgress == TaskMap.Enums.EnumTaskProgress.Sucessed))) && //判断完成任务是否复合
            temp.thisTask < 0//检测不需要任务要求
            );
        if (allTalkCondition != null && allTalkCondition.Length > 0)
        {
            //随机出一个任务,尽量从后往前
            int selectIndex = allTalkCondition.Length - 1;
            while (true)
            {
                float rangeValue = Random.Range(0, 10);
                if (rangeValue > 2)
                    break;
                selectIndex--;
                if (selectIndex < 0)
                    selectIndex = allTalkCondition.Length - 1;
            }
            DialogueCondition selectTalkCondition = allTalkCondition[selectIndex];
            ShowTalk(selectTalkCondition);
        }
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
    /// 对话显示结构
    /// </summary>
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
