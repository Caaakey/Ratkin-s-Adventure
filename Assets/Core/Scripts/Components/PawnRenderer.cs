using System;

using UnityEngine;
using UnityEngine.Rendering;

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
    public class PawnRenderer
    {
        public PawnRenderer(Pawn pawn)
        {
            m_Pawn = pawn;
            var transform = pawn.GetTransform;

            if (m_SortingGroup == null)
                m_SortingGroup = transform.GetComponent<SortingGroup>();

            m_Parts = new SpriteRenderer[m_PawnSprite.Part.Length];
            for (PawnPart i = 0; i <= PawnPart.Tail; ++i)
            {
                m_Parts[(int)i] = Utility.FindComponentInChild<SpriteRenderer>(transform, i.ToString());
            }

            UpdatePartSprite(PawnPart.Body, "Default");
            UpdatePartSprite(PawnPart.Apparel, "Default");
            UpdatePartSprite(PawnPart.Tail, "Default");
            UpdatePartSprite(PawnPart.Face, "Default");
            UpdatePartSprite(PawnPart.Hair, "LongB");
            UpdatePartSprite(PawnPart.Ear, "Default");
        }

        private readonly Pawn m_Pawn;
        private readonly SortingGroup m_SortingGroup = null;
        private readonly SpriteRenderer[] m_Parts = null;
        private readonly PawnSprite m_PawnSprite = new PawnSprite();

        public PawnDirection Direction
        {
            set
            {
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
