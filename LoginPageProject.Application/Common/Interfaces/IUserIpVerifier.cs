namespace LoginPageProject.Application.Common.Interfaces;

public interface IUserIpVerifier
{
    Task<bool> IsIpBlocked(string ip);
}