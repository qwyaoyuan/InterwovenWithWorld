using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// �������� (���������Ȳ�Ҫ��,�ȱ༭������)
/// </summary>
public enum EnumGoodsType
{
    #region �ز���
    /// <summary>
    /// �ز������ʼ
    /// </summary>
    [FieldExplan("�ز�")]
    SourceMaterial = 1000000,

    #region ��ʯ����
    /// <summary>
    /// ��ʯ����
    /// </summary>
    [FieldExplan("��ʯ����")]
    MineralBig = 1100000,

    #region ��ʯ
    /// <summary>
    /// ��ʯ
    /// </summary>
    [FieldExplan("��ʯ")]
    MineralLittle = 1110000,

    #region ����Ŀ�ʯ
    /// <summary>
    /// ����ʯ
    /// </summary>
    [FieldExplan("����ʯ")]
    TieKuangShi = 1110001,
    #endregion

    #endregion

    #region ��ʯ
    /// <summary>
    /// ��ʯ
    /// </summary>
    [FieldExplan("��ʯ ")]
    Gemston = 1111000,

    #region ����ı�ʯ

    #endregion

    #endregion

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    IngotCasting = 1112000,

    #region ���������
    /// <summary>
    /// ë��
    /// </summary>
    [FieldExplan("ë��")]
    MaoPei = 1112001,
    #endregion

    #endregion

    #endregion

    #region �����ز���
    /// <summary>
    /// �����ز�
    /// </summary>
    [FieldExplan("�����ز�")]
    BiologyMaterial = 1200000,

    #region ��Ƥ
    /// <summary>
    /// ��Ƥ 
    /// </summary>
    [FieldExplan("��Ƥ")]
    Hide = 1210000,

    #region �������Ƥ 

    #endregion

    #endregion

    #region ��צ 
    /// <summary>
    /// ��צ
    /// </summary>
    [FieldExplan("��צ")]
    KemonoZume = 1211000,

    #region �������צ

    #endregion

    #endregion

    #region �޹� 
    /// <summary>
    /// �޹�
    /// </summary>
    [FieldExplan("�޹�")]
    AnimalBone = 1212000,

    #region ������޹� 

    #endregion

    #endregion

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    Meat = 1213000,

    #region  ���������
    #endregion

    #endregion

    #endregion

    #region ��Ȼ�ز���
    /// <summary>
    /// ��Ȼ�ز��� 
    /// </summary>
    [FieldExplan("��Ȼ�ز�")]
    NaturalMaterial = 1300000,

    #region ��ҩ
    /// <summary>
    /// ��ҩ
    /// </summary>
    HerbalMedicine = 1310000,

    #region ����Ĳ�ҩ

    #endregion

    #endregion

    #region ľ��
    /// <summary>
    /// ľ��
    /// </summary>
    [FieldExplan("ľ��")]
    Wood = 1311000,

    #region �����ľ��

    #endregion

    #endregion

    #region ��ʵ
    /// <summary>
    /// ��ʵ
    /// </summary>
    [FieldExplan("��ʵ")]
    Fruit = 1312000,

    #region ����Ĺ�ʵ

    #endregion

    #endregion

    #endregion

    #region �����ز���
    /// <summary>
    /// �����ز���
    /// </summary>
    [FieldExplan("�����ز�")]
    SpecialMaterial = 1400000,

    #region ���������ز���
    /// <summary>
    /// ���������ز�
    /// </summary>
    [FieldExplan("���������ز�")]
    DropSpecialMaterial = 1410000,

    #region ����ĵ��������ز�

    #endregion

    #endregion

    #region ���������ز���
    /// <summary>
    /// ���������ز�
    /// </summary>
    [FieldExplan("���������ز�")]
    TaskSpecialMaterial = 1411000,

    #region ��������������ز�

    #endregion

    #endregion

    #endregion

    #endregion

