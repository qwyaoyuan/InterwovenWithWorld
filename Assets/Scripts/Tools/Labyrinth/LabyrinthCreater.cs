using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 迷宫生成器
/// </summary>
public class LabyrinthCreater
{
    /// <summary>
    /// 创建迷宫
    /// </summary>
    /// <param name="input">输入对象</param>
    /// <returns></returns>
    public static LabyrinthOutputStruct Create(LabyrinthInputStruct input)
    {
        LabyrinthOutputStruct output = new LabyrinthOutputStruct();
        output.random = new System.Random(input.randomSeed); ;
        output.x = input.piecemealXCount * 2 + 1;
        output.y = input.piecemealYCount * 2 + 1;
        output.labyrinthData = GetBaseLabyrinthData(output.x, output.y);
        output.labyrinthRangeStructs = SetRange(output.random, output.labyrinthData, input.rangeExpectCount, input.rangeMinLength, input.rangeMaxLength);
        SetPiecemeal(output.random, output.labyrinthData, new Vector2(1, 1), input.terminalExpectCount);
        CreateDoor(output.random, output.labyrinthData, output.labyrinthRangeStructs);
        output.entrys = OptmizeEntrys(output.random, output.labyrinthData, input.terminalExpectCount);
        return output;
    }

    /// <summary>
    /// 获取基础的迷宫数据
    /// </summary>
    /// <param name="x">迷宫的x</param>
    /// <param name="y">迷宫的y</param>
    /// <returns></returns>
    private static LabyrinthPiecemealType[,] GetBaseLabyrinthData(int x, int y)
    {
        LabyrinthPiecemealType[,] data = new LabyrinthPiecemealType[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i == 0 || i == x - 1 || j == 0 || j == y - 1 || i % 2 == 0 || j % 2 == 0)
                    data[i, j] = LabyrinthPiecemealType.Wall;
                else data[i, j] = LabyrinthPiecemealType.Road;
            }
        }
        return data;
    }

    /// <summary>
    /// 获取并设置迷宫的区域
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="data">迷宫当前数据</param>
    /// <param name="rangeExpectCount">区域期望总数</param>
    /// <param name="rangeMinLength">区域的最小宽度</param>
    /// <param name="rangeMaxLength">区域的最大宽度</param>
    /// <returns></returns>
    private static LabyrinthRangeStruct[] SetRange(System.Random random, LabyrinthPiecemealType[,] data, int rangeExpectCount, int rangeMinLength, int rangeMaxLength)
    {
        int tempX = (data.GetLength(0) - 1) / 2;
        int tempY = (data.GetLength(1) - 1) / 2;
        int[,] tempXY = new int[tempX, tempY];
        List<Rect> rects = new List<Rect>();
        //检测这一块区域是否可用，如果可用则将该区域的值改为1
        Func<int, int, int, int, bool> CheckCanSet = (x, y, w, h) =>
        {
            if (x + w + 1 >= tempX || y + h + 1 >= tempY || x - 1 < 0 || y - 1 < 0)
                return false;
            for (int i = x - 1; i <= x + w + 1; i++)
                for (int j = y - 1; j <= y + h + 1; j++)
                    if (tempXY[i, j] == 1)
                        return false;
            for (int i = x; i <= x + w; i++)
                for (int j = y; j <= y + h; j++)
                    tempXY[i, j] = 1;
            return true;
        };
        //0,0点不可被占用
        int startX = random.Next(1, 4); int startY = random.Next(0, 3);
        NextY://标记y轴移动
        while (startX < tempX)
        {
            int width = random.Next(rangeMinLength, rangeMaxLength + 1);
            int height = random.Next(rangeMinLength, rangeMaxLength + 1);
            int randomY = random.Next(-rangeMinLength, rangeMinLength);
            int testNum = 0;//尝试次数
            Test://标记尝试次数
            //如果通过则设置该区域
            if (CheckCanSet(startX, startY + randomY, width, height))
            {
                rects.Add(new Rect(startX, startY + randomY, width, height));
                startX -= testNum;
                startX += random.Next(width + 1, width += 3);
            }
            //如果不通过则x轴向右偏移,如果已经尝试了五次依然失败则不在继续
            else if (testNum < 5)
            {
                testNum++;
                startX++;
                goto Test;
            }
            else
            {
                startX += random.Next(1, 4);
            }
        }
        startY += random.Next(rangeMaxLength / 2 + 1, rangeMaxLength / 2 + 4);
        if (startY < tempY)
        {
            startX = random.Next(0, 3);
            goto NextY;
        }
        List<LabyrinthRangeStruct> labyrinthRangeStructs = new List<LabyrinthRangeStruct>();
        foreach (Rect rect in rects)
        {
            LabyrinthRangeStruct labyrinthRangeStruct = new LabyrinthRangeStruct();
            labyrinthRangeStruct.range = new Rect(rect.x * 2 + 1, rect.y * 2 + 1, rect.width * 2 + 1, rect.height * 2 + 1);
            labyrinthRangeStructs.Add(labyrinthRangeStruct);
        }
        while (labyrinthRangeStructs.Count > rangeExpectCount)
        {
            labyrinthRangeStructs.RemoveAt(random.Next(0, labyrinthRangeStructs.Count));
        }
        foreach (LabyrinthRangeStruct labyrinthRangeStruct in labyrinthRangeStructs)
        {
            int x = (int)labyrinthRangeStruct.range.x;
            int y = (int)labyrinthRangeStruct.range.y;
            int lengthX = (int)(x + labyrinthRangeStruct.range.width);
            int lengthY = (int)(y + labyrinthRangeStruct.range.height);
            for (int i = x; i < lengthX; i++)
            {
                for (int j = y; j < lengthY; j++)
                {
                    data[i, j] = LabyrinthPiecemealType.Range;
                }
            }
        }
        return labyrinthRangeStructs.ToArray();
    }

    /// <summary>
    /// 设置迷宫的路径
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="data">迷宫当前数据</param>
    /// <param name="start">起始点</param>
    /// <param name="terminalCount">所有终点数量</param>
    /// <returns></returns>
    private static void SetPiecemeal(System.Random random, LabyrinthPiecemealType[,] data, Vector2 start, int terminalCount)
    {
        int x = data.GetLength(0);
        int y = data.GetLength(1);
        //构造一个用于计算到道路的结构
        CreateRoadType[,] tempDatas = new CreateRoadType[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                CreateRoadType tempType = CreateRoadType.Wall_Normal;
                switch (data[i, j])
                {
                    case LabyrinthPiecemealType.Road:
                        tempType = CreateRoadType.Road_Normal;
                        break;
                    case LabyrinthPiecemealType.Range:
                        tempType = CreateRoadType.Range;
                        break;
                }
                tempDatas[i, j] = tempType;
            }
        }
        //寻路栈
        Stack<Vector2> findWayStack = new Stack<Vector2>();
        findWayStack.Push(start);
        tempDatas[(int)start.x, (int)start.y] = CreateRoadType.Road_Select;
        //临时范围集合
        List<Vector2> tempFindList = new List<Vector2>();
        while (findWayStack.Count > 0)
        {
            //取出栈顶元素，使用这个元素向四周寻路
            Vector2 findStart = findWayStack.Pop();
            tempFindList.Clear();
            int _x = (int)findStart.x;
            int _y = (int)findStart.y;
            if (_x > 1 && tempDatas[_x - 2, _y] == CreateRoadType.Road_Normal)
                tempFindList.Add(new Vector2(_x - 2, _y));
            if (_x < x - 3 && tempDatas[_x + 2, _y] == CreateRoadType.Road_Normal)
                tempFindList.Add(new Vector2(_x + 2, _y));
            if (_y > 1 && tempDatas[_x, _y - 2] == CreateRoadType.Road_Normal)
                tempFindList.Add(new Vector2(_x, _y - 2));
            if (_y < y - 3 && tempDatas[_x, _y + 2] == CreateRoadType.Road_Normal)
                tempFindList.Add(new Vector2(_x, _y + 2));
            if (tempFindList.Count > 0)
            {
                int index = random.Next(0, tempFindList.Count);
                Vector2 selectVec = tempFindList[index];
                int s_x = (int)selectVec.x;
                int s_y = (int)selectVec.y;
                tempDatas[s_x, s_y] = CreateRoadType.Road_Select;
                tempDatas[(s_x + _x) / 2, (s_y + _y) / 2] = CreateRoadType.Wall_Select;
                findWayStack.Push(findStart);
                findWayStack.Push(selectVec);
            }
        }
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                switch (tempDatas[i, j])
                {
                    case CreateRoadType.Road_Select:
                    case CreateRoadType.Wall_Select:
                        data[i, j] = LabyrinthPiecemealType.Road;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 设置门
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="data">迷宫当前数据</param>
    /// <param name="labyrinthRangeStructs">迷宫区域数据</param>
    private static void CreateDoor(System.Random random, LabyrinthPiecemealType[,] data, LabyrinthRangeStruct[] labyrinthRangeStructs)
    {
        foreach (LabyrinthRangeStruct labyrinthRangeStruct in labyrinthRangeStructs)
        {
            List<Vector2> doors = new List<Vector2>();
            int count = 4;
            Func<bool> CanSetDoor = () =>
            {
                if (random.Next(0, count) == 0)
                    return true;
                else
                {
                    count--;
                    return false;
                }
            };
            int testCount = 0;
            //上
            if (CanSetDoor())
            {
                testCount = 0;
                int y = (int)labyrinthRangeStruct.range.yMax - 1;
                Test:
                int x = random.Next((int)labyrinthRangeStruct.range.xMin + 1, (int)labyrinthRangeStruct.range.xMax);
                if (data[x, y + 2] == LabyrinthPiecemealType.Road)
                {
                    data[x, y + 1] = LabyrinthPiecemealType.Door;
                    doors.Add(new Vector2(x, y + 1));
                }
                else
                {
                    testCount++;
                    if (testCount <= 10)
                        goto Test;
                    else
                        count--;
                }
            }
            //下
            if (CanSetDoor())
            {
                testCount = 0;
                int y = (int)labyrinthRangeStruct.range.yMin;
                Test:
                int x = random.Next((int)labyrinthRangeStruct.range.xMin + 1, (int)labyrinthRangeStruct.range.xMax);
                if (data[x, y - 2] == LabyrinthPiecemealType.Road)
                {
                    data[x, y - 1] = LabyrinthPiecemealType.Door;
                    doors.Add(new Vector2(x, y - 1));
                }
                else
                {
                    testCount++;
                    if (testCount <= 10)
                        goto Test;
                    else
                        count--;
                }
            }
            //左
            if (CanSetDoor())
            {
                testCount = 0;
                int x = (int)labyrinthRangeStruct.range.xMin;
                Test:
                int y = random.Next((int)labyrinthRangeStruct.range.yMin + 1, (int)labyrinthRangeStruct.range.yMax);
                if (data[x - 2, y] == LabyrinthPiecemealType.Road)
                {
                    data[x - 1, y] = LabyrinthPiecemealType.Door;
                    doors.Add(new Vector2(x - 1, y));
                }
                else
                {
                    testCount++;
                    if (testCount <= 10)
                        goto Test;
                    else
                        count--;
                }
            }
            //右
            if (CanSetDoor())
            {
                testCount = 0;
                int x = (int)labyrinthRangeStruct.range.xMax - 1;
                Test:
                int y = random.Next((int)labyrinthRangeStruct.range.yMin + 1, (int)labyrinthRangeStruct.range.yMax);
                if (data[x + 2, y] == LabyrinthPiecemealType.Road)
                {
                    data[x + 1, y] = LabyrinthPiecemealType.Door;
                    doors.Add(new Vector2(x + 1, y));
                }
                else
                {
                    testCount++;
                    if (testCount <= 30)
                        goto Test;
                    else
                        count--;
                }
            }
            labyrinthRangeStruct.doors = doors.ToArray();
        }
    }

    /// <summary>
    /// 优化并获取入口
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="data">迷宫当前数据</param>
    /// <param name="terminalCount">入口数量期望</param>
    /// <returns></returns>
    private static Vector2[] OptmizeEntrys(System.Random random, LabyrinthPiecemealType[,] data, int terminalCount)
    {
        int x = data.GetLength(0);
        int y = data.GetLength(1);
        List<Vector2> terminalList = new List<Vector2>();
        for (int i = 1; i < x - 1; i++)
        {
            for (int j = 1; j < y - 1; j++)
            {
                if (data[i, j] == LabyrinthPiecemealType.Road)
                {
                    int wallCount = 0;
                    if (data[i - 1, j] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[i + 1, j] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[i, j - 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[i, j + 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (wallCount == 3)
                    {
                        terminalList.Add(new Vector2(i, j));
                    }
                }
            }
        }
        while (terminalList.Count > terminalCount)
        {
            int index = random.Next(1, terminalCount);
            int _x = (int)terminalList[index].x;
            int _y = (int)terminalList[index].y;
            terminalList.RemoveAt(index);
            Reset://设置该点为墙
            data[_x, _y] = LabyrinthPiecemealType.Wall;
            Func<int, int, bool> Check = (tempX, tempY) =>
            {
                if (data[tempX, tempY] == LabyrinthPiecemealType.Road)
                {
                    int wallCount = 0;
                    if (data[tempX - 1, tempY] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[tempX + 1, tempY] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[tempX, tempY - 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (data[tempX, tempY + 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (wallCount == 3)
                    {
                        _x = tempX;
                        _y = tempY;
                        return true;
                    }
                }
                return false;
            };
            if (Check(_x - 1, _y) || Check(_x + 1, _y) || Check(_x, _y - 1) || Check(_x, _y + 1))
            {
                goto Reset;
            }
        }
        return terminalList.ToArray();
    }

    /// <summary>
    /// 创建路线时的类型
    /// </summary>
    private enum CreateRoadType
    {
        /// <summary>
        /// 未选择的道路
        /// </summary>
        Road_Normal,
        /// <summary>
        /// 已经被选择的道路
        /// </summary>
        Road_Select,
        /// <summary>
        /// 普通的墙壁
        /// </summary>
        Wall_Normal,
        /// <summary>
        /// 被选择的墙壁
        /// </summary>
        Wall_Select,
        /// <summary>
        /// 区域
        /// </summary>
        Range
    }
}
