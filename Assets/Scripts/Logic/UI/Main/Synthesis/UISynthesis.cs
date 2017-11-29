using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 合成界面 
/// </summary>
public class UISynthesis : MonoBehaviour
{

    /// <summary>
    /// 合成列表树
    /// </summary>
    [SerializeField]
    private UITree synthesisItemTree;
    /// <summary>
    /// 需求材料以及说明例子对象
    /// </summary>
    [SerializeField]
    private GameObject exampleTextObj;
    /// <summary>
    /// 展示说明的容器对象 
    /// </summary>
    [SerializeField]
    private Transform detailedInformationTrans;
    /// <summary>
    /// 展示说明的条目对象(用于生成)
    /// </summary>
    [SerializeField]
    private GameObject detailedInformationTestObj;

    /// <summary>
    /// 合成资料对象
    /// </summary>
    SynthesisStructData synthesisStructData;
    /// <summary>
    /// 合成状态枚举
    /// </summary>
    ISynthesisState iSynthesisState;
    /// <summary>
    /// 玩家状态(存档)
    /// </summary>
    PlayerState playerState;
    /// <summary>
    /// 游戏状态对象
    /// </summary>
    IGameState iGameState;

    /// <summary>
    /// 刚开始时游戏状态
    /// </summary>
    EnumGameRunType oldGameRunType;

    private void Awake()
    {
        synthesisItemTree.SelectNodeChangedHandle += SynthesisItemTree_SelectNodeChangedHandle;
    }

    private void OnEnable()
    {
        iGameState = GameState.Instance.GetEntity<IGameState>();
        synthesisStructData = DataCenter.Instance.GetMetaData<SynthesisStructData>();
        playerState = DataCenter.Instance.GetEntity<PlayerState>();
        iSynthesisState = GameState.Instance.GetEntity<ISynthesisState>();
        oldGameRunType = iGameState.GameRunType;
        iGameState.GameRunType = EnumGameRunType.Synthesis;
        UIManager.Instance.KeyUpHandle += Instance_KeyUpHandle;
    }

