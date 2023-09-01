using System.Reflection;
using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Common;
using LoginPageProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginPageProject.Persistence;


public class LoginPageProjectDbContext : IdentityDbContext<ApplicationUser>, ILoginPageProjectDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginPageProjectDbContext(DbContextOptions<LoginPageProjectDbContext> options) : base(options)
    {
    }
    
    public LoginPageProjectDbContext(DbContextOptions<LoginPageProjectDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<OldUserPassword>()
            .HasOne<ApplicationUser>()
            .WithMany(x => x.OldUserPasswords)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);;

        modelBuilder.Entity<UserActionInfo>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserActionInfos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public DbSet<UserActionInfo> UserActionInfos { get; set; }
    public DbSet<OldUserPassword> OldUserPasswords { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.Entity.ModifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name ?? "";
                    entry.Entity.Modified = DateTime.Now;
                    entry.Entity.Inactivated = DateTime.Now;
                    entry.Entity.InactivatedBy = _httpContextAccessor.HttpContext.User.Identity.Name ?? "";
                    entry.Entity.StatusId = 0;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name ?? "";
                    entry.Entity.Modified = DateTime.Now;
                    break;
                case EntityState.Added:
                    entry.Entity.CreatedBy = _httpContextAccessor.HttpContext.User.Identity.Name ?? "";
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.StatusId = 1;
                    break;
                default:
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}