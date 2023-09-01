using LoginPageProject.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LoginPageProject.Application.Common.Services;

public class UserIpVerifier : IUserIpVerifier
{
    private readonly List<string> _blockedIps = new();

    public UserIpVerifier(IConfiguration configuration)
    {
        _blockedIps = configuration.GetSection("BlockedIps").Get<List<string>>();
    }
    
    public async Task<bool> IsIpBlocked(string ip)
    {
        return _blockedIps.Contains(ip);
    }
}