using UnityEngine;

namespace MonoSystem
{
    public struct TransformUtils
    {
        public static void SetupTransform(GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
            go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            go.transform.localScale = Vector3.one;
        }
    }
}