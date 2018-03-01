using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using MapStruct;
using System.Runtime.InteropServices;

namespace TaskMap
{
    /// <summary>
    /// 任务编辑器
    /// </summary>
    public class TaskEditor : EditorWindow
    {
        public string dataPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\Task\Task.txt";

        [MenuItem("小工具/任务编辑器")]
        static void AddWindow()
        {
            TaskEditor taskEditor = EditorWindow.GetWindow<TaskEditor>();
            taskEditor.Show();
        }

        TaskEditor()
        {
            this.titleContent = new GUIContent("任务编辑器");
        }

        /// <summary>
        /// 任务图
        /// </summary>
        TaskMap taskMap;

        /// <summary>
        /// 按钮选中样式
        /// </summary>
        GUIStyle buttonSelectStyle;
        /// <summary>
        /// 按钮没有选中样式
        /// </summary>
        GUIStyle buttonNotSelectStyle;
        /// <summary>
        /// 按钮关闭样式
        /// </summary>
        GUIStyle buttonCloseStyle;

        /// <summary>
        /// 左侧按钮样式(第一个选择节点,选中)
        /// </summary>
        GUIStyle buttonFirstSelectNodeStyle;
        /// <summary>
        /// 左侧按钮样式(第一个选择节点,未选中)
        /// </summary>
        GUIStyle buttonFirstNotSelectNodeStyle;

        /// <summary>
        /// 显示关系面板的宽
        /// </summary>
        const int RelationShipWidth = 10000;
        /// <summary>
        /// 显示关系面板的高
        /// </summary>
        const int RelationShipHeight = 10000;
        /// <summary>
        /// 节点窗体的宽
        /// </summary>
        const int NodeWindowWidth = 240;
        /// <summary>
        /// 节点窗体的高
        /// </summary>
        const int NodeWindwoHeight = 40;
        /// <summary>
        /// 显示详细信息面板宽
        /// </summary>
        const int ShowExplanWidth = 200;
        /// <summary>
        /// 显示详细信息面板高
        /// </summary>
        const int ShowExplanHeight = 400;

        /// <summary>
        /// 显示关系面板的北京
        /// </summary>
        Texture2D ShowBackImage;

        /// <summary>
        /// 显示详细信息的面板
        /// </summary>
        TaskEditor_Explan taskEditor_Explan;
        /// <summary>
        /// 是否显示详细信息面板
        /// </summary>
        bool showTaskEditorExplan;

