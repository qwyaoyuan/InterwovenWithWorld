using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 迷宫输入结构体
/// </summary>
public class LabyrinthInputStruct
{
    /// <summary>
    /// 随机种子
    /// </summary>
    public int randomSeed;
    /// <summary>
    /// 分块宽度-横向
    /// 具体的分块按照piecemealXCount*2+1计算
    /// </summary>
    public int piecemealXCount;
    /// <summary>
    /// 分块长度-竖向
    /// 具体的分块按照piecemealYCount*2+1计算
    /// </summary>
    public int piecemealYCount;
    /// <summary>
    /// 区域个数的期望值
    /// </summary>
    public int rangeExpectCount;
    /// <summary>
    /// 区域的最大宽度
    /// </summary>
    public int rangeMaxLength;
    /// <summary>
    /// 区域的最小宽度
    /// </summary>
    public int rangeMinLength;
    /// <summary>
    /// 起始点数量的期望值
    /// </summary>
    public int terminalExpectCount;
}

/// <summary>
/// 迷宫输出结构体
/// </summary>
public class LabyrinthOutputStruct
{
    /// <summary>
    /// 迷宫的总宽度
    /// </summary>
    public int x;
    /// <summary>
    /// 迷宫的总长度
    /// </summary>
    public int y;
    /// <summary>
    /// 迷宫每一块的类型数据
    /// </summary>
    public LabyrinthPiecemealType[,] labyrinthData;
    /// <summary>
    /// 所有可以表示入口的位置数组
    /// </summary>
    public Vector2[] entrys;
    /// <summary>
    /// 区域结构体数组
    /// </summary>
    public LabyrinthRangeStruct[] labyrinthRangeStructs;
    /// <summary>
    /// 随机对象
    /// </summary>
    public System.Random random;
    /// <summary>
    /// 通过一个起点，计算从这个点通向所有点的通路
    /// </summary>
    /// <param name="entry">起点位置</param>
    /// <returns></returns>
    public LabyrinthRoadStruct GetRoad(Vector2 entry)
    {
        int x = (int)entry.x;
        int y = (int)entry.y;
        if (x < labyrinthData.GetLength(0) - 1 && y < labyrinthData.GetLength(1) - 1 && x > 0 && y > 0)
        {
            if (labyrinthData[x, y] == LabyrinthPiecemealType.Road)
            {
                //检测这个点是否可以被延伸
                Func<int, int, int, int, bool> CanCheck = (checkX, checkY, parentX, parentY) =>
                {
                    if (checkX == parentX && checkY == parentY)
                        return false;
                    return labyrinthData[checkX, checkY] == LabyrinthPiecemealType.Road ||
                            labyrinthData[checkX, checkY] == LabyrinthPiecemealType.Door;
                };
                //创建一个新的节点
                Func<int, int, LabyrinthRoadStruct, LabyrinthRoadStruct> CreateNew = (checkX, checkY, parent) =>
                {
                    LabyrinthRoadStruct create = new LabyrinthRoadStruct();
                    create.value = new Vector2(checkX, checkY);
                    create.parent = parent;
                    create.type = labyrinthData[checkX, checkY];
                    return create;
                };
                //移动到下一个节点
                Action<LabyrinthRoadStruct> MoveNext = null;
                MoveNext = (parent) =>
                {
                    int _x = (int)parent.value.x;
                    int _y = (int)parent.value.y;
                    int _parentX = _x;
                    int _parentY = _y;
                    if (parent.parent != null)
                    {
                        _parentX = (int)parent.parent.value.x;
                        _parentY = (int)parent.parent.value.y;
                    }
                    List<LabyrinthRoadStruct> nextList = new List<LabyrinthRoadStruct>();
                    if (CanCheck(_x - 1, _y, _parentX, _parentY))
                        nextList.Add(CreateNew(_x - 1, _y, parent));
                    if (CanCheck(_x + 1, _y, _parentX, _parentY))
                        nextList.Add(CreateNew(_x + 1, _y, parent));
                    if (CanCheck(_x, _y - 1, _parentX, _parentY))
                        nextList.Add(CreateNew(_x, _y - 1, parent));
                    if (CanCheck(_x, _y + 1, _parentX, _parentY))
                        nextList.Add(CreateNew(_x, _y + 1, parent));
                    parent.nextRoad = nextList.ToArray();
                    foreach (LabyrinthRoadStruct item in parent.nextRoad)
                    {
                        MoveNext(item);
                    }
                };
                LabyrinthRoadStruct labyrinthRoadStruct = new LabyrinthRoadStruct();
                labyrinthRoadStruct.value = entry;
                MoveNext(labyrinthRoadStruct);
                return labyrinthRoadStruct;
            }
        }
        return null;
    }

}

/// <summary>
/// 区域结构体
/// </summary>
public class LabyrinthRangeStruct
{
    /// <summary>
    /// 区域结构体的包裹范围
    /// </summary>
    public Rect range;
    /// <summary>
    /// 与周围道路连接的门位置
    /// </summary>
    public Vector2[] doors;
}

/// <summary>
/// 道路结构体
/// 每一个对象存储一个位置，并存储向下通路的结构
/// </summary>
public class LabyrinthRoadStruct
{
    /// <summary>
    /// 自身所在位置
    /// </summary>
    public Vector2 value;
    /// <summary>
    /// 该节点是从哪一个节点传递过来的
    /// </summary>
    public LabyrinthRoadStruct parent;
    /// <summary>
    /// 下一块道路的对象
    /// </summary>
    public LabyrinthRoadStruct[] nextRoad;
    /// <summary>
    /// 该块的类型
    /// </summary>
    public LabyrinthPiecemealType type;
}

/// <summary>
/// 迷宫每一块的类型
/// </summary>
public enum LabyrinthPiecemealType
{
    /// <summary>
    /// 墙壁
    /// </summary>
    Wall,
    /// <summary>
    /// 道路
    /// </summary>
    Road,
    /// <summary>
    /// 门
    /// </summary>
    Door,
    /// <summary>
    /// 区域
    /// </summary>
    Range,
}
