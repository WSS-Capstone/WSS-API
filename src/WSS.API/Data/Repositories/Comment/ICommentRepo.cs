namespace WSS.API.Data.Repositories.Comment;

public interface ICommentRepo
{
    IQueryable<Models.Comment> GetComments(Expression<Func<Models.Comment, bool>>? predicate = null,
        Expression<Func<Models.Comment, object>>[]? includeProperties = null);

    Task<Models.Comment?> GetCommentById(Guid id, Expression<Func<Models.Comment, object>>[]? includeProperties = null);
    Task<Models.Comment> CreateComment(Models.Comment user, bool tempSave = false);
    Task<Models.Comment> UpdateComment(Models.Comment user, bool tempSave = false);
    Task<Models.Comment> DeleteComment(Models.Comment user, bool tempSave = false);
}