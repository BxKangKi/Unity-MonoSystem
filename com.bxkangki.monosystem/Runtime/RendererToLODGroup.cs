using UnityEngine;

namespace MonoSystem
{
    public class RendererToLODGroup
    {
        public GameObject gameObject;
        public float cullingPoint = 0.01f;
        public bool animate = false;

        public void Start()
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers != null)
            {
                var lodGroup = gameObject.AddComponent<LODGroup>();
                var lods = new LOD[1];
                lods[0].renderers = renderers;
                lods[0].screenRelativeTransitionHeight = cullingPoint;
                lodGroup.SetLODs(lods);
                lodGroup.animateCrossFading = animate;
            }
        }
    }
}