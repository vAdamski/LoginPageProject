using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LoginPageProject.Persistence.Repositories;

public class OldPasswordRepository : IOldPasswordRepository
{
    private readonly ILoginPageProjectDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public OldPasswordRepository(ILoginPageProjectDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task AddPasswordToHistory(string password)
    {
        var userId = await GetUserId();
        
        var oldPassword = new OldUserPassword
        {
            OldPassword = password,
            UserId = userId
        };
        
        await AddPassword(oldPassword);
    }

    public async Task AddPasswordToHistory(string userId, string password)
    {
        var oldPassword = new OldUserPassword
        {
            OldPassword = password,
            UserId = userId
        };
        
        await AddPassword(oldPassword);
    }
    
    private async Task AddPassword(OldUserPassword oldPassword)
    {
        await _context.OldUserPasswords.AddAsync(oldPassword);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
    
    private async Task<string> GetUserId()
    {
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        
        var user = await _userManager.FindByNameAsync(userName);
        
        var guid = user.Id;
        
        return guid;
    }
}