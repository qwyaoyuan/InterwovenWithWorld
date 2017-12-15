using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParticalStartColor : MonoBehaviour, IParticalConduct
{
    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {
 
    }

    public void SetColor(Color color)
    {

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = color;
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
