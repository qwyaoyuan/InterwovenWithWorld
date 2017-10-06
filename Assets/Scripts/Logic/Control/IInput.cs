using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入接口
/// </summary>
public interface IInput
{
    /// <summary>
    /// 按下指定键
    /// </summary>
    /// <param name="key">键的数值</param>
    void KeyPress(int key);
    /// <summary>
    /// 释放指定键
    /// 如果是手柄则L1 L2 R1 R2键在按下时，如果松开其他键，则此时的键位也要计算进去
    /// 同理键盘的Shift或Ctrl键也是一样的
    /// </summary>
    /// <param name="key">键的数值</param>
    void KeyUp(int key);
    /// <summary>
    /// 输入的移动数值
    /// 对应键盘的WASD键
    /// 对应手柄的左摇杆
    /// </summary>
    /// <param name="forward"></param>
    void Move(Vector2 forward);
    /// <summary>
    /// 输入的视角数值
    /// 对应键盘的上下左右键或鼠标的上下左右移动
    /// 对应手柄的右摇杆
    /// </summary>
    /// <param name="view"></param>
    void View(Vector2 view);
}


