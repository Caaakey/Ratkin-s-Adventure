using System;

using UnityEngine;

using RksAdventure.Core.Managers;

namespace RksAdventure.Core.Components
{
    public class Pawn : MonoBehaviour
    {
        [SerializeField] private PawnMovement m_Movement;
        [SerializeField] private PawnRenderer m_Renderer;
        [SerializeField] private PawnDirection m_Direction = PawnDirection.South;

        private Transform m_CachedTransform;
        public Transform GetTransform
        {
            get
            {
                if (m_CachedTransform == null) m_CachedTransform = transform;
                return m_CachedTransform;
            }
        }

        private void Awake()
        {
            m_Movement = new PawnMovement(this);
            m_Renderer = new PawnRenderer(this);
        }

        private void Update()
        {
            m_Movement.Update();

            if (Input.GetKeyDown(KeyCode.W)) Direction = PawnDirection.North;
            if (Input.GetKeyDown(KeyCode.S)) Direction = PawnDirection.South;
            if (Input.GetKeyDown(KeyCode.A)) Direction = PawnDirection.West;
            if (Input.GetKeyDown(KeyCode.D)) Direction = PawnDirection.East;
        }

        public PawnDirection Direction
        {
            get => m_Direction;
            set
            {
                if (m_Direction == value) return;
                m_Direction = value;

                m_Renderer.Direction = m_Direction;
            }
        }

    }
}
