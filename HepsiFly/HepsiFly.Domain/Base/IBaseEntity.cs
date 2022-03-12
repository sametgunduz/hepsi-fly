namespace HepsiFly.Domain.Base;

public interface IBaseEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Id { get; }
    DateTime CreatedAt { get; set; }
}