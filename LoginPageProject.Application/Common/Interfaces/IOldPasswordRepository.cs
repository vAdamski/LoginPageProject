namespace LoginPageProject.Application.Common.Interfaces;

public interface IOldPasswordRepository
{
    Task AddPasswordToHistory(string password);
    Task AddPasswordToHistory(string userId, string password);
}