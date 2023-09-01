using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Application.Common.Services;

public class UserLoginTracker : IUserLoginTracker
{
    private readonly ILoginPageProjectDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserLoginTracker(ILoginPageProjectDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task TrackLogin(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var guid = user.Id;
        
        var userActionInfo = new UserActionInfo
        {
            UserId = guid,
            Action = "Login",
            ActionDate = DateTime.Now,
        };
        
        await _context.UserActionInfos.AddAsync(userActionInfo);
        await _context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task TrackLogout()
    {
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        var user = await _userManager.FindByNameAsync(userName);
        var guid = user.Id;
        
        var userActionInfo = new UserActionInfo
        {
            UserId = guid,
            Action = "Logout",
            ActionDate = DateTime.Now,
        };
        
        await _context.UserActionInfos.AddAsync(userActionInfo);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
    
}