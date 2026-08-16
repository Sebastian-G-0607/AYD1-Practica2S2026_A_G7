using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.GetMyShares;

public static class GetMySharesEndpoint
{
    public static void MapGetMyShares(this IEndpointRouteBuilder app)
    {
        // GET: /api/shares/my-shares and /api/shares/my-shares/{remitenteId}
        var handler = async (int? remitenteId, HttpContext httpContext, CineCraftContext dbContext) =>
        {
            var targetId = remitenteId;
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
                return Results.BadRequest(new { Mensaje = "Identificador de remitente no especificado." });
            }

            var myShares = await dbContext.CompartirReseniaUsuarios
                .Include(c => c.Resenia)
                    .ThenInclude(r => r!.Etiqueta)
                .Include(c => c.UsuarioDestinatario)
                .Where(c => c.UsuarioRemitenteId == targetId.Value && c.Resenia != null && !c.Resenia.Deleted)
                .Select(c => new MySharedDto(
                    c.ReseniaId,
                    c.Resenia!.TituloPelicula,
                    c.Resenia.Calificacion,
                    c.Resenia.Comentario,
                    c.Resenia.Etiqueta != null ? c.Resenia.Etiqueta.Descripcion : string.Empty,
                    c.UsuarioDestinatarioId,
                    c.UsuarioDestinatario != null ? c.UsuarioDestinatario.Nombre : "Usuario CineCraft",
                    c.UsuarioDestinatario != null ? c.UsuarioDestinatario.Correo : string.Empty
                ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(myShares);
        };

        app.MapGet("/my-shares", (HttpContext ctx, CineCraftContext db) => handler(null, ctx, db))
           .Produces<List<MySharedDto>>(StatusCodes.Status200OK);

        app.MapGet("/my-shares/{remitenteId:int}", (int remitenteId, HttpContext ctx, CineCraftContext db) => handler(remitenteId, ctx, db))
           .Produces<List<MySharedDto>>(StatusCodes.Status200OK);
    }
}