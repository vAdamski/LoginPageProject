using System.Security.Claims;
using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using LoginPageProject.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Application.UserAuthentication.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Status>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IUserIpVerifier _userIpVerifier;
    private readonly IUserLoginTracker _userLoginTracker;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly int _maxLoginAttempts = 3;
    private readonly int _blockedTimeInMinutes = 1;

    public LoginUserCommandHandler(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IUserIpVerifier userIpVerifier,
        IUserLoginTracker userLoginTracker,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        _userIpVerifier = userIpVerifier;
        _userLoginTracker = userLoginTracker;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Status> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var status = new Status();

        if (await _userIpVerifier.IsIpBlocked(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
        {
            status.StatusCode = 0;
            status.Message = "User is blocked";
            return status;
        }


        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            status.StatusCode = 0;
            status.Message = "Invalid username";
            return status;
        }

        if (IsUserBlocked(user))
        {
            status.StatusCode = 0;
            status.Message = $"User is blocked to {user.BlockedTo}";
            return status;
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            status.StatusCode = 0;
            status.Message = "Invalid Password";
            await UpdateUserLoginAttempts(user);
            return status;
        }

        var signInResult = await signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (signInResult.Succeeded)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            status.StatusCode = 1;
            status.Message = "Logged in successfully";
            await ClearUserLoginAttempts(user);
            await _userLoginTracker.TrackLogin(user.UserName);
        }
        else if (signInResult.IsLockedOut)
        {
            status.StatusCode = 0;
            status.Message = "User is locked out";
        }
        else
        {
            status.StatusCode = 0;
            status.Message = "Error on logging in";
        }

        return status;
    }

    private async Task UpdateUserLoginAttempts(ApplicationUser user)
    {
        user.LoginAttempts++;

        if (user.LoginAttempts >= _maxLoginAttempts)
        {
            BlockUserFor(user, _blockedTimeInMinutes);
            user.LoginAttempts = 0;
        }

        await userManager.UpdateAsync(user);
    }
    
    private async Task ClearUserLoginAttempts(ApplicationUser user)
    {
        user.LoginAttempts = 0;
        await userManager.UpdateAsync(user);
    }

    private void BlockUserFor(ApplicationUser user, int timeInMinutes)
    {
        user.BlockedTo = DateTime.Now.AddMinutes(timeInMinutes);
    }

    private bool IsUserBlocked(ApplicationUser user)
    {
        return user.BlockedTo > DateTime.Now;
    }
}