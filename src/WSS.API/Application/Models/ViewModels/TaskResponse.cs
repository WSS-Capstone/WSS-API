namespace WSS.API.Application.Models.ViewModels;

public class TaskResponse
{
    public Guid Id { get; set; }
    public Guid? StaffId { get; set; }
    public Guid? PartnerId { get; set; }
    public Guid? OrderDetailId { get; set; }
    public string? TaskName { get; set; }

    public string? Content { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ImageEvidence { get; set; }

    public TaskStatus? Status { get; set; }
}

public enum TaskStatus
{
    TO_DO = 1,
    IN_PROGRESS = 2,
    DONE = 3,
}
