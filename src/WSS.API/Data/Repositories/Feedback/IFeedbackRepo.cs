namespace WSS.API.Data.Repositories.Feedback;

public interface IFeedbackRepo
{
    IQueryable<Models.Feedback> GetFeedbacks(Expression<Func<Models.Feedback, bool>>? predicate = null,
        Expression<Func<Models.Feedback, object>>[]? includeProperties = null);

    Task<Models.Feedback?> GetFeedbackById(Guid id, Expression<Func<Models.Feedback, object>>[]? includeProperties = null);
    Task<Models.Feedback> CreateFeedback(Models.Feedback user, bool tempSave = false);
    Task<Models.Feedback> UpdateFeedback(Models.Feedback user, bool tempSave = false);
    Task<Models.Feedback> DeleteFeedback(Models.Feedback user, bool tempSave = false);
}