using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

