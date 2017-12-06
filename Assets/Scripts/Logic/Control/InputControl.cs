using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入管理器
/// </summary>
public class InputControl : IEntrance
{
    /// <summary>
    /// 输入对象集合
    /// </summary>
    List<IInput> inputList;

    /// <summary>
    /// 十字键的基础值
    /// </summary>
    float XBOX_XY_Base = 0.5f;

    /// <summary>
    /// 扳机键的基础值 
    /// </summary>
    float XBOX_LRT_Base = 0.5f;

    /// <summary>
    /// 按下集合
    /// </summary>
    List<int> downList;

    /// <summary>
    /// 按住集合
    /// </summary>
    List<int> pressList;

    /// <summary>
    /// 松开集合
    /// </summary>
    List<int> upList;

    public void Start()
    {
        downList = new List<int>();
        pressList = new List<int>();
        upList = new List<int>();

        inputList = new List<IInput>();
        inputList.Add(UIManager.Instance);
        //inputList.Add(SettingManager.Instance);
        inputList.Add(MoveManager.Instance);
        inputList.Add(InteractiveManager.Instance);
        inputList.Add(SkillManager.Instance);
    }

    /// <summary>
    /// 是否按下十字键左
    /// </summary>
    bool pressLeft;
    /// <summary>
    /// 是否按下十字键右
    /// </summary>
    bool pressRight;
    /// <summary>
    /// 是否按下十字键上
    /// </summary>
    bool pressUP;
    /// <summary>
    /// 是否按下十字键下
    /// </summary>
    bool pressDown;
    /// <summary>
    /// 是否按下RT键
    /// </summary>
    bool pressRT;
    /// <summary>
    /// 是否按下LT键
    /// </summary>
    bool pressLT;



