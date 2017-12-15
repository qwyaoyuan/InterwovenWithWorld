using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 设置灯光颜色
/// </summary>
class SetLightColor : MonoBehaviour, IParticalConduct
{
    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {

    }

    public void SetColor(Color color)
    {
        Light light = GetComponent<Light>();
        if (light != null)
        {
            light.color = color;
        }
    }

    public void SetForward(Vector3 forward)
    {

    }

    public void SetLayerMask(LayerMask layerMask)
    {

    }

    public void SetRange(float range)
    {
        
    }
}

