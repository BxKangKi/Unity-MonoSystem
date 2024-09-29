namespace MonoSystem
{
    public interface IFixedUpdate : IUpdateSystem
    {
        void OnFixedUpdate(int priority);
    }
}