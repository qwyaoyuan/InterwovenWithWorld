/// <summary>
/// ��������
/// </summary>
public enum EnumItemType
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
    KuangShiLeiBig = 1100000,
    #region ��ʯС��
    /// <summary>
    /// ��ʯС��
    /// </summary>
    [FieldExplan("��ʯС��")]
    KuangShiLeiLittle = 1101000,
    #region ����Ŀ�ʯ
    /// <summary>
    /// ����ʯ
    /// </summary>
    [FieldExplan("����ʯ")]
    TieKuangShi= 1101001,
    #endregion
    #endregion
    #endregion
    #endregion
}