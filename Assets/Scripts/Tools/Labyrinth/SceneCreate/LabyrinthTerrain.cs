using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建地形式迷宫
/// </summary>
public class LabyrinthTerrain
{

    #region 水面式迷宫
    /// <summary>
    /// 创建一个水面迷宫
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <param name="size">大小</param>
    /// <param name="maxHeight">最高值</param>
    /// <returns>创建的地形</returns>
    public static GameObject CreateWater(
        LabyrinthOutputStruct labyrinthStruct,
        int pixel,
        float maxHeight)
    {
        Vector2 size = new Vector2(labyrinthStruct.x * pixel, labyrinthStruct.y * pixel);

        //创建
        GameObject terrainObj = new GameObject("WaterTerrain");
        Terrain createTerrain = terrainObj.AddComponent<Terrain>();
        TerrainData createTerrainData = new TerrainData();
        int heightmapResolution = GetHeightMapResolution(labyrinthStruct.x * pixel, labyrinthStruct.y * pixel);
        createTerrainData.heightmapResolution = heightmapResolution;
        //根据迷宫结构构建一个基础高度的地形
        float[,] heightMaps = GetDefaultTerrainHeightMap_Water(
            labyrinthStruct.random,
            labyrinthStruct,
            heightmapResolution,
            pixel);
        createTerrainData.size = new Vector3(size.x, maxHeight, size.y);
        createTerrainData.SetHeights(
            0, 0, heightMaps);
        //材质
        SplatPrototype[] splats;
        float[,,] splatsValues = GetDefaultTerrainSplat_Water(
            labyrinthStruct, pixel, heightMaps, heightmapResolution, out splats);
        createTerrainData.alphamapResolution = heightmapResolution;
        createTerrainData.splatPrototypes = splats;
        createTerrainData.SetAlphamaps(
            0,
            0,
            splatsValues);
        createTerrain.materialType = Terrain.MaterialType.Custom;

        createTerrain.terrainData = createTerrainData;
        terrainObj.AddComponent<TerrainCollider>().terrainData = createTerrainData;
        //创建水面
        GameObject waterPlanObj = CreateWaterPlan(size * 1.2f);
        waterPlanObj.transform.position = new Vector3(size.x * 0.5f, maxHeight * 0.6f, size.y * 0.5f);
        waterPlanObj.transform.SetParent(terrainObj.transform);

        
        return terrainObj;
    }

