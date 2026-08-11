using System.ComponentModel.DataAnnotations;

namespace CineCraft.Api.Shared.Authentication;

/// <summary>
/// Configuration options for JWT authentication settings.
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// The configuration section name for authentication options.
    /// </summary>
    public const string SectionName = "Auth";

    /// <summary>
    /// Gets or sets the secret key used to sign JWT tokens.
    /// Must be at least 32 characters long.
    /// </summary>
    [Required]
    [MinLength(32)]
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JWT issuer.
    /// </summary>
    [Required]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JWT audience.
    /// </summary>
    [Required]
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time in minutes.
    /// </summary>
    [Range(1, 1440)]
    public int ExpirationMinutes { get; set; } = 60;
}
