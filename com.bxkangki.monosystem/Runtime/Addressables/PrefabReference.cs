#if ADDRESSABLES
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MonoSystem
{
    /// <summary>
    /// Inherit UnityEngine.AddressableAssets.AssetReference class. Specialized for GameObject AssetReference.
    /// </summary>
    [Serializable]
    public class PrefabReference : UnityEngine.AddressableAssets.AssetReference
    {
        protected GameObject gameObject;
        //private bool currenIsLoaded = false;
        public bool IsLoaded { get => gameObject != null; }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Destroy()
        {
            //if (currenIsLoaded && gameObject != null)
            if (gameObject != null)
            {
                //currenIsLoaded = false;
                //GameObject.Destroy(gameObject);
                GameObject.Destroy(gameObject);
            }
        }


        public void Reset()
        {
            Destroy();
            //currenIsLoaded = false;
        }

        public void InstantiateAsync(Vector3 position, Quaternion rotation)
        {
            InstantiateAsyncInternal(position, rotation, null).Forget();
        }

        public void InstantiateAsync()
        {
            InstantiateAsyncInternal(Vector3.zero, Quaternion.identity, null).Forget();
        }

        public async UniTask<T> GetComponentAsync<T>() where T : Component
        {
            await UniTask.WaitUntil(() => gameObject != null);
            return gameObject.GetComponent<T>();
        }

        private async UniTaskVoid InstantiateAsyncInternal(Vector3 position, Quaternion rotation, Transform parent)
        {
            //if (!currenIsLoaded && gameObject == null)
            if (gameObject == null)
            {
                //currenIsLoaded = true;
                gameObject = await InstantiateAsync(position, rotation, parent);
                gameObject.AddComponent<ReleaseDestroy>();
            }
        }
    }
}
#endif