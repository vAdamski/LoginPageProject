using FluentValidation;

namespace LoginPageProject.Application.UserAuthentication.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\$\#\@\!\?\*\.\%\^\&\*]+").WithMessage("Your password must contain at least one ($ # @ ! ? * . % ^ & *).");
        RuleFor(x => x.PasswordConfirm).Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}