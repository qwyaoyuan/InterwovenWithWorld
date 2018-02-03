using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///  文本你输入框获取焦点
/// </summary>
public class UIFocusInputField : UIFocus {

    InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    public override void SetForcus()
    {
        if (inputField)
        {
            inputField.Select();
        }
    }

    public void EnterInputField()
    {
        if (inputField)
        {
            EventTrigger eventTrigger = inputField.GetComponent<EventTrigger>();
            eventTrigger.OnSubmit(new PointerEventData(EventSystem.current));
        }
    }
}
