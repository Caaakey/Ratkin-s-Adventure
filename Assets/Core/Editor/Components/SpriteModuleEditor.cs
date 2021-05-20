using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

using UnityEditor;
using UnityEngine.U2D;

namespace RksAdventure.Core.Components
{
    [CustomEditor(typeof(SpriteModule), true)]
    public class SpriteModuleEditor : Editor
    {
        private SpriteModule m_Module;
        private SpriteModule[] m_Modules;

        private void OnEnable()
        {
            m_Modules = new SpriteModule[targets.Length];

            for (int i = 0; i < targets.Length; ++i)
                m_Modules[i] = targets[i] as SpriteModule;

            m_Module = m_Modules[0];
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            var texture = (Texture2D)EditorGUILayout.ObjectField(
                "Sprite",
                m_Module.SharedMaterial == null ? null : m_Module.SharedMaterial.mainTexture,
                typeof(Texture2D),
                false);

            if (EditorGUI.EndChangeCheck())
            {
                foreach (var t in m_Modules)
                {
                    t.Initialize();

                    Undo.RecordObject(t.SharedMaterial, "Change Texture");
                    t.SharedMaterial.mainTexture = texture;

                    if (texture != null)
                        t.UpdateVertices();
                }
            }
        }

    }
}
