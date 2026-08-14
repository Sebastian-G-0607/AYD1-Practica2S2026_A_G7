using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.GetMyShares;

public static class GetMySharesEndpoint
{
    public static void MapGetMyShares(this IEndpointRouteBuilder app)
    {
        // GET: /api/shares/my-shares/{remitenteId}
        app.MapGet("/my-shares/{remitenteId:int}", async (int remitenteId, CineCraftContext dbContext) =>
        {
            var myShares = await dbContext.CompartirReseniaUsuarios
                .Include(c => c.Resenia)
                .Include(c => c.UsuarioDestinatario)
                .Where(c => c.UsuarioRemitenteId == remitenteId)
                .Select(c => new MySharedDto(
                    c.ReseniaId,
                    c.Resenia!.TituloPelicula,
                    c.Resenia.Comentario,
                    c.UsuarioDestinatarioId,
                    c.UsuarioDestinatario!.Nombre
                ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(myShares);
        });
    }
}