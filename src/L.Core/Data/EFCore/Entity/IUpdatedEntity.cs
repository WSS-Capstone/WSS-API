namespace L.Core.Data.EFCore.Entity;

public interface IUpdatedEntity<TKey>
{
    DateTime? UpdatedAt { get; set; }
    TKey? UpdatedBy { get; set; }
}