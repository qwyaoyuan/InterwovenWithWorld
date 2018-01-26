using UnityEngine;

[ExecuteInEditMode]
public class OutLineTargetComponent : MonoBehaviour
{
    public Color color = Color.green;

    public Material material { set; get; }

    void OnEnable()
    {
        Camera[] allCameras = Camera.allCameras;
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i].GetComponent<OutLineCameraComponent>() != null)
            {
                allCameras[i].GetComponent<OutLineCameraComponent>().AddTarget(this);
            }
        }
    }

    private void Update()
    {
        if (material != null)
            material.SetColor("_OutLineColor", color);
    }

    void OnDisable()
    {
        Camera[] allCameras = Camera.allCameras;
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i].GetComponent<OutLineCameraComponent>() != null)
            {
                allCameras[i].GetComponent<OutLineCameraComponent>().RemoveTarget(this);
            }
        }
    }
}