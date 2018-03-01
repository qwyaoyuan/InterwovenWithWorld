using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC图片渲染摄像机
/// </summary>
public class NPCSpriteCamera : MonoBehaviour
{

    /// <summary>
    /// NPC的ID
    /// </summary>
    public int NPCID;

    /// <summary>
    /// 渲染的摄像机
    /// </summary>
    [SerializeField]
    Camera rendererCamera;

    /// <summary>
    /// NPC图像状态
    /// </summary>
    INPCSpriteState iNPCSpriteState;

    RenderTexture rt;  //声明一个截图时候用的中间变量   
    Texture2D t2d;

    private void Start()
    {
        iNPCSpriteState = GameState.Instance.GetEntity<INPCSpriteState>();
        t2d = new Texture2D(800, 600, TextureFormat.ARGB32, false);
        rt = new RenderTexture(800, 600, 24, RenderTextureFormat.ARGB32);
        rendererCamera.targetTexture = rt;
        StartCoroutine(StartRenderer());
    }

    IEnumerator StartRenderer()
    {
        yield return null;
        RenderTexture.active = rt;
        t2d.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        t2d.Apply();
        RenderTexture.active = null;
        Sprite sprite = Sprite.Create(t2d, new Rect(0, 0, 800, 600), new Vector2(400, 300));
        iNPCSpriteState.SetSprite(NPCID, sprite);
    }
}
