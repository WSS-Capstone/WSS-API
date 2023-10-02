namespace WSS.API.Application.Models.ViewModels;

public class FeedbackResponse
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? Rating { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? UserId { get; set; }
    public FeedbackStatus? Status { get; set; } 
}

public enum FeedbackStatus
{
    New = 0,
    Approved = 1,
    Rejected = 2
}