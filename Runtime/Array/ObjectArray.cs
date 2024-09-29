namespace MonoSystem
{
    [System.Serializable]
    public sealed class ObjectArray<T>
    {
        public T[] array;
        public T[] ToArray()
        {
            return array;
        }

        public ObjectArray(T[] value)
        {
            array = value;
        }
    }
}