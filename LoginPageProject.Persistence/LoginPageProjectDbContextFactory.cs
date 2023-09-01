using Microsoft.EntityFrameworkCore;

namespace LoginPageProject.Persistence;

public class LoginPageProjectDbContextFactory : DesignTimeDbContextFactoryBase<LoginPageProjectDbContext>
{
    protected override LoginPageProjectDbContext CreateNewInstance(DbContextOptions<LoginPageProjectDbContext> options)
    {
        return new LoginPageProjectDbContext(options);
    }
}