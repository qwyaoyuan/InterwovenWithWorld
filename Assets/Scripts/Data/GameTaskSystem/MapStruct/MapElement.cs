using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapStruct
{
    /// <summary>
    /// 图节点的接口
    /// </summary>
    public interface IMapElement
    {
        /// <summary>
        /// 该节点的ID
        /// </summary>
        int ID { get; }

        /// <summary>
        /// 该节点的名字
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// 图节点的接口
    /// </summary>
    public interface IMapElement<T> : IMapElement, IMapTraversal<T>
    {
        /// <summary>
        /// 该节点的数据
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// 关联的节点
        /// </summary>
        MapElementCollection<T> CorrelativesNode { get; }
    }

    /// <summary>
    /// 图节点的遍历接口
    /// </summary>
    public interface IMapTraversal<T>
    {
        /// <summary>
        /// 节点深度(遍历时只可以向相同或者指定方向的大小遍历)
        /// </summary>
        int Deep { get; set; }

        /// <summary>
        /// 向下遍历
        /// </summary>
        /// <param name="model">便利模式</param>
        /// <param name="OutCheckFunc">额外检测函数</param>
        /// <returns></returns>
        IMapElement<T>[] Next(EnumMapTraversalModel model, Func<T, bool> OutCheckFunc = null);


    }

    /// <summary>
    /// 图元素的遍历模式(只判断关联节点)
    /// </summary>
    public enum EnumMapTraversalModel
    {
        /// <summary>
        /// 遍历深度相等的节点
        /// </summary>
        Equal = 1,
        /// <summary>
        /// 遍历深度大的节点
        /// </summary>
        More = 2,
        /// <summary>
        /// 遍历深度小的节点
        /// </summary>
        Less = 4,
        /// <summary>
        /// 遍历所有的节点
        /// </summary>
        All=7

    }

    /// <summary>
    /// 图节点
    /// </summary>
    public class MapElement<T> : IMapElement<T>
    {
        /// <summary>
        /// 该节点的id
        /// </summary>
        [JsonProperty]
        private int id;
        /// <summary>
        /// 该节点的ID
        /// </summary>
        [JsonIgnore]
        public int ID
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// 关联的节点
        /// </summary>
        [JsonIgnore]
        private MapElementCollection<T> correlativesNode;
        /// <summary>
        /// 关联的节点
        /// </summary>
        [JsonIgnore]
        public MapElementCollection<T> CorrelativesNode
        {
            get
            {
                return correlativesNode;
            }
        }

        /// <summary>
        /// 该节点的名字
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 该节点的数据
        /// </summary>
        [JsonProperty]
        public T Value { get; set; }

        /// <summary>
        /// 图对象
        /// </summary>
        [JsonIgnore]
        private Map<T> baseMap;

        /// <summary>
        /// 节点深度(遍历时只可以向相同或者指定方向的大小遍历)
        /// </summary>
        [JsonProperty]
        public int Deep { get; set; }

        internal MapElement() { }

        internal void  LoadMapElement(Map<T> map)
        {
            this.baseMap = map;
            correlativesNode = new MapElementCollection<T>(this, temp =>
            {
                MapElement<T> addElement = temp as MapElement<T>;
                if (addElement == null)//如果不能转换类型则表示不可以添加
                    return false;
                return this.baseMap.GetExtensionController().Constains(addElement);//如果存在则可以添加,不过不存在表示不是一个图不可以添加
            });
        }

        internal void SetID(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// 向下遍历
        /// </summary>
        /// <param name="model">便利模式</param>
        /// <param name="OutCheckFunc">额外检测函数</param>
        /// <returns></returns>
        public IMapElement<T>[] Next(EnumMapTraversalModel model, Func<T, bool> OutCheckFunc = null)
        {
            List<IMapElement<T>> resultList = new List<IMapElement<T>>();
            if ((model | EnumMapTraversalModel.Equal) == model)
            {
                resultList.AddRange(correlativesNode.GetAll().Where(temp => temp.Deep == Deep));
            }
            if ((model | EnumMapTraversalModel.Less) == model)
            {
                resultList.AddRange(correlativesNode.GetAll().Where(temp => temp.Deep < Deep));
            }
            if ((model | EnumMapTraversalModel.More) == model)
            {
                resultList.AddRange(correlativesNode.GetAll().Where(temp => temp.Deep > Deep));
            }
            if (OutCheckFunc != null)
            {
                resultList = resultList.Distinct().Where(temp => OutCheckFunc(temp.Value)).ToList();
            }
            return resultList.ToArray() ;
        }
    }

    /// <summary>
    /// 图节点集合
    /// </summary>
    public class MapElementCollection<T>
    {
        /// <summary>
        /// 与该集合关联的本节点
        /// </summary>
        IMapElement<T> baseIMapElement;

        /// <summary>
        /// 图节点集合
        /// </summary>
        List<IMapElement<T>> iMapElements;

        /// <summary>
        /// 是否添加的回调(主要是用于限制随意添加)
        /// </summary>
        Func<IMapElement<T>, bool> CanAddFunc;

        /// <summary>
        /// 构造
        /// </summary>
        public MapElementCollection()
        {
            iMapElements = new List<IMapElement<T>>();
            this.CanAddFunc = (temp) => false;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="baseIMapElement">与该集合关联的本节点</param>
        /// <param name="CanAddFunc">是否添加的回调(主要是用于限制随意添加)</param>
        public MapElementCollection(IMapElement<T> baseIMapElement, Func<IMapElement<T>, bool> CanAddFunc) : this()
        {
            this.baseIMapElement = baseIMapElement;
            this.CanAddFunc = CanAddFunc;
        }

        /// <summary>
        /// 集合数据 
        /// </summary>
        public int Count
        {
            get
            {
                return iMapElements.Count;
            }
        }

        /// <summary>
        /// 获取图节点元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IMapElement<T> this[int index]
        {
            get
            {
                if (iMapElements.Count > index && index >= 0)
                {
                    return iMapElements[index];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取图节点元素
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        public IMapElement<T>[] this[string mapName]
        {
            get
            {
                return iMapElements.Where(temp => string.Equals(temp.Name, mapName)).ToArray();
            }
        }

        /// <summary>
        /// 获取所有的关联节点
        /// </summary>
        /// <returns></returns>
        public IMapElement<T>[] GetAll()
        {
            return iMapElements.ToArray();
        }

        /// <summary>
        ///  是否存在指定元素
        /// </summary>
        /// <param name="iMapElement"></param>
        /// <returns></returns>
        public bool Contains(IMapElement<T> iMapElement)
        {
            return iMapElements.Contains(iMapElement);
        }

        #region 移除
        /// <summary>
        /// 清空关系
        /// </summary>
        public void Clear()
        {
            IMapElement<T>[] removeElements = iMapElements.ToArray();
            foreach (IMapElement<T> iMapElement in removeElements)
            {
                Remove(iMapElement);
            }
        }

        /// <summary>
        /// 移除指定下标的元素
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>是否成功</returns>
        public bool Remove(int index)
        {
            IMapElement<T> iMapElement = this[index];
            if (iMapElement != null)
                return Remove(iMapElement);
            return false;
        }

        /// <summary>
        /// 移除指定名字的元素
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="success">成功的对象</param>
        /// <param name="faild">失败的对象</param>
        /// <returns>是否全部成功</returns>
        public bool RemoveAll(string name, out IMapElement<T>[] success, out IMapElement<T>[] faild)
        {
            IMapElement<T>[] iMapElements = this[name];
            List<IMapElement<T>> successList = new List<IMapElement<T>>();
            List<IMapElement<T>> faildList = new List<IMapElement<T>>();
            bool result = iMapElements.Count() > 0;
            foreach (IMapElement<T> iMapElement in iMapElements)
            {
                bool removeResult = Remove(iMapElement);
                if (removeResult)
                    successList.Add(iMapElement);
                else
                {
                    result = removeResult;
                    faildList.Add(iMapElement);
                }
            }
            success = successList.ToArray();
            faild = faildList.ToArray();
            return result;
        }

        /// <summary>
        /// 移除指定的元素
        /// </summary>
        /// <param name="iMapElement">指定的元素</param>
        /// <returns>是否移除成功</returns>
        public bool Remove(IMapElement<T> iMapElement)
        {
            if (iMapElements.Contains(iMapElement))
            {
                iMapElements.Remove(iMapElement);//本届点移除对应的元素
                iMapElement.CorrelativesNode.UpdateRemove(baseIMapElement); //对应的元素移除本届点
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新移除指定节点
        /// 仅供内部调用
        /// </summary>
        /// <param name="iMapElement"></param>
        public void UpdateRemove(IMapElement<T> iMapElement)
        {
            if (!iMapElement.CorrelativesNode.Contains(baseIMapElement))//如果传来的机欸但已经不包含本届点了,则移除,如果还包含,则说明不是使用Remove函数移除的,不需要理财
                if (iMapElements.Contains(iMapElement))
                    iMapElements.Remove(iMapElement);
        }
        #endregion

        #region 添加

        /// <summary>
        /// 添加图节点
        /// </summary>
        /// <param name="iMapElement">图节点</param>
        /// <returns>是否添加成功</returns>
        public bool Add(IMapElement<T> iMapElement)
        {
            if (!CanAddFunc(iMapElement))
                return false;
            if (iMapElements.Contains(iMapElement))
                return false;
            if (baseIMapElement == iMapElement)
                return false;
            iMapElements.Add(iMapElement);//添加该节点
            iMapElement.CorrelativesNode.UpdateAdd(baseIMapElement);//该节点添加本届点
            return true;
        }

        /// <summary>
        /// 更新添加图节点
        /// 仅供内部调用
        /// </summary>
        /// <param name="iMapElement">图节点</param>
        public void UpdateAdd(IMapElement<T> iMapElement)
        {
            if (iMapElement.CorrelativesNode.Contains(baseIMapElement))
            {
                if (!iMapElements.Contains(iMapElement))
                    iMapElements.Add(iMapElement);
            }
        }

        #endregion

    }
}
