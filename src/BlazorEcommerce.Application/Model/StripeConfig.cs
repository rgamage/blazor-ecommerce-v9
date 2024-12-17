using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Application.Model;

public class StripeConfig
{
    [Required]
    public string Secret { get; set; } = string.Empty;
    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
