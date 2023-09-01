using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using LoginPageProject.Shared.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginPageProject.Application.UserActionInfos.Queries.GetUserActionInfos;

public class GetUserActionInfosQueryHandler : IRequestHandler<GetUserActionInfosQuery, UserActionInfosVm>
{
    private readonly ILoginPageProjectDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserActionInfosQueryHandler(ILoginPageProjectDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    
    public async Task<UserActionInfosVm> Handle(GetUserActionInfosQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        var user = await _userManager.FindByNameAsync(currentUser);
        var guid = user.Id;
        
        var userActionInfos = await _context.UserActionInfos
            .Where(x => x.UserId == guid)
            .OrderByDescending(x => x.ActionDate)
            .ToListAsync(cancellationToken);

        var userActionInfosVm = new UserActionInfosVm
        {
            UserActionInfos = userActionInfos
        };

        return userActionInfosVm;
    }
}