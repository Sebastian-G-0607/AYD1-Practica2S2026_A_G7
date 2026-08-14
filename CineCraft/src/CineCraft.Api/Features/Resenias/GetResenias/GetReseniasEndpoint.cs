using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Resenias.GetResenias;

public static class GetReseniasEndpoint
{
    public static void MapGetResenias(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (CineCraftContext dbContext) =>
        {
            var resenias = await dbContext.Resenias
                .AsNoTracking()
                .Where(r => !r.Deleted && !r.Archivada)
                .Select(r => new ReseniaDto(
                    r.Id,
                    r.TituloPelicula,
                    r.Calificacion,
                    r.Comentario,
                    r.EtiquetaId,
                    r.Etiqueta!.Descripcion,
                    r.Destacada,
                    r.Archivada
                ))
                .ToListAsync();

            return Results.Ok(resenias);
        });
    }
}