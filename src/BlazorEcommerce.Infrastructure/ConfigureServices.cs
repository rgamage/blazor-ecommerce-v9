using BlazorEcommerce.Application.Contracts.Payment;
using BlazorEcommerce.Application.Model;
using BlazorEcommerce.Infrastructure.Services.PaymentService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorEcommerce.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<StripeConfig>()
            .Bind(configuration.GetSection("StripeSettings"))
            .ValidateDataAnnotations(); // Optional, validates attributes like [Required]

        services.AddOptions<AppConfig>()
            .Bind(configuration.GetSection("AppConfig"))
            .ValidateDataAnnotations(); // Optional, validates attributes like [Required]

        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
