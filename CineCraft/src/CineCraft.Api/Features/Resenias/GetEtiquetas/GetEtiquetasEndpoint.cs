using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.GetEtiquetas;

public static class GetEtiquetasEndpoint
{
    public static void MapGetEtiquetas(this IEndpointRouteBuilder app)
    {
        app.MapGet("/etiquetas", async (
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User
                .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var etiquetas = await dbContext.Etiquetas
                .AsNoTracking()
                .OrderBy(e => e.Descripcion)
                .Select(e => new EtiquetaDto(
                    e.Id,
                    e.Descripcion
                ))
                .ToListAsync();

            return Results.Ok(etiquetas);
        });
    }
}
