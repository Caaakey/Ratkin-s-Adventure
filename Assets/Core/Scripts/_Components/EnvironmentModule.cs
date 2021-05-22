using UnityEngine;
using Unity.Mathematics;

namespace RksAdventure.Core.Components
{
    public class EnvironmentModule : MonoBehaviour
    {
        [SerializeField] private MeshFilter m_MeshFilter;
        [SerializeField] private MeshRenderer m_MeshRenderer;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
        }

    }
}
