using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  魔力脉冲设置冲击长度
/// </summary>
public class MagicPulseSetShotRange : MonoBehaviour, IParticalConduct
{
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
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, new Vector3(range, 0, 0));
        }
    }
}
