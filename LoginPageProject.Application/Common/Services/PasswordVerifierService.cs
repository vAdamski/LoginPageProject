using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoginPageProject.Application.Common.Services;

public class PasswordVerifierService : IPasswordVerifierService
{
    private readonly ILoginPageProjectDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public PasswordVerifierService(ILoginPageProjectDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }
    
    public async Task<bool> IsPasswordWasUsedBefore(string password)
    {
        var userId = await GetUserId();
        
        var oldPassword = await _context.OldUserPasswords
            .OrderByDescending(x => x.Created)
            .Take(20)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.OldPassword == password);
        
        return oldPassword != null;
    }

    public async Task<bool> IfPasswordCanBeChanged()
    {
        var lastPasswordDate = await GetLastPassword();
        
        if (lastPasswordDate.AddDays(1) > DateTime.UtcNow)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> IsCurrentPasswordOutdated()
    {
        var passwordExpirationDays = _configuration.GetValue<int>("PasswordExpirationDays");
        
        var lastPasswordDate = await GetLastPassword();
        
        var currentDate = DateTime.UtcNow;
        
        var days = (currentDate - lastPasswordDate).Days;
        
        var isPasswordExpired = days > passwordExpirationDays;
        
        return isPasswordExpired;
    }

    private async Task<DateTime> GetLastPassword()
    {
        var userId = await GetUserId();

        var lastPasswordDate = await _context.OldUserPasswords.Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Created)
            .Take(1)
            .Select(x => x.Created)
            .FirstOrDefaultAsync();

        return lastPasswordDate;
    }
    
    private async Task<string> GetUserId()
    {
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        
        var user = await _userManager.FindByNameAsync(userName);
        
        var guid = user.Id;
        
        return guid;
    }
}