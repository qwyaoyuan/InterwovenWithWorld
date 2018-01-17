using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UI.Focus;
using System.Linq;

/// <summary>
/// 扩展焦点
/// </summary>
[CustomEditor(typeof(UIFocusPath))]
public class UIFocusPathEditor : Editor
{

    UIFocusPath _targetObj;

    UIFocusPath TargetObj
    {
        get
        {
            if (_targetObj == null) _targetObj = target as UIFocusPath;
            if (_targetObj.UIFocuesArray == null)
                _targetObj.UIFocuesArray = new UIFocus[0];
            return _targetObj;
        }
    }

    public override void OnInspectorGUI()
    {


        UIFocus[,] lastValue = new UIFocus[TargetObj.row, TargetObj.column];
        for (int i = 0; i < TargetObj.row; i++)
        {
            for (int j = 0; j < TargetObj.column; j++)
            {
                try
                {
                    lastValue[i, j] = TargetObj.UIFocuesArray[i * TargetObj.column + j];
                }
                catch { }
            }
        }
        TargetObj.row = EditorGUILayout.IntField("行", TargetObj.row);
        TargetObj.column = EditorGUILayout.IntField("列", TargetObj.column);
        //重设数组
        if (TargetObj.row * TargetObj.column != TargetObj.UIFocuesArray.Length)
        {
            UIFocus[] uiFocusArray = new UIFocus[TargetObj.row * TargetObj.column];
            for (int i = 0; i < TargetObj.row; i++)
            {
                if (lastValue.GetLength(0) > i)
                    for (int j = 0; j < TargetObj.column; j++)
                    {
                        if (lastValue.GetLength(1) > j)
                        {
                            uiFocusArray[i * TargetObj.column + j] = lastValue[i, j];
                        }
                    }
            }
            TargetObj.UIFocuesArray = uiFocusArray;
        }
        //绘制
        EditorGUILayout.LabelField("网格");
        for (int i = 0; i < TargetObj.row; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < TargetObj.column; j++)
            {
                TargetObj.UIFocuesArray[i * TargetObj.column + j] = EditorGUILayout.ObjectField(TargetObj.UIFocuesArray[i * TargetObj.column + j], typeof(UIFocus), true) as UIFocus;
            }
            EditorGUILayout.EndHorizontal();
        }

        //测试 在窗体中绘制连线
        if (TargetObj.UIFocuesArray != null && TargetObj.UIFocuesArray.Length >= 2)
        {
            
        }
       
    }

}
