using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapStruct;
using UnityEngine;
using Newtonsoft.Json;

namespace TaskMap
{
    /// <summary>
    /// 任务图结构
    /// </summary>
    public class TaskMap
    {
        /// <summary>
        /// 任务图对象
        /// </summary>
        Map<TaskInfoStruct> taskMap;
        /// <summary>
        /// 节点关系集合
        /// </summary>
        List<NodeRelationShip> nodeRelationShipList;
        /// <summary>
        /// 节点组关系集合
        /// </summary>
        List<RelationShipZone> relationShipZoneList;

        public TaskMap()
        {
            nodeRelationShipList = new List<NodeRelationShip>();
            relationShipZoneList = new List<RelationShipZone>();
            taskMap = new Map<TaskInfoStruct>();
        }

        /// <summary>
        /// 获取所有节点组对象
        /// </summary>
        /// <returns></returns>
        public RelationShipZone[] GetAllRelationShipZone()
        {
            return relationShipZoneList.ToArray();
        }

        /// <summary>
        /// 获取指定节点的组关系数组(因为一个节点可以有多个关系组)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationShipZone[] GetRelationShipZone(int id)
        {
            return relationShipZoneList.Where(temp => temp.StartIDList.Contains(id) || temp.EndIDList.Contains(id)).ToArray();
        }

