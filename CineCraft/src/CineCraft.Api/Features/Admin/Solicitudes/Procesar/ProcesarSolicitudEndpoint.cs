using CineCraft.Api.Data;
using CineCraft.Api.Features.Admin.Solicitudes;
using CineCraft.Api.Models;
using CineCraft.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Admin.Solicitudes.Procesar;

public static class ProcesarSolicitudEndpoint
{
    public static void MapProcesarSolicitud(this IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/solicitudes/procesar", async (
            ProcesarSolicitudRequestDto request,
            CineCraftContext dbContext,
            IEmailService emailService,
            ILogger<Program> logger) =>
        {
            var solicitud = await dbContext.Solicitudes
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.Id == request.SolicitudId);

            if (solicitud is null)
            {
                return Results.NotFound(new { message = "Solicitud no encontrada." });
            }

            if (solicitud.Status?.Descripcion != "pendiente")
            {
                return Results.BadRequest(new { message = "La solicitud ya fue procesada." });
            }

            var statusAprobado = await dbContext.SolicitudStatuses
                .FirstOrDefaultAsync(s => s.Descripcion== "Aprobado");

            var statusRechazado = await dbContext.SolicitudStatuses
                .FirstOrDefaultAsync(s => s.Descripcion == "Rechazado");

            if (statusAprobado is null || statusRechazado is null)
            {
                logger.LogError("Los estados 'Aprobada' o 'Rechazada' no existen en la base de datos.");
                return Results.Problem("Los estados necesarios no existen en el sistema.");
            }

            if (request.Aprobar)
            {
                var correoNormalizado = solicitud.Correo.Trim().ToLower();
                var usuarioExistente = await dbContext.Usuarios
                    .AnyAsync(u => u.Correo.ToLower() == correoNormalizado);

                if (usuarioExistente)
                {
                    return Results.Conflict(new { message = "Ya existe un usuario con este correo." });
                }

                var rolUsuario = await dbContext.Roles
                    .FirstOrDefaultAsync(r => r.Nombre == "estandar");

                if (rolUsuario is null)
                {
                    logger.LogError("El rol 'Usuario' no existe en la base de datos.");
                    return Results.Problem("El rol de usuario no existe en el sistema.");
                }

                var nuevoUsuario = new Usuario
                {
                    Nombre = solicitud.Nombre,
                    Correo = solicitud.Correo,
                    Contrasenia = solicitud.Contrasenia,
                    RolId = rolUsuario.Id,
                    SolicitudId = solicitud.Id
                };

                dbContext.Usuarios.Add(nuevoUsuario);

                solicitud.StatusId = statusAprobado.Id;

                await dbContext.SaveChangesAsync();

                await emailService.SendApprovalEmail(solicitud.Correo, solicitud.Nombre);

                logger.LogInformation("Solicitud {SolicitudId} aprobada. Usuario {UsuarioId} creado.", 
                    solicitud.Id, nuevoUsuario.Id);

                return Results.Ok(new { 
                    message = "Solicitud aprobada y usuario creado exitosamente.",
                    usuario_id = nuevoUsuario.Id
                });
            }
            else
            {

                if (string.IsNullOrWhiteSpace(request.MotivoRechazo))
                {
                    return Results.BadRequest(new { message = "Debe proporcionar un motivo para el rechazo." });
                }

                solicitud.StatusId = statusRechazado.Id;
                solicitud.MotivoRechazo = request.MotivoRechazo.Trim();

                await dbContext.SaveChangesAsync();

                await emailService.SendRejectionEmail(solicitud.Correo, solicitud.Nombre, request.MotivoRechazo);

                logger.LogInformation("Solicitud {SolicitudId} rechazada. Motivo: {Motivo}", 
                    solicitud.Id, request.MotivoRechazo);

                return Results.Ok(new { 
                    message = "Solicitud rechazada exitosamente."
                });
            }
        })
        .RequireAuthorization("AdminOnly")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
