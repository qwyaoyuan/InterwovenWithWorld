using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 设置火舞的PivotFire粒子的大小
/// </summary>
public class SetPartical_FireDanceRnage_PivotFire : MonoBehaviour, IParticalConduct
{
    ParticleSystem particalSystem;

    float baseRadius;

    float baseRateOverTime;

    private void Awake()
    {
        particalSystem = GetComponent<ParticleSystem>();
        baseRadius = particalSystem.shape.radius;
        baseRateOverTime = particalSystem.emission.rateOverTime.constant;
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
        ParticleSystem.ShapeModule shapeModule = particalSystem.shape;
        shapeModule.radius = baseRadius * range;
        ParticleSystem.EmissionModule emissionModule = particalSystem.emission;
        ParticleSystem.MinMaxCurve emmissionModule_MinMaxCurve = emissionModule.rateOverTime;
        emmissionModule_MinMaxCurve.constant = baseRateOverTime * range;
        emissionModule.rateOverTime = emmissionModule_MinMaxCurve;
    }

    public void Open()
    {
       
    }
}
