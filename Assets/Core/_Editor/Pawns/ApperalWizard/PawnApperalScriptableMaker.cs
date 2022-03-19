using System.IO;

using UnityEngine;
using UnityEditor;

using RKCore;
using RKCore.Pawns;

namespace RKEditor
{
    public class PawnApperalScriptableMaker : EditorWindow
    {
        [MenuItem("Appreal Maker/Make Apperal Scriptable")]
        public static void ShowWindow()
            => GetWindow<PawnApperalScriptableMaker>();

        [SerializeField] private DirectionalSprite m_Sprite = null;
        [SerializeField] private SpriteData m_EastSprite = default;
        [SerializeField] private SpriteData m_WestSprite = default;
        [SerializeField] private SpriteData m_SouthSprite = default;
        [SerializeField] private SpriteData m_NorthSprite = default;
        [SerializeField] private bool m_IsFlipX = false;
        [SerializeField] private PawnBodypartsType m_Bodypart = PawnBodypartsType.None;

        [SerializeField] private string m_TableName = "Sample";
        [SerializeField] private string m_Directory = "";
        [SerializeField] private string m_AssetDirectory;

        public string SetDirectory
        {
            set
            {
                if (m_Directory.Equals(value)) return;

                m_Directory = value;
                m_AssetDirectory = UnityPath.LocalToAssetPath(value);
            }
        }

        private void Awake()
        {
            titleContent = new GUIContent("Apperal Maker");

            //  Default directory
            SetDirectory = UnityPath.Combine(Application.dataPath, "Core", "Resources");
        }

        private void Reset()
        {
            m_TableName = "";
            m_Directory = "";
            m_AssetDirectory = null;

            m_Sprite = null;
            m_EastSprite = default;
            m_WestSprite = default;
            m_SouthSprite = default;
            m_NorthSprite = default;
            m_IsFlipX = false;
            m_Bodypart = PawnBodypartsType.None;
        }

        private void OnGUI()
        {
            ShowToolbarFileGUI();

            ShowDirectoryBrowserGUI();
            ShowImportSpriteGUI();

            OnPreviewGUI();
        }

        private void ShowToolbarFileGUI()
        {
            using var Horizontal = new EditorGUILayout.HorizontalScope(EditorStyles.toolbar);

            EditorGUI.BeginChangeCheck();
            {
                m_Sprite =
                    (DirectionalSprite)EditorGUILayout.ObjectField(obj: m_Sprite, objType: typeof(DirectionalSprite), true);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (m_Sprite != null)
                {
                    var so = new SerializedObject(m_Sprite);
                    string str = AssetDatabase.GetAssetPath(m_Sprite);
                    m_TableName = Path.GetFileNameWithoutExtension(str);

                    str = UnityPath.AssetToLocalPath(str);
                    SetDirectory = Path.GetDirectoryName(str);

                    m_EastSprite = SetProperty(SerializeUtility.GetProperty(so, "m_East"));
                    m_NorthSprite = SetProperty(SerializeUtility.GetProperty(so, "m_North"));
                    m_SouthSprite = SetProperty(SerializeUtility.GetProperty(so, "m_South"));
                    m_WestSprite = SetProperty(SerializeUtility.GetProperty(so, "m_West"));
                    m_IsFlipX = SerializeUtility.GetProperty(so, "m_IsFlipX").boolValue;
                    m_Bodypart = (PawnBodypartsType)SerializeUtility.GetProperty(so, "m_Bodypart").enumValueIndex;
                }
                else
                    Reset();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(m_Sprite == null ? "Make" : "Save", EditorStyles.toolbarButton))
            {
                SaveScriptable();
            }

            static SpriteData SetProperty(SerializedProperty property)
            {
                return new SpriteData()
                {
                    Sprite = (Sprite)property.FindPropertyRelative("Sprite").objectReferenceValue,
                    Offset = property.FindPropertyRelative("Offset").vector2Value
                };
            }
        }

