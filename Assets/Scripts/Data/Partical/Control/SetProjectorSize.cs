using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置Projector的大小
/// </summary>
public class SetProjectorSize : MonoBehaviour, IParticalConduct
{

    /// <summary>
    /// 投射对象
    /// </summary>
    Projector projector;

    /// <summary>
    /// 基础的大小
    /// </summary>
    float baseSize;

    private void Awake()
    {
        projector = GetComponent<Projector>();
        if (projector)
        {
            baseSize = projector.orthographicSize;
        }
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
        if (projector)
        {
            projector.orthographicSize = baseSize * range;
        }
    }

    public void Open()
    {
      
    }
}
