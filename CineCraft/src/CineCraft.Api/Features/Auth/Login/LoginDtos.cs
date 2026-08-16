using System.ComponentModel.DataAnnotations;

namespace CineCraft.Api.Features.Auth.Login;

public record LoginRequestDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

public record LoginResponseDto(
    string Token,
    int Id,
    string Email,
    string Nombre,
    string Role,
    DateTime ExpiresAt
);
