namespace WSS.API.Application.Models.ViewModels;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
    public virtual AccountResponse IdNavigation { get; set; } = null!;
    public virtual ICollection<CartResponse> Carts { get; set; }
    public virtual ICollection<FeedbackResponse> Feedbacks { get; set; }
    public virtual ICollection<OrderResponse> Orders { get; set; }
}