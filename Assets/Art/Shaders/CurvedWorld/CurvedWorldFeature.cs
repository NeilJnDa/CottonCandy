using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CurvedWorldFeature : ScriptableRendererFeature
{
    [SerializeField] private CurvedWorldSettings settings;
    [SerializeField] private Shader shader;
    private Material material;
    private CurvedWorldRenderPass curvedWorldRenderPass;

    public override void Create()
    {
        if (shader == null)
        {
            return;
        }
        material = new Material(shader);
        curvedWorldRenderPass = new CurvedWorldRenderPass(material, settings);
        curvedWorldRenderPass.renderPassEvent = RenderPassEvent.BeforeRendering;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer,
        ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(curvedWorldRenderPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        curvedWorldRenderPass.Dispose();
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Destroy(material);
        }
        else
        {
            DestroyImmediate(material);
        }
#else
                Destroy(material);
#endif
    }
}

[Serializable]
public class CurvedWorldSettings
{
    [Range(0, 1f)] public float intensity = 0.5f;
}
