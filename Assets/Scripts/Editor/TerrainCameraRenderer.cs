using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/// <summary>
/// 渲染地形上的摄像机
/// </summary>
public class TerrainCameraRenderer : EditorWindow
{
    /// <summary>
    /// 用到的摄像机
    /// </summary>
    Camera camera;

    /// <summary>
    /// 摄像机的高度
    /// </summary>
    float cameraHeight;

    /// <summary>
    /// 地形的范围
    /// </summary>
    Rect terrainRect;

    /// <summary>
    /// 渲染的图片
    /// </summary>
    RenderTexture renderText;

    /// <summary>
    /// 倍率
    /// </summary>
    float rate;

    [MenuItem("小工具/场景俯视截图")]
    static void AddWindow()
    {
        TerrainCameraRenderer window = (TerrainCameraRenderer)EditorWindow.GetWindow(typeof(TerrainCameraRenderer), false);
        window.Show();
    }

    private void Awake()
    {
        renderText = new RenderTexture(100, 100, 1, RenderTextureFormat.ARGB32);
        terrainRect = new Rect(0, 0, 100, 100);
        cameraHeight = 100;
        rate = 1;
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        terrainRect = EditorGUILayout.RectField("地形的范围", terrainRect);
        camera = (Camera)EditorGUILayout.ObjectField("渲染用的摄像机", camera, typeof(Camera), true);
        if (camera != null)
        {
            cameraHeight = EditorGUILayout.FloatField("摄像机的高度", cameraHeight);
            rate = EditorGUILayout.FloatField("渲染倍率", rate);
            if (!camera.orthographic)
                camera.orthographic = true;
            camera.transform.forward = Vector3.down;
            camera.transform.position = new Vector3(terrainRect.center.x, cameraHeight, terrainRect.center.y);
            camera.orthographicSize = terrainRect.height / 2;

            if (renderText.width != (int)(terrainRect.width * rate) || renderText.height != (int)(terrainRect.height * rate))
            {
                if ((int)terrainRect.width != 0 && (int)terrainRect.height != 0 && rate != 0) 
                    renderText = new RenderTexture((int)(terrainRect.width * rate), (int)(terrainRect.height * rate), 1, RenderTextureFormat.ARGB32);
            }
            if (camera.targetTexture == null || camera.targetTexture != renderText)
                camera.targetTexture = renderText;
            if (camera.targetTexture != null && terrainRect.height != 0 && terrainRect.width != 0)
            {
                GUI.DrawTexture(new Rect(0.0f, 120f, 500 * terrainRect.width / terrainRect.height, 500), renderText, ScaleMode.ScaleAndCrop, true);
            }
            if (camera.targetTexture != null && camera.targetTexture == renderText && terrainRect.height != 0 && terrainRect.width != 0)
            {
                if (GUILayout.Button("渲染"))
                {
                    string directoryPath = @"E:\MyProject\Unity\地图数据";
                    string fileName = DateTime.Now.ToBinary().ToString();
                    SaveRenderTextureToPNG(renderText, directoryPath, fileName);
                }
            }
        }
        EditorGUILayout.EndVertical();

    }

    public bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;

    }
}
