using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace RksAdventure.Core.Components
{
    public class FrameModule : MonoBehaviour
    {
        [SerializeField] private Transform m_Transform;
        [SerializeField] private SortingGroup m_SortingGroup = null;
        public Transform GetTransform
        {
            get
            {
                if (m_Transform == null) m_Transform = transform;
                return m_Transform;
            }
        }

        private int m_Width = 100;
        private int m_Height = 100;
        private Vector2 m_Pivot = new Vector2(0.5f, 0.5f);
        protected int m_Depth = 0;

        public virtual int Width { get => m_Width; set => m_Width = value; }
        public virtual int Height { get => m_Height; set => m_Height = value; }
        public virtual Vector2Int LocalSize
        {
            get => new Vector2Int(m_Width, m_Height);
            set
            {
                m_Width = value.x;
                m_Height = value.y;
            }
        }
        public virtual Vector2 Pivot
        {
            get => m_Pivot;
            set
            {
                m_Pivot = value;
            }
        }
        public virtual int Depth
        {
            get => m_Depth;
            set
            {
                m_Depth = value;
                if (m_SortingGroup != null) m_SortingGroup.sortingOrder = m_Depth;
            }
        }

        public void GetCorners(NativeArray<Vector3> corners)
        {
            float left = -Pivot.x * Width;
            float top = -Pivot.y * Height;
            float right = left + Width;
            float bottom = top + Height;

            corners[0] = new Vector3(left, top);
            corners[1] = new Vector3(left, bottom);
            corners[2] = new Vector3(right, bottom);
            corners[3] = new Vector3(right, top);
        }

    }

}