using System.Text;
using UnityEngine;

namespace FileSystem
{
    public readonly struct Serialization
    {
        /// <summary>
        /// Deserialize object from byte array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data));
        }

        /// <summary>
        /// Deserialize object to byte array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T data)
        {
            string json = JsonUtility.ToJson(data, false);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}