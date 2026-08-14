using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Reportes.GetTopUsuariosCompartidos;

public static class GetTopUsuariosCompartidosEndpoint
{
    public static void MapGetTopUsuariosCompartidos(this IEndpointRouteBuilder app)
    {
        app.MapGet("/top-compartidos", async (CineCraftContext dbContext) =>
        {
            var topUsuarios = await dbContext.CompartirReseniaUsuarios
                .GroupBy(c => c.UsuarioRemitenteId)
                .Select(g => new { UsuarioId = g.Key, CantidadCompartidos = g.Count() })
                .OrderByDescending(x => x.CantidadCompartidos)
                .Take(5)
                .Join(dbContext.Usuarios,
                    c => c.UsuarioId,
                    u => u.Id,
                    (c, u) => new TopUsuarioCompartidoDto(u.Id, u.Nombre, c.CantidadCompartidos))
                .ToListAsync();

            return Results.Ok(topUsuarios);
        })
        .Produces<List<TopUsuarioCompartidoDto>>();
    }
}