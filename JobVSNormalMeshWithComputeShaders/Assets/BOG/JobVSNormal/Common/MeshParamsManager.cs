using UnityEngine;

namespace BOG
{
    [CreateAssetMenu(fileName = "MeshParamsData", menuName = "ScriptableObjects/MeshParamsScriptableObject", order = 1)]
    public class MeshParamsManager : ScriptableObject
    {
        [SerializeField] private int resolution;

        public int Resolution { get => resolution; }
    }
}
