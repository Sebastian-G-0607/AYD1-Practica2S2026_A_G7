using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Users.GetProfile;

public static class GetProfileEndpoint
{
    public static void MapGetProfile(this IEndpointRouteBuilder app)
    {
        // GET: /api/users/profile or /api/users/profile/{id} or /api/users/{id}
        var handler = async (int? id, HttpContext httpContext, CineCraftContext dbContext) =>
        {
            var targetId = id;
            if (!targetId.HasValue)
            {
                var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (int.TryParse(userIdClaim, out var parsedId))
                {
                    targetId = parsedId;
                }
            }

            if (!targetId.HasValue || targetId.Value <= 0)
            {
                return Results.BadRequest(new { Mensaje = "Identificador de usuario inválido o no especificado." });
            }

            var usuario = await dbContext.Usuarios
                .Include(u => u.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == targetId.Value);

            if (usuario is null)
            {
                return Results.NotFound(new { Mensaje = "Usuario no encontrado." });
            }

            var totalResenias = await dbContext.Resenias
                .CountAsync(r => r.UsuarioAutorId == usuario.Id && !r.Deleted);

            var totalCompartidas = await dbContext.CompartirReseniaUsuarios
                .CountAsync(c => c.UsuarioRemitenteId == usuario.Id);

            var profileDto = new UserProfileDto(
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol != null ? usuario.Rol.Nombre : "estandar",
                totalResenias,
                totalCompartidas
            );

            return Results.Ok(profileDto);
        };

        app.MapGet("/profile", (HttpContext ctx, CineCraftContext db) => handler(null, ctx, db))
           .Produces<UserProfileDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/profile/{id:int}", (int id, HttpContext ctx, CineCraftContext db) => handler(id, ctx, db))
           .Produces<UserProfileDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/{id:int}", (int id, HttpContext ctx, CineCraftContext db) => handler(id, ctx, db))
           .Produces<UserProfileDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }
}
