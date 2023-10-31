namespace WSS.API.Application.Models.ViewModels;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public UserResponse? Staff { get; set; }
    public UserResponse? Partner { get; set; }
    public ServiceResponse? Service { get; set; }
    public OrderResponse? Order { get; set; }
    public string? TaskName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ImageEvidence { get; set; }

    public TaskStatus? Status { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

public enum TaskStatus
{
    EXPECTED = 0,
    TO_DO = 1,
    IN_PROGRESS = 2,
    DONE = 3,
}
