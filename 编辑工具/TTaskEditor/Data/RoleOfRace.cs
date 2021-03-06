﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
/// <summary>
/// 角色种族
/// </summary>
public enum RoleOfRace
{
    /// <summary>
    /// 人类
    /// </summary>
    Human,


    /// <summary>
    /// 精灵
    /// </summary>
    Elven,

    /// <summary>
    /// 不死者
    /// </summary>
    Athanasy,




    /// <summary>
    /// 混血人
    /// </summary>
    Hybrid,

    /// <summary>
    /// 勇士
    /// </summary>
    Warrior,

    /// <summary>
    /// 半精灵
    /// </summary>
    Halfelven,

    /// <summary>
    /// 木精灵
    /// </summary>
    WoodElven,

    /// <summary>
    /// 光精灵
    /// </summary>
    LightElven,

    /// <summary>
    /// 黑精灵
    /// </summary>
    BlackElven,

    /// <summary>
    /// 死灵
    /// </summary>
    SoulsOfTheDead,

    /// <summary>
    /// 骷颅
    /// </summary>
    Skeleton,


    /// <summary>
    /// 遗忘者
    /// </summary>
    Amnesiac,




    /// <summary>
    /// 逐光者
    /// </summary>
    ByTheLight,

    /// <summary>
    /// 狂人
    /// </summary>
    Madman,

    /// <summary>
    /// 龙人
    /// </summary>
    Draconian,

    /// <summary>
    /// 勇者
    /// </summary>
    Brave,

    /// <summary>
    /// 侠客
    /// </summary>
    ChivalrousMan,

    /// <summary>
    /// 唤兽者
    /// </summary>
    CallTheBeastor,

    /// <summary>
    /// 聆听者
    /// </summary>
    Listener,

    /// <summary>
    /// 自然精灵
    /// </summary>
    NatureElven,

    /// <summary>
    /// 洞穴精灵
    /// </summary>
    CaveElven,

    /// <summary>
    /// 太阳精灵
    /// </summary>
    SunElven,

    /// <summary>
    /// 彩虹精灵
    /// </summary>
    RainbowElven,


    /// <summary>
    /// 月精灵
    /// </summary>
    MoonElven,

    /// <summary>
    /// 暗夜精灵
    /// </summary>
    DarkNightElven,

    /// <summary>
    /// 死灵法师
    /// </summary>
    SoulsOfTheDeadWizard,

    /// <summary>
    /// 食尸鬼
    /// </summary>
    Ghoul,

    /// <summary>
    /// 死亡骑士
    /// </summary>
    DeathKnight,

    /// <summary>
    /// 骷髅王
    /// </summary>
    SkeletonKing,

    /// <summary>
    /// 吸血鬼
    /// </summary>
    Vampire,

    /// <summary>
    /// 暗影
    /// </summary>
    Shadow,

    /// <summary>
    /// 英雄
    /// </summary>
    Hero,

    /// <summary>
    /// 自然之声
    /// </summary>
    NatureSound,

    /// <summary>
    /// 精灵王
    /// </summary>
    ElvenKing,

    /// <summary>
    /// 灵体
    /// </summary>
    Souler,

    /// <summary>
    /// 魔王
    /// </summary>
    Devil,

    None,

}



/// <summary>
/// 物种辅助类
/// </summary>
public class RoleOfRaceHelper
{

    public static Tree<RoleOfRace> roleOfRaceTree;

