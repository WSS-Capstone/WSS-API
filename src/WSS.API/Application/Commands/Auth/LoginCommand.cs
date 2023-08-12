using MediatR;
using WSS.API.Application.Models.ViewModels;

namespace WSS.API.Application.Commands.Auth;

public class LoginCommand : IRequest<LoginInfo>
{
    public string Username { get; set; }
    public string Password { get; set; }
}