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
    public string dataDirectoryPath = "";//@"E:\MyProject\Unity\InterwovenWithWorld\InterwovenWithWorld\Assets\Scripts\Data\Resources\Data\SkillSprite";
    //SkillSprite.txt   用于显示
    //SkillSprite_Combine.txt  用于组合

    [MenuItem("小工具/技能图标编辑器")]
    static void AddWindow()
    {
        SkillSpriteManagerEditor skillSpriteManagerEditor = EditorWindow.GetWindow<SkillSpriteManagerEditor>();
        skillSpriteManagerEditor.Show();
    }

    /// <summary>
    /// 技能类型关联技能图标的字典(用于显示)
    /// </summary>
    Dictionary<EnumSkillType, Sprite> skillTypeToSpriteShowDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典(用于显示)
    /// </summary>
    Dictionary<EnumSkillType, string> skillTypeToSpriteIDShowDic;

    /// <summary>
    /// 技能类型关联技能图标的字典(用于组合)
    /// </summary>
    Dictionary<EnumSkillType, Sprite> skillTypeToSpriteCombineDic;

    /// <summary>
    /// 技能类型关联技能图标ID的字典(用于组合)
    /// </summary>
    Dictionary<EnumSkillType, string> skillTypeToSpriteIDCombineDic;

    /// <summary>
    /// 技能对应技能说明字典
    /// </summary>
    Dictionary<EnumSkillType, string> skillTypeToExplanDic;

    private void Awake()
    {
        //重置路径
        dataDirectoryPath = Application.dataPath + @"\Scripts\Data\Resources\Data\SkillSprite";

        skillTypeToSpriteShowDic = new Dictionary<EnumSkillType, Sprite>();
        skillTypeToSpriteCombineDic = new Dictionary<EnumSkillType, Sprite>();
        skillTypeToExplanDic = new Dictionary<EnumSkillType, string>();
        if (!Directory.Exists(dataDirectoryPath))
        {
            Directory.CreateDirectory(dataDirectoryPath);
        }
        //用于显示
        if (!File.Exists(dataDirectoryPath + "/SkillSprite.txt"))
        {
            File.Create(dataDirectoryPath + "/SkillSprite.txt").Close();
            skillTypeToSpriteIDShowDic = new Dictionary<EnumSkillType, string>();
            File.WriteAllText(dataDirectoryPath + "/SkillSprite.txt", "{}", Encoding.UTF8);
        }
        else
        {
            string assetText = File.ReadAllText(dataDirectoryPath + "/SkillSprite.txt", Encoding.UTF8);
            skillTypeToSpriteIDShowDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
        }
        //用于组合
        if (!File.Exists(dataDirectoryPath + "/SkillSprite_Combine.txt"))
        {
            File.Create(dataDirectoryPath + "/SkillSprite_Combine.txt").Close();
            skillTypeToSpriteIDCombineDic = new Dictionary<EnumSkillType, string>();
            File.WriteAllText(dataDirectoryPath + "/SkillSprite_Combine.txt", "{}", Encoding.UTF8);
        }
        else
        {
            string assetText = File.ReadAllText(dataDirectoryPath + "/SkillSprite_Combine.txt", Encoding.UTF8);
            skillTypeToSpriteIDCombineDic = DeSerializeNow<Dictionary<EnumSkillType, string>>(assetText);
        }
        //检测是否增加或减少 
        EnumSkillType[] defaultEnumSKillTypes = Enum.GetValues(typeof(EnumSkillType)).OfType<EnumSkillType>().ToArray();
        //增加二阶技能的枚举
        EnumSkillType[] combineEnumSkillTypes = SkillCombineStaticTools.GetCombineSkillByCombineSkillIndex(2);
        defaultEnumSKillTypes = defaultEnumSKillTypes.Concat(combineEnumSkillTypes).Distinct().ToArray();
        foreach (EnumSkillType enumSkillType in defaultEnumSKillTypes)//检测增加量
        {
            if (!skillTypeToSpriteIDShowDic.ContainsKey(enumSkillType))
                skillTypeToSpriteIDShowDic.Add(enumSkillType, "");
            if (!skillTypeToSpriteIDCombineDic.ContainsKey(enumSkillType))
                skillTypeToSpriteIDCombineDic.Add(enumSkillType, "");
        }
        //显示
        EnumSkillType[] nowShowDicEnumSkillTypes = skillTypeToSpriteIDShowDic.Keys.OfType<EnumSkillType>().ToArray();
        foreach (EnumSkillType enumSkillType in nowShowDicEnumSkillTypes)//检测减少量
        {
            if (!defaultEnumSKillTypes.Contains(enumSkillType))
                skillTypeToSpriteIDShowDic.Remove(enumSkillType);
        }
        //组合
        EnumSkillType[] nowCombineDicEnumSkillTypes = skillTypeToSpriteIDCombineDic.Keys.OfType<EnumSkillType>().ToArray();
        foreach (EnumSkillType enumSkillType in nowCombineDicEnumSkillTypes)//检测减少量
        {
            if (!defaultEnumSKillTypes.Contains(enumSkillType))
                skillTypeToSpriteIDCombineDic.Remove(enumSkillType);
        }
        //根据当前id字典构建sprite字典(显示)
        foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToSpriteIDShowDic)
        {
            Sprite sprite = SpriteManager.GetSrpite(item.Value);
            skillTypeToSpriteShowDic.Add(item.Key, sprite);
        }
        //根据当前id字典构建sprite字典(组合)
        foreach (KeyValuePair<EnumSkillType, string> item in skillTypeToSpriteIDCombineDic)
        {
            Sprite sprite = SpriteManager.GetSrpite(item.Value);
            skillTypeToSpriteCombineDic.Add(item.Key, sprite);
        }
        //构建技能类型对应说明字典
        Type enumType = typeof(EnumSkillType);
        Func<EnumSkillType, string> GetSkillNameFunc = (skillType) =>
        {
            FieldInfo fieldInfo = enumType.GetField(skillType.ToString());
            if (fieldInfo != null)
            {
                FieldExplanAttribute fieldExplanAttribute = fieldInfo.GetCustomAttributes(typeof(FieldExplanAttribute), false).OfType<FieldExplanAttribute>().FirstOrDefault();
                if (fieldExplanAttribute != null)
                {
                    return fieldExplanAttribute.GetExplan();
                }
            }
            return null;
        };
        foreach (EnumSkillType enumSkillType in defaultEnumSKillTypes)
        {
            string skillName = GetSkillNameFunc(enumSkillType);
            if (skillName != null)
            {
                skillTypeToExplanDic.Add(enumSkillType, skillName);
            }
            else//这可能是组合技能
            {
                int key = (int)enumSkillType;
                if (key <= (int)EnumSkillType.MagicCombinedStart || key>=(int)EnumSkillType.EndMagic)//这不是组合技能
                    continue;
                EnumSkillType[] childSkills = SkillCombineStaticTools.GetCombineSkills(key);
                if (childSkills != null && childSkills.Length > 0)
                {
                    string names = "组合:";
                    childSkills.ToList().ForEach(temp => names += GetSkillNameFunc(temp) + "--");
                    skillTypeToExplanDic.Add(enumSkillType, names);
                }
                else
                {
                    skillTypeToExplanDic.Add(enumSkillType, "获取命名失败");
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
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        if (GUILayout.Button("保存"))
        {
            string valueText_Show = SerializeNow(skillTypeToSpriteIDShowDic);
            File.WriteAllText(dataDirectoryPath + "/SkillSprite.txt", valueText_Show, Encoding.UTF8);
            string valueText_Combine = SerializeNow(skillTypeToSpriteIDCombineDic);
            File.WriteAllText(dataDirectoryPath + "/SkillSprite_Combine.txt", valueText_Combine, Encoding.UTF8);
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
            EditorGUILayout.LabelField("显示图标:");
            {
                Sprite oldSprite = skillTypeToSpriteShowDic[selectEnumSkillType];
                Sprite sprite = (Sprite)EditorGUILayout.ObjectField(oldSprite, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
                if (!Sprite.Equals(oldSprite, sprite))
                {
                    string spriteID = SpriteManager.GetName(sprite);
                    skillTypeToSpriteShowDic[selectEnumSkillType] = sprite;
                    skillTypeToSpriteIDShowDic[selectEnumSkillType] = spriteID;
                }
            }
            EditorGUILayout.LabelField("组合图标:");
            {
                Sprite oldSprite = skillTypeToSpriteCombineDic[selectEnumSkillType];
                Sprite sprite = (Sprite)EditorGUILayout.ObjectField(oldSprite, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
                if (!Sprite.Equals(oldSprite, sprite))
                {
                    string spriteID = SpriteManager.GetName(sprite);
                    skillTypeToSpriteCombineDic[selectEnumSkillType] = sprite;
                    skillTypeToSpriteIDCombineDic[selectEnumSkillType] = spriteID;
                }
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