        private void Awake()
        {
            //if (!File.Exists(dataPath))
            //{
            //    File.Create(dataPath).Close();
            //    taskMap = new TaskMap();
            //    string jsonValue = taskMap.Save();
            //    File.WriteAllText(dataPath, jsonValue, Encoding.UTF8);
            //}
            //else
            //{
            //    string jsonValue = File.ReadAllText(dataPath, Encoding.UTF8);
            //    taskMap = new TaskMap();
            //    taskMap.Load(jsonValue);
            //}
            //secondScroll = new Vector2(RelationShipWidth / 2, RelationShipHeight / 2);
            //ShowBackImage = Resources.Load<Texture2D>("Task/ShowBack");
            LoadFile(dataPath);
            //按钮样式
            buttonSelectStyle = new GUIStyle();
            buttonSelectStyle.fontSize = 10;  //字体大小
            buttonSelectStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
            buttonSelectStyle.normal.background = Resources.Load<Texture2D>("Task/Blue");//背景.
            buttonSelectStyle.normal.textColor = Color.yellow;//文字颜色。

            buttonNotSelectStyle = new GUIStyle();
            buttonNotSelectStyle.fontSize = 10;  //字体大小
            buttonNotSelectStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
            buttonNotSelectStyle.normal.background = Resources.Load<Texture2D>("Task/Black");//背景.
            buttonNotSelectStyle.normal.textColor = Color.yellow;//文字颜色。

            buttonFirstSelectNodeStyle = new GUIStyle();
            buttonFirstSelectNodeStyle.fontSize = 10;  //字体大小
            buttonFirstSelectNodeStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
            buttonFirstSelectNodeStyle.normal.background = Resources.Load<Texture2D>("Task/Blue");//背景.
            buttonFirstSelectNodeStyle.normal.textColor = Color.red;//文字颜色。

            buttonFirstNotSelectNodeStyle = new GUIStyle();
            buttonFirstNotSelectNodeStyle.fontSize = 10;  //字体大小
            buttonFirstNotSelectNodeStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
            buttonFirstNotSelectNodeStyle.normal.background = Resources.Load<Texture2D>("Task/Black");//背景.
            buttonFirstNotSelectNodeStyle.normal.textColor = Color.red;//文字颜色。

            buttonCloseStyle = new GUIStyle();
            buttonCloseStyle.fontSize = 14;  //字体大小
            buttonCloseStyle.alignment = TextAnchor.MiddleCenter;//文字位置上下左右居中，
            buttonCloseStyle.normal.background = Resources.Load<Texture2D>("Task/Close");//背景.
            buttonCloseStyle.normal.textColor = Color.yellow;//文字颜色。

        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool LoadFile(string filePath)
        {
            try
            {
                TaskMap tempTaskMap = null;
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                    tempTaskMap = new TaskMap();
                    string jsonValue = tempTaskMap.Save();
                    File.WriteAllText(filePath, jsonValue, Encoding.UTF8);
                }
                else
                {
                    string jsonValue = File.ReadAllText(filePath, Encoding.UTF8);
                    tempTaskMap = new TaskMap();
                    tempTaskMap.Load(jsonValue);
                }
                if (tempTaskMap != null)
                    taskMap = tempTaskMap;
                else
                    return false;
                secondScroll = new Vector2(RelationShipWidth / 2, RelationShipHeight / 2);
                ShowBackImage = Resources.Load<Texture2D>("Task/ShowBack");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        private void OnGUI()
        {
            if (taskMap == null)
                return;
            //分二列展示
            EditorGUILayout.BeginHorizontal();
            //第一列显示所有节点
            ShowAllNode();
            //第二列显示节点的关系图
            ShowNodeCorrelatives();
            //显示详细信息
            if (taskEditor_Explan != null)
            {
                taskEditor_Explan.nowSelectNode = nowSelectNode;
                taskEditor_Explan.taskMap = taskMap;
            }
            EditorGUILayout.EndHorizontal();

        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }





        /// <summary>
        /// 当前选择的节点
        /// </summary>
        MapElement<TaskInfoStruct> nowSelectNode;
        /// <summary>
        /// 第一列的列表滑动条
        /// </summary>
        Vector2 firstScroll;
        /// <summary>
        /// 显示所有节点(列表形式)
        /// </summary>
        private void ShowAllNode()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("路径:", GUILayout.Width(50)))
            {
                OpenFileName ofn = LocalDialog.CreateOpenFileName();
                if (LocalDialog.GetOFN(ofn))
                {
                    if (LoadFile(ofn.file))
                    {
                        dataPath = ofn.file;
                    }
                }
            }
            EditorGUILayout.TextArea(dataPath, GUILayout.Width(165));
            //EditorGUILayout.LabelField(dataPath, GUILayout.Width(400));

            if (GUILayout.Button("保存", GUILayout.Width(80)))
            {
                string jsonValue = taskMap.Save();
                File.WriteAllText(dataPath, jsonValue, Encoding.UTF8);
                EditorUtility.DisplayDialog("提示!", "保存成功", "是");
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加", GUILayout.Width(80)))
            {
                MapElement<TaskInfoStruct> taskNode = taskMap.CreateElement();
                taskNode.Value = new TaskInfoStruct(taskNode.ID);
                taskNode.Value.ReceiveTaskNpcId = -1;
                taskNode.Value.NeedReputation = -1;
                taskNode.Value.NeedGetReputation = -1;
                taskNode.Value.X_Editor = secondScroll.x;
                taskNode.Value.Y_Editor = secondScroll.y;
            }
            EditorGUILayout.LabelField("详细设置", GUILayout.Width(50));
            showTaskEditorExplan = EditorGUILayout.Toggle(showTaskEditorExplan, GUILayout.Width(15));
            if (!showTaskEditorExplan && taskEditor_Explan != null)
                taskEditor_Explan.Close();
            EditorGUILayout.EndHorizontal();
            firstScroll = EditorGUILayout.BeginScrollView(firstScroll, GUILayout.Width(165));
            MapElement<TaskInfoStruct>[] nodes = taskMap.GetAllElement();
            MapElement<TaskInfoStruct> firstNode = taskMap.GetFirstElement();
            foreach (MapElement<TaskInfoStruct> node in nodes)
            {
                EditorGUILayout.BeginHorizontal();
                GUIStyle guiStyle = firstNode.ID == node.ID ? (node == nowSelectNode ? buttonFirstSelectNodeStyle : buttonFirstNotSelectNodeStyle) : (node == nowSelectNode ? buttonSelectStyle : buttonNotSelectStyle);
                if (GUILayout.Button(node.Name + ":" + node.ID, guiStyle, GUILayout.Width(120), GUILayout.Height(18)))
                {
                    nowSelectNode = node;
                    ShowExplan();
                }
                if (GUILayout.Button(buttonCloseStyle.normal.background, GUILayout.Width(20), GUILayout.Height(20)))
                {
                    if (EditorUtility.DisplayDialog("警告!", "是否删除该节点", "是", "否"))
                    {
                        taskMap.Remove(node.ID);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 第二列的列表滑动条
        /// </summary>
        Vector2 secondScroll;

        /// <summary>
        /// 关系键值对对应位置字典
        /// </summary>
        Dictionary<KeyValuePair<int, int>, Vector2[]> nodeRelationShipPointDic;

        /// <summary>
        /// 右键选择的节点
        /// </summary>
        MapElement<TaskInfoStruct> rightSelectNode;

        /// <summary>
        /// 选择的线
        /// </summary>
        KeyValuePair<int, int> selectRelationLine;

        /// <summary>
        /// 关系组对应连线节点位置字典
        /// </summary>
        Dictionary<RelationShipZone, Vector2[]> relationShipZonePointsDic;

        /// <summary>
        /// 选择的组
        /// </summary>
        RelationShipZone selectRelationShipZone;

        /// <summary>
        /// 框选区域
        /// </summary>
        Rect SelectRect;

        /// <summary>
        /// 使用Control选择ID数组
        /// </summary>
        int[] controlSelectIDs;

        /// <summary>
        /// 展示节点关系
        /// </summary>
        private void ShowNodeCorrelatives()
        {
            float width = position.width - 170;
            float height = position.height;
            secondScroll = GUI.BeginScrollView(new Rect(170, 0, width, height), secondScroll, new Rect(0, 0, RelationShipWidth, RelationShipHeight));
            GUI.DrawTexture(new Rect(0, 0, RelationShipWidth, RelationShipHeight), ShowBackImage);
            MapElement<TaskInfoStruct>[] nodes = taskMap.GetAllElement();
            #region 绘制图节点的关系(后绘制节点是因为这样可以将线条隐藏)
            //绘制节点关系
            nodeRelationShipPointDic = new Dictionary<KeyValuePair<int, int>, Vector2[]>();
            foreach (MapElement<TaskInfoStruct> node in nodes)
            {
                IMapElement<TaskInfoStruct>[] allTargets = node.CorrelativesNode.GetAll();
                foreach (IMapElement<TaskInfoStruct> target in allTargets)
                {
                    if (nodeRelationShipPointDic.Count(temp => (temp.Key.Key == node.ID && temp.Key.Value == target.ID) || (temp.Key.Value == node.ID && temp.Key.Key == target.ID)) == 0)
                    {
                        KeyValuePair<int, int> relationShip = new KeyValuePair<int, int>(node.ID, target.ID);
                        //开始位置
                        float startX = node.Value.X_Editor + NodeWindowWidth / 2;
                        float startY = node.Value.Y_Editor + NodeWindwoHeight / 2;
                        Vector2 startVec = new Vector2(startX, startY);
                        //结束位置
                        float endX = target.Value.X_Editor + NodeWindowWidth / 2;
                        float endY = target.Value.Y_Editor + NodeWindwoHeight / 2;
                        Vector2 endVec = new Vector2(endX, endY);
                        nodeRelationShipPointDic.Add(relationShip, new Vector2[] { startVec, endVec });
                        //绘制
                        if (selectRelationLine.Key == node.ID && selectRelationLine.Value == target.ID)
                            Handles.color = Color.red;
                        else
                            Handles.color = Color.green;
                        Handles.DrawLine(new Vector3(startX, startY, 0), new Vector3(endX, endY, 0));
                    }
                }
            }
            //绘制前置组的关系(将两个组用方框框起来,选取最近的点连接)
            relationShipZonePointsDic = new Dictionary<RelationShipZone, Vector2[]>();
            RelationShipZone[] relationShipZones = taskMap.GetAllRelationShipZone();
            Func<MapElement<TaskInfoStruct>[], Color, Rect> DrawLineAndReturnRectFunc = (zoneNodes, lineColor) =>//传入组的对象获取组对象的范围
            {
                if (zoneNodes.Count() == 0)
                    return default(Rect);
                float minX = zoneNodes.Min(temp => temp.Value.X_Editor) - 5;
                float minY = zoneNodes.Min(temp => temp.Value.Y_Editor) - 5;
                float maxX = zoneNodes.Max(temp => temp.Value.X_Editor) + NodeWindowWidth + 5;
                float maxY = zoneNodes.Max(temp => temp.Value.Y_Editor) + NodeWindwoHeight + 5;
                Handles.color = lineColor;
                Handles.DrawLine(new Vector3(minX, minY), new Vector3(minX, maxY));
                Handles.DrawLine(new Vector3(minX, maxY), new Vector3(maxX, maxY));
                Handles.DrawLine(new Vector3(maxX, maxY), new Vector3(maxX, minY));
                Handles.DrawLine(new Vector3(maxX, minY), new Vector3(minX, minY));
                return new Rect(minX, minY, maxX - minX, maxY - minY);
            };
            Func<Rect, Rect, Vector2[]> DrawLineBetweenRectAndReturnVecFunc = (startZoneRect, endZoneRect) =>//传入组的范围获取组的连接线点
            {
                //计算x轴的最近边
                float startX = startZoneRect.xMin;
                float endX = endZoneRect.xMin;
                CheckMinIntervalAndSet(startZoneRect.xMin, endZoneRect.xMax, ref startX, ref endX);
                CheckMinIntervalAndSet(startZoneRect.xMax, endZoneRect.xMax, ref startX, ref endX);
                CheckMinIntervalAndSet(startZoneRect.xMax, endZoneRect.xMin, ref startX, ref endX);
                //计算y轴的最近边
                float startY = startZoneRect.yMin;
                float endY = endZoneRect.yMin;
                CheckMinIntervalAndSet(startZoneRect.yMin, endZoneRect.yMax, ref startY, ref endY);
                CheckMinIntervalAndSet(startZoneRect.yMax, endZoneRect.yMax, ref startY, ref endY);
                CheckMinIntervalAndSet(startZoneRect.yMax, endZoneRect.yMin, ref startY, ref endY);
                return new Vector2[] { new Vector2(startX, startY), new Vector2(endX, endY) };
            };
            foreach (RelationShipZone relationShipZone in relationShipZones)
            {
                MapElement<TaskInfoStruct>[] startZoneNodes = relationShipZone.StartIDList.Select(temp => nodes.FirstOrDefault(inner => inner.ID == temp)).Where(temp => temp != null).ToArray();
                MapElement<TaskInfoStruct>[] endZoneNodes = relationShipZone.EndIDList.Select(temp => nodes.FirstOrDefault(inner => inner.ID == temp)).Where(temp => temp != null).ToArray();
                Rect startZoneRect = DrawLineAndReturnRectFunc(startZoneNodes, relationShipZone.EditorColor);
                Rect endZoneRect = DrawLineAndReturnRectFunc(endZoneNodes, relationShipZone.EditorColor);
                if (startZoneRect == default(Rect) || endZoneRect == default(Rect))
                    continue;
                Vector2[] relationShipZoneLinePoints = DrawLineBetweenRectAndReturnVecFunc(startZoneRect, endZoneRect);
                if (selectRelationShipZone == relationShipZone)
                    Handles.color = Color.red;
                else
                    Handles.color = Color.green;
                Handles.DrawLine(relationShipZoneLinePoints[0], relationShipZoneLinePoints[1]);
                relationShipZonePointsDic.Add(relationShipZone, relationShipZoneLinePoints);
            }
            #endregion

            #region 绘制图节点关系的附加条件(互斥,前置,互斥组,前置组)
            //绘制特殊的关系互斥则在线中间画一个X 如果是前后置,则绘制一个箭头前置指向后置
            //绘制非前置组的类型
            NodeRelationShip[] allNodeRelationShips = taskMap.GetAllNodeRelationShip();
            foreach (NodeRelationShip nodeRelationShip in allNodeRelationShips)
            {
                KeyValuePair<KeyValuePair<int, int>, Vector2[]> tempReationShipNodeValue = nodeRelationShipPointDic.FirstOrDefault(temp =>
                  (temp.Key.Key == nodeRelationShip.StartID && temp.Key.Value == nodeRelationShip.EndID) ||
                  (temp.Key.Value == nodeRelationShip.StartID && temp.Key.Key == nodeRelationShip.EndID));
                if (tempReationShipNodeValue.Value != null)//可以得到该关系的当前显示坐标
                {
                    //通过向量计算法向量
                    Vector2 lineVec = (tempReationShipNodeValue.Value[1] - tempReationShipNodeValue.Value[0]).normalized;//start指向end
                    Vector2 lineNormalVec = GetLineNormalVec(lineVec);//将上面的计算抽象到方法
                    Vector2 centerVec = (tempReationShipNodeValue.Value[1] + tempReationShipNodeValue.Value[0]) / 2;
                    Handles.color = Color.yellow;
                    int lineLength = 7;
                    switch (nodeRelationShip.RelationShip)
                    {
                        case EnumNodeRelationShip.Mutex://互斥
                            {
                                Vector2 point1 = (lineVec + lineNormalVec) * lineLength + centerVec;
                                Vector2 point2 = -(lineVec + lineNormalVec) * lineLength + centerVec;
                                Handles.DrawLine(point1, point2);
                                Vector2 point3 = (lineVec - lineNormalVec) * lineLength + centerVec;
                                Vector2 point4 = -(lineVec - lineNormalVec) * lineLength + centerVec;
                                Handles.DrawLine(point3, point4);
                            }
                            break;
                        case EnumNodeRelationShip.SingleExclusion://前置失败
                            {
                                Vector2 point1 = centerVec;
                                Vector2 point2 = centerVec;
                                Vector2 point3 = centerVec;
                                Vector2 point4 = centerVec;
                                if (nodeRelationShip.StartID == tempReationShipNodeValue.Key.Key)//指向end 偏向start
                                {
                                    point1 += (-lineVec + lineNormalVec) * lineLength;
                                    point2 += (-lineVec - lineNormalVec) * lineLength;

                                }
                                else//指向start 偏向end
                                {
                                    point1 += (lineVec + lineNormalVec) * lineLength;
                                    point2 += (lineVec - lineNormalVec) * lineLength;
                                }
                                point3 += lineNormalVec * lineLength;
                                point4 -= lineNormalVec * lineLength;
                                Handles.DrawLine(point1, centerVec);
                                Handles.DrawLine(point2, centerVec);
                                Handles.DrawLine(point3, point4);
                            }
                            break;
                        case EnumNodeRelationShip.Predecessor://前置成功
                            {
                                Vector2 point1 = centerVec;
                                Vector2 point2 = centerVec;
                                if (nodeRelationShip.StartID == tempReationShipNodeValue.Key.Key)//指向end 偏向start
                                {
                                    point1 += (-lineVec + lineNormalVec) * lineLength;
                                    point2 += (-lineVec - lineNormalVec) * lineLength;

                                }
                                else//指向start 偏向end
                                {
                                    point1 += (lineVec + lineNormalVec) * lineLength;
                                    point2 += (lineVec - lineNormalVec) * lineLength;
                                }
                                Handles.DrawLine(point1, centerVec);
                                Handles.DrawLine(point2, centerVec);
                            }
                            break;
                    }
                }
            }
            //绘制前置组的类型
            RelationShipZone[] allRelationShipZones = taskMap.GetAllRelationShipZone();
            foreach (RelationShipZone relationShipZone in allRelationShipZones)
            {
                KeyValuePair<RelationShipZone, Vector2[]> relationShipZonePoint = relationShipZonePointsDic.FirstOrDefault(temp => temp.Key == relationShipZone);
                if (relationShipZonePoint.Key != null && relationShipZonePoint.Value != null)//可以得到该关系组的当前连接坐标
                {
                    //通过向量计算法向量
                    Vector2 lineVec = (relationShipZonePoint.Value[1] - relationShipZonePoint.Value[0]).normalized;//start指向end
                    Vector2 lineNormalVec = GetLineNormalVec(lineVec);
                    Vector2 centerVec = (relationShipZonePoint.Value[1] + relationShipZonePoint.Value[0]) / 2;
                    Handles.color = Color.yellow;
                    int lineLength = 7;
                    switch (relationShipZone.RelationShip)
                    {
                        case EnumNodeRelationShip.Mutex://互斥
                            {
                                Vector2 point1 = (lineVec + lineNormalVec) * lineLength + centerVec;
                                Vector2 point2 = -(lineVec + lineNormalVec) * lineLength + centerVec;
                                Handles.DrawLine(point1, point2);
                                Vector2 point3 = (lineVec - lineNormalVec) * lineLength + centerVec;
                                Vector2 point4 = -(lineVec - lineNormalVec) * lineLength + centerVec;
                                Handles.DrawLine(point3, point4);
                            }
                            break;
                        case EnumNodeRelationShip.SingleExclusion://前置失败
                            {
                                Vector2 point1 = centerVec;
                                Vector2 point2 = centerVec;
                                Vector2 point3 = centerVec;
                                Vector2 point4 = centerVec;
                                point1 += (-lineVec + lineNormalVec) * lineLength;
                                point2 += (-lineVec - lineNormalVec) * lineLength;
                                point3 += lineNormalVec * lineLength;
                                point4 -= lineNormalVec * lineLength;
                                Handles.DrawLine(point1, centerVec);
                                Handles.DrawLine(point2, centerVec);
                                Handles.DrawLine(point3, point4);
                            }
                            break;
                        case EnumNodeRelationShip.Predecessor://前置完成
                            {
                                Vector2 point1 = centerVec;
                                Vector2 point2 = centerVec;
                                point1 += (-lineVec + lineNormalVec) * lineLength;
                                point2 += (-lineVec - lineNormalVec) * lineLength;
                                Handles.DrawLine(point1, centerVec);
                                Handles.DrawLine(point2, centerVec);
                            }
                            break;
                    }
                }
            }
            #endregion

            //用于检测拦截鼠标事件的对象
            Event e = Event.current;

            #region 绘制框选区域
            Rect thisSelectRect = default(Rect);//本次的框选区域
            if (e.rawType == EventType.mouseDown && e.button == 0)//鼠标左键按下
            {
                SelectRect = default(Rect);
                if (nowSelectNode == null)
                {
                    SelectRect.xMin = e.mousePosition.x;
                    SelectRect.yMin = e.mousePosition.y;
                }
            }
            else if (e.rawType == EventType.mouseUp && e.button == 0)//鼠标抬起
            {
                if (nowSelectNode == null)
                {
                    thisSelectRect = SelectRect;
                    SelectRect = default(Rect);
                }
            }
            else if (SelectRect != default(Rect) && nowSelectNode == null)
            {
                SelectRect.xMax = e.mousePosition.x;
                SelectRect.yMax = e.mousePosition.y;
                Handles.color = Color.gray;
                Handles.DrawLines(new Vector3[]
                {
                    new Vector3(SelectRect.xMin, SelectRect.yMin),
                    new Vector3(SelectRect.xMax, SelectRect.yMin),

                    new Vector3(SelectRect.xMax, SelectRect.yMin),
                    new Vector3(SelectRect.xMax, SelectRect.yMax),

                    new Vector3(SelectRect.xMax, SelectRect.yMax),
                    new Vector3(SelectRect.xMin, SelectRect.yMax),

                    new Vector3(SelectRect.xMin, SelectRect.yMax),
                    new Vector3(SelectRect.xMin, SelectRect.yMin),
                });
            }
            #endregion

            #region 绘制建立图节点关系过程的连线与设置图节点的关系
            //绘制右键箭头
            if (e.control == false)//按住control键有其他用途,因此如果按住control则不可以进行操作并且清除之前内部的选择
            {
                if (rightSelectNode == null)
                {
                    if (e.rawType == EventType.mouseDown && e.button == 1)//判断如果是按下右键看鼠标的位置上是否有目标
                    {
                        //选取最后面的一个目标
                        MapElement<TaskInfoStruct> selectNode = nodes.LastOrDefault(temp =>
                            (temp.Value.X_Editor + NodeWindowWidth > e.mousePosition.x) &&
                            (temp.Value.X_Editor < e.mousePosition.x) &&
                            (temp.Value.Y_Editor + NodeWindwoHeight > e.mousePosition.y) &&
                            (temp.Value.Y_Editor < e.mousePosition.y));
                        if (selectNode != null)
                            rightSelectNode = selectNode;
                    }
                }
                else
                {
                    //绘制线
                    Handles.DrawLine(new Vector3(rightSelectNode.Value.X_Editor + NodeWindowWidth / 2, rightSelectNode.Value.Y_Editor + NodeWindwoHeight / 2, 0), new Vector3(e.mousePosition.x, e.mousePosition.y, 0));
                    if (e.rawType == EventType.mouseUp && e.button == 1)//松开右键则判断当前鼠标所在位置
                    {
                        //选取最后面的一个目标
                        MapElement<TaskInfoStruct> selectNode = nodes.LastOrDefault(temp =>
                            (temp.Value.X_Editor + NodeWindowWidth > e.mousePosition.x) &&
                            (temp.Value.X_Editor < e.mousePosition.x) &&
                            (temp.Value.Y_Editor + NodeWindwoHeight > e.mousePosition.y) &&
                            (temp.Value.Y_Editor < e.mousePosition.y));
                        if (selectNode != null && selectNode != rightSelectNode)
                        {
                            //设置关系
                            selectNode.CorrelativesNode.Add(rightSelectNode);
                        }
                        rightSelectNode = null;
                    }
                }
            }
            else
            {
                rightSelectNode = null;
            }
            #endregion

            #region  绘制节点
            BeginWindows();
            foreach (MapElement<TaskInfoStruct> node in nodes)
            {
                Rect newPosition = GUI.Window(node.ID, new Rect(node.Value.X_Editor, node.Value.Y_Editor, NodeWindowWidth, NodeWindwoHeight), NodeWindowFunction, node.Name + "[ID:" + node.ID + "][Deep:" + node.Deep + "]");
                node.Value.X_Editor = newPosition.x;
                node.Value.Y_Editor = newPosition.y;
            }
            EndWindows();
            #endregion

            #region 按esc键移除当前框选id 
            if (e.keyCode == KeyCode.Escape)
            {
                controlSelectIDs = null;
            }
            #endregion

            #region 鼠标的左右键点击事件,点击节点显示详细信息,点击连线会选中连线,再次右击选中连线可以设置该连线
            if (e.control == false)//按住control键有其他用途,因此如果按住control则不可以进行操作并且清除之前内部的选择
            {   //选中节点
                if ((e.button == 0 || e.button == 1) && e.rawType == EventType.mouseUp)
                {
                    if (e.button == 0)//左键点击
                    {
                        //选取最后面的一个目标
                        MapElement<TaskInfoStruct> selectNode = nodes.LastOrDefault(temp =>
                            (temp.Value.X_Editor + NodeWindowWidth > e.mousePosition.x) &&
                            (temp.Value.X_Editor < e.mousePosition.x) &&
                            (temp.Value.Y_Editor + NodeWindwoHeight > e.mousePosition.y) &&
                            (temp.Value.Y_Editor < e.mousePosition.y));
                        nowSelectNode = selectNode;
                        ShowExplan();
                        if (nowSelectNode == null)//判断是否选中了线(判断是否选中了节点,如果选中节点,则肯定不可能点中线的)
                        {
                            #region 节点关系的线
                            {
                                float mouseX = e.mousePosition.x;//x0
                                float mouseY = e.mousePosition.y;//y0
                                Vector2 tempVec = new Vector2(mouseX, mouseY);
                                var v = nodeRelationShipPointDic.Select(temp => new
                                {
                                    start = temp.Value[0],
                                    end = temp.Value[1],
                                    KVP = temp.Key
                                }).Select(temp => new
                                {
                                    dis = GetDistanceToLine(temp.start, temp.end, tempVec),
                                    KVP = temp.KVP
                                }).OrderBy(temp => temp.dis);

                                if (v.Count() > 0 && v.FirstOrDefault().dis < 5)//存在线并且点到线的距离小于5
                                {
                                    selectRelationLine = v.FirstOrDefault().KVP;
                                }
                                else
                                {
                                    selectRelationLine = new KeyValuePair<int, int>(0, 0);
                                }
                            }
                            #endregion

                            #region 节点组关系的线
                            {
                                float mouseX = e.mousePosition.x;
                                float mouseY = e.mousePosition.y;
                                Vector2 tempVec = new Vector2(mouseX, mouseY);
                                var v = relationShipZonePointsDic.Select(temp => new
                                {
                                    start = temp.Value[0],
                                    end = temp.Value[1],
                                    zone = temp.Key
                                }).Select(temp => new
                                {
                                    dis = GetDistanceToLine(temp.start, temp.end, tempVec),
                                    zone = temp.zone
                                }).OrderBy(temp => temp.dis);
                                if (v.Count() > 0 && v.FirstOrDefault().dis < 5)//存在线并且点到线的距离小于5
                                {
                                    selectRelationShipZone = v.FirstOrDefault().zone;
                                }
                                else
                                {
                                    selectRelationShipZone = null;
                                }
                            }
                            #endregion
                        }
                    }
                    else if (e.button == 1 && !(selectRelationLine.Key == 0 && selectRelationLine.Value == 0))//右键点击并且存在选择的线
                    {
                        KeyValuePair<KeyValuePair<int, int>, Vector2[]> var = nodeRelationShipPointDic.FirstOrDefault(temp => (
                            temp.Key.Key == selectRelationLine.Key && temp.Key.Value == selectRelationLine.Value) ||
                            (temp.Key.Value == selectRelationLine.Key && temp.Key.Key == selectRelationLine.Value));
                        if (!(var.Key.Key == 0 && var.Key.Value == 0))//存在该数据
                        {
                            float dis = GetDistanceToLine(var.Value[0], var.Value[1], e.mousePosition);
                            if (dis < 5)//右键点击了该线
                            {
                                //设置连线两个节点之间的关系
                                TaskEditor_NodeRelationShip taskEditor_nodeRelationShip = EditorWindow.GetWindow<TaskEditor_NodeRelationShip>();
                                taskEditor_nodeRelationShip.taskMap = taskMap;
                                taskEditor_nodeRelationShip.KVP = var.Key;
                                taskEditor_nodeRelationShip.Show();
                            }

                        }
                    }
                    else if (e.button == 1 && selectRelationShipZone != null)//右键点击并且存在节点组选择的线
                    {
                        Vector2[] zonePoints = relationShipZonePointsDic[selectRelationShipZone];
                        if (zonePoints != null && zonePoints.Length == 2)
                        {
                            float dis = GetDistanceToLine(zonePoints[0], zonePoints[1], e.mousePosition);
                            if (dis < 5)//右键点击了该线
                            {
                                //设置连接两组之间的关系
                                TaskEditor_RelationShipZone taskEditor_RelationShipZone = EditorWindow.GetWindow<TaskEditor_RelationShipZone>();
                                taskEditor_RelationShipZone.taskMap = taskMap;
                                taskEditor_RelationShipZone.zone = selectRelationShipZone;
                                taskEditor_RelationShipZone.Show();
                            }
                        }
                    }
                }
            }
            else
            {
                selectRelationLine = default(KeyValuePair<int, int>);
                //绘制已经框选的id
                if (controlSelectIDs != null && controlSelectIDs.Length > 0)
                {
                    MapElement<TaskInfoStruct>[] selectZone = controlSelectIDs.Select(temp => nodes.FirstOrDefault(inner => inner.ID == temp)).Where(temp => temp != null).ToArray();
                    DrawLineAndReturnRectFunc(selectZone, Color.yellow);
                }
                //查看框选
                if (thisSelectRect != default(Rect))
                {
                    //计算本次的框选id
                    int[] thisControlSelectIDs = nodes.Where(temp => temp.Value.X_Editor > thisSelectRect.xMin && temp.Value.X_Editor + NodeWindowWidth < thisSelectRect.xMax && temp.Value.Y_Editor > thisSelectRect.yMin && temp.Value.Y_Editor + NodeWindwoHeight < thisSelectRect.yMax).Select(temp => temp.ID).ToArray();
                    if (thisControlSelectIDs.Length > 0)
                        if (controlSelectIDs == null)
                        {
                            controlSelectIDs = thisControlSelectIDs;
                        }
                        else
                        {
                            if (thisControlSelectIDs.Intersect(controlSelectIDs).Count() == 0)//如果两个序列没有重叠则可以建立组联系
                            {
                                if (taskMap.GetRelationShipZone(thisControlSelectIDs, controlSelectIDs) == null)
                                {
                                    RelationShipZone relationShipZone;
                                    taskMap.CreateRelationShipZone(controlSelectIDs.ToList(), thisControlSelectIDs.ToList(), out relationShipZone);
                                    controlSelectIDs = null;
                                }
                            }
                        }
                }
            }
            #endregion
            GUI.EndScrollView();
        }

        /// <summary>
        /// 获取向量的法向量
        /// </summary>
        /// <param name="lineVec">向量的单位向量</param>
        /// <returns></returns>
        private Vector2 GetLineNormalVec(Vector2 lineVec)
        {
            lineVec.Normalize();
            Vector3 tempVec = new Vector3(1, 1, 0);
            if (Vector3.Dot(lineVec, tempVec) == 0)
                tempVec = new Vector3(-1, 1, 0);
            Vector3 tempNormal = Vector3.Cross(tempVec, lineVec);
            Vector3 tempNormal_1 = Vector3.Cross(tempNormal, lineVec);
            Vector2 lineNormalVec = new Vector2(tempNormal_1.x, tempNormal_1.y).normalized;
            return lineNormalVec;
        }

        /// <summary>
        /// 检测差值并设置
        /// </summary>
        /// <param name="newStart">新的开始</param>
        /// <param name="newEnd">新的结束</param>
        /// <param name="nowStart">当前的开始</param>
        /// <param name="nowEnd">当前的结束</param>
        private void CheckMinIntervalAndSet(float newStart, float newEnd, ref float nowStart, ref float nowEnd)
        {
            float nowInterval = Mathf.Abs(nowStart - nowEnd);
            float newInterval = Mathf.Abs(newStart - newEnd);
            if (newInterval < nowInterval)
            {
                nowStart = newStart;
                nowEnd = newEnd;
            }
        }

        /// <summary>
        /// 计算点到线段的距离
        /// </summary>
        /// <param name="startA">线段起点</param>
        /// <param name="endB">线段终点</param>
        /// <param name="pointC">点</param>
        /// <returns></returns>
        private float GetDistanceToLine(Vector2 startA, Vector2 endB, Vector2 pointC)
        {
            Vector2 ab = endB - startA;
            Vector2 ac = pointC - startA;
            float f = Vector2.Dot(ab, ac);
            if (f < 0)
                return Vector2.Distance(pointC, startA);
            float d = Vector2.Dot(ab, ab);
            if (f > d)
                return Vector2.Distance(pointC, endB);
            f = f / d;
            Vector2 D = startA + f * ab;
            return Vector2.Distance(pointC, D);
        }

        /// <summary>
        /// 窗体中显示对应id的节点
        /// </summary>
        /// <param name="id"></param>
        private void NodeWindowFunction(int id)
        {
            GUI.DragWindow(new Rect(0, 0, NodeWindowWidth, 20));
            MapElement<TaskInfoStruct> thisTaskNode = taskMap.GetElement(id);
            if (thisTaskNode != null)
            {
                EditorGUI.LabelField(new Rect(0, 20, NodeWindowWidth, NodeWindwoHeight - 20), thisTaskNode.Value.TaskTitile != null ? thisTaskNode.Value.TaskTitile : "");
            }
        }

        private void ShowExplan()
        {
            if (taskEditor_Explan != null)
                taskEditor_Explan.Close();
            if (nowSelectNode != null && showTaskEditorExplan)
            {
                taskEditor_Explan = EditorWindow.GetWindow<TaskEditor_Explan>();
                taskEditor_Explan.Show();
            }
        }

        private void OnDestroy()
        {
            if (taskEditor_Explan != null)
                taskEditor_Explan.Close();
        }


    }

    /// <summary>
    /// 任务的详细设置面板
    /// </summary>
    public class TaskEditor_Explan : EditorWindow
    {
        /// <summary>
        /// 当前选择的节点
        /// </summary>
        public MapElement<TaskInfoStruct> nowSelectNode;

        /// <summary>
        /// 任务图
        /// </summary>
        public TaskMap taskMap;

        private void Awake()
        {
            TaskTypeKeyValueList = new List<KeyValuePair<Enums.EnumTaskType, string>>();
            SetEnumExplanDic(TaskTypeKeyValueList);
            CharacterTendencyKeyValueList = new List<KeyValuePair<Enums.CharacterTendency, string>>();
            SetEnumExplanDic(CharacterTendencyKeyValueList);
            GoodsTypeKeyValueList = new List<KeyValuePair<EnumGoodsType, string>>();
            SetEnumExplanDic(GoodsTypeKeyValueList,
                (goodsType) =>
                {
                    int goodsID = (int)goodsType;
                    if (goodsID % 1000 == 0)//必须是具体的类型
                        return false;
                    return true;
                });
            MonsterTypeKeyValueList = new List<KeyValuePair<EnumMonsterType, string>>();
            SetEnumExplanDic(MonsterTypeKeyValueList);
            TaskEventKeyValueList = new List<KeyValuePair<Enums.EnumTaskEventType, string>>();
            SetEnumExplanDic(TaskEventKeyValueList);
            TaskProgressKeyValueList = new List<KeyValuePair<Enums.EnumTaskProgress, string>>();
            SetEnumExplanDic(TaskProgressKeyValueList);
            TaskSpecialCheckKeyValueList = new List<KeyValuePair<Enums.EnumTaskSpecialCheck, string>>();
            SetEnumExplanDic(TaskSpecialCheckKeyValueList);
            taskEventDataToTypeDic = new Dictionary<Enums.EnumTaskEventType, TargetTypeExplanAttribute.EnumTargetType>();
            TargetTypeExplanAttribute.SetEnumExplanDic(taskEventDataToTypeDic);

        }

        TaskEditor_Explan()
        {
            this.titleContent = new GUIContent("节点数据设置");
        }

        /// <summary>
        /// 设置枚举说明到键值对集合中
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="targetList"></param>
        /// <param name="CallBackCheck"></param>
        private void SetEnumExplanDic<T>(List<KeyValuePair<T, string>> targetList, Func<T, bool> CallBackCheck = null)
        {
            Type EnumType = typeof(T);
            if (EnumType.IsEnum)
            {
                IEnumerable<T> enumTaskTypes = Enum.GetValues(typeof(T)).OfType<T>();
                foreach (T enumTarget in enumTaskTypes)
                {
                    FieldInfo fieldInfo = EnumType.GetField(enumTarget.ToString());
                    if (fieldInfo != null)
                    {
                        FieldExplanAttribute fieldExplan = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                        if (fieldExplan != null)
                        {
                            if (CallBackCheck == null || CallBackCheck(enumTarget))
                            {
                                targetList.Add(new KeyValuePair<T, string>(enumTarget, fieldExplan.GetExplan()));
                            }
                        }
                    }
                }
            }
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        /// <summary>
        /// 任务类型和该枚举的描述键值对的集合
        /// </summary>
        List<KeyValuePair<Enums.EnumTaskType, string>> TaskTypeKeyValueList;
        /// <summary>
        /// 性格倾向和该枚举的描述键值对集合
        /// </summary>
        List<KeyValuePair<Enums.CharacterTendency, string>> CharacterTendencyKeyValueList;
        /// <summary>
        /// 物品和该枚举的描述键值对集合
        /// </summary>
        List<KeyValuePair<EnumGoodsType, string>> GoodsTypeKeyValueList;
        /// <summary>
        /// 怪物和该枚举的描述键值对集合
        /// </summary>
        List<KeyValuePair<EnumMonsterType, string>> MonsterTypeKeyValueList;
        /// <summary>
        /// 任务事件与该枚举的描述键值对集合
        /// </summary>
        List<KeyValuePair<Enums.EnumTaskEventType, string>> TaskEventKeyValueList;
        /// <summary>
        /// 任务进度与该枚举的描述键值对集合
        /// </summary>
        List<KeyValuePair<Enums.EnumTaskProgress, string>> TaskProgressKeyValueList;
        /// <summary>
        /// 特殊检测状态与该枚举描述键值对集合
        /// </summary>
        List<KeyValuePair<Enums.EnumTaskSpecialCheck, string>> TaskSpecialCheckKeyValueList;
        /// <summary>
        /// 任务事件类型对应附加数据类型的字典
        /// </summary>
        Dictionary<Enums.EnumTaskEventType, TargetTypeExplanAttribute.EnumTargetType> taskEventDataToTypeDic;

        /// <summary>
        /// 临时的显示需要添加的奖励物品类型
        /// </summary>
        EnumGoodsType tempAddGetGoodsType;

        /// <summary>
        /// 临时的显示需要添加的任务需求物品类型
        /// </summary>
        EnumGoodsType tempAddNeedGoodsType;

        /// <summary>
        /// 临时的显示需要添加的任务需求杀死怪物类型
        /// </summary>
        EnumMonsterType tempAddNeedMonsterType;

        /// <summary>
        /// 临时的显示需要添加任务状态(任务事件所需)
        /// </summary>
        Enums.EnumTaskProgress tempAddTaskProgressOfEvent;

        /// <summary>
        /// 获取值在数组的下标
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int Index(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 滑动值
        /// </summary>
        Vector2 scrollValue;

        private void OnGUI()
        {
            if (nowSelectNode == null)
                return;
            EditorGUILayout.BeginVertical();
            scrollValue = EditorGUILayout.BeginScrollView(scrollValue);

            #region name与id 节点相关的信息
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID:", nowSelectNode.ID.ToString());
            //如果当前节点不是入口节点,则显示询问是否设置为入口的按钮
            if (taskMap.GetFirstElement() != nowSelectNode && GUILayout.Button("设为入口", GUILayout.Width(55)))
            {
                if (EditorUtility.DisplayDialog("提示!", "是否设置该节点为入口节点?", "是", "否"))
                {
                    if (taskMap != null)
                        taskMap.SetFirstElement(nowSelectNode);
                }
            }
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Name = EditorGUILayout.TextField("Name:", nowSelectNode.Name);
            nowSelectNode.Deep = EditorGUILayout.IntField("Deep:", nowSelectNode.Deep);
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region 任务的基础信息
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------基础设置--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Value.TaskTitile = EditorGUILayout.TextField("任务标题:", nowSelectNode.Value.TaskTitile);
            EditorGUILayout.LabelField("任务说明:");
            nowSelectNode.Value.TaskExplain = EditorGUILayout.TextArea(nowSelectNode.Value.TaskExplain != null ? nowSelectNode.Value.TaskExplain : "", GUILayout.Height(60));
            int[] taskTypeValues = TaskTypeKeyValueList.Select(temp => (int)temp.Key).ToArray();
            string[] taskTypeExplans = TaskTypeKeyValueList.Select(temp => temp.Value).ToArray();
            int taskTypeIndex = Index(taskTypeValues, (int)nowSelectNode.Value.TaskType);
            taskTypeIndex = EditorGUILayout.Popup("任务类型:", taskTypeIndex, taskTypeExplans);
            if (taskTypeIndex >= 0)
                nowSelectNode.Value.TaskType = (Enums.EnumTaskType)taskTypeValues[taskTypeIndex];
            else
                nowSelectNode.Value.TaskType = default(Enums.EnumTaskType);
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region 任务的接取条件
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------任务接取限制--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Value.LevelLimit = EditorGUILayout.IntField("等级限制(>=):", nowSelectNode.Value.LevelLimit);
            int[] characterTendencyValues = CharacterTendencyKeyValueList.Select(temp => (int)temp.Key).ToArray();
            string[] characterTendencyExplans = CharacterTendencyKeyValueList.Select(temp => temp.Value).ToArray();
            int characterTendencyIndex = Index(characterTendencyValues, (int)nowSelectNode.Value.ChaTendency);
            characterTendencyIndex = EditorGUILayout.Popup("性格倾向:", characterTendencyIndex, characterTendencyExplans);
            if (characterTendencyIndex >= 0)
                nowSelectNode.Value.ChaTendency = (Enums.CharacterTendency)characterTendencyValues[characterTendencyIndex];
            else
                nowSelectNode.Value.ChaTendency = default(Enums.CharacterTendency);

            nowSelectNode.Value.ReceiveTaskNpcId = EditorGUILayout.IntField("接取任务NPC", nowSelectNode.Value.ReceiveTaskNpcId);
            nowSelectNode.Value.NeedReputation = EditorGUILayout.IntField("需要的声望", nowSelectNode.Value.NeedReputation);
            EditorGUILayout.BeginHorizontal();
            nowSelectNode.Value.NeedShowTalk = EditorGUILayout.Toggle(nowSelectNode.Value.NeedShowTalk, GUILayout.Width(20));
            EditorGUILayout.LabelField("直接接取任务是否要显示对话(注意如果不显示则直接接取,且与提示只能显示一种!)");
            if (nowSelectNode.Value.NeedShowTalk)
                nowSelectNode.Value.NeedShowImageTip = false;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            nowSelectNode.Value.NeedShowImageTip = EditorGUILayout.Toggle(nowSelectNode.Value.NeedShowImageTip, GUILayout.Width(20));
            EditorGUILayout.LabelField("直接接取任务是否要显示提示(注意如果不显示则直接接取,且与对话只能显示一种!)");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (nowSelectNode.Value.NeedShowImageTip)
            {
                nowSelectNode.Value.NeedShowTalk = false;
                //设置文件夹路径
                EditorGUILayout.LabelField("提示图片所在的文件夹(该文件夹的父路径为Data/Task/ImageTip):");
                nowSelectNode.Value.ShowImageTipDirectoryName = EditorGUILayout.TextField(nowSelectNode.Value.ShowImageTipDirectoryName);
            }
            EditorGUILayout.EndHorizontal();
            if (nowSelectNode.Value.ReceiveTaskLocation != null)
            {
                EditorGUILayout.LabelField("接取任务地点(场景、中心点、半径):");
                nowSelectNode.Value.ReceiveTaskLocation.SceneName = EditorGUILayout.TextField("场景:", nowSelectNode.Value.ReceiveTaskLocation.SceneName);
                nowSelectNode.Value.ReceiveTaskLocation.ArrivedCenterPos = EditorGUILayout.Vector3Field("中心点:", nowSelectNode.Value.ReceiveTaskLocation.ArrivedCenterPos);
                nowSelectNode.Value.ReceiveTaskLocation.Radius = EditorGUILayout.IntField("半径:", nowSelectNode.Value.ReceiveTaskLocation.Radius);
                if (GUILayout.Button("移除接取任务地点"))
                {
                    if (EditorUtility.DisplayDialog("请再次确认!", "是否要移除接取任务地点", "是", "否"))
                    {
                        nowSelectNode.Value.ReceiveTaskLocation = null;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("创建接取任务地点"))
                {
                    nowSelectNode.Value.ReceiveTaskLocation = new Enums.TaskLocation();
                }
            }
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region 任务的交付方式
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------交付方式--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Value.DeliveryTaskNpcId = EditorGUILayout.IntField("交付任务NPC", nowSelectNode.Value.DeliveryTaskNpcId);
            if (nowSelectNode.Value.DeliveryTaskLocation != null)
            {
                EditorGUILayout.LabelField("交付任务地点(场景、中心点、半径)");
                nowSelectNode.Value.DeliveryTaskLocation.SceneName = EditorGUILayout.TextField("场景:", nowSelectNode.Value.DeliveryTaskLocation.SceneName);
                nowSelectNode.Value.DeliveryTaskLocation.ArrivedCenterPos = EditorGUILayout.Vector3Field("中心点:", nowSelectNode.Value.DeliveryTaskLocation.ArrivedCenterPos);
                nowSelectNode.Value.DeliveryTaskLocation.Radius = EditorGUILayout.IntField("半径:", nowSelectNode.Value.DeliveryTaskLocation.Radius);
                if (GUILayout.Button("移除交付任务地点"))
                {
                    if (EditorUtility.DisplayDialog("请再次确认!", "是否要移除交付任务地点", "是", "否"))
                    {
                        nowSelectNode.Value.DeliveryTaskLocation = null;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("创建交付任务地点"))
                {
                    nowSelectNode.Value.DeliveryTaskLocation = new Enums.TaskLocation();
                }
            }
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region 任务的达成条件
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------任务达成条件--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Value.NeedGetReputation = EditorGUILayout.IntField("达到该声望:", nowSelectNode.Value.NeedGetReputation);
            nowSelectNode.Value.TimeLimit = EditorGUILayout.IntField("经过指定游戏时间:", nowSelectNode.Value.TimeLimit);
           List< Enums.EnumTaskSpecialCheck> taskSpecialCheckValues = TaskSpecialCheckKeyValueList.Select(temp => temp.Key).ToList();
            string[] taskSpecialCheckShows = TaskSpecialCheckKeyValueList.Select(temp => temp.Value).ToArray();
            int taskSpecialCheckIndex = taskSpecialCheckValues.IndexOf(nowSelectNode.Value.NeedSpecialCheck);
            taskSpecialCheckIndex = EditorGUILayout.Popup("特殊条件检测:", taskSpecialCheckIndex, taskSpecialCheckShows);
            if (taskSpecialCheckIndex >= 0)
            {
                nowSelectNode.Value.NeedSpecialCheck = taskSpecialCheckValues[taskSpecialCheckIndex];
            }
            /*需要获取物品的数量*/
            EditorGUILayout.BeginHorizontal();
            if (nowSelectNode.Value.NeedGetGoodsCount == null)
                nowSelectNode.Value.NeedGetGoodsCount = new Dictionary<EnumGoodsType, int>();
            EditorGUILayout.LabelField("需要获取的物品的数量");
            //找出一个可添加的物品(任务需求)
            KeyValuePair<EnumGoodsType, string>[] canAddNeedGoodes = GoodsTypeKeyValueList.Where(temp => !nowSelectNode.Value.NeedGetGoodsCount.ContainsKey(temp.Key)).ToArray();
            int[] canAddNeedGoodsValues = canAddNeedGoodes.Select(temp => (int)temp.Key).ToArray();
            string[] canAddNeedGoodsExplans = canAddNeedGoodes.Select(temp => temp.Value).ToArray();
            int canAddNeedGoodsIndex = Index(canAddNeedGoodsValues, (int)tempAddNeedGoodsType);
            canAddNeedGoodsIndex = EditorGUILayout.Popup(canAddNeedGoodsIndex, canAddNeedGoodsExplans);
            if (canAddNeedGoodsIndex >= 0)
                tempAddNeedGoodsType = (EnumGoodsType)canAddNeedGoodsValues[canAddNeedGoodsIndex];
            else
                tempAddNeedGoodsType = GoodsTypeKeyValueList.Select(temp => temp.Key).FirstOrDefault();
            if (GUILayout.Button("+"))
            {
                //添加一个类型
                if (GoodsTypeKeyValueList.Count(temp => temp.Key == tempAddNeedGoodsType) > 0 && !nowSelectNode.Value.NeedGetGoodsCount.ContainsKey(tempAddNeedGoodsType))
                {
                    nowSelectNode.Value.NeedGetGoodsCount.Add(tempAddNeedGoodsType, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
            KeyValuePair<EnumGoodsType, int>[] needGetGoodsArray = nowSelectNode.Value.NeedGetGoodsCount.ToArray();
            foreach (KeyValuePair<EnumGoodsType, int> needGetGoods in needGetGoodsArray)
            {
                KeyValuePair<EnumGoodsType, string> goodsExplan = GoodsTypeKeyValueList.FirstOrDefault(temp => temp.Key == needGetGoods.Key);
                if (goodsExplan.Key == needGetGoods.Key)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示!", "是否移除该任务需求的物品", "是", "否"))
                        {
                            nowSelectNode.Value.NeedGetGoodsCount.Remove(needGetGoods.Key);
                            continue;
                        }
                    }
                    int goodsCount = EditorGUILayout.IntField(goodsExplan.Value, needGetGoods.Value);
                    if (nowSelectNode.Value.NeedGetGoodsCount.ContainsKey(needGetGoods.Key))
                        nowSelectNode.Value.NeedGetGoodsCount[needGetGoods.Key] = goodsCount;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.Space();
            /*需要杀死怪物的数量*/
            EditorGUILayout.BeginHorizontal();
            if (nowSelectNode.Value.NeedKillMonsterCount == null)
                nowSelectNode.Value.NeedKillMonsterCount = new Dictionary<EnumMonsterType, int>();
            EditorGUILayout.LabelField("需要杀死怪物的数量");
            //找出一个可添加的怪物(任务需求)
            KeyValuePair<EnumMonsterType, string>[] canAddNeedMonsters = MonsterTypeKeyValueList.Where(temp => !nowSelectNode.Value.NeedKillMonsterCount.ContainsKey(temp.Key)).ToArray();
            int[] canAddNeedMonsterValues = canAddNeedMonsters.Select(temp => (int)temp.Key).ToArray();
            string[] canAddNeedMonsterExplans = canAddNeedMonsters.Select(temp => temp.Value).ToArray();
            int canAddNeedMonsterIndex = Index(canAddNeedMonsterValues, (int)tempAddNeedMonsterType);
            canAddNeedMonsterIndex = EditorGUILayout.Popup(canAddNeedMonsterIndex, canAddNeedMonsterExplans);
            if (canAddNeedMonsterIndex > 0)
                tempAddNeedMonsterType = (EnumMonsterType)canAddNeedMonsterValues[canAddNeedMonsterIndex];
            else
                tempAddNeedMonsterType = MonsterTypeKeyValueList.Select(temp => temp.Key).FirstOrDefault();
            if (GUILayout.Button("+"))
            {
                //添加一个类型
                if (MonsterTypeKeyValueList.Count(temp => temp.Key == tempAddNeedMonsterType) > 0 && !nowSelectNode.Value.NeedKillMonsterCount.ContainsKey(tempAddNeedMonsterType))
                {
                    nowSelectNode.Value.NeedKillMonsterCount.Add(tempAddNeedMonsterType, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
            KeyValuePair<EnumMonsterType, int>[] needKillMonsterArray = nowSelectNode.Value.NeedKillMonsterCount.ToArray();
            foreach (KeyValuePair<EnumMonsterType, int> needKillMonster in needKillMonsterArray)
            {
                KeyValuePair<EnumMonsterType, string> monsterExplan = MonsterTypeKeyValueList.FirstOrDefault(temp => temp.Key == needKillMonster.Key);
                if (needKillMonster.Key == needKillMonster.Key)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示!", "是否移除该任务需求杀死的怪物", "是", "否"))
                        {
                            nowSelectNode.Value.NeedKillMonsterCount.Remove(needKillMonster.Key);
                            continue;
                        }
                    }
                    int monsterCount = EditorGUILayout.IntField(monsterExplan.Value, needKillMonster.Value);
                    if (nowSelectNode.Value.NeedKillMonsterCount.ContainsKey(needKillMonster.Key))
                        nowSelectNode.Value.NeedKillMonsterCount[needKillMonster.Key] = monsterCount;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            #endregion

            #region 任务完成后的奖励
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------奖励--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            nowSelectNode.Value.AwardExperience = EditorGUILayout.IntField("奖励经验:", nowSelectNode.Value.AwardExperience);
            nowSelectNode.Value.AwardSkillPoint = EditorGUILayout.IntField("奖励技能点:", nowSelectNode.Value.AwardSkillPoint);
            nowSelectNode.Value.AwardReputation = EditorGUILayout.IntField("奖励声望:", nowSelectNode.Value.AwardReputation);
            EditorGUILayout.BeginHorizontal();
            if (nowSelectNode.Value.AwardGoods == null)
                nowSelectNode.Value.AwardGoods = new Dictionary<EnumGoodsType, int>();
            EditorGUILayout.LabelField("奖励物品:");
            //找出一个可以添加的物品(任务奖励)
            KeyValuePair<EnumGoodsType, string>[] canAddGetGoodses = GoodsTypeKeyValueList.Where(temp => !nowSelectNode.Value.AwardGoods.ContainsKey(temp.Key)).ToArray();//计算出可以添加的类型
            int[] canAddGetGoodsValues = canAddGetGoodses.Select(temp => (int)temp.Key).ToArray();
            string[] canAddGetGoodsExplans = canAddGetGoodses.Select(temp => temp.Value).ToArray();
            int canAddGetGoodsIndex = Index(canAddGetGoodsValues, (int)tempAddGetGoodsType);
            canAddGetGoodsIndex = EditorGUILayout.Popup(canAddGetGoodsIndex, canAddGetGoodsExplans);
            if (canAddGetGoodsIndex >= 0)
                tempAddGetGoodsType = (EnumGoodsType)canAddGetGoodsValues[canAddGetGoodsIndex];
            else
                tempAddGetGoodsType = GoodsTypeKeyValueList.Select(temp => temp.Key).FirstOrDefault();
            if (GUILayout.Button("+"))
            {
                //添加一个类型
                if (GoodsTypeKeyValueList.Count(temp => temp.Key == tempAddGetGoodsType) > 0 && !nowSelectNode.Value.AwardGoods.ContainsKey(tempAddGetGoodsType))
                {
                    nowSelectNode.Value.AwardGoods.Add(tempAddGetGoodsType, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
            KeyValuePair<EnumGoodsType, int>[] awardGoodsArray = nowSelectNode.Value.AwardGoods.ToArray();
            foreach (KeyValuePair<EnumGoodsType, int> awardGoods in awardGoodsArray)
            {
                KeyValuePair<EnumGoodsType, string> goodsExplan = GoodsTypeKeyValueList.FirstOrDefault(temp => temp.Key == awardGoods.Key);
                if (goodsExplan.Key == awardGoods.Key)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        if (EditorUtility.DisplayDialog("提示!", "是否移除该奖励物品", "是", "否"))
                        {
                            nowSelectNode.Value.AwardGoods.Remove(awardGoods.Key);
                            continue;
                        }
                    }
                    int goodsCount = EditorGUILayout.IntField(goodsExplan.Value, awardGoods.Value);
                    if (nowSelectNode.Value.AwardGoods.ContainsKey(awardGoods.Key))
                        nowSelectNode.Value.AwardGoods[awardGoods.Key] = goodsCount;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            #endregion

            #region 任务触发事件
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<---------------------任务触发事件--------------------->");
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            if (nowSelectNode.Value.TaskEventTriggerDic != null)
            {
                if (GUILayout.Button("移除任务触发事件") && EditorUtility.DisplayDialog("请再次确认!", "是否要移除所有任务事件", "确定", "取消"))
                {
                    nowSelectNode.Value.TaskEventTriggerDic = null;
                }
            }
            if (nowSelectNode.Value.TaskEventTriggerDic == null)
            {
                if (GUILayout.Button("创建任务触发事件"))
                {
                    nowSelectNode.Value.TaskEventTriggerDic = new Dictionary<Enums.EnumTaskProgress, List<TaskEventData>>();
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                List<Enums.EnumTaskProgress> taskProgressOfEventValues = TaskProgressKeyValueList.Select(temp => temp.Key).ToList();
                string[] taskTypeOfEventExplans = TaskProgressKeyValueList.Select(temp => temp.Value).ToArray();
                int taskTypeOfEventIndex = taskProgressOfEventValues.IndexOf(tempAddTaskProgressOfEvent);
                taskTypeOfEventIndex = EditorGUILayout.Popup(taskTypeOfEventIndex, taskTypeOfEventExplans);
                if (taskTypeOfEventIndex >= 0)
                {
                    tempAddTaskProgressOfEvent = taskProgressOfEventValues[taskTypeOfEventIndex];
                    if (GUILayout.Button("添加事件"))//添加一个任务事件状态
                    {
                        if (!nowSelectNode.Value.TaskEventTriggerDic.ContainsKey(tempAddTaskProgressOfEvent))
                        {
                            nowSelectNode.Value.TaskEventTriggerDic.Add(tempAddTaskProgressOfEvent, new List<TaskEventData>());
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                List<Enums.EnumTaskEventType> taskEventValues = TaskEventKeyValueList.Select(temp => temp.Key).ToList();
                string[] taskEventExplans = TaskEventKeyValueList.Select(temp => temp.Value).ToArray();
                List<Enums.EnumTaskProgress> tempRemoveList = new List<Enums.EnumTaskProgress>();
                foreach (KeyValuePair<Enums.EnumTaskProgress, List<TaskEventData>> item in nowSelectNode.Value.TaskEventTriggerDic)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("请再次确认!", "是否要移除该阶段的事件", "确定", "取消"))
                    {
                        tempRemoveList.Add(item.Key);
                    }
                    string thisTaskTypeExplan = "未找到状态";
                    int tempIndex = taskProgressOfEventValues.IndexOf(item.Key);
                    if (tempIndex >= 0)
                    {
                        thisTaskTypeExplan = taskTypeOfEventExplans[tempIndex];
                    }
                    EditorGUILayout.LabelField(thisTaskTypeExplan);
                    if (GUILayout.Button("＋", GUILayout.Width(20)))//添加一个具体的事件类型
                    {
                        item.Value.Add(new TaskEventData() { EventType = Enums.EnumTaskEventType.None, EventData = "" });
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    //便利子节点
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        if (GUILayout.Button("×", GUILayout.Width(20)) && EditorUtility.DisplayDialog("请再次确认!", "是否要移除该事件", "确定", "取消"))
                        {
                            item.Value.RemoveAt(i);
                            i--;
                            goto DeleteEvent;
                        }
                        int eventIndex = taskEventValues.IndexOf(item.Value[i].EventType);
                        eventIndex = EditorGUILayout.Popup(eventIndex, taskEventExplans);
                        if (eventIndex >= 0)
                            item.Value[i].EventType = taskEventValues[eventIndex];
                        if (!taskEventDataToTypeDic.ContainsKey(item.Value[i].EventType))
                        {
                            item.Value[i].EventType = Enums.EnumTaskEventType.None;
                        }
                        switch (taskEventDataToTypeDic[item.Value[i].EventType])
                        {
                            case TargetTypeExplanAttribute.EnumTargetType.String:
                                if (item.Value[i].EventData == null)
                                {
                                    item.Value[i].EventData = "";
                                }
                                {
                                    string tempData = item.Value[i].EventData.ToString();
                                    item.Value[i].EventData = EditorGUILayout.TextField(tempData);
                                }
                                break;
                            case TargetTypeExplanAttribute.EnumTargetType.Int:
                                if (item.Value[i].EventData == null)
                                {
                                    item.Value[i].EventData = 0;
                                }
                                {
                                    int tempData = 0;
                                    try
                                    {
                                        tempData = (int)item.Value[i].EventData;
                                    }
                                    catch { }
                                    item.Value[i].EventData = EditorGUILayout.IntField(tempData);
                                }
                                break;
                            case TargetTypeExplanAttribute.EnumTargetType.Float:
                                if (item.Value[i].EventData == null)
                                {
                                    item.Value[i].EventData = 0f;
                                }
                                {
                                    float tempData = 0f;
                                    try
                                    {
                                        tempData = (float)item.Value[i].EventData;
                                    }
                                    catch { }
                                    item.Value[i].EventData = EditorGUILayout.FloatField(tempData);
                                }
                                break;
                            case TargetTypeExplanAttribute.EnumTargetType.Bool:
                                if (item.Value[i].EventData == null)
                                {
                                    item.Value[i].EventData = false;
                                }
                                {
                                    bool tempData = false;
                                    try
                                    {
                                        tempData = (bool)item.Value[i].EventData;
                                    }
                                    catch { }
                                    item.Value[i].EventData = EditorGUILayout.Toggle(tempData);
                                }
                                break;
                            default:
                                item.Value[i].EventData = null;
                                break;
                        }

                        DeleteEvent:
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();

                    }
                }
                if (tempRemoveList.Count > 0)
                {
                    foreach (var item in tempRemoveList)
                    {
                        nowSelectNode.Value.TaskEventTriggerDic.Remove(item);
                    }
                }
            }
            EditorGUILayout.LabelField("******************************************************");
            EditorGUILayout.Space();
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }

    /// <summary>
    /// 任务的节点关系设置面板
    /// </summary>
    public class TaskEditor_NodeRelationShip : EditorWindow
    {
        /// <summary>
        /// 任务图对象
        /// </summary>
        public TaskMap taskMap;
        /// <summary>
        /// 关系节点对下给你
        /// </summary>
        public KeyValuePair<int, int> KVP;

        private void Awake()
        {
            Rect nowRect = position;
            position = new Rect(nowRect.x, nowRect.y, 150, 200);
        }

        TaskEditor_NodeRelationShip()
        {
            this.titleContent = new GUIContent("节点关系设置");
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        private void OnGUI()
        {
            if (taskMap == null || (KVP.Key == 0 && KVP.Value == 0))
                return;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("删除连结", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("请再次确认!", "是否删除两个节点的关联", "删除", "取消"))
                {
                    taskMap.DeleteRelationShip(KVP.Key, KVP.Value);
                    this.Close();
                }
            }
            NodeRelationShip nodeRelationShip = taskMap.GetNodeRelationShip(KVP.Key, KVP.Value);
            if (nodeRelationShip == null)
            {
                if (GUILayout.Button("设置关系", GUILayout.Width(60)))
                {
                    taskMap.CreateNodeRelationShip(KVP.Key, KVP.Value);
                }
            }
            else
            {
                if (GUILayout.Button("移除关系", GUILayout.Width(60)))
                {
                    taskMap.RemoveNodeRelationShip(nodeRelationShip);
                }
            }
            EditorGUILayout.EndHorizontal();
            if (nodeRelationShip != null)
            {
                if (nodeRelationShip.JudgingStatus != Enums.EnumTaskProgress.Started && nodeRelationShip.JudgingStatus != Enums.EnumTaskProgress.Sucessed)
                    nodeRelationShip.JudgingStatus = Enums.EnumTaskProgress.Sucessed;//默认完成才可以进行互斥的选择判断
                string[] explans = new string[] { "互斥", "前置失败", "前置完成" };
                int[] values = new int[] { (int)EnumNodeRelationShip.Mutex, (int)EnumNodeRelationShip.SingleExclusion, (int)EnumNodeRelationShip.Predecessor };
                int index = nodeRelationShip.RelationShip == EnumNodeRelationShip.Mutex ? 0 : (nodeRelationShip.RelationShip == EnumNodeRelationShip.SingleExclusion ? 1 : 2);
                index = EditorGUILayout.Popup("关系类型", index, explans);
                if (index > -1)
                {
                    nodeRelationShip.RelationShip = (EnumNodeRelationShip)values[index];
                }
                EditorGUILayout.Space();
                //设置节点显示效果的函数
                Action<MapElement<TaskInfoStruct>> ShowNodeMessageAction = (node) =>
                {
                    GUILayout.Button(node.Name + "[ID:" + node.ID + "][Deep:" + node.Deep + "][标题:" + node.Value.TaskTitile + "]");
                };
                MapElement<TaskInfoStruct> node1 = taskMap.GetElement(nodeRelationShip.StartID);
                MapElement<TaskInfoStruct> node2 = taskMap.GetElement(nodeRelationShip.EndID);
                EditorGUILayout.BeginHorizontal();
                bool judgingStatus = EditorGUILayout.Toggle(nodeRelationShip.JudgingStatus == Enums.EnumTaskProgress.Sucessed, GUILayout.Width(20));
                EditorGUILayout.LabelField("选择时判断对方状态(√->判断为完成;×->判断为执行中)");
                nodeRelationShip.JudgingStatus = judgingStatus ? Enums.EnumTaskProgress.Sucessed : Enums.EnumTaskProgress.Started;
                EditorGUILayout.EndHorizontal();
                switch (nodeRelationShip.RelationShip)
                {
                    case EnumNodeRelationShip.Mutex://互斥
                        ShowNodeMessageAction(node1);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("X", GUILayout.Width(15));
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();
                        ShowNodeMessageAction(node2);
                        break;
                    case EnumNodeRelationShip.SingleExclusion://前置失败
                    case EnumNodeRelationShip.Predecessor://前置完成
                        EditorGUILayout.LabelField("前置节点");
                        ShowNodeMessageAction(node1);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        if (GUILayout.Button("交换前后置关系", GUILayout.Width(75)))
                        {
                            int tempID = nodeRelationShip.StartID;
                            nodeRelationShip.StartID = nodeRelationShip.EndID;
                            nodeRelationShip.EndID = tempID;
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.LabelField("后置节点");
                        ShowNodeMessageAction(node2);
                        break;
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnLostFocus()
        {
            this.Close();
        }
    }

    /// <summary>
    /// 任务的组关系设置面板
    /// </summary>
    public class TaskEditor_RelationShipZone : EditorWindow
    {
        /// <summary>
        /// 任务图对象
        /// </summary>
        public TaskMap taskMap;
        /// <summary>
        /// 关系组对象
        /// </summary>
        public RelationShipZone zone;

        /// <summary>
        /// 渐变图片
        /// </summary>
        Texture2D texture2D;

        private void Awake()
        {
            Rect nowRect = position;
            position = new Rect(nowRect.x, nowRect.y, 150, 300);
            texture2D = Resources.Load<Texture2D>("Task/Gradual");
        }

        TaskEditor_RelationShipZone()
        {
            this.titleContent = new GUIContent("组关系设置");
        }

        private void OnInspectorUpdate()
        {
            this.Repaint();
        }

        /// <summary>
        /// 开始集合想要添加的id
        /// </summary>
        private int startListWantAddID;
        /// <summary>
        /// 结束结合想要添加的id 
        /// </summary>
        private int endListWantAddID;

        private void OnGUI()
        {
            if (taskMap == null || zone == null)
                return;
            if (zone.JudgingStatus != Enums.EnumTaskProgress.Started && zone.JudgingStatus != Enums.EnumTaskProgress.Sucessed)
                zone.JudgingStatus = Enums.EnumTaskProgress.Sucessed;//默认完成才可以进行互斥的选择判断
            EditorGUILayout.BeginVertical();
            //删除
            if (GUILayout.Button("删除组关系", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("请再次确认!", "是否删除两个组的关系", "删除", "取消"))
                {
                    taskMap.RemoveRelationShipZone(zone);
                    this.Close();
                }
            }
            //显示的颜色
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("线框显示颜色", GUILayout.Width(70));
            EditorGUILayout.BeginVertical();
            Handles.color = zone.EditorColor;
            Handles.DrawAAPolyLine(10, new Vector3(80, 25, 0), new Vector3(position.width - 70, 25, 0));
            zone.ColorSlider = EditorGUILayout.Slider(zone.ColorSlider, 0, 1);//滑动条
            //通过0-1的数据取出颜色
            float widthIndex = texture2D.width * zone.ColorSlider;
            widthIndex = Mathf.Clamp(widthIndex, 0, texture2D.width - 1);
            zone.EditorColor = texture2D.GetPixel((int)widthIndex, 0);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            //关系类型
            string[] explans = new string[] { "互斥", "前置失败", "前置完成" };
            int[] values = new int[] { (int)EnumNodeRelationShip.Mutex, (int)EnumNodeRelationShip.SingleExclusion, (int)EnumNodeRelationShip.Predecessor };
            int index = zone.RelationShip == EnumNodeRelationShip.Mutex ? 0 : (zone.RelationShip == EnumNodeRelationShip.SingleExclusion ? 1 : 2);
            index = EditorGUILayout.Popup("关系类型", index, explans);
            if (index > -1)
            {
                zone.RelationShip = (EnumNodeRelationShip)values[index];
            }
            //设置节点显示效果的函数
            Func<MapElement<TaskInfoStruct>, bool> ShowNodeMessageAction = (node) =>
             {
                 if (node == null)
                     return false;
                 EditorGUILayout.BeginHorizontal();
                 bool result = GUILayout.Button("X", GUILayout.Width(20));
                 GUILayout.Button(node.Name + "[ID:" + node.ID + "][Deep:" + node.Deep + "][标题:" + node.Value.TaskTitile + "]");
                 EditorGUILayout.EndHorizontal();
                 return result;
             };
            //取出可以添加的节点
            MapElement<TaskInfoStruct>[] canAddNodes = taskMap.GetAllElement().Where(temp => !zone.StartIDList.Contains(temp.ID) && !zone.EndIDList.Contains(temp.ID)).ToArray();
            List<int> canAddNodes_IDs = canAddNodes.Select(temp => temp.ID).ToList();
            string[] cannAddNodes_Explan = canAddNodes.Select(temp => temp.Name + "[ID:" + temp.ID + "][Deep:" + temp.Deep + "][标题:" + temp.Value.TaskTitile + "]").ToArray();
            //取出显示的节点
            MapElement<TaskInfoStruct>[] node1s = zone.StartIDList.Select(temp => taskMap.GetElement(temp)).ToArray();
            MapElement<TaskInfoStruct>[] node2s = zone.EndIDList.Select(temp => taskMap.GetElement(temp)).ToArray();
            //添加节点的处理函数
            Func<int, List<int>, string[], int> ShowAddNodesFunc = (wantAddID, lists, popShows) =>
               {
                   EditorGUILayout.BeginHorizontal();
                   int nowIndex = lists.IndexOf(wantAddID);
                   nowIndex = EditorGUILayout.Popup(nowIndex, popShows);
                   if (nowIndex > -1)
                       wantAddID = lists[nowIndex];
                   else wantAddID = 0;//如果没有则想要添加的id是0(id从1开始的)
                   if (wantAddID > 0 && GUILayout.Button("添加", GUILayout.Width(35)))
                   {
                       wantAddID = -1;//id为-1表示点击了添加
                   }
                   EditorGUILayout.EndHorizontal();
                   return wantAddID;
               };
            EditorGUILayout.BeginHorizontal();
            bool judgingStatus = EditorGUILayout.Toggle(zone.JudgingStatus == Enums.EnumTaskProgress.Sucessed, GUILayout.Width(20));
            EditorGUILayout.LabelField("选择时判断对方状态(√->判断为完成;×->判断为执行中)");
            zone.JudgingStatus = judgingStatus ? Enums.EnumTaskProgress.Sucessed : Enums.EnumTaskProgress.Started;
            EditorGUILayout.EndHorizontal();
            switch (zone.RelationShip)
            {
                case EnumNodeRelationShip.Mutex://互斥
                    zone.StartPassCount = EditorGUILayout.IntField("最小任务数判断(用于判断节点集合的完成与否)", zone.StartPassCount);
                    {
                        int wantAddID_Start = ShowAddNodesFunc(startListWantAddID, canAddNodes_IDs, cannAddNodes_Explan);
                        if (wantAddID_Start == -1)
                            zone.StartIDList.Add(startListWantAddID);
                        startListWantAddID = wantAddID_Start;
                    }
                    foreach (MapElement<TaskInfoStruct> taskNode in node1s)
                    {
                        bool delete = ShowNodeMessageAction(taskNode);
                        if (delete)
                            zone.StartIDList.Remove(taskNode.ID);
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("X", GUILayout.Width(15));
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    zone.EndPassCount = EditorGUILayout.IntField("最小任务数判断(用于判断节点集合的完成与否)", zone.EndPassCount);
                    {
                        int wantAddID_End = ShowAddNodesFunc(endListWantAddID, canAddNodes_IDs, cannAddNodes_Explan);
                        if (wantAddID_End == -1)
                            zone.EndIDList.Add(endListWantAddID);
                        endListWantAddID = wantAddID_End;
                    }
                    foreach (MapElement<TaskInfoStruct> taskNode in node2s)
                    {
                        bool delete = ShowNodeMessageAction(taskNode);
                        if (delete)
                            zone.EndIDList.Remove(taskNode.ID);
                    }
                    break;
                case EnumNodeRelationShip.SingleExclusion:
                case EnumNodeRelationShip.Predecessor:
                    EditorGUILayout.LabelField("前置组");
                    zone.StartPassCount = EditorGUILayout.IntField("最小任务数判断(用于判断节点集合的完成与否)", zone.StartPassCount);
                    {
                        int wantAddID_Start = ShowAddNodesFunc(startListWantAddID, canAddNodes_IDs, cannAddNodes_Explan);
                        if (wantAddID_Start == -1)
                            zone.StartIDList.Add(startListWantAddID);
                        startListWantAddID = wantAddID_Start;
                    }
                    foreach (MapElement<TaskInfoStruct> taskNode in node1s)
                    {
                        bool delete = ShowNodeMessageAction(taskNode);
                        if (delete)
                            zone.StartIDList.Remove(taskNode.ID);
                    }
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (GUILayout.Button("交换前后置关系", GUILayout.Width(75)))
                    {
                        List<int> tempIDs = zone.StartIDList;
                        zone.StartIDList = zone.EndIDList;
                        zone.EndIDList = tempIDs;
                        int tempPassCount = zone.StartPassCount;
                        zone.StartPassCount = zone.EndPassCount;
                        zone.EndPassCount = tempPassCount;
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.LabelField("后置组");
                    zone.EndPassCount = EditorGUILayout.IntField("最小任务数判断(用于判断节点集合的完成与否)", zone.EndPassCount);
                    {
                        int wantAddID_End = ShowAddNodesFunc(endListWantAddID, canAddNodes_IDs, cannAddNodes_Explan);
                        if (wantAddID_End == -1)
                            zone.EndIDList.Add(endListWantAddID);
                        endListWantAddID = wantAddID_End;
                    }
                    foreach (MapElement<TaskInfoStruct> taskNode in node2s)
                    {
                        bool delete = ShowNodeMessageAction(taskNode);
                        if (delete)
                            zone.EndIDList.Remove(taskNode.ID);
                    }
                    break;

            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            CheckState();
        }

        /// <summary>
        /// 检测当前状态
        /// </summary>
        private void CheckState()
        {
            if (taskMap == null || zone == null)
                return;
            if (zone.EndIDList.Count == 0 || zone.StartIDList.Count == 0)
            {
                taskMap.RemoveRelationShipZone(zone);
                this.Close();
            }
        }

        private void OnLostFocus()
        {
            this.Close();
        }
    }
}