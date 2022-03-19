using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEditor;

using Newtonsoft.Json;

namespace RKEditor
{
    public static class SerializeUtility
    {
        public class SerializedException : System.Exception
        {
            public SerializedException(string message) : base(
                $"<b><color=red> {"ErrCode"} </color>{message}</b>")
            { }
        }

        public static SerializedProperty GetProperty(SerializedObject serializedObject, string prop)
        {
            if (serializedObject == null || string.IsNullOrWhiteSpace(prop))
                throw new SerializedException($"GetProperty() is Null");

            var property = serializedObject.FindProperty(prop);
            return property ?? throw new SerializedException($"GetProperty() prop Not Found");
        }

        public static SerializedProperty GetProperty(SerializedProperty serializedProperty, string prop)
        {
            if (serializedProperty == null || string.IsNullOrWhiteSpace(prop))
                throw new SerializedException($"GetProperty() is Null");

            var property = serializedProperty.FindPropertyRelative(prop);
            return property ?? throw new SerializedException($"GetProperty() prop Not Found");
        }

        public static void BinarySerialize(string fullPath, object obj)
        {
            BinaryFormatter binary = new BinaryFormatter();

            using FileStream fs = new FileStream(fullPath, FileMode.Create);
            binary.Serialize(fs, obj);
        }

        public static T BinaryDeserialize<T>(string fullPath)
        {
            BinaryFormatter binary = new BinaryFormatter();
            using FileStream fs = new FileStream(fullPath, FileMode.Open);

            T value = (T)binary.Deserialize(fs);

            return value;
        }

        public static string JsonSerialize(object obj)
                => JsonConvert.SerializeObject(obj, Formatting.Indented);

        public static void JsonSerialize(string fullPath, object obj)
        {
            string str = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fullPath, str, System.Text.Encoding.UTF8);
        }

        public static void JsonSerialize(string fullPath, object obj, JsonSerializerSettings settings)
        {
            string str = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
            File.WriteAllText(fullPath, str, System.Text.Encoding.UTF8);
        }

        public static object JsonDeserialize(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            string str = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject(str);
        }

        public static object JsonDeserialize(string fullPath, System.Type type)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            string str = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject(
                str,
                type,
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
        }

        public static T JsonDeserialize<T>(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            string str = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static T JsonDeserialize<T>(string fullPath, JsonSerializerSettings settings)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            string str = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(str, settings);
        }

        public static void JsonPopulateObject(string fullPath, UnityEngine.ScriptableObject obj)
        {
            string str = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);
            JsonConvert.PopulateObject(str, obj);
        }
    }
}
