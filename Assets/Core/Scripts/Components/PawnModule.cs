using System.Collections.Generic;
using Unity.Collections;

using UnityEngine;
using UnityEngine.Rendering;

using RksAdventure.Core.Managers;

namespace RksAdventure.Core.Components
{
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

        private void Awake()
        {
            SpriteParts = new Dictionary<PawnPart, SpriteRenderer>
            {
                { PawnPart.Body, Utility.FindComponent<SpriteRenderer>(GetTransform.GetChild(0).GetChild(0)) },
                { PawnPart.Apparel, Utility.FindComponent<SpriteRenderer>(GetTransform.GetChild(0).GetChild(1)) },
                { PawnPart.Face, Utility.FindComponent<SpriteRenderer>(GetTransform.GetChild(1).GetChild(0)) },
                { PawnPart.Hair, Utility.FindComponent<SpriteRenderer>(GetTransform.GetChild(1).GetChild(1)) }
            };

            var sp = ResourceManager.GetApprel("RK_ApronSkirt_Thin");
        }

        public Dictionary<PawnPart, SpriteRenderer> SpriteParts { get; set; }
        
        //private SpriteDirection m_Direction = SpriteDirection.South;

    }
}
