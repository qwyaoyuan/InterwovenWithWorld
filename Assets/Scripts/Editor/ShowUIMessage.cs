using UnityEngine;
using UnityEditor;
using System.Collections;

public class ShowUIMessage : EditorWindow
{
    [MenuItem("MyUGUIToos/ShowUIMessage")]
    static void AddWindow()
    {
        ShowUIMessage window = (ShowUIMessage)EditorWindow.GetWindow(typeof(ShowUIMessage), false);
        window.Show();
    }

    void OnInspectorUpdate()
    {
        this.Repaint();

    }

    void OnGUI()
    {
        GameObject[] selectObjs = Selection.gameObjects;
        for (int i = 0; i < selectObjs.Length; i++)
        {
            RectTransform rectTrans = selectObjs[i].GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                DrawRectTransform(rectTrans);
            }
        }
    }

    void DrawRectTransform(RectTransform rectTrans)
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("UIName:" + rectTrans.name);
        EditorGUILayout.LabelField("    Location:" + rectTrans.rect.position);
        EditorGUILayout.LabelField("    Size:" + rectTrans.rect.size);
        EditorGUILayout.LabelField("    Center:" + rectTrans.position);
        if (rectTrans.parent != null && rectTrans.parent.GetComponent<RectTransform>() != null)
        {
            Rect rect = EditorGUILayout.RectField("    Rect:", rectTrans.rect);
            Vector2 size = rectTrans.rect.size - rect.size;//缩放
            Vector2 move = rect.position - rectTrans.rect.position;//移动
            rectTrans.offsetMin = new Vector2(rectTrans.offsetMin.x + size.x / 2, rectTrans.offsetMin.y + size.y / 2) + move;
            rectTrans.offsetMax = new Vector2(rectTrans.offsetMax.x - size.x / 2, rectTrans.offsetMax.y - size.y / 2) + move;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("    ");
            if (GUILayout.Button("AutoAnchor"))
            {
                RectTransform parent = rectTrans.parent.GetComponent<RectTransform>();
                Vector2 parentSize = parent.rect.size;
                Vector2 offsetMin = rectTrans.offsetMin;
                Vector2 offsetMax = rectTrans.offsetMax;
                Vector2 anchorMin = rectTrans.anchorMin;
                Vector2 anchorMax = rectTrans.anchorMax;
                Vector2 biliMin = new Vector2(offsetMin.x / parentSize.x, offsetMin.y / parentSize.y);
                Vector2 biliMax = new Vector2(offsetMax.x / parentSize.x, offsetMax.y / parentSize.y);
                rectTrans.anchorMin = anchorMin + biliMin;
                rectTrans.anchorMax = anchorMax + biliMax;
                rectTrans.offsetMin = Vector2.zero;
                rectTrans.offsetMax = Vector2.zero;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}
