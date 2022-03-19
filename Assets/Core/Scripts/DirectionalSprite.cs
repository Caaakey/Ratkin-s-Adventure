using UnityEngine;

using RKCore.Pawns;

namespace RKCore
{
    [System.Serializable]
    public struct SpriteData
    {
        public Sprite Sprite;
        public Vector2 Offset;

        public bool Empty => Sprite == null;
    }

    public class DirectionalSprite : ScriptableObject
    {
        [SerializeField] private SpriteData m_East;
        [SerializeField] private SpriteData m_West;
        [SerializeField] private SpriteData m_North;
        [SerializeField] private SpriteData m_South;
        [SerializeField] private bool m_IsFlipX = true;
        [SerializeField] private PawnBodypartsType m_Bodypart;
        private SpriteData m_CachedFlipSprite = default;

        public bool UseFlipEastSprite => m_IsFlipX;

        public void OnAwake()
        {
            if (m_IsFlipX)
                m_CachedFlipSprite = m_West.Empty ? m_East : m_West;
        }

        public SpriteData GetSpriteData(PawnDirection dir)
        {
            if (dir == PawnDirection.North) return m_North;
            else if (dir == PawnDirection.East || dir == PawnDirection.West)
            {
                if (m_IsFlipX) return m_CachedFlipSprite;
                else return dir == PawnDirection.East ? m_East : m_West;
            }
            else if (dir == PawnDirection.South) return m_South;

            return default;
        }

    }
}
