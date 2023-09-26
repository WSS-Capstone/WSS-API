namespace WSS.API.Application.Models.ViewModels;

public class LoginInfo
{
    public string Token { get; set; }
    public DateTime ExpiresIn { get; set; }
}