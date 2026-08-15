using CineCraft.Api.Data;
using CineCraft.Api.Features.Admin.Solicitudes;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Admin.Solicitudes.GetPendientes;

public static class GetPendientesEndpoint
{
    public static void MapGetPendientes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/solicitudes/pendientes", async (CineCraftContext dbContext) =>
        {
            var statusPendiente = await dbContext.SolicitudStatuses
                .FirstOrDefaultAsync(s => s.Descripcion.ToLower() == "pendiente");

            if (statusPendiente is null)
            {
                return Results.Problem("El estado 'Pendiente' no existe en el sistema.");
            }

            var solicitudes = await dbContext.Solicitudes
                .Where(s => s.StatusId == statusPendiente.Id)
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new AdminSolicitudDto(
                    s.Id,
                    s.Nombre,
                    s.Correo,
                    s.FechaSolicitud,
                    statusPendiente.Descripcion,
                    s.MotivoRechazo
                ))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(solicitudes);
        })
        .RequireAuthorization("AdminOnly")
        .Produces<List<AdminSolicitudDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
