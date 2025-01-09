#if ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MonoSystem
{
    public class ReleaseDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
#endif