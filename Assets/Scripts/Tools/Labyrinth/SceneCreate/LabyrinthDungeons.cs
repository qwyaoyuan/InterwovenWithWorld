using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建地下城式迷宫
/// </summary>
public class LabyrinthDungeons
{
    /// <summary>
    /// 创建一个地下城市迷宫
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <returns></returns>
    public static GameObject Create(LabyrinthOutputStruct labyrinthStruct, int pixel)
    {
        GameObject dungeonObj = new GameObject("Dungeons");
        GameObject floorObj = GetFloor(labyrinthStruct, pixel);
        floorObj.transform.SetParent(dungeonObj.transform);
        GameObject wallObj = GetWall(labyrinthStruct, pixel);
        wallObj.transform.SetParent(dungeonObj.transform);
        return dungeonObj;
    }

    /// <summary>
    /// 获取地板游戏对象
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <returns></returns>
    private static GameObject GetFloor(LabyrinthOutputStruct labyrinthStruct, int pixel)
    {
        GameObject floorObj = new GameObject("Floors");
        GameObject floorPrefab = Resources.Load<GameObject>("Dungeon/floor");
        //每pixel米一个地板
        int floorSize = pixel;
        int width = labyrinthStruct.x * pixel / floorSize + ((labyrinthStruct.x * pixel) % floorSize == 0 ? 0 : 1);
        int height = labyrinthStruct.y * pixel / floorSize + ((labyrinthStruct.y * pixel) % floorSize == 0 ? 0 : 1);

        Vector3 offset = new Vector3(
            floorSize / 2f - (width * floorSize - labyrinthStruct.x * pixel) / 2,
            0,
            floorSize / 2f - (height * floorSize - labyrinthStruct.y * pixel) / 2);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float h = labyrinthStruct.labyrinthData[i * floorSize / pixel, j * floorSize / pixel] == LabyrinthPiecemealType.Wall ? 2 : 0;//这一套模型的高是2
                GameObject createObj = GameObject.Instantiate<GameObject>(floorPrefab);
                createObj.name = "floor_" + i + "_" + j;
                createObj.transform.localScale = new Vector3(floorSize, 1, floorSize);
                createObj.transform.position = new Vector3(i * floorSize + offset.x, h, j * floorSize + offset.z);
                createObj.transform.SetParent(floorObj.transform);
            }
        }
        return floorObj;
    }

    /// <summary>
    /// 墙体拐弯处结构（包括连接处）
    /// </summary>
    private struct WallCornerStruct
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 pos;
        /// <summary>
        /// y轴旋转角度
        /// 0度用于左下
        /// -90度用于右下
        /// 90度用于左上
        /// +-180度用于右上
        /// </summary>
        public float angle;
        /// <summary>
        /// 连接处类型
        /// </summary>
        public CornerEnum cornerType;

        public enum CornerEnum
        {
            /// <summary>
            /// 内拐
            /// </summary>
            In,
            /// <summary>
            /// 外拐
            /// </summary>
            Out,
            /// <summary>
            /// 连接
            /// </summary>
            Connect
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType().Equals(typeof(WallCornerStruct)))
            {
                WallCornerStruct target = (WallCornerStruct)obj;
                return pos.Equals(target.pos);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// 获取墙壁游戏对象
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <returns></returns>
    private static GameObject GetWall(LabyrinthOutputStruct labyrinthStruct, int pixel)
    {
        GameObject wallObj = new GameObject("Wall");
        //墙壁预设体
        GameObject[] wallPrefabs = new[]
        {
            Resources.Load<GameObject>("Dungeon/wall1"),
            Resources.Load<GameObject>("Dungeon/wall2")
        };
        //墙壁拐角预设体（0 为内拐，1为外拐）
        GameObject[] wallCornersPrefabs = new[]
        {
            Resources.Load<GameObject>("Dungeon/wall_corner_in"),
            Resources.Load<GameObject>("Dungeon/wall_corner_out")
        };
        //墙壁中间穿插预设体
        GameObject[] wallInsertsPrefabs = new[]
        {
            Resources.Load<GameObject>("Dungeon/wall_in_1"),
            Resources.Load<GameObject>("Dungeon/wall_in_2"),
            Resources.Load<GameObject>("Dungeon/wall_in_3"),
            Resources.Load<GameObject>("Dungeon/wall_in_4")
        };
        //获取一个起点
        Vector2 entry = labyrinthStruct.entrys[0];
        LabyrinthRoadStruct labyrinthRoadStruct = labyrinthStruct.GetRoad(entry);
        List<WallCornerStruct> wallCornerStructs = new List<WallCornerStruct>();
        //创建道路的墙壁
        CreateRoadWall(labyrinthStruct.random, pixel, wallObj, wallPrefabs, wallCornerStructs, null, labyrinthRoadStruct);
        //创建区域的墙壁
        CreateRangeWall(labyrinthStruct.random, pixel, wallObj, wallPrefabs, wallCornerStructs, labyrinthStruct.labyrinthRangeStructs);
        //创建拐角与连接点
        int insertsPrefabsLength = wallInsertsPrefabs.Length;
        int cornerIndex = 0;
        foreach (WallCornerStruct wallCornerStruct in wallCornerStructs)
        {
            GameObject selectPrefab = null;
            switch (wallCornerStruct.cornerType)
            {
                case WallCornerStruct.CornerEnum.In:
                    selectPrefab = wallCornersPrefabs[0];
                    break;
                case WallCornerStruct.CornerEnum.Out:
                    selectPrefab = wallCornersPrefabs[1];
                    break;
                case WallCornerStruct.CornerEnum.Connect:
                    selectPrefab = wallInsertsPrefabs[labyrinthStruct.random.Next(0, insertsPrefabsLength)];
                    break;
            }
            if (selectPrefab != null)
            {
                GameObject createObj = GameObject.Instantiate<GameObject>(selectPrefab);
                createObj.name = "wall_" + wallCornerStruct.cornerType + "_" + cornerIndex++;
                createObj.transform.position = wallCornerStruct.pos;
                createObj.transform.eulerAngles = new Vector3(0, wallCornerStruct.angle, 0);
                createObj.transform.SetParent(wallObj.transform);
            }
        }
        return wallObj;
    }

    /// <summary>
    /// 创建道路墙壁
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="pixel">没单位的宽度</param>
    /// <param name="parent">父物体</param>
    /// <param name="wallPrefabs">墙壁预设体</param>
    /// <param name="wallCornerStructs">拐弯处结果，函数内部填充</param>
    /// <param name="parentRoadStruct">父道路结构</param>
    /// <param name="thisRoadStruct">当前道路结构</param>
    private static void CreateRoadWall(
        System.Random random,
        int pixel,
        GameObject parent,
        GameObject[] wallPrefabs,
        List<WallCornerStruct> wallCornerStructs,
        LabyrinthRoadStruct parentRoadStruct,
        LabyrinthRoadStruct thisRoadStruct)
    {
        //各个方向是否存在墙壁
        bool up = true, down = true, left = true, right = true;
        //使用当前道路结构的位置减去父道路结构的位置，计算通路方向，有通路则改变对应的值为false
        Action<Vector2> ChangeBool = (v) =>
        {
            if (v.x > 0.5f)
            {

                left = false;
                return;
            }
            if (v.x < -0.5f)
            {
                right = false;
                return;
            }
            if (v.y > 0.5f)
            {
                down = false;
                return;
            }
            if (v.y < -0.5f)
            {
                up = false;
                return;
            }
        };
        if (parentRoadStruct != null)
        {
            Vector2 v = thisRoadStruct.value - parentRoadStruct.value;
            ChangeBool(v);
            if (thisRoadStruct.type == LabyrinthPiecemealType.Door)//如果该点是门
            {
                if (!up)
                    down = false;
                if (!down)
                    up = false;
                if (!left)
                    right = false;
                if (!right)
                    left = false;
            }
        }
        foreach (LabyrinthRoadStruct child in thisRoadStruct.nextRoad)
        {
            Vector2 v = thisRoadStruct.value - child.value;
            ChangeBool(v);
        }
        int wallLength = wallPrefabs.Length;
        if (up)//上方存在墙体
        {
            for (int i = 0; i < pixel - 1; i++)
            {
                int r = random.Next(0, wallLength);
                GameObject createObj = GameObject.Instantiate<GameObject>(wallPrefabs[r]);
                createObj.name = "wall_" + thisRoadStruct.value.x + "_" + thisRoadStruct.value.y + "_up_" + i;
                createObj.transform.position = new Vector3(
                     thisRoadStruct.value.x * pixel + i + 1f, 0,
                     thisRoadStruct.value.y * pixel + pixel
                    );
                createObj.transform.eulerAngles = new Vector3(0, 180, 0);
                createObj.transform.SetParent(parent.transform);
            }
            //设置连接墙
            if (!left && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果左侧不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 180,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            if (!right && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果右测不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 180,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            //设置拐角
            if (left)//如果左侧也存在墙体,左上角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 90,
                    cornerType = WallCornerStruct.CornerEnum.In,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }

            }
            if (right)//如果右侧也存在墙体，右上角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 180,
                    cornerType = WallCornerStruct.CornerEnum.In,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
        }
        else//上方不存在墙体
        {
            if (!left)//左侧也不存在墙体，左上角外角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 90,
                    cornerType = WallCornerStruct.CornerEnum.Out,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
            if (!right)//右侧也不存在墙体，右上角外角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 180,
                    cornerType = WallCornerStruct.CornerEnum.Out,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
        }
        if (down)//下方存在墙体
        {
            for (int i = 0; i < pixel - 1; i++)
            {
                int r = random.Next(0, wallLength);
                GameObject createObj = GameObject.Instantiate<GameObject>(wallPrefabs[r]);
                createObj.name = "wall_" + thisRoadStruct.value.x + "_" + thisRoadStruct.value.y + "_down_" + i;
                createObj.transform.position = new Vector3(
                     thisRoadStruct.value.x * pixel + i + 1, 0,
                     thisRoadStruct.value.y * pixel
                    );
                createObj.transform.eulerAngles = new Vector3(0, 0, 0);
                createObj.transform.SetParent(parent.transform);
            }
            //设置连接墙
            if (!left && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果左侧不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 0,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            if (!right && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果右测不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 0,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            //设置拐角
            if (left)//如果左侧也存在墙体，左下角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 0,
                    cornerType = WallCornerStruct.CornerEnum.In,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
            if (right)//如果右侧也存在墙体，右下角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = -90,
                    cornerType = WallCornerStruct.CornerEnum.In,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
        }
        else//下方不存在墙体
        {
            if (!left)//左侧也不存在墙体，左下角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 0,
                    cornerType = WallCornerStruct.CornerEnum.Out,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
            if (!right)//右侧也不存在墙体，右下角内角度
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = -90,
                    cornerType = WallCornerStruct.CornerEnum.Out,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel)
                };
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
        }
        if (left)//左侧存在墙体
        {
            for (int i = 0; i < pixel - 1; i++)
            {
                int r = random.Next(0, wallLength);
                GameObject createObj = GameObject.Instantiate<GameObject>(wallPrefabs[r]);
                createObj.name = "wall_" + thisRoadStruct.value.x + "_" + thisRoadStruct.value.y + "_left_" + i;
                createObj.transform.position = new Vector3(
                    thisRoadStruct.value.x * pixel, 0,
                    thisRoadStruct.value.y * pixel + i + 1
                    );
                createObj.transform.eulerAngles = new Vector3(0, 90, 0);
                createObj.transform.SetParent(parent.transform);
            }
            //设置连接墙
            if (!up && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果上方不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 90,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            if (!down && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果下方不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = 90,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel, 0, thisRoadStruct.value.y * pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
        }
        if (right)//右侧存在墙体
        {
            for (int i = 0; i < pixel - 1; i++)
            {
                int r = random.Next(0, wallLength);
                GameObject createObj = GameObject.Instantiate<GameObject>(wallPrefabs[r]);
                createObj.name = "wall_" + thisRoadStruct.value.x + "_" + thisRoadStruct.value.y + "_right_" + i;
                createObj.transform.position = new Vector3(
                    thisRoadStruct.value.x * pixel + pixel, 0,
                    thisRoadStruct.value.y * pixel + i + 1
                    );
                createObj.transform.eulerAngles = new Vector3(0, -90, 0);
                createObj.transform.SetParent(parent.transform);
            }
            //设置连接墙
            if (!up && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果上方不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = -90,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel + pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
            if (!down && thisRoadStruct.type != LabyrinthPiecemealType.Door)//如果下方不存在墙体，并且该点不是门
            {
                WallCornerStruct wallCornerStruct = new WallCornerStruct()
                {
                    angle = -90,
                    cornerType = WallCornerStruct.CornerEnum.Connect,
                    pos = new Vector3(thisRoadStruct.value.x * pixel + pixel, 0, thisRoadStruct.value.y * pixel)
                };
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
        }
        //继续便利子节点
        foreach (LabyrinthRoadStruct child in thisRoadStruct.nextRoad)
        {
            CreateRoadWall(random, pixel, parent, wallPrefabs, wallCornerStructs,
                thisRoadStruct, child);
        }
    }

    /// <summary>
    /// 创建区域墙壁
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="pixel">没单位的宽度</param>
    /// <param name="parent">父物体</param>
    /// <param name="wallPrefabs">墙壁预设体</param>
    /// <param name="wallCornerStructs">拐弯处结果，函数内部填充</param>
    /// <param name="labyrinthRangeStructs">区域结构数组</param>
    private static void CreateRangeWall(
        System.Random random,
        int pixel,
        GameObject parent,
        GameObject[] wallPrefabs,
        List<WallCornerStruct> wallCornerStructs,
        LabyrinthRangeStruct[] labyrinthRangeStructs)
    {
        int wallPrefabLength = wallPrefabs.Length;
        //创建墙壁函数
        Action<Vector3, int> CreateWallAction = (pos, angle) =>
        {
            int r = random.Next(0, wallPrefabLength);
            GameObject createObj = GameObject.Instantiate<GameObject>(wallPrefabs[r]);
            createObj.transform.position = pos;
            createObj.transform.eulerAngles = new Vector3(0, angle, 0);
            createObj.transform.SetParent(parent.transform);
        };
        //检测是否是门
        Func<int, int, LabyrinthRangeStruct, bool> CheckNearDoor = (x, y, labyrinthRangeStruct) =>
          {
              bool isDoor = false;
              foreach (Vector2 item in labyrinthRangeStruct.doors)
              {
                  int near = Math.Abs((int)item.x - x) + Math.Abs((int)item.y - y);
                  if (near == 0)
                  {
                      isDoor = true;
                      break;
                  }
              }
              return isDoor;
          };
        //添加拐角
        Action<WallCornerStruct> AddCorner = (wallCornerStruct) =>
        {
            if (wallCornerStruct.cornerType != WallCornerStruct.CornerEnum.Connect)
            {
                int index = wallCornerStructs.IndexOf(wallCornerStruct);
                if (index < 0 || wallCornerStructs[index].cornerType == WallCornerStruct.CornerEnum.Connect)
                {
                    if (index >= 0)
                        wallCornerStructs.RemoveAt(index);
                    wallCornerStructs.Add(wallCornerStruct);
                }
            }
            else
            {
                if (!wallCornerStructs.Contains(wallCornerStruct))
                    wallCornerStructs.Add(wallCornerStruct);
            }
        };
        foreach (LabyrinthRangeStruct labyrinthRangeStruct in labyrinthRangeStructs)
        {
            //上 
            for (int i = 0; i < labyrinthRangeStruct.range.width; i++)
            {
                //如果不是门
                if (!CheckNearDoor((int)labyrinthRangeStruct.range.xMin + i, (int)labyrinthRangeStruct.range.yMax, labyrinthRangeStruct))
                {
                    WallCornerStruct wallCornerStruct_Left = new WallCornerStruct()
                    {
                        angle = 180,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3((labyrinthRangeStruct.range.xMin + i) * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
                    };
                    AddCorner(wallCornerStruct_Left);
                    for (int j = 0; j < pixel - 1; j++)
                    {
                        CreateWallAction(
                            new Vector3(
                                (labyrinthRangeStruct.range.xMin + i) * pixel + j + 1, 0,
                                labyrinthRangeStruct.range.yMax * pixel),
                            180);
                    }
                    WallCornerStruct wallCornerStruct_Right = new WallCornerStruct()
                    {
                        angle = 180,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3((labyrinthRangeStruct.range.xMin + i + 1) * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
                    };
                    AddCorner(wallCornerStruct_Right);
                }
                else//如果是门
                {
                    if (i > 0) //左上
                    {
                        WallCornerStruct wallCornerStruct_Left = new WallCornerStruct()
                        {
                            angle = 90,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3((labyrinthRangeStruct.range.xMin + i) * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
                        };
                        AddCorner(wallCornerStruct_Left);
                    }
                    if (i < labyrinthRangeStruct.range.width - 1)//右上
                    {
                        WallCornerStruct wallCornerStruct_Right = new WallCornerStruct()
                        {
                            angle = 180,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3((labyrinthRangeStruct.range.xMin + i + 1) * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
                        };
                        AddCorner(wallCornerStruct_Right);
                    }
                }
            }
            //下 
            for (int i = 0; i < labyrinthRangeStruct.range.width; i++)
            {
                //如果不是门
                if (!CheckNearDoor((int)labyrinthRangeStruct.range.xMin + i, (int)labyrinthRangeStruct.range.yMin - 1, labyrinthRangeStruct))
                {
                    WallCornerStruct wallCornerStruct_Left = new WallCornerStruct()
                    {
                        angle = 0,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3((labyrinthRangeStruct.range.xMin + i) * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
                    };
                    AddCorner(wallCornerStruct_Left);
                    for (int j = 0; j < pixel - 1; j++)
                    {
                        CreateWallAction(
                           new Vector3(
                               (labyrinthRangeStruct.range.xMin + i) * pixel + j + 1, 0,
                               labyrinthRangeStruct.range.yMin * pixel),
                           0);
                    }
                    WallCornerStruct wallCornerStruct_Right = new WallCornerStruct()
                    {
                        angle = 0,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3((labyrinthRangeStruct.range.xMin + i + 1) * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
                    };
                    AddCorner(wallCornerStruct_Right);
                }
                else//如果是门
                {
                    if (i > 0) //左下
                    {
                        WallCornerStruct wallCornerStruct_Left = new WallCornerStruct()
                        {
                            angle = 0,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3((labyrinthRangeStruct.range.xMin + i) * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
                        };
                        AddCorner(wallCornerStruct_Left);
                    }
                    if (i < labyrinthRangeStruct.range.width - 1)//右下
                    {
                        WallCornerStruct wallCornerStruct_Right = new WallCornerStruct()
                        {
                            angle = -90,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3((labyrinthRangeStruct.range.xMin + i + 1) * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
                        };
                        AddCorner(wallCornerStruct_Right);
                    }
                }
            }
            //左
            for (int i = 0; i < labyrinthRangeStruct.range.height; i++)
            {
                //如果不是门
                if (!CheckNearDoor((int)labyrinthRangeStruct.range.xMin - 1, (int)labyrinthRangeStruct.range.yMin + i, labyrinthRangeStruct))
                {
                    WallCornerStruct wallCornerStruct_Down = new WallCornerStruct()
                    {
                        angle = 90,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, (labyrinthRangeStruct.range.yMin + i) * pixel)
                    };
                    AddCorner(wallCornerStruct_Down);
                    for (int j = 0; j < pixel - 1; j++)
                    {
                        CreateWallAction(
                            new Vector3(
                                labyrinthRangeStruct.range.xMin * pixel, 0,
                                (labyrinthRangeStruct.range.yMin + i) * pixel + j + 1),
                            90);
                    }
                    WallCornerStruct wallCornerStruct_Up = new WallCornerStruct()
                    {
                        angle = 90,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, (labyrinthRangeStruct.range.yMin + i + 1) * pixel)
                    };
                    AddCorner(wallCornerStruct_Up);
                }
                else//如果是门
                {
                    if (i > 0)//左下
                    {
                        WallCornerStruct wallCornerStruct_Down = new WallCornerStruct()
                        {
                            angle = 0,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, (labyrinthRangeStruct.range.yMin + i) * pixel)
                        };
                        AddCorner(wallCornerStruct_Down);
                    }
                    if (i < labyrinthRangeStruct.range.height - 1)//左上
                    {
                        WallCornerStruct wallCornerStruct_Up = new WallCornerStruct()
                        {
                            angle = 90,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, (labyrinthRangeStruct.range.yMin + i + 1) * pixel)
                        };
                        AddCorner(wallCornerStruct_Up);
                    }
                }
            }
            //右
            for (int i = 0; i < labyrinthRangeStruct.range.height; i++)
            {
                //如果不是门
                if (!CheckNearDoor((int)labyrinthRangeStruct.range.xMax, (int)labyrinthRangeStruct.range.yMin + i, labyrinthRangeStruct))
                {
                    WallCornerStruct wallCornerStruct_Down = new WallCornerStruct()
                    {
                        angle = -90,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, (labyrinthRangeStruct.range.yMin + i) * pixel)
                    };
                    AddCorner(wallCornerStruct_Down);
                    for (int j = 0; j < pixel - 1; j++)
                    {
                        CreateWallAction(
                            new Vector3(
                                labyrinthRangeStruct.range.xMax * pixel, 0,
                                (labyrinthRangeStruct.range.yMin + i) * pixel + j + 1),
                            -90);
                    }
                    WallCornerStruct wallCornerStruct_Up = new WallCornerStruct()
                    {
                        angle = -90,
                        cornerType = WallCornerStruct.CornerEnum.Connect,
                        pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, (labyrinthRangeStruct.range.yMin + i + 1) * pixel)
                    };
                    AddCorner(wallCornerStruct_Up);
                }
                else//如果是门
                {
                    if (i > 0)//右下
                    {
                        WallCornerStruct wallCornerStruct_Down = new WallCornerStruct()
                        {
                            angle = -90,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, (labyrinthRangeStruct.range.yMin + i) * pixel)
                        };
                        AddCorner(wallCornerStruct_Down);
                    }
                    if (i < labyrinthRangeStruct.range.height - 1)//右上
                    {
                        WallCornerStruct wallCornerStruct_Up = new WallCornerStruct()
                        {
                            angle = 180,
                            cornerType = WallCornerStruct.CornerEnum.Out,
                            pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, (labyrinthRangeStruct.range.yMin + i + 1) * pixel)
                        };
                        AddCorner(wallCornerStruct_Up);
                    }
                }
            }
            //左上角
            bool leftUp_L = CheckNearDoor((int)labyrinthRangeStruct.range.xMin - 1, (int)labyrinthRangeStruct.range.yMax - 1, labyrinthRangeStruct);
            bool leftUp_U = CheckNearDoor((int)labyrinthRangeStruct.range.xMin, (int)labyrinthRangeStruct.range.yMax, labyrinthRangeStruct);
            WallCornerStruct.CornerEnum leftUp_Type = WallCornerStruct.CornerEnum.Connect;
            float leftUp_Angle = 90;
            if (leftUp_L && leftUp_U)
                leftUp_Type = WallCornerStruct.CornerEnum.Out;
            else if (!leftUp_L && !leftUp_U)
                leftUp_Type = WallCornerStruct.CornerEnum.In;
            WallCornerStruct wallCornerStruct_LeftUP = new WallCornerStruct()
            {
                angle = leftUp_Angle,
                cornerType = leftUp_Type,
                pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
            };
            AddCorner(wallCornerStruct_LeftUP);
            //左下角
            bool leftDown_L = CheckNearDoor((int)labyrinthRangeStruct.range.xMin - 1, (int)labyrinthRangeStruct.range.yMin, labyrinthRangeStruct);
            bool leftDown_D = CheckNearDoor((int)labyrinthRangeStruct.range.xMin, (int)labyrinthRangeStruct.range.yMin - 1, labyrinthRangeStruct);
            WallCornerStruct.CornerEnum leftDown_Type = WallCornerStruct.CornerEnum.Connect;
            float leftDown_Angle = 0;
            if (leftDown_L && leftDown_D)
                leftDown_Type = WallCornerStruct.CornerEnum.Out;
            else if (!leftDown_L && !leftDown_D)
                leftDown_Type = WallCornerStruct.CornerEnum.In;
            WallCornerStruct wallCornerStruct_LeftDown = new WallCornerStruct()
            {
                angle = leftDown_Angle,
                cornerType = leftDown_Type,
                pos = new Vector3(labyrinthRangeStruct.range.xMin * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
            };
            AddCorner(wallCornerStruct_LeftDown);
            //右下角
            bool rightDown_R = CheckNearDoor((int)labyrinthRangeStruct.range.xMax, (int)labyrinthRangeStruct.range.yMin, labyrinthRangeStruct);
            bool rightDown_D = CheckNearDoor((int)labyrinthRangeStruct.range.xMax - 1, (int)labyrinthRangeStruct.range.yMin - 1, labyrinthRangeStruct);
            WallCornerStruct.CornerEnum rightDown_Type = WallCornerStruct.CornerEnum.Connect;
            float rightDown_Angle = -90;
            if (rightDown_R && rightDown_D)
                rightDown_Type = WallCornerStruct.CornerEnum.Out;
            else if (!rightDown_R && !rightDown_D)
                rightDown_Type = WallCornerStruct.CornerEnum.In;
            WallCornerStruct wallCornerStruct_RightDown = new WallCornerStruct()
            {
                angle = rightDown_Angle,
                cornerType = rightDown_Type,
                pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, labyrinthRangeStruct.range.yMin * pixel)
            };
            AddCorner(wallCornerStruct_RightDown);
            //右上角
            bool rightUp_R = CheckNearDoor((int)labyrinthRangeStruct.range.xMax, (int)labyrinthRangeStruct.range.yMax -1, labyrinthRangeStruct);
            bool rightUp_U = CheckNearDoor((int)labyrinthRangeStruct.range.xMax - 1, (int)labyrinthRangeStruct.range.yMax, labyrinthRangeStruct);
            WallCornerStruct.CornerEnum rightUp_Type = WallCornerStruct.CornerEnum.Connect;
            float rightUp_Angle = 180;
            if (rightUp_R && rightUp_U)
                rightUp_Type = WallCornerStruct.CornerEnum.Out;
            else if (!rightUp_R && !rightUp_U)
                rightUp_Type = WallCornerStruct.CornerEnum.In;
            WallCornerStruct wallCornerStruct_RightUp = new WallCornerStruct()
            {
                angle = rightUp_Angle,
                cornerType = rightUp_Type,
                pos = new Vector3(labyrinthRangeStruct.range.xMax * pixel, 0, labyrinthRangeStruct.range.yMax * pixel)
            };
            AddCorner(wallCornerStruct_RightUp);
        }
    }
}
