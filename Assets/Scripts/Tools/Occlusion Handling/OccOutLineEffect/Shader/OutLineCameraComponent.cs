using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;


[ExecuteInEditMode]
public class OutLineCameraComponent : MonoBehaviour
{
    private RenderTexture renderTexture = null;
    private CommandBuffer commandBuffer = null;

    [Header("Outline/OutLineEffect.shader")]
    public Shader preoutlineShader = null;
    [Header("Outline/OutlinePrePass.shader")]
    public Shader shader = null;

    private Material _material = null;

    public Material mMaterial
    {
        get
        {
            if (_material == null)
                _material = GenerateMaterial(shader);
            return _material;
        }
    }



    [Header("采样范围")]
    public float samplerArea = 1;


    [Header("降分辨率")]
    public int downSample = 1;


    [Header("迭代次数")]
    public int iteration = 2;

    [Header("描边强度")]
    [Range(0.0f, 10.0f)]
    public float outLineStrength = 3.0f;

    //根据shader创建用于屏幕特效的材质
    protected Material GenerateMaterial(Shader shader)
    {
        if (shader == null)
            return null;

        if (shader.isSupported == false)
            return null;
        Material material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

        if (material)
            return material;

        return null;
    }


    //目标对象  
    private List<OutLineTargetComponent> targetObjects = new List<OutLineTargetComponent>();

    public void AddTarget(OutLineTargetComponent target)
    {
        if (target.material == null)
            target.material = new Material(preoutlineShader);
        targetObjects.Add(target);

        RefreshCommandBuff();

    }
    public void RemoveTarget(OutLineTargetComponent target)
    {
        bool found = false;

        for (int i = 0; i < targetObjects.Count; i++)
        {
            if (targetObjects[i] == target)
            {
                targetObjects.Remove(target);
                DestroyImmediate(target.material);
                target.material = null;
                found = true;
                break;
            }
        }

        if (found)
            RefreshCommandBuff();
    }

    public void RefreshCommandBuff()
    {
        if (renderTexture)
            RenderTexture.ReleaseTemporary(renderTexture);
        renderTexture = RenderTexture.GetTemporary(Screen.width >> downSample, Screen.height >> downSample, 0);

        commandBuffer = new CommandBuffer();
        commandBuffer.SetRenderTarget(renderTexture);
        commandBuffer.ClearRenderTarget(true, true, Color.black);
        for (int i = 0; i < targetObjects.Count; i++)
        {
            Renderer[] renderers = targetObjects[i].GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                commandBuffer.DrawRenderer(r, targetObjects[i].material);
        }
    }


    void OnEnable()
    {
        if (preoutlineShader == null)
            return;
        RefreshCommandBuff();
    }

    void OnDisable()
    {
        if (renderTexture)
        {
            RenderTexture.ReleaseTemporary(renderTexture);
            renderTexture = null;
        }

        if (commandBuffer != null)
        {
            commandBuffer.Release();
            commandBuffer = null;
        }

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mMaterial && renderTexture && commandBuffer != null)
        {
            Graphics.ExecuteCommandBuffer(commandBuffer);

            //对RT进行Blur处理  
            RenderTexture temp1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);
            RenderTexture temp2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0);

            //高斯模糊，两次模糊，横向纵向，使用pass0进行高斯模糊  
            mMaterial.SetVector("_offsets", new Vector4(0, samplerArea, 0, 0));
            Graphics.Blit(renderTexture, temp1, mMaterial, 0);
            mMaterial.SetVector("_offsets", new Vector4(samplerArea, 0, 0, 0));
            Graphics.Blit(temp1, temp2, mMaterial, 0);

            //如果有叠加再进行迭代模糊处理  
            for (int i = 0; i < iteration; i++)
            {
                mMaterial.SetVector("_offsets", new Vector4(0, samplerArea, 0, 0));
                Graphics.Blit(temp2, temp1, mMaterial, 0);
                mMaterial.SetVector("_offsets", new Vector4(samplerArea, 0, 0, 0));
                Graphics.Blit(temp1, temp2, mMaterial, 0);
            }

            //用模糊图和原始图计算出轮廓图
            mMaterial.SetTexture("_BlurTex", temp2);
            Graphics.Blit(renderTexture, temp1, mMaterial, 1);

            //轮廓图和场景图叠加  
            mMaterial.SetTexture("_BlurTex", temp1);
            mMaterial.SetFloat("_OutlineStrength", outLineStrength);
            Graphics.Blit(source, destination, mMaterial, 2);

            RenderTexture.ReleaseTemporary(temp1);
            RenderTexture.ReleaseTemporary(temp2);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
