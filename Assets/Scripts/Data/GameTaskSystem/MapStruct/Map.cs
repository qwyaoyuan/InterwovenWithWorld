using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapStruct
{
    public class Map<T>
    {
        /// <summary>
        /// 扩展控制对象
        /// </summary>
        ExtensionController extensionController;

        /// <summary>
        /// 图的元素集合
        /// </summary>
        List<MapElement<T>> mapElements;

        public Map()
        {
            extensionController = new ExtensionController(this);
            mapElements = new List<MapElement<T>>();
        }

        #region 加载与保存
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="jsonValue"></param>
        public void Load(string jsonValue)
        {
            JsonSerializationStruct jsonStruct = DeSerializeNow<JsonSerializationStruct>(jsonValue);
            if (jsonStruct != null)
            {
                mapElements = jsonStruct.mapElements;
                jsonStruct.SetCorrelatives(this);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            JsonSerializationStruct jsonStruct = new JsonSerializationStruct(mapElements);
            string jsonValue = SerializeNow(jsonStruct);
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
        #endregion


        /// <summary>
        /// 获取或设置头元素
        /// </summary>
        public MapElement<T> FirstElement
        {
            get
            {
                return mapElements.FirstOrDefault();
            }
            set
            {
                if (mapElements.Contains(value))
                {
                    mapElements.Remove(value);
                    mapElements.Insert(0, value);
                }
            }
        }

        /// <summary>
        /// 通过ID查找图节点
        /// </summary>
        /// <param name="id">图元素的id</param>
        /// <returns></returns>
        public MapElement<T> GetElement(int id)
        {
            return mapElements.Where(temp => temp.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有图节点
        /// </summary>
        /// <returns></returns>
        public MapElement<T>[] GetElementAll()
        {
            return mapElements.ToArray();
        }

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <returns></returns>
        public MapElement<T> CreateMapElement()
        {
            MapElement<T> target = new MapElement<T>();
            target.LoadMapElement(this);
            target.SetID(extensionController.NextID());
            mapElements.Add(target);
            return target;
        }

        /// <summary>
        /// 获取扩展控制对象
        /// </summary>
        /// <returns></returns>
        internal ExtensionController GetExtensionController()
        {
            return extensionController;
        }

        /// <summary>
        /// 移除元素
        /// 注意这里移除后是整体的移除
        /// </summary>
        /// <param name="mapElement">元素</param>
        /// <returns>是否移除成功</returns>
        public bool Remove(MapElement<T> mapElement)
        {
            if (mapElements.Contains(mapElement))
            {
                //移除该节点的所有关系
                mapElement.CorrelativesNode.Clear();
                //移除该节点
                mapElements.Remove(mapElement);
            }
            return false;
        }

        /// <summary>
        /// 扩展管理器
        /// </summary>
        internal class ExtensionController
        {
            /// <summary>
            /// 图对象 
            /// </summary>
            Map<T> baseMap;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="map">图对象</param>
            internal ExtensionController(Map<T> map)
            {
                this.baseMap = map;
            }

            /// <summary>
            /// 获取下一个id
            /// </summary>
            /// <returns>返回id</returns>
            internal int NextID()
            {
                int nowMaxID = baseMap.mapElements.Count() > 0 ? baseMap.mapElements.Max(temp => temp.ID) : 0;
                return nowMaxID + 1;
            }

            /// <summary>
            /// 是否存在该对象 
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            internal bool Constains(MapElement<T> target)
            {
                return baseMap.mapElements.Contains(target);
            }
        }

        /// <summary>
        /// 用于json序列化以及反序列化的机构
        /// </summary>
        internal class JsonSerializationStruct
        {
            /// <summary>
            /// 图的元素集合
            /// </summary>
            public List<MapElement<T>> mapElements;
            /// <summary>
            /// 关系字典
            /// </summary>
            public Dictionary<int, int[]> correlativesDic;

            public JsonSerializationStruct()
            {
                mapElements = new List<MapElement<T>>();
                correlativesDic = new Dictionary<int, int[]>();
            }

            public JsonSerializationStruct(List<MapElement<T>> mapElements) : this()
            {
                this.mapElements = mapElements;
                GetCorrelatives();
            }

            /// <summary>
            /// 读取关系
            /// </summary>
            void GetCorrelatives()
            {
                foreach (MapElement<T> mapElement in mapElements)
                {
                    if (correlativesDic.ContainsKey(mapElement.ID))
                        continue;
                    if (mapElement.CorrelativesNode == null)
                        continue;
                    correlativesDic.Add(mapElement.ID, mapElement.CorrelativesNode.GetAll().Select(temp => temp.ID).ToArray());
                }
            }

            /// <summary>
            /// 设置关系
            /// 设置前请将mapElements复制给map的mapElements
            /// </summary>
            public void SetCorrelatives(Map<T> map)
            {
                foreach (MapElement<T> mapElement in mapElements)
                {
                    mapElement.LoadMapElement(map);//首先初始化数据
                }
                foreach (KeyValuePair<int, int[]> correlative in correlativesDic)
                {
                    MapElement<T> mapElement = mapElements.FirstOrDefault(temp => temp.ID == correlative.Key);
                    if (mapElement == null)
                        continue;
                    foreach (int targetID in correlative.Value)
                    {
                        MapElement<T> targetMapElement = mapElements.FirstOrDefault(temp => temp.ID == targetID);
                        if (targetMapElement == null)
                            continue;
                        mapElement.CorrelativesNode.Add(targetMapElement);
                    }
                }
            }
        }
    }

}
