using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于在游戏启动时提前加载部分游戏资源 
/// </summary>
public class ResourcesLoading : MonoBehaviour
{
    /// <summary>
    /// 加载粒子的携程
    /// </summary>
    IEnumerator particalLoad;
    /// <summary>
    /// 加载图片的携程
    /// </summary>
    IEnumerator spriteLoad;

    void Start()
    {
        ParticalManager.Load(temp => particalLoad = temp.GetEnumerator(), () => particalLoad = null);
        SpriteManager.Load(temp => spriteLoad = temp.GetEnumerator(), () => spriteLoad = null);
    }

    private void Update()
    {
        if (particalLoad != null)
        {
            bool result = particalLoad.MoveNext();
            if (!result)
                particalLoad = null;
        }
        if (spriteLoad != null)
        {
            bool result = spriteLoad.MoveNext();
            if (!result)
                particalLoad = null;
        }
    }
}
