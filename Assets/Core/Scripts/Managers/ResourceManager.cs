using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.U2D;

using RksAdventure.Core.Components;

namespace RksAdventure.Core.Managers
{
    public enum ResourceType : int
    {
        None = 0,
        Sprites,
        Max
    }

    public class ResourceManager : BaseSingleton<ResourceManager>
    {
        private readonly Dictionary<ResourceType, Object[]> m_Resources = new Dictionary<ResourceType, Object[]>();

        private void Awake()
        {
            for (ResourceType i = ResourceType.Sprites, length = ResourceType.Max; i < length; ++i)
            {
                var values = Resources.LoadAll(i.ToString());
                m_Resources.Add(i, values);
            }
        }

        public static T GetSource<T>(ResourceType type, string name)
            where T : Object
        {
            var get = Get;

            if (!get.m_Resources.ContainsKey(type)) return null;
            var values = get.m_Resources[type];

            return (T)System.Array.Find(get.m_Resources[type], x => x.name.Equals(name));
        }

        public static Sprite[] GetPartSprites(PawnPart part, string groupName)
        {
            string resourceName = $"{part} Sprites";
            Sprite[] values = new Sprite[4];

            SpriteAtlas atlas = (SpriteAtlas)Get.m_Resources[ResourceType.Sprites].First(x => x.name.Equals(resourceName));
            Sprite[] sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);

            sprites = System.Array.FindAll(sprites, x => x.name.Contains(groupName));
            for (int i = 0; i < values.Length; ++i)
            {
                PawnDirection dir = (PawnDirection)i;
#if UNITY_EDITOR
                string str = $"{dir}(Clone)";
#else
                string str = dir.ToString();
#endif
                var value = sprites.FirstOrDefault(x => x.name.EndsWith(str, System.StringComparison.OrdinalIgnoreCase));
                values[i] = value;
            }

            return values;
        }
    }
}
