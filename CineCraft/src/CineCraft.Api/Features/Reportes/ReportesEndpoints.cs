using CineCraft.Api.Features.Reportes.GetTopUsuariosResenias;
using CineCraft.Api.Features.Reportes.GetTopUsuariosCompartidos;

namespace CineCraft.Api.Features.Reportes;

public static class ReportesEndpoints
{
    public static void MapReportes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/reportes")
                        .RequireAuthorization("AdminOnly");

        group.MapGetTopUsuariosResenias();
        group.MapGetTopUsuariosCompartidos();
    }
}