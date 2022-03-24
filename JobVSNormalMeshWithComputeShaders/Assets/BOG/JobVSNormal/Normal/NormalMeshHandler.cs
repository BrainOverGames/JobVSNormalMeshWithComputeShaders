using UnityEngine;
using UnityEngine.Rendering;

namespace BOG
{
    //A class can prevent other classes from inheriting from it, or from any of its members, by declaring itself or the member as sealed.
    public sealed class NormalMeshHandler : MeshHandler
    {
        internal override void Init(ref Mesh mesh, int resolution)
        {
            Debug.LogError("NORMAL MESH HANDLER");
            mesh = new Mesh
            {
                name = "Normal Quad Mesh"
            };
            // Use 32 bit index buffer to allow water grids larger than ~250x250
            mesh.indexFormat = IndexFormat.UInt32;
            NormalMeshParams(ref mesh, resolution);
        }

        private void NormalMeshParams(ref Mesh mesh, int resolution)
        {
            MeshVertices(mesh, resolution);
            MeshTriangles(mesh, resolution);
            //tangent & uv calculations not required
            mesh.RecalculateNormals();
        }

        private void MeshVertices(Mesh mesh, int resolution)
        {
            //vertices based on resolution
            Vector3[] meshVertices = new Vector3[resolution * resolution];
            int index = 0;
            for (var i = 0; i < resolution; i++)
            {
                for (var j = 0; j < resolution; j++)
                {
                    float x = MapValue(i, 0.0f, resolution - 1, -1 / 2.0f, 1 / 2.0f);
                    float z = MapValue(j, 0.0f, resolution - 1, -1 / 2.0f, 1 / 2.0f);
                    meshVertices[index++] = new Vector3(x, 0f, z);
                }
            }
            mesh.vertices = meshVertices;
        }

        private void MeshTriangles(Mesh mesh, int resolution)
        {
            // Create an index buffer for the grid
            int[] indices = new int[(resolution - 1) * (resolution - 1) * 6];
            int index = 0;
            for (var i = 0; i < resolution - 1; i++)
            {
                for (var j = 0; j < resolution - 1; j++)
                {
                    var baseIndex = i * resolution + j;
                    indices[index++] = baseIndex;
                    indices[index++] = baseIndex + 1;
                    indices[index++] = baseIndex + resolution + 1;
                    indices[index++] = baseIndex;
                    indices[index++] = baseIndex + resolution + 1;
                    indices[index++] = baseIndex + resolution;
                }
            }
            mesh.triangles = indices;
        }

        private float MapValue(float refValue, float refMin, float refMax, float targetMin, float targetMax)
        {
            return targetMin + (refValue - refMin) * (targetMax - targetMin) / (refMax - refMin);
        }
    }
}
