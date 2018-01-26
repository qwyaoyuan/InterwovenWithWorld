using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UI.Focus;
using System.Linq;
using System;

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
            if (_targetObj.UIFocusArrayRelaships == null)
                _targetObj.UIFocusArrayRelaships = new UIFocusPath.FocusRelaship[0];
            return _targetObj;
        }
    }

    RectTransform parentRect;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("批量导入的UIFocus对象的父对象");
        parentRect = EditorGUILayout.ObjectField(parentRect, typeof(RectTransform), true) as RectTransform;
        if (parentRect != null && GUILayout.Button("批量导入") && EditorUtility.DisplayDialog("提示!", "是否导入?", "确认", "取消"))
        {
            int count = parentRect.childCount;
            var uiFocuses = Enumerable.Range(0, count).Select(temp => parentRect.GetChild(temp).GetComponent<UIFocus>()).Where(temp => temp != null);
            bool add = false;
            List<UIFocusPath.FocusRelaship> tempList = new List<UIFocusPath.FocusRelaship>(TargetObj.UIFocusArrayRelaships);
            foreach (UIFocus uiFocus in uiFocuses)
            {
                if (TargetObj.UIFocusArrayRelaships.Count(temp => temp.This == uiFocus) == 0)
                {
                    UIFocusPath.FocusRelaship temp = new UIFocusPath.FocusRelaship();
                    temp.This = uiFocus;
                    tempList.Add(temp);
                    add = true;
                }
            }
            if (add)
                TargetObj.UIFocusArrayRelaships = tempList.ToArray();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("单独的UIFocus对象");
        UIFocus addTempUIFocus = EditorGUILayout.ObjectField(null, typeof(UIFocus), true) as UIFocus;
        if (addTempUIFocus != null)
        {
            if (TargetObj.UIFocusArrayRelaships.Count(temp => temp.This == addTempUIFocus) == 0)
            {
                List<UIFocusPath.FocusRelaship> tempList = new List<UIFocusPath.FocusRelaship>(TargetObj.UIFocusArrayRelaships);
                UIFocusPath.FocusRelaship temp = new UIFocusPath.FocusRelaship();
                temp.This = addTempUIFocus;
                tempList.Add(temp);
                TargetObj.UIFocusArrayRelaships = tempList.ToArray();
            }
        }
        EditorGUILayout.EndHorizontal();
        ////如果不加这一句,则无法保存
        //if (GUI.changed|| GUILayout.Button("保存"))
        //{
        //    EditorUtility.SetDirty(target);
        //    Undo.RecordObject(target, "UIFocusPathEditorChanged");
        //}
        if (GUILayout.Button("编辑"))
        {
            UIFocusPathEditorWindow uiFocusPathEditorWindow = EditorWindow.GetWindow<UIFocusPathEditorWindow>();
            uiFocusPathEditorWindow.Show();
        }
        EditorGUILayout.LabelField("自身|上|下|左|右");
        List<UIFocusPath.FocusRelaship> mustRemoveList = new List<UIFocusPath.FocusRelaship>();
        if (TargetObj.UIFocusArrayRelaships.Count(temp => temp == null || temp.This == null) > 0)
        {
            TargetObj.UIFocusArrayRelaships = TargetObj.UIFocusArrayRelaships.Where(temp => temp != null && temp.This != null).ToArray();
        }
        foreach (UIFocusPath.FocusRelaship focusRelaship in TargetObj.UIFocusArrayRelaships)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(focusRelaship.This, typeof(UIFocus), true);
            EditorGUILayout.ObjectField(focusRelaship.Up, typeof(UIFocus), true);
            EditorGUILayout.ObjectField(focusRelaship.Down, typeof(UIFocus), true);
            EditorGUILayout.ObjectField(focusRelaship.Left, typeof(UIFocus), true);
            EditorGUILayout.ObjectField(focusRelaship.Right, typeof(UIFocus), true);
            if (GUILayout.Button("X", GUILayout.Width(20)) && EditorUtility.DisplayDialog("提示", "是否移除?", "是", "否"))
            {
                mustRemoveList.Add(focusRelaship);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (mustRemoveList.Count > 0)
        {
            TargetObj.UIFocusArrayRelaships = TargetObj.UIFocusArrayRelaships.Where(temp => !mustRemoveList.Contains(temp)).ToArray();
        }
        /*
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
       */
    }

}
