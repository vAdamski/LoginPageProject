using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Application.Common.Services;
using LoginPageProject.Application.UserAuthentication.Commands.ChangePassword;
using LoginPageProject.Application.UserAuthentication.Commands.LoginUser;
using LoginPageProject.Application.UserAuthentication.Commands.RegisterUser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LoginPageProject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IUserIpVerifier, UserIpVerifier>();
        services.AddScoped<IUserLoginTracker, UserLoginTracker>();
        services.AddScoped<IPasswordVerifierService, PasswordVerifierService>();

        services.AddScoped<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();

        return services;
    }
}