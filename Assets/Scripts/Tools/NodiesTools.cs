using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 噪波工具
/// </summary>
public static class NodiesTools
{

    /// <summary>
    /// 一维的噪音
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="count">总长度</param>
    /// <param name="frequencies">频率</param>
    /// <param name="amplitudes">振幅</param>
    /// <returns></returns>
    public static float[] OneDNodies(System.Random random, int count, int frequencies, int amplitudes, params KeyValuePair<int, float>[] defaultValues)
    {
        List<KeyValuePair<int, float>> defaultValueList = new List<KeyValuePair<int, float>>(defaultValues);
        int unitLenght = count / frequencies;//单位长度
        //构建一个梯度数组
        float[] gradientArray = new float[unitLenght + 2];
        //构建每个基础点的振幅
        float[] amplitudesArray = new float[unitLenght + 2];
        //填充数据
        for (int i = 0; i < unitLenght + 2; i++)
        {
            gradientArray[i] = RandomRange(random.Next(0, 100), -1, 1, 0, 100);//梯度向量
        }
        for (int i = 1; i < unitLenght + 2; i++)
        {
            int index = defaultValueList.FindIndex(temp => temp.Key / frequencies == i);
            if (index > -1)
            {
                amplitudesArray[i] = defaultValueList[index].Value;
                defaultValueList.RemoveAt(index);
            }
            else
                amplitudesArray[i] = RandomRange(random.Next(0, 100), -amplitudes, amplitudes, 0, 100);//单位点的振幅
        }
        //计算非单位点数值
        float frequencies_f = (float)frequencies;
        float[] result = new float[count];
        for (int i = 0; i < count; i++)
        {
            int start = i / frequencies;
            int end = start + 1;
            float u = (i % frequencies) / frequencies_f;
            float u_fade = Fade(u);//在x轴上的权值
            float l = gradientArray[start] * amplitudesArray[start];
            float r = gradientArray[end] * amplitudesArray[end];
            float average = Mathf.Lerp(l, r, u_fade);
            result[i] = average;
        }
        return result;
    }

    /// <summary>
    /// 2D柏林噪音
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="countX">x方向总长度</param>
    /// <param name="countY">y方向总长度</param>
    /// <param name="frequenciesXs">x方向频率数组,每个元素必须是2的n次幂</param>
    /// <param name="frequenciesYs">y方向频率数组,每个元素必须是2的n次幂</param>
    /// <param name="amplitudes">振幅，每个元素的范围为(0-1)</param>
    /// <returns></returns>
    public static float[,] TwoDPerlinNodies(System.Random random, int countX, int countY, int[] frequenciesXs, int[] frequenciesY, float[] amplitudes)
    {
        float[,] baseMapHeights = new float[countX, countY];
        int count = frequenciesXs.Length;
        for (int i = 0; i < count; i++)
        {
            float[,] temps = TwoDNodies(random, countX, countY, frequenciesXs[i], frequenciesY[i], amplitudes[i]);
            for (int j = 0; j < countX; j++)
            {
                for (int k = 0; k < countY; k++)
                {
                    baseMapHeights[j, k] += temps[j, k];
                }
            }
        }
        return baseMapHeights;
    }


    /// <summary>
    /// 计算一组2维的噪音
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="countX">x方向总长度</param>
    /// <param name="countY">y方向总长度</param>
    /// <param name="frequenciesX">x方向频率,必须是2的n次幂</param>
    /// <param name="frequenciesY">y方向频率,必须是2的n次幂</param>
    /// <param name="amplitudes">振幅，范围为(0-1)</param>
    /// <returns></returns>
    public static float[,] TwoDNodies(System.Random random, int countX, int countY, int frequenciesX, int frequenciesY, float amplitudes)
    {
        int unitXLenght = countX / frequenciesX;//x轴的单位
        int unitYLenght = countY / frequenciesY;//y轴的单位
        //构建一个梯度数组
        Vector2[,] gradientArray = new Vector2[unitXLenght + 2, unitYLenght + 2];
        //构建每个基础点的振幅
        float[,] amplitudesArray = new float[unitXLenght + 2, unitYLenght + 2];
        //填充数据
        for (int i = 0; i < unitXLenght + 2; i++)
        {
            for (int j = 0; j < unitYLenght + 2; j++)
            {
                gradientArray[i, j] = (new Vector2(
                    RandomRange(random.Next(0, 100), -1, 1, 0, 100),
                    RandomRange(random.Next(0, 100), -1, 1, 0, 100)
                    )).normalized;//梯度向量

            }
        }
        for (int i = 1; i < unitXLenght; i++)
        {
            for (int j = 1; j < unitYLenght; j++)
            {
                amplitudesArray[i, j] = RandomRange(random.Next(0, 100), -amplitudes, amplitudes, 0, 100);//单位点的振幅
            }
        }

        //计算非单位点数值
        float frequenciesX_f = (float)frequenciesX;
        float frequenciesY_f = (float)frequenciesY;
        float[,] result = new float[countX, countY];
        for (int i = 0; i < countX; i++)
        {
            int startX = i / frequenciesX;
            //int endX = startX + 1;
            float u = (i % frequenciesX) / frequenciesX_f;
            float u_fade = Fade(u);//在x轴上的权值
            for (int j = 0; j < countY; j++)
            {
                int startY = j / frequenciesY;
                //int endY = startY + 1;
                float v = (j % frequenciesY) / frequenciesY_f;
                float v_fade = Fade((j % frequenciesY) / frequenciesY_f);//在y轴上的权值
                                                                         //左下 右下  点积
                float l_d = Vector2.Dot(new Vector2(u, v), gradientArray[startX, startY]) * amplitudesArray[startX, startY];
                float r_d = Vector2.Dot(new Vector2(u - 1, v), gradientArray[startX + 1, startY]) * amplitudesArray[startX + 1, startY];
                //左上 右上  点积
                float l_u = Vector2.Dot(new Vector2(u, v - 1), gradientArray[startX, startY + 1]) * amplitudesArray[startX, startY + 1];
                float r_u = Vector2.Dot(new Vector2(u - 1, v - 1), gradientArray[startX + 1, startY + 1]) * amplitudesArray[startX + 1, startY + 1];
                //差值计算
                float down = Mathf.Lerp(l_d, r_d, u_fade);
                float up = Mathf.Lerp(l_u, r_u, u_fade);
                float average = Mathf.Lerp(down, up, v_fade);
                result[i, j] = average;
            }
        }
        return result;
    }

    /// <summary>
    /// Fade计算加权函数
    /// </summary>
    /// <param name="t">-1到1的值</param>
    /// <returns></returns>
    public static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);//6t^5 - 15t^4 + 10t^3
    }

    /// <summary>
    /// 将随机数限制在指定范围内
    /// </summary>
    /// <param name="target">生成的随机数</param>
    /// <param name="start">开始数据</param>
    /// <param name="end">结束数据</param>
    /// <param name="startRandom">随机数的结束数据</param>
    /// <param name="endRandom">随机数的开始数据</param>
    /// <returns></returns>
    public static float RandomRange(int target, float start, float end, int startRandom, int endRandom)
    {
        return start + (end - start) * ((float)(target - startRandom) / (float)(endRandom - startRandom));
    }
}
