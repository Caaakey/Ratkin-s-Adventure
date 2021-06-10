using System;

using UnityEngine;

using RksAdventure.Core.Managers;

namespace RksAdventure.Core.Components
{
    public class PawnMovement
    {
        public PawnMovement(Pawn pawn)
        {
            m_Pawn = pawn;
        }

        private readonly Pawn m_Pawn;

        public void Update()
        {
        }

        public PawnDirection Direction
        {
            set
            {

            }
        }

    }
}
