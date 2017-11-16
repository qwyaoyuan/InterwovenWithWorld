using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;
/// <summary>
/// DataCenter为partial类,此部分负责功能,另一部分为所有实体
/// </summary>
public partial class DataCenter
{
    /*                                    
     当前载入的存档:提供各种接口可以访问存档的指定数据    | GetEntity<T>获取玩家的个人相关数据
     提供保存当前状态                                   |Save(string archiveName)
     提供加载存档                                       |LoadArchive(string archiveName)   
     静态的数据：各种预先编辑好的数据                    |GetMetaData<T>获取各种编辑好的元数据
     提供主角设置与读取当前的状态（各个装备以及自身各自的状态，buff状态，debuff状态）
     支持升级兼容                                       |向后兼容->  向后版本可增加字段以及删除字段,若增加字段,反序列化至以前版本则多余字段忽略,以前版本存档开启新版本则多余字段全部为默认值。
     */


    private static DataCenter instance = null;

    /// <summary>
    /// 数据中心单例对象
    /// </summary>
    public static DataCenter Instance
    {
        get
        {

            if (instance == null)
            {
                instance = new DataCenter();
            }
            if (instance.allCanLoadable.Count <= 0)
                instance.LoadMetaData();
            return instance;
        }
        private set
        {
            instance = value;
            if (instance.allCanLoadable.Count <= 0)
                instance.LoadMetaData();
        }

    }



    public DataCenter()
    {
        //PlayerState = new PlayerState();
        //EquipmentState = new EquipmentState();
        //BuffState = new BuffState();
        //DebuffState = new DebuffState();
    }

    /// <summary>
    /// 可加载元数据集合
    /// </summary>
    private Dictionary<Type, ILoadBase> allCanLoadable = new Dictionary<Type, ILoadBase>();


    /// <summary>
    /// 存档路径
    /// </summary>
    private string archivePath = "Archive/";

    /// <summary>
    /// 存档文件后缀
    /// </summary>
    private string archiveExt = ".archive";

    /// <summary>
    /// 加载所有元数据,即所有编辑器编辑好的数据
    /// </summary>
    private void LoadMetaData()
    {
        foreach (Type t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ILoadable<>))))
        {
            Object subClassInstance = Activator.CreateInstance(t);

            allCanLoadable.Add(t, (ILoadBase)subClassInstance);
        }
        foreach (var item in allCanLoadable.Values)
        {
            item.Load();
        }
    }



    /// <summary>
    /// 当前实体的所有propertyInfo
    /// </summary>
    private PropertyInfo[] properties;

    /// <summary>
    /// 缓存字段引用
    /// </summary>
    private Dictionary<PropertyInfo, Object> cachedEntityProperties = new Dictionary<PropertyInfo, Object>();


    /// <summary>
    /// 获取某元数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMetaData<T>()
    {
        if (allCanLoadable.Keys.Contains(typeof(T)))
            return (T)(Object)(allCanLoadable[typeof(T)]);
        else
            return default(T);
    }


    /// <summary>
    /// 获取实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetEntity<T>()
    {
        if (properties == null)
        {
            properties = this.GetType().GetProperties();
        }
        PropertyInfo filterProperty = properties.SingleOrDefault(p => p.PropertyType.Equals(typeof(T)));
        if (filterProperty == null)
            return default(T);
        if (!cachedEntityProperties.Keys.Contains(filterProperty))
        {
            object propertyVal = filterProperty.GetValue(this, null);
            cachedEntityProperties.Add(filterProperty, propertyVal);
        }
        return (T)cachedEntityProperties[filterProperty];
    }


    /// <summary>
    /// 保存当前的状态至名称为archiveName的存档中
    /// </summary>
    public void Save(string archiveName)
    {
        string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
        File.WriteAllText(Path.Combine(archivePath, archiveName + archiveExt), json);
    }



    /// <summary>
    /// 获取当前所有存档名称
    /// </summary>
    /// <returns></returns>
    public List<String> GetAllArchive()
    {
        if (!Directory.Exists(archivePath)) Directory.CreateDirectory(archivePath);
        string[] archives = Directory.GetFiles(archivePath, "*" + archiveExt);
        List<string> archiveNames = archives.Select(archivePath => Path.GetFileNameWithoutExtension(archivePath)).ToList();
        return archiveNames;
    }


    /// <summary>
    /// 加载存档
    /// </summary>
    /// <param name="archiveName"></param>
    public void LoadArchive(string archiveName)
    {
        string archiveFile = Path.Combine(archivePath, archiveName + archiveExt);
        if (File.Exists(archiveFile))
        {
            Instance = JsonConvert.DeserializeObject<DataCenter>(File.ReadAllText(archiveFile));
        }
    }

}


public interface ILoadBase
{
    void Load();
}


/// <summary>
/// 可加载的插件接口
/// </summary>
public interface ILoadable<T> : ILoadBase where T : new()
{

}






