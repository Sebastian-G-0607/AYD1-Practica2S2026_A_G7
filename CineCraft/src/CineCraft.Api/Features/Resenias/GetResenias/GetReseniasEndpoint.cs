using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.GetResenias;

public static class GetReseniasEndpoint
{
    public static void MapGetResenias(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User
                .FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }
            var resenias = await dbContext.Resenias
                .AsNoTracking()
                .Where(r => r.UsuarioAutorId == userId && !r.Deleted && !r.Archivada)
                .Select(r => new ReseniaDto(
                    r.Id,
                    r.TituloPelicula,
                    r.Calificacion,
                    r.Comentario,
                    r.EtiquetaId,
                    r.Etiqueta!.Descripcion,
                    r.Destacada,
                    r.Archivada
                ))
                .ToListAsync();

            return Results.Ok(resenias);
        });
    }
}