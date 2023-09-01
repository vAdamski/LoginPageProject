using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using LoginPageProject.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Application.UserAuthentication.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Status>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IOldPasswordRepository _oldPasswordRepository;
    private readonly IPasswordVerifierService _passwordVerifierService;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager,
        IOldPasswordRepository oldPasswordRepository, IPasswordVerifierService passwordVerifierService)
    {
        this.userManager = userManager;
        _oldPasswordRepository = oldPasswordRepository;
        _passwordVerifierService = passwordVerifierService;
    }

    public async Task<Status> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var status = new Status();

        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            status.Message = "User does not exist";
            status.StatusCode = 0;
            return status;
        }

        if (await _passwordVerifierService.IfPasswordCanBeChanged())
        {
            status.Message = "Password was changed less than 1 day ago!";
            status.StatusCode = 0;
            return status;
        }
        
        if (await _passwordVerifierService.IsPasswordWasUsedBefore(request.NewPassword))
        {
            status.Message = "Password was used before";
            status.StatusCode = 0;
            return status;
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
        {
            await _oldPasswordRepository.AddPasswordToHistory(request.NewPassword);
            status.Message = "Password has updated successfully";
            status.StatusCode = 1;
        }
        else
        {
            status.Message = "Some error occcured";
            status.StatusCode = 0;
        }

        return status;
    }
}