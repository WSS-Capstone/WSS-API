using MediatR;
using WSS.API.Infrastructure.Config;

namespace WSS.API.Application.Commands.Account;

public class CreateAccountCommand : IRequest<Data.Models.Customer?>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int Gender { get; set; }
    public RoleEnum RoleName { get; set; }
}