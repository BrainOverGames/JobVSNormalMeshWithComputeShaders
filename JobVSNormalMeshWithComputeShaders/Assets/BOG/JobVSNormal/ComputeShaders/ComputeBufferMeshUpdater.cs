using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Around 60 FPS if we do not use AsyncGPUReadback
// Around 145 FPS if we use AsyncGPUReadback!
namespace BOG
{
    //A class can prevent other classes from inheriting from it, or from any of its members, by declaring itself or the member as sealed.
    public sealed class ComputeBufferMeshUpdater : MeshUpdater
    {
        private ComputeBuffer vertexBuffer;

        //The callback will be run when the request is ready
        private static event System.Action<AsyncGPUReadbackRequest> asyncGPUReadbackCallback;
        private AsyncGPUReadbackRequest request;

        protected override void Start()
        {
            base.Start();
            Initialize();
            isMeshGenerated = true;
        }

        protected override void Update()
        {
            base.Update();
            computeShader.Dispatch(mainKernel, dispatchCount, 1, 1);
        }

        private void Initialize()
        {
            InitVertexArray();
            InitVertexbuffer();
            AsyncReadback();
        }

        private void InitVertexArray()
        {
            // Init mesh vertex array
            Vector3[] meshVerts = mesh.vertices;
            vertData = new NativeArray<Vector3>(mesh.vertexCount, Allocator.Temp);
            for (int i = 0; i < mesh.vertexCount; ++i)
            {
                vertData[i] = meshVerts[i];
            }
        }

        private void InitVertexbuffer()
        {
            //init vertex buffer
            vertexBuffer = new ComputeBuffer(mesh.vertexCount, 12); // 3*4bytes = sizeof(Vector3)
            if (vertData.IsCreated) vertexBuffer.SetData(vertData);
            computeShader.SetBuffer(mainKernel, "vertexBuffer", vertexBuffer);
        }

        private void AsyncReadback()
        {
            //Request AsyncReadback
            asyncGPUReadbackCallback -= AsyncGPUReadbackCallback;
            asyncGPUReadbackCallback += AsyncGPUReadbackCallback;
            request = AsyncGPUReadback.Request(vertexBuffer, asyncGPUReadbackCallback);
        }

        public void AsyncGPUReadbackCallback(AsyncGPUReadbackRequest request)
        {
            if (!mesh) return;
            //Readback and show result on texture
            vertData = request.GetData<Vector3>();
            //Update mesh
            mesh.MarkDynamic();
            mesh.SetVertices(vertData);
            mesh.RecalculateNormals();
            //Update to collider, INCASE its attached
            //mc.sharedMesh = mesh;
            //Request AsyncReadback again
            request = AsyncGPUReadback.Request(vertexBuffer, asyncGPUReadbackCallback);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void CleanupComputeResources()
        {
            if (vertexBuffer != null) vertexBuffer.Release();
            asyncGPUReadbackCallback -= AsyncGPUReadbackCallback;
        }
    }
}
