
using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.ToggleDestacar;

public static class ToggleDestacarEndpoint
{
    public static void MapToggleDestacar(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/{id:int}/destacar", async (
            int id,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User
                .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var resenia = await dbContext.Resenias
                .FirstOrDefaultAsync(r =>
                    r.Id == id &&
                    r.UsuarioAutorId == userId &&
                    !r.Deleted);

            if (resenia is null)
            {
                return Results.NotFound(new
                {
                    message = "La reseña no existe o no pertenece al usuario."
                });
            }

            resenia.Destacada = !resenia.Destacada;
            await dbContext.SaveChangesAsync();

            var estado = resenia.Destacada ? "destacada" : "desmarcada como destacada";
            return Results.Ok(new ToggleDestacarResponse(
                resenia.Id,
                resenia.Destacada,
                $"La reseña ha sido {estado} exitosamente."
            ));
        });
    }
}