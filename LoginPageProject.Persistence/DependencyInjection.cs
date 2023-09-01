using LoginPageProject.Application.Common.Interfaces;
using LoginPageProject.Domain.Entities;
using LoginPageProject.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoginPageProject.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<LoginPageProjectDbContext>(options => options.UseSqlServer(ConnectionStringDbContext.GetConnectionString()));
        services.AddIdentity<ApplicationUser, IdentityRole>().
            AddEntityFrameworkStores<LoginPageProjectDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");
        
        services.AddScoped<ILoginPageProjectDbContext, LoginPageProjectDbContext>();
        services.AddScoped<IOldPasswordRepository, OldPasswordRepository>();
        
        
        return services;
    }
}