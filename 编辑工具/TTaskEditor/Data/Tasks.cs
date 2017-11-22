using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TTaskEditor.Data
{
    public class Tasks
    {
        //提供父任务、子任务、检索任务是否互斥、提供当前所有可以做的任务

        private static Tasks _instance;

        public static Tasks Instance
        {
            get
            {

                return _instance ?? (_instance = new Tasks());
            }
        }


        private Grapic<MetaTaskInfo> Data;


        private NameValueCollection exlucsionTaskDic = new NameValueCollection();
        /// <summary>
        /// 通过配置文件加载任务
        /// </summary>
        /// <param name="path"></param>
        public void LoadTasks(string path)
        {
            if (!File.Exists(path)) return;
            Data = new Grapic<MetaTaskInfo>();
            Data.AllNodes = JsonConvert.DeserializeObject<List<MetaTaskInfo>>(File.ReadAllText(path));
            Data.RootNode = Data.AllNodes.Single(t => t.Parents == null);


        }

        /// <summary>
        /// 两任务是否互斥
        /// </summary>
        /// <param name="id1">任务1id</param>
        /// <param name="id2">任务2id</param>
        /// <returns></returns>
        public bool IsTaskExclusive(int id1, int id2)
        {
            if (exlucsionTaskDic.AllKeys.Contains(id1.ToString()))
            {
                if (exlucsionTaskDic.GetValues(id1.ToString()).Contains(id2.ToString()))
                    return true;
            }
            if (exlucsionTaskDic.AllKeys.Contains(id2.ToString()))
            {
                if (exlucsionTaskDic.GetValues(id2.ToString()).Contains(id1.ToString()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取当前所有可做任务
        /// </summary>
        /// <returns></returns>
        public List<MetaTaskInfo> GetAllToDoList()
        {
            return Data.GetLastFrameNodes().ToList();
        }
    }

    public class Grapic<T> where T : IGraphicNode<T>
    {

        /// <summary>
        /// root task没有parent
        /// </summary>
        public T RootNode { get; set; }


        public Grapic(T root)
        {
            this.RootNode = root;
        }

        public Grapic()
        {

        }

        public List<T> AllNodes = new List<T>();

        /// <summary>
        /// BFS遍历
        /// </summary>
        /// <param name="action"></param>
        public void BFSTransfer(Action<T> action)
        {
            if (RootNode == null) return;
            List<T> hasTransferNode = new List<T>();
            Queue<T> remainNode = new Queue<T>();
            remainNode.Enqueue(RootNode);
            while (remainNode.Count > 0)
            {
                T currentTransferNode = remainNode.Dequeue();
                action(currentTransferNode);
                if (currentTransferNode.Children == null) continue;
                foreach (var child in currentTransferNode.Children)
                {
                    if (!hasTransferNode.Contains(currentTransferNode) && child.CanVisit)
                    {
                        hasTransferNode.Add(currentTransferNode);
                        remainNode.Enqueue(child);
                    }
                }
            }
        }



        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        public T[] GetAllNode()
        {
            return AllNodes.ToArray();
        }

        /// <summary>
        /// DFS遍历
        /// </summary>
        /// <param name="action"></param>
        public void DFSTransfer(Action<T> action)
        {
            hasDFSNode.Clear();
            DFS(action, this.RootNode);
        }


        /// <summary>
        /// 得到最后一层节点
        /// </summary>
        /// <returns></returns>
        public T[] GetLastFrameNodes()
        {
            T[] allNode = GetAllNode();

            List<T> retNodes = new List<T>();

            foreach (var node in allNode)
            {
                if (node.CanVisit && node.Children != null && node.Children.All(n => n != null && n.CanVisit == false))
                {
                    if (!node.Visited)
                        retNodes.Add(node);
                }
            }
            return retNodes.ToArray();
        }


        List<T> hasDFSNode = new List<T>();


        private void DFS(Action<T> action, T startNode)
        {
            if (startNode == null) return;
            action(startNode);
            hasDFSNode.Add(startNode);
            if (startNode.Children == null) return;
            foreach (T childNode in startNode.Children)
            {
                if (hasDFSNode.Contains(childNode) || (hasDFSNode != null && childNode.CanVisit == false))
                    continue;
                DFS(action, childNode);
            }
        }
    }

    public interface IGraphicNode<T>
    {
        List<T> Children { get; set; }

        List<T> Parents { get; set; }

        bool CanVisit { get; set; }

        bool Visited { get; set; }

    }

    public class MetaTaskInfo : IGraphicNode<MetaTaskInfo>
    {

        /// <summary>
        /// 任务ID
        /// </summary>
        public int ID { get; set; }

        private bool isStart;


     


        public MetaTaskInfo()
        {
            MetaTaskNode = new MetaTaskNode();
        }
        /// <summary>
        /// 任务节点
        /// </summary>
        [JsonProperty("TaskNode")]
        public MetaTaskNode MetaTaskNode { get; set; }

        /// <summary>
        /// 当前节点的孩子们
        /// </summary>
        public List<MetaTaskInfo> Children { get; set; }

        /// <summary>
        /// 当前节点的父亲们
        /// </summary>
        public List<MetaTaskInfo> Parents { get; set; }

        /// <summary>
        /// 是否可以visit
        /// </summary>
        public bool CanVisit { get; set; }

        /// <summary>
        /// 是否已经Visited
        /// </summary>
        public bool Visited { get; set; }


        private void EnsureHasChildren()
        {
            if (Children == null)
                Children = new List<MetaTaskInfo>();
        }

        private void EnsureHasParents()
        {
            if (Parents == null)
                Parents = new List<MetaTaskInfo>();
        }

        /// <summary>
        /// 增加一个孩纸
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(MetaTaskInfo child)
        {
            EnsureHasChildren();
            this.Children.Add(child);
        }

        /// <summary>
        /// 增加一个父亲
        /// </summary>
        /// <param name="parent"></param>
        public void AddParent(MetaTaskInfo parent)
        {
            EnsureHasParents();
            this.Parents.Add(parent);
        }

        /// <summary>
        /// 增加孩子节点
        /// </summary>
        /// <param name="childs"></param>
        public void AddChildren(params MetaTaskInfo[] childs)
        {
            foreach (var taskNode in childs)
            {
                AddChild(taskNode);
            }
        }

        /// <summary>
        /// 增加父亲节点
        /// </summary>
        /// <param name="parents"></param>
        public void AddParents(params MetaTaskInfo[] parents)
        {
            foreach (var taskNode in parents)
            {
                this.Parents.Add(taskNode);
            }
        }

        /// <summary>
        /// 移除孩子节点
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(MetaTaskInfo child)
        {
            if (this.Children == null || this.Children.Count <= 0) return;
            this.Children.RemoveAll(t => t.ID.Equals(child.ID));

        }

        /// <summary>
        /// 移除父亲节点
        /// </summary>
        /// <param name="parent"></param>
        public void RemoveParent(MetaTaskInfo parent)
        {
            if (this.Parents == null || this.Parents.Count <= 0) return;
            this.Parents.RemoveAll(t => t.ID.Equals(parent.ID));
        }

        /// <summary>
        /// 移除孩子节点们
        /// </summary>
        /// <param name="childs"></param>
        public void RemoveChilds(params MetaTaskInfo[] childs)
        {
            foreach (var taskNode in childs)
            {
                RemoveChild(taskNode);
            }
        }

        /// <summary>
        /// 移动父亲节点们
        /// </summary>
        /// <param name="parents"></param>
        public void RemoveParents(params MetaTaskInfo[] parents)
        {
            foreach (var taskNode in parents)
            {
                RemoveParent(taskNode);
            }
        }



    }


}
