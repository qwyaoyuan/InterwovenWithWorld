using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// 解析技能数据类
/// </summary>
public class SkillAnalysisData
{

    /// <summary>
    /// 技能结构字典
    /// 第一层 ID to Data（key为ID value为整合了所有的要素）
    /// 第二层 Field to Data（key为字段名 value为该字段的值）
    /// </summary>
    Dictionary<string, Dictionary<string, string>> skillDatasDic;
    /// <summary>
    /// 换行分割字符串所用数组
    /// </summary>
    string[] split_line = new string[] { "\r\n" };
    /// <summary>
    /// 赋值分割字符串所用数组
    /// </summary>
    string[] split_assignment = new string[] { "^^^" };
    /// <summary>
    /// 数组数据长度与数据分割字符串所用数组
    /// </summary>
    string[] split_arrayLength = new string[] { "???" };
    /// <summary>
    /// 数组元素分割字符串所用数组
    /// </summary>
    string[] split_arrayValue = new string[] { "|||" };
    public SkillAnalysisData()
    {
        skillDatasDic = new Dictionary<string, Dictionary<string, string>>();
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="values">每个对象的值</param>
    public void AnalysisData(string[] values)
    {
        foreach (string value in values)
        {
            string[] lines = value.Split(split_line, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 0)
            {
                string id = lines[0].Trim();//第一行存储的是id
                Dictionary<string, string> fieldValueDic = new Dictionary<string, string>();
                for (int i = 1; i < lines.Length; i++)
                {
                    string fieldValueStr = lines[i];
                    string[] fieldValue = fieldValueStr.Split(split_assignment, StringSplitOptions.RemoveEmptyEntries);
                    if (fieldValue.Length == 2)
                    {
                        string temp = fieldValue[1].Remove(0, 1);
                        temp = temp.Remove(temp.Length - 1, 1);
                        fieldValueDic.Add(fieldValue[0], temp);
                    }
                }
                skillDatasDic.Add(id, fieldValueDic);
            }
        }
    }

    /// <summary>
    /// 压缩数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> Disanalysis()
    {
        Dictionary<string, string> resultDic = new Dictionary<string, string>();
        foreach (KeyValuePair<string, Dictionary<string, string>> idToData in skillDatasDic)
        {
            string id = idToData.Key;
            string value = Disanalysis(id);
            resultDic.Add(id, value);
        }
        return resultDic;
    }

    /// <summary>
    /// 通过id获取压缩有的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Disanalysis(string id)
    {
        if (!skillDatasDic.ContainsKey(id))
            skillDatasDic.Add(id, new Dictionary<string, string>());
        string value = id + split_line[0];
        foreach (KeyValuePair<string, string> fieldToValue in skillDatasDic[id])
        {
            value += fieldToValue.Key + split_assignment[0] + "[" + fieldToValue.Value + "]" + split_line[0];
        }
        return value;
    }

    /// <summary>
    /// 获取所有的id
    /// </summary>
    /// <returns></returns>
    public string[] GetIDArray()
    {
        return skillDatasDic.Keys.ToArray();
    }

    /// <summary>
    /// 移除指定id的数据
    /// </summary>
    /// <param name="id"></param>
    public void RemoveID(string id)
    {
        if (skillDatasDic.ContainsKey(id))
            skillDatasDic.Remove(id);
    }

    /// <summary>
    /// 添加指定id的数据
    /// </summary>
    /// <param name="id"></param>
    public void AddID(string id)
    {
        if (!skillDatasDic.ContainsKey(id))
        {
            skillDatasDic.Add(id, new Dictionary<string, string>());
        }
    }

    /// <summary>
    /// 检查技能id和字段
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="defaultValue">该字段的默认值</param>
    private void Check(string id, string fieldName = null, string defaultValue = "")
    {
        if (!skillDatasDic.ContainsKey(id))
            skillDatasDic.Add(id, new Dictionary<string, string>());
        if (!string.IsNullOrEmpty(fieldName))
        {
            Dictionary<string, string> fieldToValue = skillDatasDic[id];
            if (!fieldToValue.ContainsKey(fieldName))
                fieldToValue.Add(fieldName, defaultValue);
        }
    }

    /// <summary>
    /// 移除指定id的指定字段数据
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    public void RemoveValue(string id, string fieldName)
    {
        if (string.IsNullOrEmpty(id))
            return;
        if (string.IsNullOrEmpty(fieldName))
            return;
        if (skillDatasDic.ContainsKey(id))
        {
            if (skillDatasDic[id].ContainsKey(fieldName))
            {
                skillDatasDic[id].Remove(fieldName);
            }
        }
    }

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <typeparam name="T">枚举的类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public T GetEnum<T>(string id, string fieldName)
    {
        Check(id, fieldName, default(T) == null ? "" : default(T).ToString());
        string value = skillDatasDic[id][fieldName];
        try
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        catch
        {
            return default(T);
        }
    }

    /// <summary>
    /// 设置枚举值
    /// </summary>
    /// <typeparam name="T">枚举的类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    public void SetEnum<T>(string id, string fieldName, T value)
    {
        Check(id, fieldName, default(T) == null ? "" : default(T).ToString());
        skillDatasDic[id][fieldName] = value.ToString();
    }

    /// <summary>
    /// 获取枚举值
    /// </summary>
    /// <param name="type">枚举的类型</param>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public object GetEnum(Type type, string id, string fieldName)
    {
        Check(id, fieldName);
        string value = skillDatasDic[id][fieldName];
        try
        {
            return Enum.Parse(type, value);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 设置枚举值
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    public void SetEnum(string id, string fieldName, object value)
    {
        Check(id, fieldName);
        skillDatasDic[id][fieldName] = value.ToString();
    }

    /// <summary>
    /// 获取值（非数组）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public T GetValue<T>(string id, string fieldName)
    {
        Check(id, fieldName, default(T) == null ? "" : default(T).ToString());
        string value = skillDatasDic[id][fieldName];
        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return default(T);
        }
    }

    /// <summary>
    /// 设置值（非数组）
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    public void SetValue<T>(string id, string fieldName, T value)
    {
        Check(id, fieldName, default(T) == null ? "" : default(T).ToString());
        skillDatasDic[id][fieldName] = value.ToString();
    }

    /// <summary>
    /// 获取值（非数组）
    /// </summary>
    /// <param name="type">值类型</param>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public object GetValue(Type type, string id, string fieldName)
    {
        Check(id, fieldName);
        string value = skillDatasDic[id][fieldName];
        try
        {
            return Convert.ChangeType(value, type);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    public void SetValue(string id, string fieldName, object value)
    {
        Check(id, fieldName);
        skillDatasDic[id][fieldName] = value.ToString();
    }

    /// <summary>
    /// 获取元素数组值
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public T[] GetValues<T>(string id, string fieldName)
    {
        Check(id, fieldName, "0");
        string value = skillDatasDic[id][fieldName];
        try
        {
            string[] arrayDatas = value.Split(split_arrayLength.Concat(split_arrayValue).ToArray(), StringSplitOptions.RemoveEmptyEntries);
            int length = 0;
            if (arrayDatas.Length > 0)
                int.TryParse(arrayDatas[0], out length);
            T[] datas = (T[])Array.CreateInstance(typeof(T), length);
            for (int i = 0; i < length; i++)
            {
                string dataStr = arrayDatas[i + 1];
                dataStr = dataStr.Remove(0, 1);
                dataStr = dataStr.Remove(dataStr.Length - 1, 1);
                if (string.IsNullOrEmpty(dataStr))
                    datas[i] = default(T);
                else
                {
                    T data = (T)Convert.ChangeType(dataStr, typeof(T));
                    datas[i] = data;
                }
            }
            return datas;
        }
        catch
        {
            return (T[])Array.CreateInstance(typeof(T), 0);
        }
    }

    /// <summary>
    /// 设置数组元素值
    /// </summary>
    /// <typeparam name="T">元素的类型</typeparam>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="values">值</param>
    public void SetValues<T>(string id, string fieldName, T[] values)
    {
        Check(id, fieldName, "0");
        if (values == null)
        {
            skillDatasDic[id][fieldName] = "0";
            return;
        }
        string data = values.Length + split_arrayLength[0];
        foreach (T value in values)
        {
            data += "[";
            try
            {
                data += value.ToString();
            }
            catch { }
            data += "]";
            data += split_arrayValue[0];
        }
        skillDatasDic[id][fieldName] = data;
    }

    /// <summary>
    /// 获取元素数组值
    /// </summary>
    /// <param name="type">元素类型</param>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <returns></returns>
    public object[] GetValues(Type type, string id, string fieldName)
    {
        Check(id, fieldName, "0");
        string value = skillDatasDic[id][fieldName];
        try
        {
            string[] arrayDatas = value.Split(split_arrayLength.Concat(split_arrayValue).ToArray(), StringSplitOptions.RemoveEmptyEntries);
            int length = 0;
            if (arrayDatas.Length > 0)
                int.TryParse(arrayDatas[0], out length);
            object[] datas = new object[length];
            for (int i = 0; i < length; i++)
            {
                string dataStr = arrayDatas[i + 1];
                dataStr = dataStr.Remove(0, 1);
                dataStr = dataStr.Remove(dataStr.Length - 1, 1);
                if (string.IsNullOrEmpty(dataStr))
                    datas[i] = null;
                else
                {
                    object data = Convert.ChangeType(dataStr, type);
                    datas[i] = data;
                }
            }
            return datas;
        }
        catch
        {
            return new object[0];
        }
    }

    /// <summary>
    /// 设置数组元素值
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="values">值</param>
    public void SetValues(string id, string fieldName, Array values)
    {
        Check(id, fieldName, "0");
        if (values == null)
        {
            skillDatasDic[id][fieldName] = "0";
            return;
        }
        string data = values.Length + split_arrayLength[0];
        foreach (object value in values)
        {
            data += "[";
            try
            {
                data += value.ToString();
            }
            catch { }
            data += "]";
            data += split_arrayValue[0];
        }
        skillDatasDic[id][fieldName] = data;
    }

    /// <summary>
    /// 清理
    /// </summary>
    public void Clear()
    {
        skillDatasDic.Clear();
    }
}