    static RoleOfRaceHelper()
    {
        roleOfRaceTree = new Tree<RoleOfRace>();
        roleOfRaceTree.TopNode = new TreeNode<RoleOfRace>(RoleOfRace.None);
        //人类：
        var human = roleOfRaceTree.TopNode.AddChild(RoleOfRace.Human);
        //人类-->(遗忘者、混血人、勇士、半精灵)
        human.AddChilds(RoleOfRace.Amnesiac, RoleOfRace.Hybrid, RoleOfRace.Warrior, RoleOfRace.Halfelven);
        //人类-->逐光者
        var byTheLight = human.Children[0].AddChild(RoleOfRace.ByTheLight);
        //逐光者-->暗影
        byTheLight.AddChild(RoleOfRace.Shadow);
        //混血人->狂人、龙人
        human.Children[1].AddChilds(RoleOfRace.Madman, RoleOfRace.Draconian);
        //狂人-->暗影、英雄
        human.Children[1].Children[0].AddChilds(RoleOfRace.Shadow, RoleOfRace.Hero);
        //龙人-->英雄
        human.Children[1].Children[1].AddChild(RoleOfRace.Hero);
        //勇士-->勇者、侠客
        human.Children[2].AddChilds(RoleOfRace.Brave, RoleOfRace.ChivalrousMan);
        //勇者-->英雄
        human.Children[2].Children[0].AddChild(RoleOfRace.Hero);
        //侠客-->英雄、自然之声
        human.Children[2].Children[1].AddChilds(RoleOfRace.Hero, RoleOfRace.NatureSound);
        //半精灵-->唤兽者、聆听者
        human.Children[3].AddChilds(RoleOfRace.CallTheBeastor, RoleOfRace.Listener);
        //唤兽者-->自然之声
        human.Children[3].Children[0].AddChild(RoleOfRace.NatureSound);
        //聆听者-->自然之声
        human.Children[3].Children[1].AddChild(RoleOfRace.NatureSound);

        //========精灵===========
        var elven = roleOfRaceTree.TopNode.AddChild(RoleOfRace.Elven);
        //精灵-->半精灵、木精灵、光精灵、黑精灵
        elven.AddChilds(RoleOfRace.Halfelven, RoleOfRace.WoodElven, RoleOfRace.LightElven, RoleOfRace.BlackElven);
        //半精灵-->唤兽者、聆听者
        elven.Children[0].AddChilds(RoleOfRace.CallTheBeastor, RoleOfRace.Listener);
        //唤兽者-->自然之声
        elven.Children[0].Children[0].AddChild(RoleOfRace.NatureSound);
        //聆听者-->自然之声
        elven.Children[0].Children[1].AddChild(RoleOfRace.NatureSound);
        //木精灵-->自然精灵、洞穴精灵
        elven.Children[1].AddChilds(RoleOfRace.NatureElven, RoleOfRace.CaveElven);
        //自然精灵-->自然之声、精灵王
        elven.Children[1].Children[0].AddChilds(RoleOfRace.NatureSound, RoleOfRace.ElvenKing);
        elven.Children[1].Children[1].AddChild(RoleOfRace.ElvenKing);
        //光精灵-->太阳精灵、彩虹精灵
        elven.Children[2].AddChilds(RoleOfRace.SunElven, RoleOfRace.RainbowElven);
        //太阳精灵-->精灵王
        elven.Children[2].Children[0].AddChild(RoleOfRace.ElvenKing);
        //彩虹精灵-->精灵王、灵体
        elven.Children[2].Children[1].AddChilds(RoleOfRace.ElvenKing, RoleOfRace.Souler);
        //黑精灵-->月精灵、暗夜精灵
        elven.Children[3].AddChilds(RoleOfRace.MoonElven, RoleOfRace.DarkNightElven);
        //月精灵-->灵体
        elven.Children[3].Children[0].AddChild(RoleOfRace.Souler);
        //暗夜精灵-->灵体
        elven.Children[3].Children[1].AddChild(RoleOfRace.Souler);

        //============不死者========================
        var athanasy = roleOfRaceTree.TopNode.AddChild(RoleOfRace.Athanasy);
        //不死者-->黑精灵、死灵、骷髅、遗忘者
        athanasy.AddChilds(RoleOfRace.BlackElven, RoleOfRace.SoulsOfTheDead, RoleOfRace.Skeleton, RoleOfRace.Amnesiac);
        //黑精灵-->月精灵、暗夜精灵
        athanasy.Children[0].AddChilds(RoleOfRace.MoonElven, RoleOfRace.DarkNightElven);
        //月精灵-->灵体
        athanasy.Children[0].Children[0].AddChild(RoleOfRace.Souler);
        //暗夜精灵-->灵体
        athanasy.Children[0].Children[1].AddChild(RoleOfRace.Souler);

        //死灵-->死灵法师、食尸鬼
        athanasy.Children[1].AddChilds(RoleOfRace.SoulsOfTheDeadWizard, RoleOfRace.Ghoul);
        //死灵法师-->灵体、魔王
        athanasy.Children[1].Children[0].AddChilds(RoleOfRace.Souler, RoleOfRace.Devil);
        //食尸鬼-->魔王
        athanasy.Children[1].Children[1].AddChild(RoleOfRace.Devil);
        //骷髅-->死亡骑士、骷髅王
        athanasy.Children[2].AddChilds(RoleOfRace.DeathKnight, RoleOfRace.SkeletonKing);
        //死亡骑士-->魔王
        athanasy.Children[2].Children[0].AddChild(RoleOfRace.Devil);
        //骷髅王-->魔王、暗影
        athanasy.Children[2].Children[1].AddChilds(RoleOfRace.Devil, RoleOfRace.Shadow);
        //遗忘者-->吸血鬼
        athanasy.Children[3].AddChild(RoleOfRace.Vampire);
        //吸血鬼-->暗影
        athanasy.Children[3].Children[0].AddChild(RoleOfRace.Shadow);
    }

