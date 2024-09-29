
#if ADDRESSABLES
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoSystem
{
    [Serializable]
    public class AssetReference : UnityEngine.AddressableAssets.AssetReference
    {
        /// <summary>
        /// InstantiateAsync coroutine.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerator InstantiateAsync(MonoBehaviour behaviour, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(parent, Vector3.zero, Quaternion.identity));
        }


        /// <summary>
        /// InstantiateAsync coroutine.
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerator InstantiateAsync(MonoBehaviour behaviour, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            yield return behaviour.StartCoroutine(InstantiateAsync_Internal(parent, pos, rot));
        }

        private IEnumerator InstantiateAsync_Internal(Transform parent, Vector3 pos, Quaternion rot)
        {
            var handle = base.InstantiateAsync(pos, rot, parent);
            yield return handle;
            handle.Result.AddComponent<ReleaseDestroy>();
        }

        /// <summary>
        /// LoadSceneAsync coroutine.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = base.LoadSceneAsync(mode, false);
            yield return handle;
            yield return handle.Result.ActivateAsync();
        }
    }
}
#endif
