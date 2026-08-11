using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CineCraft.Api.Shared.Authentication;

/// <summary>
/// Configures JWT Bearer options using AuthOptions with symmetric key validation.
/// </summary>
public class JwtBearerOptionsSetup(IOptions<AuthOptions> authOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name == JwtBearerDefaults.AuthenticationScheme || string.IsNullOrEmpty(name))
        {
            Configure(options);
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        var opts = authOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opts.SecretKey));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = opts.Issuer,
            ValidateAudience = true,
            ValidAudience = opts.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "role"
        };

        options.MapInboundClaims = false;
    }
}
