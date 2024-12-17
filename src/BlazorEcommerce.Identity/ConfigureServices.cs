using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Application.Model;
using BlazorEcommerce.Identity.Contexts;
using BlazorEcommerce.Identity.Service;
using BlazorEcommerce.Persistence.Contexts;
using BlazorEcommerce.Shared.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorEcommerce.Identity;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("JwtSettings"))
            .ValidateDataAnnotations(); // Optional, validates attributes like [Required]

        var connectionString = configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<UserIdentityDbContext>(options =>
            options.UseSqlServer(connectionString, opts => opts.MigrationsHistoryTable(tableName: "__EFMigrationsHistory", schema: Constants.IdentityDbSchema)));

        services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;

            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<UserIdentityDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? string.Empty))

            };
        });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<UserIdentityDbContextInitialiser>();

        return services;
    }
}