    /// <summary>
    /// 获取基础的地形材质（水面）
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <param name="heightMap">地图的高度细节数组</param>
    /// <param name="heightmapResolution">地图的高度细节</param>
    /// <param name="splats">返回一个材质细节数组</param>
    /// <returns></returns>
    private static float[,,] GetDefaultTerrainSplat_Water(
        LabyrinthOutputStruct labyrinthStruct,
        int pixel,
        float[,] heightMap,
        int heightmapResolution,
        out SplatPrototype[] splats)
    {
        splats = new SplatPrototype[2];
        SplatPrototype splat0 = new SplatPrototype();
        splat0.texture = Resources.Load<Texture2D>("Terrain/草地");
        splats[0] = splat0;
        SplatPrototype splat1 = new SplatPrototype();
        splat1.texture = Resources.Load<Texture2D>("Terrain/山地");
        splats[1] = splat1;
        //int width = labyrinthStruct.x * pixel;
        //int height = labyrinthStruct.y * pixel;
        int x = heightMap.GetLength(0);
        int y = heightMap.GetLength(1);
        int splatCount = splats.Length;
        float[,,] result = new float[x, y, splatCount];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (heightMap[i, j] < 0.5f)//河底
                {
                    result[i, j, 0] = 0;
                    result[i, j, 1] = 1;
                }
                else
                {
                    float offset = heightMap[i, j] - 0.7f;
                    result[i, j, 0] = 0.5f - offset * 3;
                    result[i, j, 1] = 0.5f + offset * 3;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 获取基础的地形数据
    /// 该地形仅仅包含区域道路等的高低信息（包括噪波）
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="heightmapResolution">地图高度图分辨率</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <returns></returns>
    private static float[,] GetDefaultTerrainHeightMap_Water(
        System.Random random,
        LabyrinthOutputStruct labyrinthStruct,
        int heightmapResolution,
        int pixel)
    {
        float[,] heightMaps = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                heightMaps[i, j] = 0.7f;
            }
        }
        //设置凹下位置
        int xCount = labyrinthStruct.x;
        int yCount = labyrinthStruct.y;
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                if (labyrinthStruct.labyrinthData[i, j] == LabyrinthPiecemealType.Wall)
                {
                    int minX = (heightmapResolution * i) / xCount;
                    int maxX = (heightmapResolution * (i + 1)) / xCount;
                    int minY = (heightmapResolution * j) / yCount;
                    int maxY = (heightmapResolution * (j + 1)) / yCount;
                    for (int u = minX; u < maxX; u++)
                    {
                        for (int v = minY; v < maxY; v++)
                        {
                            heightMaps[v, u] -= 0.5f;
                        }
                    }
                }
            }
        }
        //平滑
        float[,] heightMaps_0 = (float[,])heightMaps.Clone();
        int l_max = pixel-1;
        Func<int, int, float, int> GetMinLength = (_x, _y, t) =>
        {
            int _length = 1;
            ReCheck:
            for (int i = _x - _length; i <= _x + _length; i++)
            {
                if (i >= 0 && i < heightmapResolution)
                    if ((_y - _length >= 0 && heightMaps_0[i, _y - _length] > t)
                    || (_y + _length < heightmapResolution && heightMaps_0[i, _y + _length] > t))
                        goto BreakLine;
            }
            for (int i = _y - _length; i <= _y + _length; i++)
            {
                if (i >= 0 && i < heightmapResolution)
                    if ((_x - _length >= 0 && heightMaps_0[_x - _length, i] > t)
                    || (_x + _length < heightmapResolution && heightMaps_0[_x + _length, i] > t))
                        goto BreakLine;
            }
            _length++;
            if (_length < l_max)
                goto ReCheck;
            BreakLine:
            return _length;
        };
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                if (heightMaps_0[i, j] < 0.5f)
                {
                    int l_min = GetMinLength(i, j, 0.5f);
                    l_min = l_min > l_max ? l_max : l_min;
                    float l_f = NodiesTools.RandomRange(l_min, 0, 1, 0, l_max);
                    float l_f_fade = NodiesTools.Fade(l_f);
                    heightMaps[i, j] = Mathf.Lerp(0.7f, heightMaps_0[i, j], l_f_fade);
                }
            }
        }
        //获取噪波数组
        float[,] nodies = NodiesTools.TwoDPerlinNodies(
            random,
            heightmapResolution,
            heightmapResolution,
            new int[] { pixel, pixel / 2, pixel / 3 },
            new int[] { pixel, pixel / 2, pixel / 3 },
            new float[] { 0.1f, 0.05f, 0.02f });
        //将噪波添加上去
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                if (heightMaps[i, j] == 0.5f)
                {
                    heightMaps[i, j] += nodies[i, j];
                }
            }
        }
        return heightMaps;
    }

    /// <summary>
    /// 创建水平面
    /// </summary>
    /// <param name="size">睡眠的宽度</param>
    /// <returns></returns>
    private static GameObject CreateWaterPlan(Vector2 size)
    {
        //GameObject waterPlanObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //waterPlanObj.transform.localScale = new Vector3(size.x / 10, 1, size.y / 10);
        //waterPlanObj.GetComponent<Renderer>().sharedMaterial = Resources.Load<Material>("Material/SeaWave");
        GameObject waterPlanePrefab = Resources.Load<GameObject>("WaterPlane/AQUAS Waterplane");
        GameObject waterPlanObj = GameObject.Instantiate<GameObject>(waterPlanePrefab);
        return waterPlanObj;
    }

    #endregion

    #region 森林式迷宫
    /// <summary>
    /// 创建一个森林迷宫
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <param name="maxHeight">最高值</param>
    /// <returns></returns>
    public static GameObject CreateForest(
        LabyrinthOutputStruct labyrinthStruct,
        int pixel,
        float maxHeight)
    {
        Vector2 size = new Vector2(labyrinthStruct.x * pixel, labyrinthStruct.y * pixel);
        //创建
        GameObject terrainObj = new GameObject("ForestTerrain");
        Terrain createTerrain = terrainObj.AddComponent<Terrain>();
        TerrainData createTerrainData = new TerrainData();
        int heightmapResolution = GetHeightMapResolution(labyrinthStruct.x * pixel, labyrinthStruct.y * pixel);
        float[,] heightMaps = GetDefaultTerrainHeightMap_Forest(labyrinthStruct.random, labyrinthStruct, maxHeight, heightmapResolution, pixel);
        createTerrainData.heightmapResolution = heightmapResolution;
        createTerrainData.size = new Vector3(size.x, maxHeight, size.y);
        createTerrainData.SetHeights(
           0, 0, heightMaps);
        //材质
        SplatPrototype[] splats;
        float[,,] splatsValues = GetDefaultTerrainSplat_Forest(
            labyrinthStruct, heightMaps, out splats);
        createTerrainData.alphamapResolution = heightmapResolution;
        createTerrainData.splatPrototypes = splats;
        createTerrainData.SetAlphamaps(
            0,
            0,
            splatsValues);
        createTerrain.materialType = Terrain.MaterialType.Custom;
        //树木
        TreePrototype[] trees;
        TreeInstance[] treeInstances = GetDefaultTreeainTree_Forest(
            labyrinthStruct.random, labyrinthStruct, pixel, size, maxHeight, heightMaps, heightmapResolution, out trees);
        createTerrainData.treePrototypes = trees;
        createTerrainData.treeInstances = treeInstances;

        createTerrain.terrainData = createTerrainData;
        terrainObj.AddComponent<TerrainCollider>().terrainData = createTerrainData;
        GameObject windZoneObj = new GameObject();
        windZoneObj.AddComponent<WindZone>();
        windZoneObj.transform.SetParent(terrainObj.transform);
        return terrainObj;
    }

    /// <summary>
    /// 获取基础的地形数据
    /// 该地形仅仅包含基础的噪波信息
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="maxHeight">最大高度</param>
    /// <param name="heightmapResolution">地图细节</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <returns></returns>
    private static float[,] GetDefaultTerrainHeightMap_Forest(
        System.Random random,
        LabyrinthOutputStruct labyrinthStruct,
        float maxHeight,
        int heightmapResolution,
        int pixel)
    {
        float[,] heightMaps = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                heightMaps[i, j] = 0.2f;
            }
        }
        //设置隆起位置
        int xCount = labyrinthStruct.x;
        int yCount = labyrinthStruct.y;
        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < yCount; j++)
            {
                if (labyrinthStruct.labyrinthData[i, j] == LabyrinthPiecemealType.Wall)
                {
                    int minX = (heightmapResolution * i) / xCount;
                    int maxX = (heightmapResolution * (i + 1)) / xCount;
                    int minY = (heightmapResolution * j) / yCount;
                    int maxY = (heightmapResolution * (j + 1)) / yCount;
                    for (int u = minX; u < maxX; u++)
                    {
                        for (int v = minY; v < maxY; v++)
                        {
                            heightMaps[v, u] += 0.7f;
                        }
                    }
                }
            }
        }
        //平滑
        float[,] heightMaps_0 = (float[,])heightMaps.Clone();
        int l_max = (int)(pixel * 2f);
        Func<int, int, float, int> GetMinLength = (_x, _y, t) =>
        {
            int _length = 1;
            ReCheck:
            for (int i = _x - _length; i <= _x + _length; i++)
            {
                if (i >= 0 && i < heightmapResolution)
                    if ((_y - _length >= 0 && heightMaps_0[i, _y - _length] < t)
                    || (_y + _length < heightmapResolution && heightMaps_0[i, _y + _length] < t))
                        goto BreakLine;
            }
            for (int i = _y - _length; i <= _y + _length; i++)
            {
                if (i >= 0 && i < heightmapResolution)
                    if ((_x - _length >= 0 && heightMaps_0[_x - _length, i] < t)
                    || (_x + _length < heightmapResolution && heightMaps_0[_x + _length, i] < t))
                        goto BreakLine;
            }
            _length++;
            if (_length < l_max)
                goto ReCheck;
            BreakLine:
            return _length;
        };
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                if (heightMaps_0[i, j] > 0.5f)
                {
                    int l_min = GetMinLength(i, j, 0.5f) * 2;
                    l_min = l_min > l_max ? l_max : l_min;
                    float l_f = NodiesTools.RandomRange(l_min, 0, 1, 0, l_max);
                    float l_f_fade = NodiesTools.Fade(l_f);
                    heightMaps[i, j] = Mathf.Lerp(0.2f, heightMaps_0[i, j], l_f_fade);
                }
            }
        }
        //获取噪波数组
        float[,] nodies = NodiesTools.TwoDPerlinNodies(
            random,
            heightmapResolution,
            heightmapResolution,
            new int[] { pixel, pixel / 2, pixel / 3 },
            new int[] { pixel, pixel / 2, pixel / 3 },
            new float[] { 0.1f, 0.05f, 0.02f });
        //将噪波添加上去
        float offset = 10 / maxHeight;
        for (int i = 0; i < heightmapResolution; i++)
        {
            for (int j = 0; j < heightmapResolution; j++)
            {
                heightMaps[i, j] += nodies[i, j] * offset;
            }
        }
        return heightMaps;
    }

    /// <summary>
    /// 获取基础的地形材质（森林）
    /// </summary>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="heightMap">地图的高度细节数组</param>
    /// <param name="splats">返回一个材质细节数组</param>
    /// <returns></returns>
    private static float[,,] GetDefaultTerrainSplat_Forest(
        LabyrinthOutputStruct labyrinthStruct,
        float[,] heightMap,
        out SplatPrototype[] splats
        )
    {
        splats = new SplatPrototype[2];
        SplatPrototype splat0 = new SplatPrototype();
        splat0.texture = Resources.Load<Texture2D>("Terrain/草地");
        splats[0] = splat0;
        SplatPrototype splat1 = new SplatPrototype();
        splat1.texture = Resources.Load<Texture2D>("Terrain/山地");
        splats[1] = splat1;
        int x = heightMap.GetLength(0);
        int y = heightMap.GetLength(1);
        int splatCount = splats.Length;
        float[,,] result = new float[x, y, splatCount];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float offset = heightMap[i, j] - 0.5f;
                result[i, j, 0] = 0.5f - offset * 3;
                result[i, j, 1] = 0.5f + offset * 3;
            }
        }
        return result;
    }

    /// <summary>
    /// 获取基础的地形树木（森林）
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="labyrinthStruct">迷宫结构</param>
    /// <param name="pixel">迷宫结构中每一个单位的宽度</param>
    /// <param name="size">地图的大小</param>
    /// <param name="maxHeight">地图最大高度</param>
    /// <param name="heightMaps">高度图</param>
    /// <param name="heightmapResolution">高度图分辨率</param>
    /// <param name="treePrototypes">返回一个树木心结数组</param>
    /// <returns></returns>
    private static TreeInstance[] GetDefaultTreeainTree_Forest(
        System.Random random,
        LabyrinthOutputStruct labyrinthStruct,
        int pixel,
        Vector2 size,
        float maxHeight,
        float[,] heightMaps,
        int heightmapResolution,
        out TreePrototype[] treePrototypes
        )
    {
        treePrototypes = new TreePrototype[1];
        TreePrototype treePrototype0 = new TreePrototype();
        treePrototype0.prefab = Resources.Load<GameObject>("Tree/Tree1");
        treePrototype0.bendFactor = 1;
        treePrototypes[0] = treePrototype0;
        //树木种下的偏差值
        int x = labyrinthStruct.labyrinthData.GetLength(0);
        int y = labyrinthStruct.labyrinthData.GetLength(1);
        List<Vector3> posList = new List<Vector3>();
        List<int> wallCounts = new List<int>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (labyrinthStruct.labyrinthData[i, j] == LabyrinthPiecemealType.Wall)
                {
                    int setTree = random.Next(0, 10);
                    if (setTree > 2)
                        continue;

                    int wallCount = 0;
                    if (i == 0 || labyrinthStruct.labyrinthData[i - 1, j] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (i == x - 1 || labyrinthStruct.labyrinthData[i + 1, j] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (j == 0 || labyrinthStruct.labyrinthData[i, j - 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    if (j == y - 1 || labyrinthStruct.labyrinthData[i, j + 1] == LabyrinthPiecemealType.Wall)
                        wallCount++;
                    wallCounts.Add(wallCount);
                    float setX = NodiesTools.RandomRange(random.Next(0, 100), -0.5f, 0.5f, 0, 100);
                    float setY = NodiesTools.RandomRange(random.Next(0, 100), -0.5f, 0.5f, 0, 100);
                    posList.Add(
                        new Vector3(
                            ((i + 0.5f + setX) * pixel) / size.x,
                            heightMaps[(int)((j + 0.5f + setY) / y * heightmapResolution), (int)((i + 0.5f + setX) / x * heightmapResolution)] - 0.1f,
                            ((j + 0.5f + setY) * pixel) / size.y
                            ));
                }
            }
        }
        int count = posList.Count;
        int treeTypeLength = treePrototypes.Length;
        TreeInstance[] results = new TreeInstance[count];
        for (int i = 0; i < count; i++)
        {
            TreeInstance treeInstance = new TreeInstance();
            treeInstance.position = posList[i];
            float wallCount = wallCounts[i] * 0.1f;
            treeInstance.widthScale = (0.2f + NodiesTools.RandomRange(random.Next(0, 100), wallCount / 4, wallCount + 0.01f, 0, 100)) * pixel / 5f;
            treeInstance.heightScale = (0.2f + NodiesTools.RandomRange(random.Next(0, 100), wallCount / 4, wallCount + 0.01f, 0, 100)) * pixel / 5f;
            treeInstance.rotation = random.Next(0, 360);
            treeInstance.prototypeIndex = random.Next(0, treeTypeLength);
            treeInstance.color = new Color(1, 1, 1, 1);
            treeInstance.lightmapColor = new Color(1, 1, 1, 1);
            results[i] = treeInstance;
        }
        return results;
    }

    #endregion

    /// <summary>
    /// 计算出地图的高度细节
    /// </summary>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns></returns>
    private static int GetHeightMapResolution(int width, int height)
    {
        int max = width;
        max = max > height ? max : height;
        int resolution = 1;
        while (resolution * 2 + 1 < max)
        {
            resolution *= 2;
        }
        return resolution * 2 + 1;
    }

}
