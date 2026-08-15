using CineCraft.Api.Data;
using CineCraft.Api.Shared.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CineCraft.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapLogin(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (
            LoginRequestDto request,
            CineCraftContext dbContext,
            JwtTokenService tokenService,
            IOptions<AuthOptions> authOptions,
            ILogger<Program> logger) =>
        {
            var usuario = await dbContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == request.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.Contrasenia))
            {
                logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
                return Results.Unauthorized();
            }

            var rolNombre = usuario.Rol?.Nombre ?? "estandar";
            var token = tokenService.GenerateToken(usuario, rolNombre);
            var expiration = DateTime.UtcNow.AddMinutes(authOptions.Value.ExpirationMinutes);

            logger.LogInformation("User {Email} logged in successfully", usuario.Correo);

            return Results.Ok(new LoginResponseDto(
                Token: token,
                Id: usuario.Id,
                Email: usuario.Correo,
                Nombre: usuario.Nombre,
                Role: rolNombre,
                ExpiresAt: expiration
            ));
        })
        .AllowAnonymous()
        .Produces<LoginResponseDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
