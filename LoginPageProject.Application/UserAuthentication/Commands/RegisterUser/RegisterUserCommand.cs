using LoginPageProject.Shared.Common;
using MediatR;

namespace LoginPageProject.Application.UserAuthentication.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<Status>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public string? Role { get; set; }
}