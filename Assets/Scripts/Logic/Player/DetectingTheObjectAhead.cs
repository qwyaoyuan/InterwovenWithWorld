using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 检测前方物体
/// </summary>
public class DetectingTheObjectAhead : MonoBehaviour
{
    /// <summary>
    /// 要检测的物体的层遮罩 
    /// </summary>
    [SerializeField]
    LayerMask layerMaks;

    /// <summary>
    /// 当前的对象
    /// </summary>
    GameObject nowObj;

    private void OnTriggerEnter(Collider other)
    {
        int layer = (int)Mathf.Pow(2, other.gameObject.layer);
        if (layerMaks.value == (layerMaks.value | layer))
        {
            IDataInfoType iDataInfoType = other.gameObject.GetComponent<IDataInfoType>();
            if (iDataInfoType != null)
            {
                if (!GameObject.Equals(nowObj, other.gameObject))
                {
                    GameObject lastObj = nowObj;
                    nowObj = other.gameObject;
                    IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
                    if (Type.Equals(iDataInfoType.T, typeof(StuffDataInfoMono)))
                    {
                        StuffDataInfoMono stuffDataInfoMono = iDataInfoType as StuffDataInfoMono;
                        if (stuffDataInfoMono.StuffDataInfo.ResidualCount() <= 0)
                        {
                            //尝试刷新
                            stuffDataInfoMono.StuffDataInfo.Update();
                        }
                        //如果大于零则表示可以
                        if (stuffDataInfoMono.StuffDataInfo.ResidualCount() > 0)
                        {
                            iPlayerState.TouchTargetStruct = new TouchTargetStruct()
                            {
                                ID = stuffDataInfoMono.StuffDataInfo.StuffID,
                                TerrainName = stuffDataInfoMono.StuffDataInfo.SceneName,
                                TouchTargetType = TouchTargetStruct.EnumTouchTargetType.Stuff
                            };
                        }
                        else
                        {
                            nowObj = lastObj;
                        }

                    }
                    else if (Type.Equals(iDataInfoType.T, typeof(NPCDataInfoMono)))
                    {
                        NPCDataInfoMono npcDataInfoMono = iDataInfoType as NPCDataInfoMono;
                        iPlayerState.TouchTargetStruct = new TouchTargetStruct()
                        {
                            ID = npcDataInfoMono.NPCDataInfo.NPCID,
                            TerrainName = npcDataInfoMono.NPCDataInfo.SceneName,
                            TouchTargetType = TouchTargetStruct.EnumTouchTargetType.NPC
                        };
                    }
                    else if (Type.Equals(iDataInfoType.T, typeof(ActionInteractiveDataInfoMono)))
                    {
                        ActionInteractiveDataInfoMono actionInteractiveDataInfoMono = iDataInfoType as ActionInteractiveDataInfoMono;
                        iPlayerState.TouchTargetStruct = new TouchTargetStruct()
                        {
                            ID = actionInteractiveDataInfoMono.ActionInteractiveDataInfo.ActionInteractiveID,
                            TerrainName = actionInteractiveDataInfoMono.ActionInteractiveDataInfo.SceneName,
                            TouchTargetType = TouchTargetStruct.EnumTouchTargetType.Action
                        };
                    }
                    
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int layer = (int)Mathf.Pow(2, other.gameObject.layer);
        if (layerMaks.value == (layerMaks.value | layer))
        {
            IDataInfoType iDataInfoType = other.gameObject.GetComponent<IDataInfoType>();
            if (iDataInfoType != null)
            {
                if (GameObject.Equals(nowObj, other.gameObject))
                {
                    nowObj = null;
                    IPlayerState iPlayerState = GameState.Instance.GetEntity<IPlayerState>();
                    iPlayerState.TouchTargetStruct = new TouchTargetStruct() { ID = -1, TerrainName = "", TouchTargetType = TouchTargetStruct.EnumTouchTargetType.None };
                }
            }
        }
    }
}

