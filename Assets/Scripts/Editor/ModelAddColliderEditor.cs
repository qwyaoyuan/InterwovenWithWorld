using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// 模型添加碰撞器编辑器
/// </summary>
public class ModelAddColliderEditor : EditorWindow
{

    public string[] removeStrs = new string[] { "pot","bowl" };

    [MenuItem("小工具/批量给模型添加碰撞器")]
    static void AddWindow()
    {
        ModelAddColliderEditor modelAddColliderEditor = EditorWindow.GetWindow<ModelAddColliderEditor>();
        modelAddColliderEditor.Show();
    }

    private void Awake()
    {

    }

    private void OnInspectorUpdate()
    {
        base.Repaint();
    }

    /// <summary>
    /// 选择模型的列表值
    /// </summary>
    Vector2 selectObjScroll;

    /// <summary>
    /// 顶层数组
    /// </summary>
    TargetTreeStruct[] topStructs;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        //选择游戏对象列
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("关闭"))
            Close();
        if (GUILayout.Button("刷新"))
        {
            topStructs = SetShowGameObject(Selection.gameObjects, 1);
        }
        if (topStructs != null && topStructs.Length > 0 && GUILayout.Button("添加触发器") && EditorUtility.DisplayDialog("提示!", "请再次确认,并提前保存备份!", "确认", "取消"))
        {
            foreach (TargetTreeStruct topStruct in topStructs)
            {
                AddTrigger(topStruct);
            }
        }
        EditorGUILayout.LabelField("选中的游戏对象--渲染对象--碰撞器");
        selectObjScroll = EditorGUILayout.BeginScrollView(selectObjScroll);
        if (topStructs != null && topStructs.Length > 0)
        {
            foreach (TargetTreeStruct topStruct in topStructs)
            {
                UIShowGameObject(topStruct);
            }

        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 设置显示的结构
    /// </summary>
    /// <param name="objs"></param>
    /// <returns></returns>
    private TargetTreeStruct[] SetShowGameObject(GameObject[] objs, int deep)
    {
        TargetTreeStruct[] results = new TargetTreeStruct[objs.Length];
        for (int i = 0; i < results.Length; i++)
        {
            TargetTreeStruct result = new TargetTreeStruct();
            result.space = deep * 20;
            result.targetTrans = objs[i].transform;
            result.meshRenderer = result.targetTrans.GetComponent<MeshRenderer>();
            result.collider = result.targetTrans.GetComponent<Collider>();
            int childCount = result.targetTrans.childCount;
            GameObject[] objChilds = Enumerable.Range(0, childCount).Select(temp => result.targetTrans.GetChild(temp).gameObject).ToArray();
            result.childs = SetShowGameObject(objChilds, deep + 1);
            results[i] = result;
        }
        return results;
    }

    /// <summary>
    /// UI显示模型 
    /// </summary>
    ///<param name="topStruct"></param>
    private void UIShowGameObject(TargetTreeStruct topStruct)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(topStruct.space);
        EditorGUILayout.ObjectField(topStruct.targetTrans.gameObject, typeof(GameObject), true, GUILayout.Width(100));
        EditorGUILayout.Toggle(topStruct.meshRenderer != null, GUILayout.Width(20));
        EditorGUILayout.Toggle(topStruct.collider != null, GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        if (topStruct.childs != null && topStruct.childs.Length > 0)
        {
            foreach (TargetTreeStruct childStruct in topStruct.childs)
            {
                UIShowGameObject(childStruct);
            }
        }
    }

    /// <summary>
    /// 添加触发器
    /// </summary>
    /// <param name="topStruct"></param>
    private void AddTrigger(TargetTreeStruct topStruct)
    {
        if (topStruct.meshRenderer != null && topStruct.collider == null)
        {
            bool canSet = true;
            foreach (string removeStr in removeStrs)
            {
                if (topStruct.targetTrans.name.Contains(removeStr))
                {
                    canSet = false;
                    break;
                }
            }
            if (canSet)
            {
                try
                {
                    MeshCollider meshCollider = topStruct.targetTrans.gameObject.AddComponent<MeshCollider>();
                    if (meshCollider != null)
                    {
                        topStruct.collider = meshCollider;
                        meshCollider.inflateMesh = true;
                        meshCollider.convex = true;
                        meshCollider.isTrigger = true;
                    }
                }
                catch { }
            }
        }
        if (topStruct.childs != null && topStruct.childs.Length > 0)
        {
            foreach (TargetTreeStruct childStruct in topStruct.childs)
            {
                AddTrigger(childStruct);
            }
        }
    }

    /// <summary>
    /// 目标树结构
    /// </summary>
    public class TargetTreeStruct
    {
        public Transform targetTrans;
        public int space;
        public MeshRenderer meshRenderer;
        public Collider collider;
        public TargetTreeStruct[] childs;
    }
}


