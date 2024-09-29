#if ADDRESSABLES
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MonoSystem
{
    /// <summary>
    /// Autometically run method Addressables.ReleaseInstance() when gameObject is destroying.
    /// </summary>
    public class ReleaseDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
#endif