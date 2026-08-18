using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace CineCraft.Api.Features.Resenias.UpdateResenia;

public static class UpdateReseniaEndpoint
{
    public static void MapUpdateResenia(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:int}", async (
            int id,
            UpdateReseniaRequest request,
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

            if (string.IsNullOrWhiteSpace(request.TituloPelicula))
            {
                return Results.BadRequest(new
                {
                    message = "El título de la película es obligatorio."
                });
            }

            if (request.Calificacion < 1 || request.Calificacion > 5)
            {
                return Results.BadRequest(new
                {
                    message = "La calificación debe estar entre 1 y 5 estrellas."
                });
            }

            var tieneEtiquetaExistente = request.EtiquetaId.HasValue;
            var tieneNuevaEtiqueta = !string.IsNullOrWhiteSpace(request.NuevaEtiqueta);

            if (tieneEtiquetaExistente == tieneNuevaEtiqueta)
            {
                return Results.BadRequest(new
                {
                    message = "Debe seleccionar una etiqueta existente o crear una nueva."
                });
            }

            Etiqueta? etiqueta;

            if (tieneEtiquetaExistente)
            {
                etiqueta = await dbContext.Etiquetas
                    .FirstOrDefaultAsync(e => e.Id == request.EtiquetaId!.Value);

                if (etiqueta is null)
                {
                    return Results.BadRequest(new
                    {
                        message = "La etiqueta seleccionada no existe."
                    });
                }

                resenia.EtiquetaId = etiqueta.Id;
                resenia.Etiqueta = etiqueta;
            }
            else
            {
                var descripcionNormalizada = request.NuevaEtiqueta!.Trim();

                var etiquetaExistente = await dbContext.Etiquetas
                    .AnyAsync(e =>
                        e.Descripcion.ToLower() == descripcionNormalizada.ToLower());

                if (etiquetaExistente)
                {
                    return Results.BadRequest(new
                    {
                        message = "Ya existe una etiqueta con ese nombre."
                    });
                }

                etiqueta = new Etiqueta
                {
                    UsuarioId = userId,
                    Descripcion = descripcionNormalizada
                };

                dbContext.Etiquetas.Add(etiqueta);
                resenia.Etiqueta = etiqueta;
            }

            resenia.TituloPelicula = request.TituloPelicula.Trim();
            resenia.Calificacion = request.Calificacion;
            resenia.Comentario = request.Comentario?.Trim();

            await dbContext.SaveChangesAsync();

            return Results.Ok(
                new UpdateReseniaResponse(
                    resenia.Id,
                    resenia.TituloPelicula,
                    resenia.Calificacion,
                    resenia.Comentario,
                    etiqueta.Id,
                    etiqueta.Descripcion
                ));
        });
    }
}