using Unity.Mathematics;
using UnityEngine;

namespace RKCore.Pawns
{
    [System.Serializable]
    public class DirectionRenderer
    {
        private Transform m_RenderTransform;
        private Vector2 m_PositionOffset;
        public SpriteRenderer Renderer;
        public DirectionalSprite Sprite;

        public void OnAwake()
        {
            Sprite.OnAwake();

            m_RenderTransform = Renderer.transform;
            m_PositionOffset = m_RenderTransform.localPosition;
        }

        public void SetSprite(PawnDirection dir)
        {
            var data = Sprite.GetSpriteData(dir);
            if (!data.Offset.Equals(Vector2.zero))
                m_RenderTransform.localPosition = m_PositionOffset + data.Offset;
            else
            {
                m_RenderTransform.localPosition = m_PositionOffset;
            }

            if (Sprite.UseFlipEastSprite && dir == PawnDirection.West)
                Renderer.flipX = true;
            else
                Renderer.flipX = false;

            Renderer.sprite = data.Sprite;
        }
    }

    public class PawnRenderer : MonoBehaviour
    {
        [SerializeField] private DirectionRenderer[] m_Renderers;
        [SerializeField] private PawnDirection m_Direction = PawnDirection.None;

        private void Awake()
        {
            bool isForceDirUpdate = m_Direction != PawnDirection.None;
            var dir = m_Direction;
            m_Direction = PawnDirection.None;

            if (m_Renderers != null)
            {
                for (int i = 0; i < m_Renderers.Length; ++i)
                {
                    m_Renderers[i].OnAwake();
                    if (isForceDirUpdate) m_Renderers[i].SetSprite(dir);
                }
            }

            if (isForceDirUpdate) m_Direction = dir;
        }

        public PawnDirection SpriteDirection
        {
            get => m_Direction;
            set
            {
                if (m_Direction == value) return;

                for (int i = 0; i < m_Renderers.Length; ++i)
                    m_Renderers[i].SetSprite(value);

                m_Direction = value;
            }
        }
    }
}