    #region װ����
    /// <summary>
    /// װ����
    /// </summary>
    [FieldExplan("װ��")]
    Equipment = 2000000,

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("����")]
    Arms = 2100000,

    #region ���ֽ�
    /// <summary>
    /// ���ֽ�
    /// </summary>
    [FieldExplan("���ֽ�")]
    SingleHanedSword = 2110000,

    #region ����ĵ��ֽ�
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    TieJian = 2110001,
    #endregion

    #endregion

    #region ˫�ֽ� 
    /// <summary>
    /// ˫�ֽ�
    /// </summary>
    [FieldExplan("˫�ֽ�")]
    TwoHandedSword = 2111000,

    #region �����˫�ֽ�

    #endregion

    #endregion

    #region ��
    /// <summary>
    /// ��
    /// </summary>
    [FieldExplan("��")]
    Arch = 2112000,

    #region ����Ĺ� 

    #endregion

    #endregion

    #region �� 
    /// <summary>
    /// ��
    /// </summary>
    [FieldExplan("��")]
    CrossBow = 2113000,

    #region �������

    #endregion

    #endregion

    #region ��
    /// <summary>
    /// ��
    /// </summary>
    [FieldExplan("����Ķ�")]
    Shield = 2114000,

    #region ����Ķ�

    #endregion

    #endregion

    #region ذ��
    /// <summary>
    /// ذ��
    /// </summary>
    [FieldExplan("ذ��")]
    Dagger = 2115000,

    #region �����ذ��

    #endregion

    #endregion

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    LongRod = 2116000,

    #region ����ĳ���

    #endregion

    #endregion

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    ShortRod = 2117000,

    #region ����Ķ��� 

    #endregion

    #endregion

    #region ˮ����
    /// <summary>
    /// ˮ���� 
    /// </summary>
    [FieldExplan("ˮ����")]
    CrystalBall = 2118000,

    #region �����ˮ����

    #endregion

    #endregion

    #endregion

    #region ͷ������
    /// <summary>
    /// ͷ������
    /// </summary>
    [FieldExplan("ͷ������")]
    HelmetBig = 2200000,

    #region ͷ��
    /// <summary>
    /// ͷ��
    /// </summary>
    [FieldExplan("ͷ��")]
    HelmetLittle = 2210000,

    #region �����ͷ��

    #endregion

    #endregion

    #region ͷ��
    /// <summary>
    /// ͷ��
    /// </summary>
    [FieldExplan("ͷ��")]
    HeadRing = 2211000,

    #region �����ͷ��
    #endregion

    #endregion

    #region ��ñ
    /// <summary>
    /// ��ñ
    /// </summary>
    [FieldExplan("��ñ")]
    Hood = 2212000,

    #region ����Ķ�ñ

    #endregion

    #endregion

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("����")]
    Armor = 2300000,

    #region �ؼ� 
    /// <summary>
    /// �ؼ�
    /// </summary>
    [FieldExplan("�ؼ�")]
    HeavyArmor = 2310000,

    #region ������ؼ�

    #endregion

    #endregion

    #region Ƥ��
    /// <summary>
    /// Ƥ��
    /// </summary>
    [FieldExplan("Ƥ��")]
    LeatherArmor = 2311000,

    #region �����Ƥ��

    #endregion

    #endregion

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    Robe = 2312000,

    #region ����ķ���

    #endregion

    #endregion

    #endregion

    #region Ь����
    /// <summary>
    /// Ь����
    /// </summary>
    [FieldExplan("Ь��")]
    Shoes = 2400000,

    #region ��ѥ
    /// <summary>
    /// ��ѥ
    /// </summary>
    [FieldExplan("��ѥ")]
    ArmoredBoots = 2410000,

    #region ����ļ�ѥ

    #endregion

    #endregion

    #region Ƥѥ
    /// <summary>
    /// Ƥѥ
    /// </summary>
    [FieldExplan("Ƥѥ")]
    Boots = 2411000,

    #region �����Ƥѥ

    #endregion

    #endregion

    #region ��Ь
    /// <summary>
    /// ��Ь
    /// </summary>
    [FieldExplan("��Ь")]
    ClothShoes = 2412000,

    #region ����Ĳ�Ь

    #endregion

    #endregion

    #endregion

    #region ��Ʒ��
    /// <summary>
    /// ��Ʒ��
    /// </summary>
    [FieldExplan("��Ʒ")]
    Ornaments = 2500000,

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("����")]
    Necklace = 2510000,

    #region ���������

    #endregion

    #endregion

    #region ��ָ
    /// <summary>
    /// ��ָ
    /// </summary>
    [FieldExplan("��ָ")]
    Ring = 2511000,

    #region ����Ľ�ָ
    #endregion

    #endregion

    #region �����
    /// <summary>
    /// �����
    /// </summary>
    [FieldExplan("�����")]
    Amulet = 2512000,

    #region ����Ļ����

    #endregion

    #endregion

    #region ѫ��
    /// <summary>
    /// ѫ��
    /// </summary>
    [FieldExplan("ѫ��")]
    Medal = 2513000,

    #region �����ѫ�� 

    #endregion

    #endregion

    #endregion

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("����")]
    Item = 3000000,

    #region Ͷ��������
    /// <summary>
    /// Ͷ��������
    /// </summary>
    [FieldExplan("Ͷ������")]
    ThrowItem = 3100000,

    #region ը���� 
    /// <summary>
    /// ը����
    /// </summary>
    [FieldExplan("ը��")]
    Bomb = 3110000,

    #region �����ը��

    #endregion

    #endregion

    #region ���е�����
    /// <summary>
    /// ���е�����
    /// </summary>
    [FieldExplan("���е���")]
    FlyItem = 3111000,

    #region ����ķ��е���

    #endregion

    #endregion

    #endregion

    #region ħż��
    /// <summary>
    /// ħż��
    /// </summary>
    [FieldExplan("ħż")]
    Golem = 3200000,

    #region ������ħż
    /// <summary>
    /// ������ħż
    /// </summary>
    [FieldExplan("������ħż")]
    CanMakeGolem = 3210000,

    #region ����Ŀ�����ħż

    #endregion

    #endregion

    #region ��������ħż
    /// <summary>
    /// ��������ħż
    /// </summary>
    [FieldExplan("��������ħż")]
    CannotMakeGolem = 3211000,

    #region ����Ĳ�������ħż

    #endregion

    #endregion

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("����")]
    Trap = 3300000,

    #region �̶�������
    /// <summary>
    /// �̶�������
    /// </summary>
    [FieldExplan("�̶�������")]
    FixedTrap = 3310000,

    #region ����Ĺ̶������� 

    #endregion

    #endregion

    #region �����÷�����
    /// <summary>
    /// �����÷�����
    /// </summary>
    [FieldExplan("�����÷�����")]
    SpecialTrap = 3311000,

    #region ����������÷�����

    #endregion

    #endregion

    #endregion

    #endregion

    #region ����ҩ����
    /// <summary>
    /// ����ҩ����
    /// </summary>
    [FieldExplan("����ҩ��")]
    Elixir = 4000000,

    #region ����
    /// <summary>
    /// ����
    /// </summary>
    [FieldExplan("��")]
    Wine = 4100000,

    #region ��ͨ��
    /// <summary>
    /// ��ͨ�� 
    /// </summary>
    [FieldExplan("��ͨ��")]
    NormalWine = 4110000,

    #region �������ͨ��

    #endregion

    #endregion

    #endregion

    #region �ָ�ҩ������
    /// <summary>
    /// �ָ�ҩ������
    /// </summary>
    [FieldExplan("�ָ�ҩ������")]
    RestorativeDrugsBig = 4200000,

    #region �ָ�ҩ��
    /// <summary>
    /// �ָ�ҩ��
    /// </summary>
    [FieldExplan("�ָ�ҩ��")]
    RestorativeDrugsLittle = 4210000,

    #region ����Ļظ�ҩ��

    #endregion

    #endregion

    #endregion

    #region ǿ��ҩ������
    /// <summary>
    /// ǿ��ҩ������
    /// </summary>
    [FieldExplan("ǿ��ҩ������")]
    StrengthenAgentBig = 4300000,

    #region ǿ��ҩ�� 
    /// <summary>
    /// ǿ��ҩ��
    /// </summary>
    [FieldExplan("ǿ��ҩ��")]
    StrengthenAgentLittle = 4310000,

    #region �����ǿ��ҩ��

    #endregion

    #endregion

    #endregion

    #region ��ħҩ������
    /// <summary>
    /// ��ħҩ������
    /// </summary>
    [FieldExplan("��ħҩ������")]
    EnchantElixirBig = 4400000,

    #region ��ħҩ�� 
    /// <summary>
    /// ��ħҩ��
    /// </summary>
    [FieldExplan("��ħҩ��")]
    EnchantElixirLittle = 4410000,

    #region ����ĸ�ħҩ��

    #endregion

    #endregion

    #endregion

    #region ����ҩ������
    /// <summary>
    /// ����ҩ������
    /// </summary>
    [FieldExplan("����ҩ������")]
    FunctionalAgnetBig = 4500000,

    #region ����ҩ��
    /// <summary>
    /// ����ҩ��
    /// </summary>
    [FieldExplan("����ҩ��")]
    FunctionalAgnetLittle = 4510000,

    #region ����Ĺ���ҩ��

    #endregion

    #endregion

    #endregion

    #endregion

    #region ������Ʒ��
    /// <summary>
    /// ������Ʒ��
    /// </summary>
    [FieldExplan("������Ʒ")]
    SpecialItem = 5000000,

    #region �鼮��
    /// <summary>
    /// �鼮��
    /// </summary>
    [FieldExplan("�鼮")]
    Book = 5100000,

    #region ������ 
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("������")]
    StoryBook = 5110000,

    #region ����Ĺ�����

    #endregion

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    [FieldExplan("������")]
    SkillBook = 5111000,

    #region ����ļ�����

    #endregion

    #endregion

    #region �ż�
    /// <summary>
    /// �ż�
    /// </summary>
    [FieldExplan("�ż�")]
    Letterhead = 5112000,

    #region ������ż�

    #endregion

    #endregion

    #endregion

    #endregion

}

