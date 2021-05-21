using UnityEngine;

namespace RksAdventure.Core.Managers
{
    public class BaseSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T m_Instance;
        public static T Get
        {
            get
            {
                if (m_Instance == null)
                {
                    var finds = FindObjectsOfType<T>();
                    if (finds.Length > 1) throw new System.Exception($"{typeof(T).Name} is too many");
                    if (finds.Length == 1) m_Instance = finds[0];
                    else
                    {
                        m_Instance = new GameObject($"{typeof(T).Name} Manager").AddComponent<T>();
                        DontDestroyOnLoad(m_Instance.gameObject);
                    }
                }

                return m_Instance;
            }
        }
    }
}