    /// <summary>
    /// 获取所有可达当前role的进化路径
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public static List<List<RoleOfRace>> GetAllEvolvePath(RoleOfRace role)
    {
        List<List<RoleOfRace>> retResults = new List<List<RoleOfRace>>();
        var results = roleOfRaceTree.GetAllPath(new TreeNode<RoleOfRace>(role));
        foreach (var result in results)
        {
            List<RoleOfRace> roleOfRaces = new List<RoleOfRace>();
            foreach (var r in result)
            {
                roleOfRaces.Add(r.Data);
            }
            retResults.Add(roleOfRaces);
        }
        return retResults;
    }



    /// <summary>
    /// 是否可以由from进化至to
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static bool CanEvolve(RoleOfRace from, RoleOfRace to)
    {
        return roleOfRaceTree.IsDescendant(new TreeNode<RoleOfRace>(from), new TreeNode<RoleOfRace>(to));
    }



    /// <summary>
    /// 查看当前角色所有可以进化来的路径
    /// </summary>
    /// <param name="role"></param>
    public static void LogAllEvolePath(RoleOfRace role)
    {
        var results = GetAllEvolvePath(role);
        foreach (var result in results)
        {
            string log = string.Empty;
            foreach (var r in result)
            {
                log += r + "\t";
            }
           // Debug.Log(log);
        }
    }
}


/// <summary>
/// 树节点
/// </summary>
/// <typeparam name="T"></typeparam>
public class TreeNode<T>
{
    /// <summary>
    /// 数据
    /// </summary>
    public T Data;

    public object Tag;

    /// <summary>
    /// 孩子节点
    /// </summary>
    public List<TreeNode<T>> Children;

    public TreeNode()
    {

    }

    public TreeNode(T data)
    {
        Data = data;
    }

    private void CheckChildren()
    {

        if (Children == null)
            Children = new List<TreeNode<T>>();
    }

    public TreeNode<T> AddChild(TreeNode<T> child)
    {
        CheckChildren();
        this.Children.Add(child);
        return child;
    }

    public TreeNode<T> AddChild(T child)
    {
        TreeNode<T> treeNodeChild = new TreeNode<T>(child);
        return AddChild(treeNodeChild);
    }


    public void RemoveChild(TreeNode<T> child)
    {
        CheckChildren();
        if (this.Children.Count <= 0 || !this.Children.Contains(child)) return;
        this.Children.Remove(child);
    }

    public void RemoveChild(T child)
    {
        TreeNode<T> treeNodeChild = new TreeNode<T>(child);
        RemoveChild(treeNodeChild);
    }

    public void AddChilds(params TreeNode<T>[] childs)
    {
        if (childs == null || childs.Length <= 0) return;
        foreach (var treeNode in childs)
        {
            AddChild(treeNode);
        }
    }

    public void AddChilds(params T[] childs)
    {
        List<TreeNode<T>> treeNodes = new List<TreeNode<T>>();
        foreach (var c in childs)
        {
            treeNodes.Add(new TreeNode<T>(c));
        }
        AddChilds(treeNodes.ToArray());
    }


    public void RemoveChilds(params TreeNode<T>[] childs)
    {
        if (childs == null || childs.Length <= 0) return;
        foreach (var treeNode in childs)
        {
            RemoveChild(treeNode);
        }
    }

    public void RemoveChilds(params T[] childs)
    {
        List<TreeNode<T>> treeNodes = new List<TreeNode<T>>();
        foreach (var c in childs)
        {
            treeNodes.Add(new TreeNode<T>(c));
        }
        RemoveChilds(treeNodes.ToArray());
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(TreeNode<T>)) return false;
        TreeNode<T> other = (TreeNode<T>)obj;
        if (other.Tag == null && this.Tag == null)
            return other.Data.Equals(this.Data);
        if (other.Tag == null && this.Tag != null)
            return false;
        if (other.Tag != null && this.Tag == null)
            return false;
        return false;
    }

    public override int GetHashCode()
    {
        if (Tag != null)
            return Tag.GetHashCode() ^ Data.GetHashCode();
        return Data.GetHashCode();
    }
}

