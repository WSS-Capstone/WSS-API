namespace L.Core.Data.EFCore.Entity;

public interface ICreatedEntity<TKey>
{
    DateTime CreatedAt { get; set; }
    TKey? CreatedBy { get; set; }
}