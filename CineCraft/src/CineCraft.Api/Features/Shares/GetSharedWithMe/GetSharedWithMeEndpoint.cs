using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.GetSharedWithMe;

public static class GetSharedWithMeEndpoint
{
    public static void MapGetSharedWithMe(this IEndpointRouteBuilder app)
    {
        // GET: /api/shares/shared-with-me/{destinatarioId}
        app.MapGet("/shared-with-me/{destinatarioId:int}", async (int destinatarioId, CineCraftContext dbContext) =>
        {
            var sharedReviews = await dbContext.CompartirReseniaUsuarios
                .Include(c => c.Resenia)
                .Include(c => c.UsuarioRemitente)
                .Where(c => c.UsuarioDestinatarioId == destinatarioId)
                .Select(c => new SharedWithMeDto(
                    c.ReseniaId,
                    c.Resenia!.TituloPelicula,
                    c.Resenia.Comentario,
                    c.UsuarioRemitenteId,
                    c.UsuarioRemitente!.Nombre
                ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(sharedReviews);
        });
    }
}