public class TreeNodeComparer<T> : IEqualityComparer<TreeNode<T>>
{
    public bool Equals(TreeNode<T> x, TreeNode<T> y)
    {
        if (x == null && y == null)
            return true;
        if (x == null && y != null)
            return false;
        if (x != null && y == null)
            return false;
        return x.Data.Equals(y.Data);
    }

    public int GetHashCode(TreeNode<T> obj)
    {
        return obj.Data.GetHashCode();
    }
}

public class Tree<T>
{
    /// <summary>
    /// topNode
    /// </summary>
    public TreeNode<T> TopNode { get; set; }


    /// <summary>
    /// 深复制一颗树
    /// </summary>
    /// <param name="orginTree"></param>
    public Tree(Tree<T> orginTree)
    {
        DeepCopyNode(orginTree.TopNode, this.TopNode);
    }

    public Tree()
    {

    }

    /// <summary>
    /// deep copy
    /// </summary>
    /// <param name="orignNode"></param>
    /// <param name="copyToNode"></param>
    private void DeepCopyNode(TreeNode<T> orignNode, TreeNode<T> copyToNode)
    {
        copyToNode.Data = orignNode.Data;
        if (orignNode == null || orignNode.Children == null) return;
        for (int i = 0; i < orignNode.Children.Count; i++)
        {
            copyToNode.AddChild(new TreeNode<T>());
            DeepCopyNode(orignNode.Children[i], copyToNode.Children[i]);
        }
    }

    Stack<TreeNode<T>> pathNodes = new Stack<TreeNode<T>>();

    /// <summary>
    /// 获取某个树节点的路径
    /// </summary>
    /// <param name="topNode">开始节点</param>
    /// <param name="findNode">寻找节点</param>
    /// <returns></returns>
    private Stack<TreeNode<T>> GetFirstPath(TreeNode<T> topNode, TreeNode<T> findNode)
    {
        if (topNode == null || topNode.Children == null || topNode.Children.Count <= 0) return null;
        StackTrace stackTrace = new StackTrace();
        if (stackTrace.GetFrame(1).GetMethod() != System.Reflection.MethodBase.GetCurrentMethod())
        {
            pathNodes.Clear();
        }
        if (topNode.Equals(findNode))
        {
            return pathNodes;
        }

        foreach (var childNode in topNode.Children)
        {
            pathNodes.Push(childNode);
            if (childNode.Equals(findNode))
                return pathNodes;
            else
            {
                var childResult = GetFirstPath(childNode, findNode);
                if (childResult != null)
                    return childResult;
                else
                {
                    pathNodes.Pop();
                }
            }
        }
        return null;
    }


    /// <summary>
    /// 获取某树从topNode--->findNode所有可达路径
    /// </summary>
    /// <param name="topNode"></param>
    /// <param name="findNode"></param>
    /// <returns></returns>
    private List<List<TreeNode<T>>> GetAllPathImpl(TreeNode<T> topNode, TreeNode<T> findNode)
    {
        TreeNode<T> deepCopyTop = new TreeNode<T>();
        DeepCopyNode(topNode, deepCopyTop);
        List<List<TreeNode<T>>> results = new List<List<TreeNode<T>>>();
        Stack<TreeNode<T>> result;
        while ((result = GetFirstPath(deepCopyTop, findNode)) != null)
        {
            var copy = new Stack<TreeNode<T>>(result).ToArray();
            results.Add(new List<TreeNode<T>>(copy));
            result.Peek().Tag = "Sweeped";
        }
        return results;
    }



    /// <summary>
    /// 查找所有可以到达findNode的路径
    /// </summary>
    /// <param name="findNode"></param>
    /// <returns></returns>
    public List<List<TreeNode<T>>> GetAllPath(TreeNode<T> findNode)
    {
        return GetAllPathImpl(this.TopNode, findNode);
    }




    /// <summary>
    /// 判断child节点是不是ancestor的后代节点
    /// </summary>
    /// <param name="ancestor"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public bool IsDescendant(TreeNode<T> ancestor, TreeNode<T> child)
    {
        var allPath = GetAllPath(child);
        if (allPath == null || allPath.Count == 0) return false;
        if (!allPath.Any(path => path.Contains(ancestor, new TreeNodeComparer<T>())))
            return false;
        return true;
    }



}