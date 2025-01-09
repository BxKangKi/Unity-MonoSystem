using UnityEngine;

namespace MonoSystem
{
    public class SelfDestroy : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}