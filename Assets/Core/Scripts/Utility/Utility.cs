using UnityEngine;
using UnityEngine.Rendering;

using Unity.Collections;

namespace RksAdventure.Core
{
    public static class Utility
    {
        public static T FindComponent<T>(GameObject gameObject, bool isNew = false)
            where T : Component
            => FindComponent<T>(gameObject.transform, isNew);

        public static T FindComponent<T>(Transform transform, bool isNew = false)
            where T : Component
        {
            T comp = transform.GetComponent<T>();

            if (comp == null) return isNew ? transform.gameObject.AddComponent<T>() : null;
            return comp;
        }

        public static T FindComponentInChild<T>(Transform transform, string name)
            where T : Component
        {
            var target = transform.name.Equals(name) ? transform : FindChild(transform);

            if (target != null) return target.GetComponent<T>();
            return null;

            Transform FindChild(Transform t)
            {
                for (int i = 0; i < t.childCount; ++i)
                {
                    var child = t.GetChild(i);
                    if (child.name.Equals(name)) return child;

                    if (child.childCount != 0)
                    {
                        var value = FindChild(child);
                        if (value != null) return value;
                    }
                }

                return null;
            }
        }
    }
}
