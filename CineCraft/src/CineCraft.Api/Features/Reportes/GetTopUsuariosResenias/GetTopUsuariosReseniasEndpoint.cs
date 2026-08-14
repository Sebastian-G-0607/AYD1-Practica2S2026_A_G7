using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Reportes.GetTopUsuariosResenias;

public static class GetTopUsuariosReseniasEndpoint
{
    public static void MapGetTopUsuariosResenias(this IEndpointRouteBuilder app)
    {
        app.MapGet("/top-resenias", async (CineCraftContext dbContext) =>
        {
            var topUsuarios = await dbContext.Resenias
                .Where(r => !r.Deleted)
                .GroupBy(r => r.UsuarioAutorId)
                .Select(g => new { UsuarioId = g.Key, CantidadResenias = g.Count() })
                .OrderByDescending(x => x.CantidadResenias)
                .Take(5)
                .Join(dbContext.Usuarios,
                    r => r.UsuarioId,
                    u => u.Id,
                    (r, u) => new TopUsuarioReseniaDto(u.Id, u.Nombre, r.CantidadResenias))
                .ToListAsync();

            return Results.Ok(topUsuarios);
        })
        .Produces<List<TopUsuarioReseniaDto>>();
    }
}