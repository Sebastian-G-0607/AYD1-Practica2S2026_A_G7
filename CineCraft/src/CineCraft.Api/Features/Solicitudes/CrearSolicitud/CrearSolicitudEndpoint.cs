using CineCraft.Api.Data;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Solicitudes.CrearSolicitud;

public static class CrearSolicitudEndpoint
{
    public static void MapCrearSolicitud(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
            CrearSolicitudRequestDto request,
            CineCraftContext dbContext,
            ILogger<Program> logger) =>
        {
            if (request.Contrasenia != request.ConfirmacionContrasenia)
            {
                return Results.BadRequest(new { message = "La confirmación de la contraseña no coincide con la contraseña." });
            }

            var correoNormalizado = request.Correo.Trim().ToLower();

            var usuarioExistente = await dbContext.Usuarios
                .AnyAsync(u => u.Correo.ToLower() == correoNormalizado);

            if (usuarioExistente)
            {
                return Results.Conflict(new { message = "Ya existe un usuario registrado con este correo electrónico." });
            }

            var statusPendiente = await dbContext.SolicitudStatuses
                .FirstOrDefaultAsync(s => s.Descripcion.ToLower() == "pendiente");

            if (statusPendiente is null)
            {
                logger.LogError("El estado 'Pendiente' de solicitud no fue encontrado en la base de datos.");
                return Results.Problem("El estado predeterminado para la solicitud no existe en el sistema.");
            }

            var solicitudExistente = await dbContext.Solicitudes
                .AnyAsync(s => s.Correo.ToLower() == correoNormalizado && s.StatusId == statusPendiente.Id);

            if (solicitudExistente)
            {
                return Results.Conflict(new { message = "Ya existe una solicitud pendiente para este correo electrónico." });
            }

            var contraseniaHasheada = BCrypt.Net.BCrypt.HashPassword(request.Contrasenia);

            var nuevaSolicitud = new Solicitud
            {
                Nombre = request.Nombre.Trim(),
                Correo = request.Correo.Trim(),
                Contrasenia = contraseniaHasheada,
                StatusId = statusPendiente.Id,
                FechaSolicitud = DateTime.UtcNow
            };

            dbContext.Solicitudes.Add(nuevaSolicitud);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Solicitud de registro creada exitosamente con Id {Id} para correo {Correo}", nuevaSolicitud.Id, nuevaSolicitud.Correo);

            var response = new SolicitudResponseDto(
                nuevaSolicitud.Id,
                nuevaSolicitud.Nombre,
                nuevaSolicitud.Correo,
                nuevaSolicitud.StatusId,
                statusPendiente.Descripcion,
                nuevaSolicitud.FechaSolicitud
            );

            return Results.Created($"/api/solicitudes/{nuevaSolicitud.Id}", response);
        })
        .AllowAnonymous()
        .Produces<SolicitudResponseDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);
    }
}
