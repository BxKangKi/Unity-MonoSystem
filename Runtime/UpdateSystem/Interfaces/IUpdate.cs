namespace MonoSystem
{
    public interface IUpdate : IUpdateSystem
    {
        void OnUpdate(int priority);
    }
}