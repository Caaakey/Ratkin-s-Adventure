using UnityEngine;
using UnityEngine.Rendering;

using Unity.Collections;

namespace RksAdventure.Core.Components
{
    public class SpriteModule : FrameModule
    {
        [SerializeField] private MeshRenderer m_MeshRenderer;
        [SerializeField] private MeshFilter m_MeshFilter;

        public void Initialize()
        {
            m_MeshFilter = Utility.FindComponent<MeshFilter>(gameObject, true);
            m_MeshFilter.sharedMesh = MeshUtility.CreateMesh(Width, Height, new Rect(0, 0, 1, 1));

            m_MeshRenderer = Utility.FindComponent<MeshRenderer>(gameObject, true);
            m_MeshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
        }

        public Renderer GetRenderer => m_MeshRenderer;
        public Mesh SharedMesh
        {
            get => m_MeshFilter == null ? null : m_MeshFilter.sharedMesh;
            set
            {
                if (m_MeshFilter != null) m_MeshFilter.sharedMesh = value;
            }
        }
        public Material SharedMaterial
        {
            get => m_MeshRenderer == null ? null : m_MeshRenderer.sharedMaterial;
            set
            {
                if (m_MeshRenderer != null) m_MeshRenderer.sharedMaterial = value;
            }
        }

        public override int Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                UpdateVertices();
            }
        }
        public override int Height
        {
            get => base.Height;
            set
            {
                base.Height = value;
                UpdateVertices();
            }
        }
        public override Vector2Int LocalSize
        {
            get => base.LocalSize;
            set
            {
                base.LocalSize = value;
                UpdateVertices();
            }
        }
        public override Vector2 Pivot
        {
            get => base.Pivot;
            set
            {
                base.Pivot = value;
                UpdateVertices();
            }
        }
        public override int Depth
        {
            get => base.Depth;
            set
            {
                m_Depth = value;
                if (GetRenderer != null) GetRenderer.sortingOrder = value;
            }
        }

        public void UpdateVertices()
        {
            if (SharedMesh != null)
            {
                using NativeArray<Vector3> vertices = new NativeArray<Vector3>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
                GetCorners(vertices);

                SharedMesh.SetVertices(vertices, 0, 4);
                SharedMesh.RecalculateBounds();
            }
        }

    }
}
