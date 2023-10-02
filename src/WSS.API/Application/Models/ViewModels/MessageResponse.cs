namespace WSS.API.Application.Models.ViewModels;

public class MessageResponse
{
    public Guid Id { get; set; }
    public Guid? ToId { get; set; }
    public Guid? FromId { get; set; }
    public string? Content { get; set; }
    public DateTime? CreateDate { get; set; }
}