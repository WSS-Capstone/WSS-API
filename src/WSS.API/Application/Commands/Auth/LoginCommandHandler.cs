using FirebaseAdmin;

using MediatR;
using WSS.API.Application.Models.ViewModels;

namespace WSS.API.Application.Commands.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginInfo>
{
    // private readonly FirebaseAdmin _firebaseAuth;

    // public LoginCommandHandler(FirebaseApp firebaseAuth)
    // {
    //     _firebaseAuth = firebaseAuth;
    // }

    /// <inheritdoc />
    public Task<LoginInfo> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // this._firebaseAuth
        throw new NotImplementedException();
    }
}