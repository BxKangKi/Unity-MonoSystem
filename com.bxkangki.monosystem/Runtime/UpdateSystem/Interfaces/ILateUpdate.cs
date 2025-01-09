namespace MonoSystem
{
    public interface ILateUpdate : IUpdateSystem
    {
        void OnLateUpdate(int priority);
    }
}