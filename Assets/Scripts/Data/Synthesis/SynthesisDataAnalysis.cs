using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// �ϳ����ݽ���
/// </summary>
public class SynthesisDataAnalysis
{
    /// <summary>
    /// �ϳ����ݽ�����˽�й��캯��
    /// </summary>
    public SynthesisDataAnalysis()
    {
        synthesisDataStructList = new List<SynthesisDataStruct>();
    }

    /// <summary>
    /// �ϳ����ݽṹ����
    /// </summary>
    List<SynthesisDataStruct> synthesisDataStructList;
    /// <summary>
    /// ��ȡʱ�Ŀ�ʼ���
    /// </summary>
    const string StartFlag = "[Start]";
    /// <summary>
    /// ��ȡʱ�Ľ������
    /// </summary>
    const string EndFlag = "[End]";

    /// <summary>
    /// ��ȡ����
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
    /// ��ȡ����
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
    /// ����ָ��ID�ĺϳ�����
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SynthesisDataStruct GetDataByID(int id)
    {
        return synthesisDataStructList.Where(temp => temp.id == id).FirstOrDefault();
    }

    /// <summary>
    /// ��������ID
    /// </summary>
    public int[] IDArray
    {
        get { return synthesisDataStructList.Select(temp => temp.id).ToArray(); }
    }

    /// <summary>
    /// ��ȡ���кϳ�����
    /// </summary>
    /// <returns></returns>
    public SynthesisDataStruct[] GetAllData()
    {
        return synthesisDataStructList.ToArray();
    }

    /// <summary>
    /// ���һ���ϳɽṹ����
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
    /// �Ƴ�һ���ϳɽṹ����
    /// </summary>
    /// <param name="synthesisDataStruct"></param>
    public void RemoveSynthesisDataStruct(SynthesisDataStruct synthesisDataStruct)
    {
        synthesisDataStructList.Remove(synthesisDataStruct);
    }
}

/// <summary>
/// �ϳ����ݽṹ
/// </summary>
public class SynthesisDataStruct
{
    /// <summary>
    /// �ֶ������ݷָ������
    /// </summary>
    static string[] dataValueSplit = new string[] { "P==>" };
    /// <summary>
    /// ÿ����Ϣ�ָ�����
    /// </summary>
    static string[] dataLineSplit = new string[] { "!@#$%^&*" };

    /// <summary>
    /// �ϳ����ݵ�id
    /// </summary>
    public int id;
    /// <summary>
    /// �ϳ���
    /// </summary>
    public string name;
    /// <summary>
    /// �ϳ�ʱ�䣨��λ���ӣ�
    /// </summary>
    public int time;
    /// <summary>
    /// �ɺϳɵȼ�
    /// </summary>
    public int level;
    /// <summary>
    /// �ϳɵ�����������Ǵ���
    /// </summary>
    public EnumSynthesisType synthesisType;
    /// <summary>
    /// ����ľ�����Ŀ���������ȹҹ��Ķ���
    /// </summary>
    public EnumSynthesisItem synthesisItem;
    /// <summary>
    /// ����ṹ����
    /// </summary>
    public SynthesisItemStruct[] inputStruct;
    /// <summary>
    /// ����ṹ
    /// </summary>
    public SynthesisItemStruct outputStruct;
    /// <summary>
    /// �ϳ���Ʒ�ṹ
    /// �������ģ���Ʒ�ͺϳɣ����ɣ���Ʒͨ�õĽṹ
    /// </summary>
    public class SynthesisItemStruct
    {
        /// <summary>
        /// �ֶ������ݷָ������
        /// </summary>
        static string[] dataValueSplit = new string[] { "C==>" };
        /// <summary>
        /// ÿ����Ϣ�ָ�����
        /// </summary>
        static string[] dataLineSplit = new string[] { "*&^%$#@!" };

        /// <summary>
        /// ���ϻ��Ʒ����
        /// </summary>
        public EnumItemType itemType;
        /// <summary>
        /// ���ϻ��Ʒ����
        /// </summary>
        public int num;
        /// <summary>
        /// ��СƷ��
        /// </summary>
        public EnumQualityType minQuality;
        /// <summary>
        /// ���Ʒ��
        /// </summary>
        public EnumQualityType maxQuality;

        /// <summary>
        /// ������ת�����ַ��� 
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
        /// ���ַ���ת��������
        /// </summary>
        /// <param name="value">����</param>
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
    /// ������ת�����ַ���
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
    /// ���ַ���ת��������
    /// </summary>
    /// <param name="value">����</param>
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
    /// ���ؼ򵥵��ַ��� 
    /// </summary>
    /// <returns></returns>
    public string ToStringSimple()
    {
        string result = "";
        result += "ID:" + id + "    ";
        result += "�ϳ���:" + name + "    ";
        result += "�ϳ�ʱ��:" + time + "    ";
        result += "�ɺϳɵȼ�:" + level + "    ";
        result += "�ϳɲ���:����[" + (inputStruct == null ? 0 : inputStruct.Length) + "] ";
        if (inputStruct != null)
        {
            foreach (SynthesisItemStruct synthesisItemStruct in inputStruct)
            {
                if (synthesisItemStruct != null)
                    result += GetEnumExplan(synthesisItemStruct.itemType) + " ";
                else result += "�� ";
            }
        }
        result += "   ";
        result += "�ϳ���Ʒ:" + (outputStruct != null ? GetEnumExplan(outputStruct.itemType) : "");
        return result;
    }

    /// <summary>
    /// ��ȡָ��ö�ٵ�ֵ
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
    /// ͨ���ַ�����ȡ����ڵ���ַ����ָ�����
    /// </summary>
    /// <param name="nextStr">֮��ڵ���ַ���</param>
    /// <returns></returns>
    public int[][] GetNextStringSplit(string nextStr)
    {
        List<int[]> resultList = new List<int[]>();//ÿ������������Ԫ�أ���һ����ʾ�±꣬�ڶ�����ʾλ��
        int index = 0;
        int moveLength = 0;
        int flag = 0;//����{���Ÿñ�����1������}���Ÿñ�����1�������ʱflag����0����һ����ȡ��Χȷ��
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