    /// <summary>
    /// 初始化显示
    /// </summary>
    private void InitShow()
    {
        synthesisItemTree.Clear();
        SynthesisDataStruct[] synthesisDataStructs = synthesisStructData.SearchSynthesisData(temp => temp.synthesisType == iSynthesisState.SynthesisType);
        EnumSynthesisItem[] enumSynthesisItems = synthesisDataStructs.Select(temp => temp.synthesisItem).Distinct().ToArray();
        //这里需要将枚举转换成对应的文字,但是这里直接使用枚举了(这里添加的是根节点)
        Dictionary<EnumSynthesisItem, UITreeNode> rootNodeDic = enumSynthesisItems.ToDictionary(temp => temp,
            temp =>
            {
                UITreeNode rootNode = synthesisItemTree.CreateTreeNode();
                rootNode.ExplanText = temp.ToString();
                rootNode.IsDisplay = true;
                rootNode.IsExpand = true;
                rootNode.value = null;
                synthesisItemTree.AddTreeNode(rootNode);
                return rootNode;
            });
        //这里添加的是各自类型的子节点
        foreach (KeyValuePair<EnumSynthesisItem, UITreeNode> rootNode in rootNodeDic)
        {
            SynthesisDataStruct[] tempSynthesisDataStructs = synthesisDataStructs.Where(temp => temp.synthesisItem == rootNode.Key).ToArray();
            foreach (SynthesisDataStruct tempSynthesisDataStruct in tempSynthesisDataStructs)
            {
                UITreeNode createNode = synthesisItemTree.CreateTreeNode();
                createNode.ExplanText = tempSynthesisDataStruct.name;
                createNode.IsDisplay = true;
                createNode.IsExpand = true;
                createNode.value = tempSynthesisDataStruct;
                rootNode.Value.Add(createNode);
            }
        }
        //选择设置第一个选项为当前选项
        if (synthesisItemTree.Count > 0)
        {
            synthesisItemTree[0].IsSelect = true;
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.KeyUpHandle -= Instance_KeyUpHandle;
        iGameState.GameRunType = oldGameRunType;
    }

    /// <summary>
    /// 按键检测
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="rockValue"></param>
    private void Instance_KeyUpHandle(UIManager.KeyType keyType, Vector2 rockValue)
    {
        switch (keyType)
        {
            case UIManager.KeyType.A:
                if (synthesisItemTree.SelectNode.value == null)//展开或收起该节点
                {
                    synthesisItemTree.SelectNode.IsExpand = !synthesisItemTree.SelectNode.IsExpand;
                }
                else if (synthesisItemTree.SelectNode.value.GetType().Equals(typeof(SynthesisDataStruct)))
                {
                    //合成物品
                    iSynthesisState.SynthesisGoods(synthesisItemTree.SelectNode.value as SynthesisDataStruct);
                }
                break;
            case UIManager.KeyType.B:
                //返回
                this.gameObject.SetActive(false);
                break;
            case UIManager.KeyType.UP:
                synthesisItemTree.SelectUPTreeNode();
                break;
            case UIManager.KeyType.DOWN:
                synthesisItemTree.SelectDownTreeNode();
                break;

        }
    }

    /// <summary>
    /// 树节点选择项发生变化事件
    /// </summary>
    /// <param name="obj"></param>
    private void SynthesisItemTree_SelectNodeChangedHandle(UITreeNode obj)
    {
        ShowNowSelectTreeNodeExplan();
    }

    /// <summary>
    /// 显示当前选择节点的说明
    /// </summary>
    private void ShowNowSelectTreeNodeExplan()
    {
        //清理之前的数据
        int count = detailedInformationTrans.childCount;
        Transform[] childsTrans = Enumerable.Range(0, count).Select(temp => detailedInformationTrans.GetChild(temp)).ToArray();
        foreach (Transform childTrans in childsTrans)
        {
            if (Transform.Equals(detailedInformationTestObj, childTrans))//不删除例子
                continue;
            GameObject.DestroyImmediate(childTrans.gameObject);
        }
        //通过节点的数据判断该节点是什么节点
        UITreeNode selectNode = synthesisItemTree.SelectNode;
        //如果是SynthesisDataStruct数据的节点则显示SynthesisDataStruct数据的内容
        if (selectNode.value != null && selectNode.value.GetType().Equals(typeof(SynthesisDataStruct)))
        {
            SynthesisDataStruct synthesisDataStruct = selectNode.value as SynthesisDataStruct;
            //添加条目
            Action<string> AddExplanTitleAction = (title) =>
            {
                GameObject createTitle = GameObject.Instantiate<GameObject>(detailedInformationTestObj);
                createTitle.transform.SetParent(detailedInformationTrans);
                createTitle.GetComponent<Text>().text = title;
                createTitle.SetActive(true);
            };
            //输入结构
            AddExplanTitleAction("合成条件:");
            foreach (SynthesisDataStruct.SynthesisItemStruct synthesisItemStruct in synthesisDataStruct.inputStruct)
            {
                int nowCount = playerState.PlayerAllGoods.Count(temp => temp.GoodsInfo.EnumGoodsType == synthesisItemStruct.itemType);
                //这里需要转换成对应的名字,这里暂时用枚举名
                AddExplanTitleAction(synthesisItemStruct.itemType + ":" + nowCount + "/" + synthesisItemStruct.num);
            }
            //输出结构
            AddExplanTitleAction("合成物品:");
            //这里需要转换成对应的名字,这里暂时用枚举名
            AddExplanTitleAction(synthesisDataStruct.outputStruct.itemType.ToString());
            //自动对齐
            VerticalLayoutGroup verticalLayoutGroup = detailedInformationTrans.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.SetLayoutHorizontal();
            verticalLayoutGroup.SetLayoutVertical();
        }
    }

}
