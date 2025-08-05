// IPool.cs
public interface IPool<in T> where T : class
{
    void Despawn(T item);
}
