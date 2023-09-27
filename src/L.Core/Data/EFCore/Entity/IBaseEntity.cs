namespace L.Core.Data.EFCore.Entity;

public interface IBaseEntity<TKey>
{
    public TKey Id { get; set; }
}