namespace WSS.API.Application.Models.ViewModels;

public class FeedbackResponseForGetServiceResponse
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? Rating { get; set; }
    public ServiceResponse Service { get; set; }
    public UserResponse User { get; set; }
    public FeedbackStatus? Status { get; set; } 
}