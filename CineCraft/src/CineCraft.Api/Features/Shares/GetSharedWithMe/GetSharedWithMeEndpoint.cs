using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.GetSharedWithMe;

public static class GetSharedWithMeEndpoint
{
    public static void MapGetSharedWithMe(this IEndpointRouteBuilder app)
    {
        // GET: /api/shares/shared-with-me and /api/shares/shared-with-me/{destinatarioId}
        var handler = async (int? destinatarioId, HttpContext httpContext, CineCraftContext dbContext) =>
        {
            var targetId = destinatarioId;
            if (!targetId.HasValue)
            {
                var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (int.TryParse(userIdClaim, out var parsedId))
                {
                    targetId = parsedId;
                }
            }

            if (!targetId.HasValue)
            {
                return Results.BadRequest(new { Mensaje = "Identificador de destinatario no especificado." });
            }

            var sharedReviews = await dbContext.CompartirReseniaUsuarios
                .Include(c => c.Resenia)
                    .ThenInclude(r => r!.Etiqueta)
                .Include(c => c.Resenia)
                    .ThenInclude(r => r!.UsuarioAutor)
                .Include(c => c.UsuarioRemitente)
                .Where(c => c.UsuarioDestinatarioId == targetId.Value && c.Resenia != null && !c.Resenia.Deleted)
                .Select(c => new SharedWithMeDto(
                    c.ReseniaId,
                    c.Resenia!.TituloPelicula,
                    c.Resenia.Calificacion,
                    c.Resenia.Comentario,
                    c.Resenia.Etiqueta != null ? c.Resenia.Etiqueta.Descripcion : string.Empty,
                    c.UsuarioRemitenteId,
                    c.UsuarioRemitente != null ? c.UsuarioRemitente.Nombre : "Usuario CineCraft",
                    c.UsuarioRemitente != null ? c.UsuarioRemitente.Correo : string.Empty,
                    c.Resenia.UsuarioAutorId,
                    c.Resenia.UsuarioAutor != null ? c.Resenia.UsuarioAutor.Nombre : string.Empty
                ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(sharedReviews);
        };

        app.MapGet("/shared-with-me", (HttpContext ctx, CineCraftContext db) => handler(null, ctx, db))
           .Produces<List<SharedWithMeDto>>(StatusCodes.Status200OK);

        app.MapGet("/shared-with-me/{destinatarioId:int}", (int destinatarioId, HttpContext ctx, CineCraftContext db) => handler(destinatarioId, ctx, db))
           .Produces<List<SharedWithMeDto>>(StatusCodes.Status200OK);
    }
}