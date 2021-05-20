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
    }
}
