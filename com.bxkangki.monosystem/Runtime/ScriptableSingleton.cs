using UnityEngine;

namespace MonoSystem
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] asset = Resources.LoadAll<T>("");
                    if (asset == null || asset.Length < 1 || asset.Length > 1)
                    {
                        FileSystem.LogSystem.Text($"Asset is not exist or more than one in 'Resources' directory");
                    }
                    instance = asset[0];
                }

                return instance;
            }
        }
    }
}