        private void ShowDirectoryBrowserGUI()
        {
            using var horizontal = DrawVerticalHeader("Box", "Save Apperal Directory");

            EditorGUI.BeginChangeCheck();
            m_TableName = EditorGUILayout.DelayedTextField("Name", m_TableName);
            if (EditorGUI.EndChangeCheck() && !string.IsNullOrWhiteSpace(m_AssetDirectory))
            {
                string str = UnityPath.Combine(m_AssetDirectory, $"{m_TableName}.asset");
                var asset = AssetDatabase.LoadAssetAtPath<DirectionalSprite>(str);
                if (asset != null) m_Sprite = asset;
                else m_Sprite = null;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                string str = UnityPath.Combine(m_AssetDirectory, $"{m_TableName}.asset");
                EditorGUILayout.TextField(str, GUILayout.MaxWidth(position.width - 32f));
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Browser"))
                {
                    string dir = FileUtility.OpenFolderPanel("Select directory ..");
                    if (!string.IsNullOrWhiteSpace(dir))
                    {
                        if (dir.Contains(Application.dataPath))
                            SetDirectory = dir;
                    }
                }
            }

            GUILayout.Space(3f);
        }

        private void ShowImportSpriteGUI()
        {
            using var horizontal = DrawVerticalHeader(GUIStyle.none, "Apperal Setting");

            m_Bodypart = (PawnBodypartsType)EditorGUILayout.EnumPopup(" Part", m_Bodypart);
            m_IsFlipX = EditorGUILayout.Toggle(" Use Flip Horizontal Sprite", m_IsFlipX);

            GUILayout.Space(6f);

            DrawProperty(ref m_EastSprite, "East");
            DrawProperty(ref m_WestSprite, "West");
            DrawProperty(ref m_NorthSprite, "North");
            DrawProperty(ref m_SouthSprite, "South");

            GUILayout.Space(6f);

            static void DrawProperty(ref SpriteData data, string label)
            {
                GUILayout.Label(label, EditorStyles.boldLabel);
                using (new EditorGUILayout.HorizontalScope(EditorStyles.textArea))
                {
                    GUILayout.Space(12f);
                    using var vertical = new EditorGUILayout.VerticalScope();

                    using (var h = new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Sprite");
                        data.Sprite = (Sprite)EditorGUILayout.ObjectField(data.Sprite, typeof(Sprite), false);
                    }

                    using (var h = new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Offset");
                        data.Offset = EditorGUILayout.Vector2Field("", data.Offset);
                    }
                }

                GUILayout.Space(6f);
            }
        }

        private void OnPreviewGUI()
        {

        }

        private void SaveScriptable()
        {
            if (m_Sprite == null)
            {
                string fullName = UnityPath.Combine(m_Directory, $"{m_TableName}.asset");
                string assetPath = UnityPath.LocalToAssetPath(fullName);
                DirectionalSprite asset = null;

                if (File.Exists(fullName))
                    asset = AssetDatabase.LoadAssetAtPath<DirectionalSprite>(assetPath);
                else
                    asset = CreateInstance<DirectionalSprite>();

                SetAttribute(this, asset);
                AssetDatabase.CreateAsset(asset, assetPath);

                m_Sprite = asset;
            }
            else
                SetAttribute(this, m_Sprite);

            AssetDatabase.SaveAssetIfDirty(m_Sprite);

            static void SetAttribute(PawnApperalScriptableMaker instance, DirectionalSprite sprite)
            {
                var so = new SerializedObject(sprite);
                so.Update();
                {
                    SetSpriteData(so, "m_East", instance.m_EastSprite);
                    SetSpriteData(so, "m_West", instance.m_WestSprite);
                    SetSpriteData(so, "m_North", instance.m_NorthSprite);
                    SetSpriteData(so, "m_South", instance.m_SouthSprite);
                    SerializeUtility.GetProperty(so, "m_IsFlipX").boolValue = instance.m_IsFlipX;
                    SerializeUtility.GetProperty(so, "m_Bodypart").enumValueIndex = (int)instance.m_Bodypart;
                }
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        internal static EditorGUILayout.VerticalScope DrawVerticalHeader(GUIStyle style, string label)
        {
            var horizontal = new EditorGUILayout.VerticalScope(style);
            GUILayout.Space(3f);

            GUILayout.Label(label, EditorStyles.boldLabel);
            GUILayout.Space(6f);

            return horizontal;
        }

        internal static void SetSpriteData(SerializedObject so, string propertyName, SpriteData data)
        {
            var property = so.FindProperty(propertyName);
            property.FindPropertyRelative("Sprite").objectReferenceValue = data.Sprite;
            property.FindPropertyRelative("Offset").vector2Value = data.Offset;
        }
    }
}
