using LoginPageProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginPageProject.Application.Common.Interfaces;

public interface ILoginPageProjectDbContext
{
    DbSet<UserActionInfo> UserActionInfos { get; set; }
    DbSet<OldUserPassword> OldUserPasswords { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}