using UnityEngine;

namespace MonoSystem
{
    public struct ComponentUtils
    {
        public static T GetComponent<T>(GameObject go) where T : Component
        {
            if (!go.TryGetComponent<T>(out T result))
            {
                result = go.AddComponent<T>();
            }
            return result;
        }

        public static void SetActive(GameObject go, bool value)
        {
            go.SetActive(value);
            /*
            var behaviours = go.GetComponentsInChildren<Behaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (!(behaviours[i] is Animator)) behaviours[i].enabled = value;
            }

            var colliders = go.GetComponentsInChildren<Collider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = value;
            }

            var renderers = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = value;
            }

            var bodies = go.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
            {
                if (value)
                {
                    bodies[i].WakeUp();
                }
                else
                {
                    bodies[i].Sleep();
                }
            }
            */
        }
    }
}