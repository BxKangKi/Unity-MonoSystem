using System.Threading;

namespace MonoSystem
{
    public struct CancellationExtension
    {
        public static void Regenerate(ref CancellationTokenSource source)
        {
            Cancel(ref source);
            Reset(ref source);
        }

        public static void Reset(ref CancellationTokenSource source)
        {
            source?.Dispose();
            source = new CancellationTokenSource();
        }

        public static void Cancel(ref CancellationTokenSource source)
        {
            source.Cancel();
        }

        public static CancellationToken Token(CancellationTokenSource source)
        {
            return source.Token;
        }
    }
}