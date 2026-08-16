using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.UnshareReview;

public static class UnshareReviewEndpoint
{
    public static void MapUnshareReview(this IEndpointRouteBuilder app)
    {
        // DELETE: /api/shares/{reseniaId}/{destinatarioId}
        app.MapDelete("/{reseniaId:int}/{destinatarioId:int}", async (
            int reseniaId,
            int destinatarioId,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var senderId))
            {
                return Results.Unauthorized();
            }

            var sharedRecord = await dbContext.CompartirReseniaUsuarios
                .FirstOrDefaultAsync(c =>
                    c.ReseniaId == reseniaId &&
                    c.UsuarioRemitenteId == senderId &&
                    c.UsuarioDestinatarioId == destinatarioId);

            if (sharedRecord is null)
            {
                return Results.NotFound(new { Mensaje = "No se encontró la reseña compartida con dicho usuario." });
            }

            dbContext.CompartirReseniaUsuarios.Remove(sharedRecord);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Mensaje = "Se ha cancelado el uso compartido de la reseña exitosamente." });
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}
