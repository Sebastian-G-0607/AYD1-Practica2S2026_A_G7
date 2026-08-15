using CineCraft.Api.Data;
using CineCraft.Api.Features.Admin.Solicitudes;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Admin.Solicitudes.GetHistorial;

public static class GetHistorialEndpoint
{
    public static void MapGetHistorial(this IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/solicitudes/historial", async (CineCraftContext dbContext) =>
        {
            var solicitudes = await dbContext.Solicitudes
                .Include(s => s.Status)
                .Where(s => s.Status != null && s.Status.Descripcion.ToLower() != "pendiente")
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new AdminSolicitudDto(
                    s.Id,
                    s.Nombre,
                    s.Correo,
                    s.FechaSolicitud,
                    s.Status != null ? s.Status.Descripcion : "Desconocido",
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
