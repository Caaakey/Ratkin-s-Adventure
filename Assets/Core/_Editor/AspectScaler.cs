using UnityEngine;
using UnityEditor;

using RKCore;

namespace RKEditor
{
    [CustomEditor(typeof(AspectScaler))]
    public class AspectScalerEditor : Editor
    {
        private void OnEnable()
           => (target as AspectScaler).UpdateScale();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                GUILayout.Label("ScreenSize", style: EditorStyles.boldLabel);
                using GUILayout.HorizontalScope hs = new GUILayout.HorizontalScope();

                GUILayout.Space(12f);

                using GUILayout.VerticalScope vs = new GUILayout.VerticalScope();
                var wProp = SerializeUtility.GetProperty(serializedObject, "m_Width");
                var hProp = SerializeUtility.GetProperty(serializedObject, "m_Height");

                EditorGUI.BeginChangeCheck();
                int w = EditorGUILayout.DelayedIntField("Width", wProp.intValue);
                int h = EditorGUILayout.DelayedIntField("Height", hProp.intValue);

                if (EditorGUI.EndChangeCheck())
                {
                    wProp.intValue = w;
                    hProp.intValue = h;

                    OnEnable();
                }
            }

            GUILayout.Space(6f);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Refresh", GUILayout.MaxWidth(64f)))
                    OnEnable();
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

    }
}
