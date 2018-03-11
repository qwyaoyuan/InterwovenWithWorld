using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetThunderstormParticalRange : MonoBehaviour, IParticalConduct
{
    /// <summary>
    /// 粒子系统
    /// </summary>
    ParticleSystem thisParticleSystem;

    private void Awake()
    {
        thisParticleSystem = GetComponent<ParticleSystem>();
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
        ParticleSystem.MainModule mainModule = thisParticleSystem.main;
        mainModule.startSpeed = range;
    }

    public void Open()
    {
      
    }
}
