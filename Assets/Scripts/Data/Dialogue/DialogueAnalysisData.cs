using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// 解析对话数据类
/// </summary>
public class DialogueAnalysisData
{
    /// <summary>
    /// 对话数据字典
    /// </summary>
    private Dictionary<int, DialoguePoint> dialoguePointDic;
    /// <summary>
    /// 对话条件字典（这里的对话id都是顶层的id）
    /// </summary>
    private Dictionary<int, DialogueCondition> dialogueConditionDic;

    /// <summary>
    /// 根据等级检测方法
    /// </summary>
    private Func<int, bool> CheckLevelFunc;

    public DialogueAnalysisData()
    {
        dialoguePointDic = new Dictionary<int, DialoguePoint>();
        dialogueConditionDic = new Dictionary<int, DialogueCondition>();
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="relationValue">关系以及条件数据</param>
    /// <param name="dialogueValues">对话数据</param>
    public void ReadData(string relationValue,string[] dialogueValues) { }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="relationValue">关系数据</param>
    /// <param name="dialogueValues">对话数据</param>
    public void SaveData(out string relationValue, out string[] dialogueValues)
    {
        relationValue = "";
        dialogueValues = new string[0];
    }
}

/// <summary>
/// 对话条件
/// </summary>
public class DialogueCondition
{
    /// <summary>
    /// 对话的顶级节点id
    /// </summary>
    public int topID;
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
    public int? minGoodAndEvil;
    /// <summary>
    /// 最大善恶值
    /// 如果为null表示不计算最大善恶值
    /// </summary>
    public int? maxGoodAndEvil;
    /// <summary>
    /// 可供选择的种族，如果为null或者为0，则任何种族都可以
    /// 后期需要替换为具体的枚举类型
    /// </summary>
    public Enum[] race;

    /// <summary>
    /// 检测函数
    /// </summary>
    Func<DialogueCondition,bool> checkFunc;

    public DialogueCondition()
    {
        checkFunc = (temp) => true;
    }

    /// <summary>
    /// 添加检测函数
    /// </summary>
    /// <param name="checkFunc">检测函数</param>
    public void AddCheckFunc(Func<DialogueCondition, bool> checkFunc)
    {
        RemoveCheckFunc(checkFunc);
        this.checkFunc += checkFunc;
    }

    /// <summary>
    /// 移除检测函数
    /// </summary>
    /// <param name="checkFunc"></param>
    public void RemoveCheckFunc(Func<DialogueCondition, bool> checkFunc)
    {
        try
        {
            this.checkFunc -= checkFunc;
        }
        catch { }
    }

    /// <summary>
    /// 是否可以选择该对象
    /// </summary>
    /// <returns></returns>
    public bool CanSelect()
    {
        List<bool> result = new List<bool>();
        checkFunc.GetInvocationList().Select(temp=>temp as Func<DialogueCondition,bool>).ToList().ForEach(temp => result.Add(temp(this)));
        int length = result.Where(temp => !temp).Count();
        return length == 0;
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
    Normal,
    /// <summary>
    /// 任务（单链式对话）
    /// 如果此时包含分支，则分支的titleValue字段表示最终任务的选项
    /// </summary>
    Task,
    /// <summary>
    /// 询问式，包含分支
    /// </summary>
    Ask
}

/// <summary>
/// 对话节点
/// </summary>
public class DialoguePoint
{
    /// <summary>
    /// 对话节点的id
    /// </summary>
    public int dialogueID;

    /// <summary>
    /// NPC的id
    /// </summary>
    public int npcID;

    /// <summary>
    /// 标题值（只有在对话本身是有分支选项时才有用）
    /// 标识的是该对话在上层节点中显示的文字
    /// </summary>
    public string titleValue;

    /// <summary>
    /// 接下来的对话id
    /// </summary>
    public int[] nextID;

    /// <summary>
    /// 该节点显示的对话文字
    /// </summary>
    public string showValue;

    /// <summary>
    /// 附加数据
    /// </summary>
    public string otherValue;
}
