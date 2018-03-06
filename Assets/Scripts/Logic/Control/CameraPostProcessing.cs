using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.PostProcessing;

/// <summary>
/// 摄像机镜头特效控制
/// </summary>
public class CameraPostProcessing : MonoBehaviour
{
    private void Start()
    {
        IGameState iGameState = GameState.Instance.GetEntity<IGameState>();
        PostProcessingBehaviour postProcessingBehaviour = GetComponent<PostProcessingBehaviour>();
        if ( postProcessingBehaviour!=null)
        {
            PostProcessingProfile postProcessingProfile = Resources.Load<PostProcessingProfile>("Data/CameraPostProcessing/" + iGameState.SceneName);
            if (postProcessingProfile != null)
            {
                postProcessingBehaviour.profile = postProcessingProfile;
            }
            else
            {
                Destroy(postProcessingBehaviour);
            }
        }
    }

}
