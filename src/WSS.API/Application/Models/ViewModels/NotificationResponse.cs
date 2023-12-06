namespace WSS.API.Application.Models.ViewModels;

public class NotificationResponse
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public Guid? UserId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? IsRead { get; set; }

    public virtual User? User { get; set; }
}
public enum NotificationIsRead
{
    UnRead = 0,
    Read = 1
}