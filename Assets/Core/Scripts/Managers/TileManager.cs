using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.U2D;

using RksAdventure.Core.Components;
using UnityEngine.Tilemaps;

namespace RksAdventure.Core.Managers
{
    public class TileManager : MonoBehaviour
    {
        public static TileManager Get = null;

        [SerializeField] private Tilemap m_Tilemap;
        [SerializeField] private GridLayout m_GridLayout;

        private void Awake()
            => Get = this;

        public TileBase GetTile(Vector3Int position)
            => m_Tilemap.GetTile(position);

        public Vector3Int WorldToCell(Vector3 worldPosition)
            => m_GridLayout.WorldToCell(worldPosition);

        public Vector3 CellToWorld(Vector3Int cellPosition)
            => m_GridLayout.CellToWorld(cellPosition);

    }
}
