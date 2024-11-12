using System.Threading;
using MonoSystem;
using UnityEngine;

public class CancellationSystem : MonoBehaviour
{
    private void Awake()
    {
        CancellationManager.Source?.Dispose();
        CancellationManager.Source = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        CancellationManager.Source?.Cancel();
        CancellationManager.Source?.Dispose();
    }
}