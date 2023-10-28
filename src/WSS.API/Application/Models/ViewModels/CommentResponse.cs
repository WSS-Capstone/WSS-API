namespace WSS.API.Application.Models.ViewModels;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid? TaskId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
    public Guid? CreateBy { get; set; }
}