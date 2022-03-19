using UnityEngine;

namespace RKCore
{
    public class AspectScaler : MonoBehaviour
    {
        public enum Scales
        {
            Constrained = 0,
            ConstrainedOnMobiles
        }

        [SerializeField] private Scales m_ScaleType = Scales.ConstrainedOnMobiles;
        [SerializeField] private int m_Width = 1920;
        [SerializeField] private int m_Height = 1080;

        public int Width => m_Width;
        public int Height
        {
            get
            {
                Vector2 screen = new Vector2(Screen.width, Screen.height);
                float aspect = screen.x / screen.y;

                return ((float)m_Width / m_Height > aspect) ? Mathf.RoundToInt(m_Width / aspect) : m_Height;
            }
        }

#if UNITY_EDITOR
        private void OnEnable() => UpdateScale();

        public float GetPixelSizeAdjustment(int height)
        {
            height = Mathf.Max(2, height);

            if (m_ScaleType == Scales.Constrained)
                return (float)Height / height;

            if (height < m_Height) return (float)m_Height / height;
            if (height > m_Height) return (float)Screen.height / height;

            return 1f;
        }
#else
        private void Awake() => UpdateScale();
#endif
        public void UpdateScale()
        {
            float calcActiveHeight = Height;
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
