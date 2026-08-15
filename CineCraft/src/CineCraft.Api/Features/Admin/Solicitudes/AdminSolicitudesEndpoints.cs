using CineCraft.Api.Features.Admin.Solicitudes.GetPendientes;
using CineCraft.Api.Features.Admin.Solicitudes.Procesar;
using CineCraft.Api.Features.Admin.Solicitudes.GetHistorial;

namespace CineCraft.Api.Features.Admin.Solicitudes;

public static class AdminSolicitudesEndpoints
{
    public static void MapAdminSolicitudes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
                       .RequireAuthorization("AdminOnly");

        group.MapGetPendientes();
        group.MapProcesarSolicitud();
        group.MapGetHistorial();
    }
}