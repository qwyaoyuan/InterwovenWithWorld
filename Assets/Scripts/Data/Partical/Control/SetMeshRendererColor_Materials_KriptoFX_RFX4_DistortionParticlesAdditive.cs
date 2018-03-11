using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置该类型的颜色(材质)KriptoFX_RFX4_DistortionParticlesAdditive
/// 主要适用于MeshRenderer
/// </summary>
public class SetMeshRendererColor_Materials_KriptoFX_RFX4_DistortionParticlesAdditive : MonoBehaviour, IParticalConduct
{
    public void Open()
    {
      
    }

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {

    }

    public void SetColor(Color color)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            if (material != null)
            {
                material.SetColor("_TintColor", color);
            }
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

