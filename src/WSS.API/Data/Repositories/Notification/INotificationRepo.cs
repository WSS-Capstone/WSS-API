namespace WSS.API.Data.Repositories.Notification;

public interface INotificationRepo
{
    IQueryable<Models.Notification> GetNotifications(Expression<Func<Models.Notification, bool>>? predicate = null,
        Expression<Func<Models.Notification, object>>[]? includeProperties = null);

    Task<Models.Notification?> GetNotificationById(Guid id, Expression<Func<Models.Notification, object>>[]? includeProperties = null);
    Task<Models.Notification> CreateNotification(Models.Notification order, bool tempSave = false);
    Task<Models.Notification> UpdateNotification(Models.Notification order, bool tempSave = false);
    Task<Models.Notification> DeleteNotification(Models.Notification order, bool tempSave = false);
}