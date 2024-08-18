using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CurvedWorldRenderPass : ScriptableRenderPass
{
    private static readonly int bendAmount =
        Shader.PropertyToID("_BendAmount");

    private Material material;
    private CurvedWorldSettings curvedWorldSettings;
    private RenderTextureDescriptor curveTextureDescriptor;
    private RTHandle curveTextureHandle;

    public CurvedWorldRenderPass(Material material, CurvedWorldSettings curvedWorldSettings)
    {
        this.material = material;
        this.curvedWorldSettings = curvedWorldSettings;
        curveTextureDescriptor = new RenderTextureDescriptor(Screen.width,
            Screen.height, RenderTextureFormat.Default, 0);
    }

    public override void Configure(CommandBuffer cmd,
        RenderTextureDescriptor cameraTextureDescriptor)
    {
        // Set the blur texture size to be the same as the camera target size.
        curveTextureDescriptor.width = cameraTextureDescriptor.width;
        curveTextureDescriptor.height = cameraTextureDescriptor.height;

        // Check if the descriptor has changed, and reallocate the RTHandle if necessary
        RenderingUtils.ReAllocateIfNeeded(ref curveTextureHandle, curveTextureDescriptor);
    }

    private void UpdateSettings()
    {
        if (material == null) return;
        material.SetFloat(bendAmount, curvedWorldSettings.intensity);
    }

    public override void Execute(ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        //Get a CommandBuffer from pool.
        CommandBuffer cmd = CommandBufferPool.Get();

        using(new ProfilingScope(cmd, new ProfilingSampler("Curved World")))
        {
            RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

            UpdateSettings();

            // Blit from the camera target to the temporary render texture,
            // using the first shader pass.
            Blit(cmd, cameraTargetHandle, curveTextureHandle, material, 0);
            // Blit from the temporary render texture to the camera target,
            // using the second shader pass.
            Blit(cmd, curveTextureHandle, cameraTargetHandle);
        }

        //Execute the command buffer and release it back to the pool.
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public void Dispose()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            Object.Destroy(material);
        }
        else
        {
            Object.DestroyImmediate(material);
        }
#else
                Object.Destroy(material);
#endif

        if (curveTextureHandle != null) curveTextureHandle.Release();
    }
}