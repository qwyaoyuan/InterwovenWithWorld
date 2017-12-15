using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


/// <summary>
/// 设置火舞Decal_Loop的投影的大小
/// </summary>
class SetPartical_FireDanceRange_Decal_Loop : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 投射?
    /// </summary>
    Projector projector;

    float baseSize;

    private void Awake()
    {
        projector = GetComponent<Projector>();
        baseSize = projector.orthographicSize;
    }

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
        
    }

    public void SetColor(Color color)
    {
     
    }

    public void SetForward(Vector3 forward)
    {
     
    }

    public void SetLayerMask(LayerMask layerMask)
    {
       
    }

    public void SetRange(float range)
    {
        projector.orthographicSize = baseSize * range;
    }
}

