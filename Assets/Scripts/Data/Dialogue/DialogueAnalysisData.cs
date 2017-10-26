using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Text;

/// <summary>
/// 解析对话数据类
/// </summary>
public class DialogueAnalysisData
{
    const string StartItemFlag = "[Start]";
    const string EndItemFlag = "[End]";

    /// <summary>
    /// 对话数据字典
    /// key 对话id
    /// value 对话内容
    /// </summary>
    private Dictionary<int, DialogueValue> dialogueValueDic;
    /// <summary>
    /// 对话条件集合
    /// </summary>
    List<DialogueCondition> dialogueConditionList;

    public DialogueAnalysisData()
    {
        dialogueValueDic = new Dictionary<int, DialogueValue>();
        dialogueConditionList = new List<DialogueCondition>();
    }

    /// <summary>
    /// 重设数据
    /// </summary>
    /// <param name="dialogueValueDic"></param>
    /// <param name="dialogueConditionList"></param>
    public void ResetData(List<DialogueCondition> dialogueConditionList, Dictionary<int, DialogueValue> dialogueValueDic)
    {
        if (dialogueValueDic == null)
            this.dialogueValueDic = new Dictionary<int, DialogueValue>();
        else
            this.dialogueValueDic = dialogueValueDic;
        if (dialogueConditionList == null)
            this.dialogueConditionList = new List<DialogueCondition>();
        else
            this.dialogueConditionList = dialogueConditionList;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="relationValues">关系以及条件数据</param>
    /// <param name="dialogueValues">对话数据</param>
    public void GetData(out string relationValues, out string dialogueValues)
    {
        relationValues = "";
        dialogueValues = "";
        //对话条件
        foreach (DialogueCondition dialogueCondition in dialogueConditionList)
        {
            relationValues += StartItemFlag + "\r\n";
            relationValues += dialogueCondition.ToString() + "\r\n";
            relationValues += EndItemFlag + "\r\n";
        }
        //对话内容
        foreach (KeyValuePair<int, DialogueValue> dialogueValueData in dialogueValueDic)
        {
            dialogueValues += StartItemFlag + "\r\n";
            dialogueValues += dialogueValueData.Value.ToString() + "\r\n";
            dialogueValues += EndItemFlag + "\r\n";
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="relationValues">关系以及条件数据</param>
    /// <param name="dialogueValues">对话数据</param>
    /// <returns>返回最大的id</returns>
    public int ReadData(string relationValues, string dialogueValues)
    {
        dialogueValueDic = new Dictionary<int, DialogueValue>();
        dialogueConditionList = new List<DialogueCondition>();
        //处理关系
        byte[] relationValueBytes = Encoding.UTF8.GetBytes(relationValues);
        MemoryStream ms_relation = new MemoryStream(relationValueBytes);
        using (StreamReader sr = new StreamReader(ms_relation))
        {
            string readLine = null;
            DialogueCondition dialogueCondition = null;
            string dialogueStr = "";
            while ((readLine = sr.ReadLine()) != null)
            {
                switch (readLine.Trim())
                {
                    case StartItemFlag:
                        dialogueCondition = new DialogueCondition();
                        break;
                    case EndItemFlag:
                        if (dialogueCondition != null)
                        {
                            dialogueCondition.SetData(dialogueStr);
                            dialogueConditionList.Add(dialogueCondition);
                            dialogueStr = "";
                            dialogueCondition = null;
                        }
                        break;
                    default:
                        dialogueStr += readLine + "\r\n";
                        break;
                }
            }
        }
        int maxID = 0;
        //处理对话数据
        byte[] dataValueBytes = Encoding.UTF8.GetBytes(dialogueValues);
        MemoryStream ms_data = new MemoryStream(dataValueBytes);
        using (StreamReader sr = new StreamReader(ms_data))
        {
            string readLine = null;
            DialogueValue dialogueValue = null;
            string dialogueStr = "";
            while ((readLine = sr.ReadLine()) != null)
            {
                switch (readLine.Trim())
                {
                    case StartItemFlag:
                        dialogueValue = new DialogueValue();
                        break;
                    case EndItemFlag:
                        if (dialogueValue != null)
                        {
                            int id = dialogueValue.SetData(dialogueStr);
                            if (!dialogueValueDic.ContainsKey(id))
                                dialogueValueDic.Add(id, dialogueValue);
                            maxID = maxID > id ? maxID : id;
                            dialogueStr = "";
                            dialogueValue = null;
                        }
                        break;
                    default:
                        dialogueStr += readLine + "\r\n";
                        break;
                }
            }
        }

        return maxID;
    }



    /// <summary>
    /// 通过NPCid获取多个对话条件
    /// </summary>
    /// <param name="npcID">npcid</param>
    /// <returns></returns>
    public DialogueCondition[] GetDialogueConditionIDByNPCID(int npcID)
    {
        DialogueCondition[] result = dialogueConditionList.Where(temp => temp.touchNPCID == npcID).ToArray();
        return result;
    }

    /// <summary>
    /// 通过对话id获取对话条件
    /// </summary>
    /// <param name="id">对话id</param>
    /// <returns></returns>
    public DialogueCondition GetDialogueConditionIDByID(int id)
    {
        return dialogueConditionList.Where(temp => temp.topPoint != null && temp.topPoint.dialogueID == id).FirstOrDefault();
    }

    /// <summary>
    /// 通过对话ID获取一个对话内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DialogueValue GetDialoguePointByID(int id)
    {
        return dialogueValueDic.ContainsKey(id) ? dialogueValueDic[id] : null;
    }

    /// <summary>
    /// 获取所有的对话关系id
    /// 注意该id是对话关系的直接对话内容id，如果该关系没有对话内容，则不会返回
    /// </summary>
    public int[] GetDialogueConditionAllID
    {
        get
        {
            return dialogueConditionList.Select(temp => temp.topPoint).Where(temp => temp != null).Select(temp => temp.dialogueID).ToArray();
        }
    }
}




/// <summary>
/// 对话类型
/// </summary>
public enum EnumDialogueType
{
    /// <summary>
    /// 正常的头顶显示对话
    /// </summary>
    [FieldExplan("正常")]
    Normal,
    /// <summary>
    /// 任务（单链式对话）
    /// 如果此时包含分支，则分支的titleValue字段表示最终任务的选项
    /// </summary>
    [FieldExplan("任务")]
    Task,
    /// <summary>
    /// 询问式，包含分支
    /// </summary>
    [FieldExplan("分支")]
    Ask
}

/// <summary>
/// 对话条件
/// </summary>
public class DialogueCondition
{
    /// <summary>
    /// 关系结构的每一个数据都是由=号分割的
    /// </summary>
    static string[] relationValueSplit = new string[] { "==>" };

    static string[] relationLineSplit = new string[] { "\r\n" };

    /// <summary>
    /// 一个对话节点
    /// </summary>
    public DialoguePoint topPoint;
    /// <summary>
    /// 对话名
    /// </summary>
    public string text;
    /// <summary>
    /// 点击哪个id的NPC可以触发该条件
    /// -1表示不使用该判断 
    /// </summary>
    public int touchNPCID;
    /// <summary>
    /// 对话类型
    /// </summary>
    public EnumDialogueType enumDialogueType;
    /// <summary>
    /// 最大等级
    /// -1表示不计算最大等级
    /// </summary>
    public int maxLevel;
    /// <summary>
    /// 最小等级
    /// -1表示不计算最小等级
    /// </summary>
    public int minLevel;
    /// <summary>
    /// 是否已经完成了指定任务
    /// -1表示不计算是否完成任务
    /// </summary>
    public int overTask;
    /// <summary>
    /// 最小善恶值
    /// 如果为null表示不计算最小善恶值
    /// </summary>
    public int minGoodAndEvil;
    /// <summary>
    /// 最大善恶值
    /// 如果为null表示不计算最大善恶值
    /// </summary>
    public int maxGoodAndEvil;
    /// <summary>
    /// 可供选择的种族，如果为null或者长度为0，则任何种族都可以
    /// </summary>
    public RoleOfRace race;

    /// <summary>
    /// 返回数据字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string result = "";
        if (topPoint == null)
            result += "topPoint" + relationValueSplit[0] + relationLineSplit[0];
        else
            result += "topPoint" + relationValueSplit[0] + topPoint.ToString() + relationLineSplit[0];
        result += "text" + relationValueSplit[0] + text + relationLineSplit[0];
        result += "touchNPCID" + relationValueSplit[0] + touchNPCID + relationLineSplit[0];
        result += "enumDialogueType" + relationValueSplit[0] + enumDialogueType.ToString() + relationLineSplit[0];
        result += "maxLevel" + relationValueSplit[0] + maxLevel + relationLineSplit[0];
        result += "minLevel" + relationValueSplit[0] + minLevel + relationLineSplit[0];
        result += "overTask" + relationValueSplit[0] + overTask + relationLineSplit[0];
        result += "minGoodAndEvil" + relationValueSplit[0] + minGoodAndEvil + relationLineSplit[0];
        result += "maxGoodAndEvil" + relationValueSplit[0] + maxGoodAndEvil + relationLineSplit[0];
        result += "race" + relationValueSplit[0] + race.ToString() + relationLineSplit[0];
        return result;
    }

    /// <summary>
    /// 设置数据
    /// 传入ToString函数返回的字符串
    /// </summary>
    /// <param name="data">数据字符串</param>
    public void SetData(string data)
    {
        if (string.IsNullOrEmpty(data))
            return;
        string[] lines = data.Split(relationLineSplit, StringSplitOptions.RemoveEmptyEntries);
        string[][] valuesArray = lines.Select(temp => temp.Split(relationValueSplit, StringSplitOptions.RemoveEmptyEntries))
            .Where(temp => temp.Length == 2).ToArray();
        foreach (string[] values in valuesArray)
        {
            switch (values[0])
            {
                case "topPoint":
                    if (!string.IsNullOrEmpty(values[1]))
                    {
                        topPoint = new DialoguePoint();
                        topPoint.SetData(values[1].Trim());
                    }
                    break;
                case "text":
                    text = values[1].Trim();
                    break;
                case "touchNPCID":
                    int.TryParse(values[1].Trim(), out touchNPCID);
                    break;
                case "enumDialogueType":
                    try { enumDialogueType = (EnumDialogueType)Enum.Parse(typeof(EnumDialogueType), values[1].Trim()); } catch { }
                    break;
                case "maxLevel":
                    int.TryParse(values[1].Trim(), out maxLevel);
                    break;
                case "minLevel":
                    int.TryParse(values[1].Trim(), out minLevel);
                    break;
                case "overTask":
                    int.TryParse(values[1].Trim(), out overTask);
                    break;
                case "minGoodAndEvil":
                    int.TryParse(values[1].Trim(), out minGoodAndEvil);
                    break;
                case "maxGoodAndEvil":
                    int.TryParse(values[1].Trim(), out maxGoodAndEvil);
                    break;
                case "race":
                    try { race = (RoleOfRace)Enum.Parse(typeof(RoleOfRace), values[1].Trim()); } catch { }
                    break;
            }
        }
    }
}

/// <summary>
/// 对话节点
/// </summary>
public class DialoguePoint
{
    static string idSplits = "=";

    /// <summary>
    /// 本节点ID
    /// </summary>
    public int dialogueID;

    /// <summary>
    /// 子节点
    /// </summary>
    public DialoguePoint[] childDialoguePoints;

    /// <summary>
    /// 返回数据字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string result = "[" + dialogueID + idSplits;
        if (childDialoguePoints != null)
        {
            foreach (DialoguePoint dialoguePoint in childDialoguePoints)
            {
                result += "{";
                result += dialoguePoint.ToString();
                result += "}";
            }
        }
        result += "]";
        return result;
    }

    /// <summary>
    /// 设置数据
    /// 传入ToString函数返回的字符串
    /// </summary>
    /// <param name="data">数据字符串</param>
    public void SetData(string data)
    {
        if (data.Length > 2)
        {
            data = data.Trim().Remove(0, 1);
            data = data.Remove(data.Length - 1, 1);
            int splitIndex = data.IndexOf(idSplits);
            if (splitIndex > 0)
            {
                bool canSetID = int.TryParse(data.Substring(0, splitIndex), out dialogueID);
                if (canSetID)
                {
                    string nextStr = data.Substring(splitIndex + 1, data.Length - (splitIndex + 1));
                    int[][] splitArray = GetNextStringSplit(nextStr);
                    childDialoguePoints = new DialoguePoint[splitArray.GetLength(0)];
                    for (int i = 0; i < childDialoguePoints.Length; i++)
                    {
                        childDialoguePoints[i] = new DialoguePoint();
                        childDialoguePoints[i].SetData(nextStr.Substring(splitArray[i][0], splitArray[i][1]));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 通过字符串获取下面节点的字符串分割数据
    /// </summary>
    /// <param name="nextStr">之后节点的字符串</param>
    /// <returns></returns>
    private int[][] GetNextStringSplit(string nextStr)
    {
        List<int[]> resultList = new List<int[]>();//每个数组有两个元素，第一个表示下标，第二个表示位置
        int index = 0;
        int moveLength = 0;
        int flag = 0;//遇到{符号该变量加1，遇到}符号该变量减1，如果此时flag等于0，则一个截取范围确定
        for (int i = 0; i < nextStr.Length; i++)
        {
            moveLength++;
            switch (nextStr[i])
            {
                case '{':
                    flag++;
                    break;
                case '}':
                    flag--;
                    if (flag == 0)
                    {
                        resultList.Add(new[] { index + 1, moveLength - 2 });
                        index = i + 1;
                        moveLength = 0;
                    }
                    break;
            }
        }
        return resultList.ToArray();
    }
}

/// <summary>
/// 对话数据
/// </summary>
public class DialogueValue
{
    /// <summary>
    /// 字段与数据分割符数组
    /// </summary>
    static string[] dataValueSplit = new string[] { "==>" };
    /// <summary>
    /// 每行信息分割数组
    /// </summary>
    static string[] dataLineSplit = new string[] { "!@#$%^&*" };

    /// <summary>
    /// 对话的id
    /// </summary>
    public int dialogueID;

    /// <summary>
    /// NPC的id
    /// </summary>
    public int npcID;

    /// <summary>
    /// 配音id
    /// </summary>
    public int voiceID;

    /// <summary>
    /// 标题值（只有在对话本身是有分支选项时才有用）
    /// 标识的是该对话在上层节点中显示的文字
    /// </summary>
    public string titleValue;

    /// <summary>
    /// 该节点显示的对话文字
    /// </summary>
    public string showValue;

    /// <summary>
    /// 附加数据
    /// </summary>
    public string otherValue;

    /// <summary>
    /// 返回数据字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string result = "";
        result += "dialogueID" + dataValueSplit[0] + dialogueID + dataLineSplit[0];
        result += "npcID" + dataValueSplit[0] + npcID + dataLineSplit[0];
        result += "voiceID" + dataValueSplit[0] + voiceID + dataLineSplit[0];
        result += "titleValue" + dataValueSplit[0] + titleValue + dataLineSplit[0];
        result += "showValue" + dataValueSplit[0] + showValue + dataLineSplit[0];
        result += "otherValue" + dataValueSplit[0] + otherValue + dataLineSplit[0];
        return result;
    }

    /// <summary>
    /// 设置数据
    /// 传入ToString函数返回的字符串
    /// </summary>
    /// <param name="data">数据字符串</param>
    public int SetData(string data)
    {
        string[] lines = data.Split(dataLineSplit, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] values = line.Split(dataValueSplit, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 2)
            {
                switch (values[0])
                {
                    case "dialogueID":
                        int.TryParse(values[1].Trim(), out dialogueID);
                        break;
                    case "npcID":
                        int.TryParse(values[1].Trim(), out npcID);
                        break;
                    case "voiceID":
                        int.TryParse(values[1].Trim(), out voiceID);
                        break;
                    case "titleValue":
                        titleValue = values[1].Trim();
                        break;
                    case "showValue":
                        showValue = values[1].Trim();
                        break;
                    case "otherValue":
                        otherValue = values[1].Trim();
                        break;
                }
            }
        }
        return dialogueID;
    }
}
