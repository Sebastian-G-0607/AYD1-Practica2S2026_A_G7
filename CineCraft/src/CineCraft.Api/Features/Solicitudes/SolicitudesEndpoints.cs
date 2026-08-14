using CineCraft.Api.Features.Solicitudes.CrearSolicitud;

namespace CineCraft.Api.Features.Solicitudes;

public static class SolicitudesEndpoints
{
    public static void MapSolicitudes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/solicitudes");
        group.MapCrearSolicitud();
    }
}
