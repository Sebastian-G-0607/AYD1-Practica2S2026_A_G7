using System.ComponentModel.DataAnnotations;

namespace CineCraft.Api.Shared.Authentication;

/// <summary>
/// Configuration options for default seed users settings.
/// </summary>
public class UsersOptions
{
    /// <summary>
    /// The configuration section name for users options.
    /// </summary>
    public const string SectionName = "Users";

    /// <summary>
    /// Gets or sets the default admin email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string DefaultAdminEmail { get; set; } = "admin@cinecraft.com";

    /// <summary>
    /// Gets or sets the default admin password.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string DefaultAdminPassword { get; set; } = "Admin123!";
}
