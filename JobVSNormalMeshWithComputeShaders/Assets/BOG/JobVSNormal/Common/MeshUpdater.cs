using Unity.Collections;
using UnityEngine;

// Around 60 FPS if we do not use AsyncGPUReadback
// Around 145 FPS if we use AsyncGPUReadback!
namespace BOG
{
    public class MeshUpdater : MonoBehaviour
    {
        [SerializeField] internal ComputeShader computeShader;
        [SerializeField] internal MeshFilter meshFilter;
        [SerializeField] internal float rippleWaveSpeed;
        [SerializeField] internal string mainKernelName = "RippleWave";

        internal Mesh mesh;
        internal int mainKernel;
        internal NativeArray<Vector3> vertData;
        internal bool isMeshGenerated;
        internal float localTime;
        internal int dispatchCount = 0;

        protected virtual void Start()
        {
            mesh = meshFilter.sharedMesh;
            if (!mesh) return;
            InitComputeShader();
        }

        protected virtual void InitComputeShader()
        {
            //compute shader
            mainKernel = computeShader.FindKernel(mainKernelName);
            computeShader.GetKernelThreadGroupSizes(mainKernel, out uint threadX, out uint threadY, out uint threadZ);
            dispatchCount = Mathf.CeilToInt(mesh.vertexCount / threadX + 1);
        }

        protected virtual void Update()
        {
            if (!isMeshGenerated) return;
            localTime += Time.deltaTime * rippleWaveSpeed;
            computeShader.SetFloat("_Time", localTime);
        }

        protected virtual void OnDestroy()
        {
            isMeshGenerated = false;
            CleanupComputeResources();
        }

        protected virtual void CleanupComputeResources()
        {

        }
    }
}
