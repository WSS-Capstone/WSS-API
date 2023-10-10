namespace WSS.API.Application.Models.ViewModels;

public class OrderDetailResponse
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? PartnerId { get; set; }
    public string? Address { get; set; }
    public DateTime? StartTime { get; set; }
    public string? QuantityService { get; set; }
    public double? Price { get; set; }
    public double? Total { get; set; }
    public string? Description { get; set; }
    public OrderDetailStatus? Status { get; set; }
    public OrderResponse? Order { get; set; }
    public ServiceResponse? Service { get; set; }
    public ICollection<FeedbackResponse> Feedbacks { get; set; }
    public ICollection<TaskResponse> Tasks { get; set; }
}

public enum OrderDetailStatus
{
    ACTIVE = 1,
    INACTIVE = 2,
    DELETED = 3,
}