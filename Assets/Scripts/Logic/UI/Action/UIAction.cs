using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 功能界面
/// </summary>
public class UIAction : MonoBehaviour
{
    /// <summary>
    /// 标签路径
    /// </summary>
    UIFocusPath uiFocusPath;
    /// <summary>
    /// 当前的标签页焦点
    /// </summary>
    UIFocusTabPage nowTabPageFocus;

    /// <summary>
    /// 游戏状态接口对象 
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 是否可以切换标签,如果是通过特定功能点开的界面则不可以(判断方法ShowIndex函数传入参数为0表示随意,传入参数不为0则不可以)
    /// </summary>
    bool CanChangeTab;

    /// <summary>
    /// 是否正在保存中
    /// </summary>
    public static bool isSaving;

    void Awake()
    {
        uiFocusPath = GetComponent<UIFocusPath>();
    }

    private void OnEnable()
    {
        isSaving = false;
        iGameState = GameState.Instance.GetEntity<IGameState>();
        //压入状态 
        iGameState.PushEnumGameRunType(EnumGameRunType.Setting);
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
        if (uiFocusPath)
        {
            nowTabPageFocus = uiFocusPath.GetFirstFocus() as UIFocusTabPage;
            if (nowTabPageFocus)
            {
                nowTabPageFocus.SetForcus();
                if (nowTabPageFocus.panel)
                    nowTabPageFocus.panel.gameObject.SetActive(true);
            }
        }
        //重新载入数据
        //检测此时是否有点击地图的ID(如果是则载入地图标签)
        IInteractiveState iInteractiveState = GameState.Instance.GetEntity<IInteractiveState>();
        NPCData npcData = DataCenter.Instance.GetMetaData<NPCData>();
        NPCDataInfo npcDataInfo = npcData.GetNPCDataInfo(iGameState.SceneName, iInteractiveState.ClickInteractiveNPCID);
        if (npcDataInfo != null && npcDataInfo.NPCType == EnumNPCType.Street)
        {
            UIFocusTabPage[] tabPages = uiFocusPath.NewUIFocusArray.OfType<UIFocusTabPage>().ToArray();
            var tempUIBigMaps = Enumerable.Range(0, tabPages.Length).Select(temp => new { index = temp, uiBigMap = tabPages[temp].panel.GetComponent<UIBigMap>() }).Where(temp => temp.uiBigMap != null).FirstOrDefault();
            if (tempUIBigMaps != null)
            {
                ShowIndex(tempUIBigMaps.index);
            }
            else
            {
                ShowIndex(0);
            }
        }
        else//如果不是则载入第一个标签
        {
            ShowIndex(0);
        }
    }

    private void OnDisable()
    {
        //弹出状态
        iGameState.PopEnumGameRunType();
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        ShowIndex(-1);
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        if (isSaving)
            return;
        if (uiFocusPath && CanChangeTab)//切换标签页
        {
            UIFocus nextTabPageFocus = null;
            switch (keyType)
            {
                case UIManager.KeyType.R1:
                    nextTabPageFocus = uiFocusPath.GetNewNextFocus(nowTabPageFocus, UIFocusPath.MoveType.RIGHT);// uiFocusPath.GetNextFocus(nowTabPageFocus, UIFocusPath.MoveType.RIGHT, true);
                    break;
                case UIManager.KeyType.L1:
                    nextTabPageFocus = uiFocusPath.GetNewNextFocus(nowTabPageFocus, UIFocusPath.MoveType.LEFT);//uiFocusPath.GetNextFocus(nowTabPageFocus, UIFocusPath.MoveType.LEFT, true);
                    break;
            }
            TabPageClick(nextTabPageFocus as UIFocusTabPage);
        }
        switch (keyType)
        {
            case UIManager.KeyType.START://返回
                CloseActionClick();
                break;
        }
    }

    /// <summary>
    /// 点击关闭功能面板
    /// </summary>
    public void CloseActionClick()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击标签页
    /// </summary>
    /// <param name="nextTabPageFocus"></param>
    public void TabPageClick(UIFocusTabPage nextTabPageFocus)
    {
        if (nextTabPageFocus != null)
        {
            nowTabPageFocus.LostForcus();
            nowTabPageFocus.panel.gameObject.SetActive(false);
            nowTabPageFocus = nextTabPageFocus as UIFocusTabPage;
            nowTabPageFocus.panel.gameObject.SetActive(true);
            nowTabPageFocus.SetForcus();
        }
    }

    /// <summary>
    /// 显示指定的标签
    /// </summary>
    /// <param name="index"></param>
    public void ShowIndex(int index)
    {
        CanChangeTab = index == 0;
        UIFocusTabPage[] tabPages = uiFocusPath.NewUIFocusArray.OfType<UIFocusTabPage>().ToArray();//uiFocusPath.UIFocuesArray.OfType<UIFocusTabPage>().ToArray();
        if (tabPages.Length > 0)
        {
            if (index < 0)
                tabPages.ToList().ForEach(temp =>
                {
                    temp.panel.gameObject.SetActive(false);
                    temp.LostForcus();
                });
            else
            {
                if (index > tabPages.Length - 1)
                    index = 0;
                for (int i = 0; i < tabPages.Length; i++)
                {
                    tabPages[i].panel.gameObject.SetActive(i == index);
                    if (i == index)
                        tabPages[i].SetForcus();
                    else
                        tabPages[i].LostForcus();
                }
            }
        }
    }


}
