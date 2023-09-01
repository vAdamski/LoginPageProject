using FluentValidation;

namespace LoginPageProject.Application.UserAuthentication.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")
            .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
            .Matches(@"[\$\#\@\!\?\*\.\%\^\&\*]+").WithMessage("Your password must contain at least one ($ # @ ! ? * . % ^ & *).");
        RuleFor(x => x.PasswordConfirm).Equal(x => x.NewPassword).WithMessage("Passwords do not match");
    }
}