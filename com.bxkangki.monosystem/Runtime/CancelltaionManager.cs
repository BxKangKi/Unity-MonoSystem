using System.Threading;

public struct CancellationManager
{
    private static CancellationTokenSource cancellationTokenSource;
    public static CancellationTokenSource Source { get => cancellationTokenSource; set => cancellationTokenSource = value; }
    public static CancellationToken Token { get => cancellationTokenSource.Token; }

}