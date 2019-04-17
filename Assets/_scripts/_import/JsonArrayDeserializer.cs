using System;
using UnityEngine;

namespace Model
{
    // The original code is from a "Stack Overflow" post: 
    // - https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity.
    // The serialization part was removed as it is not necessary for this project.
    public class JsonArrayDeserializer
    {
        public T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(this.Wrap(json));
            return wrapper.Items;
        }

        private string Wrap(string json)
        {
            return "{\"Items\":" + json + "}";
        }

        [Serializable]
        private class Wrapper<T>
        {
#pragma warning disable 0649
            public T[] Items;
#pragma warning restore 0649
        }
    }
}
