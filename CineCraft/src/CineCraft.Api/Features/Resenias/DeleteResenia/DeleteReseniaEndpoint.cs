using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.DeleteResenia;

public static class DeleteReseniaEndpoint
{
    public static void MapDeleteResenia(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:int}", async (
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

            resenia.Deleted = true;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}