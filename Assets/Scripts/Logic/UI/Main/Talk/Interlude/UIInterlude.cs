using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主线过场对话
/// </summary>
public class UIInterlude : MonoBehaviour
{
    /// <summary>
    /// 左侧图像
    /// </summary>
    [SerializeField]
    Image leftImage;
    /// <summary>
    /// 左侧文字显示 
    /// </summary>
    [SerializeField]
    Text leftText;

    /// <summary>
    /// 右侧图像
    /// </summary>
    [SerializeField]
    Image rightImage;
    /// <summary>
    /// 右侧文字显示 
    /// </summary>
    [SerializeField]
    Text rightText;

    /// <summary>
    /// 对话数据对象
    /// </summary>
    DialogueStructData dialogueStructData;
    /// <summary>
    /// 交互状态接口对象
    /// </summary>
    IInteractiveState iInteractiveState;
    /// <summary>
    /// 运行时任务管理对象 
    /// </summary>
    RuntimeTasksData runtimeTasksData;
    /// <summary>
    /// 当前展示的任务信息对象
    /// </summary>
    RunTimeTaskInfo runTimeTaskInfo;
    /// <summary>
    /// 当前任务状态对象
    /// </summary>
    INowTaskState iNowTaskState;
    /// <summary>
    /// 游戏状态
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 对话条件对象 
    /// </summary>
    DialogueCondition dialogueCodition;
    /// <summary>
    /// 当前的对话的节点
    /// </summary>
    DialoguePoint nowDialoguePoint;

    /// <summary>
    /// 显示在左侧还是显示在右侧
    /// false表示左侧 true表示右侧
    /// </summary>
    bool showLeftOrRight;

    /// <summary>
    /// 之前的游戏状态
    /// </summary>
    EnumGameRunType oldGameRunType;

    private void OnEnable()
    {
        dialogueStructData = DataCenter.Instance.GetMetaData<DialogueStructData>();
        runtimeTasksData = DataCenter.Instance.GetEntity<RuntimeTasksData>();
        iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        iNowTaskState = GameState.Instance.GetEntity<INowTaskState>();
        iGameState = GameState.Instance.GetEntity<IGameState>();
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        InitTalk();
    }

    /// <summary>
    /// 初始化对话
    /// </summary>
    private void InitTalk()
    {
        oldGameRunType = iGameState.GameRunType;
        int touchNPCID = iInteractiveState.ClickInteractiveNPCID;
        RunTimeTaskInfo[] runTimeTaskInfos = runtimeTasksData.GetAllToDoList()
                       .Where(temp => temp.RunTimeTaskNode.ReceiveTaskNpcId == touchNPCID && temp.IsOver == false && temp.IsStart == false)
                       .ToArray();
        RunTimeTaskInfo runTimeTaskInfo = runTimeTaskInfos.Where(temp => temp.RunTimeTaskNode.TaskType == Enums.TaskType.PrincipalLine).FirstOrDefault();
        if (runTimeTaskInfo != null)
        {
            this.runTimeTaskInfo = runTimeTaskInfo;
            this.dialogueCodition = dialogueStructData.SearchDialogueConditionsByNPCID(runTimeTaskInfo.RunTimeTaskNode.ReceiveTaskNpcId,
                temp => temp.enumDialogueType == EnumDialogueType.Task).FirstOrDefault();
            if (this.dialogueCodition != null)
            {
                this.nowDialoguePoint = this.dialogueCodition.topPoint; 
                showLeftOrRight = false;
                iGameState.GameRunType = EnumGameRunType.TaskTalk;
                ShowTalk();
            }
            else gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 展示对话
    /// </summary>
    private void ShowTalk()
    {
        //通过npc的id获取显示的图片
        Sprite showImage = null;
        DialogueValue nowDialogueValue = dialogueStructData.SearchDialogueValueByID(nowDialoguePoint.dialogueID);
        string showText = nowDialogueValue.showValue;
        if (showLeftOrRight)
        {
            rightImage.gameObject.SetActive(false);
            leftImage.gameObject.SetActive(true);
            leftImage.sprite = showImage;
            leftText.text = showText;
        }
        else
        {
            rightImage.gameObject.SetActive(true);
            leftImage.gameObject.SetActive(false);
            rightImage.sprite = showImage;
            rightText.text = showText;
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        iGameState.GameRunType = oldGameRunType;
    }

    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.A:
            case UIManager.KeyType.B:
                NextTalkOrEnd();
                break;
        }
    }

    /// <summary>
    /// 点击确认
    /// </summary>
    public void ClickOK()
    {
        NextTalkOrEnd();
    }

    /// <summary>
    /// 下一个任务或结束任务
    /// </summary>
    void NextTalkOrEnd()
    {
        showLeftOrRight = !showLeftOrRight;
        if (nowDialoguePoint.childDialoguePoints != null && nowDialoguePoint.childDialoguePoints.Length > 0)
        {
            nowDialoguePoint = nowDialoguePoint.childDialoguePoints[0];
            ShowTalk();
        }
        else
        {
            gameObject.SetActive(false);
            //接受任务
            iNowTaskState.StartTask(runTimeTaskInfo.ID);
        }
    }
}
