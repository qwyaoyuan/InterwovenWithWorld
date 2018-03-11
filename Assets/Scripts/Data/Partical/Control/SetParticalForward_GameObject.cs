using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 设置粒子的朝向(粒子的游戏对象,非发射方向)
/// </summary>
class SetParticalForward_GameObject : MonoBehaviour, IParticalConduct
{
    public void Open()
    {
     
    }

    public void SetCollisionCallback(Func<CollisionHitCallbackStruct, bool> CallBack)
    {

    }

    public void SetColor(Color color)
    {

    }

    public void SetForward(Vector3 forward)
    {
        transform.forward = forward;
    }

    public void SetLayerMask(LayerMask layerMask)
    {

    }

    public void SetRange(float range)
    {
  
    }
}

