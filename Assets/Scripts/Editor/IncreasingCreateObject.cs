using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 递增创建游戏对象
/// </summary>
public class IncreasingCreateObject : EditorWindow
{
    [MenuItem("小工具/递增创建游戏对象")]
    static void CreateWindow()
    {
        IncreasingCreateObject increasingCreateObject = EditorWindow.CreateInstance<IncreasingCreateObject>();
        increasingCreateObject.Show();
    }

    /// <summary>
    /// 目标游戏对象
    /// </summary>
    private GameObject targetObj;

    /// <summary>
    /// 创建的游戏对象
    /// </summary>
    private GameObject createObj;

    /// <summary>
    /// 位移偏差
    /// </summary>
    private Vector3 positionOffset;

    /// <summary>
    /// 角度偏差
    /// </summary>
    private Vector3 eulerOffset;

    /// <summary>
    /// 创建数量
    /// </summary>
    private int createNum;

    bool isCreate = false;

    private void OnInspectorUpdate()
    {
        if (createObj != null)
        {
            //设置位置
            createObj.transform.position = targetObj.transform.position + positionOffset;
            createObj.transform.eulerAngles = targetObj.transform.eulerAngles + eulerOffset;
        }

        if (isCreate && targetObj != null && createObj != null && createNum > 0)
        {
            isCreate = false;
            Vector3 allPositionOffset = positionOffset + targetObj.transform.position;
            Vector3 allEulerOffset = eulerOffset + targetObj.transform.eulerAngles;
            Transform parent = targetObj.transform.parent;
            for (int i = 0; i < createNum; i++)
            {
                GameObject nextObj = GameObject.Instantiate<GameObject>(targetObj);
                nextObj.transform.position = allPositionOffset;
                nextObj.transform.eulerAngles = allEulerOffset;
                nextObj.transform.localScale = targetObj.transform.localScale;
                nextObj.name = targetObj.name;
                allPositionOffset += positionOffset;
                allEulerOffset += eulerOffset;
                if (parent != null)
                {
                    nextObj.transform.SetParent(parent);
                }
            }
            //初始化
            if (createObj != null)
            {
                GameObject.DestroyImmediate(createObj);
                createObj = null;
                positionOffset = Vector3.zero;
                eulerOffset = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 重绘
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        //第一个游戏对象
        EditorGUILayout.BeginHorizontal();
        GameObject lastObj = targetObj;
        targetObj = EditorGUILayout.ObjectField(targetObj, typeof(GameObject), true) as GameObject;
        if (!GameObject.Equals(lastObj, targetObj))
        {
            if (createObj != null)
            {
                GameObject.DestroyImmediate(createObj);
                createObj = null;
            }
        }
        EditorGUILayout.EndHorizontal();
        if (targetObj != null)
        {
            //两者的偏差
            positionOffset = EditorGUILayout.Vector3Field("Postion:", positionOffset);
            eulerOffset = EditorGUILayout.Vector3Field("Euler:", eulerOffset);

            if (createObj == null)
            {
                //创建
                createObj = GameObject.Instantiate<GameObject>(targetObj);
            }
            else
            {
                //设置数量
                createNum = EditorGUILayout.IntField("Create Count:", createNum);
                //开始创建
                if (GUILayout.Button("创建") && createNum > 0)
                {
                    isCreate = true;
                }
            }
        }
        EditorGUILayout.EndVertical();
    }

    private void OnDestroy()
    {
        if (createObj != null)
        {
            GameObject.DestroyImmediate(createObj);
            createObj = null;
        }
    }

}
