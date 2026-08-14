using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.GetReseniasArchivadas;

public static class GetReseniasArchivadasEndpoint
{
    public static void MapGetReseniasArchivadas(this IEndpointRouteBuilder app)
    {
        app.MapGet("/archivadas", async (
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User
                .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var reseniasArchivadas = await dbContext.Resenias
                .AsNoTracking()
                .Where(r => r.UsuarioAutorId == userId && !r.Deleted && r.Archivada)
                .Select(r => new ReseniaArchivadaDto(
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

            return Results.Ok(reseniasArchivadas);
        });
    }
}