/// <summary>
/// ���߸�����
/// </summary>
public static class GoodsStaticTools
{
    /// <summary>
    /// ���߸��ڵ�
    /// </summary>
    static GoodsNode root;
    /// <summary>
    /// �����������ڵ��ֵ�(�����ѯ)
    /// </summary>
    static Dictionary<EnumGoodsType, GoodsNode> goodsNodeDic;

    static GoodsStaticTools()
    {
        InitGoodsNode();
    }

    /// <summary>
    /// �ж��Ƿ���˫������
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsTwoHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.TwoHandedSword) ||//˫�ֽ�
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Arch) ||//��
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.CrossBow) ||//��
                        GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.LongRod);//����
    }

    /// <summary>
    /// �ж��Ƿ����������� 
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsRightOneHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.SingleHanedSword) ||//���ֽ� 
            GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Dagger) ||//ذ��
             GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.ShortRod);//����
    }

    /// <summary>
    /// �ж��Ƿ��Ǹ�������
    /// </summary>
    /// <param name="enumGoodsType"></param>
    /// <returns></returns>
    public static bool IsLeftOneHandedWeapon(EnumGoodsType enumGoodsType)
    {
        return GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.Shield) ||//����
                     GoodsStaticTools.IsChildGoodsNode(enumGoodsType, EnumGoodsType.CrystalBall);//ˮ���� 
    }

    /// <summary>
    /// ��ʼ���ڵ�
    /// </summary>
    static void InitGoodsNode()
    {
        root = new GoodsNode();
        goodsNodeDic = new Dictionary<EnumGoodsType, GoodsNode>();
        int layer1 = 1000000;
        int layer2 = 100000;
        int layer3 = 1000;
        var tempDataStruct = Enum.GetValues(typeof(EnumGoodsType)).OfType<EnumGoodsType>()
            .Select(temp => new { type = temp, value = (int)temp })
            .OrderBy(temp => temp.value)
            .ToArray();
        root.Childs = new List<GoodsNode>();
        for (int i = 0; i < 10; i++)//��һ��
        {
            #region ��һ��
            int layer1Min = layer1 * i;
            int layer1Max = layer1 * (i + 1);
            var layer1TempDataStruct = tempDataStruct.Where(temp => temp.value >= layer1Min && temp.value < layer1Max)
                .Select(temp => new { type = temp.type, value = temp.value % layer1, baseValue = temp.value })
                .ToArray();
            if (layer1TempDataStruct.Length == 0)
                continue;
            GoodsNode layer1TreeNode = new GoodsNode();
            layer1TreeNode.Childs = new List<GoodsNode>();
            layer1TreeNode.GoodsType = layer1TempDataStruct[0].type;
            goodsNodeDic.Add(layer1TreeNode.GoodsType, layer1TreeNode);
            root.Childs.Add(layer1TreeNode);
            layer1TreeNode.Parent = root;
            #endregion
            for (int j = 0; j < 10; j++)//�ڶ���
            {
                #region �ڶ���
                int layer2Min = layer2 * j;
                int layer2Max = layer2 * (j + 1);
                var layer2TempDataStruct = layer1TempDataStruct.Where(temp => temp.value > layer2Min && temp.value < layer2Max)
                    .Select(temp => new { type = temp.type, value = temp.value % layer2, baseValue = temp.baseValue })
                    .ToArray();
                if (layer2TempDataStruct.Length == 0)
                    continue;
                GoodsNode layer2TreeNode = new GoodsNode();
                layer2TreeNode.Childs = new List<GoodsNode>();
                layer2TreeNode.GoodsType = layer2TempDataStruct[0].type;
                goodsNodeDic.Add(layer2TreeNode.GoodsType, layer2TreeNode);
                layer1TreeNode.Childs.Add(layer2TreeNode);
                layer2TreeNode.Parent = layer1TreeNode;
                #endregion
                for (int k = 0; k < 100; k++)//������
                {
                    #region ������
                    int layer3Min = layer3 * k;
                    int layer3Max = layer3 * (k + 1);
                    var layer3TempDataStruct = layer2TempDataStruct.Where(temp => temp.value >= layer3Min && temp.value < layer3Max)
                        .Select(temp => new { type = temp.type, value = temp.value % layer3, baseValue = temp.baseValue })
                        .ToArray();
                    if (layer3TempDataStruct.Length == 0)
                        continue;
                    GoodsNode layer3TreeNode = new GoodsNode();
                    layer3TreeNode.Childs = new List<GoodsNode>();
                    layer3TreeNode.GoodsType = layer3TempDataStruct[0].type;
                    goodsNodeDic.Add(layer3TreeNode.GoodsType, layer3TreeNode);
                    layer2TreeNode.Childs.Add(layer3TreeNode);
                    layer3TreeNode.Parent = layer2TreeNode;
                    #endregion
                    for (int l = 1; l < layer3TempDataStruct.Length; l++)//���Ĳ� 
                    {
                        #region ���Ĳ�
                        GoodsNode layer4TreeNode = new GoodsNode();
                        layer4TreeNode.Childs = new List<GoodsNode>();
                        layer4TreeNode.GoodsType = layer3TempDataStruct[l].type;
                        goodsNodeDic.Add(layer4TreeNode.GoodsType, layer4TreeNode);
                        layer3TreeNode.Childs.Add(layer4TreeNode);
                        layer4TreeNode.Parent = layer3TreeNode;
                        #endregion
                    }
                }
            }
        }
    }

    /// <summary>
    /// ��ȡָ����Ʒ�Ĵ���
    /// </summary>
    /// <param name="child">�ӽڵ�</param>
    /// <param name="deep">�������,Ĭ��Ϊ1(�����²��ϵ )</param>
    /// <returns></returns>
    public static EnumGoodsType? GetParentGoodsType(EnumGoodsType child, int deep = 1)
    {
        GoodsNode childNode = GetGoodNode(child);
        if (childNode == null)
            return null;
        GoodsNode parentNode = childNode;
        while (deep > 0 && parentNode != null)
        {
            deep--;
            parentNode = parentNode.Parent;
        }
        if (parentNode != null)
            return parentNode.GoodsType;
        return null;
    }

    /// <summary>
    /// �жϸ����ĸ��ӹ�ϵ�Ƿ����
    /// </summary>
    /// <param name="child">�ӽڵ�</param>
    /// <param name="parent">���ڵ�</param>
    /// <param name="ignoreDeep">�Ƿ�������,�����������������֮��Ľڵ�</param>
    /// <returns></returns>
    public static bool IsChildGoodsNode(EnumGoodsType child, EnumGoodsType parent, bool ignoreDeep = true)
    {
        if ((int)child < (int)parent)
            return false;
        if ((int)child - (int)parent > 1000000)
            return false;
        GoodsNode childNode = GetGoodNode(child);
        GoodsNode parentNode = GetGoodNode(parent);
        if (childNode == null || parentNode == null)
            return false;
        if (!ignoreDeep)
        {
            return Equals(childNode.Parent, parentNode);
        }
        else
        {
            GoodsNode _parentNode = childNode;
            while (_parentNode != null)
            {
                if (Equals(_parentNode, parentNode))
                {
                    return true;
                }
                _parentNode = _parentNode.Parent;
            }
            return false;
        }
    }

    /// <summary>
    /// ͨ�����ͻ�ȡ�ڵ����
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    private static GoodsNode GetGoodNode(EnumGoodsType targetType)
    {
        GoodsNode goodsNode = null;
        goodsNodeDic.TryGetValue(targetType, out goodsNode);
        return goodsNode;
    }

    /// <summary>
    /// ���߽ڵ�
    /// </summary>
    public class GoodsNode
    {
        public GoodsNode Parent;

        public List<GoodsNode> Childs;

        public EnumGoodsType GoodsType;
    }
}