    public void Update()
    {
        float hl = Input.GetAxis("Horizontal_Left");//左摇杆左右(左=-1 右=1)
        float vl = Input.GetAxis("Vertical_Left");//左摇杆上下(上=1 下=-1)
        float x = Input.GetAxis("XBox+X");//十字键左右(左=-1 右=1)
        float y = Input.GetAxis("XBox+Y");//十字键上下(上=1 下=-1)
        float hr = Input.GetAxis("Horizontal_Right");//右摇杆左右(左=-1 右=1)
        float vr = Input.GetAxis("Vertical_Right");//右摇杆上下(上=1 下=-1)
        float t = Input.GetAxis("LRT");//LT和RT键(LT=-1 RT=1 这两个是互斥的,如果同时按为0)
        //十字键左的按下释放与按住
        bool pressDownLeft = pressLeft ? false : (x < -XBOX_XY_Base ? true : false);//如果之前持续按住十字键左键则这里没有按下十字键,如果没有持续按住左键,在值到达指定范围时按下了,没有则没有按下
        bool releaseDownLeft = pressLeft ? (x > -XBOX_XY_Base ? true : false) : false;//如果之前持续按十字键,在如果值在指定范围内则松开,否则没有松开,如果没有持续按十字键,则没有松开
        pressLeft = x < -XBOX_XY_Base;
        //十字键右的按下释放与按住
        bool pressDownRight = pressRight ? false : (x > XBOX_XY_Base ? true : false);//同上
        bool releaseDownRight = pressRight ? (x < XBOX_XY_Base ? true : false) : false;//同上
        pressRight = x > XBOX_XY_Base;
        //十字键上的按下释放与按住
        bool pressDownUP = pressUP ? false : (y > XBOX_XY_Base ? true : false);//同上
        bool releaseDownUP = pressUP ? (y < XBOX_XY_Base ? true : false) : false;//同上
        pressUP = y > XBOX_XY_Base;
        //十字键下的按下释放与按住
        bool pressDownDown = pressDown ? false : (y < -XBOX_XY_Base ? true : false);//同上
        bool releaseDownDown = pressDown ? (y > -XBOX_XY_Base ? true : false) : false;//同上
        pressDown = y < -XBOX_XY_Base;
        //LT(左扳机键)的按下释放与按住
        bool pressDownLT = pressLT ? false : (t < -XBOX_LRT_Base ? true : false);//同上
        bool releaseDownLT = pressLT ? (t > -XBOX_LRT_Base ? true : false) : false;//同上
        pressLT = t < -XBOX_LRT_Base;
        //RT(右扳机键)的按下释放与按住
        bool pressDownRT = pressRT ? false : (t > XBOX_LRT_Base ? true : false);//同上
        bool releaseDownRT = pressRT ? (t < XBOX_LRT_Base ? true : false) : false;//同上 
        pressRT = t > XBOX_LRT_Base;
        //A键的按下释放与按住
        bool pressDownA = Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.JoystickButton0);
        bool releaseDownA = Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.JoystickButton0);
        bool pressA = Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.JoystickButton0);
        //B键的按下释放与按住 
        bool pressDownB = Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.JoystickButton1);
        bool releaseDownB = Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.JoystickButton1);
        bool pressB = Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.JoystickButton1);
        //X键的按下释放与按住
        bool pressDownX = Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.JoystickButton2);
        bool releaseDownX = Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.JoystickButton2);
        bool pressX = Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.JoystickButton2);
        //Y键的按下释放与按住
        bool pressDownY = Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.JoystickButton3);
        bool releaseDownY = Input.GetKeyUp(KeyCode.Joystick1Button3) || Input.GetKeyUp(KeyCode.JoystickButton3);
        bool pressY = Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.JoystickButton3);
        //LB键的按下释放与按住
        bool pressDownLB = Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.JoystickButton4);
        bool releaseDownLB = Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.JoystickButton4);
        bool pressLB = Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.JoystickButton4);
        //RB键的按下释放与按住
        bool pressDownRB = Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.JoystickButton5);
        bool releaseDownRB = Input.GetKeyUp(KeyCode.Joystick1Button5) || Input.GetKeyUp(KeyCode.JoystickButton5);
        bool pressRB = Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.JoystickButton5);
        //Back键的按下释放与按住
        bool pressDownBack = Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.JoystickButton6);
        bool releaseDownBack = Input.GetKeyUp(KeyCode.Joystick1Button6) || Input.GetKeyUp(KeyCode.JoystickButton6);
        bool pressBack = Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.JoystickButton6);
        //Start键的按下释放与按住
        bool pressDownStart = Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.JoystickButton7);
        bool releaseDownStart = Input.GetKeyUp(KeyCode.Joystick1Button7) || Input.GetKeyUp(KeyCode.JoystickButton7);
        bool pressStart = Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.JoystickButton7);
        //左摇杆Down键的按下释放与按住
        bool pressDownL = Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKeyDown(KeyCode.JoystickButton8);
        bool releaseDownL = Input.GetKeyUp(KeyCode.Joystick1Button8) || Input.GetKeyUp(KeyCode.JoystickButton8);
        bool pressL = Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.JoystickButton8);
        //右摇杆Down键的按下释放与按住
        bool pressDownR = Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.JoystickButton9);
        bool releaseDownR = Input.GetKeyUp(KeyCode.Joystick1Button9) || Input.GetKeyUp(KeyCode.JoystickButton9);
        bool pressR = Input.GetKey(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.JoystickButton9);

        //EnumInputType
        downList.Clear();
        pressList.Clear();
        upList.Clear();
        Func<int, int> OverlayFunc = (baseData) => //叠加
        {
            return baseData | (pressLB ? (int)EnumInputType.LB : 0) | (pressLT ? (int)EnumInputType.LT : 0) | (pressRB ? (int)EnumInputType.RB : 0) | (pressRT ? (int)EnumInputType.RT : 0);
        };
        //LB键
        if (pressDownLB)
            downList.Add((int)EnumInputType.LB);
        if (pressLB)
            pressList.Add((int)EnumInputType.LB);
        if (releaseDownLB)
            upList.Add((int)EnumInputType.LB);
        //RB键
        if (pressDownRB)
            downList.Add((int)EnumInputType.RB);
        if (pressRB)
            pressList.Add((int)EnumInputType.RB);
        if (releaseDownRB)
            upList.Add((int)EnumInputType.RB);
        //LT键
        if (pressDownLT)
            downList.Add((int)EnumInputType.LT);
        if (pressLT)
            pressList.Add((int)EnumInputType.LT);
        if (releaseDownLT)
            upList.Add((int)EnumInputType.LT);
        //RT键
        if (pressDownRT)
            downList.Add((int)EnumInputType.RT);
        if (pressRT)
            pressList.Add((int)EnumInputType.RT);
        if (releaseDownRT)
            upList.Add((int)EnumInputType.RT);
        //十字左键
        if (pressDownLeft)
            downList.Add(OverlayFunc((int)EnumInputType.Left));
        if (pressLeft)
            pressList.Add(OverlayFunc((int)EnumInputType.Left));
        if (releaseDownLeft)
            upList.Add(OverlayFunc((int)EnumInputType.Left));
        //十字右键
        if (pressDownRight)
            downList.Add(OverlayFunc((int)EnumInputType.Right));
        if (pressRight)
            pressList.Add(OverlayFunc((int)EnumInputType.Right));
        if (releaseDownRight)
            upList.Add(OverlayFunc((int)EnumInputType.Right));
        //十字上键
        if (pressDownUP)
            downList.Add(OverlayFunc((int)EnumInputType.Up));
        if (pressUP)
            pressList.Add(OverlayFunc((int)EnumInputType.Up));
        if (releaseDownUP)
            upList.Add(OverlayFunc((int)EnumInputType.Up));
        //十字下键
        if (pressDownDown)
            downList.Add(OverlayFunc((int)EnumInputType.Down));
        if (pressDown)
            pressList.Add(OverlayFunc((int)EnumInputType.Down));
        if (releaseDownDown)
            upList.Add(OverlayFunc((int)EnumInputType.Down));
        //左摇杆中键
        if (pressDownL)
            downList.Add(OverlayFunc((int)EnumInputType.L3));
        if (pressL)
            pressList.Add(OverlayFunc((int)EnumInputType.L3));
        if (releaseDownL)
            upList.Add(OverlayFunc((int)EnumInputType.L3));
        //右摇杆中键
        if (pressDownR)
            downList.Add(OverlayFunc((int)EnumInputType.R3));
        if (pressR)
            pressList.Add(OverlayFunc((int)EnumInputType.R3));
        if (releaseDownR)
            upList.Add(OverlayFunc((int)EnumInputType.R3));
        //A键
        if (pressDownA)
            downList.Add(OverlayFunc((int)EnumInputType.A));
        if (pressA)
            pressList.Add(OverlayFunc((int)EnumInputType.A));
        if (releaseDownA)
            upList.Add(OverlayFunc((int)EnumInputType.A));
        //B键
        if (pressDownB)
            downList.Add(OverlayFunc((int)EnumInputType.B));
        if (pressB)
            pressList.Add(OverlayFunc((int)EnumInputType.B));
        if (releaseDownB)
            upList.Add(OverlayFunc((int)EnumInputType.B));
        //X键
        if (pressDownX)
            downList.Add(OverlayFunc((int)EnumInputType.X));
        if (pressX)
            pressList.Add(OverlayFunc((int)EnumInputType.X));
        if (releaseDownX)
            upList.Add(OverlayFunc((int)EnumInputType.X));
        //Y键
        if (pressDownY)
            downList.Add(OverlayFunc((int)EnumInputType.Y));
        if (pressY)
            pressList.Add(OverlayFunc((int)EnumInputType.Y));
        if (releaseDownY)
            upList.Add(OverlayFunc((int)EnumInputType.Y));
        

        //循环给键
        foreach (IInput iInput in inputList)
        {
            //按下
            foreach (int key in downList)
            {
                iInput.KeyDown(key);
            }
            //按住
            foreach (int key in pressList)
            {
                iInput.KeyPress(key);
            }
            //松开
            foreach (int key in upList)
            {
                iInput.KeyUp(key);
            }
            //左摇杆
            iInput.Move(new Vector2(hl, vl));
            //右摇杆 
            iInput.View(new Vector2(hr, vr));
        }
    }

    public void OnDestroy() { }
    
}
