using UnityEngine;

namespace BOG
{
    public class MeshGenerator : MonoBehaviour
    {
        [SerializeField] private MeshParamsManager meshParamsManagerSO;
        [SerializeField] internal MeshFilter meshFilter;

        internal Mesh mesh;

        protected virtual void OnEnable()
        {
            meshFilter.sharedMesh = mesh;
        }

        protected virtual void GenerateMesh<T>() where T : MeshHandler, new()
        {
            if (!SystemInfo.supportsAsyncGPUReadback) 
            {
                gameObject.SetActive(false); 
                return; 
            }
            T meshHandler = new T();
            meshHandler.Init(ref mesh, meshParamsManagerSO.Resolution);
        }

        protected virtual void OnDisable()
        {

        }
    }
}
