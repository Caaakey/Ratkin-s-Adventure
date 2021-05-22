using System;

using UnityEngine;

using RksAdventure.Core.Managers;

namespace RksAdventure.Core.Components
{
    public enum PawnDirection : short
    {
        East = 0,
        West, South, North
    }
    public enum PawnPart : int
    {
        Body = 0,
        Apparel,
        Face,
        Hair,
        Ear,
        Tail
    }

    public class PawnSprite
    {
        public Sprite[] this[int index]
        {
            get => Sprites[index];
            set => Sprites[index] = value;
        }
        public Sprite[] this[PawnPart part]
        {
            get => this[(int)part];
            set => this[(int)part] = value;
        }

        public PawnSprite()
        {
            Part = (PawnPart[])Enum.GetValues(typeof(PawnPart));
            Sprites = new Sprite[Part.Length][];
        }

        public PawnPart[] Part;
        public Sprite[][] Sprites;
    }
    public class PawnModule : MonoBehaviour
    {
        private Transform m_CachedTransform;
        public Transform GetTransform
        {
            get
            {
                if (m_CachedTransform == null) m_CachedTransform = transform;
                return m_CachedTransform;
            }
        }

        [SerializeField] private PawnDirection m_Direction = PawnDirection.South;
        private SpriteRenderer[] m_Parts = null;
        private readonly PawnSprite m_PawnSprite = new PawnSprite();

        private void Awake()
        {
            m_Parts = new SpriteRenderer[m_PawnSprite.Part.Length];
            for (PawnPart i = 0; i <= PawnPart.Tail; ++i)
            {
                m_Parts[(int)i] = Utility.FindComponentInChild<SpriteRenderer>(GetTransform, i.ToString());
            }

            UpdatePartSprite(PawnPart.Body, "Default");
            UpdatePartSprite(PawnPart.Apparel, "Default");
            UpdatePartSprite(PawnPart.Tail, "Default");
            UpdatePartSprite(PawnPart.Face, "Default");
            UpdatePartSprite(PawnPart.Hair, "LongB");
            UpdatePartSprite(PawnPart.Ear, "Default");
        }

        public PawnDirection Direction
        {
            get => m_Direction;
            set
            {
                if (m_Direction == value) return;
                m_Direction = value;

                int index = (int)value;
                for (int i = 0; i < m_Parts.Length; ++i)
                {
                    var sprite = m_PawnSprite[i][index];
                    if (sprite == null && value == PawnDirection.West)
                    {
                        m_Parts[i].flipX = true;
                        sprite = m_PawnSprite[i][(int)PawnDirection.East];
                    }
                    else m_Parts[i].flipX = false;

                    m_Parts[i].sprite = sprite;
                }

                int tail = (int)PawnPart.Tail;
                if (m_Parts[tail] != null)
                    m_Parts[tail].sortingOrder = value == PawnDirection.North ? 3 : 1;
            }
        }

        public void UpdatePartSprite(PawnPart part, string groupName)
        {
            m_PawnSprite[part] = ResourceManager.GetPartSprites(part, groupName);
        }

    }
}
