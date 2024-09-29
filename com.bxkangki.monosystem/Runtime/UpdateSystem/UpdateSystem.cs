using UnityEngine;

namespace MonoSystem
{
    public sealed class UpdateSystem : MonoBehaviour, IUpdateSystem, ISystem
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void OnEnable()
        {
            UpdateManager.OnEnable();
        }

        public void OnDisable()
        {
            UpdateManager.OnDisable();
        }


        // Update is called once per frame
        private void Update()
        {
            UpdateManager.Update();
        }

        private void FixedUpdate()
        {
            UpdateManager.FixedUpdate();
        }

        private void LateUpdate()
        {
            UpdateManager.LateUpdate();
        }
    }
}