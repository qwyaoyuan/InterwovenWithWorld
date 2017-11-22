using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

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

    ///// <summary>
    ///// 数据中心单例对象
    ///// </summary>
    public static DataCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataCenter();
                instance.LoadArchive(0);
            }
            if (instance.allCanLoadable.Count <= 0)
            {
                instance.LoadMetaData();
                instance.LoadAllArchiveInfo();
            }
            return instance;
        }
        private set
        {
            instance = value;
            if (instance.allCanLoadable.Count <= 0)
            {
                instance.LoadMetaData();
                instance.LoadAllArchiveInfo();
            }
        }

    }

    /// <summary>
    /// 加载所有存档信息
    /// </summary>
    private void LoadAllArchiveInfo()
    {
        string archiveInfoDir = Path.Combine(baseDataPath, "allArchiveInfo");
        if (File.Exists(archiveInfoDir))
        {
            allArchiveBaseInfo = JsonConvert.DeserializeObject<List<Archive>>(File.ReadAllText(archiveInfoDir));
        }
    }


    /// <summary>
    /// 可加载元数据集合
    /// </summary>
    [JsonIgnore]
    private Dictionary<Type, ILoadBase> allCanLoadable = new Dictionary<Type, ILoadBase>();


    /// <summary>
    /// 存档路径
    /// </summary>
    [JsonIgnore]
    private string archivePath = "Archive/";

    /// <summary>
    /// 存档文件后缀
    /// </summary>
    [JsonIgnore]
    private string archiveExt = ".archive";

    /// <summary>
    /// 基础数据路径
    /// </summary>
    [JsonIgnore]
    private string baseDataPath = "Archive/BaseData/";

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



    [JsonIgnore]
    private FieldInfo[] Fields;


    /// <summary>
    /// 缓存字段引用
    /// </summary>
    [JsonIgnore]
    private Dictionary<FieldInfo, System.Object> cachedEntityFields = new Dictionary<FieldInfo, System.Object>();

    /// <summary>
    /// 获取某元数据,所有实现自ILoadable<>的类,目前有DialogueStructData,GoodsMetaInfoMations,SkillStructData,SynthesisStructData,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMetaData<T>()
    {
        if (allCanLoadable.Keys.Contains(typeof(T)))
            return (T)(System.Object)(allCanLoadable[typeof(T)]);
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

        if (Fields == null)
        {
            Fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        }
        FieldInfo filterField = Fields.SingleOrDefault(f => f.FieldType.Equals(typeof(T)));
        if (filterField == null)
            return default(T);
        if (!cachedEntityFields.Keys.Contains(filterField))
        {
            object fieldVal = filterField.GetValue(this);
            cachedEntityFields.Add(filterField, fieldVal);
        }
        return (T)cachedEntityFields[filterField];
    }


    /// <summary>
    /// 保存一份存档,新id首次存档必须填写名字和介绍。
    /// 覆盖存档如果不写名字和介绍则复用以前的。如果写了则覆盖。
    /// </summary>
    /// <param name="archiveName"></param>
    /// <param name="archiveIntro"></param>
    public void Save(int id, string archiveName = "", string archiveIntro = "")
    {
        if (id == 0)
        {
            Debug.LogError("0号存档禁用");
            return;
        }

        //new id
        if (allArchiveBaseInfo.TrueForAll(a => a.ID != id))
        {
            if (string.IsNullOrEmpty(archiveName) || string.IsNullOrEmpty(archiveName))
            {
                UnityEngine.Debug.LogError("新id首次存档名称和介绍必须提供,存档失败");
                return;
            }

            allArchiveBaseInfo.Add(new Archive(id, archiveName, archiveIntro) { LastedModifiedTime = DateTime.Now });
        }
        //已存在id
        else
        {
            Archive saveToArchive = allArchiveBaseInfo.Single(a => a.ID == id);
            if (!string.IsNullOrEmpty(archiveName))
                saveToArchive.Name = archiveName;
            if (!string.IsNullOrEmpty(archiveIntro))
                saveToArchive.ArchiveIntro = archiveIntro;
            saveToArchive.LastedModifiedTime = DateTime.Now;

        }
        //存档
        string json = JsonConvert.SerializeObject(this, new JsonSerializerSettings() { ContractResolver = new MyContractResolver(), PreserveReferencesHandling = PreserveReferencesHandling.Objects });

        File.WriteAllText(Path.Combine(archivePath, id + archiveExt), json);

        CheckPhysisArchives();

        //保存目前的所有存档信息
        string archiveInfo = JsonConvert.SerializeObject(allArchiveBaseInfo);
        File.WriteAllText(Path.Combine(baseDataPath, "allArchiveInfo"), archiveInfo);

    }


    private void CheckPhysisArchives()
    {


        var allPhysisArchives = Directory.GetFiles(archivePath, "*" + archiveExt, SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToList();
        for (int i = 0; i < allArchiveBaseInfo.Count; i++)
        {
            var archive = allArchiveBaseInfo[i];
            //如果物理磁盘中已经不包含存档,删除内存中记录
            if (!allPhysisArchives.Contains(archive.ID.ToString()))
            {
                allArchiveBaseInfo.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// 所有存档的基本信息
    /// </summary>
    [JsonIgnore]
    private List<Archive> allArchiveBaseInfo = new List<Archive>();

    /// <summary>
    /// 获取当前所有存档名称
    /// </summary>
    /// <returns></returns>
    public List<Archive> GetAllArchive()
    {
        CheckPhysisArchives();
        return allArchiveBaseInfo.Where(a => a.ID != 0).ToList();
    }


    /// <summary>
    /// 加载存档
    /// </summary>
    /// <param name="archiveName"></param>
    public void LoadArchive(int id)
    {
        string archiveFile = Path.Combine(archivePath, id + archiveExt);
        if (id == 0)
        {
            archiveFile = Path.Combine(baseDataPath, id + archiveExt);
        }
        if (File.Exists(archiveFile))
        {
            Instance = JsonConvert.DeserializeObject<DataCenter>(File.ReadAllText(archiveFile), new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ContractResolver = new MyContractResolver() });
        }
        else
        {
            UnityEngine.Debug.LogError("存档不存在");
        }
    }

}


/// <summary>
/// 存档基本结构
/// </summary>
public class Archive
{
    /// <summary>
    /// 存档id
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 存档名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 存档简介
    /// </summary>
    public string ArchiveIntro { get; set; }

    /// <summary>
    /// 最后更改时间
    /// </summary>

    public DateTime LastedModifiedTime { get; set; }

    public Archive()
    {

    }

    public Archive(int id, string name, string intro)
    {
        this.ID = id;
        this.Name = name;
        this.ArchiveIntro = intro;
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






