using Task = WSS.API.Data.Models.Task;

namespace WSS.API.Application.Models.ViewModels;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid? TaskId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
    public Guid? CreateBy { get; set; }
    public virtual TaskResponse? Task { get; set; }
    public virtual UserResponse? CreateByNavigation { get; set; }
}