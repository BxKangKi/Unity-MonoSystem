using System.Threading;
using UnityEngine;

public sealed class CancellationSystem : MonoBehaviour
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