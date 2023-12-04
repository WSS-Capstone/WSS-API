namespace WSS.API.Application.Models.ViewModels;

public class OrderDetailResponse
{
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? PartnerId { get; set; }
    public string? Address { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public double? Price { get; set; }
    public double? Total { get; set; }
    public string? Description { get; set; }
    public OrderDetailStatus? Status { get; set; }
    public OrderResponse? Order { get; set; }
    public ServiceResponse? Service { get; set; }
    public ICollection<FeedbackResponse> Feedbacks { get; set; }
    public ICollection<TaskResponse> Tasks { get; set; }
    public bool InCombo { get; set; } = false;
    
}

public enum OrderDetailStatus
{
    PENDING = 1,
    INPROCESS = 2,
    DONE = 3,
    CANCEL = 4
}