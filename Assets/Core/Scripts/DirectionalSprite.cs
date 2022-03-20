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

        public bool UseFlipEastSprite => m_IsFlipX;

        public void OnAwake()
        {
            if (UseFlipEastSprite)
            {
                if (m_East.Sprite == null) m_East.Sprite = m_West.Sprite;
                else if (m_West.Sprite == null) m_West.Sprite = m_East.Sprite;
            }
        }

#pragma warning disable IDE0066
        public SpriteData GetSpriteData(PawnDirection dir)
        {
            switch (dir)
            {
                case PawnDirection.East: return m_East;
                case PawnDirection.West: return m_West;
                case PawnDirection.North: return m_North;
                case PawnDirection.South: return m_South;
                default: return default;
            }
        }
#pragma warning restore IDE0066

    }
}
