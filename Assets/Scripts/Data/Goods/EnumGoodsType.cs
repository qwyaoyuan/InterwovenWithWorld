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