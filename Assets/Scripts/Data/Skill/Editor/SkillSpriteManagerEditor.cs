using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 技能图标编辑器
/// </summary>
public class SkillSpriteManagerEditor : EditorWindow
{
    /// <summary>
    /// 保存信息的文件夹路径
    /// </summary>
    public string dataDirectoryPath = @"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\SkillSprite";
    //SkillSprite.txt

    [MenuItem("小工具/技能图标编辑器")]
    static void AddWindow()
    {
        SkillSpriteManagerEditor skillSpriteManagerEditor = EditorWindow.GetWindow<SkillSpriteManagerEditor>();
        skillSpriteManagerEditor.Show();
    }

    /// <summary>
    /// 技能类型关联技能图标的字典
    /// </summary>
    Dictionary<EnumSkillType, Sprite> skillTypeToSpriteDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典
    /// </summary>
    Dictionary<EnumSkillType, string> skillTypeToSpriteIDDic;

    /// <summary>
    /// 技能对应技能说明字典
    /// </summary>
    Dictionary<EnumSkillType, string> skillTypeToExplanDic;

    private void Awake()
    {
        skillTypeToSpriteDic = new Dictionary<EnumSkillType, Sprite>();
        skillTypeToExplanDic = new Dictionary<EnumSkillType, string>();
        if (!Directory.Exists(dataDirectoryPath))
        {
            Directory.CreateDirectory(dataDirectoryPath);
        }
        if (!File.Exists(dataDirectoryPath + "/SkillSprite.txt"))
        {
            File.Create(dataDirectoryPath + "/SkillSprite.txt");
            skillTypeToSpriteIDDic = new Dictionary<EnumSkillType, string>();
            File.WriteAllText(dataDirectoryPath + "/SkillSprite.txt", "{}", Encoding.UTF8);
        }
        else
        {
            string assetText = File.ReadAllText(dataDirectoryPath + "/SkillSprite.txt", Encoding.UTF8);
            skillTypeToSpriteIDDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
        }
        //检测是否增加或减少 
        EnumSkillType[] defaultEnumSKillTypes = Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().ToArray();
        foreach (EnumSkillType enumSkillType in defaultEnumSKillTypes)//检测增加量
        {
            if (!skillTypeToSpriteIDDic.ContainsKey(enumSkillType))
                skillTypeToSpriteIDDic.Add(enumSkillType, "");
        }
        EnumSkillType[] nowDicEnumSkillTypes = skillTypeToSpriteIDDic.Keys.OfType<EnumSkillType>().ToArray();
        foreach (EnumSkillType enumSkillType in nowDicEnumSkillTypes)//检测减少量
        {
            if (!defaultEnumSKillTypes.Contains(enumSkillType))
                skillTypeToSpriteIDDic.Remove(enumSkillType);
        }
        //根据当前id字典构建sprite字典
        foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToSpriteIDDic)
        {
            Sprite sprite = SpriteManager.GetSrpite(item.Value);
            skillTypeToSpriteDic.Add(item.Key, sprite);
        }
        //构建技能类型对应说明字典
        Type enumType = typeof(EnumSkillType);
        foreach (EnumSkillType enumSkillType in defaultEnumSKillTypes)
        {
            FieldInfo fieldInfo = enumType.GetField(enumSkillType.ToString());
            if (fieldInfo != null)
            {
                FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                if (fieldExplanAttribute != null)
                {
                    skillTypeToExplanDic.Add(enumSkillType, fieldExplanAttribute.GetExplan());
                }
            }
        }
    }

    private void OnInspectorUpdate()
    {
        this.Repaint();
    }


    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="target">对象</param>
    /// <returns>返回的字符串</returns>
    public string SerializeNow<T>(T target) where T : class
    {
        if (target == null)
            return "";
        string value = JsonConvert.SerializeObject(target);
        return value;
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T">反序列化后的类型</typeparam>
    /// <param name="value">字符串</param>
    /// <returns>对象</returns>
    public T DeSerializeNow<T>(string value) where T : class
    {
        T target = JsonConvert.DeserializeObject<T>(value);
        return target;
    }

    /// <summary>
    /// 左侧的滑动条
    /// </summary>
    Vector2 scrollLeft;

    /// <summary>
    /// 选择的技能类型 
    /// </summary>
    EnumSkillType selectEnumSkillType;

    void OnGUI()
    {
        if (skillTypeToExplanDic == null)
            return;
        EditorGUILayout.BeginHorizontal();
        //左侧选择技能
        EditorGUILayout.BeginVertical(GUILayout.Width(150));
        if (GUILayout.Button("保存"))
        {
            string valueText = SerializeNow(skillTypeToSpriteIDDic);
            File.WriteAllText(dataDirectoryPath + "/SkillSprite.txt", valueText, Encoding.UTF8);
            EditorUtility.DisplayDialog("保存数据", "保存成功!", "确认");
        }
        EditorGUILayout.LabelField("技能:");
        scrollLeft = EditorGUILayout.BeginScrollView(scrollLeft);
        foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToExplanDic)
        {
            int offsetX = 0;
            if (item.Key == selectEnumSkillType)
                offsetX = 20;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offsetX);
            if (GUILayout.Button(item.Value))
            {
                selectEnumSkillType = item.Key;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        //右侧显示图片(或粒子?)
        //图片
        if (skillTypeToExplanDic.ContainsKey(selectEnumSkillType))
        {
            EditorGUILayout.LabelField("图标:");
            Sprite oldSprite = skillTypeToSpriteDic[selectEnumSkillType];
            Sprite sprite = (Sprite)EditorGUILayout.ObjectField(oldSprite, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
            if (!Sprite.Equals(oldSprite, sprite))
            {
                string spriteID = SpriteManager.GetName(sprite);
                skillTypeToSpriteDic[selectEnumSkillType] = sprite;
                skillTypeToSpriteIDDic[selectEnumSkillType] = spriteID;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
