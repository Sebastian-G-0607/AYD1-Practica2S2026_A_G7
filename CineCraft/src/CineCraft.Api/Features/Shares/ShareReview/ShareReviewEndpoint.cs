using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Shares.ShareReview;

public static class ShareReviewEndpoint
{
    public static void MapShareReview(this IEndpointRouteBuilder app)
    {
        // POST: /api/shares (single share)
        app.MapPost("/", async (
            ShareReviewRequest request,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var senderId = request.UsuarioRemitenteId;
            if (!senderId.HasValue || senderId.Value <= 0)
            {
                var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (int.TryParse(userIdClaim, out var parsedId))
                {
                    senderId = parsedId;
                }
            }

            if (!senderId.HasValue || senderId.Value <= 0)
            {
                return Results.BadRequest(new { Mensaje = "Identificador del usuario remitente inválido." });
            }

            if (senderId.Value == request.UsuarioDestinatarioId)
            {
                return Results.BadRequest(new { Mensaje = "No puedes compartir una reseña contigo mismo." });
            }

            var resenia = await dbContext.Resenias
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.ReseniaId && !r.Deleted);

            if (resenia is null)
            {
                return Results.NotFound(new { Mensaje = "La reseña seleccionada no existe o fue eliminada." });
            }

            var destinatarioExiste = await dbContext.Usuarios
                .AnyAsync(u => u.Id == request.UsuarioDestinatarioId);

            if (!destinatarioExiste)
            {
                return Results.NotFound(new { Mensaje = "El usuario destinatario no existe." });
            }

            var yaCompartido = await dbContext.CompartirReseniaUsuarios
                .AnyAsync(c => c.ReseniaId == request.ReseniaId && 
                               c.UsuarioRemitenteId == senderId.Value && 
                               c.UsuarioDestinatarioId == request.UsuarioDestinatarioId);

            if (yaCompartido)
            {
                return Results.BadRequest(new { Mensaje = "Esta reseña ya fue compartida previamente con este usuario." });
            }

            var nuevaCompartida = new CompartirReseniaUsuario
            {
                ReseniaId = request.ReseniaId,
                UsuarioRemitenteId = senderId.Value,
                UsuarioDestinatarioId = request.UsuarioDestinatarioId
            };

            dbContext.CompartirReseniaUsuarios.Add(nuevaCompartida);
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Mensaje = "Reseña compartida exitosamente." });
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        // POST: /api/shares/batch (batch share)
        app.MapPost("/batch", async (
            ShareReviewBatchRequest request,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var senderId = request.UsuarioRemitenteId;
            if (!senderId.HasValue || senderId.Value <= 0)
            {
                var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (int.TryParse(userIdClaim, out var parsedId))
                {
                    senderId = parsedId;
                }
            }

            if (!senderId.HasValue || senderId.Value <= 0)
            {
                return Results.BadRequest(new { Mensaje = "Identificador del usuario remitente inválido." });
            }

            var resenia = await dbContext.Resenias
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.ReseniaId && !r.Deleted);

            if (resenia is null)
            {
                return Results.NotFound(new { Mensaje = "La reseña seleccionada no existe o fue eliminada." });
            }

            var recipients = request.DestinatariosIds
                .Where(id => id != senderId.Value)
                .Distinct()
                .ToList();

            if (recipients.Count == 0)
            {
                return Results.BadRequest(new { Mensaje = "No se seleccionaron destinatarios válidos." });
            }

            var countCompartidos = 0;
            foreach (var destId in recipients)
            {
                var destinatarioExiste = await dbContext.Usuarios.AnyAsync(u => u.Id == destId);
                if (!destinatarioExiste) continue;

                var yaCompartido = await dbContext.CompartirReseniaUsuarios
                    .AnyAsync(c => c.ReseniaId == request.ReseniaId && 
                                   c.UsuarioRemitenteId == senderId.Value && 
                                   c.UsuarioDestinatarioId == destId);

                if (!yaCompartido)
                {
                    dbContext.CompartirReseniaUsuarios.Add(new CompartirReseniaUsuario
                    {
                        ReseniaId = request.ReseniaId,
                        UsuarioRemitenteId = senderId.Value,
                        UsuarioDestinatarioId = destId
                    });
                    countCompartidos++;
                }
            }

            await dbContext.SaveChangesAsync();

            return Results.Ok(new { 
                Mensaje = countCompartidos > 0 
                    ? $"Reseña compartida exitosamente con {countCompartidos} usuario(s)." 
                    : "La reseña ya había sido compartida con todos los usuarios seleccionados.",
                Compartidos = countCompartidos
            });
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
}