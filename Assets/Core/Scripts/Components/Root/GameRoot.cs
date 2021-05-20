using UnityEngine;

namespace RksAdventure.Core.Components.Root
{
    [ExecuteInEditMode]
    public class GameRoot : MonoBehaviour
    {
        public int Width = 1920;
        public int Height = 1080;

        public int ActiveHeight
        {
            get
            {
                Vector2 screen = new Vector2(Screen.width, Screen.height);
                float aspect = screen.x / screen.y;

                return ((float)Width / Height > aspect) ? Mathf.RoundToInt(Width / aspect) : Height;
            }
        }

#if UNITY_EDITOR
        public void Update() => UpdateScale();
#else
        private void Awake() => UpdateScale();
#endif
        public void UpdateScale()
        {
            float calcActiveHeight = ActiveHeight;
            if (calcActiveHeight > 0f)
            {
                float size = 2f / calcActiveHeight;
                Vector3 ls = transform.localScale;

                if (!(Mathf.Abs(ls.x - size) <= float.Epsilon) ||
                    !(Mathf.Abs(ls.y - size) <= float.Epsilon) ||
                    !(Mathf.Abs(ls.z - size) <= float.Epsilon))
                {
                    transform.localScale = new Vector3(size, size, size);
                }
            }
        }
    }

}
