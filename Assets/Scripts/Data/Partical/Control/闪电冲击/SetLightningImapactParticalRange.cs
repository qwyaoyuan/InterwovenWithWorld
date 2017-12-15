using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置闪电冲击粒子的冲击长度
/// </summary>
public class SetLightningImapactParticalRange : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 浮动值
    /// </summary>
    public float clamp;

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

        ParticleSystemRenderer particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        if (particleSystemRenderer)
        {
            particleSystemRenderer.lengthScale = UnityEngine.Random.Range(range - clamp, range + clamp) * 0.7f;
        }
    }
}
