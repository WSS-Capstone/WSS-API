namespace WSS.API.Data.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? Gender { get; set; }

    public DateTime? CreateAt { get; set; }

    public Guid? CreateBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public Guid? UpdateBy { get; set; }

    public bool? IsActive { get; set; }
}