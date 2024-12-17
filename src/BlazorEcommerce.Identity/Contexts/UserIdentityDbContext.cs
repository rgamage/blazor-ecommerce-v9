using BlazorEcommerce.Shared.Constant;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace BlazorEcommerce.Identity.Contexts;

public class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Change the schema for Identity tables
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetSchema(Constants.IdentityDbSchema);
        }

        builder.ApplyConfigurationsFromAssembly(typeof(UserIdentityDbContext).Assembly);
    }
}
