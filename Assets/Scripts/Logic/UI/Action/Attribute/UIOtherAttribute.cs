using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// 属性展示界面的其他属性
/// </summary>
public class UIOtherAttribute : MonoBehaviour
{
    /// <summary>
    /// 显示文字的控件
    /// </summary>
    [SerializeField]
    Text showText;

    /// <summary>
    /// 要显示的属性
    /// </summary>
    [SerializeField]
    string field_Attribute;

    /// <summary>
    /// 角色属性状态
    /// </summary>
    IPlayerAttributeState iPlayerAttributeState;

    /// <summary>
    /// 显示的名字
    /// </summary>
    string showName;

    private void OnEnable()
    {
        iPlayerAttributeState = GameState.Instance.GetEntity<IPlayerAttributeState>();
        GameState.Instance.Registor<IPlayerAttributeState>(IPlayerAttributeStateChanged);
        Type t = typeof(IAttributeState);
        PropertyInfo propertyInfo = t.GetProperty(field_Attribute);
        if (propertyInfo != null)
        {
            FieldExplanAttribute fea = FieldExplanAttribute.GetPropertyExplan(propertyInfo);
            if (fea != null)
            {
                showName = fea.GetExplan();
            }
            else showName = null;
        }
        else showName = null;
        if (!string.IsNullOrEmpty(showName))
        {
            IPlayerAttributeStateChanged(iPlayerAttributeState, propertyInfo.Name);
        }
    }

    /// <summary>
    /// 玩家属性发生变化时
    /// </summary>
    /// <param name="iPlayerAttributeState"></param>
    /// <param name="fieldName"></param>
    private void IPlayerAttributeStateChanged(IPlayerAttributeState iPlayerAttributeState, string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(field_Attribute))
            return;
        if (string.Equals(fieldName, field_Attribute))
        {
            Type t = typeof(IAttributeState);
            PropertyInfo propertyInfo = t.GetProperty(field_Attribute);
            MethodInfo methodInfo = propertyInfo.GetGetMethod();
            if (methodInfo != null)
            {
                showText.text = showName + ":" + methodInfo.Invoke(iPlayerAttributeState, null);
            }
        }
    }

    private void OnDisable()
    {
        GameState.Instance.UnRegistor<IPlayerAttributeState>(IPlayerAttributeStateChanged);
    }
}
