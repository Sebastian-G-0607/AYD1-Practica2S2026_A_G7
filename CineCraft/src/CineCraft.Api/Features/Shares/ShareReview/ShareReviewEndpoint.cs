using CineCraft.Api.Data;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.ShareReview;

public static class ShareReviewEndpoint
{
    public static void MapShareReview(this IEndpointRouteBuilder app)
    {
        // Endpoint POST: /api/shares
        app.MapPost("/", async (ShareReviewRequest request, CineCraftContext dbContext) =>
        {
            // Validar que no se haya compartido previamente para evitar duplicados en la llave primaria
            var yaCompartido = await dbContext.CompartirReseniaUsuarios
                .AnyAsync(c => c.ReseniaId == request.ReseniaId && 
                               c.UsuarioRemitenteId == request.UsuarioRemitenteId && 
                               c.UsuarioDestinatarioId == request.UsuarioDestinatarioId);

            if (yaCompartido)
            {
                return Results.BadRequest("Esta reseña ya fue compartida con este usuario.");
            }

            var nuevaCompartida = new CompartirReseniaUsuario
            {
                ReseniaId = request.ReseniaId,
                UsuarioRemitenteId = request.UsuarioRemitenteId,
                UsuarioDestinatarioId = request.UsuarioDestinatarioId
            };

            dbContext.CompartirReseniaUsuarios.Add(nuevaCompartida);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Mensaje = "Reseña compartida exitosamente." });
        });
    }
}