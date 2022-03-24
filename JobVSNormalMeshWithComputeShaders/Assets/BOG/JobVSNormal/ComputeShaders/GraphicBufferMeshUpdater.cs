using UnityEngine;

namespace BOG
{
    //A class can prevent other classes from inheriting from it, or from any of its members, by declaring itself or the member as sealed.
    public sealed class GraphicBufferMeshUpdater : MeshUpdater
    {
        [SerializeField] private string calcNormalsKernelName = "RippleCalcNormals";
        [SerializeField] private MeshParamsManager meshParamsManagerSO;
        [SerializeField] private int meshVerticesMultiplier;
        [Header("TODO issues are there especially while generating mesh through code")]
        [SerializeField] private bool calculateNormals;

        private int calcNormalsKernel;
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2
        private GraphicsBuffer gpuVertices;
#endif
        protected override void Start()
        {
            base.Start();
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.
            // In order for the GPU code path to work, we want the mesh vertex
            // buffer to be usable as a "Raw" buffer (RWBuffer) in a compute shader.
            
            // Note that while using StructuredBuffer might be more convenient, a
            // vertex buffer that is also a structured buffer is not supported on
            // some graphics APIs (most notably DX11). That's why we use a Raw buffer
            // instead.
            mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
#endif
            calcNormalsKernel = computeShader.FindKernel(calcNormalsKernelName);
            isMeshGenerated = true;
        }

        protected override void Update()
        {
            base.Update();
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2
            // Acquire mesh GPU vertex buffer, if needed. Note that we can't do this just once,
            // since the buffer can become invalid when doing CPU based vertex modifications.
            gpuVertices ??= mesh.GetVertexBuffer(0);
            //set compute shader params
            computeShader.SetInt("gVertexCount", mesh.vertexCount);
            computeShader.SetInt("gVertexGridX", meshParamsManagerSO.Resolution);
            computeShader.SetInt("gVertexGridY", meshParamsManagerSO.Resolution);
            computeShader.SetInt("gVerticesMultiplier", meshVerticesMultiplier);
            computeShader.SetBuffer(mainKernel, "bufVertices", gpuVertices);
            // update vertex positions
            //computeShader.Dispatch(mainKernel, dispatchCount, 1, 1);
            computeShader.Dispatch(mainKernel, (mesh.vertexCount + 63) / 63, 1, 1);
            // calculate normals TODO issues are there especially while generating mesh through code
            if (calculateNormals)
            {
                computeShader.SetBuffer(calcNormalsKernel, "bufVertices", gpuVertices);
                computeShader.Dispatch(calcNormalsKernel, (mesh.vertexCount + 63) / 63, 1, 1);
            }
#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void CleanupComputeResources()
        {
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2
            gpuVertices?.Dispose();
            gpuVertices = null;
#endif
        }
    }
}
