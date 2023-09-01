using LoginPageProject.Shared.Common;
using MediatR;

namespace LoginPageProject.Application.UserAuthentication.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Status>
{
    public string Username { get; set; }
    public string ? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string ? PasswordConfirm { get; set; }
}