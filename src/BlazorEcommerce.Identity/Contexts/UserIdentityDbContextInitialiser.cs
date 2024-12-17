using BlazorEcommerce.Identity;
using BlazorEcommerce.Identity.Contexts;
using BlazorEcommerce.Shared.Constant;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorEcommerce.Persistence.Contexts;

public class UserIdentityDbContextInitialiser
{
    private readonly UserIdentityDbContext _context;

    public UserIdentityDbContextInitialiser(UserIdentityDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await InitialiseWithMigrationsAsync();
    }

    private async Task InitialiseWithMigrationsAsync()
    {
        if (_context.Database.IsSqlServer())
        {
            await _context.Database.MigrateAsync();
        }
        else
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }

    public async Task SeedData(IServiceScope scope)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = Constants.AdminEmail;
        var existinAdmingUser = await userManager.FindByEmailAsync(adminEmail);
        if (existinAdmingUser == null)
        {
            var adminUser = new ApplicationUser
            {
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                FirstName = "System",
                LastName = "Admin",
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(),
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, Constants.AdminPassword);
            await userManager.AddToRoleAsync(adminUser, Constants.AdminRoleName);
        }
    }
}
