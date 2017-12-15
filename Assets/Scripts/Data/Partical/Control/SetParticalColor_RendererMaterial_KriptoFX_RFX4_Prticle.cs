using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置该类型的颜色(材质-KriptoFX_RFX4_Prticle)
/// 主要适用于粒子
/// </summary>
public class SetParticalColor_RendererMaterial_KriptoFX_RFX4_Prticle : MonoBehaviour, IParticalConduct
{
    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {

    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Renderer renderer = particleSystem.GetComponent<Renderer>();
        Material material = renderer.material;
        if (material != null)
        {
            material.SetColor("_TintColor", color);
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
