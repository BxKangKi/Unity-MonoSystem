#if ADDRESSABLES
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoSystem
{
    /// <summary>
    /// Overriden Unity's AssetReference class.
    /// Support InstantiateAsync and LoadSceneAsync method.
    /// </summary>
    [Serializable]
    public class AssetReference : UnityEngine.AddressableAssets.AssetReference
    {
        public async UniTask<GameObject> InstantiateAsync(CancellationToken cancellationToken)
        {
            return await InstantiateAsyncInternal(null, Vector3.zero, Quaternion.identity, cancellationToken);
        }


        public async UniTask<GameObject> InstantiateAsync(Vector3 pos, Quaternion rot, CancellationToken cancellationToken)
        {
            return await InstantiateAsyncInternal(null, pos, rot, cancellationToken);
        }

        public async UniTask<GameObject> InstantiateAsync(Transform parent, CancellationToken cancellationToken)
        {
            return await InstantiateAsyncInternal(parent, Vector3.zero, Quaternion.identity, cancellationToken);

        }

        private async UniTask<GameObject> InstantiateAsyncInternal(Transform parent, Vector3 pos, Quaternion rot, CancellationToken cancellationToken)
        {
            var go = await base.InstantiateAsync(pos, rot, parent).WithCancellation(cancellationToken);
            go.AddComponent<ReleaseDestroy>();
            return go;
        }

        public async UniTask LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            await LoadSceneAsyncInternal(mode);
        }

        private async UniTask LoadSceneAsyncInternal(LoadSceneMode mode = LoadSceneMode.Single)
        {
            var instance = await base.LoadSceneAsync(mode, false);
            await instance.ActivateAsync();
        }
    }
}
#endif