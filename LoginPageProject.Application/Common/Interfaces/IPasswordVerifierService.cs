namespace LoginPageProject.Application.Common.Interfaces;

public interface IPasswordVerifierService
{
    Task<bool> IsPasswordWasUsedBefore(string password);
    Task<bool> IsCurrentPasswordOutdated();
    Task<bool> IfPasswordCanBeChanged();
}