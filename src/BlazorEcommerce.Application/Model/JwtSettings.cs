using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Application.Model;

public class JwtSettings
{
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; } = string.Empty;
    [Required]
    public double DurationInMinutes { get; set; }
}
