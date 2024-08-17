using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BendingRenderFeature : ScriptableRendererFeature
{
    public Material bendingMaterial;
    BendingRenderPass bendingRenderPass;

    public override void Create()
    {
        bendingRenderPass = new BendingRenderPass(bendingMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(bendingRenderPass);
    }


    public class BendingRenderPass : ScriptableRenderPass
    {
        private Material bendingMaterial;
        private RTHandle renderTarget;

        public BendingRenderPass(Material material)
        {
            this.bendingMaterial = material;
            renderTarget = RTHandles.Alloc("_TemporaryColorTexture", name: "_TemporaryColorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("BendingEffect");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            RTHandle cameraTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
            opaqueDesc.depthBufferBits = 0;

            cmd.GetTemporaryRT(Shader.PropertyToID(renderTarget.name), opaqueDesc);
            Blit(cmd, cameraTargetHandle, renderTarget, bendingMaterial, 0);
            //Blit(cmd, renderTarget, cameraTargetHandle);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (cmd == null) throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(renderTarget.name));
        }
    }
}