using UnityEngine;

namespace RKCore.Pawns
{
    [RequireComponent(typeof(PawnRenderer))]
    public class PawnMovement : MonoBehaviour
    {
        private PawnRenderer m_Renderer;

        private void Awake()
        {
            m_Renderer = GetComponent<PawnRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) m_Renderer.SpriteDirection = PawnDirection.North;
            else if (Input.GetKeyDown(KeyCode.S)) m_Renderer.SpriteDirection = PawnDirection.South;
            else if (Input.GetKeyDown(KeyCode.A)) m_Renderer.SpriteDirection = PawnDirection.West;
            else if (Input.GetKeyDown(KeyCode.D)) m_Renderer.SpriteDirection = PawnDirection.East;
        }

    }
}