        /// <summary>
        /// 获取指定关联节点数组的关系组
        /// </summary>
        /// <param name="ids1"></param>
        /// <param name="ids2"></param>
        /// <returns></returns>
        public RelationShipZone GetRelationShipZone(int[] ids1, int[] ids2)
        {
            if (ids1 == null || ids2 == null)
                return null;
            //去除重复节点
            ids1 = ids1.Distinct().ToArray();
            ids2 = ids2.Distinct().ToArray();
            return relationShipZoneList.Where(temp =>
            {
                if (temp.StartIDList.Count == ids1.Length && temp.EndIDList.Count == ids2.Length)
                {
                    int test1_ids1 = temp.StartIDList.Intersect(ids1).Count();
                    int test1_ids2 = temp.EndIDList.Intersect(ids2).Count();
                    if (test1_ids1 == ids1.Length && test1_ids2 == ids2.Length)
                        return true;
                }
                else if (temp.StartIDList.Count == ids2.Length && temp.EndIDList.Count == ids1.Length)
                {
                    int test2_ids2 = temp.StartIDList.Intersect(ids2).Count();
                    int test2_ids1 = temp.EndIDList.Intersect(ids1).Count();
                    if (test2_ids1 == ids1.Length && test2_ids2 == ids2.Length)
                        return true;
                }
                return false;
            }).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有任务节点的对应关系(这里值得是任务的互斥前置等关系)
        /// </summary>
        /// <returns></returns>
        public NodeRelationShip[] GetAllNodeRelationShip()
        {
            return nodeRelationShipList.ToArray();
        }

        /// <summary>
        /// 获取指定节点的关系数组(因为一个节点可以有多个关系)
        /// </summary>
        /// <param name="id">指定节点</param>
        /// <returns></returns>
        public NodeRelationShip[] GetNodeRelationShip(int id)
        {
            return nodeRelationShipList.Where(temp => temp.StartID == id || temp.EndID == id).ToArray();
        }

        /// <summary>
        /// 获取指定关联节点的关系对象 
        /// </summary>
        /// <param name="id1">节点1</param>
        /// <param name="id2">节点2</param>
        /// <returns></returns>
        public NodeRelationShip GetNodeRelationShip(int id1, int id2)
        {
            return nodeRelationShipList.FirstOrDefault(temp => (temp.StartID == id1 && temp.EndID == id2) || (temp.StartID == id2 && temp.EndID == id1));
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        public MapElement<TaskInfoStruct>[] GetAllElement()
        {
            return taskMap.GetElementAll();
        }

        /// <summary>
        /// 获取第一个节点
        /// </summary>
        /// <returns></returns>
        public MapElement<TaskInfoStruct> GetFirstElement()
        {
            return taskMap.FirstElement;
        }

        /// <summary>
        /// 设置该节点为第一个节点
        /// </summary>
        /// <param name="target"></param>
        public void SetFirstElement(MapElement<TaskInfoStruct> target)
        {
            taskMap.FirstElement = target;
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public MapElement<TaskInfoStruct> GetElement(int id)
        {
            return taskMap.GetElement(id);
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public MapElement<TaskInfoStruct> CreateElement()
        {
            return taskMap.CreateMapElement();
        }

        /// <summary>
        /// 创建节点关系
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public NodeRelationShip CreateNodeRelationShip(int id1, int id2)
        {
            NodeRelationShip nodeRelationShip = GetNodeRelationShip(id1, id2);
            if (nodeRelationShip == null)
            {
                nodeRelationShip = new NodeRelationShip();
                nodeRelationShip.StartID = id1;
                nodeRelationShip.EndID = id2;
                nodeRelationShipList.Add(nodeRelationShip);
            }
            return nodeRelationShip;
        }

        /// <summary>
        /// 创建组关系
        /// </summary>
        /// <param name="startZone">起始组</param>
        /// <param name="endZone">结束组</param>
        /// <param name="relationShipZone">返回的节点组</param>
        /// <returns></returns>
        public bool CreateRelationShipZone(List<int> startZone, List<int> endZone, out RelationShipZone relationShipZone)
        {
            relationShipZone = null;
            if (!(startZone == null || startZone.Count == 0 || endZone == null || endZone.Count == 0))
            {
                startZone = startZone.Distinct().ToList();
                endZone = endZone.Distinct().ToList();
                int sumCount = startZone.Concat(endZone).Distinct().Count();
                if (sumCount == startZone.Count + endZone.Count)//判断两者之间没有交集
                {
                    relationShipZone = GetRelationShipZone(startZone.ToArray(), endZone.ToArray());
                    if (relationShipZone == null)
                    {
                        relationShipZone = new RelationShipZone();
                        relationShipZone.StartIDList = startZone;
                        relationShipZone.EndIDList = endZone;
                        relationShipZone.EditorColor = Color.green;
                        relationShipZoneList.Add(relationShipZone);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 移除指定id的节点(关系也会一并移除)
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            MapElement<TaskInfoStruct> target = taskMap.GetElement(id);
            if (target != null)
                taskMap.Remove(target);
            //查看关系列表是否存在关系,存在则移除
            NodeRelationShip[] thisIDNodeRelationShipArray = GetNodeRelationShip(id);//与该节点相关联的关系对象 
            foreach (NodeRelationShip nodeRelationShip in thisIDNodeRelationShipArray)
            {
                nodeRelationShipList.Remove(nodeRelationShip);
            }
            //查看组关系列表是否存在关系,存在则移除内部数据,如果内部数据出现空的情况则移除组
            RelationShipZone[] thisIDRelationShipZoneArray = GetRelationShipZone(id);//与该节点关联的组对象
            foreach (RelationShipZone relationShipZone in thisIDRelationShipZoneArray)
            {
                relationShipZone.StartIDList.Remove(id);
                relationShipZone.EndIDList.Remove(id);
                if (relationShipZone.StartIDList.Count() == 0 || relationShipZone.EndIDList.Count() == 0)
                    relationShipZoneList.Remove(relationShipZone);
            }
        }

        /// <summary>
        /// 移除节点关系对象(不会移除节点以及节点的关联)
        /// </summary>
        /// <param name="nodeRelationShip"></param>
        public void RemoveNodeRelationShip(NodeRelationShip nodeRelationShip)
        {
            if (nodeRelationShip != null)
                nodeRelationShipList.Remove(nodeRelationShip);
        }

        /// <summary>
        /// 移除组关系对象(不会移除节点以及节点的关联)
        /// </summary>
        /// <param name="relationShipZone"></param>
        public void RemoveRelationShipZone(RelationShipZone relationShipZone)
        {
            if (relationShipZone != null)
                relationShipZoneList.Remove(relationShipZone);
        }

        /// <summary>
        /// 移除指定id之间的关系(包括附加的关系-->前置以及互斥)
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        public void DeleteRelationShip(int id1, int id2)
        {
            NodeRelationShip nodeRelationShip = GetNodeRelationShip(id1, id2);
            RemoveNodeRelationShip(nodeRelationShip);
            MapElement<TaskInfoStruct> taskInfoNode1 = GetElement(id1);
            MapElement<TaskInfoStruct> taskInfoNode2 = GetElement(id2);
            if (taskInfoNode1 != null && taskInfoNode2 != null)
                taskInfoNode1.CorrelativesNode.Remove(taskInfoNode2);
        }



        #region 加载于保存
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="jsonValue"></param>
        public void Load(string jsonValue)
        {
            JsonSerializationStruct jsonStruct = DeSerializeNow<JsonSerializationStruct>(jsonValue);
            taskMap.Load(jsonStruct.Value);
            nodeRelationShipList = jsonStruct.nodeRelationShipList;
            if (nodeRelationShipList == null)
                nodeRelationShipList = new List<NodeRelationShip>();
            relationShipZoneList = jsonStruct.relationShipZoneList;
            if (relationShipZoneList == null)
                relationShipZoneList = new List<RelationShipZone>();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            JsonSerializationStruct jsonStruct = new JsonSerializationStruct();
            jsonStruct.Value = taskMap.Save();
            jsonStruct.nodeRelationShipList = nodeRelationShipList;
            jsonStruct.relationShipZoneList = relationShipZoneList;
            string jsonValue = SerializeNow<JsonSerializationStruct>(jsonStruct);
            return jsonValue;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="U">反序列化后的类型</typeparam>
        /// <param name="target">对象</param>
        /// <returns>返回的字符串</returns>
        public string SerializeNow<U>(U target) where U : class
        {
            if (target == null)
                return "";
            string value = JsonConvert.SerializeObject(target, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return value;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="U">反序列化后的类型</typeparam>
        /// <param name="value">字符串</param>
        /// <returns>对象</returns>
        public U DeSerializeNow<U>(string value) where U : class
        {
            U target = JsonConvert.DeserializeObject<U>(value, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return target;
        }

        /// <summary>
        /// 序列化保存结构
        /// </summary>
        public class JsonSerializationStruct
        {
            /// <summary>
            /// 结构数据
            /// </summary>
            public string Value;
            /// <summary>
            /// 节点关系数据
            /// </summary>
            public List<NodeRelationShip> nodeRelationShipList;
            /// <summary>
            /// 组关系数据
            /// </summary>
            public List<RelationShipZone> relationShipZoneList;
        }
        #endregion

        #region 提供给调用方
        /// <summary>
        /// 最后一层未接取的任务
        /// </summary>
        List<MapElement<TaskInfoStruct>> LastFrameNodes;

        /// <summary>
        /// 获取当前最近的一层为接取任务
        /// </summary>
        /// <returns></returns>
        internal MapElement<TaskInfoStruct>[] GetLastFrameNodes()
        {
            if (LastFrameNodes == null)
            {
                List<MapElement<TaskInfoStruct>> resultList = new List<MapElement<TaskInfoStruct>>();
                List<MapElement<TaskInfoStruct>> passList = new List<MapElement<TaskInfoStruct>>();//已经便利过的节点
                MapElement<TaskInfoStruct> firstTaskInfoNode = taskMap.FirstElement;
                List<MapElement<TaskInfoStruct>> tempNodeList = new List<MapElement<TaskInfoStruct>>();
                tempNodeList.Add(firstTaskInfoNode);
                while (tempNodeList.Count > 0)
                {
                    List<MapElement<TaskInfoStruct>> innerTempNodeList = new List<MapElement<TaskInfoStruct>>();
                    foreach (MapElement<TaskInfoStruct> tempNode in tempNodeList)
                    {
                        if (passList.Contains(tempNode))//已经遍历过的就不需要便利了
                            continue;
                        switch (tempNode.Value.TaskProgress)
                        {
                            case Enums.EnumTaskProgress.Sucessed://成功则选取他的子节点进入判断
                                //选取没有失败的任务(成功的任务会在下次判断中递归 未接取的任务会在下次判断中拾取 正在执行的任务下次过程中不做处理)
                                innerTempNodeList.AddRange(tempNode.Next(EnumMapTraversalModel.Equal | EnumMapTraversalModel.More, temp => temp.TaskProgress != Enums.EnumTaskProgress.Failed)
                                    .Select(temp => temp as MapElement<TaskInfoStruct>));
                                break;
                            case Enums.EnumTaskProgress.NoTake://未接取的任务放入结果集合
                                bool relationShipPass = true;
                                //判断该任务的前置是否完成
                                NodeRelationShip[] nodeRelationShips = GetNodeRelationShip(tempNode.ID).Where(temp => temp.RelationShip == EnumNodeRelationShip.Predecessor && temp.EndID == tempNode.ID).ToArray();
                                //检测是否所有的前置都完成了
                                foreach (NodeRelationShip nodeRelationShip in nodeRelationShips)
                                {
                                    MapElement<TaskInfoStruct> targetNode = GetElement(nodeRelationShip.StartID);
                                    if (targetNode.Value.TaskProgress != Enums.EnumTaskProgress.Sucessed)
                                    {
                                        relationShipPass = false;
                                        break;
                                    }
                                }
                                //检测前置组是否通过
                                RelationShipZone[] relationShipZones = GetRelationShipZone(tempNode.ID).Where(temp => temp.RelationShip == EnumNodeRelationShip.Predecessor && temp.EndIDList.Contains(tempNode.ID)).ToArray();
                                foreach (RelationShipZone relationShipZone in relationShipZones)
                                {
                                    int passCount = relationShipZone.StartIDList.Select(temp => GetElement(temp)).Where(temp => temp != null).Count(temp=>temp.Value.TaskProgress == Enums.EnumTaskProgress.Sucessed);
                                    if (passCount < relationShipZone.StartPassCount)
                                    {
                                        relationShipPass = false;
                                        break;
                                    }
                                }
                                if (relationShipPass)
                                    resultList.Add(tempNode);
                                break;
                            default://失败与正在执行的不做处理
                                break;
                        }
                        //便利过的放入便利后的集合
                        passList.Add(tempNode);
                    }
                    tempNodeList = innerTempNodeList;
                }
                LastFrameNodes = resultList;
            }
            return LastFrameNodes.ToArray();
        }

        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="taskInfoNode">任务节点</param>
        /// <param name="taskProgress">任务设置成的状态</param>
        internal void SetTaskState(MapElement<TaskInfoStruct> taskInfoNode, Enums.EnumTaskProgress taskProgress)
        {
            switch (taskProgress)
            {
                case Enums.EnumTaskProgress.NoTake://设置为未接取
                    if (taskInfoNode.Value.TaskProgress == Enums.EnumTaskProgress.Started)
                    {
                        taskInfoNode.Value.TaskProgress = taskProgress;
                        LastFrameNodes = null;
                    }
                    break;
                case Enums.EnumTaskProgress.Sucessed://设置为成功
                    if (taskInfoNode.Value.TaskProgress == Enums.EnumTaskProgress.Started)
                    {
                        taskInfoNode.Value.TaskProgress = taskProgress;
                        //互斥任务设置为faild
                        NodeRelationShip[] nodeRelationShips = GetNodeRelationShip(taskInfoNode.ID);
                        foreach (NodeRelationShip nodeRelationShip in nodeRelationShips)
                        {
                            if (nodeRelationShip.RelationShip == EnumNodeRelationShip.Mutex)
                            {
                                int targetID = nodeRelationShip.StartID == taskInfoNode.ID ? nodeRelationShip.EndID : nodeRelationShip.StartID;
                                MapElement<TaskInfoStruct> targetNode = GetElement(targetID);
                                if (targetNode != null)
                                    targetNode.Value.TaskProgress = Enums.EnumTaskProgress.Failed;//互斥任务设置为失败
                            }
                        }
                        //组互斥则设置互斥的组中任务为faild
                        RelationShipZone[] relationShipZones = GetRelationShipZone(taskInfoNode.ID).Where(temp=>temp.RelationShip == EnumNodeRelationShip.Mutex).ToArray();
                        Action<IEnumerable<MapElement<TaskInfoStruct>>> SetTaskNodesFialdAction = (taskNodes) =>
                        {
                            foreach (MapElement<TaskInfoStruct> taskNode in taskNodes)
                            {
                                if (taskNode.Value.TaskProgress == Enums.EnumTaskProgress.Started || taskNode.Value.TaskProgress == Enums.EnumTaskProgress.NoTake)
                                    taskNode.Value.TaskProgress = Enums.EnumTaskProgress.Failed;
                            }
                        };
                        foreach (RelationShipZone relationShipZone in relationShipZones)
                        {
                            if (relationShipZone.StartIDList.Contains(taskInfoNode.ID))//设置end组的任务为faild
                            {
                                IEnumerable<MapElement<TaskInfoStruct>> tempRemoveTaskNodes = relationShipZone.EndIDList.Select(temp => GetElement(temp)).Where(temp => temp != null);
                                SetTaskNodesFialdAction(tempRemoveTaskNodes);
                            }
                            else//设置start组的任务为faild
                            {
                                IEnumerable<MapElement<TaskInfoStruct>> tempRemoveTaskNodes = relationShipZone.StartIDList.Select(temp => GetElement(temp)).Where(temp => temp != null);
                                SetTaskNodesFialdAction(tempRemoveTaskNodes);
                            }
                        }
                        LastFrameNodes = null;
                    }
                    break;
                case Enums.EnumTaskProgress.Started://设置为正在执行
                    if (taskInfoNode.Value.TaskProgress == Enums.EnumTaskProgress.NoTake)
                    {
                        taskInfoNode.Value.TaskProgress = taskProgress;
                        LastFrameNodes = null;
                    }
                    break;
                default://失败属于内部状态
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// 有关系的节点
    /// </summary>
    public class NodeRelationShip
    {
        /// <summary>
        /// 第一个节点
        /// </summary>
        public int StartID;
        /// <summary>
        /// 第二个节点
        /// </summary>
        public int EndID;

        /// <summary>
        /// 节点的关系
        /// 如果是互斥,则两个节点互斥
        /// 如果是前置,第一个节点是第二个节点的前置
        /// </summary>
        public EnumNodeRelationShip RelationShip;

    }

    /// <summary>
    /// 关系组
    /// </summary>
    public class RelationShipZone
    {
        /// <summary>
        /// 第一个节点组
        /// </summary>
        public List<int> StartIDList;
        /// <summary>
        /// 第二个节点组
        /// </summary>
        public List<int> EndIDList;
        /// <summary>
        /// 第一个节点组最少通过数量(如果有最少该数量的任务完成则通过该节点)
        /// </summary>
        public int StartPassCount;
        /// <summary>
        /// 第二个节点组最少通过数量(如果有最少该数量的任务完成则通过该节点),一般用于互斥
        /// </summary>
        public int EndPassCount;

        /// <summary>
        /// 节点组的关系
        /// 如果是互斥,则两个节点组互斥
        /// 如果是前置,第一个节点组是第二个节点组的前置
        /// </summary>
        public EnumNodeRelationShip RelationShip;

        /// <summary>
        /// 颜色的标尺
        /// </summary>
        public float ColorSlider;
        [JsonProperty] private float R;
        [JsonProperty] private float G;
        [JsonProperty] private float B;
        /// <summary>
        /// 编辑时显示的颜色
        /// </summary>
        [JsonIgnore]
        public Color EditorColor
        {
            get
            {
                return new Color(R, G, B);
            }
            set
            {
                R = value.r;
                G = value.g;
                B = value.b;
            }
        }
        public RelationShipZone()
        {
            StartIDList = new List<int>();
            EndIDList = new List<int>();
        }
    }

    /// <summary>
    /// 节点的关系类型
    /// </summary>
    public enum EnumNodeRelationShip
    {
        /// <summary>
        /// 互斥
        /// </summary>
        Mutex,
        /// <summary>
        /// 前置
        /// </summary>
        Predecessor

    }
}
