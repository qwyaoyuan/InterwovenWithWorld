using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示玩家角色经验值的UI
/// </summary>
public class UIPlayerExperience : MonoBehaviour
{
    /// <summary>
    /// 用于显示经验进度的图片
    /// </summary>
    public Image exprienceImage;

    /// <summary>
    /// 用于显示详细信息的文本
    /// </summary>
    public Text exprienceText;

    /// <summary>
    /// 玩家运行时状态
    /// </summary>
    IPlayerState iPlayerState;

    /// <summary>
    /// 等级数据
    /// </summary>
    LevelData levelData;

    void Start()
    {
        iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
        levelData = DataCenter.Instance.GetMetaData<LevelData>();
        GameState.Instance.Registor<IPlayerState>(IPlayerStateChanged);
        SetExperience();
    }

    /// <summary>
    /// 玩家状态发生变化
    /// </summary>
    /// <param name="iPlayerState"></param>
    /// <param name="fieldName"></param>
    private void IPlayerStateChanged(IPlayerState iPlayerState, string fieldName)
    {
        if (string.Equals(fieldName, GameState.Instance.GetFieldName<IPlayerState, int>(temp => temp.Experience)))//经验发生变化
        {
            SetExperience();
        }
    }

    /// <summary>
    /// 显示经验的图片
    /// </summary>
    private void SetExperience()
    {
        LevelDataInfo levelDataInfo = levelData[iPlayerState.Level];
        if (levelDataInfo != null)
        {
            int maxEx = levelDataInfo.Experience;
            int nowEx = iPlayerState.Experience;
            float bili = ((float)nowEx) / ((float)maxEx);
            exprienceImage.fillAmount = Mathf.Clamp(bili, 0, 1);
            exprienceText.text = nowEx + "/" + maxEx + "(" + ((int)(bili * 100)) + "%)";
        }
        else
        {
            exprienceImage.fillAmount = 0;
            exprienceText.text = "";
        }
    }
}
