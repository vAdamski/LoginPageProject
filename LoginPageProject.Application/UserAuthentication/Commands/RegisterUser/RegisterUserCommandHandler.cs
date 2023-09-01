using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using LoginPageProject.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Application.UserAuthentication.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Status>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IOldPasswordRepository _oldPasswordRepository;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IOldPasswordRepository oldPasswordRepository)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        _oldPasswordRepository = oldPasswordRepository;
    }

    public async Task<Status> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var status = new Status();
        var userExists = await userManager.FindByNameAsync(request.Username);
        if (userExists != null)
        {
            status.StatusCode = 0;
            status.Message = "User already exist";
            return status;
        }

        ApplicationUser user = new ApplicationUser()
        {
            Name = request.Username,
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "User creation failed";
            return status;
        }

        if (!await roleManager.RoleExistsAsync(request.Role))
            await roleManager.CreateAsync(new IdentityRole(request.Role));


        if (await roleManager.RoleExistsAsync(request.Role))
        {
            await userManager.AddToRoleAsync(user, request.Role);
        }

        await _oldPasswordRepository.AddPasswordToHistory(user.Id, request.Password);

        status.StatusCode = 1;
        status.Message = "You have registered successfully";
        
        return status;
    }
}