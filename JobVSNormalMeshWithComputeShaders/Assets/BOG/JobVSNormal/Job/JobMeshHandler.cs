using UnityEngine;
using UnityEngine.Rendering;
using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;

namespace BOG
{
    //A class can prevent other classes from inheriting from it, or from any of its members, by declaring itself or the member as sealed.
    public sealed class JobMeshHandler : MeshHandler
    {
        internal override void Init(ref Mesh mesh, int resolution)
        {
            Debug.LogError("JOB MESH HANDLER");
            Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
            Mesh.MeshData meshData = meshDataArray[0];
            mesh = new Mesh
            {
                name = "Job Quad Mesh"
            };
            MeshJob<SquareGrid, SingleStream>.ScheduleParallel(mesh, meshData, resolution, default).Complete();
            // Use 32 bit index buffer to allow water grids larger than ~250x250
            mesh.indexFormat = IndexFormat.UInt32;
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }
    }
}
