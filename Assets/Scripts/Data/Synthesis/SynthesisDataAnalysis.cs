using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// 合成数据解析
/// </summary>
public class SynthesisDataAnalysis
{
    /// <summary>
    /// 合成数据解析的私有构造函数
    /// </summary>
    public SynthesisDataAnalysis()
    {
        synthesisDataStructList = new List<SynthesisDataStruct>();
    }

    /// <summary>
    /// 合成数据结构集合
    /// </summary>
    List<SynthesisDataStruct> synthesisDataStructList;
    /// <summary>
    /// 读取时的开始标记
    /// </summary>
    const string StartFlag = "[Start]";
    /// <summary>
    /// 读取时的结束标记
    /// </summary>
    const string EndFlag = "[End]";

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="data"></param>
    public void ReadData(string data)
    {
        synthesisDataStructList.Clear();
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        MemoryStream ms = new MemoryStream(dataBytes);
        using (StreamReader sr = new StreamReader(ms))
        {
            string readLine = null;
            string tempData = null;
            SynthesisDataStruct tempSynthesisDataStruct = null;
            while ((readLine = sr.ReadLine()) != null)
            {
                switch (readLine.Trim())
                {
                    case StartFlag:
                        tempData = "";
                        tempSynthesisDataStruct = new SynthesisDataStruct();
                        break;
                    case EndFlag:
                        tempSynthesisDataStruct.SetData(tempData);
                        synthesisDataStructList.Add(tempSynthesisDataStruct);
                        break;
                    default:
                        tempData += readLine + "\r\n";
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public string GetData()
    {
        string result = "";
        foreach (SynthesisDataStruct synthesisDataStruct in synthesisDataStructList)
        {
            result += StartFlag + "\r\n";
            result += synthesisDataStruct.ToString() + "\r\n";
            result += EndFlag + "\r\n";
        }
        return result;
    }

    /// <summary>
    /// 返回指定ID的合成数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SynthesisDataStruct GetDataByID(int id)
    {
        return synthesisDataStructList.Where(temp => temp.id == id).FirstOrDefault();
    }

    /// <summary>
    /// 返回所有ID
    /// </summary>
    public int[] IDArray
    {
        get { return synthesisDataStructList.Select(temp => temp.id).ToArray(); }
    }

    /// <summary>
    /// 获取所有合成数据
    /// </summary>
    /// <returns></returns>
    public SynthesisDataStruct[] GetAllData()
    {
        return synthesisDataStructList.ToArray();
    }

    /// <summary>
    /// 添加一个合成结构对象
    /// </summary>
    /// <param name="synthesisDataStruct"></param>
    public void AddSynthesisDataStruct(SynthesisDataStruct synthesisDataStruct)
    {
        int index = synthesisDataStructList.FindIndex(temp => temp.id == synthesisDataStruct.id);
        if (index < 0)
            synthesisDataStructList.Add(synthesisDataStruct);
        else
            synthesisDataStructList[index] = synthesisDataStruct;
    }

    /// <summary>
    /// 移除一个合成结构对象
    /// </summary>
    /// <param name="synthesisDataStruct"></param>
    public void RemoveSynthesisDataStruct(SynthesisDataStruct synthesisDataStruct)
    {
        synthesisDataStructList.Remove(synthesisDataStruct);
    }
}

/// <summary>
/// 合成数据结构
/// </summary>
public class SynthesisDataStruct
{
    /// <summary>
    /// 字段与数据分割符数组
    /// </summary>
    static string[] dataValueSplit = new string[] { "P==>" };
    /// <summary>
    /// 每行信息分割数组
    /// </summary>
    static string[] dataLineSplit = new string[] { "!@#$%^&*" };

    /// <summary>
    /// 合成数据的id
    /// </summary>
    public int id;
    /// <summary>
    /// 合成名
    /// </summary>
    public string name;
    /// <summary>
    /// 合成时间（单位分钟）
    /// </summary>
    public int time;
    /// <summary>
    /// 可合成等级
    /// </summary>
    public int level;
    /// <summary>
    /// 合成的类别，是炼金还是打造
    /// </summary>
    public EnumSynthesisType synthesisType;
    /// <summary>
    /// 炼金的具体条目，于熟练度挂钩的东西
    /// </summary>
    public EnumSynthesisItem synthesisItem;
    /// <summary>
    /// 输入结构数组
    /// </summary>
    public SynthesisItemStruct[] inputStruct;
    /// <summary>
    /// 输出结构
    /// </summary>
    public SynthesisItemStruct outputStruct;
    /// <summary>
    /// 合成物品结构
    /// 需求（消耗）物品和合成（生成）物品通用的结构
    /// </summary>
    public class SynthesisItemStruct
    {
        /// <summary>
        /// 字段与数据分割符数组
        /// </summary>
        static string[] dataValueSplit = new string[] { "C==>" };
        /// <summary>
        /// 每行信息分割数组
        /// </summary>
        static string[] dataLineSplit = new string[] { "*&^%$#@!" };

        /// <summary>
        /// 材料或成品类型
        /// </summary>
        public EnumItemType itemType;
        /// <summary>
        /// 材料或成品数量
        /// </summary>
        public int num;
        /// <summary>
        /// 最小品质
        /// </summary>
        public EnumQualityType minQuality;
        /// <summary>
        /// 最大品质
        /// </summary>
        public EnumQualityType maxQuality;

        /// <summary>
        /// 将数据转换成字符串 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";
            result += "itemType" + dataValueSplit[0] + itemType + dataLineSplit[0];
            result += "num" + dataValueSplit[0] + num + dataLineSplit[0];
            result += "minQuality" + dataValueSplit[0] + minQuality + dataLineSplit[0];
            result += "maxQuality" + dataValueSplit[0] + maxQuality + dataLineSplit[0];
            return result;
        }

        /// <summary>
        /// 将字符串转换成数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SetData(string value)
        {
            string[] lines = value.Split(dataLineSplit, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string[] datas = line.Split(dataValueSplit, StringSplitOptions.RemoveEmptyEntries);
                if (datas.Length == 2)
                {
                    switch (datas[0])
                    {
                        case "itemType":
                            try { itemType = (EnumItemType)Enum.Parse(typeof(EnumItemType), datas[1].Trim()); } catch { }
                            break;
                        case "num":
                            int.TryParse(datas[1].Trim(), out num);
                            break;
                        case "minQuality":
                            try { minQuality = (EnumQualityType)Enum.Parse(typeof(EnumQualityType), datas[1].Trim()); } catch { }
                            break;
                        case "maxQuality":
                            try { maxQuality = (EnumQualityType)Enum.Parse(typeof(EnumQualityType), datas[1].Trim()); } catch { }
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 将数据转换成字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string result = "";
        result += "id" + dataValueSplit[0] + id + dataLineSplit[0];
        result += "name" + dataValueSplit[0] + name + dataLineSplit[0];
        result += "time" + dataValueSplit[0] + time + dataLineSplit[0];
        result += "level" + dataValueSplit[0] + level + dataLineSplit[0];
        result += "synthesisType" + dataValueSplit[0] + synthesisType + dataLineSplit[0];
        result += "synthesisItem" + dataValueSplit[0] + synthesisItem + dataLineSplit[0];
        result += "inputStruct" + dataValueSplit[0];
        if (inputStruct != null)
        {
            foreach (SynthesisItemStruct synthesisItemStruct in inputStruct)
            {
                result += "{";
                result += synthesisItemStruct.ToString();
                result += "}";
            }
        }
        result += dataLineSplit[0];
        result += "outputStruct" + dataValueSplit[0] + (outputStruct == null ? "" : outputStruct.ToString()) + dataLineSplit[0];
        return result;
    }

    /// <summary>
    /// 将字符串转换成数据
    /// </summary>
    /// <param name="value">数据</param>
    public void SetData(string value)
    {
        string[] lines = value.Split(dataLineSplit, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] datas = line.Split(dataValueSplit, StringSplitOptions.RemoveEmptyEntries);
            if (datas.Length == 2)
            {
                switch (datas[0])
                {
                    case "id":
                        int.TryParse(datas[1].Trim(), out id);
                        break;
                    case "name":
                        name = datas[1].Trim();
                        break;
                    case "time":
                        int.TryParse(datas[1].Trim(), out time);
                        break;
                    case "level":
                        int.TryParse(datas[1].Trim(), out level);
                        break;
                    case "synthesisType":
                        try { synthesisType = (EnumSynthesisType)Enum.Parse(typeof(EnumSynthesisType), datas[1].Trim()); } catch { }
                        break;
                    case "synthesisItem":
                        try { synthesisItem = (EnumSynthesisItem)Enum.Parse(typeof(EnumSynthesisItem), datas[1].Trim()); } catch { }
                        break;
                    case "inputStruct":
                        int[][] arrayValues = GetNextStringSplit(datas[1].Trim());
                        inputStruct = new SynthesisItemStruct[arrayValues.GetLength(0)];
                        for (int i = 0; i < inputStruct.Length; i++)
                        {
                            inputStruct[i] = new SynthesisItemStruct();
                            inputStruct[i].SetData(datas[1].Substring(arrayValues[i][0], arrayValues[i][1]));
                        }
                        break;
                    case "outputStruct":
                        outputStruct = new SynthesisItemStruct();
                        outputStruct.SetData(datas[1].Trim());
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 返回简单的字符串 
    /// </summary>
    /// <returns></returns>
    public string ToStringSimple()
    {
        string result = "";
        result += "ID:" + id + "    ";
        result += "合成名:" + name + "    ";
        result += "合成时间:" + time + "    ";
        result += "可合成等级:" + level + "    ";
        result += "合成材料:种类[" + (inputStruct == null ? 0 : inputStruct.Length) + "] ";
        if (inputStruct != null)
        {
            foreach (SynthesisItemStruct synthesisItemStruct in inputStruct)
            {
                if (synthesisItemStruct != null)
                    result += GetEnumExplan(synthesisItemStruct.itemType) + " ";
                else result += "空 ";
            }
        }
        result += "   ";
        result += "合成物品:" + (outputStruct != null ? GetEnumExplan(outputStruct.itemType) : "");
        return result;
    }

    /// <summary>
    /// 获取指定枚举的值
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string GetEnumExplan(Enum target)
    {
        Type type = target.GetType();
        FieldInfo fieldInfo = type.GetFields().Where(temp => temp.Name.Equals(target.ToString())).FirstOrDefault();
        if (fieldInfo != null)
        {
            FieldExplanAttribute attr = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).Select(temp => temp as FieldExplanAttribute).FirstOrDefault();
            if (attr != null)
                return attr.GetExplan();
        }
        return target.ToString();
    }

    /// <summary>
    /// 通过字符串获取下面节点的字符串分割数据
    /// </summary>
    /// <param name="nextStr">之后节点的字符串</param>
    /// <returns></returns>
    public int[][] GetNextStringSplit(string nextStr)
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