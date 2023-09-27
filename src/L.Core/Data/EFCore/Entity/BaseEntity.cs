namespace L.Core.Data.EFCore.Entity;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; set; }
}