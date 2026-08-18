using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.CreateResenia;

public static class CreateReseniaEndpoint
{
    public static void MapCreateResenia(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            CreateReseniaRequest request,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var userIdClaim = httpContext.User
                .FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
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
            }

            var resenia = new Resenia
            {
                UsuarioAutorId = userId,
                TituloPelicula = request.TituloPelicula.Trim(),
                Calificacion = request.Calificacion,
                Comentario = request.Comentario?.Trim(),
                Etiqueta = etiqueta,
                Destacada = false,
                Archivada = false,
                Deleted = false
            };

            dbContext.Resenias.Add(resenia);
            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/api/resenias/{resenia.Id}",
                new CreateReseniaResponse(
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