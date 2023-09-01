using LoginPageProject.Shared.Common;
using MediatR;

namespace LoginPageProject.Application.UserAuthentication.Commands.LoginUser;

public class LoginUserCommand : IRequest<Status>
{
    public string Username { get; set; }
    public string Password { get; set; }
}