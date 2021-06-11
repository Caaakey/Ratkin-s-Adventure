﻿using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.U2D;

using RksAdventure.Core.Components;

namespace RksAdventure.Core.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Get { get; private set; }

        private void Awake()
        {
            Get = this;

            m_Location = new Rect(50f, -200f, Screen.width * 0.5f, -(Screen.height - 100f));
        }

        private Rect m_Location;
        private Transform m_CachedTransform = null;
        private Transform GetTransform
        {
            get
            {
                if (m_CachedTransform == null) m_CachedTransform = GetComponent<Transform>();
                return m_CachedTransform;
            }
        }

        private void Start()
        {
            var pawns = FindObjectsOfType<Pawn>();
            Spawn(pawns);
        }

        //public void OnDrawGizmos()
        //{
        //    Gizmos.DrawWireCube(new Vector3(
        //        m_Location.center.x, m_Location.center.y, 0.01f),
        //        new Vector3(m_Location.size.x, m_Location.size.y, 0.01f));
        //}

        public void Spawn(Pawn[] pawns)
        {
            foreach(var i in pawns)
            {
                Vector2 position = new Vector2(
                    Random.Range(m_Location.x, m_Location.xMax),
                    Random.Range(m_Location.y, m_Location.yMax));

                Debug.Log(position);
                i.GetTransform.localPosition = position;

                i.Direction = PawnDirection.South;
            }
        }
    }
}