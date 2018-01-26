using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class UIFocusPathEditorWindow : EditorWindow
{
    [MenuItem("小工具/配置UIFocus的路径")]
    static void AddWindow()
    {
        UIFocusPathEditorWindow uiFocusPathEditorWindow = EditorWindow.GetWindow<UIFocusPathEditorWindow>();
        uiFocusPathEditorWindow.Show();
    }


    UIFocusPath uiFocusPath;

    /// <summary>
    /// 按钮选中样式
    /// </summary>
    GUIStyle buttonSelectStyle;
    /// <summary>
    /// 按钮没有选中样式
    /// </summary>
    GUIStyle buttonNotSelectStyle;

    private void Awake()
    {
        relashipButtonTexture = Resources.Load<Texture2D>("CustomUI/Focus/RelashipButton");
        //按钮样式
        buttonSelectStyle = new GUIStyle();
        buttonSelectStyle.fontSize = 14;  //字体大小
        buttonSelectStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
        buttonSelectStyle.normal.background = Resources.Load<Texture2D>("Task/Blue");//背景.
        buttonSelectStyle.normal.textColor = Color.yellow;//文字颜色。

        buttonNotSelectStyle = new GUIStyle();
        buttonNotSelectStyle.fontSize = 14;  //字体大小
        buttonNotSelectStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
        buttonNotSelectStyle.normal.background = Resources.Load<Texture2D>("Task/Black");//背景.
        buttonNotSelectStyle.normal.textColor = Color.yellow;//文字颜色。
    }

    private void OnInspectorUpdate()
    {

        GameObject selectObj = Selection.activeGameObject;
        if (selectObj != null)
        {
            UIFocusPath uiFocusPath = selectObj.GetComponent<UIFocusPath>();
            if (uiFocusPath != null)
                this.uiFocusPath = uiFocusPath;
        }
        this.Repaint();
    }

    /// <summary>
    /// 焦点对应位置字典 
    /// </summary>
    Dictionary<UIFocus, Vector3> uiFocusToLocaionDic;

    /// <summary>
    /// 缩放比例
    /// </summary>
    float scale = 1;

    /// <summary>
    /// 显示全部,否则只显示选中的对象
    /// </summary>
    bool showAll = false;

    /// <summary>
    /// 选中的对象
    /// </summary>
    UIFocus selectUIFcous;

    /// <summary>
    /// 滑杆
    /// </summary>
    Vector2 scroll;

    /// <summary>
    /// 点击按钮后切换选择对下给你
    /// </summary>
    bool clickButtonToSelect;

    /// <summary>
    /// 选中的朝向
    /// </summary>
    EnumEditorGUIFocusPathForward? forwardType;

    /// <summary>
    /// 关系小按钮的图片
    /// </summary>
    Texture2D relashipButtonTexture;

    private void OnGUI()
    {
        if (uiFocusPath == null)
            return;
        //提取所有焦点
        uiFocusToLocaionDic = uiFocusPath.NewUIFocusArray.Where(temp => temp != null).ToDictionary(temp => temp, temp => temp.transform.position);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("缩放比例:", GUILayout.Width(50));
        scale = GUILayout.HorizontalSlider(scale, 0.2f, 5f, GUILayout.Width(100));
        EditorGUILayout.LabelField("显示全部", GUILayout.Width(50));
        showAll = EditorGUILayout.Toggle(showAll, GUILayout.Width(15));
        EditorGUILayout.LabelField("点击选择对象", GUILayout.Width(75));
        clickButtonToSelect = EditorGUILayout.Toggle(clickButtonToSelect, GUILayout.Width(15));
        EditorGUILayout.EndHorizontal();
        if (uiFocusToLocaionDic.Count <= 0)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        //计算出一个范围 
        float minX = uiFocusToLocaionDic.Min(temp => temp.Value.x) - 30;
        float minY = uiFocusToLocaionDic.Min(temp => temp.Value.y) - 30;
        float maxX = uiFocusToLocaionDic.Max(temp => temp.Value.x) + 30;
        float maxY = uiFocusToLocaionDic.Max(temp => temp.Value.y) + 30;
        Vector2 baseOffset = new Vector2(minX, minY);//基础的偏差值
        Vector2 baseSize = new Vector2(maxX - minX, maxY - minY);//基础的大小
        Rect baseRect = new Rect(baseOffset, baseSize);//基础的位置
        Dictionary<UIFocus, Rect> uiFocusToShowRectDic = new Dictionary<UIFocus, Rect>();//本次运行时的显示区域
        scroll = GUI.BeginScrollView(new Rect(0, 40, position.width, position.height - 40), scroll, new Rect(0, 0, baseRect.width * scale, baseRect.height * scale));
        foreach (KeyValuePair<UIFocus, Vector3> item in uiFocusToLocaionDic)//遍历绘制
        {
            Vector2 nowPos = item.Value;
            nowPos = nowPos - baseOffset;
            nowPos.y = baseSize.y - nowPos.y;
            nowPos *= scale;
            Rect tempRect = new Rect(nowPos.x, nowPos.y, 70, 15);
            GUIStyle buttonStyle = buttonNotSelectStyle;
            if (selectUIFcous == item.Key)
                buttonStyle = buttonSelectStyle;
            if (GUI.Button(tempRect, item.Key.name, buttonStyle))
            {
                selectUIFcous = item.Key;
                if (clickButtonToSelect)
                    Selection.activeGameObject = selectUIFcous.gameObject;
            }
            uiFocusToShowRectDic.Add(item.Key, new Rect(nowPos.x - scroll.x, nowPos.y + 40 - scroll.y, 70, 15));
        }
        GUI.EndScrollView();
        EditorGUILayout.EndVertical();
        Dictionary<Vector3[], UIFocus[]> lineToFocusDic = new Dictionary<Vector3[], UIFocus[]>();//线对应焦点字典
        //绘制线
        Action<Vector3, Vector3, UIFocus, UIFocus> DrawLineAction = (startVec, endVec, startFocus, endFocus) =>
          {
              Handles.color = Color.red;
              Handles.DrawLine(startVec, endVec);
              lineToFocusDic.Add(new Vector3[] { startVec, endVec }, new UIFocus[] { startFocus, endFocus });
          };
        foreach (UIFocusPath.FocusRelaship item in uiFocusPath.UIFocusArrayRelaships)
        {
            if (showAll || item.This == selectUIFcous)
            {
                if (!uiFocusToShowRectDic.ContainsKey(item.This))
                    continue;
                Rect thisRect = uiFocusToShowRectDic[item.This];
                if (item.Up)
                {
                    Vector3 thisVec = new Vector3(thisRect.xMin + thisRect.width / 2, thisRect.yMin);
                    if (uiFocusToShowRectDic.ContainsKey(item.Up))
                    {
                        Rect upRect = uiFocusToShowRectDic[item.Up];
                        Vector3 target = upRect.center;
                        DrawLineAction(thisVec, target, item.This, item.Up);
                    }
                }
                if (item.Down)
                {
                    Vector3 thisVec = new Vector3(thisRect.xMin + thisRect.width / 2, thisRect.yMax);
                    if (uiFocusToShowRectDic.ContainsKey(item.Down))
                    {
                        Rect downRect = uiFocusToShowRectDic[item.Down];
                        Vector3 target = downRect.center;
                        DrawLineAction(thisVec, target, item.This, item.Down);
                    }
                }
                if (item.Left)
                {
                    Vector3 thisVec = new Vector3(thisRect.xMin, thisRect.yMin + thisRect.height / 2);
                    if (uiFocusToShowRectDic.ContainsKey(item.Left))
                    {
                        Rect leftRect = uiFocusToShowRectDic[item.Left];
                        Vector3 target = leftRect.center;
                        DrawLineAction(thisVec, target, item.This, item.Left);
                    }
                }
                if (item.Right)
                {
                    Vector3 thisVec = new Vector3(thisRect.xMax, thisRect.yMin + thisRect.height / 2);
                    if (uiFocusToShowRectDic.ContainsKey(item.Right))
                    {
                        Rect rightRect = uiFocusToShowRectDic[item.Right];
                        Vector3 target = rightRect.center;
                        DrawLineAction(thisVec, target, item.This, item.Right);
                    }
                }
            }
        }

        Event e = Event.current;
        //绘制选择对象的四个游标
        if (selectUIFcous)
        {
            Rect thisRect = uiFocusToShowRectDic[selectUIFcous];
            Rect upRect = new Rect(thisRect.xMin + thisRect.width / 2 - 5 , thisRect.yMin - 10  , 10, 10);
            Rect downRect = new Rect(thisRect.xMin + thisRect.width / 2 - 5 , thisRect.yMax , 10, 10);
            Rect leftRect = new Rect(thisRect.xMin - 10 , thisRect.yMin + thisRect.height / 2 - 5 , 10, 10);
            Rect rightRect = new Rect(thisRect.xMax, thisRect.yMin + thisRect.height / 2 - 5 , 10, 10);
            GUI.DrawTexture(upRect, relashipButtonTexture);
            GUI.DrawTexture(downRect, relashipButtonTexture);
            GUI.DrawTexture(leftRect, relashipButtonTexture);
            GUI.DrawTexture(rightRect, relashipButtonTexture);
            if (e.button == 1 && e.rawType == EventType.mouseDown)
            {
                if (upRect.Contains(e.mousePosition))
                {
                    forwardType = EnumEditorGUIFocusPathForward.Up;
                }
                else if (downRect.Contains(e.mousePosition))
                {
                    forwardType = EnumEditorGUIFocusPathForward.Down;
                }
                else if (leftRect.Contains(e.mousePosition))
                {
                    forwardType = EnumEditorGUIFocusPathForward.Left;
                }
                else if (rightRect.Contains(e.mousePosition))
                {
                    forwardType = EnumEditorGUIFocusPathForward.Right;
                }
            }
        }

        if (e.button == 1)
        {
            if (forwardType != null)
            {
                if (selectUIFcous)
                {
                    Rect thisRect = uiFocusToShowRectDic[selectUIFcous];
                    Rect upRect = new Rect(thisRect.xMin + thisRect.width / 2 - 5, thisRect.yMin - 10, 10, 10);
                    Rect downRect = new Rect(thisRect.xMin + thisRect.width / 2 - 5, thisRect.yMax, 10, 10);
                    Rect leftRect = new Rect(thisRect.xMin - 10, thisRect.yMin + thisRect.height / 2 - 5, 10, 10);
                    Rect rightRect = new Rect(thisRect.xMax, thisRect.yMin + thisRect.height / 2 - 5, 10, 10);
                    Handles.color = Color.gray;
                    switch (forwardType.Value)
                    {
                        case EnumEditorGUIFocusPathForward.Left:
                            Handles.DrawLine(leftRect.center, e.mousePosition);
                            break;
                        case EnumEditorGUIFocusPathForward.Right:
                            Handles.DrawLine(rightRect.center, e.mousePosition);
                            break;
                        case EnumEditorGUIFocusPathForward.Up:
                            Handles.DrawLine(upRect.center, e.mousePosition);
                            break;
                        case EnumEditorGUIFocusPathForward.Down:
                            Handles.DrawLine(downRect.center, e.mousePosition);
                            break;
                    }
                }
            }
        }

        if (e.button == 1 && e.rawType == EventType.mouseUp)
        {
            if (forwardType != null)
            {
                KeyValuePair<UIFocus, Rect>[] mouseUpResultArray = uiFocusToShowRectDic.Where(temp => temp.Value.Contains(e.mousePosition)).ToArray();

                KeyValuePair<UIFocus, Rect> mouseUpResult = mouseUpResultArray.FirstOrDefault();
                UIFocusPath.FocusRelaship focusRelaship = uiFocusPath.UIFocusArrayRelaships.FirstOrDefault(temp => temp.This == selectUIFcous);
                if (focusRelaship != null)
                {
                    switch (forwardType.Value)
                    {
                        case EnumEditorGUIFocusPathForward.Left:
                            focusRelaship.Left = mouseUpResult.Key;
                            break;
                        case EnumEditorGUIFocusPathForward.Right:
                            focusRelaship.Right = mouseUpResult.Key;
                            break;
                        case EnumEditorGUIFocusPathForward.Up:
                            focusRelaship.Up = mouseUpResult.Key;
                            break;
                        case EnumEditorGUIFocusPathForward.Down:
                            focusRelaship.Down = mouseUpResult.Key;
                            break;
                    }
                }
                forwardType = null;
            }
        }
    }

    public enum EnumEditorGUIFocusPathForward
    {
        Left,
        Right,
        Up,
        Down
    }

}
