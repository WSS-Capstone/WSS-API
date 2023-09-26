namespace WSS.API.Data.Repositories.Message;

public interface IMessageRepo
{
    IQueryable<Models.Message> GetMessages(Expression<Func<Models.Message, bool>>? predicate = null,
        Expression<Func<Models.Message, object>>[]? includeProperties = null);

    Task<Models.Message?> GetMessageById(Guid id, Expression<Func<Models.Message, object>>[]? includeProperties = null);
    Task<Models.Message> CreateMessage(Models.Message user, bool tempSave = false);
    Task<Models.Message> UpdateMessage(Models.Message user, bool tempSave = false);
    Task<Models.Message> DeleteMessage(Models.Message user, bool tempSave = false);
}