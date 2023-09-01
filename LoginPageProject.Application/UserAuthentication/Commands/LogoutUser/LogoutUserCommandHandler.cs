using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Application.UserAuthentication.Commands.LogoutUser;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Unit>
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IUserLoginTracker _userLoginTracker;

    public LogoutUserCommandHandler(SignInManager<ApplicationUser> signInManager, IUserLoginTracker userLoginTracker)
    {
        this.signInManager = signInManager;
        _userLoginTracker = userLoginTracker;
    }
    
    public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();
        await _userLoginTracker.TrackLogout();
        return Unit.Value;
    }
}