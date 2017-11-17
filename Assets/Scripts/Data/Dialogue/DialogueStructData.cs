using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 对话结构数据
/// </summary>
public class DialogueStructData:ILoadable<DialogueStructData>
{
    /// <summary>
    /// 对话结构数据的私有构造函数
    /// </summary>
    public DialogueStructData()
    {
        dialogueAnalysisData = new DialogueAnalysisData();
    }

    /// <summary>
    /// 实现ILoadable接口
    /// </summary>
    public void Load()
    {
        ReadDialogueStructData(true);
    }

    /// <summary>
    /// 对话文件解析
    /// </summary>
    DialogueAnalysisData dialogueAnalysisData;

    /// <summary>
    /// 从文件读取对话结构数据
    /// </summary>
    /// <param name="must">是否必须读取</param>
    public void ReadDialogueStructData(bool must = false)
    {
        if (dialogueAnalysisData == null || must)
        {
            TextAsset dialogueTextAsset = Resources.Load<TextAsset>("Data/Dialogue/Dialogue");
            if (dialogueTextAsset != null)
            {
                string[] pathNames = dialogueTextAsset.text.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (pathNames.Length == 2)
                {
                    TextAsset dialogueConditionTextAsset = Resources.Load<TextAsset>("Data/Dialogue/" + pathNames[0]);
                    TextAsset dialogueValueTextAsset = Resources.Load<TextAsset>("Data/Dialogue/" + pathNames[1]);
                    if (dialogueConditionTextAsset != null && dialogueValueTextAsset != null)
                    {
                        dialogueAnalysisData.ReadData(dialogueConditionTextAsset.text, dialogueValueTextAsset.text);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 通过npcID获取所有对该npc的可用条件（内部包含判断条件以及其实的对话数据）
    /// 如果npcID是-1则表示这个对话不需要点击NPC
    /// </summary>
    /// <param name="npcID">npcID</param>
    /// <param name="selector">选择器</param>
    /// <returns></returns>
    public DialogueCondition[] SearchDialogueConditionsByNPCID(int npcID, Func<DialogueCondition, bool> selector = null)
    {
        return dialogueAnalysisData.GetDialogueConditionIDByNPCID(npcID).Where(temp => selector == null ? true : selector(temp)).ToArray();
    }

    /// <summary>
    /// 通过对话id获取直接包含该id的可用条件
    /// </summary>
    /// <param name="dialogueValueID">对话id</param>
    /// <returns></returns>
    public DialogueCondition SearchDialogueConditionByID(int dialogueValueID)
    {
        return dialogueAnalysisData.GetDialogueConditionIDByID(dialogueValueID);
    }

    /// <summary>
    /// 通过对话id获取对话数据
    /// </summary>
    /// <param name="dialogueValueID">对话的id，可以通过对话条件获取，也可以使用确定好的id</param>
    /// <returns></returns>
    public DialogueValue SearchDialogueValueByID(int dialogueValueID)
    {
        return dialogueAnalysisData.GetDialoguePointByID(dialogueValueID);
    }


}
