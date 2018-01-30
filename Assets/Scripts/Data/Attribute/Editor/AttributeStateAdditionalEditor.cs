using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ReflectEncapsulation;
using System;
using System.Reflection;
using System.Linq;

/// <summary>
/// 属性类编辑器
/// </summary>
public class AttributeStateAdditionalEditor : EditorWindow
{
    /// <summary>
    /// 需要编辑的目标
    /// </summary>
    public AttributeStateAdditional Target;

    [MenuItem("小工具/属性编辑器")]
    static void AddWindow()
    {
        AttributeStateAdditionalEditor attributeStateAdditionalEditor = EditorWindow.GetWindow<AttributeStateAdditionalEditor>();
        attributeStateAdditionalEditor.Show();
    }

    private void Awake()
    {
        Target = new AttributeStateAdditional();
    }

    AttributeStateAdditionalEditor()
    {
        this.titleContent = new GUIContent("属性编辑器");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    /// <summary>
    /// 滑动条数值
    /// </summary>
    Vector2 scrolls;

    private void OnGUI()
    {
        if (Target == null)
        {
            EditorGUILayout.LabelField("该编辑器是辅助编辑,请通过其他编辑器打开");
            return;
        }
        scrolls = EditorGUILayout.BeginScrollView(scrolls);
        Type attributeType = typeof(IAttributeState);
        PropertyInfo[] propertyInfos = attributeType.GetProperties();
        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            if (!Type.Equals(propertyInfo.PropertyType, typeof(float)))
                continue;
            FieldExplanAttribute fieldExplanAttribute = propertyInfo.GetCustomAttributes(typeof(FieldExplanAttribute), true).OfType<FieldExplanAttribute>().FirstOrDefault();
            if (fieldExplanAttribute == null)
                continue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fieldExplanAttribute.GetExplan());
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            float value = 0;
            if (getMethod != null)
            {
                value = (float)getMethod.Invoke(Target, null);
            }
            value = EditorGUILayout.FloatField(value);
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod != null)
            {
                setMethod.Invoke(Target, new object[] { value });
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

}
